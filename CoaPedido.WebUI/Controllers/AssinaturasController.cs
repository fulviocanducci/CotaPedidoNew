using CoaPedido.WebUI.Models;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.Infra.Repositorios.SQLServer;
using System;
using System.Web.Mvc;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Domain.Direct;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Service;



namespace CoaPedido.WebUI.Controllers
{
    [SessionExpire("userVendedor")]
    public class AssinaturasController : Controller
    {
        internal IRepositorioMensalidade RepositorioMensalidade { get; }
        internal IRepositorioAssinatura RepositorioAssinatura { get; }
        internal SessionVendedor SessionVendedor { get; set; }

        private SessionVendedor RecuperarVendedor()
        {
            if (Session["userVendedor"] != null)
                return Session["userVendedor"] as SessionVendedor;
            return null;
        }

        public AssinaturasController()
        {
            RepositorioMensalidade = new RepositorioMensalidade();
            RepositorioAssinatura = new RepositorioAssinatura();            
        }       

        public void Load_Datas()
        {            
            ViewBag.Mensalidades = RepositorioMensalidade.GetAll();            
            ViewBag.MensalidadesPorVendedor = RepositorioAssinatura.GetVendedorId(SessionVendedor.VendedorId);
        }
        [HttpGet]
        public ActionResult Index()
        {
            
                SessionVendedor = RecuperarVendedor();
                Load_Datas();
                if (TempData.ContainsKey("Status")
                    && TempData["Status"].ToString() == "3")
                {
                    ViewBag.Status = "1"; //tem que ser status = 1 para dizer que o usuario tem assinaturas
                }
               
            
            return View();
        }

        [HttpPost]
        public ActionResult Save(int? MensalidadeId)
        {
            if (MensalidadeId.HasValue)
            {
                SessionVendedor = RecuperarVendedor();
                if (SessionVendedor != null)
                {
                    Mensalidade mensalidade = RepositorioMensalidade.Get(MensalidadeId.Value);
                    if (mensalidade != null)
                    {
                        int idAssinatura = 0;
                        Assinatura assinatura = new Assinatura();
                        assinatura.DataCadastro = DateTime.Now;
                        assinatura.ChavePagSeguro = "";
                        assinatura.DataFinal = assinatura.DataCadastro.Value.AddDays(mensalidade.QuantidadeDias.Value);
                        assinatura.FormaPagamento = 0;
                        assinatura.IdMensalidade = mensalidade.Id;
                        assinatura.IdVendedor = SessionVendedor.VendedorId;
                        assinatura.Status = "0";
                        assinatura.Valor = mensalidade.Valor;
                        RepositorioAssinatura.Insert(assinatura);
                        idAssinatura = assinatura.Id;

                       

                        if (idAssinatura != 0) //Inicia o processo com o PagSeguro
                        {

                            decimal valorAssinatura = Convert.ToDecimal(mensalidade.Valor);

                            //Novo teste de envio
                            // Essa variável define se é o ambiente de teste ou produção.
                            const bool isSandbox = false;
                            EnvironmentConfiguration.ChangeEnvironment(isSandbox);

                            try
                            {
                                var credentials = PagSeguroConfiguration.Credentials(isSandbox);

                                // Instanciar uma nova requisição de pagamento
                                var payment = new PaymentRequest { Currency = Currency.Brl };

                                // Adicionar produtos
                                if (mensalidade.Id == 1) //Opção Mensal
                                {
                                    payment.Items.Add(new Item("0001", "Assinatura de 30 dias ", 1, valorAssinatura));
                                }
                                else if (mensalidade.Id == 2) //Opção Trimestral 
                                {
                                    payment.Items.Add(new Item("0001", "Assinatura de 90 dias ", 1, valorAssinatura));
                                }
                                else if (mensalidade.Id == 3) //Opção Semestral
                                {
                                    payment.Items.Add(new Item("0001", "Assinatura de 180 dias ", 1, valorAssinatura));
                                }


                                // Código que identifica o pagamento
                                payment.Reference = idAssinatura.ToString();

                                payment.NotificationURL = "http://cotapedido.com/Assinaturas";


                                // Informações do remetente
                                //payment.Sender = new Sender(DadosAluno.AlunoNome, DadosAluno.AlunoEmail, new Phone("18", "981167328"));

                                // URL a redirecionar o usuário após pagamento
                                payment.RedirectUri = new Uri("http://www.cotapedido.com");

                                // Informações extras para identificar o pagamento.
                                // Essas informações são livres para adicionar o que for necessário.
                                //payment.AddMetaData(MetaDataItemKeys.GetItemKeyByDescription("CPF do passageiro"), "123.456.789-09", 1);
                                //payment.AddMetaData("PASSENGER_PASSPORT", "23456", 1);


                                //var senderCpf = new SenderDocument(Documents.GetDocumentByType("CPF"), DadosAluno.AlunoCPF.ToString());
                                //payment.Sender.Documents.Add(senderCpf);

                                var paymentRedirectUri = payment.Register(credentials);

                                Response.Redirect(paymentRedirectUri.ToString());

                            }
                            catch (PagSeguroServiceException exception)
                            {
                                Console.WriteLine(exception.Message + "\n");

                                foreach (var element in exception.Errors)
                                {
                                    Console.WriteLine(element + "\n");
                                }
                                Console.ReadKey();
                            }

                        }

                        TempData.Add("Status", "1");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}