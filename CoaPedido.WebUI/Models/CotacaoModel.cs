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
    public class CotacaoModel
    {
        #region Construtores

        public CotacaoModel()
        {
            Itens = new List<ItemCotacaoModel>();
            Pedidos = new List<Pedido>();
        }
        
        #endregion

        #region Propriedades Públicas

        public List<ItemCotacaoModel> Itens { get; set; }

        public int Id { get; set; }

        public int IdPedido { get; set; }

        public int IdVendedor { get; set; }

        [Required(ErrorMessage = "O campo Data Cotação é obrigatório")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Pedido. Ex.: 01/01/2015")]
        public string DataCotacao { get; set; }

        public HttpPostedFileBase Arquivo { get; set; }

        public int? CodigoArea { get; set; }

        public int? IdCidade { get; set; }

        public int? IdCategoria { get; set; }
        
        public string DescricaoCategoria { get; set; }

        public int? IdSubCategoria { get; set; }

        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Pedido. Ex.: 01/01/2015")]
        public string DataLimiteProposta { get; set; }

        public string EmailVendedor { get; set; }

        public string NomeVendedor { get; set; }

        public List<Cotacao> Cotacoes { get; set; }

        public List<Pedido> Pedidos { get; set; }

        public List<Grupo> Categorias { get; set; }

        public IPagedList<Cotacao> CotacoesPaged { get; set; }

        public IPagedList<Pedido> PedidosPaged { get; set; }

        public Pedido Pedido { get; set; }

        public string NomeCidade { get; set; }

        public int IdCotacao { get; set; }
        #endregion

        #region Métodos Públicos

        public void CarregarCotacoes(int? page)
        {
            var repositorioCotacoes = new RepositorioCotacao();

            var cotacoes = repositorioCotacoes.GetList(new CotacaoFilter
            {
                CodigoArea = this.CodigoArea,
                IdCidade = this.IdCidade,
                IdGrupo = this.IdCategoria,
                IdSubGrupo = this.IdSubCategoria,
                DescricaoCategoria = this.DescricaoCategoria,
                DataLimiteProposta = !string.IsNullOrEmpty(this.DataLimiteProposta) ? (DateTime?)Convert.ToDateTime(this.DataLimiteProposta).Date : null,
                IdVendedor = this.IdVendedor,
                Status = 1,
                NomeCidade = this.NomeCidade
            });

            CotacoesPaged = cotacoes.ToPagedList(page.HasValue ? page.Value : 1, 10);
        }

        //Tentar passar isso para o pedido, para listar os pedidos que estão publicados e não os que possuem uma cotacao.
        public void CarregarTodasCotacoes()
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var repositorioItensPedidos = new RepositorioItemPedido();

            Cotacoes = repositorioCotacoes.GetAll(new CotacaoFilter
            {
                DataLimiteProposta = DateTime.Now.Date,
                SituacaoPedido = 2
            });

            var idsPedidos = Cotacoes.Select(c => c.IdPedido).Distinct().ToList();
            var itens = repositorioItensPedidos.GetList(new ItemPedidoFilter { IdsPedidos = string.Join(", ", idsPedidos) });

            Cotacoes.Select(c => c.Pedido).ToList().ForEach(p => p.Itens.AddRange(itens.Where(i => i.IdPedido == p.Id)));
        }



        public void CarregarCotacoesCategoria(int idCategoria, int idSubCategoria)
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var repositorioItensPedidos = new RepositorioItemPedido();

            Cotacoes = repositorioCotacoes.GetCotacoesCategoria(idCategoria, idSubCategoria);

            var idsPedidos = Cotacoes.Select(c => c.IdPedido).Distinct().ToList();
            var itens = repositorioItensPedidos.GetList(new ItemPedidoFilter { IdsPedidos = string.Join(", ", idsPedidos) });

            Cotacoes.Select(c => c.Pedido).ToList().ForEach(p => p.Itens.AddRange(itens.Where(i => i.IdPedido == p.Id)));
        }

        public void CarregarPedidos(int? page)
        {
            var repositorioPedidos = new RepositorioPedido();
            var repositorioCotacoes = new RepositorioCotacao();

            var pedidos = repositorioPedidos.GetList(new PedidoFilter
            {
                CodigoArea = this.CodigoArea,
                IdCidade = this.IdCidade,
                IdGrupo = this.IdCategoria,
                IdSubGrupo = this.IdSubCategoria,
                DescricaoCategoria = this.DescricaoCategoria,
                DataLimiteProposta = !string.IsNullOrEmpty(this.DataLimiteProposta) ? (DateTime?)Convert.ToDateTime(this.DataLimiteProposta).Date : null,
                SituacaoPedido = (int)SituacaoPedido.EmAberto,
                Status = 1, 
                NomeCidade = this.NomeCidade
            });

            var listIdsPedidos = pedidos.Where(p => p.NumeroCotacoes >= 0).Select(p => p.Id).Distinct().ToList();
            var idsPedidos = string.Join(",", listIdsPedidos);
            if (idsPedidos == "")
            {
                idsPedidos = "0";
            }
            var cotacoes = repositorioCotacoes.GetIdCotacoesPedidos(idsPedidos, this.IdVendedor);

            pedidos.ForEach(p => p.Cotacoes.AddRange(cotacoes.Where(c => c.IdPedido == p.Id)));

            PedidosPaged = pedidos.ToPagedList(page.HasValue ? page.Value : 1, 10);
        }

        public void CarregarPedido(int idPedido)
        {
            var repositorioPedidos = new RepositorioPedido();
            var repositorioItensPedidos = new RepositorioItemPedido();

            Pedido = repositorioPedidos.Get(idPedido);

            var itens = repositorioItensPedidos.GetList(new ItemPedidoFilter { IdPedido = Pedido.Id });
            Pedido.Itens = itens;
        }

        public void CarregaItens(List<ItemPedido> list)
        {
            foreach (var item in list)
            {
                Itens.Add(new ItemCotacaoModel
                {
                    NomeProduto = item.Descricao,
                    IdItem = item.Id,
                    IdCategoria = item.IdCategoria,
                    Categoria = item.Categoria,
                    IdSubCategoria = item.IdSubCategoria,
                    SubCategoria = item.SubCategoria,
                    IdUnidade =  item.IdUnidade,
                    Unidade = item.Unidade,
                    Quantidade = item.Quantidade.Value,
                    ValorUnitario = 0.00M,
                    ValorTotal = 0.00M,
                });
            }
        }

        public List<Grupo> ObterCategorias()
        {
            var grupoRepositorio = new RepositorioGrupo();
            return grupoRepositorio.GetAll();
        }

        public List<SubGrupo> ObterSubCategorias()
        {
            var subGrupoRepositorio = new RepositorioSubGrupo();
            return subGrupoRepositorio.GetAll();
        }

        public List<Regiao> ObterRegioes()
        {
            var regiaoRepositorio = new RepositorioRegiao();
            return regiaoRepositorio.GetAll();
        }

        public List<Cidade> ObterCidades()
        {
            var cidadeRepositorio = new RepositorioCidade();
            return cidadeRepositorio.GetAll();
        }

        public List<SelectListItem> ObterSelectCidades()
        {
            return ObterCidades().Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() + "\" data-regiao=\"" + x.IdRegiao }).ToList();
        }

        public List<SelectListItem> ObterSelectRegioes()
        {
            return ObterRegioes().Select(x => new SelectListItem { Value = x.CodigoArea.ToString(), Text = x.Nome }).ToList();
        }

        public List<SelectListItem> ObterSelectRegioesUFId()
        {
            return ObterRegioes().Select(x => new SelectListItem { Value = x.IdUF.ToString(), Text = x.Nome }).ToList();
        }

        public List<SelectListItem> ObterSelectCategorias()
        {
            return ObterCategorias().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome  }).ToList();
        }

        public List<SelectListItem> ObterSelectSubCategorias()
        {
            return ObterSubCategorias().Select(x => new SelectListItem { Text = x.Nome,  Value = x.Id.ToString() + "\" data-categoria=\"" + x.IdGrupo  }).ToList();
        }

        public void CarregarCotacao(int idCotacao)
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var cotacao = repositorioCotacoes.Get(idCotacao);

            IdPedido = cotacao.IdPedido.Value;
            IdVendedor = cotacao.IdVendedor.Value;
            DataCotacao = cotacao.DataCadastro.Value.ToString("dd/MM/yyyy");
            IdCotacao = cotacao.Id;

            CarregarPedido(IdPedido);

            foreach (var item in Pedido.Itens)
            {
                Itens.Add(new ItemCotacaoModel
                {
                    Id = cotacao.Itens.Where(i => i.IdItem == item.Id).Select(i => i.Id).SingleOrDefault(),
                    NomeProduto = item.Descricao,
                    IdItem = item.Id,
                    IdCategoria = item.IdCategoria,
                    Categoria = item.Categoria,
                    IdSubCategoria = item.IdSubCategoria,
                    SubCategoria = item.SubCategoria,
                    Unidade = item.Unidade,
                    Quantidade = item.Quantidade.Value,
                    ValorUnitario = cotacao.Itens.Where(i => i.IdItem == item.Id).Select(i => i.ValorUnitario.Value).SingleOrDefault(),
                    ValorTotal = cotacao.Itens.Where(i => i.IdItem == item.Id).Select(i => i.ValorTotal.Value).SingleOrDefault(),
                });
            }
        }

        public void InserirCotacao()
        {
            using (var scope = new TransactionScope())
            {
                var cotacaoRepositorio = new RepositorioCotacao();
                var itemCotacaoRepositorio = new RepositorioItemCotacao();

                var cotacao = new Cotacao
                {
                    DataCadastro = Convert.ToDateTime(DataCotacao),
                    IdPedido = IdPedido,
                    IdVendedor = IdVendedor,
                    Status = true,
                    ValorTotal = Itens.Sum(i => (i.ValorUnitario * i.Quantidade))
                };
                cotacao.Id = cotacaoRepositorio.Insert(cotacao);

                var itensCotacao = Itens.Select(x => new ItemCotacao
                {
                    IdCotacao = cotacao.Id,
                    IdItem = x.IdItem,
                    DataCadastro = cotacao.DataCadastro,
                    Quantidade = x.Quantidade,
                    ValorUnitario = x.ValorUnitario,
                    ValorTotal = (x.ValorUnitario * x.Quantidade)
                }).ToList();

                foreach (var item in itensCotacao)
                {
                    itemCotacaoRepositorio.Insert(item);
                }

                var compradorRepositorio = new RepositorioComprador();
                var emailComprador = new List<string>();
                emailComprador.Add(compradorRepositorio.GetEmailCompradorPorIdPedido(this.IdPedido));

                var email = new Email
                {
                    Titulo = "Nova Cotação",
                    Conteudo = string.Format(GerarConteudoEmailCotacao(), "Nova cotação do pedido " + this.IdPedido + " realizada!", GerarBotaoEmailCotacao("http://www.cotapedido.com/Compras/Comparar/" + this.IdPedido)),
                    EmailRemetente = this.EmailVendedor,
                    NomeRemetente = "Cota Pedido", //this.NomeVendedor,
                    Destinatarios = emailComprador
                };

                SmtpMailer.EnviarEmail(email);

                scope.Complete();
            }
        }

        public void EditarCotacao()
        {
            using (var scope = new TransactionScope())
            {
                var cotacaoRepositorio = new RepositorioCotacao();
                var itemCotacaoRepositorio = new RepositorioItemCotacao();

                var cotacao = new Cotacao
                {
                    Id = Id,
                    DataCadastro = Convert.ToDateTime(DataCotacao),
                    IdPedido = IdPedido,
                    IdVendedor = IdVendedor,
                    Status = true,
                    ValorTotal = Itens.Sum(i => i.ValorTotal)
                };
                cotacaoRepositorio.Update(cotacao);

                var itensCotacao = Itens.Select(x => new ItemCotacao
                {
                    Id = x.Id,
                    IdCotacao = cotacao.Id,
                    IdItem = x.IdItem,
                    DataCadastro = cotacao.DataCadastro,
                    Quantidade = x.Quantidade,
                    ValorUnitario = x.ValorUnitario,
                    ValorTotal = x.ValorTotal
                }).ToList();

                foreach (var item in itensCotacao)
                {
                    itemCotacaoRepositorio.Update(item);
                }

                var compradorRepositorio = new RepositorioComprador();
                var emailComprador = new List<string>();
                emailComprador.Add(compradorRepositorio.GetEmailCompradorPorIdPedido(this.IdPedido));

                var email = new Email
                {
                    Titulo = "Nova Cotação",
                    Conteudo = string.Format(GerarConteudoEmailCotacao(), "Nova cotação do pedido " + this.IdPedido + " realizada!", GerarBotaoEmailCotacao("http://localhost:3222/Compras/Comparar/" + this.IdPedido)),
                    EmailRemetente = this.EmailVendedor,
                    NomeRemetente = "Cota Pedido", //this.NomeVendedor,
                    Destinatarios = emailComprador
                };

                SmtpMailer.EnviarEmail(email);

                scope.Complete();
            }
        }

        public void LerArquivoItensCotacao(string arquivo)
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

                CarregarItensArquivo(dados);
            }
        }

        #endregion

        #region Métodos Privados

        private void CarregarItensArquivo(DataTable dados)
        {
            foreach (DataRow dr in dados.Rows)
            {
                var idItem = Convert.ToInt32(dr.ItemArray[0]);
                if (!Itens.Any(i => i.IdItem == idItem))
                {
                    throw new InvalidOperationException("Item importado não faz parte da lista de itens do pedido");
                }

                decimal valorUnitario;
                if(!Decimal.TryParse(dr.ItemArray[2].ToString(), out valorUnitario))
                {
                    throw new InvalidOperationException("Valor unitário informado, não é um valor válido");
                }

                var quantidade = Itens.Where(i => i.IdItem == idItem).FirstOrDefault().Quantidade;
                Itens.Where(i => i.IdItem == idItem).FirstOrDefault().ValorUnitario = valorUnitario;
                Itens.Where(i => i.IdItem == idItem).FirstOrDefault().ValorTotal = valorUnitario * quantidade;
            }

            if (Id > 0)
            {
                EditarCotacao();
            }
            else
            {
                InserirCotacao();
            }
        }

        private string GerarConteudoEmailCotacao()
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

        private string GerarBotaoEmailCotacao(string url)
        {
            var conteudo = @"<tr>
                                <td style='padding:0px; background:#FFF; border:1px solid #E1E1E1; border-top:none; border-bottom:none'>
                                    <table width='100%' cellpadding='0' cellspacing='0' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='text-align:center; padding:0px 20px 30px'><br>
                                                    <a href='{0}' target='_blank' 
                                                        style='background:#FE980F; border: none; border-radius: 0; color: #FFFFFF; margin-top: 17px; padding: 10px; text-decoration: none; border-radius: 10px' rel='nofollow'>Comparar Cotações</a>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>";

            return string.Format(conteudo, url);
        }

        #endregion

        public void ExibirPedidosHome()
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var repositorioItensPedidos = new RepositorioItemPedido();
            var repositorioPedidos = new RepositorioPedido();

            Cotacoes = repositorioCotacoes.GetAll(new CotacaoFilter
            {
                DataLimiteProposta = DateTime.Now.Date,
                SituacaoPedido = 2                
            });

            Pedidos = repositorioPedidos.GetList(new PedidoFilter
            {
                DataLimiteProposta = DateTime.Now.Date,
                SituacaoPedido = 2
            });

            var idsPedidos = Pedidos.Select(c => c.Id).Distinct().ToList();
            var itens = repositorioItensPedidos.GetList(new ItemPedidoFilter { IdsPedidos = string.Join(", ", idsPedidos) });

           Pedidos.ToList().ForEach(p => p.Itens.AddRange(itens.Where(i => i.IdPedido == p.Id)));
        }

        //Alterado por Sérgio em 15/01/2018 -> para listar os pedidos sem a necessidade de ter cotações, filtrando pela sua categoria e subcategoria.
        public void ExibirPedidosHomePorCategorias(int idCategoria, int idSubCategoria)
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var repositorioItensPedidos = new RepositorioItemPedido();
            var repositorioPedidos = new RepositorioPedido();

            Cotacoes = repositorioCotacoes.GetCotacoesCategoria(idCategoria, idSubCategoria);

            Pedidos = repositorioPedidos.GetList(new PedidoFilter
            {
                DataLimiteProposta = DateTime.Now.Date,
                SituacaoPedido = 2, 
                IdGrupo = idCategoria,
                IdSubGrupo = idSubCategoria
            });

            var idsPedidos = Pedidos.Select(c => c.Id).Distinct().ToList();
            var itens = repositorioItensPedidos.GetList(new ItemPedidoFilter { IdsPedidos = string.Join(", ", idsPedidos) });

            //Pedidos.Select(c => c.Pedido).ToList().ForEach(p => p.Itens.AddRange(itens.Where(i => i.IdPedido == p.Id)));

            Pedidos.ToList().ForEach(p => p.Itens.AddRange(itens.Where(i => i.IdPedido == p.Id)));

        }

        //Adição Fulvio
        public void ExibirPedidosHomePorCategorias(string categoria, string subCategoria)
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var repositorioItensPedidos = new RepositorioItemPedido();
            var repositorioPedidos = new RepositorioPedido();

            var repositorioGrupo = new RepositorioGrupo();
            var repositorioSubGrupo = new RepositorioSubGrupo();

            Grupo grupo = repositorioGrupo.Get(categoria);
            SubGrupo subGrupo = repositorioSubGrupo.Get(subCategoria);

            int idCategoria = grupo.Id;
            int idSubCategoria = subGrupo.Id;

            Cotacoes = repositorioCotacoes.GetCotacoesCategoria(idCategoria, idSubCategoria);

            Pedidos = repositorioPedidos.GetList(new PedidoFilter
            {
                DataLimiteProposta = DateTime.Now.Date,
                SituacaoPedido = 2,
                IdGrupo = idCategoria,
                IdSubGrupo = idSubCategoria
            });

            var idsPedidos = Pedidos.Select(c => c.Id).Distinct().ToList();
            var itens = repositorioItensPedidos.GetList(
                new ItemPedidoFilter { IdsPedidos = string.Join(", ", idsPedidos) }
            );
            Pedidos.ToList().ForEach(p => p.Itens.AddRange(itens.Where(i => i.IdPedido == p.Id)));
        }
        public void ExibirPedidosHomePorDescricaoPedido(string descricao)
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var repositorioItensPedidos = new RepositorioItemPedido();
            var repositorioPedidos = new RepositorioPedido();
            var repositorioGrupo = new RepositorioGrupo();
            var repositorioSubGrupo = new RepositorioSubGrupo();

            //Grupo grupo = repositorioGrupo.Get(categoria);
            //SubGrupo subGrupo = repositorioSubGrupo.Get(subCategoria);
            //int idCategoria = grupo.Id;
            //int idSubCategoria = subGrupo.Id;
            //Cotacoes = repositorioCotacoes.GetCotacoesCategoria(idCategoria, idSubCategoria);

            var itens = repositorioItensPedidos.GetList(new ItemPedidoFilter { Descricao = descricao });
            var idsPedidos = itens.Select(c => c.IdPedido).Distinct().ToList();
            Pedidos = repositorioPedidos.GetList(new PedidoFilter { Ids = string.Join(", ", idsPedidos) });            
            Pedidos.ToList().ForEach(p => p.Itens.AddRange(itens.Where(i => i.IdPedido == p.Id)));
            
            //Pedidos = Pedidos.Where(p => p.Itens.Count > 0).ToList();
        }
    }
}