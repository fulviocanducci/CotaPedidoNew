using CoaPedido.WebUI.Models;
using CotaPedido.Dominio.Util;
using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;
using CotaPedido.Infra.Repositorios.SQLServer;
using CotaPedido.WebUI.Models;
using Microsoft.Owin;
using NPOI.HSSF.Record.Chart;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CoaPedido.WebUI.Controllers
{
    public class CadastroController : Controller
    {
        #region BlocoPrincipal
        [Route("cadastra-se", Name = "CadastroSeIndex")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EmailNoRepeat(System.Web.Mvc.FormCollection form)
        {
            string email = form["CadastroReduzido.Email"];
            string TypeCadastro = form["CadastroReduzido.TypeCadastro"];
            return Json(VerifyEmailNoRepeat(email, TypeCadastro) == false);
        }

        protected bool VerifyEmailNoRepeat(string email, string TypeCadastro)
        {
            if (TypeCadastro == "Comprador")
            {
                RepositorioComprador repositorio = new RepositorioComprador();
                return repositorio.GetEmailExists(email);
            }
            if (TypeCadastro == "Vendedor")
            {
                RepositorioVendedor repositorio = new RepositorioVendedor();
                return repositorio.GetEmailExists(email);
            }
            return false;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        [Route("cadastro-lista-cidade", Name = "CadastroListaCidadeByIdUf")]
        public ActionResult ListaCidade(string id)
        {
            return Json(RepositorioCidade.GetListporUF(id), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        [Route("cadastro-lista-cidade-by-regiao", Name = "CadastroListaCidadeByIdRegiao")]
        public ActionResult ListaCidadeByRegiao(string id)
        {
            return Json(RepositorioCidade.GetListporRegiao(id), JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region CadastroSiteComprador
        [HttpGet]
        [Route("cadastro/comprador", Name = "CadastroCompradorGet")]
        public ActionResult Comprador(string link = null)
        {
            if (!string.IsNullOrEmpty(link))
            {
                TempData["link"] = link;
            }
            var success = TempData["success"] as bool?;
            if (success.HasValue && success.Value)
            {
                ViewBag.Success = true;
            }
            //var model = new CompradorModel();
            //model.SelectCidades();
            //model.SelectCidadesCobranca();
            ViewBag.RouteNamePost = "CadastroCompradorPost";
            ViewBag.TypeCadastro = "Comprador";
            return View("Cadastros");
        }

        [HttpPost]
        [Route("cadastro/comprador", Name = "CadastroCompradorPost")]
        public ActionResult Comprador(ViewModelCadastro master)
        {
            CadastroReduzido model = master.CadastroReduzido;
            model.IdCidadeCobranca = model.IdCidade;
            if (ModelState.IsValid)
            {
                if (VerifyEmailNoRepeat(model.Email, model.TypeCadastro) == false)
                {
                    RepositorioComprador repositorio = new RepositorioComprador();
                    repositorio.Insert(
                        model.Email,
                        model.Senha, 
                        model.NomeFantasia,
                        model.RazaoSocial, 
                        model.IdCidade,
                        model.IdCidadeCobranca,
                        model.DataCadastro, 
                        model.Status
                    );
                    TempData["success"] = true;
                    return RedirectToAction("Comprador");
                }
                else
                {
                    ModelState.AddModelError("Email", "Email já existente");
                }
            }
            ViewBag.TypeCadastro = "Comprador";
            ViewBag.RouteNamePost = "CadastroCompradorPost";
            return View("Cadastros", master);
        }
        #endregion

        #region Comments
        //[HttpPost]
        //public ActionResult Comprador(CompradorModel model)
        //{
        //    var repositorio = new RepositorioComprador();

        //    ValidarDadosComprador(model);

        //    if (ModelState.IsValid)
        //    {
        //        var comprador = model.CadastroCompradorModel.GetComprador();
        //        comprador.DataCadastro = DateTime.Now;
        //        comprador.Status = true;

        //        repositorio.Insert(comprador);

        //        TempData.Add("Success", true);

        //        var user = repositorio.GetAll().Where(x => x.Email == comprador.Email && x.Senha == comprador.Senha).ToList();

        //        if (user.Count() == 1)
        //        {
        //            Session.Add("user", user.SingleOrDefault());
        //            return RedirectToAction("Index", "Compras", null);
        //        }
        //    }

        //    //model.SelectCidades();
        //    //model.SelectCidadesCobranca();

        //    return View(model);
        //}
        #endregion

        #region Load
        protected SelectList Load_Cidade_Update(UF uf, int? selectValue = null)
        {
            RepositorioEstado repositorioEstado = new RepositorioEstado();
            Estado estado = repositorioEstado.GetByNome(uf);
            if (estado != null)
            {
                return new SelectList(RepositorioCidade.GetListporUF(estado.Id.ToString()), "Id", "Nome", selectValue);
            }
            return null;
        }

        protected void Load_Cidades_And_CidadesCobranca(UF ufcidade, int idCidade, UF ufCidadeCobranca, int idCidadeCobranca)
        {
            ViewBag.Cidades = Load_Cidade_Update(ufcidade, idCidade);
            ViewBag.CidadesCobranca = Load_Cidade_Update(ufCidadeCobranca, idCidadeCobranca);
        }
        #endregion

        #region LogadoDadosComprador
        [HttpGet]
        [SessionExpire("user")]
        [Route("dados-comprador")]
        public ActionResult DadosComprador()
        {
            var user = Session["user"] as Comprador;
            if (user == null)
            {
                return RedirectToAction("Comprador", "Cadastro");
            }

            var model = new CompradorModel();
            model.CadastroCompradorModel.GetCompradorModel(user);

            //model.SelectCidades();
            //model.SelectCidadesCobranca();
            Load_Cidades_And_CidadesCobranca(
                model.CadastroCompradorModel.UF,
                model.CadastroCompradorModel.IdCidade,
                model.CadastroCompradorModel.UFCobranca,
                model.CadastroCompradorModel.IdCidadeCobranca
            );
            return View(model);
        }

        [HttpPost]
        [SessionExpire("user")]
        [Route("dados-comprador")]
        public ActionResult DadosComprador(CompradorModel model)
        {
            var repositorio = new RepositorioComprador();

            ValidarDadosComprador(model);

            if (ModelState.IsValid)
            {
                var user = Session["user"] as Comprador;
                var comprador = model.CadastroCompradorModel.GetComprador();
                comprador.Status = true;
                comprador.DataCadastro = user.DataCadastro;

                try
                {
                    repositorio.Update(comprador);
                    Session.Add("user", comprador);
                    TempData.Add("Success", true);
                    return RedirectToAction("DadosComprador");
                }
                catch
                {
                    ModelState.AddModelError("", "Não foi possível efetuar a alteração de seus dados.");
                }
            }

            //model.SelectCidades();
            //model.SelectCidadesCobranca();
            Load_Cidades_And_CidadesCobranca(
                model.CadastroCompradorModel.UF,
                model.CadastroCompradorModel.IdCidade,
                model.CadastroCompradorModel.UFCobranca,
                model.CadastroCompradorModel.IdCidadeCobranca
            );

            return View(model);
        }

        #endregion

        #region CadastroSiteVendedor

        [HttpGet]
        [Route("cadastro/vendedor", Name = "CadastroVendedorGet")]
        public ActionResult Vendedor(string link = null)
        {
            if (!string.IsNullOrEmpty(link))
            {
                TempData["link"] = link;
            }
            var success = TempData["success"] as bool?;
            if (success.HasValue && success.Value)
            {
                ViewBag.Success = true;
            }

            //var model = new VendedorModel();
            ////model.SelectCidades();
            ////model.SelectCidadesCobranca();
            //Load_Cidades_And_CidadesCobranca(
            //    model.CadastroVendedorModel.UF,
            //    model.CadastroVendedorModel.IdCidade,
            //    model.CadastroVendedorModel.UFCobranca,
            //    model.CadastroVendedorModel.IdCidadeCobranca
            //);
            ViewBag.TypeCadastro = "Vendedor";
            ViewBag.RouteNamePost = "CadastroVendedorPost";
            return View("Cadastros");
        }

        [HttpPost]
        [Route("cadastro/vendedor", Name = "CadastroVendedorPost")]
        public ActionResult Vendedor(ViewModelCadastro master)
        {
            CadastroReduzido model = master.CadastroReduzido;
            model.IdCidadeCobranca = model.IdCidade;
            if (ModelState.IsValid)
            {
                if (VerifyEmailNoRepeat(model.Email, model.TypeCadastro) == false)
                {
                    RepositorioVendedor repositorio = new RepositorioVendedor();
                    repositorio.Insert(
                        model.Email,
                        model.Senha,
                        model.NomeFantasia,
                        model.RazaoSocial,
                        model.IdCidade,
                        model.IdCidadeCobranca,
                        model.DataCadastro,
                        model.Status
                    );
                    TempData["success"] = true;
                    return RedirectToAction("Vendedor");
                } 
                else
                {
                    ModelState.AddModelError("Email", "Email já existente");
                }
            }
            ViewBag.TypeCadastro = "Vendedor";
            ViewBag.RouteNamePost = "CadastroVendedorPost";
            return View("Cadastros", master);
        }
        #endregion

        #region Comments2
        //[HttpPost]
        //public ActionResult Vendedor(VendedorModel model)
        //{
        //    var repositorio = new RepositorioVendedor();
        //    var repositorioAssinatura = new RepositorioAssinatura();
        //    ValidarDadosVendedor(model);

        //    if (ModelState.IsValid)
        //    {
        //        var vendedor = model.CadastroVendedorModel.GetVendedor();
        //        vendedor.DataCadastro = DateTime.Now;
        //        vendedor.Status = true;

        //        repositorio.Insert(vendedor);

        //        TempData.Add("Success", true);

        //        var user = repositorio.Get(vendedor.Email, vendedor.Senha);
        //        if (user != null)
        //        {
        //            bool isAssinante = repositorioAssinatura.GetVendedorIdIsAssinante(user.Id);
        //            Session.Add("userVendedor", SessionVendedor.Create(user.Id, isAssinante, user.DataCadastro.Value, user.Email, user.NomeFantasia));
        //            return RedirectToAction("Index", "Vendas", null);
        //        }
        //    }

        //    //model.SelectCidades();
        //    //model.SelectCidadesCobranca();
        //    Load_Cidades_And_CidadesCobranca(
        //        model.CadastroVendedorModel.UF,
        //        model.CadastroVendedorModel.IdCidade,
        //        model.CadastroVendedorModel.UFCobranca,
        //        model.CadastroVendedorModel.IdCidadeCobranca
        //    );

        //    return View(model);
        //}

        #endregion

        #region DadosVendedorLogado
        [HttpGet]
        [SessionExpire("userVendedor")]
        [Route("dados-vendedor")]
        public ActionResult DadosVendedor()
        {
            SessionVendedor user = Session["userVendedor"] as SessionVendedor;
            RepositorioVendedor repositorioVendedor = new RepositorioVendedor();
            if (user == null)
            {
                return RedirectToAction("Vendedor", "Cadastro");
            }

            var model = new VendedorModel();
            model.CadastroVendedorModel.GetVendedorModel(repositorioVendedor.Get(user.VendedorId));

            //model.SelectCidades();
            //model.SelectCidadesCobranca();
            Load_Cidades_And_CidadesCobranca(
                model.CadastroVendedorModel.UF,
                model.CadastroVendedorModel.IdCidade,
                model.CadastroVendedorModel.UFCobranca,
                model.CadastroVendedorModel.IdCidadeCobranca
            );

            return View(model);
        }

        [HttpPost]
        [SessionExpire("userVendedor")]
        [Route("dados-vendedor")]
        public ActionResult DadosVendedor(VendedorModel model)
        {
            var repositorio = new RepositorioVendedor();
            var repositorioAssinatura = new RepositorioAssinatura();

            ValidarDadosVendedor(model);

            if (ModelState.IsValid)
            {
                var user = Session["userVendedor"] as SessionVendedor;
                var vendedor = model.CadastroVendedorModel.GetVendedor();
                vendedor.Status = true;
                vendedor.DataCadastro = user.DataCadastro;

                try
                {
                    repositorio.Update(vendedor);
                    bool isAssinante = repositorioAssinatura.GetVendedorIdIsAssinante(user.VendedorId);
                    Session.Add("userVendedor", SessionVendedor.Create(vendedor.Id, isAssinante, vendedor.DataCadastro.Value, vendedor.Email, vendedor.NomeFantasia));
                    TempData.Add("Success", true);
                    return RedirectToAction("DadosVendedor");
                }
                catch
                {
                    ModelState.AddModelError("", "Não foi possível efetuar a alteração de seus dados.");
                }
            }

            //model.SelectCidades();
            //model.SelectCidadesCobranca();
            Load_Cidades_And_CidadesCobranca(
                model.CadastroVendedorModel.UF,
                model.CadastroVendedorModel.IdCidade,
                model.CadastroVendedorModel.UFCobranca,
                model.CadastroVendedorModel.IdCidadeCobranca
            );

            return View(model);
        }
        #endregion


        [HttpGet()]
        [Route("login", Name = "LoginViewModelRouteGet")]
        public ActionResult LoginViewModel(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return View(new CotaPedido.WebUI.Models.LoginViewModel { Url = url });
            }
            return View();
        }

        [HttpPost()]
        [Route("login", Name = "LoginViewModelRoutePost")]
        [ValidateAntiForgeryToken]        
        public ActionResult LoginViewModel(CotaPedido.WebUI.Models.LoginViewModel model)
        {            
            if (ModelState.IsValid)
            {
                #region verificando_login_usuario_comprador
                RepositorioComprador repositorio = new RepositorioComprador();
                Comprador comprador = repositorio.Get(model.Email.ToLower(), model.Senha);
                if (comprador != null)
                {
                    comprador.Senha = null;
                    Session.Add("user", comprador);
                    if (!string.IsNullOrEmpty(model.Url))
                    {
                        return Redirect(model.Url);
                    }
                    return RedirectToAction("Index", "Compras", null);
                }
                else
                {
                    #region verificando_login_usuario_vendedor                    
                    RepositorioVendedor repositorioVendedor = new RepositorioVendedor();
                    RepositorioAssinatura repositorioAssinatura = new RepositorioAssinatura();
                    Vendedor vendedor = repositorioVendedor.Get(model.Email.ToLower(), model.Senha);
                    if (vendedor != null)
                    {
                        bool isAssinante = repositorioAssinatura
                            .GetVendedorIdIsAssinante(vendedor.Id);
                        Session.Add("userVendedor", SessionVendedor
                            .Create(vendedor.Id, isAssinante, vendedor.DataCadastro.Value, vendedor.Email, vendedor.NomeFantasia));
                        if (!string.IsNullOrEmpty(model.Url))
                        {
                            return Redirect(model.Url);
                        }
                        return RedirectToAction("Index", "Vendas", null);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email e/ou senha inválidos");
                    }
                    #endregion verificando_login_usuario_vendedor
                }
                #endregion verificando_login_usuario_comprador
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login-comprador")]
        public ActionResult LoginComprador(ViewModelCadastro master)
        {
            LoginCompradorModel model = master.LoginCompradorModel;
            if (ModelState.IsValid)
            {
                var repositorio = new RepositorioComprador();
                var user = repositorio.GetAll().Where(x => x.Email == model.Email.ToLower() && x.Senha == model.Senha).ToList();
                if (user.Count() == 1)
                {
                    Session.Add("user", user.SingleOrDefault());
                    var link = TempData["link"] as string;
                    if (!string.IsNullOrEmpty(link))
                    {
                        var splitLink = link.Split('/');
                        var controller = splitLink[1];
                        var action = splitLink[2];
                        return RedirectToAction(action, controller, new { id = splitLink[3] });
                    }
                    return RedirectToAction("Index", "Compras", null);
                }
                else
                {
                    ModelState.AddModelError("", "Email e/ou senha inválidos");
                }
            }
            return View("Comprador", master);
        }

        [HttpGet]
        [Route("logout-comprador")]
        public ActionResult LogoutComprador()
        {
            Session.Remove("user");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login-vendedor")]
        public ActionResult LoginVendedor(ViewModelCadastro master)
        {
            LoginVendedorModel model = master.LoginVendedorModel;
            if (ModelState.IsValid)
            {
                var repositorio = new RepositorioVendedor();
                var repositorioAssinatura = new RepositorioAssinatura();
                var user = repositorio.Get(model.Email.ToLower(), model.Senha);
                if (user != null)
                {
                    bool isAssinante = repositorioAssinatura.GetVendedorIdIsAssinante(user.Id);
                    Session.Add("userVendedor", SessionVendedor.Create(user.Id, isAssinante, user.DataCadastro.Value, user.Email, user.NomeFantasia));
                    var link = TempData["link"] as string;
                    if (!string.IsNullOrEmpty(link))
                    {
                        var splitLink = link.Split('/');
                        var controller = splitLink[1];
                        var action = splitLink[2];
                        return RedirectToAction(action, controller, new { id = splitLink[3] });
                    }
                    return RedirectToAction("Index", "Vendas", null);
                }
                else
                {
                    ModelState.AddModelError("", "Email e/ou senha inválidos");
                }
            }
            return View("Vendedor", master);
        }

        [HttpGet]
        [Route("logout-vendedor")]
        public ActionResult LogoutVendedor()
        {
            Session.Remove("userVendedor");

            return RedirectToAction("Index", "Home");
        }

        #region metodos_privados
        private void ValidarDadosComprador(CompradorModel model)
        {
            var repositorio = new RepositorioComprador();

            if (!string.IsNullOrEmpty(model.CadastroCompradorModel.Email))
            {
                var compradoresEmail = repositorio.GetAll().Where(x => x.Email == model.CadastroCompradorModel.Email).ToList();
                if (compradoresEmail.Count > 1 || (compradoresEmail.Count == 1 && compradoresEmail.FirstOrDefault().Id != model.CadastroCompradorModel.Id))
                {
                    ModelState.AddModelError("CadastroCompradorModel.Email", "E-Mail já cadastrado");
                }
            }
            if (!string.IsNullOrEmpty(model.CadastroCompradorModel.CNPJ))
            {
                if (model.CadastroCompradorModel.CNPJ.Length == 14 || model.CadastroCompradorModel.CNPJ.Length == 18)
                {
                    if (model.CadastroCompradorModel.CNPJ.Length == 14)
                    {
                        if (!Validate.CPF(model.CadastroCompradorModel.CNPJ))
                        {
                            ModelState.AddModelError("CadastroCompradorModel.CNPJ", "Digite um CPF válido");
                        }
                        else
                        {
                            var compradoresCpf = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroCompradorModel.CNPJ).ToList();
                            if (compradoresCpf.Count > 1 || (compradoresCpf.Count == 1 && compradoresCpf.FirstOrDefault().Id != model.CadastroCompradorModel.Id))
                                ModelState.AddModelError("CadastroCompradorModel.CNPJ", "Este CPF já está cadastrado");
                        }
                    }
                    if (model.CadastroCompradorModel.CNPJ.Length == 18)
                    {
                        if (!Validate.CNPJ(model.CadastroCompradorModel.CNPJ))
                        {
                            ModelState.AddModelError("CadastroCompradorModel.CNPJ", "Digite um CNPJ válido");
                        }
                        else
                        {
                            var compradoresCnpj = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroCompradorModel.CNPJ).ToList();
                            if (compradoresCnpj.Count > 1 || (compradoresCnpj.Count == 1 && compradoresCnpj.FirstOrDefault().Id != model.CadastroCompradorModel.Id))
                                ModelState.AddModelError("CadastroCompradorModel.CNPJ", "Este CNPJ já está cadastrado");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("CadastroCompradorModel.CNPJ", "Digite um CPF/CNPJ válido");
                }
            }
        }

        private void ValidarDadosVendedor(VendedorModel model)
        {
            var repositorio = new RepositorioVendedor();

            if (!string.IsNullOrEmpty(model.CadastroVendedorModel.Email))
            {
                var vendedoresEmail = repositorio.GetAll().Where(x => x.Email == model.CadastroVendedorModel.Email).ToList();
                if (vendedoresEmail.Count > 1 || (vendedoresEmail.Count == 1 && vendedoresEmail.FirstOrDefault().Id != model.CadastroVendedorModel.Id))
                {
                    ModelState.AddModelError("CadastroVendedorModel.Email", "E-Mail já cadastrado");
                }
            }
            if (!string.IsNullOrEmpty(model.CadastroVendedorModel.CNPJ))
            {
                if (model.CadastroVendedorModel.CNPJ.Length == 14 || model.CadastroVendedorModel.CNPJ.Length == 18)
                {
                    if (model.CadastroVendedorModel.CNPJ.Length == 14)
                    {
                        if (!Validate.CPF(model.CadastroVendedorModel.CNPJ))
                        {
                            ModelState.AddModelError("CadastroVendedorModel.CNPJ", "Digite um CPF válido");
                        }
                        else
                        {
                            var vendedoresCpf = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroVendedorModel.CNPJ).ToList();
                            if (vendedoresCpf.Count > 1 || (vendedoresCpf.Count == 1 && vendedoresCpf.FirstOrDefault().Id != model.CadastroVendedorModel.Id))
                                ModelState.AddModelError("CadastroVendedorModel.CNPJ", "Este CPF já está cadastrado");
                        }
                    }
                    if (model.CadastroVendedorModel.CNPJ.Length == 18)
                    {
                        if (!Validate.CNPJ(model.CadastroVendedorModel.CNPJ))
                        {
                            ModelState.AddModelError("CadastroVendedorModel.CNPJ", "Digite um CNPJ válido");
                        }
                        else
                        {
                            var vendedoresCnpj = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroVendedorModel.CNPJ).ToList();
                            if (vendedoresCnpj.Count > 1 || (vendedoresCnpj.Count == 1 && vendedoresCnpj.FirstOrDefault().Id != model.CadastroVendedorModel.Id))
                                ModelState.AddModelError("CadastroVendedorModel.CNPJ", "Este CNPJ já está cadastrado");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("CadastroVendedorModel.CNPJ", "Digite um CPF/CNPJ válido");
                }
            }
        }
        #endregion
    }
}