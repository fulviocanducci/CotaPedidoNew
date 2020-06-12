using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using Dapper;
using System.Linq;
namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioAviso : SqlRepositoryBase<Aviso>, IRepositorioAviso
    {
        public RepositorioAviso()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;
        }

        public override int Delete(Aviso entity)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var param = new { AvisoId = entity.AvisoId };
                return _connection.Execute("DELETE FROM Aviso WHERE AvisoId = @AvisoId", param);
            }
        }

        public override Aviso Get(int id)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var param = new { AvisoId = id };
                return _connection.Query<Aviso>("SELECT * FROM Aviso WHERE AvisoId = @AvisoId", param)
                    .FirstOrDefault();
            }
        }

        public override List<Aviso> GetAll()
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<Aviso>("SELECT * FROM Aviso ORDER BY AvisoId ")
                    .ToList();                    
            }
        }
        
        public IList<AvisoViewModel> GetAvisosFromVendedor(int? id)
        {
            IList<AvisoViewModel> listaAvisoViewModel = null;
            using (_connection = new SqlConnection(_connectionString))
            {
                var param = new { VendedorId = id };
                Query = " SELECT AvisoId,AvisoTipo,AvisoPais,AvisoRegiao,AvisoEstado, ";
                Query += " AvisoCidade,AvisoGrupo,AvisoSubGrupo,VendedorId,Grupos.Nome as GrupoNome,SubGrupos.Nome as SubGrupoNome";
                Query += " FROM Aviso INNER JOIN Grupos ON Grupos.Id = Aviso.AvisoGrupo ";
                Query += " LEFT JOIN SubGrupos ON SubGrupos.Id = Aviso.AvisoSubGrupo ";
                Query += " WHERE VendedorId=@VendedorId ";

                listaAvisoViewModel = _connection.Query<AvisoViewModel>(Query, param).ToList(); 
                
                if (listaAvisoViewModel.Any(x => x.AvisoTipo == 1)) // Pais
                {
                    var filter = listaAvisoViewModel.Where(x => x.AvisoTipo == 1).ToList();                    
                    var items = _connection.Query<DefaultViewModel>("SELECT Id, Nome FROM Pais WHERE Pais.Id in @Ids", new { Ids = filter.Select(x => (int)x.AvisoPais).ToArray() });
                    filter.ToList().ForEach(x =>
                    {
                        x.Valor = items.Where(a => a.Id == x.AvisoPais).Select(s => s.Nome).FirstOrDefault();
                    });
                }
                if (listaAvisoViewModel.Any(x => x.AvisoTipo == 2))  //Estado
                {
                    var filter = listaAvisoViewModel.Where(x => x.AvisoTipo == 2).ToList();
                    var items = _connection.Query<DefaultViewModel>("SELECT Id, Nome FROM Estado WHERE Estado.Id in @Ids", new { Ids = filter.Select(x => (int)x.AvisoEstado).ToArray() });
                    filter.ToList().ForEach(x =>
                    {
                        x.Valor = items.Where(a => a.Id == x.AvisoEstado).Select(s => s.Nome).FirstOrDefault();
                    });
                }
                if (listaAvisoViewModel.Any(x => x.AvisoTipo == 3)) // Regiao
                {
                    var filter = listaAvisoViewModel.Where(x => x.AvisoTipo == 3).ToList();
                    var items = _connection.Query<DefaultViewModel>("SELECT Id, Nome FROM Regioes WHERE Regioes.Id in @Ids", new { Ids = filter.Select(x => (int)x.AvisoRegiao).ToArray() });
                    filter.ToList().ForEach(x =>
                    {
                        x.Valor = items.Where(a => a.Id == x.AvisoRegiao).Select(s => s.Nome).FirstOrDefault();
                    });
                }
                if (listaAvisoViewModel.Any(x => x.AvisoTipo == 4)) // Cidade
                {
                    var filter = listaAvisoViewModel.Where(x => x.AvisoTipo == 4).ToList();
                    var sql = @" SELECT Cidades.Id, CONCAT(Cidades.Nome,' - ',Estado.Sigla) as Nome FROM Cidades ";
                    sql += " INNER JOIN Estado ON Cidades.UF = Estado.Id ";
                    sql += " WHERE Cidades.Id in @Ids ORDER BY Cidades.Nome";
                    var items = _connection.Query<DefaultViewModel>(sql, new { Ids = filter.Select(x => (int)x.AvisoCidade).ToArray() });
                    filter.ToList().ForEach(x =>
                    {
                        x.Valor = items.Where(a => a.Id == x.AvisoCidade).Select(s => s.Nome).FirstOrDefault();
                    });
                }
            }
            return listaAvisoViewModel;
        }

        public override List<Aviso> GetList(Dictionary<string, object> filters)
        {
            throw new System.NotImplementedException();
        }

        public override List<Aviso> GetList(object filter)
        {
            throw new System.NotImplementedException();
        }

        public override int Insert(Aviso entity)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                Query = "INSERT INTO Aviso(AvisoTipo,AvisoPais,AvisoRegiao,AvisoEstado,AvisoCidade,AvisoGrupo,AvisoSubGrupo,VendedorId)";
                Query += "VALUES(@AvisoTipo,@AvisoPais,@AvisoRegiao,@AvisoEstado,@AvisoCidade,@AvisoGrupo,@AvisoSubGrupo,@VendedorId);";
                Query += "SELECT SCOPE_IDENTITY()";
                entity.AvisoId = _connection.ExecuteScalar<int>(Query, entity);                
            }
            return entity.AvisoId;
        }

        public override int Update(Aviso entity)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                Query = "UPDATE Aviso SET AvisoTipo=@AvisoTipo,AvisoPais=@AvisoPais,AvisoRegiao=@AvisoRegiao,AvisoEstado=@AvisoEstado,";
                Query += "AvisoCidade=@AvisoCidade,AvisoGrupo=@AvisoGrupo,AvisoSubGrupo=@AvisoSubGrupo,VendedorId=@VendedorId ";
                Query += "WHERE AvisoId=@AvisoId";
                return _connection.Execute(Query, entity);
            }
        }

        protected override void InitializeQuery()
        {
            Query = @"
            SELECT  AvisoId, 
                    AvisoTipo, 
                    AvisoPais, 
                    AvisoRegiao,
                    AvisoEstado,
                    AvisoCidade,
                    AvisoGrupo,
                    AvisoSubGrupo,
                    VendedorId
            FROM Aviso ";
        }
    }
}
