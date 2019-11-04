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

        public string CodigoCss { get; set; }
        public string CodigoHtml { get; set; }

        public bool ModalDireita { get; set; }

        public bool Layout { get; set; }

        public virtual List<Background> Background { get; set; }
        public virtual ICollection<Imagem> Imagem { get; set; }
        public virtual ICollection<Div> Div { get; set; }        

        [Range(1, 10000, ErrorMessage = "Escolha qual o site para esta pagina")]
        [Display(Name = "Qual é o site desta pagina?")]
        public int pedido_ { get; set; }
        [ForeignKey("pedido_")]
        public virtual Pedido Pedido { get; set; }


        public List<Background> CriarBackgrounds(Pagina pag, Pagina PagLayout, List<Imagem> imagens)
        {
            List<Background> Background = new List<Background>();
            bool padrao = false;
            
            foreach (var pagi in pag.Pedido.Paginas)
            {
                if (pag.Layout)
                {
                    padrao = true;
                }
            }

            if (padrao)
            {
                Background = PagLayout.Background;
            }
            else
            {
                Background back1 = new Background
                {
                    backgroundImage = true,
                    backgroundTransparente = false,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pag.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = imagens[0],
                    imagem_ = imagens[0].IdImagem,
                    Nome = "plano de fundo da pagina"
                };

                Background back2 = new Background
                {
                    backgroundImage = true,
                    backgroundTransparente = false,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pag.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = imagens[1],
                    imagem_ = imagens[1].IdImagem,
                    Nome = "topo"
                };


                Background back3 = new Background
                {
                    backgroundImage = true,
                    backgroundTransparente = false,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pag.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = imagens[2],
                    imagem_ = imagens[2].IdImagem,
                    Nome = "menu"
                };

                Background back4 = new Background
                {
                    backgroundImage = false,
                    backgroundTransparente = true,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pag.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = imagens[0],
                    imagem_ = imagens[0].IdImagem,
                    Nome = "borda esquerda"
                };

                Background back5 = new Background
                {
                    backgroundImage = false,
                    backgroundTransparente = true,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pag.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = imagens[0],
                    imagem_ = imagens[0].IdImagem,
                    Nome = "borda direita"
                };

                Background back6 = new Background
                {
                    backgroundImage = false,
                    backgroundTransparente = true,
                    Background_Position = "",
                    Background_Repeat = "",
                    pagina_2 = pag.IdPagina,
                    Codigo = "",
                    Cor = "#000000",
                    imagem = imagens[0],
                    imagem_ = imagens[0].IdImagem,
                    Nome = "blocos"
                };

                Background.Add(back1);
                Background.Add(back2);
                Background.Add(back3);
                Background.Add(back4);
                Background.Add(back5);
                Background.Add(back6);

                
            }

            return Background;
        }


        public int[] CriarRows(Pagina pag)
        {
            int espaco = 0;
            int rows = 0;

            foreach (var bloco in pag.Div)
            {
                if (bloco.Divisao == "col-md-12" || bloco.Divisao == "col-sm-12")
                    espaco += 12;

                if (bloco.Divisao == "col-md-6" || bloco.Divisao == "col-sm-6")
                    espaco += 6;

                if (bloco.Divisao == "col-md-4" || bloco.Divisao == "col-sm-4")
                    espaco += 4;

                if (bloco.Divisao == "col-md-3" || bloco.Divisao == "col-sm-3")
                    espaco += 3;

                if (bloco.Divisao == "col-md-2" || bloco.Divisao == "col-sm-2")
                    espaco += 2;


            }

            rows = espaco / 12;

            rows += 1;

            int[] numero = new int[rows];

            for (int i = 0; i < numero.Length; i++)
            {
                numero[i] += i + 1;

            }

            return numero;
        }
        
    }
}
