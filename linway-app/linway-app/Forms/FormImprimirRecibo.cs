using linway_app.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DRecibo;

namespace linway_app.Forms
{
    public partial class FormImprimirRecibo : Form
    {
        private Recibo _recibo;
        private Bitmap memoryImage;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth,
            int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public FormImprimirRecibo()
        {
            InitializeComponent();
            _recibo = new Recibo();
        }
        public void Rellenar_Datos(Recibo recibo)
        {
            _recibo = recibo;
            lFecha.Text = invertirFecha(_recibo.Fecha);
            string elcodigo = _recibo.Id.ToString();
            for (int i = elcodigo.Length; i < 5; i++)
            {
                elcodigo = "0" + elcodigo;
            }
            lCodigo.Text = elcodigo;
            
            if (_recibo.DireccionCliente.Contains("-"))
            {
                int separador = _recibo.DireccionCliente.IndexOf('-');
                lCalle.Text = _recibo.DireccionCliente.Substring(0, separador);
                lLocalidad.Text = _recibo.DireccionCliente.Substring(separador + 1);
            }
            else
            {
                lCalle.Text = _recibo.DireccionCliente;
                lLocalidad.Text = " ";
            }

            lTotal.Text = "$ " + _recibo.ImporteTotal.ToString(".00");
            foreach (DetalleRecibo detalle in _recibo.DetalleRecibos)
            {
                label1.Text = label1.Text + detalle.Detalle + Environment.NewLine;
                label2.Text = label2.Text + detalle.Importe.ToString(".00") + Environment.NewLine;
            }
        }
        private void MarcarImpresa()
        {
            if (_recibo.Impreso == 0)
            {
                _recibo.Impreso = 1;
                editRecibo(_recibo);
            }
        }
        private void CaptureScreen()
        {
            try
            {
                Graphics mygraphics = CreateGraphics();
                Size s = Size;
                memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                IntPtr dc1 = mygraphics.GetHdc();
                IntPtr dc2 = memoryGraphics.GetHdc();
                BitBlt(dc2, 0, 0, ClientRectangle.Width, ClientRectangle.Height, dc1, 0, 0, 13369376);
                mygraphics.ReleaseHdc(dc1);
                memoryGraphics.ReleaseHdc(dc2);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Falló captura de pantalla:", exception.Message);
            }
        }
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            CaptureScreen();
            try
            {
                PrintDialog printDialog1 = new PrintDialog { Document = printDocument1 };
                DialogResult result = printDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    printDocument1.Print();
                    Close();
                }
                MarcarImpresa();
            }
            catch (Exception eee)
            {
                MessageBox.Show("Falló impresión: " + eee.Message);
            }
        }
    }
}

