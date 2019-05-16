using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Codigo
    {
        [Key]
        public int IdCodigo { get; set; }
        public string Efeito { get; set; }
        public int pagina_ { get; set; }
        [ForeignKey("pagina_")]
        public virtual Pagina Pagina { get; set; }
    }
}
