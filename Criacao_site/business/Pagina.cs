using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace business
{
    public class Pagina
    {
        [Key]
        public int IdPagina { get; set; }
        public string Titulo { get; set; }
        public ICollection<Codigo> Efeitos { get; set; }
        public string CodigoHtml { get; set; }
        public int servico_ { get; set; }
        public virtual ICollection<Background> Background { get; set; }     
        public virtual ICollection<Div> Div { get; set; }


        [ForeignKey("servico_")]
        public virtual Servico Servico { get; set; }
    }
}
