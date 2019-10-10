using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Texto
    {
        [Key]
        public int IdTexto { get; set; }

        [Required(ErrorMessage = "O nome do texto é necessário")]
        [Display(Name = "Nome do texto")]
        public string Nome { get; set; }

        [Display(Name = "Texto")]
        public string Palavras { get; set; }
        public string Codigo { get; set; }

        [Range(1, 10000, ErrorMessage = "Escolha em qual bloco vai estar o texto")]
        [Display(Name = "Colocar em qual bloco o Texto?")]
        public int div_ { get; set; }
        [ForeignKey("div_")]
        public virtual Div div { get; set; }
        
    }
}
