using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Collections;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioMensalidade : SqlRepositoryBase<Mensalidade>, IRepositorioMensalidade
    {
        #region Construtores

        public RepositorioMensalidade()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Mensalidade entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Mensalidades (Nome, QuantidadeDias, Status, Valor )
                         VALUES (@Nome, @QuantidadeDias, @Status, @Valor)  ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Nome", mensalidade.Nome);
            //cmd.Parameters.AddWithValue("@QuantidadeDias", mensalidade.QuantidadeDias);
            //cmd.Parameters.AddWithValue("@Status", mensalidade.Status);
            //cmd.Parameters.AddWithValue("@Valor", mensalidade.Valor);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Mensalidade entity)
        {
            var retorno = -1;

            Query = @"UPDATE Mensalidades SET Nome = @Nome, QuantidadeDias = @QuantidadeDias , Status = @Status, Valor = @Valor
                       WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Nome", mensalidade.Nome);
            //cmd.Parameters.AddWithValue("@QuantidadeDias", mensalidade.QuantidadeDias);
            //cmd.Parameters.AddWithValue("@Status", mensalidade.Status);
            //cmd.Parameters.AddWithValue("@Valor", mensalidade.Valor);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Mensalidade entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Mensalidades WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Id", mensalidade.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Mensalidade Get(int id)
        {
            Mensalidade Mensalidade;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                Mensalidade = _connection.Query<Mensalidade>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return Mensalidade;
        }

        public override List<Mensalidade> GetAll()
        {
            List<Mensalidade> anexos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<Mensalidade>(Query)
                    .ToList();
            }

            return anexos;
        }

        public override List<Mensalidade> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Mensalidade> GetList(object filter)
        {
            List<Mensalidade> anexos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<Mensalidade>(Query)
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
        Nome, 
        QuantidadeDias, 
        DataCadastro, 
        Status, 
        Valor  
FROM Mensalidades ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id, Nome
        }

        

        #endregion
    }
}
