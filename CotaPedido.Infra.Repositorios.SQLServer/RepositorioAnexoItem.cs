using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioAnexoItem : SqlRepositoryBase<AnexoItem>, IRepositorioAnexoItem
    {
        #region Construtores

        public RepositorioAnexoItem()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(AnexoItem entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO AnexoItens (Arquivo, IdItem) " +
                         " VALUES (@Arquivo, @IdItem) ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(AnexoItem entity)
        {
            var retorno = -1;

            Query = @"UPDATE AnexoItens SET Arqvuivo = @Arquivo, IdItem = @IdItem
                       WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(AnexoItem entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM AnexoItens WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override AnexoItem Get(int id)
        {
            AnexoItem anexoItem;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                anexoItem = _connection.Query<AnexoItem>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return anexoItem;
        }

        public override List<AnexoItem> GetAll()
        {
            List<AnexoItem> anexos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<AnexoItem>(Query)
                    .ToList();
            }

            return anexos;
        }

        public override List<AnexoItem> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<AnexoItem> GetList(object filter)
        {
            List<AnexoItem> anexos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<AnexoItem>(Query)
                    .ToList();
            }

            return anexos;
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        Arquivo, 
        IdItem 
FROM AnexoItens";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id
        }

        #endregion
    }
}
