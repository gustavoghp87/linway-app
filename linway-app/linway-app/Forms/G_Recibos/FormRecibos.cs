using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRecibos : Form
    {
        private decimal _subTo = 0;
        private List<Recibo> _lstRecibos = new List<Recibo>();
        private readonly List<DetalleRecibo> _lstDetallesAAgregar = new List<DetalleRecibo>();
        private readonly IServiceScope _scope;
        public FormRecibos()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        private async void FormRecibos_Load(object sender, EventArgs ev)
        {
            await Actualizar();
        }
        private async Task Actualizar()
        {
            await CargarRecibos();
            ActualizarGridRecibos(_lstRecibos);
            ActualizarGridDetalles();
        }
        private async Task CargarRecibos()
        {
            var recibos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var reciboServices = sp.GetRequiredService<IReciboServices>();
                    return await reciboServices.GetRecibosAsync();
                },
                "No se pudieron buscar los Recibos",
                null
            );
            if (recibos == null)
            {
                return;
            }
            _lstRecibos = recibos;
        }
        private void ActualizarGridRecibos(ICollection<Recibo> lstRecibos)
        {
            if (lstRecibos == null)
            {
                return;
            }
            var grid1 = new List<ERecibo>();
            foreach (Recibo recibo in lstRecibos)
            {
                grid1.Add(Form1.Mapper.Map<ERecibo>(recibo));
            }
            dataGridView1.DataSource = grid1;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[2].Width = 350;
            dataGridView1.Columns[3].Width = 80;
            lCantRecibos.Text = lstRecibos.Count.ToString() + " recibos.";
        }
        private void ActualizarGridDetalles()
        {
            if (_lstDetallesAAgregar == null)
            {
                return;
            }
            var grid2 = new List<EDetalleRecibo>();
            foreach (DetalleRecibo detalleRecibo in _lstDetallesAAgregar)
            {
                grid2.Add(Form1.Mapper.Map<EDetalleRecibo>(detalleRecibo));
            }
            dataGridView2.DataSource = grid2;
            dataGridView2.Columns[0].Width = 140;
        }
        private void RadioButton1_CheckedChanged(object sender, EventArgs ev)
        {
            textBox7.Enabled = radioButton4.Checked;
        }
        private void SoloNumeros(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private void TextBox8_KeyPress(object sender, KeyPressEventArgs ev)
        {
            bool IsDec = false;
            int nroDec = 0;
            if (ev.KeyChar == 8)
            {
                ev.Handled = false;
                return;
            }
            for (int i = 0; i < textBox8.Text.Length; i++)
            {
                if (textBox8.Text[i] == ',')
                {
                    IsDec = true;
                }
                if (IsDec && nroDec++ >= 2)
                {
                    ev.Handled = true;
                    return;
                }
            }
            if (ev.KeyChar >= 48 && ev.KeyChar <= 57)
            {
                ev.Handled = false;
            }
            else if (ev.KeyChar == 44)
            {
                ev.Handled = IsDec;
            }
            else
            {
                ev.Handled = true;
            }
        }
        private void LimpiarCampos()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            textBox7.Text = "";
            textBox8.Text = "";
        }
        private void Button8_Click(object sender, EventArgs ev)
        {
            LimpiarCampos();
            button6.Enabled = false;
            label18.Text = "0";
            ActualizarGridDetalles();
        }
    }
}
