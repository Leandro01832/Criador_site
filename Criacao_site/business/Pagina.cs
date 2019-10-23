using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    public class Pagina
    {
        [Key]
        public int IdPagina { get; set; }

        [Required(ErrorMessage = "O titulo é necessário")]
        [Display(Name = "Titulo da pagina")]
        public string Titulo { get; set; }

        [DataType(DataType.Url)]
        public string Facebook { get; set; }

        [DataType(DataType.Url)]
        public string Twiter { get; set; }

        [DataType(DataType.Url)]
        public string Instagram { get; set; }

        public string Codigo { get; set; }

        public string CodigoRenderizado { get; set; }

        public virtual ICollection<Background> Background { get; set; }
        public virtual ICollection<Imagem> Imagem { get; set; }
        public virtual ICollection<Div> Div { get; set; }        

        [Range(1, 10000, ErrorMessage = "Escolha qual o site para esta pagina")]
        [Display(Name = "Qual é o site desta pagina?")]
        public int pedido_ { get; set; }
        [ForeignKey("pedido_")]
        public virtual Pedido Pedido { get; set; }
    }
}
