using linway_app.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormImprimirRecibo : Form
    {
        private int NotaId;
        private bool impresa;
        List<Recibo> listaRecibos = new List<Recibo>();
        private Bitmap memoryImage;
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public FormImprimirRecibo()
        {
            InitializeComponent();
        }

        private void FormImprimirRecibo_Load(object sender, EventArgs e)
        {
        }

        void CargarRecibos()
        {
            //listaRecibos = GetData.GetReceipt();
        }

        void GuardarRecibos()
        {
            //SetData.SetReceipt(listaRecibos);
        }

        public void Rellenar_Datos(Recibo elRecibo)
        {
            lFecha.Text = elRecibo.Fecha;
            //NotaId = elRecibo.Id;
            string elcodigo = elRecibo.Id.ToString();
            for (int i = elRecibo.Id.ToString().Length; i < 5; i++)
            {
                elcodigo = "0" + elcodigo;
            }

            lCodigo.Text = elcodigo;
            int separador = elRecibo.DireccionCliente.IndexOf('-');
            lCalle.Text = elRecibo.DireccionCliente.Substring(0, separador);
            lLocalidad.Text = elRecibo.DireccionCliente.Substring(separador + 1);

            lTotal.Text = "$ " + elRecibo.ImporteTotal.ToString(".00");
            //foreach (DetalleRecibo drActual in elRecibo.ListaDetalles)
            //{
            //    label1.Text = label1.Text + drActual.Detalle + Environment.NewLine;
            //    label2.Text = label2.Text + drActual.Importe.ToString(".00") + Environment.NewLine;
            //}
            //impresa = elRecibo.Impresa;
        }

        /// IMPRIMIR

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
                MarcarImpresa();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Falló marcado como impreso: " + ee.Message);
            }

            try
            {
                CaptureScreen();
                try
                {
                    PrintDialog printDialog1 = new PrintDialog
                    {
                        Document = printDocument1
                    };
                    DialogResult result = printDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        printDocument1.Print();
                        Close();
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

        void MarcarImpresa()
        {
            if (!impresa)
            {
                // MessageBox.Show("1 " + listaRecibos.Count.ToString());
                CargarRecibos();
                // MessageBox.Show("2");
                var thisRecibo = listaRecibos.Find(x => x.Id == NotaId);
                thisRecibo.Impresa = 1;
                // MessageBox.Show("3 " + thisRecibo.Cliente + thisRecibo.Fecha + thisRecibo.Impresa.ToString());
                GuardarRecibos();
            }
        }
    }
}

