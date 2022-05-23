using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DNotaDeEnvio;
using static linway_app.Services.Delegates.DPedido;
using static linway_app.Services.Delegates.DProducto;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DRegistroVenta;
using static linway_app.Services.Delegates.DReparto;
using static linway_app.Services.Delegates.DVentas;

namespace linway_app.Forms
{
    public partial class FormCrearNota : Form
    {
        private readonly List<ProdVendido> _lstProdVendidosAAgregar;
        public FormCrearNota()
        {
            InitializeComponent();
            _lstProdVendidosAAgregar = new List<ProdVendido>();
        }
        private void FormCrearNota_Load(object sender, EventArgs e)
        {
            ActualizarGrid();
        }
        private void ActualizarGrid()
        {
            if (_lstProdVendidosAAgregar == null) return;
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                grid.Add(Form1.mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView4.DataSource = grid;
            dataGridView4.Columns[0].Width = 28;
            dataGridView4.Columns[1].Width = 200;
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
        }
        private void TextBox15_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (textBox15.Text != "")
            {
                try { long.Parse(textBox15.Text); } catch { return; };
                Cliente cliente = getCliente(long.Parse(textBox15.Text));
                if (cliente == null)
                {
                    label36.Text = "No encontrado";
                    labelClienteId.Text = "";
                    return;
                }
                label36.Text = cliente.Direccion;
                labelClienteId.Text = cliente.Id.ToString();
            }
            else
            {
                label36.Text = "";
                labelClienteId.Text = "";
            }
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Cliente cliente = getClientePorDireccion(textBox1.Text);
                if (cliente == null)
                {
                    label36.Text = "No encontrado";
                    labelClienteId.Text = "";
                    return;
                }
                label36.Text = cliente.Direccion;
                labelClienteId.Text = cliente.Id.ToString();
            }
            else
            {
                label36.Text = "";
                labelClienteId.Text = "";
            }
        }
        private void TextBox16_TextChanged(object sender, EventArgs e)
        {
            if (textBox16.Text != "")
            {
                try { long.Parse(textBox16.Text); } catch { };
                var producto = getProducto(long.Parse(textBox16.Text));
                if (producto == null)
                {
                    label38.Text = "No encontrado";
                    labelProductoId.Text = "";
                    return;
                }
                label38.Text = producto.Nombre;
                labelProductoId.Text = producto.Id.ToString();
                if (label38.Text.Contains("actura")) textBox20.Visible = true;
                else textBox20.Visible = false;
            }
            else
            {
                label38.Text = "";
                labelProductoId.Text = "";
            }
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)  // producto por nombre
        {
            if (textBox2.Text != "")
            {
                Producto producto = getProductoPorNombre(textBox2.Text);
                if (producto == null)
                {
                    label38.Text = "No encontrado";
                    labelProductoId.Text = "";
                    return;
                }
                label38.Text = producto.Nombre;
                labelProductoId.Text = producto.Id.ToString();
                if (label38.Text.Contains("actura")) textBox20.Visible = true;
                else textBox20.Visible = false;
            }
            else
            {
                label38.Text = "";
                labelProductoId.Text = "";
            }
        }
        private void TextBox17_TextChanged(object sender, EventArgs e)
        {
            if (labelProductoId.Text == "")
            {
                label40.Text = "";
                return;
            }
            try { long.Parse(labelProductoId.Text); } catch { return; };
            Producto producto = getProducto(long.Parse(labelProductoId.Text));
            if (producto == null || labelProductoId.Text == "" || textBox17.Text == "")
            {
                label40.Text = "";
                return;
            }
            label40.Text = (producto.Precio * int.Parse(textBox17.Text)).ToString();   // subtotal
        }
        private void LimpiarLista_Click(object sender, EventArgs e)
        {
            _lstProdVendidosAAgregar.Clear();
            ActualizarGrid();
            label42.Text = "";
        }
        private void AnyadirProdVendidos_Click(object sender, EventArgs e)
        {
            if (labelClienteId.Text == "" || labelProductoId.Text == "") return;
            try { long.Parse(labelProductoId.Text); int.Parse(textBox17.Text); } catch { return; };
            Producto producto = getProducto(long.Parse(labelProductoId.Text));
            if (producto == null) return;
            int cantidad = int.Parse(textBox17.Text);

            bool exists = false;
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                if (exists) continue;
                if (prodVendido.ProductoId == producto.Id)
                {
                    exists = true;
                    prodVendido.Cantidad += cantidad;
                }
            }

