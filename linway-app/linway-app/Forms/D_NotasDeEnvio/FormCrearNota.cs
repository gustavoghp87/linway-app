using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormCrearNota : Form
    {
        private readonly List<ProdVendido> _lstProdVendidosAAgregar = new List<ProdVendido>();
        private readonly List<Producto> _lstProductosAAgregar = new List<Producto>();
        private List<DiaReparto> _lstDias = new List<DiaReparto>();
        private Cliente _cliente;
        private Producto _producto;
        private Reparto _reparto;
        private Pedido _pedido;
        private readonly IServiceScope _scope;
        public FormCrearNota()
        {
            InitializeComponent();
            _scope = Program.LinwayServiceProvider.CreateScope();
            FormClosed += (s, e) => _scope.Dispose();
        }
        private async void FormCrearNota_Load(object sender, EventArgs ev)
        {
            ActualizarGrid();
            var diaRepartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    return await diaRepartoServices.GetAllAsync();
                },
                "No se pudieron buscar los Días de Reparto",
                null
            );
            if (diaRepartos == null)
            {
                return;
            }
            _lstDias = diaRepartos;
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back)
            {
                ev.Handled = true;
            }
        }
        private void EstablecerReparto()
        {
            if (_cliente == null || _reparto == null)
            {
                _pedido = null;
                return;
            }
            _pedido = _reparto.Pedidos.ToList().FirstOrDefault(x => x.ClienteId == _cliente.Id);
        }
        private async void ConfirmarCrearNota_Click(object sender, EventArgs ev)
        {
            if (_cliente == null || _lstProdVendidosAAgregar.Count == 0)
            {
                MessageBox.Show("Verifique los campos");
                return;
            }
            bool agregarProdVendidosARegistrosYVentas = checkBox4.Checked;
            bool enviarAHojaDeReparto = checkBox3EnviarA_HDR.Checked;
            bool imprimir = checkBox1Imprimir.Checked;
            if (enviarAHojaDeReparto && _reparto == null)
            {
                MessageBox.Show("Falta el Reparto");
                return;
            }
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IAgregarNotaDeEnvioUseCase>();
                    return await useCase.ExecuteAsync(_cliente, _lstProductosAAgregar, _lstProdVendidosAAgregar, _pedido, _reparto, agregarProdVendidosARegistrosYVentas, enviarAHojaDeReparto, imprimir);
                },
                "No se pudo realizar",
                this
            );
            if (resultado == null || !resultado.Success)
            {
                if (resultado?.ErrorMessage != null)
                {
                    MessageBox.Show(resultado.ErrorMessage);
                }
                return;
            }
            if (imprimir)  // imprimir
            {
                var form = Program.LinwayServiceProvider.GetRequiredService<FormImprimirNota>();
                var nuevaNotaDeEnvio = resultado.Data;
                form.Rellenar_Datos(nuevaNotaDeEnvio);
                form.Show(this);  // no necesita renovar scope porque el cambio a imprimido no se muestra en este form
            }
            _cliente = null;
            _pedido = null;
            _reparto = null;
            textBox15ClienteNumeroBusqueda.Text = "";
            textBox1ClienteDireccionBusqueda.Text = "";
            label36ClienteDireccion.Text = "";
            labelClienteId.Text = "";
            label33Dia.Visible = false;
            label34Nombre.Visible = false;
            comboBox3NombreDeReparto.Text = "";
            comboBox3NombreDeReparto.Visible = false;
            comboBox4DiaDeReparto.Visible = false;
            checkBox4.Checked = false;
            checkBox3EnviarA_HDR.Checked = false;
            checkBox1Imprimir.Checked = false;
            LimpiarLista();
        }
        private void CerrarBtn_Click(object sender, EventArgs ev)
        {
            Close();
        }
    }
}
