using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CotaPedido.Dominio.Util.Email
{
    public class Email
    {
        public Email()
        {
            Anexos = new List<Attachment>();
        }

        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public List<string> Destinatarios { get; set; }
        public string NomeRemetente { get; set; }
        public string EmailRemetente { get; set; }
        public List<Attachment> Anexos { get; set; }
    }
}
