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
    public class RepositorioVendedor : SqlRepositoryBase<Vendedor>, IRepositorioVendedor
    {
        #region Construtores

        public RepositorioVendedor()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }


        public bool GetEmailExists(string email)
        {
            bool retorno = false;
            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection
                    .Query<int>("SELECT count(email) FROM Vendedores WHERE email=@email", new { email })
                    .FirstOrDefault() > 0;
            }

            return retorno;
        }

        #endregion

        #region Métodos Publicos
        public int Insert(
            string email,
            string senha,
            string nomeFantasia,
            string razaoSocial,
            int idCidade,
            int idCidadeCobranca,
            DateTime dataCadastro,
            bool status = true)
        {
            var retorno = -1;
            Query = @"INSERT INTO Vendedores(Email, Senha, NomeFantasia, RazaoSocial, IdCidade, IdCidadeCobranca, DataCadastro, Status) ";
            Query += "VALUES(@email, @senha, @nomeFantasia, @razaoSocial, @idCidade, @idCidadeCobranca, @dataCadastro, @status)";
            using (_connection = new SqlConnection(_connectionString))
            {
                object parameters = new
                {
                    email,
                    senha,
                    nomeFantasia,
                    razaoSocial,
                    idCidade,
                    idCidadeCobranca,
                    dataCadastro,
                    status
                };
                retorno = _connection.Execute(Query, parameters);
            }

            return retorno;
        }
        public override int Insert(Vendedor entity)
        {
            var retorno = -1;

            Query = @" INSERT INTO Vendedores (RazaoSocial , NomeFantasia , CNPJ , Endereco , CEP , Telefone , Celular , Numero , Bairro , Complemento , DataCadastro , Email , Site , Senha , Status , IdCidade , IE , IM , EnderecoCobranca , 
                       CEPCobranca , TelefoneCobranca , CelularCobranca , NumeroCobranca , BairroCobranca , ComplementoCobranca , IdCidadeCobranca  , RG )
                       VALUES ( @RazaoSocial , @NomeFantasia , @CNPJ , @Endereco , @CEP , @Telefone , @Celular , @Numero , @Bairro , @Complemento , @DataCadastro , @Email , @Site , @Senha , @Status , @IdCidade , @IE , @IM , @EnderecoCobranca ,
                       @CEPCobranca , @TelefoneCobranca , @CelularCobranca , @NumeroCobranca , @BairroCobranca , @ComplementoCobranca , @IdCidadeCobranca  , @RG) ";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Vendedor entity)
        {
            var retorno = -1;

            Query = @"UPDATE Vendedores SET RazaoSocial = @RazaoSocial, NomeFantasia = @NomeFantasia, CNPJ = @CNPJ, Endereco =@Endereco , CEP = @CEP , Telefone =@Telefone , Celular = @Celular , Numero = @Numero , Bairro = @Bairro , 
                        Complemento = @Complemento , Email = @Email , Site = @Site , Senha = @Senha , Status = @Status , IdCidade = @IdCidade , IE = @IE , IM = @IM , EnderecoCobranca = @EnderecoCobranca , 
                        CEPCobranca = @CEPCobranca , TelefoneCobranca = @TelefoneCobranca , CelularCobranca = @CelularCobranca , NumeroCobranca = @NumeroCobranca , BairroCobranca = @BairroCobranca , ComplementoCobranca = @ComplementoCobranca , 
                        IdCidadeCobranca = @IdCidadeCobranca  , RG = @RG WHERE Id = @Id ";

            _parameters.Id = entity.Id;
            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Vendedor entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Vendedores WHERE Id = @Id";

            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Vendedor Get(int id)
        {
            Vendedor vendedor;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                vendedor = _connection.Query<Vendedor>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return vendedor;
        }

        public Vendedor Get(string email, string senha)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(Query);
            SQL.Append(" WHERE Email = @Email AND Senha = @senha ");
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection.Query<Vendedor>(SQL.ToString(), new { Email = email, Senha = senha }).FirstOrDefault();
            }
        }

        public override List<Vendedor> GetAll()
        {
            List<Vendedor> vendedores;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                vendedores = _connection.Query<Vendedor>(Query)
                    .ToList();
            }

            return vendedores;
        }

        public override List<Vendedor> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Vendedor> GetList(object filter)
        {
            List<Vendedor> vendedores;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                vendedores = _connection.Query<Vendedor>(Query)
                    .ToList();
            }

            return vendedores;
        }

        public List<string> GetEmailsVendedores()
        {
            var emails = new List<string>();

            Query = "SELECT distinct Email FROM Vendedores";

            using (_connection = new SqlConnection(_connectionString))
            {
                emails = _connection.Query<string>(Query).ToList();
            }

            return emails;
        }

        public List<string> GetEmailsVendedoresPorAviso(int idPedido)
        {
            var emails = new List<string>();

            Query = "exec ListaDestinatariosPorAviso " + idPedido ;

            using (_connection = new SqlConnection(_connectionString))
            {
                emails = _connection.Query<string>(Query).ToList();
            }

            return emails;
        }

        public List<string> GetEmailsVendedoresNaoContemplados(List<int> ids)
        {
            var emails = new List<string>();
            var list = " ";

            Query = @"SELECT distinct Email FROM Vendedores v
                            INNER JOIN Cotacoes c on v.Id = c.IdVendedor
                            WHERE c.CotacaoEscolhida = 0 and c.Id in ( ";

            for (int i = 0; i < ids.Count; i++)
            {
                if (i < ids.Count - 1)
                {
                    list += ids[i] + ", ";
                }
                    
                else
                {
                    list += ids[i] ;
                }  
            }
            //Alterado por Sérgio em 19/07/2018, para pegar apenas os vendedores não contemplados.
            /*
            _parameters.Ids = list;

            using (_connection = new SqlConnection(_connectionString))
            {
                emails = _connection.Query<string>(Query, _parameters as object).ToList();
            }
            */

            Query += list + " )";

            using (_connection = new SqlConnection(_connectionString))
            {
                emails = _connection.Query<string>(Query).ToList();
            }

            return emails;
        }

        #endregion

        #region Métodos Privados

        private void SetParameters(Vendedor entity)
        {
            _parameters.RazaoSocial = entity.RazaoSocial;
            _parameters.NomeFantasia = entity.NomeFantasia;
            _parameters.CNPJ = entity.CNPJ;
            _parameters.Endereco = entity.Endereco;
            _parameters.CEP = entity.CEP;
            _parameters.Telefone = entity.Telefone;
            _parameters.Celular = entity.Celular;
            _parameters.Numero = entity.Numero;
            _parameters.Bairro = entity.Bairro;
            _parameters.Complemento = entity.Complemento;
            _parameters.Email = entity.Email.ToLower();
            _parameters.Site = entity.Site;
            _parameters.Senha = entity.Senha;
            _parameters.Status = entity.Status;
            _parameters.IdCidade = entity.IdCidade;
            _parameters.IM = entity.IM;
            _parameters.IE = entity.IE;
            _parameters.EnderecoCobranca = entity.EnderecoCobranca;
            _parameters.NumeroCobranca = entity.NumeroCobranca;
            _parameters.ComplementoCobranca = entity.ComplementoCobranca;
            _parameters.BairroCobranca = entity.BairroCobranca;
            _parameters.CEPCobranca = entity.CEPCobranca;
            _parameters.TelefoneCobranca = entity.TelefoneCobranca;
            _parameters.CelularCobranca = entity.CelularCobranca;
            _parameters.IdCidadeCobranca = entity.IdCidadeCobranca;
            _parameters.RG = entity.RG;
            _parameters.DataCadastro = entity.DataCadastro;
            _parameters.Status = entity.Status;
            //_parameters.Id = entity.Id;  
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id, 
        RazaoSocial, 
        NomeFantasia, 
        CNPJ, Endereco,
        CEP, 
        Telefone, 
        Celular, 
        Numero,
        Bairro, 
        Complemento,
        DataCadastro, 
        Email, 
        Site, 
        Senha, 
        Status,
        IdCidade, 
        IE,
        IM, 
        EnderecoCobranca, 
        CEPCobranca, 
        TelefoneCobranca, 
        CelularCobranca, 
        NumeroCobranca, 
        BairroCobranca, 
        ComplementoCobranca,
        IdCidadeCobranca, 
        RG   
FROM  Vendedores ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: RazaoSocial, Id
        }        

        #endregion
    }
}
