using CoaPedido.WebUI.Models;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.Infra.Repositorios.SQLServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace CoaPedido.WebUI.Controllers
{
    [SessionExpire("userVendedor")]
    public class NotificacaoController : Controller
    {
        internal IRepositorioGrupo RepositorioGrupo { get; }
        internal IRepositorioSubGrupo RepositorioSubGrupo { get; }
        internal IRepositorioCidade RepositorioCidade { get; }
        internal IRepositorioRegiao RepositorioRegiao { get; }
        internal IRepositorioPais RepositorioPais { get; }
        internal IRepositorioEstado RepositorioEstado { get; }
        internal IRepositorioAviso RepositorioAviso { get; }
        
        public Expression<Func<SessionVendedor, IList<AvisoViewModel>, bool>> TestValidationButtonAdd =
            (a, b) => (a.IsAssinante && b.Count < 5) || (!a.IsAssinante && b.Count < 1);

        public NotificacaoController()
        {
            RepositorioGrupo = new RepositorioGrupo();
            RepositorioSubGrupo = new RepositorioSubGrupo();
            RepositorioCidade = new RepositorioCidade();
            RepositorioRegiao = new RepositorioRegiao();
            RepositorioPais = new RepositorioPais();
            RepositorioEstado = new RepositorioEstado();
            RepositorioAviso = new RepositorioAviso();
        }

        protected override void Dispose(bool disposing)
        {            
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Index()
        {
            SessionVendedor vendedor = RecuperarVendedor();
            Load_Select();
            if (TempData.ContainsKey("Status"))
            {
                ViewBag.Status = TempData["Status"];
            }
            IList<AvisoViewModel> model = ViewBag.AvisosFromVendedor as IList<AvisoViewModel>;
            ViewBag.BtnIsAssinante = false;
            if (model != null)
            {
                if (TestValidationButtonAdd.Compile()(vendedor, model))
                {
                    ViewBag.BtnIsAssinante = true;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                Aviso aviso = RepositorioAviso.Get(id.Value);
                if (aviso != null)
                {
                    if (RepositorioAviso.Delete(aviso) > 0)
                    {
                        TempData["Status"] = "excluído com êxito";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Aviso aviso = RepositorioAviso.Get(id.Value);
                if (aviso != null)
                {
                    int? valor = 0;
                    if (aviso.AvisoTipo == 1)
                        valor = aviso.AvisoPais;
                    else if (aviso.AvisoTipo == 2)
                        valor = aviso.AvisoEstado;
                    else if (aviso.AvisoTipo == 3)
                        valor = aviso.AvisoRegiao;
                    else if (aviso.AvisoTipo == 4)
                        valor = aviso.AvisoCidade;                    
                    Load_Select(aviso.AvisoTipo, aviso.AvisoGrupo, aviso.AvisoSubGrupo, valor, aviso.AvisoId);
                }
                return View("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Save(Notificacao model)
        {
            SessionVendedor vendedor = RecuperarVendedor();
            if (ModelState.IsValid)
            {
                Aviso aviso = (Aviso)model;
                if (aviso.AvisoId > 0)
                {
                    RepositorioAviso.Update(aviso);                    
                    TempData["Status"] = "atualizada com êxito";
                }
                else
                {
                    var avisos = RepositorioAviso.GetAvisosFromVendedor(vendedor.VendedorId);
                    if (TestValidationButtonAdd.Compile()(vendedor, avisos))
                    {
                        RepositorioAviso.Insert(aviso);
                        TempData["Status"] = "cadastrada com êxito";
                    }
                    else
                    {
                        TempData["Status"] = "O número de avisos já ultrapassou o seu limite";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        protected void Load_Select(int? area = null, int? categoria = null, int? subcategoria = null, int? valor = null, int? id = null)
        {
            string [] itemNames = System.Enum.GetNames(typeof(CotaPedido.Entidades.Enum.AvisoTipo));
            var items = new object[itemNames.Length];
            int itemCount = 0;
            foreach (string item in itemNames)
            {
                if (System.Enum.TryParse(item, out CotaPedido.Entidades.Enum.AvisoTipo value))
                {
                    items[itemCount++] = new { Value = (int)value, Name = item };
                }
            }            

            var grupo = RepositorioGrupo.GetAll();
            var idGrupo = categoria ?? ((grupo.Count > 0) ? grupo.FirstOrDefault().Id : 0);

            var subGrupo = RepositorioSubGrupo.GetListOfGrupo(idGrupo);
            var idSubGrupo = subcategoria; // ?? ((subGrupo.Count() > 0) ? subGrupo.FirstOrDefault().Id : 0);
                        
            ViewBag.Categoria = new SelectList(grupo, "Id", "Nome", idGrupo);
            ViewBag.SubCategoria = new SelectList(subGrupo, "Id", "Nome", idSubGrupo);

            ViewBag.AreadeAtuacao = new SelectList(items.ToArray(), "Value", "Name", area);
            ViewBag.Valor = new SelectList(area == null ? new object[] { }.ToArray() : GetValorToIEnumerable(area), "Id", "Nome", valor);

            ViewBag.VendedorId = RecuperarGetVendedorId();
            ViewBag.Id = id.HasValue ? id.Value.ToString() : "0";

            ViewBag.Btn = "Gravar";
            if (id.HasValue && id > 0)
            {
                ViewBag.Btn = "Alterar";
            }

            ViewBag.AvisosFromVendedor = RepositorioAviso.GetAvisosFromVendedor(RecuperarGetVendedorId());
        }

        [HttpGet]
        public JsonResult GetSubCategoria(int? id)
        {
            if (id.HasValue)
            {
                return Json(RepositorioSubGrupo.GetListOfGrupo(id.Value), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        //[OutputCache(Duration = 3600, VaryByParam = "id")]
        public JsonResult GetValor(int? id)
        {
            return Json(GetValorToIEnumerable(id), JsonRequestBehavior.AllowGet);
        }
                
        private SessionVendedor RecuperarVendedor()
        {
            if (Session["userVendedor"] != null)
                return Session["userVendedor"] as SessionVendedor;
            return null;
        }

        private int RecuperarGetVendedorId()
        {
            var vendedor = RecuperarVendedor();
            if (vendedor != null)
            {
                return vendedor.VendedorId;
            }
            return 0;
        }

        public IEnumerable GetValorToIEnumerable(int? id)
        {
            IEnumerable items = null;
            if (id.HasValue)
            {
                switch (id.Value)
                {
                    case 1: // Pais
                        {
                            items = RepositorioPais.GetAll();
                            break;
                        }
                    case 2: // Estado
                        {
                            items = RepositorioEstado.GetAll();
                            break;
                        }
                    case 3: // Região
                        {
                            items = RepositorioRegiao.GetAll();
                            break;
                        }
                    case 4: // Cidade
                        {
                            items = RepositorioCidade.GetAllWithUf();
                            break;
                        }
                }
            }
            return items;
        }
    }
}