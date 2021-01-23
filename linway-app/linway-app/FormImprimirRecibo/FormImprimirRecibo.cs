using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace linway_app
{
    public partial class FormImprimirRecibo : Form
    {

        const string direccionRecibos = "Recibos.bin";
        List<Recibo> listaRecibos = new List<Recibo>();
        int CodigoDeLaNota;
        bool impresa;
        private Bitmap memoryImage;

        public FormImprimirRecibo()
        {
            InitializeComponent();
        }

        private void FormImprimirRecibo_Load(object sender, EventArgs e)
        {
        }
        
        public void Rellenar_Datos(Recibo elRecibo)
        {
            lFecha.Text = elRecibo.Fecha;
            CodigoDeLaNota = elRecibo.Codigo;
            string elcodigo = elRecibo.Codigo.ToString();
            for (int i = elRecibo.Codigo.ToString().Length; i < 5; i++)
            {
                elcodigo = "0" + elcodigo;
            }

            lCodigo.Text = elcodigo;
            int separador = elRecibo.Cliente.IndexOf('-');
            lCalle.Text = elRecibo.Cliente.Substring(0, separador);
            lLocalidad.Text = elRecibo.Cliente.Substring(separador + 1);

            lTotal.Text = "$ " + elRecibo.ImporteTotal.ToString(".00");
            foreach (DetalleRecibo drActual in elRecibo.detalle)
            {
                label1.Text = label1.Text + drActual.detalle + Environment.NewLine;
                label2.Text = label2.Text + drActual.importe.ToString(".00") + Environment.NewLine;
            }
            impresa = elRecibo.Impresa;

        }

        /// IMPRIMIR
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest,
            int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        private void CaptureScreen()
        {
            Graphics mygraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height,
                mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(
                memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width,
                this.ClientRectangle.Height, dc1, 0, 0,
                13369376);
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
            CaptureScreen();
            PrintDialog printDialog1 = new PrintDialog();
            printDialog1.Document = printDocument1;
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument1.Print();
                MarcarImpresa();
                this.Close();
            }
        }

        void GuardarRecibos()
        {
            try
            {
                Stream archivoRecibos = File.Create(direccionRecibos);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivoRecibos, listaRecibos);
                archivoRecibos.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar los recibos:" + e.Message);
            }
        }

        void AbrirRecibos()
        {
            if (File.Exists(direccionRecibos))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccionRecibos);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaRecibos = (List<Recibo>) traductor.Deserialize(archivo);
                    archivo.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer los recibos para imprimir:" + e.Message);
                }
            }
        }

        void MarcarImpresa()
        {
            if (!impresa)
            {
                AbrirRecibos();
                listaRecibos.Find(x => x.Codigo == CodigoDeLaNota).Impresa = true;
                GuardarRecibos();
            }
        }
    }
}

