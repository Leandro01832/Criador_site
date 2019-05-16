using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Letra
    {
        [Key]
        public int IdLetra { get; set; }
        public string Tipo { get; set; }
        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public int div_ { get; set; }
        [ForeignKey("div_")]
        public virtual Div Div { get; set; }
    }
}
