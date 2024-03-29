using Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DRecibo;
using static linway_app.Services.Delegates.DZGeneral;

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
                if (detalle.Detalle.Contains("Factura")) {
                    detalle.Detalle = detalle.Detalle.Replace("Factura", "Fc.");
                }
                label1.Text = label1.Text + detalle.Detalle + Environment.NewLine;
                label2.Text = label2.Text + detalle.Importe.ToString(".00") + Environment.NewLine;
            }
        }
        private void MarcarImpresa()
        {
            if (_recibo.Impreso == 1) return;
            _recibo.Impreso = 1;
            bool success = editRecibo(_recibo);
            if (!success)
            {
                MessageBox.Show("No se pudo marcar Recibo como Imprimido");
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
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show("Falló captura de pantalla:", e.Message);
            }
        }
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {
            ev.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private void Button1_Click(object sender, EventArgs ev)
        {
            button1.Visible = false;
            CaptureScreen();
            try
            {
                PrintDialog printDialog1 = new PrintDialog { Document = printDocument1 };
                DialogResult result = printDialog1.ShowDialog();
                if (result != DialogResult.OK)
                {
                    MessageBox.Show("Falló impresión en generación de Diálogo");
                    return;
                }
                printDocument1.Print();
                MarcarImpresa();
                Close();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show("Falló impresión: " + e.Message);
            }
        }
    }
}

