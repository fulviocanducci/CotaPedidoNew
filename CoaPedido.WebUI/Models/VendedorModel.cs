using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios.SQLServer;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace CotaPedido.WebUI.Models
{
    public class VendedorModel
    {
        #region Propriedades Públicas

        public VendedorModel()
        {
            CadastroVendedorModel = new CadastroVendedorModel();
            Cidades = new List<SelectListItem>();
            CidadesCobranca = new List<SelectListItem>();
        }

        public CadastroVendedorModel CadastroVendedorModel { get; set; }

        public LoginVendedorModel LoginVendedorModel { get; set; }

        public List<SelectListItem> Cidades { get; set; }

        public List<SelectListItem> CidadesCobranca { get; set; }

        #endregion

        #region Métodos Públicos

        public void SelectCidades()
        {
            Cidades = (new RepositorioCidade()).GetAll().Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() + "\" data-uf=\"" + x.UF }).ToList();
        }

        public void SelectCidadesCobranca()
        {
            CidadesCobranca = (new RepositorioCidade()).GetAll().Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() + "\" data-uf=\"" + x.UF }).ToList();
        }

        #endregion
    }
}