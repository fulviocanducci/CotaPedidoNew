using CotaPedido.Dominio.Util;
using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;
using CotaPedido.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CotaPedido.WebSite.Models
{
    public class CadastroController : Controller
    {
        //
        // GET: /Cadastro/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Comprador()
        {
            var success = TempData["success"] as bool?;
            if (success.HasValue && success.Value)
            {
                ViewBag.Success = true;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Comprador(CompradorModel model)
        {
            var repositorio = new RepositorioComprador();

            var cpfCnpjCadastrado = false;
            if (!string.IsNullOrEmpty(model.CadastroCompradorModel.Email))
            {
                if (repositorio.GetAll().Where(x => x.Email == model.CadastroCompradorModel.Email).ToList().Count() > 0)
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
                            cpfCnpjCadastrado = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroCompradorModel.CNPJ).ToList().Count > 0;
                            if (cpfCnpjCadastrado)
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
                            cpfCnpjCadastrado = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroCompradorModel.CNPJ).ToList().Count() > 0;

                            if (cpfCnpjCadastrado)
                                ModelState.AddModelError("CadastroCompradorModel.CNPJ", "Este CNPJ já está cadastrado");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("CadastroCompradorModel.CNPJ", "Digite um CPF/CNPJ válido");
                }

            }

            if (ModelState.IsValid)
            {
                var comprador = model.CadastroCompradorModel.GetComprador();
                comprador.DataCadastro = DateTime.Now;
                comprador.Status = true;
                
                repositorio.Insert(comprador);
                
                TempData.Add("Success", true);

                var user = repositorio.GetAll().Where(x => x.Email == comprador.Email && x.Senha == comprador.Senha).ToList();

                if (user.Count() == 1)
                {
                    Session.Add("user", user.SingleOrDefault());
                    return RedirectToAction("ListarPedidos","Compras", null);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DadosComprador(CadastroCompradorModel model)
        {
            var repositorio = new RepositorioComprador();

            if (ModelState.IsValid)
            {
                var modelComprador = new CompradorModel();
                var user = Session["user"] as Comprador;
                modelComprador.CadastroCompradorModel = model;
                var comprador = model.GetComprador();
                comprador.CNPJ = user.CNPJ;
                comprador.RG = user.RG;
                comprador.Email = user.Email;
                comprador.Status = true;
                comprador.Id = user.Id;
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

            return View(model);
        }

        [HttpGet]
        public ActionResult DadosComprador()
        {
            var user = Session["user"] as Comprador;
            if(user == null)
            {
                return RedirectToAction("Comprador", "Cadastro");
            }
            if (TempData["Success"] != null)
            {
                ViewBag.Success = true;
            }
            var repositorioCidade = new RepositorioCidade();

            var model = new CadastroCompradorModel
            {
                Bairro = user.Bairro,
                BairroCobranca = user.BairroCobranca,
                Celular = user.Celular,
                CelularCobranca = user.CelularCobranca,
                CEP = user.CEP,
                CEPCobranca = user.CEPCobranca,
                CNPJ = user.CNPJ,
                Complemento = user.Complemento,
                ComplementoCobranca = user.ComplementoCobranca,
                Endereco = user.Endereco,
                EnderecoCobranca = user.EnderecoCobranca,
                IdCidade = user.IdCidade,
                IdCidadeCobranca = user.IdCidadeCobranca,
                IE = user.IE,
                IM = user.IM,
                NomeCompleto = user.RazaoSocial,
                PrimeiroNome = user.NomeFantasia,
                Numero = user.Numero,
                NumeroCobranca = user.NumeroCobranca,
                Site = user.Site,
                Telefone = user.Telefone,
                TelefoneCobranca = user.TelefoneCobranca,
                Email = user.Email
            };
            var cidades = repositorioCidade.GetAll().Where(x => (new List<int> { model.IdCidade, model.IdCidadeCobranca }).Contains(x.Id));
            model.UF = (UF)cidades.Where(x => x.Id == model.IdCidade).FirstOrDefault().UF;
            model.UFCobranca = (UF)cidades.Where(x => x.Id == model.IdCidadeCobranca).FirstOrDefault().UF;

            return View(model);
        }

        [HttpGet]
        public ActionResult Vendedor()
        {
            var success = TempData["success"] as bool?;
            if (success.HasValue && success.Value)
            {
                ViewBag.Success = true;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Vendedor(VendedorModel model)
        {
            var repositorio = new RepositorioVendedor();

            var cpfCnpjCadastrado = false;
            if (!string.IsNullOrEmpty(model.CadastroVendedorModel.Email))
            {
                if (repositorio.GetAll().Where(x => x.Email == model.CadastroVendedorModel.Email).ToList().Count() > 0)
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
                            cpfCnpjCadastrado = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroVendedorModel.CNPJ).ToList().Count > 0;
                            if (cpfCnpjCadastrado)
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
                            cpfCnpjCadastrado = repositorio.GetAll().Where(x => x.CNPJ == model.CadastroVendedorModel.CNPJ).ToList().Count > 0;
                            if (cpfCnpjCadastrado)
                                ModelState.AddModelError("CadastroVendedorModel.CNPJ", "Este CNPJ já está cadastrado");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("CadastroVendedorModel.CNPJ", "Digite um CPF/CNPJ válido");
                }

            }

            if (ModelState.IsValid)
            {
                var vendedor = model.CadastroVendedorModel.GetVendedor();
                vendedor.DataCadastro = DateTime.Now;
                vendedor.Status = true;
                
                repositorio.Insert(vendedor);
                
                TempData.Add("Success", true);

                var user = repositorio.GetAll().Where(x => x.Email == vendedor.Email && x.Senha == vendedor.Senha).ToList();
                if (user.Count() == 1)
                {
                    Session.Add("userVendedor", user.SingleOrDefault());
                    return RedirectToAction("ConsultarPedidos", "Vendas", null);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DadosVendedor()
        {
            var user = Session["userVendedor"] as Vendedor;
            if (user == null)
            {
                return RedirectToAction("Vendedor", "Cadastro");
            }
            if (TempData["Success"] != null)
            {
                ViewBag.Success = true;
            }
            var repositorioCidade = new RepositorioCidade();

            var model = new CadastroVendedorModel
            {
                Bairro = user.Bairro,
                BairroCobranca = user.BairroCobranca,
                Celular = user.Celular,
                CelularCobranca = user.CelularCobranca,
                CEP = user.CEP,
                CEPCobranca = user.CEPCobranca,
                CNPJ = user.CNPJ,
                Complemento = user.Complemento,
                ComplementoCobranca = user.ComplementoCobranca,
                Endereco = user.Endereco,
                EnderecoCobranca = user.EnderecoCobranca,
                IdCidade = user.IdCidade,
                IdCidadeCobranca = user.IdCidadeCobranca,
                IE = user.IE,
                IM = user.IM,
                NomeCompleto = user.RazaoSocial,
                PrimeiroNome = user.NomeFantasia,
                Numero = user.Numero,
                NumeroCobranca = user.NumeroCobranca,
                Site = user.Site,
                Telefone = user.Telefone,
                TelefoneCobranca = user.TelefoneCobranca,
                Email = user.Email
            };

            var cidades = repositorioCidade.GetAll().Where(x => (new List<int> { model.IdCidade, model.IdCidadeCobranca }).Contains(x.Id));
            model.UF = (UF)cidades.Where(x => x.Id == model.IdCidade).FirstOrDefault().UF;
            model.UFCobranca = (UF)cidades.Where(x => x.Id == model.IdCidadeCobranca).FirstOrDefault().UF;

            return View(model);
        }

        [HttpPost]
        public ActionResult DadosVendedor(CadastroVendedorModel model)
        {
            var repositorio = new RepositorioVendedor();

            if (ModelState.IsValid)
            {
                var modelComprador = new VendedorModel();
                var user = Session["user"] as Comprador;
                modelComprador.CadastroVendedorModel = model;
                var vendedor = model.GetVendedor();
                vendedor.CNPJ = user.CNPJ;
                vendedor.RG = user.RG;
                vendedor.Email = user.Email;
                vendedor.Status = true;
                vendedor.Id = user.Id;
                try
                {
                    repositorio.Update(vendedor);
                    Session.Add("user", vendedor);
                    TempData.Add("Success", true);
                    return RedirectToAction("DadosVendedor");
                }
                catch
                {
                    ModelState.AddModelError("", "Não foi possível efetuar a alteração de seus dados.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginComprador(CompradorModel model)
        {
            if (ModelState.IsValid)
            {
                var repositorio = new RepositorioComprador();
                var user = repositorio.GetAll().Where(x => x.Email == model.LoginCompradorModel.Email && x.Senha == model.LoginCompradorModel.Senha).ToList();
                if (user.Count() == 1)
                {
                    Session.Add("user", user.SingleOrDefault());
                    return RedirectToAction("ListarPedidos","Compras", null);
                }
                else
                {
                    ModelState.AddModelError("", "Email e/ou senha inválidos");
                }
            }
            return View("Comprador", model);
        }

        [HttpPost]
        public ActionResult LoginVendedor(VendedorModel model)
        {
            if (ModelState.IsValid)
            {
                var repositorio = new RepositorioVendedor();
                var user = repositorio.GetAll().Where(x => x.Email == model.LoginVendedorModel.Email && x.Senha == model.LoginVendedorModel.Senha).ToList();
                if (user.Count() == 1)
                {
                    Session.Add("userVendedor", user.SingleOrDefault());
                    return RedirectToAction("ConsultarPedidos", "Vendas", null);
                }
                else
                {
                    ModelState.AddModelError("", "Email e/ou senha inválidos");
                }
            }
            return View("Vendedor", model);
        }

    }
}