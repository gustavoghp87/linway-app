using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormImprimirRecibo : Form
    {
        private long impresa = 0;
        private long NotaId = 0;
        private List<Recibo> _lstRecibos = new List<Recibo>();
        //private List<DetalleRecibo> _lstDetalles = new List<DetalleRecibo>();
        private readonly IServicioRecibo _servRecibo;
        private readonly IServicioDetalleRecibo _servDetalleRecibo;
        private Bitmap memoryImage;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth,
            int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public FormImprimirRecibo(IServicioRecibo servRecibo
            //, IServicioDetalleRecibo servDetalleRecibo
        )
        {
            InitializeComponent();
            _servRecibo = servRecibo;
            //_servDetalleRecibo = servDetalleRecibo;
        }

        private void FormImprimirRecibo_Load(object sender, EventArgs e)
        {
            CargarRecibos();
            CargarDetalles();
        }
        void CargarRecibos()
        {
            _lstRecibos = _servRecibo.GetAll();
        }
        void CargarDetalles()
        {
            //_lstDetalles = _servDetalleRecibo.GetAll();
        }
        private void EditarRecibo(Recibo recibo)
        {
            _servRecibo.Edit(recibo);
        }
        public void Rellenar_Datos(Recibo recibo)
        {
            lFecha.Text = recibo.Fecha;
            string elcodigo = recibo.Id.ToString();
            for (int i = elcodigo.Length; i < 5; i++)
            {
                elcodigo = "0" + elcodigo;
            }
            lCodigo.Text = elcodigo;
            int separador = recibo.DireccionCliente.IndexOf('-');
            lCalle.Text = recibo.DireccionCliente.Substring(0, separador);
            lLocalidad.Text = recibo.DireccionCliente.Substring(separador + 1);
            lTotal.Text = "$ " + recibo.ImporteTotal.ToString(".00");
            foreach (DetalleRecibo detalle in recibo.DetalleRecibos)
            {
                label1.Text = label1.Text + detalle.Detalle + Environment.NewLine;
                label2.Text = label2.Text + detalle.Importe.ToString(".00") + Environment.NewLine;
            }
            impresa = recibo.Impresa;
            NotaId = recibo.Id;
        }
        private void MarcarImpresa()
        {
            if (impresa == 0)
            {
                CargarRecibos();
                var recibo = _lstRecibos.Find(x => x.Id == NotaId);
                recibo.Impresa = 1;
                EditarRecibo(recibo);
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

