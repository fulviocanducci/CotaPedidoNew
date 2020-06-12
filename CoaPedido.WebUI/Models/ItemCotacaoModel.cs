using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios.SQLServer;
using System;
using System.ComponentModel.DataAnnotations;

namespace CotaPedido.WebUI.Models
{
    public class ItemCotacaoModel
    {
        public int Id { get; set; }

        public int IdCotacao { get; set; }

        [Required(ErrorMessage = "O campo Valor Unitário é obrigatório")]
        public decimal ValorUnitario { get; set; }
        
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "O campo Valor Total é obrigatório")]
        public decimal ValorTotal { get; set; }
        
        public string NomeProduto { get; set; }
        
        public int IdUnidade { get; set; }
        
        public int IdItem { get; set; }
        
        public DateTime DataCadastro { get; set; }
        
        public string Embalagem { get; set; }
        
        public int IdSubCategoria { get; set; }
        
        public int IdCategoria { get; set; }        
        
        public Grupo Categoria { get; set; }

        public SubGrupo SubCategoria { get; set; }

        public Unidade Unidade { get; set; }
    }
}