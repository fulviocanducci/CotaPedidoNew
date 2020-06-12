using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Text;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioAssinatura : SqlRepositoryBase<Assinatura>, IRepositorioAssinatura
    {
        #region Construtores

        public RepositorioAssinatura()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(Assinatura entity)
        {
            int retorno = -1;

            Query = @"INSERT INTO Assinaturas (Status, ChavePagSeguro, IdVendedor, IdMensalidade, DataFinal, Valor, FormaPagamento)
                           VALUES (@Status, @ChavePagSeguro, @IdVendedor, @IdMensalidade, @DataFinal, @Valor, @FormaPagamento); SELECT SCOPE_IDENTITY();";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Status", assinatura.Status);
            //cmd.Parameters.AddWithValue("@ChavePagSeguro", assinatura.ChavePagSeguro);
            //cmd.Parameters.AddWithValue("@IdVendedor", assinatura.IdVendedor);
            //cmd.Parameters.AddWithValue("@IdMensalidade", assinatura.IdMensalidade);
            //cmd.Parameters.AddWithValue("@DataFinal", assinatura.DataFinal);
            //cmd.Parameters.AddWithValue("@Valor", assinatura.Valor);
            //cmd.Parameters.AddWithValue("@FormaPagmento", assinatura.FormaPagamento);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, entity).SingleOrDefault();
                entity.Id = retorno;
            }            
            return retorno;
        }

        
        public int AtualizaStatusPagamento(Assinatura entity)
        {
            var retorno = -1;

            Query = @"UPDATE Assinaturas SET Status = '1' , ChavePagSeguro = @ChavePagSeguro   WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Status", assinatura.Status);
            //cmd.Parameters.AddWithValue("@ChavePagSeguro", assinatura.ChavePagSeguro);
            //cmd.Parameters.AddWithValue("@IdVendedor", assinatura.IdVendedor);
            //cmd.Parameters.AddWithValue("@IdMensalidade", assinatura.IdMensalidade);
            //cmd.Parameters.AddWithValue("@DataFinal", assinatura.DataFinal);
            //cmd.Parameters.AddWithValue("@Valor", assinatura.Valor);
            //cmd.Parameters.AddWithValue("@FormaPagmento", assinatura.FormaPagamento);
            //cmd.Parameters.AddWithValue("@Id", assinatura.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, entity).SingleOrDefault();
                //entity.Id = retorno;
            }

           

            return retorno;
        }


        public override int Update(Assinatura entity)
        {
            var retorno = -1;

            Query = @"UPDATE Assinaturas SET Status = @Status , ChavePagSeguro = @ChavePagSeguro, IdVendedor = @IdVendedor , IdMensalidade = @IdMensalidade, DataFinal = @DataFinal, Valor = @Valor, FormaPagmento = @FormaPagmento
                       WHERE Id = @Id ";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //cmd.Parameters.AddWithValue("@Status", assinatura.Status);
            //cmd.Parameters.AddWithValue("@ChavePagSeguro", assinatura.ChavePagSeguro);
            //cmd.Parameters.AddWithValue("@IdVendedor", assinatura.IdVendedor);
            //cmd.Parameters.AddWithValue("@IdMensalidade", assinatura.IdMensalidade);
            //cmd.Parameters.AddWithValue("@DataFinal", assinatura.DataFinal);
            //cmd.Parameters.AddWithValue("@Valor", assinatura.Valor);
            //cmd.Parameters.AddWithValue("@FormaPagmento", assinatura.FormaPagamento);
            //cmd.Parameters.AddWithValue("@Id", assinatura.Id);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Assinatura entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Assinatures WHERE ID = @ID";

            //_parameters.NotaFiscal = notaFiscalEntrada;
            //_parameters.Serie = serieNotaFiscalEntrada;
            //_parameters.IdRomaneio = idRomaneioEntrada;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Assinatura Get(int id)
        {
            Assinatura Assinatura;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                Assinatura = _connection.Query<Assinatura>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return Assinatura;
        }

        public override List<Assinatura> GetAll()
        {
            List<Assinatura> anexos;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<Assinatura>(Query)
                    .ToList();
            }

            return anexos;
        }

        public bool GetVendedorIdIsAssinante(int id)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" SELECT COUNT(*) as quantidade FROM Assinaturas ");
            SQL.Append(" WHERE (IdVendedor=@IdVendedor) ");
            SQL.Append(" AND (GETDATE() BETWEEN DataCadastro AND DataFinal) ");
            SQL.Append(" AND Status = '3' ");
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.ExecuteScalar<int>(SQL.ToString(), new { IdVendedor = id }) > 0;
            }
        }
        public IEnumerable<AssinaturaViewModel> GetVendedorId(int id)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" SELECT Assinaturas.Id, Assinaturas.DataCadastro, Convert(int, Assinaturas.Status) as Status, Assinaturas.ChavePagSeguro,Assinaturas.IdVendedor,Assinaturas.IdMensalidade, ");
            SQL.Append(" Assinaturas.DataFinal, Assinaturas.Valor, Mensalidades.Nome as MensalidadeName, Vendedores.RazaoSocial as VendedorName, ");
            SQL.Append(" Assinaturas.FormaPagamento FROM Assinaturas ");
            SQL.Append(" INNER JOIN Vendedores ON Vendedores.ID = Assinaturas.IdVendedor");
            SQL.Append(" INNER JOIN Mensalidades ON Mensalidades.ID = Assinaturas.IdMensalidade");
            SQL.Append(" WHERE Assinaturas.IdVendedor=@IdVendedor ");
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection
                    .Query<AssinaturaViewModel>(SQL.ToString(), new { IdVendedor = id })
                    .AsEnumerable();
            }
        }

        public override List<Assinatura> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Assinatura> GetList(object filter)
        {
            List<Assinatura> anexos;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                anexos = _connection.Query<Assinatura>(Query)
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
        DataCadastro, 
        Status, 
        ChavePagSeguro, 
        IdVendedor, 
        IdMensalidade, 
        DataFinal, 
        Valor, 
        FormaPagamento 
FROM Assinaturas ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id
        }

        #endregion
    }
}
