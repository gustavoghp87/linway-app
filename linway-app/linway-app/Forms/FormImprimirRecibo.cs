using linway_app.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
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
            lFecha.Text = _recibo.Fecha;
            string elcodigo = _recibo.Id.ToString();
            for (int i = elcodigo.Length; i < 5; i++)
            {
                elcodigo = "0" + elcodigo;
            }
            lCodigo.Text = elcodigo;
            //int separador = recibo.DireccionCliente.IndexOf('-');
            //lCalle.Text = recibo.DireccionCliente.Substring(0, separador);
            lCalle.Text = " ";
            //lLocalidad.Text = recibo.DireccionCliente.Substring(separador + 1);
            lLocalidad.Text = _recibo.DireccionCliente;
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
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            button1.Visible = false;
            try
            {
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

                    try
                    {
                        MarcarImpresa();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("Falló marcado como impreso: " + ee.Message);
                    }
                }
                catch (Exception eee)
                {
                    MessageBox.Show("Falló impresión: " + eee.Message);
                }
            }
            catch (Exception eeee)
            {
                MessageBox.Show("Falló captura de datos: " + eeee.Message);
            }
        }
    }
}

