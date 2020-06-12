using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios.SQLServer;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace CotaPedido.WebUI.Models
{
    public class CompradorModel
    {
        public CompradorModel()
        {
            CadastroCompradorModel = new CadastroCompradorModel();
            Cidades = new List<SelectListItem>();
            CidadesCobranca = new List<SelectListItem>();            
        }

        public CadastroCompradorModel CadastroCompradorModel { get; set; }

        public LoginCompradorModel LoginCompradorModel { get; set; }

        public List<SelectListItem> Cidades { get; set; }

        public List<SelectListItem> CidadesCobranca { get; set; }        

        public void SelectCidades()
        {
            //Cidades = (new RepositorioCidade()).GetAll().Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() + "\" data-uf=\"" + x.UF }).ToList();
            Cidades = (new RepositorioCidade())
                .GetAll()
                .Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() })
                .ToList();
        }

        public void SelectCidadesCobranca()
        {
            //CidadesCobranca = (new RepositorioCidade()).GetAll().Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() + "\" data-uf=\"" + x.UF }).ToList();
            CidadesCobranca = (new RepositorioCidade())
                .GetAll()
                .Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() })
                .ToList();
        }

    }
}