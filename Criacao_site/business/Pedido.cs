using business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "O nome do site é necessário")]
        [Display(Name = "Nome do site")]
        public string Nome { get; set; }

        public virtual Servico Servico { get; set; }
        public virtual DateTime Datapedido { get; set; }
        public virtual CLiente Cliente { get; set; }
        public virtual List<Pagina> Paginas { get; set; }
        public string Status { get; set; }
    }
}
