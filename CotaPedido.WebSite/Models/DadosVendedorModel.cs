using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CotaPedido.WebSite.Models
{
    public class DadosVendedorModel
    {
        [Display(Name = "Primeiro Nome")]
        [StringLength(64)]
        [Required(ErrorMessage = "Preencha o campo Primeiro Nome")]
        public string PrimeiroNome { get; set; }

        [Display(Name = "Nome Completo")]
        [StringLength(200)]
        [Required(ErrorMessage = "Preencha o campo Nome Completo")]
        public string NomeCompleto { get; set; }

        [Display(Name = "E-Mail")]
        [StringLength(200)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail não é válido")]
        [Required(ErrorMessage = "Preencha o campo E-Mail")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [StringLength(20)]
        [Required(ErrorMessage = "Preencha o campo Senha")]
        public string Senha { get; set; }

        [Display(Name = "Repita a Senha")]
        [StringLength(20)]
        [Required(ErrorMessage = "Preencha o campo Repita a Senha")]
        public string RepitaSenha { get; set; }

        [Display(Name = "UF")]
        [StringLength(2)]
        [Required(ErrorMessage = "Preencha o campo UF")]
        public string UF { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(100)]
        [Required(ErrorMessage = "Preencha o campo Cidade")]
        public string Cidade { get; set; }

        [Display(Name = "Telefone")]
        [StringLength(15)]
        [Required(ErrorMessage = "Preencha o campo Telefone")]
        public string Telefone { get; set; }

        [Display(Name = "Celular")]
        [StringLength(15)]
        [Required(ErrorMessage = "Preencha o campo Celular")]
        public string Celular { get; set; }

        [Display(Name = "Nome Fantasia")]
        [StringLength(120)]
        public string NomeFantasia { get; set; }

        [Display(Name = "CNPJ")]
        [StringLength(15)]
        public string CNPJ { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(120)]
        public string Endereco { get; set; }

        [Display(Name = "Número")]
        [StringLength(15)]
        public string Numero { get; set; }

        [Display(Name = "CEP")]
        [StringLength(15)]
        public string CEP { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(64)]
        public string Bairro { get; set; }

        [Display(Name = "Site")]
        [StringLength(200)]
        public string Site { get; set; }
    }
}