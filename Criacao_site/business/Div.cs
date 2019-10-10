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
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O nome do bloco é necessário")]
        [Display(Name ="Nome do bloco")]
        public string Nome { get; set; }

        [Display(Name = "Conteudo centralizado")]
        public int Padding { get; set; }

        [Display(Name = "Altura")]
        public int Height { get; set; }

        public int Desenhado { get; set; }

        [Display(Name = "Espaçamento")]
        public string Divisao { get; set; }

        [Display(Name = "Borda arredondada")]
        public int BorderRadius { get; set; }

        [Range(1, 10000, ErrorMessage ="Escolha um plano de fundo para a div")]
        [Display(Name = "Qual plano de fundo do bloco?")]
        public int background_ { get; set; }
        [ForeignKey("background_")]
        public virtual Background Background { get; set; }

        public virtual List<Texto> Textos { get; set; }
        public virtual List<Carousel> Carousel { get; set; }
        public virtual List<Video> Video { get; set; }
        public virtual List<Elemento> Elemento { get; set; }
        public virtual List<Div> Blocos { get; set; }


        [Range(1, 10000, ErrorMessage = "Escolha em qual pagina vai estar o bloco")]
        [Display(Name = "colocar em qual pagina o bloco?")]
        public int pagina_ { get; set; }
        [ForeignKey("pagina_")]
        public virtual Pagina Pagina { get; set; }

        [Display(Name = "Qual é a imagem do bloco?")]
        public int? imagem_ { get; set; }
        [ForeignKey("imagem_")]
        public virtual Imagem imagem { get; set; }


    }
}