            if (!exists)
            {
                var nuevoPV = new ProdVendido
                {
                    ProductoId = producto.Id,
                    Descripcion = label38.Text,
                    Cantidad = cantidad,
                    Precio = producto.Precio
                };
                if (producto.Tipo == TipoProducto.Saldo.ToString() && producto.SubTipo != TipoSaldo.SaldoPendiente.ToString())
                {
                    if (isNegativePrice(producto))
                        nuevoPV.Precio = producto.Precio * -1;
                    else if (producto.SubTipo == TipoSaldo.AFacturar.ToString())
                        nuevoPV.Descripcion = label38.Text + textBox20.Text;
                }
                _lstProdVendidosAAgregar.Add(nuevoPV);
            }

            decimal impTotal = 0;
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                impTotal += prodVendido.Precio * prodVendido.Cantidad;
            }
            label42.Text = impTotal.ToString();
            textBox20.Text = "";
            textBox20.Visible = false;
            textBox16.Text = "";
            textBox17.Text = "";
            textBox2.Text = "";
            label38.Text = "";
            label40.Text = "";
            labelProductoId.Text = "";
            ActualizarGrid();
        }
        private void CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)       // enviar a hoja de reparto
            {
                label33.Visible = true;
                label34.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
            }
            else
            {
                label33.Visible = false;
                label34.Visible = false;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
            }
        }
        private void ConfirmarCrearNota_Click(object sender, EventArgs e)
        {
            if (labelClienteId.Text == "" || labelClienteId.Text == "No encontrado" || _lstProdVendidosAAgregar == null || _lstProdVendidosAAgregar.Count == 0)
            {
                MessageBox.Show("Verifique los campos");
                return;
            }

            Form1.loadingForm.OpenIt();
            Cliente cliente = getClientePorDireccionExacta(label36.Text);
            if (cliente == null)
            {
                Form1.loadingForm.CloseIt();
                return;
            }
            NotaDeEnvio nuevaNota = new NotaDeEnvio
            {
                ClienteId = cliente.Id,
                Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                Impresa = 0,
                Detalle = extraerDetalleDeNotaDeEnvio(_lstProdVendidosAAgregar),
                ImporteTotal = extraerImporteDeNotaDeEnvio(_lstProdVendidosAAgregar)
            };
            long notaId = addNotaDeEnvioReturnId(nuevaNota);
            if (notaId == 0)
            {
                MessageBox.Show("Falló al procesar Nota nueva");
                Form1.loadingForm.CloseIt();
                return;
            }
            var prodVendidosAAgregar = new List<ProdVendido>();
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                prodVendido.NotaDeEnvioId = notaId;
                prodVendidosAAgregar.Add(prodVendido);
            }
            addProdVendidos(prodVendidosAAgregar);

            if (checkBox4.Checked)      // agregar productos vendidos a lista de registros y a lista de ventas
            {
                RegistroVenta nuevoRegistro = new RegistroVenta
                {
                    ClienteId = cliente.Id,
                    Cliente = cliente,
                    Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                    NombreCliente = cliente.Direccion
                };
                long registroId = addRegistroVentaReturnId(nuevoRegistro);
                var prodVendidosAEditar = new List<ProdVendido>();
                foreach (var prodVendido in _lstProdVendidosAAgregar)
                {
                    prodVendido.RegistroVentaId = registroId;
                    prodVendidosAEditar.Add(prodVendido);
                }
                editProdVendidos(prodVendidosAEditar);
                updateVentasDesdeProdVendidos(prodVendidosAEditar, true);
            }

            if (checkBox3.Checked)     // enviar a hoja de reparto
            {
                Reparto reparto = getRepartoPorDiaYNombre(comboBox4.Text, comboBox3.Text);
                if (reparto == null) {
                    Form1.loadingForm.CloseIt();
                    MessageBox.Show("Falló enviar al Reparto");
                    return;
                }
                long pedidoId = addPedidoIfNotExistsAndReturnId(reparto.Id, cliente.Id);
                Pedido pedido = getPedido(pedidoId);
                if (pedidoId == 0 || pedido == null)
                {
                    Form1.loadingForm.CloseIt();
                    MessageBox.Show("Falló enviar al Reparto");
                    return;
                }
                var prodVendidosAEditar = new List<ProdVendido>();
                foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                {
                    prodVendido.PedidoId = pedidoId;
                    prodVendidosAEditar.Add(prodVendido);
                }
                editProdVendidos(prodVendidosAEditar);
                pedido = getPedido(pedidoId);
                updatePedido(pedido);
            }

            if (checkBox1.Checked)      // imprimir
            {
                var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nuevaNota);
                form.Show();
            }

            Form1.loadingForm.CloseIt();
            Close();
        }
        private void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Reparto> repartos = getRepartosPorDia(comboBox4.Text);
            if (repartos == null) return;
            comboBox3.DataSource = repartos;
            comboBox3.DisplayMember = "Nombre";
            comboBox3.ValueMember = "Nombre";
        }
        private void CerrarBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
