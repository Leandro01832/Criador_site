using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace business
{
    public class Imagem
    {
        [Key]
        public int IdImagem { get; set; }
        public string Figura { get; set; }
        [NotMapped]
        public HttpPostedFileBase FiguraFile { get; set; }
    }
}
