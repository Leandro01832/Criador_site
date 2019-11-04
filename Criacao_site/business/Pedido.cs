using business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business
{
   public class Pedido
    {       

        [Key]
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "O nome do site é necessário")]
        [Display(Name = "Nome do site")]
        public string Nome { get; set; }

        public virtual Servico Servico { get; set; }
        public virtual DateTime Datapedido { get; set; }
        public virtual CLiente Cliente { get; set; }
        public virtual List<Pagina> Paginas { get; set; }
        public string Status { get; set; }

        public void CriarPaginaLixeira(Pedido ped, List<Imagem> imagens)
        {
            ped.Paginas[0].Background = new List<Background>
                    {
                        new Background
                        {
                            backgroundImage = true,
                            backgroundTransparente = false,
                            Background_Position = "",
                            Background_Repeat = "",
                            Codigo = "",
                            Cor = "#000000",
                            imagem = imagens[0],
                            imagem_ = imagens[0].IdImagem,
                            Nome = "plano de fundo da pagina",
                            Pagina = ped.Paginas[0],
                            pagina_2 = ped.Paginas[0].IdPagina
                        },
                        new Background
                        {
                            backgroundImage = true,
                            backgroundTransparente = false,
                            Background_Position = "",
                            Background_Repeat = "",
                            Codigo = "",
                            Cor = "#000000",
                            imagem = imagens[1],
                            imagem_ = imagens[1].IdImagem,
                            Nome = "topo",
                            Pagina = ped.Paginas[0],
                            pagina_2 = ped.Paginas[0].IdPagina
                        },
                        new Background
                        {
                            backgroundImage = true,
                            backgroundTransparente = false,
                            Background_Position = "",
                            Background_Repeat = "",
                            Codigo = "",
                            Cor = "#000000",
                            imagem = imagens[2],
                            imagem_ = imagens[2].IdImagem,
                            Nome = "menu",
                            Pagina = ped.Paginas[0],
                            pagina_2 = ped.Paginas[0].IdPagina
                        },
                        new Background
                        {
                            backgroundImage = false,
                            backgroundTransparente = true,
                            Background_Position = "",
                            Background_Repeat = "",
                            Codigo = "",
                            Cor = "#000000",
                            imagem = imagens[0],
                            imagem_ = imagens[0].IdImagem,
                            Nome = "borda esquerda",
                            Pagina = ped.Paginas[0],
                            pagina_2 = ped.Paginas[0].IdPagina
                        },
                        new Background
                        {
                            backgroundImage = false,
                            backgroundTransparente = true,
                            Background_Position = "",
                            Background_Repeat = "",
                            Codigo = "",
                            Cor = "#000000",
                            imagem = imagens[0],
                            imagem_ = imagens[0].IdImagem,
                            Nome = "borda direita",
                            Pagina = ped.Paginas[0],
                            pagina_2 = ped.Paginas[0].IdPagina
                        },
                        new Background
                        {
                            backgroundImage = false,
                            backgroundTransparente = true,
                            Background_Position = "",
                            Background_Repeat = "",
                            Codigo = "",
                            Cor = "#000000",
                            imagem = imagens[0],
                            imagem_ = imagens[0].IdImagem,
                            Nome = "blocos",
                            Pagina = ped.Paginas[0],
                            pagina_2 = ped.Paginas[0].IdPagina
                        }
                    };
        }

        public void CriarBlocoLixeira(Pedido ped)
        {
            ped.Paginas[0].Div = new List<Div>
                     {
                         new Div
                         {
                            Nome = "Lixeira",
                            Pagina = ped.Paginas[0],
                            pagina_ = ped.Paginas[0].IdPagina,
                            Video = new List<Video>(),
                            Textos = new List<Texto>(),
                            Carousel = new List<Carousel>(),
                            Imagem = new List<Imagem>(),
                            Elemento = new List<Elemento>(),
                            background_ = ped.Paginas[0].Background.ToList()[2].IdBackground,
                            Background = ped.Paginas[0].Background.ToList()[2],
                            BorderRadius = 10,
                            Colunas = "auto auto auto auto auto auto auto auto auto auto auto auto",
                            Desenhado = 0,
                            Divisao = "col-md-12",
                            Padding = 5,
                            Height = 400,
                            Codigo = ""
                         }
                     };
        }
    }
}
