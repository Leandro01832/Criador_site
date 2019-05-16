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
        public bool backgroundImage { get; set; }
        public string Cor { get; set; }
        public string Imagem { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImagemFile { get; set; }
        public virtual ICollection<Pagina> Pagina { get; set; }
        
    }
}
