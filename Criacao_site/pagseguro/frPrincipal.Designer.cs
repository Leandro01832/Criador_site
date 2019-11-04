namespace PagSeguro
{
    partial class frPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNumeroTelefone = new System.Windows.Forms.TextBox();
            this.txtDdd = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.btnGerar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(346, 128);
            this.txtCode.MaxLength = 120;
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(341, 22);
            this.txtCode.TabIndex = 1020;
            this.txtCode.TabStop = false;
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(561, 103);
            this.txtValor.MaxLength = 13;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(126, 22);
            this.txtValor.TabIndex = 1011;
            this.txtValor.Text = "0,00";
            this.txtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(388, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 17);
            this.label7.TabIndex = 1018;
            this.label7.Text = "-";
            // 
            // txtNumeroTelefone
            // 
            this.txtNumeroTelefone.Location = new System.Drawing.Point(402, 103);
            this.txtNumeroTelefone.MaxLength = 9;
            this.txtNumeroTelefone.Name = "txtNumeroTelefone";
            this.txtNumeroTelefone.Size = new System.Drawing.Size(102, 22);
            this.txtNumeroTelefone.TabIndex = 1009;
            this.txtNumeroTelefone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDdd
            // 
            this.txtDdd.Location = new System.Drawing.Point(346, 103);
            this.txtDdd.MaxLength = 50;
            this.txtDdd.Name = "txtDdd";
            this.txtDdd.Size = new System.Drawing.Size(39, 22);
            this.txtDdd.TabIndex = 1007;
            this.txtDdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(346, 79);
            this.txtEmail.MaxLength = 120;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(341, 22);
            this.txtEmail.TabIndex = 1006;
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(346, 55);
            this.txtNome.MaxLength = 50;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(341, 22);
            this.txtNome.TabIndex = 1004;
            // 
            // lblMensagem
            // 
            this.lblMensagem.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensagem.ForeColor = System.Drawing.Color.Silver;
            this.lblMensagem.Location = new System.Drawing.Point(225, 229);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(462, 76);
            this.lblMensagem.TabIndex = 1016;
            this.lblMensagem.Text = "clique acima para consultar o status";
            this.lblMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnConsultar
            // 
            this.btnConsultar.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsultar.Location = new System.Drawing.Point(459, 159);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(228, 63);
            this.btnConsultar.TabIndex = 1015;
            this.btnConsultar.Text = "Consultar Status";
            this.btnConsultar.UseVisualStyleBackColor = true;
            // 
            // btnGerar
            // 
            this.btnGerar.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGerar.Location = new System.Drawing.Point(225, 159);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(228, 63);
            this.btnGerar.TabIndex = 1013;
            this.btnGerar.Text = "Gerar Pagamento";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click_1);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(222, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 22);
            this.label5.TabIndex = 1014;
            this.label5.Text = "Código Gerado:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(222, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 22);
            this.label4.TabIndex = 1012;
            this.label4.Text = "Telefone:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(222, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 22);
            this.label3.TabIndex = 1010;
            this.label3.Text = "E-mail:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(222, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 22);
            this.label2.TabIndex = 1008;
            this.label2.Text = "Nome Completo:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(224, 336);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(465, 15);
            this.textBox1.TabIndex = 1017;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "https://pagseguro.uol.com.br/v2/guia-de-integracao/consulta-de-transacoes-por-cod" +
    "igo.html#!v2-item-consulta-de-transacoes-por-codigo-parametros-resposta";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(221, 319);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(457, 17);
            this.label1.TabIndex = 1005;
            this.label1.Text = "Link PagSeguro com a documentação (copie e cole no seu navegador)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(510, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 22);
            this.label8.TabIndex = 1019;
            this.label8.Text = "Valor:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 458);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtNumeroTelefone);
            this.Controls.Add(this.txtDdd);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.btnConsultar);
            this.Controls.Add(this.btnGerar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Name = "frPrincipal";
            this.Text = "frPrincipal";
            this.Load += new System.EventHandler(this.frPrincipal_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNumeroTelefone;
        private System.Windows.Forms.TextBox txtDdd;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Button btnGerar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
    }
}