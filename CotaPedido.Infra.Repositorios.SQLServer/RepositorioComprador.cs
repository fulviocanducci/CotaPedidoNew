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
    public class RepositorioComprador : SqlRepositoryBase<Comprador>, IRepositorioComprador
    {
        #region Construtores

        public RepositorioComprador()
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
                    .Query<int>("SELECT count(email) FROM Compradores WHERE email=@email", new { email })
                    .FirstOrDefault() > 0;
            }

            return retorno;
        }

        public Comprador Get(string email, string senha)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                return _connection
                    .Query<Comprador>("SELECT * FROM Compradores WHERE Email=@email AND Senha=@senha AND Status=1", new { email, senha })
                    .FirstOrDefault();
            }
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
            Query = @"INSERT INTO Compradores(Email, Senha, NomeFantasia, RazaoSocial, IdCidade, IdCidadeCobranca, DataCadastro, Status) ";
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
        public override int Insert(Comprador entity)
        {
            var retorno = -1;

            Query = @"INSERT INTO Compradores ( RazaoSocial , NomeFantasia , CNPJ , Endereco , CEP , Telefone , Celular , Numero , Bairro , Complemento ,  " +
                         " Email , Site , Senha , Status , IdCidade , IM , IE , EnderecoCobranca , NumeroCobranca , ComplementoCobranca , BairroCobranca ,  " +
                         " CEPCobranca , TelefoneCobranca , CelularCobranca , IdCidadeCobranca , RG ) " +
                         " VALUES ( @RazaoSocial , @NomeFantasia , @CNPJ , @Endereco , @CEP , @Telefone , @Celular , @Numero , @Bairro , @Complemento ,  " +
                         " @Email , @Site , @Senha , @Status , @IdCidade , @IM , @IE , @EnderecoCobranca , @NumeroCobranca , @ComplementoCobranca , @BairroCobranca ,  " +
                         " @CEPCobranca , @TelefoneCobranca , @CelularCobranca , @IdCidadeCobranca , @RG )  ";

            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Update(Comprador entity)
        {
            var retorno = -1;

            Query = @"UPDATE Compradores SET    RazaoSocial = @RazaoSocial, NomeFantasia = @NomeFantasia, CNPJ = @CNPJ , Endereco = @Endereco , CEP = @CEP , Telefone = @Telefone , Celular = @Celular , Numero = @Numero , Bairro = @Bairro , Complemento = @Complemento, 
                        Email = @Email, Site = @Site , Senha = @Senha, Status = @Status , IdCidade = @IdCidade , IM = @IM , IE = @IE , EnderecoCobranca = @EnderecoCobranca, NumeroCobranca = @NumeroCobranca , ComplementoCobranca = @ComplementoCobranca, BairroCobranca = @BairroCobranca,  
                        CEPCobranca = @CEPCobranca , TelefoneCobranca = @TelefoneCobranca , CelularCobranca = @CelularCobranca , IdCidadeCobranca = @IdCidadeCobranca, RG = @RG WHERE Id = @Id ";

            _parameters.Id = entity.Id;
            SetParameters(entity);

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override int Delete(Comprador entity)
        {
            var retorno = -1;

            Query = @"DELETE FROM Compradores WHERE Id = @Id ";

            _parameters.Id = entity.Id;

            using (_connection = new SqlConnection(_connectionString))
            {
                retorno = _connection.Query<int>(Query, _parameters as object).SingleOrDefault();
            }

            return retorno;
        }

        public override Comprador Get(int id)
        {
            Comprador comprador;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                comprador = _connection.Query<Comprador>(Query)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            }

            return comprador;
        }

        public override List<Comprador> GetAll()
        {
            List<Comprador> compradores;

            InitializeQuery();

            using (_connection = new SqlConnection(_connectionString))
            {
                compradores = _connection.Query<Comprador>(Query)
                    .ToList();
            }

            return compradores;
        }

        public override List<Comprador> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<Comprador> GetList(object filter)
        {
            List<Comprador> compradores;

            InitializeQuery();

            SetWhereClauses(filter);

            using (_connection = new SqlConnection(_connectionString))
            {
                compradores = _connection.Query<Comprador>(Query)
                    .ToList();
            }

            return compradores;
        }

        public string GetEmailCompradorPorIdPedido(int idPedido)
        {
            var email = "";

            Query = "SELECT distinct Email FROM Compradores c INNER JOIN Pedidos p on c.Id = p.IdComprador WHERE p.Id = @IdPedido";
            _parameters.IdPedido = idPedido;

            using (_connection = new SqlConnection(_connectionString))
            {
                email = _connection.Query<string>(Query, _parameters as object).SingleOrDefault();
            }

            return email;
        }

        #endregion

        #region Métodos Privados

        private void SetParameters(Comprador entity)
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
        }

        #endregion

        #region Métodos Protegidos

        protected override void InitializeQuery()
        {
            Query = @"
SELECT  Id , RazaoSocial , NomeFantasia , CNPJ , Endereco , CEP , Telefone , Celular , Numero , Bairro , Complemento , DataCadastro ,  
        Email , Site , Senha , Status , IdCidade , IM , IE , EnderecoCobranca , NumeroCobranca , ComplementoCobranca , BairroCobranca ,  
        CEPCobranca , TelefoneCobranca , CelularCobranca , IdCidadeCobranca , RG 
FROM  Compradores ";
        }

        protected override void SetWhereClauses(object filter)
        {
            //FILTROS: IdItem, Id, RazaoSocial
        }

        

        #endregion
    }
}
