using System.ComponentModel.DataAnnotations;

namespace CotaPedido.WebUI.Models
{
    public class LoginCompradorModel
    {
        [Display(Name = "E-Mail")]
        [MaxLength(200, ErrorMessage="O campo E-Mail não pode conter mais do que 200 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail não é válido")]
        [Required(ErrorMessage = "Preencha o campo E-Mail")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [MaxLength(20, ErrorMessage = "O campo Senha não pode conter mais do que 20 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Senha")]
        public string Senha { get; set; }
    }
}