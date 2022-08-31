using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Lexico;
using Compilador.Sintatico;

namespace Compilador
{
    public partial class FormMain : Form
    {
        private const bool IgnorarEspacos = true;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            // Limpa a lista de tokens e do resultado sintático.
            this.lstTokens.Items.Clear();
            this.lstSintatico.Items.Clear();

            if (this.rtxtCode.Text.Length == 0)
            {
                MessageBox.Show("Sem dados de entrada!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Cria um vetor com as linhas:
            string[] entrada = this.rtxtCode.Text.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

            if (entrada.Length == 0)
            {
                MessageBox.Show("Sem dados de entrada!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<TokenClass> listaTokens = new List<TokenClass>();

            try
            {
                // Executa a análise léxica usando o vetor de linhas
                listaTokens = LexicoClass.ProcessaEntrada(entrada);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Adiciona à lista os novos tokens encontrados, se existirem:
            foreach (var token in listaTokens)
                this.lstTokens.Items.Add(token);

            try
            {
                // Executa a análise sintática usando a lista de tokens encontrados
                AnalizadorDescendenteRecursivo.Verificar(listaTokens, this.lstSintatico);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
