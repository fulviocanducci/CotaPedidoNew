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
    public class RepositorioAnexoPedido : SqlRepositoryBase<AnexoPedido>, IRepositorioAnexoPedido
    {
        #region Construtores

        public RepositorioAnexoPedido()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(AnexoPedido entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO AnexosPedidos (Arquivo, IdPedido)
                          Values (@Arquivo, @IdPedido) ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Arquivo", anexoPedido.Arquivo);
            //cmd.Parameters.AddWithValue("@IdPedido", anexoPedido.IdPedido);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(AnexoPedido entity)
        {
            var retorno = -1;

            Query = @"UPDATE AnexosPedidos SET Arquivo = @Arquivo, IdPedido = @IdPedido 
                         WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Arquivo", anexoPedido.Arquivo);
            //cmd.Parameters.AddWithValue("@IdItem", anexoPedido.IdPedido);
            //cmd.Parameters.AddWithValue("@Id", anexoPedido.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(AnexoPedido entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM AnexosPedidos WHERE ID = @ID";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override AnexoPedido Get(int id)
        {
            AnexoPedido AnexoPedido;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                AnexoPedido = _connection.Query<AnexoPedido>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return AnexoPedido;
        }

        public override List<AnexoPedido> GetAll()
        {
            List<AnexoPedido> anexos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<AnexoPedido>(Query)
                    .ToList();
            }

            return anexos;
        }

        public override List<AnexoPedido> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<AnexoPedido> GetList(object filter)
        {
            List<AnexoPedido> anexos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<AnexoPedido>(Query)
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
        IdPedido 
FROM ArquivosPedidos ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id, IdPedido
        }

        #endregion
    }
}
