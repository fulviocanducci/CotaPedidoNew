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
    public class RepositorioItemPedido : SqlRepositoryBase<ItemPedido>, IRepositorioItemPedido
    {
        #region Construtores

        public RepositorioItemPedido()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(ItemPedido entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO ItensPedidos (IdPedido, IdUnidade, IdEmbalagem, Numero, Descricao, ValorUnitario, Quantidade, Observacao, ValorTotal, IdGrupo, IdSubGrupo)
                           VALUES (@IdPedido, @IdUnidade, @IdEmbalagem, @Numero, @Descricao, @ValorUnitario, @Quantidade, @Observacao, @ValorTotal, @IdGrupo, @IdSubGrupo) ";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public IEnumerable<int> Insert(IEnumerable<ItemPedido> entities)
        {   
            Query = @"INSERT INTO ItensPedidos (IdPedido, IdUnidade, IdEmbalagem, Numero, Descricao, ValorUnitario, Quantidade, Observacao, ValorTotal, IdGrupo, IdSubGrupo)
                           VALUES (@IdPedido, @IdUnidade, @IdEmbalagem, @Numero, @Descricao, @ValorUnitario, @Quantidade, @Observacao, @ValorTotal, @IdGrupo, @IdSubGrupo) ";

            using (_connection = new SqlConnection(_connectionString))
            {
                foreach (ItemPedido entity in entities)
                {
                    SetParameters(entity);
                    yield return _connection.Execute(Query, _parameters as object);
                }
            }
        }

        public override int Update(ItemPedido entity)
        {
            var retorno = -1;

            Query = @"UPDATE ItensPedidos SET IdPedido = @IdPedido, IdUnidade = @IdUnidade, IdEmbalagem = @IdEmbalagem, Numero = @Numero, Descricao = @Descricao, ValorUnitario = @ValorUnitario, Quantidade = @Quantidade, Observacao = @Observacao, ValorTotal = @ValorTotal, IdGrupo = @IdGrupo, IdSubGrupo = @IdSubGrupo
                       WHERE Id = @Id ";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(ItemPedido entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM ItensPedidos WHERE Id = @Id";

            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override ItemPedido Get(int id)
        {
            ItemPedido ItemPedido;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                ItemPedido = _connection.Query<ItemPedido>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return ItemPedido;
        }

        public override List<ItemPedido> GetAll()
        {
            List<ItemPedido> itensPedidos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                itensPedidos = _connection.Query<ItemPedido>(Query)
                    .ToList();
            }

            return itensPedidos;
        }

        public override List<ItemPedido> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<ItemPedido> GetList(object filter)
        {
            List<ItemPedido> itensPedidos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                itensPedidos = _connection.Query<ItemPedido>(Query)
                    .ToList();
            }

            return itensPedidos;
        }

        public List<ItemPedido> GetList(ItemPedidoFilter filter)
        {
            List<ItemPedido> itensPedidos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                itensPedidos = _connection.Query<ItemPedido, Grupo, SubGrupo, Unidade, ItemPedido>(Query, (i, g, sg, u) =>
                    {
                        i.Categoria = g;
                        i.SubCategoria = sg;
                        i.Unidade = u;

                        return i;
                    }, _parameters as object).ToList();
            }

            return itensPedidos;
        }

        #endregion


        #region Métodos Privados

        private void SetWhereClauses(ItemPedidoFilter filter)
        {
            if (filter != null)
            {
                if (filter.IdPedido.HasValue)
                {
                    Query = JoinPredicate(Query);
                    Query += @"i.IdPedido = @IdPedido";

                    _parameters.IdPedido = filter.IdPedido.Value;
                }

                if (!string.IsNullOrEmpty(filter.IdsPedidos))
                {
                    Query = JoinPredicate(Query);
                    Query += @"i.IdPedido IN (" + filter.IdsPedidos  + " )";

                    _parameters.IdsPedidos = filter.IdsPedidos;
                }

                if (!string.IsNullOrEmpty(filter.Descricao))
                {
                    Query = JoinPredicate(Query);
                    Query += @"i.Descricao LIKE @Descricao";
                    _parameters.Descricao = $"%{filter.Descricao}%";
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

        private void SetParameters(ItemPedido entity)
        {
            _parameters.IdPedido = entity.IdPedido;
            _parameters.IdUnidade = entity.IdUnidade;
            _parameters.IdEmbalagem = entity.IdEmbalagem;
            _parameters.Numero = entity.Numero;
            _parameters.Descricao = entity.Descricao;
            _parameters.ValorUnitario = entity.ValorUnitario;
            _parameters.Quantidade = entity.Quantidade;
            _parameters.Observacao = entity.Observacao;
            _parameters.ValorTotal = entity.ValorTotal;
            _parameters.IdGrupo = entity.IdCategoria;
            _parameters.IdSubGrupo = entity.IdSubCategoria;
            _parameters.ID = entity.Id;
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  i.Id, 
        i.IdPedido, 
        i.IdUnidade, 
        i.IdEmbalagem, 
        i.Numero, 
        i.Descricao, 
        i.ValorUnitario, 
        i.Quantidade, 
        i.Observacao, 
        i.ValorTotal,
        i.IdGrupo IdCategoria, 
        i.IdSubGrupo IdSubCategoria,
        g.Id Id,
        g.Nome,
        sg.Id Id,
        sg.Nome,
        u.Id,
        u.Descricao
FROM ItensPedidos i
INNER JOIN Grupos g
ON i.IdGrupo = g.Id
INNER JOIN SubGrupos sg
ON i.IdSubGrupo = sg.Id
LEFT OUTER JOIN Unidades u
ON i.IdUnidade = u.Id";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id, IdPedido
        }

        #endregion
    }
}
