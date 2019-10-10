using business;
using DataContextCriacaoSite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace igreja_WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Processar();
        }

        private void Processar()
        {
            lblResultado.Text = "Processando...";

            List<Video> arquivos = null;

            using (var db = new BD())
            {
                arquivos = db.Video.Where(a => a.Processado == false).ToList();
            }

        }
    }
}
