using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CotaPedido.WebSite.Models
{
    public class CompradorModel
    {
        public CadastroCompradorModel CadastroCompradorModel { get; set; }

        public LoginCompradorModel LoginCompradorModel { get; set; }

        public static List<Cidade> SelectCidades()
        {
            return (new RepositorioCidade()).GetAll();
        }
    }
}