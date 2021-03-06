﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;

namespace CotaPedido.WebSite.Models
{
    public class CadastroCompradorModel
    {
        #region Propriedades Públicas

        [Display(Name = "Primeiro Nome")]
        [MaxLength(64, ErrorMessage = "O campo Primeiro Nome deve conter no máximo 64 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Primeiro Nome")]
        public string PrimeiroNome { get; set; }

        [Display(Name = "Nome Completo")]
        [MaxLength(200, ErrorMessage = "O campo Nome Completo deve conter no máximo 200 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Nome Completo")]
        public string NomeCompleto { get; set; }

        [Display(Name = "E-Mail")]
        [MaxLength(200, ErrorMessage = "O campo E-Mail deve conter no máximo 200 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail não é válido")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Preencha com um E-Mail válido")]
        [Required(ErrorMessage = "Preencha o campo E-Mail")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [MaxLength(8, ErrorMessage = "O campo Senha deve conter no máximo 8 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Senha")]
        public string Senha { get; set; }

        [Display(Name = "Repita a Senha")]
        [MaxLength(20, ErrorMessage = "O campo Repita a Senha deve conter no máximo 20 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Repita a Senha")]
        [Compare("Senha", ErrorMessage = "As senhas devem ser iguais")]
        public string RepitaSenha { get; set; }

        [Display(Name = "UF")]
        [Required(ErrorMessage = "Preencha o campo UF")]
        public UF UF { get; set; }

        [Display(Name = "Telefone")]
        [MaxLength(14, ErrorMessage = "O campo Telefone deve conter no máximo 14 caracteres")]
        public string Telefone { get; set; }

        [Display(Name = "Celular")]
        [MaxLength(15, ErrorMessage = "O campo Celular deve conter no máximo 15 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Celular")]
        public string Celular { get; set; }

        [Display(Name = "Bairro")]
        [MaxLength(80, ErrorMessage = "O campo Bairro deve conter no máximo 80 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Bairro")]
        public string Bairro { get; set; }

        [Display(Name = "CPF/CNPJ")]
        [MaxLength(18, ErrorMessage = "O campo CPF/CNPJ deve conter no máximo 18 caracteres")]
        [Required(ErrorMessage = "Preencha o campo CPF/CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "RG")]
        [MaxLength(20, ErrorMessage = "O campo RG deve conter no máximo 20 caracteres")]
        public string RG { get; set; }

        [Display(Name = "Endereço")]
        [MaxLength(120, ErrorMessage = "O campo Endereço deve conter no máximo 120 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Endereço")]
        public string Endereco { get; set; }

        [Display(Name = "Complemento")]
        [MaxLength(80, ErrorMessage = "O campo Complemento deve conter no máximo 80 caracteres")]
        public string Complemento { get; set; }

        [Display(Name = "Número")]
        [MaxLength(6, ErrorMessage = "O campo Número deve conter no máximo 6 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Número")]
        public string Numero { get; set; }

        [Display(Name = "Site")]
        [MaxLength(100, ErrorMessage = "O campo Site deve conter no máximo 100 caracteres")]
        public string Site { get; set; }

        [Display(Name = "CEP")]
        [MaxLength(10, ErrorMessage = "O campo CEP deve conter no máximo 10 caracteres")]
        [Required(ErrorMessage = "Preencha o campo CEP")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Selecione uma Cidades")]
        public int IdCidade { get; set; }

        [Display(Name = "Inscrição Municipal")]
        [MaxLength(20, ErrorMessage = "O campo Inscrição Municipal deve conter no máximo 20 caracteres")]
        public string IM { get; set; }

        [Display(Name = "Inscrição Estadual")]
        [MaxLength(20, ErrorMessage = "O campo Inscrição Estadual deve conter no máximo 20 caracteres")]
        public string IE { get; set; }

        [Display(Name = "Endereço")]
        [MaxLength(120, ErrorMessage = "O campo Endereço deve conter no máximo 120 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Endereço")]
        public string EnderecoCobranca { get; set; }

        [Display(Name = "CEP")]
        [MaxLength(10, ErrorMessage = "O campo CEP deve conter no máximo 10 caracteres")]
        [Required(ErrorMessage = "Preencha o campo CEP")]
        public string CEPCobranca { get; set; }

        [Display(Name = "Telefone")]
        [MaxLength(14, ErrorMessage = "O campo Telefone deve conter no máximo 14 caracteres")]
        public string TelefoneCobranca { get; set; }

        [Display(Name = "Celular")]
        [MaxLength(15, ErrorMessage = "O campo Celular deve conter no máximo 15 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Celular")]
        public string CelularCobranca { get; set; }

        [Display(Name = "Número")]
        [MaxLength(6, ErrorMessage = "O campo Número deve conter no máximo 6 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Número")]
        public string NumeroCobranca { get; set; }

        [Display(Name = "Bairro")]
        [MaxLength(80, ErrorMessage = "O campo Bairro deve conter no máximo 80 caracteres")]
        [Required(ErrorMessage = "Preencha o campo Bairro")]
        public string BairroCobranca { get; set; }

        [Display(Name = "Complemento")]
        [MaxLength(80, ErrorMessage = "O campo Complemento deve conter no máximo 80 caracteres")]
        public string ComplementoCobranca { get; set; }

        [Required(ErrorMessage = "Selecione uma Cidades")]
        public int IdCidadeCobranca { get; set; }

        [Display(Name = "UF")]
        [Required(ErrorMessage = "Preencha o campo UF")]
        public UF UFCobranca { get; set; }

        #endregion

        #region Métodos Públicos

        public Comprador GetComprador()
        {
            var comprador = new Comprador
            {
                RazaoSocial = NomeCompleto,
                NomeFantasia = PrimeiroNome,
                Celular = Celular,
                Email = Email,
                Senha = Senha,
                Telefone = Telefone,
                Bairro = Bairro,
                IdCidade = IdCidade,
                CNPJ = CNPJ,
                Endereco = Endereco,
                Complemento = Complemento,
                Numero = Numero,
                Site = Site,
                CEP = CEP,
                IE = IE,
                IM = IM,
                BairroCobranca = BairroCobranca,
                IdCidadeCobranca = IdCidadeCobranca,
                ComplementoCobranca = ComplementoCobranca,
                CEPCobranca = CEPCobranca,
                CelularCobranca = CelularCobranca,
                EnderecoCobranca = EnderecoCobranca,
                NumeroCobranca = NumeroCobranca,
                TelefoneCobranca = TelefoneCobranca
            };
            return comprador;
        }

        #endregion
    }
}