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
    public class RepositorioGrupo : SqlRepositoryBase<Grupo>, IRepositorioGrupo
    {
        #region Construtores

        public RepositorioGrupo()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Grupo entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Grupo (Numero, OrdemExibicao, Nome)
                         VALUES (@Numero, @OrdemExibicao, @Nome) ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Nome", grupo.Nome);
            //cmd.Parameters.AddWithValue("@Numero", grupo.Nome);
            //cmd.Parameters.AddWithValue("@OrdemExibicao", grupo.OrdemExibicao);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Grupo entity)
        {
            var retorno = -1;

            Query = @"UPDATE Grupo SET Numero = @Numero, OrdemExibicao = @OrdemExibicao, Nome = @Nome  WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Nome", grupo.Nome);
            //cmd.Parameters.AddWithValue("@Numero", grupo.Nome);
            //cmd.Parameters.AddWithValue("@OrdemExibicao", grupo.OrdemExibicao);
            //cmd.Parameters.AddWithValue("@Id", grupo.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Grupo entity)
        {
            var retorno = -1;

            Query = @" DELETE FROM Grupo WHERE Id = @Id";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }
        public Grupo Get(string urlAmigavel)
        {
            Grupo Grupo;

            InitializeQuery();

            var QueryEnd = Query + " WHERE UrlAmigavel = @UrlAmigavel";
            
            using (_connection = new SqlConnection(_connectionString))
            {
                Grupo = _connection
                    .Query<Grupo>(QueryEnd, new { UrlAmigavel = urlAmigavel })
                    .FirstOrDefault();
            }

            return Grupo;
        }

        public override Grupo Get(int id)
        {
            Grupo Grupo;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                Grupo = _connection.Query<Grupo>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return Grupo;
        }

        public override List<Grupo> GetAll()
        {
            List<Grupo> grupos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                grupos = _connection.Query<Grupo>(Query)
                    .ToList();
            }

            return grupos;
        }

        public override List<Grupo> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Grupo> GetList(object filter)
        {
            List<Grupo> grupos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                grupos = _connection.Query<Grupo>(Query)
                    .ToList();
            }

            return grupos;
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        Numero, 
        OrdemExibicao, 
        Nome,
        Imagem,
        UrlAmigavel
FROM Grupos ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id, Nome
        }

        

        #endregion
    }
}
