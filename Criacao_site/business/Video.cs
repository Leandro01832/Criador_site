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
   public class Video
    {
        [Key]
        public int IdVideo { get; set; }

        [Required(ErrorMessage = "O nome do video é necessário")]
        [Display(Name = "Nome do video")]
        public string Nome { get; set; }        

        public string ArquivoVideo { get; set; } 
        
        public bool Processado { get; set; }

        [NotMapped]
        public HttpPostedFileBase videoFile { get; set; }

        [Range(1, 10000, ErrorMessage = "Escolha em qual bloco vai estar o video.")]
        [Display(Name = "Colocar em qual bloco o Video?")]
        public int div_ { get; set; }
        [ForeignKey("div_")]
        public virtual Div div { get; set; }
        
    }
}
