using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Servico
    {
        [Key, ForeignKey("Pedido")]
        public int IdServico { get; set; }
        public string Descricao { get; set; }
        public virtual Pedido Pedido { get; set; }
        
        
    }
}
