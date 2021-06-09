using linway_app.Models;
using linway_app.Models.Entities;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DClientes;
using static linway_app.Services.Delegates.DDiaReparto;
using static linway_app.Services.Delegates.DNotaDeEnvio;
using static linway_app.Services.Delegates.DProductos;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DRegistroVenta;
using static linway_app.Services.Delegates.DReparto;
using static linway_app.Services.Delegates.DVentas;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private List<Venta> _lstVentas;
        private List<RegistroVenta> _lstRegistros;
        private List<Venta> _lstAgregarVentas;
        private List<Cliente> _lstClientes;
        private List<Producto> _lstProductos;
        public FormVentas()
        {
            InitializeComponent();
            _lstVentas = new List<Venta>();
            _lstRegistros = new List<RegistroVenta>();
            _lstAgregarVentas = new List<Venta>();
            _lstClientes = new List<Cliente>();
            _lstProductos = new List<Producto>();
        }
        private void FormVentas_Load(object sender, EventArgs e)
        {
            Actualizar();
        }
        private void Actualizar()
        {
            _lstClientes = getClientes();
            _lstProductos = getProductos();
            _lstVentas = getVentas();
            _lstRegistros = getRegistroVentas();
            ActualizarGrid1Registros(_lstRegistros);
            ActualizarGrid3Ventas();
        }
        private void ActualizarGrid1Registros(List<RegistroVenta> lstRegistroVentas)
        {
            if (lstRegistroVentas != null)
            {
                List<ERegistroVenta> grid = new List<ERegistroVenta>();
                foreach (RegistroVenta registroVenta in lstRegistroVentas)
                {
                    grid.Add(Form1._mapper.Map<ERegistroVenta>(registroVenta));
                }
                dataGridView1.DataSource = grid;
                dataGridView1.Columns[0].Width = 34;
                dataGridView1.Columns[1].Width = 67;
                label1.Text = "Registro de ventas (" + lstRegistroVentas.Count.ToString() + ")";
            }
        }
        private void ActualizarGrid2ProdVendidos (List<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos != null)
            {
                List<EProdVendido> grid = new List<EProdVendido>();
                foreach (ProdVendido prodVendido in lstProdVendidos)
                {
                    grid.Add(Form1._mapper.Map<EProdVendido>(prodVendido));
                }
                dataGridView2.DataSource = grid;
            }
        }
        private void ActualizarGrid3Ventas()
        {
            if (_lstVentas != null)
            {
                List<EVenta> grid = new List<EVenta>();
                foreach (Venta venta in _lstVentas)
                {
                    grid.Add(Form1._mapper.Map<EVenta>(venta));
                }
                dataGridView3.DataSource = grid;
                dataGridView3.Columns[0].Width = 20;
                dataGridView3.Columns[1].Width = 40;
            }
        }
        private void ActualizarGrid5(List<Venta> lstVentas)
        {
            if (lstVentas != null)
            {
                List<EVenta> grid = new List<EVenta>();
                foreach (Venta venta in lstVentas)
                {
                    grid.Add(Form1._mapper.Map<EVenta>(venta));
                }
                dataGridView5.DataSource = grid;
                dataGridView5.Columns[0].Width = 20;
                dataGridView5.Columns[1].Width = 40;
            }
        }

        private void LimpiarPantalla()
        {
            gbNuevaVenta.Visible = false;
            gbVerRegistro.Visible = false;
            groupBox7.Visible = false;
            label28.Text = "";
            label20.Text = "";
            textBox19.Text = "";
            checkBox2.Checked = false;
            cbSeguro.Checked = false;
            textBox12.Text = "";
            textBox13.Text = "";
            textBox1.Text = "";
            labelFecha.Text = "";
            labelTotal.Text = "";
            labelCliente.Text = "";
            cbSeguroBorrar.Checked = false;
            gbBorrarReg.Visible = false;
            tbDesde.Text = "";
            tbHasta.Text = "";
            checkBox1.Checked = false;
        }

        //nueva venta
        private void NuevaVenta_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbNuevaVenta.Visible = true;
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
        }
        private void ReordenarVentas()
        {
            List<Venta> nuevaLista = new List<Venta>();
            foreach (Producto producto in _lstProductos)
            {
                var venta = _lstVentas.Find(x => x.ProductoId.Equals(producto.Id));
                if (venta != null) nuevaLista.Add(venta);
            }
            _lstVentas = nuevaLista;
        }
        private void CancelarClick_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
        private void SoloNumeroYNegativo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))
                e.Handled = true;

            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;
        }
        private void InputProductoId_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text != "")
            {
                try { long.Parse(textBox12.Text); } catch { return; }
                Producto producto = getProducto(long.Parse(textBox12.Text));
                if (producto != null)
                {
                    label28.Text = producto.Nombre;
                    labelPrecio.Text = producto.Precio.ToString();
                }
                else label28.Text = "No encontrado";

            } else label28.Text = "";
        }
        private bool EsProducto(long productoId)
        {
            bool es = true;
            string nombreProd = _lstProductos.Find(x => x.Id == productoId).Nombre;
            if ((nombreProd.Contains("pendiente")) || (nombreProd.Contains("favor")) || (nombreProd.Contains("actura"))
                 || (nombreProd.Contains("evoluc")) || (nombreProd.Contains("cobrar") || (nombreProd.Contains("BONIFI")))
            )
            {
                es = false;
            }
            return es;
        }
        private void AgregarVenta_Click(object sender, EventArgs e)
        {
            if (_lstAgregarVentas != null && _lstAgregarVentas.Count > 0)
            {
                // listo // generar un Venta por cada ítem en _lstAgregarVentas, no necesita precios
                // listo // generar un RegistroVenta por cada con "Venta particular"y devuelve un id para usar en los ProdVendido
                // listo // generar un ProdVendido, necesita producto, precio, cantidad, id de su RegVenta

                try { double.Parse(labelPrecio.Text); } catch { return; };
                ActualizarGrid3Ventas();
                double precio = double.Parse(labelPrecio.Text);
                long clienteId = 1;
                List<ProdVendido> lstProdVendidos = new List<ProdVendido>();
                List<RegistroVenta> lstNuevosRegistroVentas = new List<RegistroVenta>();

                
                foreach (Venta ventaParaAgregar in _lstAgregarVentas)
                {
                    if (EsProducto(ventaParaAgregar.ProductoId))
                    {
                        // hacer o editar ventas
                        if (_lstVentas != null)
                        {
                            Venta venta = _lstVentas.Find(x => x.ProductoId == ventaParaAgregar.ProductoId);
                            if (venta != null)
                            {
                                venta.Cantidad += ventaParaAgregar.Cantidad;
                                editVenta(venta);
                            }
                            else addVenta(ventaParaAgregar);
                        }
                        else addVenta(ventaParaAgregar);

                        // hacer o editar registros (no puedo editar porque no tengo cliente, se modifica si hay "enviar a hdr")
                        RegistroVenta nuevoRegistroVenta = new RegistroVenta
                        {
                            NombreCliente = "Venta Particular X",
                            Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                            ClienteId = 1
                        };
                        lstNuevosRegistroVentas.Add(nuevoRegistroVenta);

                        // hacer ProdVendido
                        long registroId = addRegistroVentaReturnId(nuevoRegistroVenta);
                        if (registroId == 0) { MessageBox.Show("Falló"); return; };
                        Producto producto = _lstProductos.Find(x => x.Id == ventaParaAgregar.ProductoId);
                        ProdVendido nuevoProdVendido = new ProdVendido
                        {
                            Cantidad = ventaParaAgregar.Cantidad,
                            Descripcion = producto.Nombre,
                            Precio = producto.Precio,
                            ProductoId = producto.Id,
                            RegistroVentaId = registroId
                        };
                        lstProdVendidos.Add(nuevoProdVendido);
                        addProdVendido(nuevoProdVendido);
                    }
                    Actualizar();
                }


                // si se manda a Pedido:
                // generar un Pedido (o modificar), necesita dia, reparto, cliente, lstProdVendidos
                // agregar cliente a cada RegistroVenta
                if (checkBox2.Checked)   // enviar a reparto, o sea agregar o modificar pedido del cliente
                {
                    try { long.Parse(textBox19.Text); } catch { return; };
                    string dia = comboBox1.Text;
                    string nombre = comboBox2.Text;
                    Cliente cliente = getCliente(long.Parse(textBox19.Text));
                    Reparto reparto = getRepartoPorNombre(dia, nombre);
                    addPedidoAReparto(reparto, cliente, lstProdVendidos);
                    
                    foreach (var nuevoRegVenta in lstNuevosRegistroVentas)
                    {
                        nuevoRegVenta.ClienteId = clienteId;
                        editRegistroVenta(nuevoRegVenta);
                    }
                }

                ReordenarVentas();
                _lstAgregarVentas.Clear();
                bActualizar.PerformClick();
                LimpiarPantalla();
            }
            else MessageBox.Show("No se han ingresado productos");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }
        private void button17_Click(object sender, EventArgs e)
        {
            if (label28.Text != "No encontrado" && textBox12.Text != "")
            {
                try { int.Parse(textBox13.Text); } catch { return; }
                Producto prod = _lstProductos.Find(x => x.Nombre == label28.Text);
                if (prod == null) return;
                Venta nuevaVenta = new Venta {
                    ProductoId = prod.Id,
                    Cantidad = int.Parse(textBox13.Text)
                };
                _lstAgregarVentas.Add(nuevaVenta);
                ActualizarGrid5(_lstAgregarVentas);
            }
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }

        //enviar HDR
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label6.Visible = true;
                label7.Visible = true;
                comboBox2.Visible = true;
                comboBox1.Visible = true;
                label15.Visible = true;
                label20.Text = "";
                textBox19.Visible = true;
            }
            else
            {
                label6.Visible = false;
                label7.Visible = false;
                comboBox2.Visible = false;
                comboBox1.Visible = false;
                label15.Visible = false;
                label20.Text = "";
                textBox19.Visible = false;
                textBox19.Text = "";
            }
        }

        private void AgregarAReparto_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Reparto> repartos = getRepartosPorDia(comboBox1.Text);
            if (repartos == null) return;
            var form = Program.GetConfig().GetRequiredService<FormRepartos>();
            comboBox2.DataSource = repartos;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
            form.Close();
        }

        private void TextBox19_TextChanged(object sender, EventArgs e)
        {
            if (textBox19.Text != "")
            {
                try
                {
                    long clienteId = long.Parse(textBox19.Text);
                    if (_lstClientes.Exists(x => x.Id == clienteId))
                    {
                        label20.Text = (_lstClientes.Find(x => x.Id == clienteId).Direccion);
                    }
                    else label20.Text = "No encontrado";
                }
                catch (Exception) { label20.Text = "No encontrado"; }
            }
            else
                label20.Text = "No encontrado";
        }

        //reiniciar ventas
        private void ReiniciarVentas_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
        }

        private void ReiniciarVentas_Click(object sender, EventArgs e)
        {
            if (cbSeguroBorrar.Checked)
            {
                foreach (Venta venta in _lstVentas)
                {
                    deleteVenta(venta);
                }
                LimpiarPantalla();
                Actualizar();
            }
            else MessageBox.Show("Seleccione si esta seguro para borrar la lista de ventas");
        }




        ///////////////////////////////REGISTRO DE VENTAS//////////////////////////////////////

        private void VerRegistro_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbVerRegistro.Visible = true;
            ActualizarGrid2ProdVendidos(new List<ProdVendido>());
        }

        
        //Deshacer una venta
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try { long.Parse(textBox1.Text); } catch { return; };
                RegistroVenta registroVenta = getRegistroVenta(long.Parse(textBox1.Text));
                if (registroVenta != null)
                {
                    labelFecha.Text = registroVenta.Fecha;
                    labelCliente.Text = registroVenta.NombreCliente;
                    ActualizarGrid2ProdVendidos(registroVenta.ProdVendido.ToList());
                    double importeTotal = 0;
                    foreach (ProdVendido prodVendido in registroVenta.ProdVendido)
                    {
                        importeTotal += prodVendido.Precio;
                    }
                    labelTotal.Text = "Total: $" + importeTotal.ToString();
                    bDeshacerVenta.Enabled = true;
                }
                else
                {
                    labelCliente.Text = "";
                    labelFecha.Text = "";
                    labelTotal.Text = "";
                    ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                    bDeshacerVenta.Enabled = false;
                }
            }
            else
            {
                labelCliente.Text = "";
                labelFecha.Text = "";
                labelTotal.Text = "";
                ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                bDeshacerVenta.Enabled = false;
            }
        }
        private void DeshacerVenta_Click(object sender, EventArgs e)
        {
            if (cbSeguro.Checked)
            {
                try { long.Parse(textBox1.Text); } catch { return; };
                RegistroVenta registro = getRegistroVenta(long.Parse(textBox1.Text));
                if (registro != null)
                {

                }
                foreach (ProdVendido prodVendido in registro.ProdVendido)
                    if (EsProducto(prodVendido.ProductoId))
                        foreach (Venta venta in _lstVentas)
                            if (venta.ProductoId.Equals(prodVendido.ProductoId)) venta.Cantidad -= prodVendido.Cantidad;
                editRegistroVenta(registro);
                Actualizar();
                LimpiarPantalla();
            }
            else MessageBox.Show("Debe confirmar que esta seguro de deshacer este registro.");
        }

        //////Filtrar datos. 
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<RegistroVenta> lFiltrada = new List<RegistroVenta>();
            if (comboBox3.SelectedItem.ToString() == "Hoy")
            {
                textBox2.Text = "";
                textBox2.Visible = false;
                foreach (RegistroVenta rActual in _lstRegistros)
                {
                    if (rActual.Fecha == DateTime.Today.ToString().Substring(0, 10)) lFiltrada.Add(rActual);
                }
                ActualizarGrid1Registros(lFiltrada);
            }
            else if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                textBox2.Text = "";
                textBox2.Visible = false;
                ActualizarGrid1Registros(_lstRegistros);
            }
            else
            {
                textBox2.Text = "";
                textBox2.Visible = true;
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Cliente")
            {
                FiltrarDatos(textBox2.Text, 'c');
            }
            if (comboBox3.SelectedItem.ToString() == "Fecha")
            {
                FiltrarDatos(textBox2.Text, 'f');
            }
        }

        private void FiltrarDatos(string texto, char x)
        {
            List<RegistroVenta> ListaFiltrada = new List<RegistroVenta>();
            foreach (RegistroVenta rActual in _lstRegistros)
            {
                if (x == 'c')
                {
                    if (rActual.NombreCliente.Contains(texto))
                    {
                        ListaFiltrada.Add(rActual);
                    }
                }
                if (x == 'f')
                {
                    if (rActual.Fecha.Contains(texto))
                    {
                        ListaFiltrada.Add(rActual);
                    }
                }
            }
            ActualizarGrid1Registros(ListaFiltrada);
        }

        //Borrar registro de ventas
        private void BorrarRegistros_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbBorrarReg.Visible = true;
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (bBorrarRegVentas.Enabled)
                bBorrarRegVentas.Enabled = false;
            else
                bBorrarRegVentas.Enabled = true;
        }
        private bool IntervaloCorrecto()
        {
            try
            {
                bool todoOk = false;
                long primero = long.Parse(tbDesde.Text);
                long segundo = long.Parse(tbHasta.Text);
                todoOk = (primero <= segundo);
                if (!todoOk)
                    MessageBox.Show("intervalo incorrecto.");
                return todoOk;
            }
            catch
            {
                MessageBox.Show("Falta llenar algunos campos");
                return false;
            }
        }
        private bool SeEncuentraEnIntervalo(long id)
        {
            try
            {
                long primero = long.Parse(tbDesde.Text);
                long segundo = long.Parse(tbHasta.Text);
                return (primero <= id & segundo >= id);
            }
            catch { return false; }
        }
        private void BorrarRegVentas_Click(object sender, EventArgs e)
        {
            List<RegistroVenta> registrosABorrar = new List<RegistroVenta>();
            if (IntervaloCorrecto())
            {
                _lstRegistros.RemoveAll(x => SeEncuentraEnIntervalo(x.Id));
                Actualizar();
                LimpiarPantalla();
                bActualizar.PerformClick();
            }
        }
    }
}
