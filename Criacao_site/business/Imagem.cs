using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace business
{
    public class Imagem
    {
        [Key]
        public int IdImagem { get; set; }

        [Display(Name = "Arquivo")]
        public string Arquivo { get; set; }

        //[NotMapped]
        //public List<string> Figura { get; set; }

        [NotMapped]
        public IEnumerable<HttpPostedFileBase> FiguraFile { get; set; }
        public virtual List<Background> Backgrounds { get; set; }
        public virtual List<Carousel> Carousels { get; set; }
        public virtual List<Div> Divs { get; set; }
        // public virtual List<Pagina> paginas { get; set; }

        public int? pagina_ { get; set; }
        [ForeignKey("pagina_")]
        public virtual Pagina pagina { get; set; }

    }
}
