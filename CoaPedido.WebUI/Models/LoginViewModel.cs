using System.ComponentModel.DataAnnotations;

namespace CotaPedido.WebUI.Models
{
    public class LoginViewModel
    {
        [Display(Name = "E-Mail")]
        [StringLength(200, ErrorMessage = "O campo E-Mail não pode conter mais do que 200 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail não é válido")]
        [Required(ErrorMessage = "Preencha o campo E-Mail")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [StringLength(20, ErrorMessage = "O campo Senha não pode conter mais do que 20 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Senha")]
        public string Senha { get; set; }

        public string Url { get; set; }
    }
}