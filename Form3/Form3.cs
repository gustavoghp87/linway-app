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
    public partial class Form3 : Form
    {
        int CodigoDeLaNota;
        bool impresa;
        
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        public void rellenar_Datos(NotaDeEnvio laNota)
        {
            lFecha.Text = laNota.Fecha;
            CodigoDeLaNota = laNota.Codigo;
            string elcodigo = laNota.Codigo.ToString();
            for (int i = laNota.Codigo.ToString().Length; i < 5; i++)
            {
                elcodigo = "0" + elcodigo;
            }

            lCodigo.Text = elcodigo;
            int separador = laNota.Cliente.IndexOf('-');
            lCalle.Text = laNota.Cliente.Substring(0, separador);
            lLocalidad.Text = laNota.Cliente.Substring(separador+1);

            lTotal.Text = "$ " +laNota.ImporteTotal.ToString(".00");
            foreach (ProdVendido pvActual in laNota.Productos)
            {
                label1.Text = label1.Text + pvActual.Cantidad.ToString() + Environment.NewLine ;
                label2.Text = label2.Text + pvActual.Descripcion + Environment.NewLine;
                label3.Text =  label3.Text +pvActual.Subtotal.ToString(".00") + Environment.NewLine;
            }
            impresa = laNota.Impresa;
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
                this.Close();
                marcarImpresa();
            }
        }

       

        const string direccionNotas = "NotasDeEnvio.bin";
        List<NotaDeEnvio> notasEnvio = new List<NotaDeEnvio>();

        void CargarNotas()
        {
            if (File.Exists(direccionNotas))
            {
                Stream archivoNotas = File.OpenRead(direccionNotas);
                BinaryFormatter traductor = new BinaryFormatter();
                notasEnvio = (List<NotaDeEnvio>)traductor.Deserialize(archivoNotas);
                archivoNotas.Close();

            }
        }

        void GuardarNotas()
        {
            Stream archivoNotas = File.Create(direccionNotas);
            BinaryFormatter traductor = new BinaryFormatter();
            traductor.Serialize(archivoNotas, notasEnvio);
            archivoNotas.Close();
        }

        void marcarImpresa()
        {
            if (!impresa)
            {
                CargarNotas();
                notasEnvio.Find(x => x.Codigo == CodigoDeLaNota).Impresa = true;
                GuardarNotas();
            }
        }
    }
}
