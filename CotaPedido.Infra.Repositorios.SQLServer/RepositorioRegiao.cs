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
    public class RepositorioRegiao : SqlRepositoryBase<Regiao>, IRepositorioRegiao
    {
        #region Construtores

        public RepositorioRegiao()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Regiao entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Regioes (Nome, CodigoArea, IdUF)  VALUES (@Nome, @CodigoArea, @IdUF) ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Regiao entity)
        {
            var retorno = -1;

            Query = @"UPDATE SET Regioes SET  Nome = @Nome, CodigoArea = @CodigoArea, IdUF = @IdUF WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Nome", regiao.Nome);
            //cmd.Parameters.AddWithValue("@CodigoArea", regiao.CodigoArea);
            //cmd.Parameters.AddWithValue("@IdUF", regiao.IdUF);
            //cmd.Parameters.AddWithValue("@Id", regiao.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Regiao entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Regioes WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Id", regiao.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Regiao Get(int id)
        {
            Regiao regiao;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                regiao = _connection.Query<Regiao>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return regiao;
        }

        public override List<Regiao> GetAll()
        {
            List<Regiao> regioes;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                regioes = _connection.Query<Regiao>(Query)
                    .ToList();
            }

            return regioes;
        }

        public override List<Regiao> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Regiao> GetList(object filter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        Nome, 
        CodigoArea, 
        IdUF 
FROM Regioes ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id
        }

        #endregion
    }
}
