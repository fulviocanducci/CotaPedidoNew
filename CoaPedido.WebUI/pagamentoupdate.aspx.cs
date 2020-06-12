using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoaPedido.WebUI.Models;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.Infra.Repositorios.SQLServer;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Domain.Direct;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Service;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;



namespace CoaPedido.WebUI
{
    public partial class pagamentoupdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.HttpMethod == "POST")
            {

                string valor = Request.Form["notificationCode"]; //Request.QueryString["transaction_id"]; //
                string notificacao = Request.Form["notificationType"];

                Assinatura assinatura = new Assinatura();

                InsereLogPagseguro("Notificao: " + notificacao + " valor: " + valor);

                if (valor != null)
                {

                    var credentials = PagSeguroConfiguration.Credentials(false);

                    // Transação Normal
                    //var transaction = NotificationService.CheckTransaction(credentials, "766B9C-AD4B044B04DA-77742F5FA653-E1AB24", false);

                    Transaction transaction = NotificationService.CheckTransaction(credentials, valor);

                    
                    assinatura.Status = transaction.TransactionStatus.ToString();
                    assinatura.ChavePagSeguro = transaction.Code.ToString();
                    assinatura.Id = Convert.ToInt32(transaction.Reference.ToString());

                    RegistraRetornoOperadorCartao(assinatura);

                }
            }

            else
            {

                string valor = Request.QueryString["transaction_id"];

                if (valor != null)
                {

                    var requisicaoWeb = WebRequest.CreateHttp("https://ws.pagseguro.uol.com.br/v2/transactions/"+ valor +"?email=sergio.dsilva@desenve.com&token=1B25F0FEC7204D1BBCC93A9FB7F2F714");
                    requisicaoWeb.Method = "GET";
                    requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                    string Retorno;
                    string codStatus;
                    string referencia;
                    int inicio;
                    int fim;

                    using (var resposta = requisicaoWeb.GetResponse())
                    {
                        var streamDados = resposta.GetResponseStream();
                        StreamReader reader = new StreamReader(streamDados);
                        object objResponse = reader.ReadToEnd();

                        Retorno = objResponse.ToString();

                        inicio = Retorno.IndexOf("<status>");
                        fim = Retorno.IndexOf("</status>");

                        int tamanho = fim - (inicio + 8);

                        codStatus = Retorno.Substring(inicio + 8, tamanho);  //Status da Trasancao

                        inicio = Retorno.IndexOf("<reference>");
                        fim = Retorno.IndexOf("</reference>");
                        tamanho = fim - (inicio + 11);

                        referencia = Retorno.Substring(inicio + 11, tamanho);  //Referencia 


                        streamDados.Close();
                        resposta.Close();

                        Assinatura assinatura = new Assinatura();

                        assinatura.Status = codStatus;
                        assinatura.ChavePagSeguro = valor;
                        assinatura.Id = Convert.ToInt32(referencia);

                        RegistraRetornoOperadorCartao(assinatura);

                        InsereLogPagseguro("Pagamento: " + referencia + " Status = " + codStatus + " ID =" + valor);

                        Response.Redirect("http://cotapedido.com/Assinaturas");
                    }

                }

            }


        }

        
        public void InsereLogPagseguro(string retorno)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ToString();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = cn,

                    CommandText = " INSERT INTO pagsegurolog (retorno) values (@retorno) ",

                };

                cmd.Parameters.AddWithValue("@retorno", retorno);
               
                cn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {
                throw new Exception("Servidor SQL Erro:" + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }

        }



        public void RegistraRetornoOperadorCartao(Assinatura dados)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ToString();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = cn,

                    CommandText = " UPDATE Assinaturas SET Status = @Status , ChavePagSeguro = @ChavePagSeguro   WHERE Id = @Id ",

                };

                cmd.Parameters.AddWithValue("@ChavePagSeguro", dados.ChavePagSeguro);
                cmd.Parameters.AddWithValue("@Status", dados.Status);
                cmd.Parameters.AddWithValue("@Id", dados.Id);

                cn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {
                throw new Exception("Servidor SQL Erro:" + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }

        }



    }
}