using linway_app.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using static linway_app.Forms.Delegates.DNotaDeEnvio;

namespace linway_app.Forms
{
    public partial class FormImprimirNota : Form
    {
        private bool impresa;
        private NotaDeEnvio NotaDeEnvio = new NotaDeEnvio();

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth,
            int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        
        private Bitmap memoryImage;

        public FormImprimirNota()
        {
            InitializeComponent();
        }
        void MarcarImpresa()
        {
            if (!impresa)
            {
                NotaDeEnvio.Impresa = 1;
                editNotaDeEnvio(NotaDeEnvio);
            }
        }
        public void Rellenar_Datos(NotaDeEnvio notaDeEnvio)
        {
            NotaDeEnvio = notaDeEnvio;
            try
            {
                lFecha.Text = notaDeEnvio.Fecha;
                string elcodigo = notaDeEnvio.Id.ToString();
                for (int i = elcodigo.Length; i < 5; i++)
                {
                    elcodigo = "0" + elcodigo;
                }
                lCodigo.Text = elcodigo;
                int separador = notaDeEnvio.Client.Direccion.IndexOf('-');
                lCalle.Text = notaDeEnvio.Client.Direccion.Substring(0, separador);
                lLocalidad.Text = notaDeEnvio.Client.Direccion.Substring(separador + 1);
                lTotal.Text = "$ " + notaDeEnvio.ImporteTotal.ToString(".00");
                if (notaDeEnvio.Client.Direccion.Contains("–"))
                    notaDeEnvio.Client.Direccion = notaDeEnvio.Client.Direccion.Replace("–", "-");

                foreach (ProdVendido pvActual in notaDeEnvio.ProdVendidos)
                {
                    label1.Text = label1.Text + pvActual.Cantidad.ToString() + Environment.NewLine;
                    label2.Text = label2.Text + pvActual.Descripcion + Environment.NewLine;
                    label3.Text = label3.Text + pvActual.Precio.ToString(".00") + Environment.NewLine;
                }
                impresa = notaDeEnvio.Impresa != 0;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error rellenando los datos: " + exc.Message);
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
                MessageBox.Show("Error al capturar pantalla para imprimir: " + e.Message);
            }
        }
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private void Imprimir_Click(object sender, System.EventArgs e)
        {
            button1.Visible = false;
            try
            {
                CaptureScreen();
            }
            catch (Exception h)
            {
                MessageBox.Show("Error al imprimir screen: " + h.Message);
            }
            PrintDialog printDialog1 = new PrintDialog { Document = printDocument1 };
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument1.Print();
                Close();
                MarcarImpresa();
            }
        }
    }
}
