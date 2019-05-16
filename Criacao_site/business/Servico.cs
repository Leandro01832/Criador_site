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
        [Key]
        public int IdServico { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Pagina> Paginas { get; set; }
    }
}
