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
    public class RepositorioSubGrupo : SqlRepositoryBase<SubGrupo>, IRepositorioSubGrupo
    {
        #region Construtores

        public RepositorioSubGrupo()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(SubGrupo entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO SUBGRUPOS (Nome, Numero, IdGrupo, OrdemExibicao) VALUES (@Nome, @Numero, @IdGrupo, @OrdemExibicao) SELECT CAST(SCOPE_IDENTITY() as int)";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(SubGrupo entity)
        {
            var retorno = -1;

            Query = @" UPDATE SubGrupos SET Nome = @Nome, Numero = @Numero, IdGrupo = @IdGrupo , OrdemExibicao = @OrdemExibicao WHERE Id = @Id";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(SubGrupo entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM SubGrupos WHERE Id = @Id";

            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public SubGrupo Get(string urlAmigavel)
        {
            SubGrupo subGrupo;

            InitializeQuery();

            var QueryEnd = Query + "WHERE UrlAmigavel=@UrlAmigavel";
            using (_connection = new SqlConnection(_connectionString))
            {
                subGrupo = _connection
                    .Query<SubGrupo>(QueryEnd, new { UrlAmigavel = urlAmigavel})
                    .FirstOrDefault();
            }

            return subGrupo;
        }
        public override SubGrupo Get(int id)
        {
            SubGrupo subGrupo;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                subGrupo = _connection.Query<SubGrupo>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return subGrupo;
        }

        public override List<SubGrupo> GetAll()
        {
            List<SubGrupo> subGrupos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                subGrupos = _connection.Query<SubGrupo>(Query)
                    .ToList();
            }

            return subGrupos;
        }

        public override List<SubGrupo> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<SubGrupo> GetList(object filter)
        {
            string sql = "SELECT * FROM [SubGrupos] WHERE Nome IN @Nomes ORDER BY Nome";
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<SubGrupo>(sql, filter).ToList();
            }
        }

        #endregion

        #region Métodos Privados

        private void SetParameters(SubGrupo entity)
        {
            _parameters.Nome = entity.Nome;
            _parameters.Numero = entity.Numero;
            _parameters.IdGrupo = entity.IdGrupo;
            _parameters.OrdemExibicao = entity.OrdemExibicao;
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        Nome, 
        Numero, 
        IdGrupo, 
        OrdemExibicao,
        UrlAmigavel
FROM SubGrupos ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: Nome, Id
        }

        public IEnumerable<SubGrupo> GetListOfGrupo(int idGrupo)
        {
            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<SubGrupo>(Query + " WHERE IdGrupo=@IdGrupo ", new { IdGrupo = idGrupo }).ToList();
            }
        }

        #endregion
    }
}
