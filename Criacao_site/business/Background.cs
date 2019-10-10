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
   public class Background
    {
        [Key]
        public int IdBackground { get; set; }

        [Required(ErrorMessage ="O nome do plano de fundo é necessário")]
        [Display(Name ="Nome do plano de fundo")]
        public string Nome { get; set; }

        public string Codigo { get; set; }

        [Display(Name = "Plano de fundo é uma imagem?")]
        public bool backgroundImage { get; set; }

        [Display(Name = "Plano de fundo é transparente?")]
        public bool backgroundTransparente { get; set; }

        public string Cor { get; set; }

        [Display(Name = "Tipo de repetição do plano de fundo")]
        public string Background_Repeat { get; set; }

        [Display(Name = "Posição do plano de fundo")]
        public string Background_Position { get; set; }

        [Range(1, 10000, ErrorMessage = "Escolha em qual pagina vai estar o background")]
        [Display(Name = "Em qual pagina colocar o plano de fundo")]
        public int pagina_2 { get; set; }
        [ForeignKey("pagina_2")]
        public virtual Pagina Pagina { get; set; }

        [Display(Name = "Imagem do plano de fundo")]
        public int imagem_ { get; set; }
        [ForeignKey("imagem_")]
        public virtual Imagem imagem { get; set; }

    }
}
