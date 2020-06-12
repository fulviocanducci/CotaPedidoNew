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
    public class RepositorioVendedorRegiao : SqlRepositoryBase<VendedorRegiao>, IRepositorioVendedorRegiao
    {
        #region Construtores

        public RepositorioVendedorRegiao()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion
        
        #region Métodos Publicos

        public override int Insert(VendedorRegiao entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO VendedoresRegioes (IdVendedor, IdRegiao) VALUES (@IdVendedor, @IdRegiao)";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@IdVendedor", vendedorRegiao.IdVendedor);
            //cmd.Parameters.AddWithValue("@IdRegiao", vendedorRegiao.IdRegiao);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(VendedorRegiao entity)
        {
            var retorno = -1;

            Query = @"UPDATE VendedoresRegioes SET IdVendedor = @IdVendedor, IdRegiao = @IdRegiao 
                       WHERE IdVendedor = @IdVendedor";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@IdVendedor", vendedorRegiao.IdRegiao);
            //cmd.Parameters.AddWithValue("@IdRegiao", vendedorRegiao.IdVendedor);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(VendedorRegiao entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM VendedoresRegioes WHERE IdVendedor = @IdVendedor AND IdRegiao = @IdRegiao ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@IdVendedor", vendedorRegiao.IdVendedor);
            //cmd.Parameters.AddWithValue("@IdRegiao", vendedorRegiao.IdRegiao);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override VendedorRegiao Get(int id)
        {
            VendedorRegiao vendedorRegiao;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                vendedorRegiao = _connection.Query<VendedorRegiao>(Query)
                .Where(a => a.IdVendedor == id)
                .FirstOrDefault();
            }

            return vendedorRegiao;
        }

        public override List<VendedorRegiao> GetAll()
        {
            List<VendedorRegiao> anexos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<VendedorRegiao>(Query)
                    .ToList();
            }

            return anexos;
        }

        public override List<VendedorRegiao> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<VendedorRegiao> GetList(object filter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  IdVendedor, 
        IdRegiao 
FROM VendedoresRegioes";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdVendedor, Id
        }

        #endregion
    }
}
