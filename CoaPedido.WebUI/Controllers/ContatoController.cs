using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoaPedido.WebUI.Models;
using CotaPedido.Dominio.Util.Email;

namespace CoaPedido.WebUI.Controllers
{
    public class ContatoController : Controller
    {
        // GET: Contato
        public ActionResult Index()
        {
            if (TempData.ContainsKey("Mensagem") && TempData["Mensagem"].ToString() == "1")
            {
                ViewBag.Status = "1"; //
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Save(ContatoEmail contato)
        {
            if (ModelState.IsValid)
            {
                string Nome = contato.Nome;

                List<string> destinatario = new List<string>();
                destinatario.Add("sergio.dsilva@desenve.com");
                destinatario.Add("contato@cotapedido.com");

                var email = new Email
                {
                    Titulo = "Contato CotaPedido",
                    Conteudo = string.Format("Nome: " + Nome + "<br>Telefone: " + contato.Telefone + "<br>E-mail: " + contato.Email + "<br>Mensagem: <br>" + contato.Mensagem),
                    EmailRemetente = contato.Email,
                    NomeRemetente = Nome,
                    Destinatarios = destinatario
                };

                SmtpMailer.EnviarEmail(email);

                TempData.Add("Mensagem", "1");

                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}