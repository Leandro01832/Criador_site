using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Elemento
    {
        [Key]
        public int IdElemento { get; set; }

        public int? texto_ { get; set; }
        [ForeignKey("texto_")]
        public virtual Texto texto { get; set; }

        public int? carousel_ { get; set; }
        [ForeignKey("carousel_")]
        public virtual Carousel carousel { get; set; }

        public int? imagem_ { get; set; }
        [ForeignKey("imagem_")]
        public virtual Imagem imagem { get; set; }

        public int? video_ { get; set; }
        [ForeignKey("video_")]
        public virtual Video video { get; set; }

        [Range(1, 10000, ErrorMessage = "Escolha em qual bloco vai estar o elemento?")]
        public int div_2 { get; set; }
        [ForeignKey("div_2")]
        public virtual Div div { get; set; }
    }
}
