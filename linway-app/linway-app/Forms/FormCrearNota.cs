using linway_app.Models;
using linway_app.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DClientes;
using static linway_app.Services.Delegates.DNotaDeEnvio;
using static linway_app.Services.Delegates.DProductos;
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
            Actualizar();
        }
        private void Actualizar()
        {
            if (_lstProdVendidos == null || _lstProdVendidos.Count == 0) return;
            dataGridView4.DataSource = _lstProdVendidos.ToArray();
            dataGridView4.Columns[0].Visible = false;
            dataGridView4.Columns[1].Width = 30;
            dataGridView4.Columns[2].Visible = false;
            dataGridView4.Columns[3].Visible = false;
            dataGridView4.Columns[4].Visible = false;
            dataGridView4.Columns[5].Width = 30;
            dataGridView4.Columns[6].Visible = true;
            dataGridView4.Columns[7].Visible = true;
            dataGridView4.Columns[8].Visible = false;
            dataGridView4.Columns[9].Visible = false;
            dataGridView4.Columns[10].Visible = false;
            dataGridView4.Columns[11].Visible = false;
            dataGridView4.Columns[12].Visible = false;
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
        private void textBox15_TextChanged(object sender, EventArgs e)
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
        private void textBox1_TextChanged(object sender, EventArgs e)
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
        private void CargarCliente_Leave(object sender, EventArgs e)
        {}
        private void textBox16_TextChanged(object sender, EventArgs e)
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
        private void textBox2_TextChanged(object sender, EventArgs e)  // prod x nombr
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
        private void CargarProducto_Leave(object sender, EventArgs e)
        {}
        private void textBox17_TextChanged(object sender, EventArgs e)
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
            else
            {
                label40.Text = "";
            }
        }
        private void CargarSubtotal_Leave(object sender, EventArgs e)
        {}
        private void LimpiarLista_Click(object sender, EventArgs e)
        {
            _lstProdVendidos.Clear();
            dataGridView4.DataSource = _lstProdVendidos.ToArray();
            label42.Text = "";
        }


        private void NuevoProdVendidos_btn13_Click(object sender, EventArgs e)
        {
            if (labelClienteId.Text != "" && labelProductoId.Text != "")
            {
                try { long.Parse(labelProductoId.Text); } catch { return; };
                var producto = getProducto(long.Parse(labelProductoId.Text));
                var cantidad = int.Parse(textBox17.Text);

                for (int i = 0; i < cantidad; i++)
                {
                    ProdVendido nuevoPV = new ProdVendido
                    {
                        ProductoId = producto.Id,
                        Descripcion = label38.Text,
                        Cantidad = 1,
                        Precio = producto.Precio,
                        Estado = "Activo"
                    };
                    if (producto.Nombre.Contains("pendiente"))
                    {

                    }
                    else if (producto.Nombre.Contains("favor")
                          || producto.Nombre.Contains("devoluci")
                          || producto.Nombre.Contains("BONIF")
                    )
                        nuevoPV.Precio = producto.Precio * -1;
                    else if (producto.Nombre.Contains("actura"))
                        nuevoPV.Descripcion = label38.Text + textBox20.Text;
                    else
                        nuevoPV.Cantidad = int.Parse(textBox17.Text);
                    _lstProdVendidos.Add(nuevoPV);
                }

                double impTotal = 0;
                foreach (ProdVendido prodVendido in _lstProdVendidos)
                {
                    impTotal += prodVendido.Precio;
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
                Actualizar(); // dataGridView4.DataSource = _lstProdVendidos.ToArray();
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

        private bool EsProducto(string nombre)
        {
            bool es = true;
            if ((nombre.Contains("pendiente")) || (nombre.Contains("favor")) || (nombre.Contains("actura"))
                || (nombre.Contains("evoluc")) || (nombre.Contains("cobrar") || (nombre.Contains("BONIFI")))
            )
            {
                es = false;
            }
            return es;
        }

        private void ConfirmarCrearNota_Click(object sender, EventArgs e)
        {
            if (labelClienteId.Text != "" && labelClienteId.Text != "No encontrado")
            {
                var cliente = getClientePorDireccionExacta(label36.Text);
                if (cliente == null) return;

                NotaDeEnvio nuevaNota = new NotaDeEnvio
                {
                    ClientId = cliente.Id,
                    Client = cliente,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Impresa = 0,
                    Detalle = ServicioNotaDeEnvio.ExtraerDetalle(_lstProdVendidos),
                    ImporteTotal = ServicioNotaDeEnvio.ExtraerImporte(_lstProdVendidos),
                    Estado = "Activo"
                };
                long notaId = addNotaDeEnvioReturnId(nuevaNota);
                if (notaId == 0)
                {
                    MessageBox.Show("Falló al procesar Nota nueva");
                    return;
                }
                foreach (var prodVendido in _lstProdVendidos)
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
                        Fecha = DateTime.Now.ToString(),
                        NombreCliente = cliente.Direccion,
                        Estado = "Activo"
                    };
                    long registroId = addRegistroVentaReturnId(nuevoRegistro);
                    foreach (var prodVendido in _lstProdVendidos)
                    {
                        prodVendido.RegistroVentaId = registroId;
                        if (EsProducto(prodVendido.Producto.Nombre))
                        {
                            List<Venta> lstVentas = getVentas();
                            bool exists = false;
                            foreach (var venta in lstVentas)
                            {
                                if (venta.ProductoId == prodVendido.ProductoId)
                                {
                                    exists = true;
                                    venta.Cantidad += prodVendido.Cantidad;
                                    editVenta(venta);
                                }
                            }
                            if (!exists)
                            {
                                Venta nuevaVenta = new Venta
                                {
                                    ProductoId = prodVendido.ProductoId,
                                    Cantidad = prodVendido.Cantidad,
                                    Producto = getProducto(prodVendido.ProductoId)
                                };
                                addVenta(nuevaVenta);
                            }
                        }
                    }
                }

                foreach (var prodVendido in _lstProdVendidos)
                {
                    prodVendido.NotaDeEnvioId = notaId;
                    addProdVendido(prodVendido);
                }

                if (checkBox1.Checked)      // imprimir checkbox
                {
                    var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                    form.Rellenar_Datos(nuevaNota);
                    form.Show();
                }

                if (checkBox3.Checked)     // enviar a hoja de reparto como pedido nuevo para X reparto
                {
                    List<Reparto> repartos = getRepartosPorDia(comboBox4.Text);
                    Reparto reparto = repartos.Find(x => x.Nombre == comboBox3.Text);
                    addPedidoARepartoPorNota(reparto, cliente, _lstProdVendidos);
                }
                Close();
            }
            else
            {
                MessageBox.Show("Verifique los campos");
            }
        }
        private void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs e)
        {
            string diaDeReparto = comboBox4.Text;
            List<Reparto> repartos = getRepartosPorDia(diaDeReparto);  // partiendo del día seleccionado, buscar sus repartos
            comboBox3.DataSource = repartos;                           // combobox "día", necesito la lista de dias de rep
            comboBox3.DisplayMember = "Nombre";
            comboBox3.ValueMember = "Nombre";
        }
        private void CerrarBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
