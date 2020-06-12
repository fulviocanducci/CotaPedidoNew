using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;
using CotaPedido.Infra.IRepositorios;
using Dapper;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioEstado : SqlRepositoryBase<Estado>, IRepositorioEstado
    {
        public RepositorioEstado()
            :base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;
        }

        public override int Delete(Estado entity)
        {
            throw new System.NotImplementedException();
        }

        public override Estado Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public override List<Estado> GetAll()
        {
            InitializeQuery();
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<Estado>(Query + " ORDER BY Sigla").ToList();
            }
        }

        public override List<Estado> GetList(Dictionary<string, object> filters)
        {
            throw new System.NotImplementedException();
        }

        public override List<Estado> GetList(object filter)
        {
            throw new System.NotImplementedException();
        }

        public override int Insert(Estado entity)
        {
            throw new System.NotImplementedException();
        }

        public override int Update(Estado entity)
        {
            throw new System.NotImplementedException();
        }

        public Estado GetByNome(UF uf)
        {
            InitializeQuery();
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection
                    .Query<Estado>(Query + " WHERE Sigla=@Sigla", new { Sigla = uf.ToString() })
                    .FirstOrDefault();
            }
        }

        protected override void InitializeQuery()
        {
            Query = @"
                SELECT  Id, 
                        Nome, 
                        Sigla, 
                        PaisId 
                FROM Estado ";
        }
    }
}
