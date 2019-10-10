using business;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContextCriacaoSite
{
   public class BD : DbContext
    {
        public BD() : base("DefaultConnection")
        {
           // Database.SetInitializer<BD>(null);
        }

        public DbSet<CLiente> Cliente { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<Servico> Servico { get; set; }
        public DbSet<Carousel> Carousel { get; set; }
        public DbSet<Imagem> Imagem { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Pagina> Pagina { get; set; }
        public DbSet<Div> Div { get; set; }
        public DbSet<Texto> Texto { get; set; }
        public DbSet<Background> Background { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Elemento> Elemento { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
