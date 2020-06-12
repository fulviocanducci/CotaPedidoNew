using CotaPedido.Dominio.Util.Email;
using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;
using CotaPedido.Entidades.Filter;
using CotaPedido.Infra.Repositorios.SQLServer;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace CotaPedido.WebUI.Models
{
    public class PedidoModel
    {
        #region Construtores

        public PedidoModel()
        {
            Itens = new List<ItemModel>();
            ItensCotacao = new List<ItemCotacaoModel>();
            TotaisUnitarios = new List<decimal>();
        }

        #endregion

        #region Propriedades Públicas

        public List<ItemModel> Itens { get; set; }

        public List<ItemCotacaoModel> ItensCotacao { get; set; }

        public List<Decimal> TotaisUnitarios { get; set; }

        public int Id { get; set; }

        public SituacaoPedido SituacaoPedido { get; set; }

        public int IdComprador { get; set; }

        [Required(ErrorMessage = "O campo Data Pedido é obrigatório")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Pedido. Ex.: 01/01/2015")]
        public string DataPedido { get; set; }

        public string NomeCidade { get; set; }

        [Required(ErrorMessage = "O campo Data Limite Proposta é obrigatório")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Limite Proposta. Ex.: 01/01/2015")]
        public string DataLimiteProposta { get; set; }

        [Required(ErrorMessage = "O campo Categoria Principal é obrigatório")]
        public int IdCategoriaPrincipal { get; set; }

        public string Observacao { get; set; }

        public string CategoriaPrincipal { get; set; }

        public int? FiltroIdCategoria { get; set; }

        public string FiltroDescricaoCategoria { get; set; }

        public int? FiltroIdSubCategoria { get; set; }

        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Pedido. Ex.: 01/01/2015")]
        public string FiltroDataLimiteProposta { get; set; }

        public Comprador Comprador { get { var repositorio = new RepositorioComprador(); return repositorio.Get(IdComprador); } }

        public IPagedList<Pedido> PedidosPaged { get; set; }

        #endregion

        #region Métodos Públicos

        public void CarregarPedidos(int? page)
        {
            var repositorioPedidos = new RepositorioPedido();

            var pedidos = repositorioPedidos.GetList(new PedidoFilter
            {
                IdGrupo = this.FiltroIdCategoria,
                IdSubGrupo = this.FiltroIdSubCategoria,
                DescricaoCategoria = this.FiltroDescricaoCategoria,
                DataLimiteProposta = !string.IsNullOrEmpty(this.FiltroDataLimiteProposta) ? (DateTime?)Convert.ToDateTime(this.FiltroDataLimiteProposta).Date : null,
                IdComprador = this.IdComprador,
                Status = 1,
                NomeCidade = this.NomeCidade
            });

            PedidosPaged = pedidos.ToPagedList(page.HasValue ? page.Value : 1, 10);
        }

        //Feito por Sergio para tentar listar apenas os pedidos na home
        public void ExibirPedidosHome()
        {

            var repositorioItensPedidos = new RepositorioItemPedido();
            var pedidos = repositorioItensPedidos.GetList(new PedidoFilter
            {
                DataLimiteProposta = DateTime.Now.Date,
                Status = 1
            });

            var idsPedidos = pedidos.Select(c => c.IdPedido).Distinct().ToList();
            var itens = repositorioItensPedidos.GetList(new ItemPedidoFilter { IdsPedidos = string.Join(", ", idsPedidos) });

        }

        public List<Grupo> ObterCategorias()
        {
            var grupoRepositorio = new RepositorioGrupo();
            return grupoRepositorio.GetAll();
        }

        public List<Unidade> ObterUnidades()
        {
            var unidadeRepositorio = new RepositorioUnidade();
            return unidadeRepositorio.GetAll();
        }

        public List<SubGrupo> ObterSubCategorias()
        {
            var subGrupoRepositorio = new RepositorioSubGrupo();
            return subGrupoRepositorio.GetAll();
        }

        public List<SelectListItem> ObterSelectCategorias()
        {
            return ObterCategorias().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome, Selected = x.Id == this.IdCategoriaPrincipal }).ToList();
        }
        public List<SelectListItem> ObterSelectUnidades()
        {
            return ObterUnidades().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Descricao, Selected = x.Id == this.Id }).ToList();
        }

        public List<SelectListItem> ObterSelectSubCategorias()
        {
            return ObterSubCategorias().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome }).ToList();
        }

        public void CarregarPedido(int idPedido)
        {
            var repositorioPedidos = new RepositorioPedido();
            var pedido = repositorioPedidos.Get(idPedido);

            Id = pedido.Id;
            IdComprador = pedido.IdComprador.Value;
            DataLimiteProposta = pedido.Validade.Value.ToString("dd/MM/yyyy");
            DataPedido = pedido.DataCadastro.Value.ToString("dd/MM/yyyy");
            IdCategoriaPrincipal = pedido.IdCategoriaPrincipal.Value;
            SituacaoPedido = pedido.SituacaoPedido;
            Observacao = pedido.Observacao;
            CategoriaPrincipal = pedido.Categoria.Nome;
            NomeCidade = pedido.NomeCidade;

        }

        public void CarregarItens(int idPedido)
        {
            var repositorioItens = new RepositorioItemPedido();
            Itens = repositorioItens.GetList(new ItemPedidoFilter { IdPedido = idPedido }).Select(i => new ItemModel
            {
                Id = i.Id,
                IdCategoria = i.IdCategoria,
                IdSubcategoria = i.IdSubCategoria,
                NomeProduto = i.Descricao,
                Quantidade = i.Quantidade.HasValue ? i.Quantidade.Value : 0.0M,
                Categoria = i.Categoria,
                SubCategoria = i.SubCategoria,
                IdUnidade = Convert.ToInt16(i.IdUnidade),
                Unidade = i.Unidade
            }).ToList();
        }

        public void InserirItensPedido()
        {
            var itensPedido = Itens.Where(i => !i.Excluido).Select(x => new ItemPedido
            {
                IdPedido = Id,
                Quantidade = x.Quantidade,
                Descricao = x.NomeProduto,
                IdUnidade = x.IdUnidade,
                IdCategoria = x.IdCategoria,
                IdSubCategoria = x.IdSubcategoria
            }).ToList();

            using (var scope = new TransactionScope())
            {
                var itemPedidoRepositorio = new RepositorioItemPedido();
                var pedidoRepositorio = new RepositorioPedido();
                var pedido = pedidoRepositorio.Get(Id);

                pedido.SituacaoPedido = SituacaoPedido.Pendente;
                pedido.Observacao = Observacao;
                pedidoRepositorio.Update(pedido);

                foreach (var item in itensPedido)
                {
                    itemPedidoRepositorio.Insert(item);
                }

                var vendedorRepositorio = new RepositorioVendedor();

                //var email = new Email
                //{
                //    Titulo = "Novo Pedido de Cotação",
                //    Conteudo = string.Format(GerarConteudoEmailPedido(), "Novo pedido de cotação cadastrado!", GerarBotaoEmailPedido(RetornaUrl() + "/Vendas/DetalhesPedido/" + this.Id)),
                //    EmailRemetente = Comprador.Email,
                //    NomeRemetente = "Cota Pedido", //Comprador.NomeFantasia,
                //    Destinatarios = vendedorRepositorio.GetEmailsVendedoresPorAviso()
                //};

                //SmtpMailer.EnviarEmail(email);

                scope.Complete();
            }
        }

        public void EditarItensPedido()
        {
            var itensPedido = Itens.Where(i => !i.Excluido).Select(x => new ItemPedido
            {
                Id = x.Id,
                IdPedido = Id,
                Quantidade = x.Quantidade,
                Descricao = x.NomeProduto,
                IdUnidade = x.IdUnidade,
                IdCategoria = x.IdCategoria,
                IdSubCategoria = x.IdSubcategoria
            }).ToList();
            var itensPedidoExcluir = Itens.Where(i => i.Excluido).Select(i => new ItemPedido { Id = i.Id }).ToList();
            var itensPedidoEditar = itensPedido.Where(x => x.Id != 0).ToList();
            var itensPedidoInserir = itensPedido.Where(x => x.Id == 0).ToList();

            using (var scope = new TransactionScope())
            {
                var itemPedidoRepositorio = new RepositorioItemPedido();
                var pedidoRepositorio = new RepositorioPedido();
                var pedido = pedidoRepositorio.Get(Id);

                pedido.SituacaoPedido = SituacaoPedido.Pendente;
                pedidoRepositorio.Update(pedido);

                foreach (var item in itensPedidoInserir)
                {
                    itemPedidoRepositorio.Insert(item);
                }

                foreach (var item in itensPedidoEditar)
                {
                    itemPedidoRepositorio.Update(item);
                }

                foreach (var item in itensPedidoExcluir)
                {
                    itemPedidoRepositorio.Delete(item);
                }

                var vendedorRepositorio = new RepositorioVendedor();
                //var email = new Email
                //{
                //    Titulo = "Edição de Pedido de Cotação",
                //    Conteudo = string.Format(GerarConteudoEmailPedido(), "Pedido de cotação " + this.Id + " editado!", GerarBotaoEmailPedido(RetornaUrl() + "/Vendas/DetalhesPedido/" + this.Id)),
                //    EmailRemetente = Comprador.Email,
                //    NomeRemetente = "Cota Pedido", //Comprador.NomeFantasia,
                //    Destinatarios = vendedorRepositorio.GetEmailsVendedores()
                //};
                //SmtpMailer.EnviarEmail(email);

                scope.Complete();
            }
        }

        public void LerArquivoItensPedido(string arquivo)
        {
            string ext = Path.GetExtension(arquivo); //arquivo é o path 
            string aspas = "\"";
            string conexao = string.Empty;

            if (ext == ".xls")
            {
                conexao = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + arquivo + ";" + "Extended Properties=" + aspas + "Excel 8.0;HDR=YES" + aspas;
            }
            if (ext == ".xlsx")
            {
                conexao = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + arquivo + ";" + "Extended Properties=" + aspas + "Excel 12.0;HDR=YES" + aspas;
            }

            System.Data.OleDb.OleDbConnection cn = new System.Data.OleDb.OleDbConnection();
            cn.ConnectionString = conexao;
            cn.Open();

            object[] restricoes = { null, null, null, "TABLE" };
            DataTable dtSchema = cn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, restricoes);

            if (dtSchema.Rows.Count > 0)
            {
                string sheet = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                System.Data.OleDb.OleDbCommand comando = new System.Data.OleDb.OleDbCommand("SELECT * FROM [" + sheet + "]", cn);

                DataTable dados = new DataTable();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(comando);
                da.Fill(dados);

                cn.Close();

                var itensPedido = CarregarItensArquivo(dados);
                InserirItensPedidoArquivo(itensPedido);
            }
        }

        public void CarregarCotacoesPedido(int idPedido)
        {
            var repositorioPedidos = new RepositorioPedido();

            CarregaItensCotacoesPedido(repositorioPedidos.GetCotacoesPedido(idPedido, 3));
        }

        public void ConfirmarCotacao(int idCotacao, Comprador comprador)
        {
            var repositorioCotacao = new RepositorioCotacao();
            var cotacao = repositorioCotacao.Get(idCotacao);
            var repositorioPedido = new RepositorioPedido();
            var pedido = repositorioPedido.Get(this.Id);
            var repositorioVendedor = new RepositorioVendedor();
            var vendedor = repositorioVendedor.Get(cotacao.IdVendedor.Value);
            var destinatarios = new List<string>();
            destinatarios.Add(vendedor.Email);

            cotacao.CotacaoEscolhida = true;
            repositorioCotacao.Update(cotacao);

            pedido.SituacaoPedido = SituacaoPedido.Cotado;
            repositorioPedido.Update(pedido);

            using (var scope = new TransactionScope())
            {               

                var emailVendedor = new Email
                {
                    Titulo = "Cotação Contemplada - Pedido: " + this.Id,
                    Conteudo = string.Format(GerarConteudoEmailPedido(), "Sua cotação do pedido " + this.Id + " foi contemplada, segue informações para contato com o comprador:", GerarConteudoEmailCotacaoEscolhida(idCotacao, comprador.NomeFantasia, comprador.RazaoSocial, comprador.Telefone, comprador.Celular, comprador.Email, comprador.Site)),
                    EmailRemetente = comprador.Email,
                    NomeRemetente = "Cota Pedido", //comprador.NomeFantasia,
                    Destinatarios = destinatarios

                };
                SmtpMailer.EnviarEmail(emailVendedor);

                destinatarios.RemoveAt(0);
                destinatarios.Add(comprador.Email);
                var emailComprador = new Email
                {
                    Titulo = "Cotação Contemplada - Pedido: " + this.Id,
                    Conteudo = string.Format(GerarConteudoEmailPedido(), "Cotação contemplada do pedido " + this.Id + ", segue informações para contato com o vendedor:", GerarConteudoEmailCotacaoEscolhida(idCotacao, vendedor.NomeFantasia, vendedor.RazaoSocial, vendedor.Telefone, vendedor.Celular, vendedor.Email, vendedor.Site)),
                    EmailRemetente = vendedor.Email,
                    NomeRemetente = "Cota Pedido", //vendedor.NomeFantasia,
                    Destinatarios = destinatarios

                };
                SmtpMailer.EnviarEmail(emailComprador);

                var idsNaoContemplados = ItensCotacao.Where(i => i.IdCotacao != idCotacao).Select(i => i.IdCotacao).Distinct().ToList();
                if (idsNaoContemplados.Count > 0)
                {
                    destinatarios.RemoveAt(0);
                    //Nas linhas abaixo está enviando o email tanto para os não contemplados e o contemplado também.
                    //Outro item seria importante enviar os não contemplados como ocultos. - 18/07/2019.
                    destinatarios.AddRange(repositorioVendedor.GetEmailsVendedoresNaoContemplados(idsNaoContemplados));
                    var emailNaoContemplados = new Email
                    {
                        Titulo = "Cotação Não Contemplada - Pedido: " + this.Id,
                        Conteudo = string.Format(GerarConteudoEmailPedido(), "Sua cotação do pedido " + this.Id + " não foi contemplada, muito obrigado pela participação!", ""),
                        EmailRemetente = comprador.Email,
                        NomeRemetente = "Cota Pedido", //comprador.NomeFantasia,
                        Destinatarios = destinatarios
                    };

                    SmtpMailer.EnviarEmail(emailNaoContemplados);
                }

                
                scope.Complete();
            }
        }

        public void CancelarPedido(int idPedido)
        {
            var pedidoRepositorio = new RepositorioPedido();
            var pedido = pedidoRepositorio.Get(idPedido);

            pedido.Status = false;
            pedidoRepositorio.Update(pedido);

            var vendedorRepositorio = new RepositorioVendedor();
            var email = new Email
            {
                Titulo = "Cancelamento de Pedido de Cotação",
                Conteudo = string.Format(GerarConteudoEmailPedido(), "Pedido de cotação " + idPedido + " cancelado!", ""),
                EmailRemetente = Comprador.Email,
                NomeRemetente = "Cota Pedido", //Comprador.NomeFantasia,
                Destinatarios = vendedorRepositorio.GetEmailsVendedores()
            };

            SmtpMailer.EnviarEmail(email);
        }

        public void PublicarPedido(int idPedido)
        {
            var pedidoRepositorio = new RepositorioPedido();
            var pedido = pedidoRepositorio.Get(idPedido);

            pedido.SituacaoPedido = SituacaoPedido.EmAberto; //Situacao = Emaberto é a mesma coisa que publicado.
            pedidoRepositorio.Update(pedido);

            var vendedorRepositorio = new RepositorioVendedor();
            List<string> Listagememail = vendedorRepositorio.GetEmailsVendedoresPorAviso(idPedido);

            if (Listagememail.Count > 0)
            { 
            
                var email = new Email
                {
                    Titulo = "Publicação de Pedido de Cotação",
                    Conteudo = string.Format(GerarConteudoEmailPedido(), "Pedido de cotação " + idPedido + " já está publicado!", GerarBotaoEmailPedido(RetornaUrl() + "/Vendas/DetalhesPedido/" + idPedido)),
                    EmailRemetente = "contato@desenve.com",  //Comprador.Email, 
                    NomeRemetente = "Cota Pedido", //Comprador.NomeFantasia,
                    Destinatarios = vendedorRepositorio.GetEmailsVendedoresPorAviso(idPedido)
                };

                SmtpMailer.EnviarEmail(email);
            }
        }

        #endregion

        #region Métodos Privados

        private List<ItemPedido> CarregarItensArquivo(DataTable dados)
        {
            var itensPedido = new List<ItemPedido>();

            foreach (DataRow dr in dados.Rows)
            {
                var itemPedido = new ItemPedido
                {
                    IdPedido = Id,
                    Quantidade = Convert.ToInt32(dr.ItemArray[0]),
                    Descricao = dr.ItemArray[1].ToString(),
                    IdCategoria = Convert.ToInt32(dr.ItemArray[2]),
                    IdSubCategoria = Convert.ToInt32(dr.ItemArray[3])
                };

                itensPedido.Add(itemPedido);
            }

            return itensPedido;
        }

        private void InserirItensPedidoArquivo(List<ItemPedido> itensPedido)
        {
            using (var scope = new TransactionScope())
            {
                var itemPedidoRepositorio = new RepositorioItemPedido();
                var pedidoRepositorio = new RepositorioPedido();
                var pedido = pedidoRepositorio.Get(Id);

                pedido.SituacaoPedido = SituacaoPedido.Pendente;
                pedidoRepositorio.Update(pedido);

                foreach (var item in itensPedido)
                {
                    itemPedidoRepositorio.Insert(item);
                }

                scope.Complete();
            }
        }

        private void CarregaItensCotacoesPedido(List<ItemCotacao> list)
        {
            foreach (var item in list)
            {
                ItensCotacao.Add(new ItemCotacaoModel
                {
                    IdItem = item.IdItem.Value,
                    IdCotacao = item.IdCotacao.Value,
                    Quantidade = item.Quantidade.Value,
                    ValorUnitario = item.ValorUnitario.Value,
                    ValorTotal = item.ValorTotal.Value
                });
            }

            var idsCotacao = ItensCotacao.Select(ic => ic.IdCotacao).Distinct().ToList();
            foreach (var idCotacao in idsCotacao)
            {
                TotaisUnitarios.Add(ItensCotacao.Where(ic => ic.IdCotacao == idCotacao).Sum(ic => ic.ValorUnitario));
            }
        }

        private string GerarConteudoEmailCotacaoEscolhida(int idCotacao, string nomeFantasia, string razaoSocial, string telefone, string celular, string email, string site)
        {
            var itensCotacaoString = " <table  border=1><tr><th>Item</th><th>Quantidade</th><th>Valor Unitário</th><th>Valor Total</th></tr><tr> ";
            var itensVendedor = ItensCotacao.Where(i => i.IdCotacao == idCotacao);
            var totalCotacao = 0M;
            var nomeProduto = "";

            foreach (var item in itensVendedor)
            {
                nomeProduto = Itens.Where(i => i.Id == item.IdItem).Select(i => i.NomeProduto).SingleOrDefault();
                totalCotacao += item.ValorUnitario * item.Quantidade;
                itensCotacaoString += "<td>" + nomeProduto + " </td><td> " + item.Quantidade + " </td><td> " + item.ValorUnitario + " </td><td> " + (item.ValorUnitario * item.Quantidade).ToString("N2") + "</td></tr>";
            }
            itensCotacaoString += "<tr><td colspan='3'>Total Cotação</td><td colspan='1'>" + totalCotacao.ToString("N2") + "</td></tr></table>";

            var conteudo = @"<tr>
                                <td style='padding:0px; background:#FFF; border:1px solid #E1E1E1; border-top:none; border-bottom:none'>
                                    <table width='100%' cellpadding='0' cellspacing='0' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='padding:0 20px; background:#FFF; word-wrap:break-word'>
                                                    <div style='padding:25px 0; border-bottom:1px solid #EEE' style='font-family:Helvetica,Arial; font-size:16px; font-weight:bold; color:#009abe; text-decoration:none'>
                                                        Nome Fantasia: {0}
                                                        <div style='font-family:Helvetica,Arial; font-size:14px; line-height:20px; color:#787878; max-width:690px'>
                                                            <br>Razão Social: {1}
                                                            <br>Telefone: {2}
                                                            <br>Celular: {3}
                                                            <br>E-mail: {4}
                                                            <br>Site: {5}              
                                                        </div>
                                                    </div>
                                                    <div style='padding:25px 0; border-bottom:1px solid #EEE' style='font-family:Helvetica,Arial; font-size:16px; font-weight:bold; color:#009abe; text-decoration:none'>
                                                        Cotação:
                                                        <div style='font-family:Helvetica,Arial; font-size:14px; line-height:20px; color:#787878; max-width:690px'>
                                                            {6}                
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>";

            return string.Format(conteudo, nomeFantasia, razaoSocial, telefone, celular, email, site, itensCotacaoString);
        }

        private string GerarConteudoEmailPedido()
        {
            var conteudo = @"<div>
                                <div>
                                    <style type='text/css'>
                                        <!-- -->
                                    </style>
                                    <div>
                                        <table width='100%' bgcolor='#EEEEEE' cellpadding='0' cellspacing='0' border='0'>
                                            <tbody>
                                                <tr>
                                                    <td align='center' style='font-family:Helvetica,Arial; padding:0px'>
                                                        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='width:100%; max-width:690px'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='padding:0px'>
                                                                        <table width='100%' cellpadding='0' cellspacing='0' border='0'>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <div>
	                                                                                    <h2 style='font-size: 27px; text-align:center; text-transform: uppercase'><span style='color:#FE980F'>COTA </span>Pedido</h2>
                                                                                    </div>
                                                                                </tr>
                                    <tr>
                                        <td bgcolor='#FFFFFF' style='padding:20px 20px 0; border:1px solid #E1E1E1; border-top:none; border-bottom:none'>
                                            <div style='font-size:22px; line-height:32px; text-align:center; color:#00638a '>
                                                {0}
                                                <br>
                                            </div>
                                        </td>
                                    </tr>
                                    </tbody>
                                    </table>
                                    </td>
                                    </tr>
                                    {1}
                                    <tr>
                                        <td valign='middle'>
                                            <div style='background:#373737; border:1px solid #E1E1E1; border-top:none'>
                                                <table width='100%' cellpadding='0' cellspacing='0' border='0' style='padding:0 20px'>
                                                    <tbody>
                                                        <tr>
                                                            <td height='50' valign='middle' align='left' style='font-size:11px; color:#8E8E8E'>
                                                                © Cota Pedido, 2016
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    </tbody>
                                    </table>
                                    </td>
                                    </tr>
                                    </tbody>
                                    </table>
                                </div>
                                <br>
                                <br>
                            </div>";

            return conteudo;
        }

        private string GerarBotaoEmailPedido(string url)
        {
            var conteudo = @"<tr>
                                <td style='padding:0px; background:#FFF; border:1px solid #E1E1E1; border-top:none; border-bottom:none'>
                                    <table width='100%' cellpadding='0' cellspacing='0' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='text-align:center; padding:0px 20px 30px'><br>
                                                    <a href='{0}' target='_blank' 
                                                        style='background:#FE980F; border: none; border-radius: 0; color: #FFFFFF; margin-top: 17px; padding: 10px; text-decoration: none; border-radius: 10px' rel='nofollow'>Detalhes do Pedido</a>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>";

            return string.Format(conteudo, url);
        }

        private string RetornaUrl()
        {
            return "http://www.cotapedido.com";
        }

        #endregion
    }
}