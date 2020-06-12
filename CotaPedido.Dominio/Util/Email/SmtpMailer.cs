using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CotaPedido.Dominio.Util.Email
{
    public class SmtpMailer
    {
        public static void EnviarEmail(Email email)
        {
            try
            {


                var cliente = CriarClienteEmail();
                var mail = MontarEmail(email);

                cliente.Send(mail);
            }
            catch (Exception ex)
            {
                throw new SmtpException("Erro ao enviar e-mail: " + ex.Message);
            }
        }

        private static MailMessage MontarEmail(Email email)
        {
            var mail = new MailMessage
            {
                Subject = email.Titulo,
                Body = email.Conteudo,
                IsBodyHtml = true,
                From = new MailAddress(email.EmailRemetente, email.NomeRemetente)
            };

            foreach (var destinatario in email.Destinatarios)
            {
                //mail.To.Add(destinatario); Teste para enviar os e-mail como ocultos
                mail.Bcc.Add(destinatario);
            }

            foreach (var anexo in email.Anexos)
            {
                mail.Attachments.Add(anexo);
            }

            return mail;
        }

        private static SmtpClient CriarClienteEmail()
        {
            var cliente = new SmtpClient
            {
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtp_porta"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["credentials"]),
                Host = ConfigurationManager.AppSettings["smtp_servidor"],
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtp_login"], ConfigurationManager.AppSettings["smtp_senha"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtp_ssl"])
            };

            return cliente;
        }
    }
}
