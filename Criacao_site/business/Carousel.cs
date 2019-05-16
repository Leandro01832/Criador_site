using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Carousel
    {
        [Key]
        public int IdCarousel { get; set; }
        public ICollection<string> Imagens { get; set; }
        public int div_2 { get; set; }
        [ForeignKey("div_2")]        
        public virtual Div Div { get; set; }
    }
}
