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
using CotaPedido.Entidades.Filter;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioCidade : SqlRepositoryBase<Cidade>, IRepositorioCidade
    {   
        #region Construtores

        public RepositorioCidade()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Cidade entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Cidades ( Nome, CodIbge, IdRegiao, UF, IdPais) " +
                         " VALUES ( @Nome, @CodIbge, @IdRegiao, @UF, @IdPais) ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Nome", cidade.Nome);
            //cmd.Parameters.AddWithValue("@CodIbge", cidade.CodIbge);
            //cmd.Parameters.AddWithValue("@IdRegiao", cidade.IdRegiao);
            //cmd.Parameters.AddWithValue("@IdUF", cidade.UF);
            //cmd.Parameters.AddWithValue("@IdPais", cidade.IdPais);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Cidade entity)
        {
            var retorno = -1;

            Query = @"UPDATE Cidades SET  Nome = @Nome, CodIbge = @CodIbge, IdRegiao = @IdRegiao, UF = @UF, IdPais = @IdPais
                          WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Nome", cidade.Nome);
            //cmd.Parameters.AddWithValue("@CodIbge", cidade.CodIbge);
            //cmd.Parameters.AddWithValue("@IdRegiao", cidade.IdRegiao);
            //cmd.Parameters.AddWithValue("@IdUF", cidade.UF);
            //cmd.Parameters.AddWithValue("@IdPais", cidade.IdPais);
            //cmd.Parameters.AddWithValue("@Id", cidade.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Cidade entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Cidades WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Cidade Get(int id)
        {
            Cidade Cidade;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                Cidade = _connection.Query<Cidade>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return Cidade;
        }

        public override  List<Cidade> GetAll()
        {
            List<Cidade> cidades;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {                
                cidades = _connection.Query<Cidade>(Query)
                    .ToList();
            }

            return cidades;
        }

        public override List<Cidade> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Cidade> GetList(object filter)
        {
            List<Cidade> cidades;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                cidades = _connection.Query<Cidade>(Query)
                    .ToList();
            }

            return cidades;
        }


        public static List<Cidade> GetListporUF(string UF)
        {
            var repositorio = new RepositorioCidade();
            List<Cidade> cidades = new List<Cidade>();
            cidades = repositorio.GetAll();                    
            return cidades.Where(x => x.UF == Convert.ToInt16(UF)).ToList();            
        }

        public static List<Cidade> GetListporRegiao(string id)
        {
            var repositorio = new RepositorioCidade();
            List<Cidade> cidades = new List<Cidade>();
            cidades = repositorio.GetAll();
            return cidades.Where(x => x.IdRegiao == Convert.ToInt16(id)).ToList();
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        Nome, 
        CodIbge, 
        IdRegiao,
        UF,
        IdPais 
FROM Cidades order by uf, nome";
        }

        protected  void SetWhereClauses(CidadeFilter filter)
        {
            //FILTROS: IdItem, Id, UF, IdPais
            if (filter != null)
            {
                if (filter.IdUF.HasValue)
                {
                    
                    Query += @"ci.UF = @IdUF";

                    _parameters.UF = filter.IdUF.Value;
                }
            }
        }

        public IEnumerable GetAllWithUf()
        {
            Query = @" SELECT Cidades.Id, CONCAT(Cidades.Nome,' - ',Estado.Sigla) as Nome FROM Cidades ";
            Query += " INNER JOIN Estado ON Cidades.UF = Estado.Id ORDER BY Cidades.Nome ";
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<DefaultViewModel>(Query).ToList();
            }
        }

        #endregion
    }
}
