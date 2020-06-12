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
    public class RepositorioUnidade : SqlRepositoryBase<Unidade>, IRepositorioUnidade
    {
        #region Construtores

        public RepositorioUnidade()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Unidade entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Unidade (Descricao) VALUES (@Descricao) ";

            //_parameters.NotaFiscal = notaFiscalEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Unidade entity)
        {
            var retorno = -1;

            Query = @"UPDATE Unidade SET Descricao = @Descricao WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Descricao", unidade.Descricao);
            //cmd.Parameters.AddWithValue("@Id", unidade.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Unidade entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Descricao WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Id", unidade.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Unidade Get(int id)
        {
            Unidade unidade;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                unidade = _connection.Query<Unidade>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return unidade;
        }

        public override List<Unidade> GetAll()
        {
            List<Unidade> unidade;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                unidade = _connection.Query<Unidade>(Query)
                    .ToList();
            }

            return unidade;
        }

        public override List<Unidade> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Unidade> GetList(object filter)
        {
            string sql = "SELECT * FROM [Unidades] WHERE Descricao IN @Descricoes ORDER BY Descricao";
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<Unidade>(sql, filter).ToList();
            }
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        Descricao 
FROM Unidades";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: Descricao, Id
        }

        #endregion
    }
}
