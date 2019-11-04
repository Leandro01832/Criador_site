using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PagSeguro.Properties;

namespace PagSeguro
{
    public partial class frPrincipal : Form
    {
        // Iniciar váriáveis locais para acesso ao PagSeguro
        private const string pMeuEmail = "leandroleanleo@gmail.com"; // Colque seu email de cadastro do PagSeguro aqui
        private const string pMeuToken = "A37C25015C8446EE938DBB58D92F6BCF"; // Coloque seu token de acesso do PagSeguro aqui

        public frPrincipal()
        {
            InitializeComponent();
            // Recupera no app.config o código do último pagamento gerado
            txtCode.Text = Settings.Default.sCode;
            // Libera botão de conulta caso tenha um código de pagamento
            btnConsultar.Enabled = !txtCode.Text.Equals(string.Empty);
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se os dados foram digitados, caso não tenha sido, gera uma excessão
                if (!sValidar.IsConnected())
                    throw new Exception("É necessário estar conectado à internet para gerar o pagamento!");
                if (txtNome.Text.Equals(string.Empty))
                    throw new Exception("Digite o nome completo antes de gerar o pagamento!");
                if (txtEmail.Text.Equals(string.Empty))
                    throw new Exception("Digite um e-mail válido antes antes de gerar o pagamento!");
                if (txtDdd.Text.Equals(string.Empty))
                    throw new Exception("Digite o DDD do seu número de telefone antes de gerar o pagamento!");
                if (txtNumeroTelefone.Text.Equals(string.Empty))
                    throw new Exception("Digite o número do seu telefone antes de gerar o pagamento!");
                if (Convert.ToDecimal(txtValor.Text).Equals(0))
                    throw new Exception("O valor do pagamento não pode estar zerado!");
                // Povoar atributos da entidade para envio
                Dados dadosEnvio = new Dados
                {
                    MeuEmail = pMeuEmail,
                    MeuToken = pMeuToken,
                    TituloPagamento = "Teste de Pagamento",
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    DDD = txtDdd.Text,
                    NumeroTelefone = txtNumeroTelefone.Text,
                    Referencia = "001",
                    Valor = txtValor.Text
                };
                // Enviar dados para o PagSeguro para gerar o pagamento
                dadosEnvio = sPagSeguro.GerarPagamento(dadosEnvio);
                // Se retornar uma string para abrir a página do pagseguro
                // quer dizer que foi gerado o código de acesso
                if (!dadosEnvio.stringConexao.Equals(string.Empty))
                {
                    txtCode.Text = dadosEnvio.CodigoAcesso;
                    btnConsultar.Enabled = true;
                    // Salvar o código de acesso no app.config para futuras pesquiss
                    Settings.Default.sCode = txtCode.Text;
                    Settings.Default.Save();
                    Process.Start(dadosEnvio.stringConexao);
                }
                // Foca no campo nome                
                txtNome.Focus();
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Foca na caixa de texto vazia
                if (txtNome.Text.Equals(string.Empty))
                    txtNome.Focus();
                else if (txtEmail.Text.Equals(string.Empty))
                    txtEmail.Focus();
                else if (txtDdd.Text.Equals(string.Empty))
                    txtDdd.Focus();
                else if (txtNumeroTelefone.Text.Equals(string.Empty))
                    txtNumeroTelefone.Focus();
                else if (Convert.ToDecimal(txtValor.Text).Equals(0))
                    txtValor.Focus();
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (!sValidar.IsConnected())
            {
                MessageBox.Show("É necessário estar conectado à internet para gerar o pagamento!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Posiciona o foco no nome
                txtNome.Focus();
                return;
            }
            // Iniciar variáveis para retorno de mensagem
            Color pFore = Color.White;
            Color pBack = Color.Red;
            var pMensagem = "o pagamento ainda não foi confirmado";
            // Criar nova entidade com os dados para consulta
            Dados meusDados = new Dados
            {
                CodigoAcesso = txtCode.Text,
                MeuEmail = pMeuEmail,
                MeuToken = pMeuToken
            };
            // Consu8lta junto ao PagSeguro se o pagamento foi efetuado
            Dados retonoConsulta = sPagSeguro.ValidarPagamento(meusDados);
            // Se o retorno não for nulo, quer dizer que o pagamento foi efetuado
            if (retonoConsulta != null)
            {
                // Se o status for 3, foi aprovado, caso contrário continua aguardando o pagamento
                if (retonoConsulta.Status == "3")
                {
                    pBack = Color.SeaGreen;
                    pMensagem = "pagamento confirmado com sucesso";
                }
            }
            // Exibe a mensagem da situação do pagamento na tela
            lblMensagem.Text = pMensagem;
            lblMensagem.BackColor = pBack;
            lblMensagem.ForeColor = pFore;
            // Posiciona o foco no nome
            txtNome.Focus();
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            // Verificar se o email foi digitado corretamente
            if (!sValidar.ValidarEmail(txtEmail.Text))
                MessageBox.Show("Digite um email válido para gerar o pagamento!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite que só desja digitado números e uma virgula e um sinal de negativo
            e.Handled = sValidar.AjustarValorMonetario(txtValor, e, true);
        }

        private void txtValor_Leave(object sender, EventArgs e)
        {
            // Valida o valor, formata e mascara com as casas decimais
            txtValor.Text = sValidar.ValidarMoeda(txtValor.Text);
        }

        private void txtNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite que só seja digitado números
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void txtDdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite que só seja digitado números
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void frPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void btnGerar_Click_1(object sender, EventArgs e)
        {

        }

        private void frPrincipal_Load_1(object sender, EventArgs e)
        {

        }
    }
}
