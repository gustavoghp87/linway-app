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

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
           
        }

        int CodigoDeLaNota;

        bool impresa;
        public void rellenar_Datos(Recibo elRecibo)
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
            foreach (detalleRecibo drActual in elRecibo.detalle)
            {
                label1.Text = label1.Text + drActual.detalle + Environment.NewLine;
                label2.Text = label2.Text + drActual.importe.ToString(".00") + Environment.NewLine;
            }
            impresa = elRecibo.Impresa;

        }

        /// IMPRIMIR
        /// 
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest,
            int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;
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
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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
                marcarImpresa();
                this.Close();
            }
        }


        const string direccionRecibos = "Recibos.bin";
        List<Recibo> listaRecibos = new List<Recibo>();
        void GuardarRecibos()
        {
            Stream archivoRecibos = File.Create(direccionRecibos);
            BinaryFormatter traductor = new BinaryFormatter();
            traductor.Serialize(archivoRecibos, listaRecibos);
            archivoRecibos.Close();
        }
        void abrirRecibos()
        {
            if (File.Exists(direccionRecibos))
            {
                Stream archivo = File.OpenRead(direccionRecibos);
                BinaryFormatter traductor = new BinaryFormatter();
                listaRecibos = (List<Recibo>)traductor.Deserialize(archivo);
                archivo.Close();
            }
        }
        void marcarImpresa()
        {
            if (!impresa)
            {
                abrirRecibos();
                listaRecibos.Find(x => x.Codigo == CodigoDeLaNota).Impresa = true;
                GuardarRecibos();
            }
        }

        

    }
}
