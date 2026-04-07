using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using Models.Enums;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormImprimirNota : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private NotaDeEnvio _notaDeEnvio = new NotaDeEnvio();
        private Bitmap memoryImage;
        private readonly IServiceScope _scope;
        public FormImprimirNota()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        public void Rellenar_Datos(NotaDeEnvio notaDeEnvio)
        {
            _notaDeEnvio = notaDeEnvio;
            try
            {
                lFecha.Text = Helpers.InvertirFecha(_notaDeEnvio.Fecha);
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
                lTotal.Text = "$ " + _notaDeEnvio.ProdVendidos.ToList().Sum(prodVendido => prodVendido.Cantidad * prodVendido.Precio).ToString(".00");
                foreach (ProdVendido pvActual in _notaDeEnvio.ProdVendidos)
                {
                    if (pvActual.Producto.Tipo == TipoProducto.Saldo.ToString())
                    {
                        label1.Text += Environment.NewLine;
                    }
                    else
                    {
                        label1.Text += pvActual.Cantidad.ToString() + Environment.NewLine;
                    }
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
        private async Task MarcarImpresa()
        {
            if (_notaDeEnvio.Impresa == 1)
            {
                return;
            }
            _notaDeEnvio.Impresa = 1;
            await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    servicesContext.NotaDeEnvioServices.Edit(_notaDeEnvio);
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo marcar la Nota de Envío como Imprimida",
                null
            );
        }
        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs ev)
        {
            ev.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private async void Imprimir_Click(object sender, EventArgs ev)
        {
            button1.Visible = false;
            try  // Capture Screen
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
                button1.Visible = true;
                Logger.LogException(e);
                MessageBox.Show("Error al imprimir screen: " + e.Message);
                return;
            }
            PrintDialog printDialog1 = new PrintDialog { Document = printDocument1 };
            DialogResult result = printDialog1.ShowDialog();
            if (result != DialogResult.OK)
            {
                button1.Visible = true;
                MessageBox.Show("Algo falló al generar Diálogo");
                return;
            }
            printDocument1.Print();
            await MarcarImpresa();
            Close();
        }
    }
}
