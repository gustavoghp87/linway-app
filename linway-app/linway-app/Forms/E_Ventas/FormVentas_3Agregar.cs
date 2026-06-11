using AppLinway.PresentationHelpers;
using AppServices.DTOs;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormVentas : Form
    {
        private Producto _productoAAgregar;
        private Cliente _clienteAgregar;
        private DiaReparto _diaRepartoAgregar;
        private Reparto _repartoAgregar;
        //private List<Reparto> _repartosAgregar;
        private readonly List<Venta> _lstAgregarVentas = new List<Venta>();
        private void ActualizarGrid5(ICollection<Venta> lstVentas)
        {
            var grid = new List<EVenta>();
            foreach (Venta venta in lstVentas)
            {
                grid.Add(Form1.Mapper.Map<EVenta>(venta));
            }
            grid = grid.OrderBy(x => x.Detalle).ToList();
            dataGridView5.DataSource = grid;
            dataGridView5.Columns[0].Width = 35;
            dataGridView5.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridView5.Columns[2].Width = 40;
            dataGridView5.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
        }
        private async void InputProductoId_TextChanged(object sender, EventArgs ev)  // producto por ID
        {
            _productoAAgregar = null;
            string numeroDeProducto = textBox12.Text;
            if (numeroDeProducto == "")
            {
                label28.Text = "";
                labelPrecio.Text = "";
                return;
            }
            if (!long.TryParse(numeroDeProducto, out long productoId))
            {
                return;
            }
            var producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    return await productoServices.GetPorIdAsync(productoId);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label28.Text = "No encontrado";
                labelPrecio.Text = "";
                return;
            }
            _productoAAgregar = producto;
            label28.Text = producto.Nombre;
            labelPrecio.Text = producto.Precio.ToString();
        }
        private async void TextBox4_TextChanged(object sender, EventArgs ev)  // producto por nombre
        {
            _productoAAgregar = null;
            string nombreDeProducto = textBox4.Text;
            if (textBox4.Text == "")
            {
                label28.Text = "";
                labelPrecio.Text = "";
                return;
            }
            var producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    return await productoServices.GetPorNombreAsync(nombreDeProducto);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label28.Text = "No encontrado";
                labelPrecio.Text = "";
                return;
            }
            _productoAAgregar = producto;
            label28.Text = producto.Nombre;
            labelPrecio.Text = producto.Precio.ToString();
        }
        private void Limpiar_Click(object sender, EventArgs ev)
        {
            _lstAgregarVentas.Clear();
            _clienteAgregar = null;
            _productoAAgregar = null;
            ActualizarGrid5(_lstAgregarVentas);
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }
        private void Anyadir_Click(object sender, EventArgs ev)
        {
            if (_productoAAgregar == null || !int.TryParse(textBox13.Text, out int cantidad) || cantidad == 0)
            {
                return;
            }
            var nuevaVenta = new Venta
            {
                ProductoId = _productoAAgregar.Id,
                Cantidad = cantidad,
                Producto = _productoAAgregar      // dejar para que cargue el nombre en el grid
            };
            _lstAgregarVentas.Add(nuevaVenta);
            ActualizarGrid5(_lstAgregarVentas);
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox4.Text = "";
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs ev)        // enviar a HDR
        {
            bool enviarAReparto = checkBox2.Checked;
            if (enviarAReparto)
            {
                label6.Visible = true;
                label7.Visible = true;
                comboBox2AgregarRegVentaRepartos.Visible = true;
                comboBox1AgregarRegVentaDias.Visible = true;
                label15.Visible = true;
                label20.Text = "";
                textBox19.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                label6.Visible = false;
                label7.Visible = false;
                comboBox2AgregarRegVentaRepartos.Visible = false;
                comboBox1AgregarRegVentaDias.Visible = false;
                label15.Visible = false;
                label20.Text = "";
                textBox19.Visible = false;
                textBox3.Visible = false;
                textBox19.Text = "";
                textBox3.Text = "";
            }
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)  // selecciona día de reparto
        {
            string dia = comboBox1AgregarRegVentaDias.Text;
            DiaReparto diaReparto = _dias.Find(x => x.Dia == dia);
            _diaRepartoAgregar = diaReparto;
            comboBox2AgregarRegVentaRepartos.DataSource = diaReparto.Repartos.ToList();
            comboBox2AgregarRegVentaRepartos.DisplayMember = "Nombre";
            comboBox2AgregarRegVentaRepartos.ValueMember = "Nombre";
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)  // selecciona reparto
        {
            string nombreReparto = comboBox2AgregarRegVentaRepartos.Text;
            List<Reparto> repartos = _diaRepartoAgregar.Repartos.ToList().Where(x => x.Nombre == nombreReparto).ToList();
            _repartoAgregar = repartos.Find(x => x.Nombre == nombreReparto);
        }
        private async void TextBox19_TextChanged(object sender, EventArgs ev)  // cliente por Id
        {
            _clienteAgregar = null;
            string numeroDeCliente = textBox19.Text;
            if (numeroDeCliente == "")
            {
                label20.Text = "";
                return;
            }
            if (!long.TryParse(numeroDeCliente, out long clienteId))
            {
                return;
            }
            var cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetPorIdAsync(clienteId);
                },
                "No se pudo buscar el cliente",
                null
            );
            if (cliente == null)
            {
                label20.Text = "No encontrado";
                return;
            }
            _clienteAgregar = cliente;
            label20.Text = cliente.Direccion;
        }
        private async void TextBox3_TextChanged(object sender, EventArgs ev)  // cliente por dirección
        {
            _clienteAgregar = null;
            string direccion = textBox3.Text;
            if (direccion == "")
            {
                label20.Text = "";
                return;
            }
            var cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetPorDireccionAsync(direccion);
                },
                "No se pudo buscar el cliente",
                null
            );
            if (cliente == null)
            {
                label20.Text = "No encontrado";
                return;
            }
            _clienteAgregar = cliente;
            label20.Text = cliente.Direccion;
        }
        private async void AgregarVenta_Click(object sender, EventArgs ev)
        {
            if (_lstAgregarVentas.Count == 0)
            {
                MessageBox.Show("No se han ingresado productos");
                return;
            }
            bool enviarAReparto = checkBox2.Checked;
            if (enviarAReparto && (_clienteAgregar == null || _repartoAgregar == null))
            {
                MessageBox.Show("Faltan el Cliente o el Reparto");
                return;
            }
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IAgregarRegistroVentaUseCase>();
                    return await useCase.ExecuteAsync(_lstAgregarVentas, _clienteAgregar, enviarAReparto, _repartoAgregar);
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
            _repartoAgregar = null;
            _clienteAgregar = null;
            _lstAgregarVentas.Clear();
            LimpiarPantalla();
            await Actualizar();
        }
    }
}
