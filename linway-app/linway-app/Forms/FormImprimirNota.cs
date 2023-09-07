using Models;
using Models.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DNotaDeEnvio;
using static linway_app.Services.Delegates.DZGeneral;

namespace linway_app.Forms
{
    public partial class FormImprimirNota : Form
    {
        private NotaDeEnvio _notaDeEnvio;
        private Bitmap memoryImage;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth,
            int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        
        public FormImprimirNota()
        {
            InitializeComponent();
            _notaDeEnvio = new NotaDeEnvio();
        }
        public void Rellenar_Datos(NotaDeEnvio notaDeEnvio)
        {
            _notaDeEnvio = notaDeEnvio;
            try
            {
                lFecha.Text = invertirFecha(_notaDeEnvio.Fecha);
                string elcodigo = _notaDeEnvio.Id.ToString();
                for (int i = elcodigo.Length; i < 5; i++)
                {
                    elcodigo = "0" + elcodigo;
                }
                lCodigo.Text = elcodigo;
                //int separador = _notaDeEnvio.Client.Direccion.IndexOf('-');
                //lCalle.Text = _notaDeEnvio.Client.Direccion.Substring(0, separador);
                lCalle.Text = _notaDeEnvio.Cliente.Direccion;
                //lLocalidad.Text = nota_notaDeEnvioDeEnvio.Client.Direccion.Substring(separador + 1);
                lLocalidad.Text = " ";
                lTotal.Text = "$ " + _notaDeEnvio.ImporteTotal.ToString(".00");
                foreach (ProdVendido pvActual in _notaDeEnvio.ProdVendidos)
                {
                    if (pvActual.Producto.Tipo == TipoProducto.Saldo.ToString())
                        label1.Text += Environment.NewLine;
                    else
                        label1.Text += pvActual.Cantidad.ToString() + Environment.NewLine;
                    //label2.Text = label2.Text + pvActual.Descripcion + " ($" + pvActual.Precio.ToString(".00") + " c/u)" + Environment.NewLine;
                    label2.Text = label2.Text + pvActual.Descripcion + Environment.NewLine;
                    label3.Text = label3.Text + (pvActual.Precio * pvActual.Cantidad).ToString(".00") + Environment.NewLine;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show("Error rellenando los datos: " + e.Message);
            }
        }
        private void MarcarImpresa()
        {
            if (_notaDeEnvio.Impresa == 1) return;
            _notaDeEnvio.Impresa = 1;
            bool success = editNotaDeEnvio(_notaDeEnvio);
            if (!success)
            {
                MessageBox.Show("No se pudo marcar la Nota de Envío como Imprimida");
            }
        }

        /// IMPRIMIR
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
                MessageBox.Show("Error al capturar pantalla para imprimir: " + e.Message);
            }
        }
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {
            ev.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private void Imprimir_Click(object sender, System.EventArgs ev)
        {
            button1.Visible = false;
            try
            {
                CaptureScreen();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show("Error al imprimir screen: " + e.Message);
            }
            PrintDialog printDialog1 = new PrintDialog { Document = printDocument1 };
            DialogResult result = printDialog1.ShowDialog();
            if (result != DialogResult.OK)
            {
                MessageBox.Show("Algo falló al generar Diálogo");
                return;
            }
            printDocument1.Print();
            MarcarImpresa();
            Close();
        }
    }
}
