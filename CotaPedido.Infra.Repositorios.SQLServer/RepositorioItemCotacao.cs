using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Collections;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioItemCotacao : SqlRepositoryBase<ItemCotacao>, IRepositorioItemCotacao
    {
        #region Construtores

        public RepositorioItemCotacao()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(ItemCotacao entity)
        {
            var retorno = -1;

            Query = @" INSERT INTO ItensCotacoes (ValorUnitario, ValorTotal, Observacao, IdUnidade, IdItem, IdCotacao, DataCadastro, Embalagem) 
                          VALUES (@ValorUnitario, @ValorTotal, @Observacao, @IdUnidade, @IdItem, @IdCotacao, @DataCadastro, @Embalagem) ";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(ItemCotacao entity)
        {
            var retorno = -1;

            Query = @"UPDATE ItensCotacoes SET ValorUnitario = @ValorUnitario , ValorTotal = @ValorTotal , Observacao = @Observacao, IdUnidade = @IdUnidade, IdItem = @IdItem, IdCotacao = @IdCotacao, DataCadastro = @DataCadastro, Embalagem = @Embalagem  WHERE Id = @Id ";

            SetParameters(entity);
            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public IEnumerable<int> UpdateFromExcel(IEnumerable items)
        {
            string SQL = "UPDATE ItensCotacoes SET ValorUnitario = @ValorUnitario, ValorTotal = @ValorTotal WHERE Id = @Id AND IdItem = @IdItem";
            using (_connection = new SqlConnection(_connectionString))
            {
                foreach (var item in items)
                {
                    yield return _connection.Execute(SQL, item);
                }
            }
        }

        public int UpdateFromExcel(int id, int item, decimal valorUnitario, decimal valorTotal)
        {

            string SQL = "UPDATE ItensCotacoes SET ValorUnitario = @ValorUnitario , ValorTotal = @ValorTotal WHERE Id = @Id AND IdItem = @IdItem";
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Execute(SQL, new { ValorUnitario = valorUnitario, ValorTotal = valorTotal, Id = id, IdItem = item });
            }
        }


        public override int Delete(ItemCotacao entity)
        {
            var retorno = -1;

            Query = @" DELETE FROM ItensCotacoes WHERE Id = @Id ";

            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override ItemCotacao Get(int id)
        {
            ItemCotacao ItemCotacao;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                ItemCotacao = _connection.Query<ItemCotacao>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return ItemCotacao;
        }

        public override List<ItemCotacao> GetAll()
        {
            List<ItemCotacao> itensCotacoes;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                itensCotacoes = _connection.Query<ItemCotacao>(Query)
                    .ToList();
            }

            return itensCotacoes;
        }

        public override List<ItemCotacao> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<ItemCotacao> GetList(object filter)
        {
            List<ItemCotacao> itensCotacoes;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                itensCotacoes = _connection.Query<ItemCotacao>(Query)
                    .ToList();
            }

            return itensCotacoes;
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        ValorUnitario,
        ValorTotal, 
        Observacao, 
        IdUnidade, 
        IdItem, 
        IdCotacao, 
        DataCadastro,
        Embalagem 
FROM ItensCotacoes";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id
        }

        #endregion

        #region Métodos Privados

        private void SetParameters(ItemCotacao entity)
        {
            _parameters.ValorUnitario = entity.ValorUnitario;
            _parameters.ValorTotal = entity.ValorTotal;
            _parameters.Observacao = entity.Observacao;
            _parameters.IdUnidade = entity.IdUnidade;
            _parameters.IdItem = entity.IdItem;
            _parameters.IdCotacao = entity.IdCotacao;
            _parameters.DataCadastro = entity.DataCadastro;
            _parameters.Embalagem = entity.Embalagem;
        }

        

        #endregion
    }
}
