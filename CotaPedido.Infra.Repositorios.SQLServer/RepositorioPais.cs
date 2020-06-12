using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using Dapper;
namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioPais : SqlRepositoryBase<Pais>, IRepositorioPais
    {
        public RepositorioPais()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;
        }

        public override int Delete(Pais entity)
        {
            throw new System.NotImplementedException();
        }

        public override Pais Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public override List<Pais> GetAll()
        {
            InitializeQuery();
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<Pais>(Query + " ORDER BY Nome").ToList();
            }
        }

        public override List<Pais> GetList(Dictionary<string, object> filters)
        {
            throw new System.NotImplementedException();
        }

        public override List<Pais> GetList(object filter)
        {
            throw new System.NotImplementedException();
        }

        public override int Insert(Pais entity)
        {
            throw new System.NotImplementedException();
        }

        public override int Update(Pais entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void InitializeQuery()
        {
            Query = @"
                SELECT  Id, 
                        Nome, 
                        DDI
                FROM Pais ";
        }
    }
}
