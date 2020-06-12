using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using CotaPedido.Entidades.Filter;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioCotacao : SqlRepositoryBase<Cotacao>, IRepositorioCotacao
    {
        #region Construtores

        public RepositorioCotacao()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Cotacao entity)
        {
            var retorno = -1;

            Query = @" INSERT INTO Cotacoes (DataCadastro, FormaPagamento, ValorTotal, Status, IdVendedor, IdPedido, Observacao) 
                            VALUES (@DataCadastro, @FormaPagamento, @ValorTotal, @Status, @IdVendedor, @IdPedido, @Observacao) SELECT CAST(SCOPE_IDENTITY() as int)";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Cotacao entity)
        {
            var retorno = -1;

            Query = @"UPDATE Cotacoes SET FormaPagamento = @FormaPagamento , DataCadastro = @DataCadastro, ValorTotal = @ValorTotal, Status = @Status , IdVendedor = @IdVendedor , IdPedido = @IdPedido, Observacao = @Observacao, CotacaoEscolhida = @CotacaoEscolhida " +
                         " WHERE Id = @Id";

            SetParameters(entity);
            _parameters.Id = entity.Id;
            _parameters.CotacaoEscolhida = entity.CotacaoEscolhida;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Cotacao entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Cotacoes WHERE Id = @Id";

            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Cotacao Get(int id)
        {
            Cotacao cotacao;
            var itens = new List<ItemCotacao>();

            InitializeQuery();

            Query = JoinPredicate(Query);
            Query += @"c.Id = @Id";

            _parameters.Id = id;

            using (_connection = new SqlConnection(_connectionString))
            {
                cotacao = _connection.Query<Cotacao, ItemCotacao, Cotacao>(Query, (c, i) =>
                {
                    itens.Add(i);

                    return c;
                }, _parameters as object).FirstOrDefault();
            }

            cotacao.Itens = itens;

            return cotacao;
        }

        public override List<Cotacao> GetAll()
        {
            List<Cotacao> cotacoes;

            InitializeQueryTodasCotacoes();

            using (_connection = new SqlConnection(_connectionString))
            {
                cotacoes = _connection.Query<Cotacao>(Query)
                    .ToList();
            }

            return cotacoes;
        }

        public override List<Cotacao> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Cotacao> GetList(object filter)
        {
            throw new NotImplementedException();
        }

        public List<Cotacao> GetAll(CotacaoFilter filter)
        {
            List<Cotacao> cotacoes;

            InitializeQueryTodasCotacoes();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                cotacoes = _connection.Query<Cotacao, Pedido, Cotacao>(Query,
                    (c, p) =>
                    {
                        c.Pedido = p;

                        return c;
                    }, _parameters as object).ToList();
            }

            return cotacoes;
        }

        public List<Cotacao> GetList(CotacaoFilter filter)
        {
            List<Cotacao> cotacoes;

            InitializeQueryFiltrarCotacoes();

            SetWhereClauses(filter);

            Query += @" GROUP BY p.Id, p.Validade, p.DataCadastro, p.IdGrupoPrincipal, g.Id, g.Nome, c.Id, c.DataCadastro ";

            using (_connection = new SqlConnection(_connectionString))
            {
                cotacoes = _connection.Query<Cotacao, Pedido, Grupo, Cotacao>(Query,
                    (c, p, g) =>
                    {
                        p.Categoria = g;
                        c.Pedido = p;

                        return c;
                    }, _parameters as object).ToList();
            }

            return cotacoes;
        }

        public List<Cotacao> GetCotacoesCategoria(int idCategoria, int idSubCategoria)
        {
            List<Cotacao> cotacoes;

            InitializeQueryCotacoesCategoria();

            _parameters.IdCategoria = idCategoria;
            _parameters.IdSubCategoria = idSubCategoria;
            _parameters.SituacaoPedido = 2;

            using (_connection = new SqlConnection(_connectionString))
            {
                cotacoes = _connection.Query<Cotacao, Pedido, Grupo, Cotacao>(Query,
                    (c, p, g) =>
                    {
                        p.Categoria = g;
                        c.Pedido = p;

                        return c;
                    }, _parameters as object).ToList();
            }

            return cotacoes;
        }
        

        public List<Cotacao> GetIdCotacoesPedidos(string idsPedidos, int idVendedor)
        {
            List<Cotacao> cotacoes;

            Query = @"
SELECT Id, 
       FormaPagamento, 
       DataCadastro, 
       ValorTotal, 
       Status, 
       IdVendedor, 
       IdPedido, 
       Observacao,
       CotacaoEscolhida       
FROM Cotacoes  
WHERE IdPedido IN (" + idsPedidos + ") AND IdVendedor = @IdVendedor";

            //_parameters.IdsPedidos = idsPedidos;
            _parameters.IdVendedor = idVendedor;

            using (_connection = new SqlConnection(_connectionString))
            {
                cotacoes = _connection.Query<Cotacao>(Query, _parameters as object).ToList();
            }

            return cotacoes;
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  c.Id, 
        c.FormaPagamento, 
        c.DataCadastro, 
        c.ValorTotal, 
        c.Status, 
        c.IdVendedor, 
        c.IdPedido, 
        c.Observacao,
        c.CotacaoEscolhida,
        i.Id Id,
        i.ValorUnitario,
        i.ValorTotal,
        i.IdUnidade,
        i.DataCadastro,
        i.IdItem
FROM Cotacoes c
INNER JOIN ItensCotacoes i
ON i.IdCotacao = c.Id";
        }

        #endregion

        #region Métodos Privados

        private void InitializeQueryFiltrarCotacoes()
        {
            Query = @"
SELECT  c.Id,
        c.DataCadastro,
        p.Id, 
        p.Validade, 
        p.DataCadastro, 
        p.IdGrupoPrincipal IdCategoriaPrincipal,
        g.Id,
        g.Nome        
FROM Cotacoes c
INNER JOIN Pedidos p
ON p.Id = c.IdPedido
INNER JOIN Compradores co
ON co.Id = p.IdComprador
INNER JOIN Cidades ci
ON ci.Id = co.IdCidade
INNER JOIN Grupos g
ON p.IdGrupoPrincipal = g.Id
INNER JOIN SubGrupos sg
ON g.Id = sg.IdGrupo";
        }

        private void InitializeQueryTodasCotacoes()
        {
            Query = @"
SELECT  c.Id,
        c.IdPedido, 
        p.Id,
        p.Validade,
        p.IdGrupoPrincipal IdCategoriaPrincipal
FROM Cotacoes c
LEFT OUTER JOIN Pedidos p
ON p.Id = c.IdPedido";
        }

        private void InitializeQueryCotacoesCategoria()
        {
            Query = @"
SELECT  c.Id,
        c.IdPedido,
        p.Id, 
        p.Validade,
        p.IdGrupoPrincipal IdCategoriaPrincipal,
        g.Id,
        g.Imagem
FROM Cotacoes c
INNER JOIN Pedidos p
ON p.Id = c.IdPedido
INNER JOIN ItensPedidos i
ON i.IdPedido = p.Id
INNER JOIN Grupos g
ON g.Id = p.IdGrupoPrincipal
INNER JOIN SubGrupos s
ON s.IdGrupo = g.Id
WHERE g.Id = @IdCategoria and i.IdSubGrupo = @IdSubCategoria and p.SituacaoPedido = @SituacaoPedido  
GROUP BY c.Id, c.IdPedido, p.Id, p.Validade, p.IdGrupoPrincipal, g.Id, g.Imagem";
        }

        private void SetWhereClauses(CotacaoFilter filter)
        {
            if (filter != null)
            {
                if (filter.IdVendedor.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"c.IdVendedor = @IdVendedor";

                    _parameters.IdVendedor = filter.IdVendedor.Value;
                }

                if (filter.CodigoArea.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"ci.IdRegiao = @IdRegiao";

                    _parameters.IdRegiao = filter.CodigoArea.Value;
                }

                if (filter.IdCidade.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"ci.Id = @IdCidade";

                    _parameters.IdCidade = filter.IdCidade.Value;
                }

                if (filter.IdGrupo.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.IdGrupoPrincipal = @IdGrupo";

                    _parameters.IdGrupo = filter.IdGrupo.Value;
                }

                if (filter.IdSubGrupo.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"sg.Id = @IdSubGrupo";

                    _parameters.IdSubGrupo = filter.IdSubGrupo.Value;
                }

                if (filter.Status.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.Status = @Status";

                    _parameters.Status = filter.Status.Value;
                }

                if (filter.SituacaoPedido.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.SituacaoPedido = @SituacaoPedido";

                    _parameters.SituacaoPedido = filter.SituacaoPedido.Value;
                }


                if (filter.DataLimiteProposta.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.Validade > @DataLimiteProposta";

                    _parameters.DataLimiteProposta = filter.DataLimiteProposta.Value.AddDays(-7);
                }

                if (!string.IsNullOrEmpty(filter.DescricaoCategoria))
                {
                    Query = JoinPredicate(Query);
                    Query += @"g.Nome LIKE '%' + @DescricaoCategoria + '%'";

                    _parameters.DescricaoCategoria = filter.DescricaoCategoria;
                }
            }
        }

        private string JoinPredicate(string query)
        {
            if (!query.Contains("WHERE"))
            {
                query += @" WHERE ";
            }
            else
            {
                query += @" AND ";
            }

            return query;
        }

        private void SetParameters(Cotacao entity)
        {
            _parameters.DataCadastro = entity.DataCadastro;
            _parameters.FormaPagamento = entity.FormaPagamento;
            _parameters.ValorTotal = entity.ValorTotal;
            _parameters.Status = entity.Status;
            _parameters.IdVendedor = entity.IdVendedor;
            _parameters.IdPedido = entity.IdPedido;
            _parameters.Observacao = entity.Observacao;
            _parameters.CotacaoEscolhida = entity.CotacaoEscolhida;
            _parameters.SituacaoPedido = entity.SituacaoPedido;
        }

        #endregion
    }
}
