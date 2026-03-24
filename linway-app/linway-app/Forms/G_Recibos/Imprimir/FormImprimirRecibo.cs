using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormImprimirRecibo : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;
        private Recibo _recibo = new Recibo();
        private readonly IServiceScope _scope;
        public FormImprimirRecibo()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        public void Rellenar_Datos(Recibo recibo)
        {
            _recibo = recibo;
            lFecha.Text = Helpers.InvertirFecha(_recibo.Fecha);
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
                if (detalle.Detalle.Contains("Factura"))
                {
                    detalle.Detalle = detalle.Detalle.Replace("Factura", "Fc.");
                }
                label1.Text = label1.Text + detalle.Detalle + Environment.NewLine;
                label2.Text = label2.Text + detalle.Importe.ToString(".00") + Environment.NewLine;
            }
        }
        private async Task MarcarImpresa()
        {
            if (_recibo.Impreso == 1)
            {
                return;
            }
            _recibo.Impreso = 1;
            await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var reciboServices = sp.GetRequiredService<IReciboServices>();
                    reciboServices.EditRecibo(_recibo);
                    return await savingServices.SaveAsync();
                },
                "No se pudo marcar Recibo como Imprimido",
                null
            );
        }
        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs ev)
        {
            ev.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private async void Button1_Click(object sender, EventArgs ev)
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
                Logger.LogException(e);
                MessageBox.Show("Falló captura de pantalla:", e.Message);
                return;
            }
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
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show("Falló impresión: " + e.Message);
                return;
            }
            await MarcarImpresa();
            Close();
        }
    }
}

