using CotaPedido.Entidades.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace CotaPedido.WebUI.Models
{
    public class CadastroReduzido
    {
        public int Id { get; set; }

        [Display(Name = "E-Mail")]
        [MaxLength(200, ErrorMessage = "O campo E-Mail deve conter no máximo 200 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail não é válido")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Preencha com um E-Mail válido")]
        [Required(ErrorMessage = "Preencha o campo E-Mail")]
        [System.Web.Mvc.Remote("EmailNoRepeat","Cadastro", ErrorMessage = "E-mail já existente", HttpMethod = "POST", AdditionalFields = "TypeCadastro")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [MaxLength(8, ErrorMessage = "O campo Senha deve conter no máximo 8 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Senha")]
        public string Senha { get; set; }

        [Display(Name = "Repita a Senha")]
        [MaxLength(20, ErrorMessage = "O campo Repita a Senha deve conter no máximo 20 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Repita a Senha")]
        [Compare("Senha", ErrorMessage = "As senhas devem ser iguais")]
        public string RepitaSenha { get; set; }

        [Display(Name = "Primeiro Nome")]
        [MaxLength(64, ErrorMessage = "O campo Primeiro Nome deve conter no máximo 64 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Primeiro Nome")]
        public string NomeFantasia { get; set; } //Nome

        [Display(Name = "Nome Completo")]
        [MaxLength(200, ErrorMessage = "O campo Nome Completo deve conter no máximo 200 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Nome Completo")]
        public string RazaoSocial { get; set; } // Nome Completo

        [Required(ErrorMessage = "Selecione uma Cidade")]
        public int IdCidade { get; set; }
        public int IdCidadeCobranca { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public bool Status { get; set; } = true;
        
        [Required(ErrorMessage = "Escolha a Unidade Federativa")]
        [Range(1, 30, ErrorMessage = "Escolha a Unidade Federativa")]
        public UF UF  { get;set; }

        [Required()]
        public string TypeCadastro { get; set; }
    }
}