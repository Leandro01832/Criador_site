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

        public bool Redimencionar { get; set; }
        public int RedimencionarAltura { get; set; }
        public int RedimencionarLargura { get; set; }

        public bool Recortar { get; set; }
        public int RecortarTop { get; set; }
        public int RecortarLeft { get; set; }
        public int RecortarRight { get; set; }
        public int RecortarBottom { get; set; }

        public bool FlipHorizontal { get; set; }
        public bool FlipVertical { get; set; }
        public bool RotacaoEsquerda { get; set; }
        public bool RotacaoDireita { get; set; }

        public bool Texto { get; set; }
        public string TextoImagem { get; set; }

        [NotMapped]
        public IEnumerable<HttpPostedFileBase> FiguraFile { get; set; }
        public virtual List<Background> Backgrounds { get; set; }
        public virtual List<Carousel> Carousels { get; set; }
        public virtual List<Div> Divs { get; set; }
        

        public int? pagina_ { get; set; }
        [ForeignKey("pagina_")]
        public virtual Pagina pagina { get; set; }

        //public int? div_ { get; set; }
        //[ForeignKey("div_")]
        //public virtual Div div { get; set; }

    }
}
