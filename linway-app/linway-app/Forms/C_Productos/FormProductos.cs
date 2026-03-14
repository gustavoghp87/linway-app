using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormProductos : Form
    {
        private readonly IServiceScope _scope;
        public FormProductos()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
            //
            ActiveControl = textBox2;
        }
        private void SoloImporte_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (ev.KeyChar == 8)
            {
                ev.Handled = false;
                return;
            }
            bool IsDec = false;
            int nroDec = 0;
            for (int i = 0; i < textBox7.Text.Length; i++)
            {
                if (textBox7.Text[i] == ',')
                {
                    IsDec = true;
                }
                if (IsDec && nroDec++ >= 2)
                {
                    ev.Handled = true;
                    return;
                }
            }
            if (ev.KeyChar >= 48 && ev.KeyChar <= 57)
            {
                ev.Handled = false;
            }
            else if (ev.KeyChar == 44)
            {
                ev.Handled = IsDec;
            }
            else
            {
                ev.Handled = true;
            }
        }
        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private void SalirBtn_Click(object sender, EventArgs ev)
        {
            Close();
        }
    }
}
