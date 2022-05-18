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
        private readonly List<ProdVendido> _lstProdVendidos;
        public FormCrearNota()
        {
            InitializeComponent();
            _lstProdVendidos = new List<ProdVendido>();
        }
        private void FormCrearNota_Load(object sender, EventArgs e)
        {
            ActualizarGrid();
        }
        private void ActualizarGrid()
        {
            if (_lstProdVendidos != null)
            {
                var grid = new List<EProdVendido>();
                foreach (ProdVendido prodVendido in _lstProdVendidos)
                {
                    grid.Add(Form1.mapper.Map<EProdVendido>(prodVendido));
                }
                dataGridView4.DataSource = grid;
                dataGridView4.Columns[0].Width = 28;
                dataGridView4.Columns[1].Width = 200;
            }
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
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
        private void TextBox2_TextChanged(object sender, EventArgs e)  // prod x nombr
        {
            if (textBox2.Text != "")
            {
                var producto = getProductoPorNombre(textBox2.Text);
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
            if (labelProductoId.Text != "")
            {
                try { long.Parse(labelProductoId.Text); } catch { return; };
                Producto producto = getProducto(long.Parse(labelProductoId.Text));
                if (producto == null || labelProductoId.Text == "" || textBox17.Text == "")
                {
                    label40.Text = "";
                    return;
                }
                label40.Text = (producto.Precio * int.Parse(textBox17.Text)).ToString();   // subtotal
            }
            else label40.Text = "";
        }
        private void LimpiarLista_Click(object sender, EventArgs e)
        {
            _lstProdVendidos.Clear();
            ActualizarGrid();
            label42.Text = "";
        }
        private void AnyadirProdVendidos_Click(object sender, EventArgs e)
        {
            if (labelClienteId.Text != "" && labelProductoId.Text != "")
            {
                try { long.Parse(labelProductoId.Text); int.Parse(textBox17.Text); } catch { return; };
                Producto producto = getProducto(long.Parse(labelProductoId.Text));
                if (producto == null) return;
                int cantidad = int.Parse(textBox17.Text);
                ProdVendido nuevoPV = new ProdVendido
                {
                    ProductoId = producto.Id,
                    Descripcion = label38.Text,
                    Cantidad = cantidad,
                    Precio = producto.Precio
                };

                if (producto.Tipo == TipoProducto.Saldo.ToString())
                {
                    if (producto.SubTipo == TipoSaldo.SaldoPendiente.ToString()) { }
                    else if (
                        producto.SubTipo == TipoSaldo.SaldoAFavor.ToString()
                        || producto.SubTipo == TipoSaldo.Devolucion.ToString()
                        || producto.SubTipo == TipoSaldo.Bonificacion.ToString()
                    )
                        nuevoPV.Precio = producto.Precio * -1;
                    else if (producto.SubTipo == TipoSaldo.AFacturar.ToString())
                        nuevoPV.Descripcion = label38.Text + textBox20.Text;
                    else
                        nuevoPV.Cantidad = int.Parse(textBox17.Text);
                }
                _lstProdVendidos.Add(nuevoPV);

                decimal impTotal = 0;
                foreach (ProdVendido prodVendido in _lstProdVendidos)
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
        }
        private void CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)       // enviar a hoja de reparto checkbox
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
            if (labelClienteId.Text != "" && labelClienteId.Text != "No encontrado")
            {
                var cliente = getClientePorDireccionExacta(label36.Text);
                if (cliente == null) return;

                NotaDeEnvio nuevaNota = new NotaDeEnvio
                {
                    ClienteId = cliente.Id,
                    Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                    Impresa = 0,
                    Detalle = extraerDetalleDeNotaDeEnvio(_lstProdVendidos),
                    ImporteTotal = extraerImporteDeNotaDeEnvio(_lstProdVendidos)
                };
                long notaId = addNotaDeEnvioReturnId(nuevaNota);
                if (notaId == 0)
                {
                    MessageBox.Show("Falló al procesar Nota nueva");
                    return;
                }
                foreach (ProdVendido prodVendido in _lstProdVendidos)
                {
                    prodVendido.NotaDeEnvioId = notaId;
                    addProdVendido(prodVendido);
                }
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
                    foreach (var prodVendido in _lstProdVendidos)
                    {
                        prodVendido.RegistroVentaId = registroId;
                        updateVenta(prodVendido, true);
                    }
                }
                if (checkBox1.Checked)      // imprimir checkbox
                {
                    var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                    form.Rellenar_Datos(nuevaNota);
                    form.Show();
                }
                if (checkBox3.Checked)     // enviar a hoja de reparto como pedido nuevo para X reparto
                {
                    Reparto reparto = getRepartoPorDiaYNombre(comboBox4.Text, comboBox3.Text);
                    Pedido pedido = reparto.Pedidos.ToList().Find(x => x.ClienteId != cliente.Id);
                    if (reparto == null || pedido == null) return;
                    addOrEditPedidoEnReparto(reparto, cliente, _lstProdVendidos);
                    foreach (ProdVendido prodVendido in _lstProdVendidos)
                    {
                        prodVendido.PedidoId = pedido.Id;
                        editProdVendido(prodVendido);
                    }
                }
                Close();
            }
            else MessageBox.Show("Verifique los campos");
        }
        private void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs e)
        {
            string diaDeReparto = comboBox4.Text;
            List<Reparto> repartos = getRepartosPorDia(diaDeReparto);   // partiendo del día seleccionado, buscar sus repartos
            comboBox3.DataSource = repartos;                            // combobox "día", necesito la lista de dias de rep
            comboBox3.DisplayMember = "Nombre";
            comboBox3.ValueMember = "Nombre";
        }
        private void CerrarBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
