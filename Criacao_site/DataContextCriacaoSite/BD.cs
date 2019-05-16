using business;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataContextCriacaoSite
{
    public class BD : DbContext
    {
        public BD() : base("DefaultConnection")
        {

        }

        public DbSet<CLiente> Cliente { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Servico> Servico { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Pagina> Pagina { get; set; }
        public DbSet<Carousel> Carousel { get; set; }
        public DbSet<Div> Div { get; set; }
        public DbSet<Letra> Letra { get; set; }
        public DbSet<Background> Background { get; set; }
        public DbSet<Codigo> Codigo { get; set; }
       

        // public DbSet<Dados> Dados { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<business.Imagem> Imagems { get; set; }
    }
}
