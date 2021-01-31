using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace linway_app
{
    public partial class FormImprimirNota : Form
    {
        int CodigoDeLaNota;
        bool impresa;
        const string direccionNotas = @"Base de datos\NotasDeEnvio.bin";
        List<NotaDeEnvio> notasEnvio = new List<NotaDeEnvio>();
        
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;

        public FormImprimirNota()
        {
            InitializeComponent();
        }

        private void FormImprimirNota_Load(object sender, EventArgs e)
        {
            CargarNotas();
        }

        void CargarNotas()
        {
            if (File.Exists(direccionNotas))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccionNotas);
                    BinaryFormatter traductor = new BinaryFormatter();
                    notasEnvio = (List<NotaDeEnvio>) traductor.Deserialize(archivo);
                    archivo.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer las notas de envío: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("No existe el archivo Notas de Envío en la carpeta Base de datos...");
            }
        }

        void GuardarNotas()
        {
            try
            {
                Stream archivo = File.Create(direccionNotas);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivo, notasEnvio);
                archivo.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar las notas de envío: " + e.Message);
            }
        }

        public void Rellenar_Datos(NotaDeEnvio laNota)
        {
            try
            {
                if (laNota.Cliente.Contains("–")) laNota.Cliente = laNota.Cliente.Replace("–", "-");
                //MessageBox.Show(laNota.Cliente);
                //MessageBox.Show(laNota.Cliente.IndexOf('-').ToString());
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
                lLocalidad.Text = laNota.Cliente.Substring(separador + 1);
                lTotal.Text = "$ " + laNota.ImporteTotal.ToString(".00");

                foreach (ProdVendido pvActual in laNota.Productos)
                {
                    label1.Text = label1.Text + pvActual.Cantidad.ToString() + Environment.NewLine;
                    label2.Text = label2.Text + pvActual.Descripcion + Environment.NewLine;
                    label3.Text = label3.Text + pvActual.Subtotal.ToString(".00") + Environment.NewLine;
                }
                impresa = laNota.Impresa;
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
                Graphics mygraphics = this.CreateGraphics();
                Size s = this.Size;
                memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                IntPtr dc1 = mygraphics.GetHdc();
                IntPtr dc2 = memoryGraphics.GetHdc();
                BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
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

        private void button1_Click(object sender, System.EventArgs e)
        {
            button1.Visible = false;
            try
            {
                CaptureScreen();
                //MessageBox.Show("Pantalla capturada");
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

        void MarcarImpresa()
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
