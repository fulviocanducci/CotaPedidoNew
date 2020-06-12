using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Vendedor
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string RG { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public Nullable<System.DateTime> DataCadastro { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public string Senha { get; set; }
        public Nullable<bool> Status { get; set; }
        public int IdCidade { get; set; }
        public string IM { get; set; }
        public string IE { get; set; }
        public string EnderecoCobranca { get; set; }
        public string CEPCobranca { get; set; }
        public string TelefoneCobranca { get; set; }
        public string CelularCobranca { get; set; }
        public string NumeroCobranca { get; set; }
        public string BairroCobranca { get; set; }
        public string ComplementoCobranca { get; set; }
        public int IdCidadeCobranca { get; set; }
        //public virtual Cidade Cidade { get; set; }
        //public virtual Cidade CidadeCobranca { get; set; }
    }
}
