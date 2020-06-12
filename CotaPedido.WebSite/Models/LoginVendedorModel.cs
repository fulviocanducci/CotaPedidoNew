using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CotaPedido.WebSite.Models
{
    public class LoginVendedorModel
    {
        [Display(Name = "E-Mail")]
        [StringLength(200)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail não é válido")]
        [Required(ErrorMessage = "Preencha o campo E-Mail")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [StringLength(20)]
        [Required(ErrorMessage = "Preencha o campo Senha")]
        public string Senha { get; set; }
    }
}