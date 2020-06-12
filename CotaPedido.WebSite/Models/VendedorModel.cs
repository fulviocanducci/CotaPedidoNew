using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CotaPedido.WebSite.Models
{
    public class VendedorModel
    {
        #region Propriedades Públicas

        public CadastroVendedorModel CadastroVendedorModel { get; set; }

        public LoginVendedorModel LoginVendedorModel { get; set; }

        #endregion

        #region Métodos Públicos

        public static List<Cidade> SelectCidades()
        {
            return (new RepositorioCidade()).GetAll();
        }

        #endregion
    }
}