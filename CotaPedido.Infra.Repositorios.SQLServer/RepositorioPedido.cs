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
    public class RepositorioPedido : SqlRepositoryBase<Pedido>, IRepositorioPedido
    {
        #region Construtores

        public RepositorioPedido()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Pedido entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Pedidos (Validade, Status, IdComprador, Observacao, SituacaoPedido, IdGrupoPrincipal, DataCadastro) 
                           VALUES (@Validade, @Status, @IdComprador, @Observacao, @SituacaoPedido, @IdGrupoPrincipal, GetDate()) SELECT CAST(SCOPE_IDENTITY() as int)";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Pedido entity)
        {
            var retorno = -1;

            Query = @"UPDATE Pedidos SET Validade = @Validade, Status = @Status, IdComprador = @IdComprador, Observacao = @Observacao, SituacaoPedido = @SituacaoPedido, IdGrupoPrincipal = @IdGrupoPrincipal  WHERE Id = @Id ";

            SetParameters(entity);
            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Pedido entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Pedidos WHERE Id = @Id";

            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Pedido Get(int id)
        {
            Pedido Pedido;

            InitializeQueryGetPedido();

            Query = JoinPredicate(Query);
            Query += @"p.Id = @Id";

            _parameters.Id = id;

            using (_connection = new SqlConnection(_connectionString))
            {
                Pedido = _connection.Query<Pedido, Grupo, Cidade, Pedido>(Query, (p, g, c) =>
                 {
                     p.Categoria = g;
                     p.NomeCidade = c.Nome;

                     return p;
                 }, _parameters as object).FirstOrDefault();
            }

            return Pedido;
        }

        public override List<Pedido> GetAll()
        {
            List<Pedido> pedidos;

            InitializeQuery();

            Query += @" GROUP BY p.Id, p.Validade, p.[Status], p.DataCadastro, p.IdComprador, CONVERT(nvarchar(max), p.Observacao), p.SituacaoPedido,
                                 p.IdGrupoPrincipal, g.Id, g.Nome";

            using (_connection = new SqlConnection(_connectionString))
            {
                pedidos = _connection.Query<Pedido>(Query)
                    .ToList();
            }

            return pedidos;
        }

        public override List<Pedido> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Pedido> GetList(object filter)
        {
            List<Pedido> pedidos;

            InitializeQuery();

            SetWhereClauses(filter);

            Query += @" GROUP BY p.Id, p.Validade, p.[Status], p.DataCadastro, p.IdComprador, CONVERT(nvarchar(max), p.Observacao), p.SituacaoPedido,
                                 p.IdGrupoPrincipal, g.Id, g.Nome";

            using (_connection = new SqlConnection(_connectionString))
            {
                pedidos = _connection.Query<Pedido>(Query)
                    .ToList();
            }

            return pedidos;
        }

        public List<Pedido> GetList(PedidoFilter filter)
        {
            List<Pedido> pedidos;

            InitializeQuery();

            SetWhereClauses(filter);

            Query += @" GROUP BY p.Id, p.Validade, p.[Status], p.DataCadastro, p.IdComprador, CONVERT(nvarchar(max), p.Observacao), p.SituacaoPedido,
                                 p.IdGrupoPrincipal, g.Id, g.Nome, ci.Id, ci.Nome";

            using (_connection = new SqlConnection(_connectionString))
            {
                pedidos = _connection.Query<Pedido, Grupo, Pedido>(Query,
                    (p, g) =>
                    {
                        p.Categoria = g;

                        return p;
                    }, _parameters as object).ToList();
            }

            return pedidos;
        }

        public List<ItemCotacao> GetCotacoesPedido(int id, int quantidade)
        {
            List<ItemCotacao> cotacoesPedido = new List<ItemCotacao>();
            DataSet ds = new DataSet();

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("LISTA_COTACOES", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idPedido", id);
                command.Parameters.AddWithValue("@quantidade_cotacao", quantidade);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;

                da.Fill(ds, "ItensCotacoesPedido");

                foreach (DataRow row in ds.Tables["ItensCotacoesPedido"].Rows)
                {
                    for (int i = 4; i < row.ItemArray.Length; i +=3)
                    {
                        var itemCotacao = new ItemCotacao
                        {
                            IdItem = Convert.ToInt32(row.ItemArray[0]),
                            Quantidade = Convert.ToDecimal(row.ItemArray[3]),
                            IdCotacao = Convert.ToInt32(row.ItemArray[i]),
                            ValorTotal = Convert.ToDecimal(row.ItemArray[i + 1]),
                            ValorUnitario = Convert.ToDecimal(row.ItemArray[i + 2])
                        };

                        cotacoesPedido.Add(itemCotacao);
                    }
                }
            }

            return cotacoesPedido;
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  p.Id, 
        p.Validade, 
        p.Status, 
        p.DataCadastro, 
        p.IdComprador,
        CONVERT(nvarchar(max), p.Observacao) as Observacao,
        p.SituacaoPedido,
        p.IdGrupoPrincipal IdCategoriaPrincipal,
        ci.Id as IdCidade,        
        ci.Nome as NomeCidade,
        Count(distinct c.Id) NumeroCotacoes,
        g.Id Id,
        g.Nome        
FROM Pedidos p
INNER JOIN Compradores co
ON co.Id = p.IdComprador
INNER JOIN Cidades ci
ON ci.Id = co.IdCidade
INNER JOIN Grupos g
ON p.IdGrupoPrincipal = g.Id
INNER JOIN SubGrupos sg
ON g.Id = sg.IdGrupo
LEFT JOIN Cotacoes c
ON p.Id = c.IdPedido";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdComprador, Id
        }

        #endregion

        #region Métodos Privados

        private void InitializeQueryGetPedido()
        {
            Query = @"
SELECT  p.Id, 
        p.Validade, 
        p.Status, 
        p.DataCadastro, 
        p.IdComprador,        
        CONVERT(nvarchar(max), p.Observacao) as Observacao ,
        p.SituacaoPedido,
        p.IdGrupoPrincipal IdCategoriaPrincipal,
        g.Id Id,
        g.Nome,
		ci.Id,
        ci.Nome,
        ci.CodIbge,
        ci.IdRegiao,
        ci.UF,
        ci.IdPais
FROM Pedidos p
INNER JOIN Grupos g ON p.IdGrupoPrincipal = g.Id
INNER JOIN Compradores c ON C.Id = p.IdComprador
INNER JOIN Cidades ci ON C.IdCidade = ci.Id";
        }

        private void SetWhereClauses(PedidoFilter filter)
        {
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Ids))
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.Id in";
                    Query += " (";
                    string[] items = filter.Ids.Split(',');                    
                    for (int i = 0; i < items.Length; i++)
                    {
                        string nameParam = "pIdIn" + i;
                        ((IDictionary<string, object>)_parameters)
                            .Add(nameParam, int.Parse(items[i]));
                        if (i > 0) Query += ",";
                        Query += $"@{nameParam}";
                    }
                    Query += ") ";     
                }
                if (filter.IdComprador.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.IdComprador = @IdComprador";

                    _parameters.IdComprador = filter.IdComprador.Value;
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

                if (filter.SituacaoPedido.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.SituacaoPedido = @SituacaoPedido";

                    _parameters.SituacaoPedido = filter.SituacaoPedido.Value;
                }

                if (filter.Status.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.Status = @Status";

                    _parameters.Status = filter.Status.Value;
                }

                if (filter.DataLimiteProposta.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"p.Validade < @DataLimiteProposta"; //alterei aqui para lista os pedidos em 18-11-2017

                    _parameters.DataLimiteProposta = filter.DataLimiteProposta.Value.AddDays(1);
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

        private void SetParameters(Pedido entity)
        {
            _parameters.Validade = entity.Validade;
            _parameters.Status = entity.Status;
            _parameters.IdComprador = entity.IdComprador;
            _parameters.Observacao = entity.Observacao;
            _parameters.SituacaoPedido = entity.SituacaoPedido;
            _parameters.IdGrupoPrincipal = entity.IdCategoriaPrincipal;
        }

        #endregion
    }
}
