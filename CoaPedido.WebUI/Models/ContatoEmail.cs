using System.ComponentModel.DataAnnotations;
namespace CoaPedido.WebUI.Models
{
    public class ContatoEmail
    {
        [Required(ErrorMessage = "Digite o seu nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o seu e-mail")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite a mensagem")]
        public string Mensagem { get; set; }
        public string Telefone { get; set; }
    }
}