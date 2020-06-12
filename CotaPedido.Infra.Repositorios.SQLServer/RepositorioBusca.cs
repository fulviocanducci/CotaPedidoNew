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
    public class RepositorioBusca : SqlRepositoryBase<Busca>, IRepositorioBusca
    {
        #region Construtores

        public RepositorioBusca()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Busca entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Buscas (BuscaTexto, IdVendedor, IdCidade, IdRegiao)
                           VALUES ( @BuscaTexto, @IdVendedor, @IdCidade, @IdRegiao) ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@BuscaTexto", busca.BuscaTexto);
            //cmd.Parameters.AddWithValue("@IdVendedor", busca.IdVendedor);
            //cmd.Parameters.AddWithValue("@IdCidade", busca.IdCidade);
            //cmd.Parameters.AddWithValue("@IdRegiao", busca.IdCidade);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Busca entity)
        {
            var retorno = -1;

            Query = @"UPDATE Buscas SET Id = @Id, BuscaTexto = @BuscaTexto, IdVendedor = @IdVendedor,  IdCidade = @IdCidade, IdRegiao = @IdRegiao
                       WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@BuscaTexto", busca.BuscaTexto);
            //cmd.Parameters.AddWithValue("@IdVendedor", busca.IdVendedor);
            //cmd.Parameters.AddWithValue("@IdCidade", busca.IdCidade);
            //cmd.Parameters.AddWithValue("@IdRegiao", busca.IdCidade);
            //cmd.Parameters.AddWithValue("@Id", busca.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Busca entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Buscas WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Id", busca.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Busca Get(int id)
        {
            Busca Busca;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                Busca = _connection.Query<Busca>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return Busca;
        }

        public override List<Busca> GetAll()
        {
            List<Busca> anexos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<Busca>(Query)
                    .ToList();
            }

            return anexos;
        }

        public override List<Busca> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Busca> GetList(object filter)
        {
            List<Busca> anexos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<Busca>(Query)
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
        BuscaTexto, 
        IdVendedor, 
        IdCidade, 
        IdRegiao  
FROM Buscas ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id, IdVendedor
        }

        #endregion
    }
}
