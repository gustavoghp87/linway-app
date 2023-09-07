using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<Producto> _lstProductosAAgregar;
        public FormCrearNota()
        {
            InitializeComponent();
            _lstProdVendidosAAgregar = new List<ProdVendido>();
            _lstProductosAAgregar = new List<Producto>();
        }
        private void FormCrearNota_Load(object sender, EventArgs ev)
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
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back) ev.Handled = true;
        }
        private void TextBox15_TextChanged(object sender, EventArgs ev)
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
        private void TextBox1_TextChanged(object sender, EventArgs ev)
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
        private void TextBox16_TextChanged(object sender, EventArgs ev)
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
        private void TextBox2_TextChanged(object sender, EventArgs ev)  // producto por nombre
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
        private void TextBox17_TextChanged(object sender, EventArgs ev)
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
        private void LimpiarLista_Click(object sender, EventArgs ev)
        {
            _lstProdVendidosAAgregar.Clear();
            _lstProductosAAgregar.Clear();
            ActualizarGrid();
            label42.Text = "";
        }
        private void AnyadirProdVendidos_Click(object sender, EventArgs ev)
        {
            if (labelClienteId.Text == "" || labelProductoId.Text == "") return;
            try { long.Parse(labelProductoId.Text); int.Parse(textBox17.Text); } catch { return; };
            Producto producto = getProducto(long.Parse(labelProductoId.Text));
            if (producto == null) return;
            int cantidad = int.Parse(textBox17.Text);

            bool exists = false;
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                if (exists || isSaldo(prodVendido.Producto)) continue;
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
                    Cantidad = cantidad,
                    Descripcion = label38.Text,
                    Precio = producto.Precio,
                    ProductoId = producto.Id
                };
                if (producto.Tipo == TipoProducto.Saldo.ToString() && producto.SubTipo != TipoSaldo.SaldoPendiente.ToString())
                {
                    if (isNegativePrice(producto))
                        nuevoPV.Precio = producto.Precio * -1;
                    else if (producto.SubTipo == TipoSaldo.AFacturar.ToString())
                        nuevoPV.Descripcion = label38.Text + textBox20.Text;
                }
                _lstProdVendidosAAgregar.Add(nuevoPV);
                _lstProductosAAgregar.Add(producto);
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
        private void CheckedChanged(object sender, EventArgs ev)
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
        private void ConfirmarCrearNota_Click(object sender, EventArgs ev)
        {
            if (labelClienteId.Text == "" || labelClienteId.Text == "No encontrado" || _lstProdVendidosAAgregar == null || _lstProdVendidosAAgregar.Count == 0)
            {
                MessageBox.Show("Verifique los campos");
                return;
            }

            Cliente cliente = getClientePorDireccionExacta(label36.Text);
            if (cliente == null)
            {
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
            bool success = addNotaDeEnvio(nuevaNota);
            if (!success || nuevaNota.Id == 0)
            {
                MessageBox.Show("Falló al procesar Nota nueva");
                return;
            }
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                prodVendido.NotaDeEnvioId = nuevaNota.Id;
            }
            success = addProdVendidos(_lstProdVendidosAAgregar);
            if (!success)
            {
                MessageBox.Show("No se agregaron Productos Vendidos");
            }

            if (checkBox4.Checked)      // agregar productos vendidos a lista de registros y a lista de ventas
            {
                RegistroVenta nuevoRegistro = new RegistroVenta
                {
                    ClienteId = cliente.Id,
                    Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                    NombreCliente = cliente.Direccion
                };
                success = addRegistroVenta(nuevoRegistro);
                if (!success || nuevoRegistro.Id == 0)
                {
                    MessageBox.Show("Falló agregar Registro de Venta");
                }
                else
                {
                    var prodVendidosAEditar = new List<ProdVendido>();
                    foreach (var prodVendido in _lstProdVendidosAAgregar)
                    {
                        prodVendido.RegistroVentaId = nuevoRegistro.Id;
                        prodVendidosAEditar.Add(prodVendido);
                        _lstProdVendidosAAgregar.Find(x => x.Id == prodVendido.Id).RegistroVentaId = nuevoRegistro.Id;  // para que el siguiente checkbox no pise los cambios
                    }
                    success = editProdVendidos(prodVendidosAEditar);
                    if (!success)
                    {
                        MessageBox.Show("No se pudieron modificar los Productos Vendidos para agregarlos al Registro de Ventas");
                    }
                    updateVentasDesdeProdVendidos(prodVendidosAEditar, true);
                }
            }

            if (checkBox3.Checked)     // enviar a hoja de reparto
            {
                Reparto reparto = getRepartoPorDiaYNombre(comboBox4.Text, comboBox3.Text);
                if (reparto == null) {
                    MessageBox.Show("Falló Reparto al enviar al Reparto");
                    return;
                }
                long pedidoId = addPedidoIfNotExistsAndReturnId(reparto.Id, cliente.Id);
                if (pedidoId == 0)
                {
                    MessageBox.Show("Falló al agregar Pedido");
                }
                Pedido pedido = reparto.Pedidos.ToList().Find(p => p.Id == pedidoId);
                //if (pedidoId == 0 || pedido == null)
                //{
                //    MessageBox.Show("Falló Pedido al enviar al Reparto");
                //    return;
                //}
                var prodVendidosAEditar = new List<ProdVendido>();
                foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                {
                    prodVendido.PedidoId = pedidoId;
                    prodVendidosAEditar.Add(prodVendido);
                }
                // pedido.Entregar = 1;
                //bool success = editPedidos(new List<Pedido>() { pedido });
                success = editProdVendidos(prodVendidosAEditar);
                if (!success)
                {
                    MessageBox.Show("No se pudieron modificar los Productos Vendidos para incluirlos al Reparto");
                }
                Pedido pedidoActualizado = updatePedido(pedido, true);
                if (pedidoActualizado == null)
                {
                    MessageBox.Show("No se actualizó el Pedido");
                }
            }

            if (checkBox1.Checked)      // imprimir
            {
                nuevaNota.Cliente = cliente;  // para que no falle la dirección
                foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
                {
                    prodVendido.Producto = _lstProductosAAgregar.Find(p => p.Id == prodVendido.ProductoId);  // para que no falten los productos
                }
                nuevaNota.ProdVendidos = _lstProdVendidosAAgregar;
                var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nuevaNota);
                form.Show();
            }

            Close();
        }
        private void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs ev)
        {
            List<Reparto> repartos = getRepartosPorDia(comboBox4.Text);
            if (repartos == null) return;
            comboBox3.DataSource = repartos;
            comboBox3.DisplayMember = "Nombre";
            comboBox3.ValueMember = "Nombre";
        }
        private void CerrarBtn_Click(object sender, EventArgs ev)
        {
            Close();
        }
    }
}
