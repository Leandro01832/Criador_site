using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace business
{
   public class Div
    {
        [Key]
        public int IdDiv { get; set; }
        public string Texto { get; set; }
        public int background_ { get; set; }
        [ForeignKey("background_")]
        public virtual Background Background { get; set; }
        public virtual ICollection<Letra> Letra { get; set; }
        public bool AdicionarImagem { get; set; }
        public string ImagemDiv { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemDivFile { get; set; }
        public virtual ICollection<Carousel> Carousel { get; set; }
        public int pagina_ { get; set; }
        [ForeignKey("pagina_")]
        public virtual Pagina Pagina { get; set; }


    }
}
