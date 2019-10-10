using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class CLiente
    {
        [Key]
        public int IdCliente { get; set; }

        [Display(Name ="Nome")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string UserName { get; set; }
        public string Cpf { get; set; }
        public virtual Telefone Telefone { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual List<Pedido> Servicos { get; set; }
        

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
