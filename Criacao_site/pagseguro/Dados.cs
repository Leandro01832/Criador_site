using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PagSeguro
{
    public class Dados
    {
        // Seus dados de acesso ao PagSeguro
        [Key]
        public int DadosId { get; set; }
        
        [NotMapped]
        public string MeuEmail { get; set; }
        [NotMapped]
        public string MeuToken { get; set; }
        public int NumeroPedido { get; set; }
        // Dados de Envvio para o PagSeguro
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DDD { get; set; }
        [Display(Name ="Numero de telefone")]
        public string NumeroTelefone { get; set; }
        public string Valor { get; set; }        
        public string CodigoAcesso { get; set; }        
        public string Referencia { get; set; }        
        public string TituloPagamento { get; set; }
        // Dados de Retorno do PagSeguro        
        public string Status { get; set; }        
        public string stringConexao { get; set; }
        public DateTime data { get; set; }
    }
}
