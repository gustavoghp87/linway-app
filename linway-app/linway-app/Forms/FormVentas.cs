﻿using linway_app.Excel;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DPedido;
using static linway_app.Services.Delegates.DProducto;
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
        private readonly List<Venta> _lstAgregarVentas;
        private string showing = "agregarReg";
        public FormVentas()
        {
            InitializeComponent();
            _lstVentas = new List<Venta>();
            _lstRegistros = new List<RegistroVenta>();
            _lstAgregarVentas = new List<Venta>();
        }
        private void FormVentas_Load(object sender, EventArgs e)
        {
            Actualizar();
        }
        private void Actualizar()
        {
            _lstVentas = getVentas();
            _lstRegistros = getRegistroVentas();
            ActualizarGrid1Registros(_lstRegistros);
            ActualizarGrid3Ventas();
        }
        private void ActualizarGrid1Registros(ICollection<RegistroVenta> lstRegistroVentas)
        {
            if (lstRegistroVentas == null) return;
            var grid = new List<ERegistroVenta>();
            lstRegistroVentas.ToList().ForEach(x =>
                grid.Add(Form1.mapper.Map<ERegistroVenta>(x))
            );
            dataGridView1.DataSource = grid;
            dataGridView1.Columns[0].Width = 48;
            dataGridView1.Columns[1].Width = 240;
            label1.Text = "Registro de ventas (" + lstRegistroVentas.Count.ToString() + ")";
        }
        private void ActualizarGrid2ProdVendidos (ICollection<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos == null) return;
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                grid.Add(Form1.mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView2.DataSource = grid;
            if (showing == "agregarReg")
                dataGridView2.Columns[0].Width = 150;
            if (showing == "verReg")
            {
                dataGridView2.Columns[0].Width = 28;
                dataGridView2.Columns[1].Width = 150;
            }
        }
        private void ActualizarGrid3Ventas()
        {
            if (_lstVentas == null) return;
            var grid = new List<EVenta>();
            foreach (Venta venta in _lstVentas)
            {
                grid.Add(Form1.mapper.Map<EVenta>(venta));
            }
            grid = grid.OrderBy(x => x.Detalle).ToList();
            dataGridView3.DataSource = grid;
            dataGridView3.Columns[0].Width = 20;
            dataGridView3.Columns[1].Width = 40;
        }
        private void ActualizarGrid5(ICollection<Venta> lstVentas)
        {
            if (lstVentas == null) return;
            var grid = new List<EVenta>();
            foreach (Venta venta in lstVentas)
            {
                grid.Add(Form1.mapper.Map<EVenta>(venta));
            }
            grid = grid.OrderBy(x => x.Detalle).ToList();
            dataGridView5.DataSource = grid;
            dataGridView5.Columns[0].Width = 280;
        }
        private void LimpiarPantalla()
        {
            gbNuevaVenta.Visible = false;
            gbVerRegistro.Visible = false;
            groupBox7.Visible = false;
            label28.Text = "";
            label20.Text = "";
            textBox19.Text = "";
            textBox3.Text = "";
            checkBox2.Checked = false;
            cbSeguro.Checked = false;
            textBox12.Text = "";
            textBox13.Text = "";
            textBox1.Text = "";
            labelFecha.Text = "";
            labelTotal.Text = "";
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
            showing = "agregarReg";
            gbNuevaVenta.Visible = true;
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
        }
        private void CancelarClick_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
        }
        private void SoloNumeroYNegativo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-') e.Handled = true;
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0) e.Handled = true;
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
                else
                {
                    label28.Text = "No encontrado";
                    labelPrecio.Text = "";
                }
            }
            else
            {
                label28.Text = "";
                labelPrecio.Text = "";
            }
        }
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                Producto producto = getProductoPorNombre(textBox4.Text);
                if (producto != null)
                {
                    label28.Text = producto.Nombre;
                    labelPrecio.Text = producto.Precio.ToString();
                }
                else
                {
                    label28.Text = "No encontrado";
                    labelPrecio.Text = "";
                }
            }
            else
            {
                label28.Text = "";
                labelPrecio.Text = "";
            }
        }
        private void Limpiar_Click(object sender, EventArgs e)
        {
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }
        private void Anyadir_Click(object sender, EventArgs e)
        {
            try { int.Parse(textBox13.Text); } catch { return; }
            Producto producto = getProductoPorNombreExacto(label28.Text);
            if (producto == null) return;
            var nuevaVenta = new Venta
            {
                ProductoId = producto.Id,
                Cantidad = int.Parse(textBox13.Text),
                Producto = producto      // dejar para que cargue el nombre en el grid
            };
            _lstAgregarVentas.Add(nuevaVenta);
            ActualizarGrid5(_lstAgregarVentas);
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox4.Text = "";
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)        //enviar HDR
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
                textBox3.Visible = true;
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
                textBox3.Visible = false;
                textBox19.Text = "";
                textBox3.Text = "";
            }
        }
        private void AgregarAReparto_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Reparto> repartos = getRepartosPorDia(comboBox1.Text);
            if (repartos == null) return;
            comboBox2.DataSource = repartos;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
        }
        private void TextBox19_TextChanged(object sender, EventArgs e)
        {
            if (textBox19.Text == "")
            {
                label20.Text = "";
                return;
            }
            try { long.Parse(textBox19.Text); } catch { return; }
            Cliente cliente = getCliente(long.Parse(textBox19.Text));
            if (cliente == null)
                label20.Text = "No encontrado";
            else
                label20.Text = cliente.Direccion;
        }
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                label20.Text = "";
                return;
            }
            Cliente cliente = getClientePorDireccion(textBox3.Text);
            if (cliente == null)
                label20.Text = "No encontrado";
            else
                label20.Text = cliente.Direccion;
        }
        private void AgregarVenta_Click(object sender, EventArgs e)
        {
            string dia = comboBox1.Text;
            string nombre = comboBox2.Text;
            string direccionCliente = label20.Text;
            Cliente cliente = null;
            Reparto reparto = null;
            Pedido pedido = null;
            if (_lstAgregarVentas == null || _lstAgregarVentas.Count == 0)
            {
                MessageBox.Show("No se han ingresado productos");
                return;
            }
            if (checkBox2.Checked)   // enviar a reparto
            {
                if (direccionCliente == "" || dia == "" || nombre == "")
                {
                    MessageBox.Show("Faltan el cliente o el reparto");
                    return;
                }
                cliente = getClientePorDireccionExacta(label20.Text);
                reparto = getRepartoPorDiaYNombre(dia, nombre);
                if (cliente == null || reparto == null)
                {
                    Form1.loadingForm.CloseIt();
                    MessageBox.Show("Falló cliente o reparto");
                    return;
                }
                long pedidoId = addPedidoIfNotExistsAndReturnId(reparto.Id, cliente.Id);
                pedido = getPedido(pedidoId);
                if (pedidoId == 0 || pedido == null)
                {
                    Form1.loadingForm.CloseIt();
                    MessageBox.Show("Falló pedido");
                    return;
                }
            }
            // generar un RegistroVenta con "Venta particular"y devuelve un id para usar en los ProdVendido
            // generar un Venta por cada ítem en _lstAgregarVentas, no necesita precios
            // generar un ProdVendido por cada ítem en _lstAgregarVentas con el id de (1)
            Form1.loadingForm.OpenIt();
            ActualizarGrid3Ventas();
            var lstProdVendidos = new List<ProdVendido>();
            RegistroVenta nuevoRegistroVenta = new RegistroVenta
            {   // hacer o editar registros (no puedo editar porque no tengo cliente, se modifica si hay "enviar a hdr")
                ClienteId = 634,
                NombreCliente = "Venta particular",
                Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha)
            };
            long registroId = addRegistroVentaReturnId(nuevoRegistroVenta);
            if (registroId == 0)
            {
                Form1.loadingForm.CloseIt();
                MessageBox.Show("Falló Registro");
                return;
            };
            var ventasAAgregar = new List<Venta>();
            var ventasAEditar = new List<Venta>();
            var prodVendidosAAgregar = new List<ProdVendido>();
            var prodVendidosAEditar = new List<ProdVendido>();
            foreach (Venta ventaParaAgregar in _lstAgregarVentas)
            {
                List<Venta> lstVentas = getVentas();
                bool exists = false;
                if (lstVentas != null)
                    foreach (Venta venta in lstVentas)
                    {
                        if (venta.ProductoId != ventaParaAgregar.ProductoId) continue;
                        exists = true;
                        venta.Cantidad += ventaParaAgregar.Cantidad;
                        ventasAEditar.Add(venta);
                    }
                if (!exists) ventasAAgregar.Add(ventaParaAgregar);

                Producto producto = getProducto(ventaParaAgregar.ProductoId);
                if (producto == null)
                {
                    Form1.loadingForm.CloseIt();
                    MessageBox.Show("Falló Producto");
                    return;
                }

                exists = false;
                if (checkBox2.Checked)
                {
                    List<ProdVendido> prodVendidos = getProdVendidos();
                    foreach (ProdVendido prodVendido in prodVendidos)
                    {
                        if (exists) continue;
                        if (prodVendido.ProductoId == producto.Id && prodVendido.PedidoId == pedido.Id)
                        {
                            exists = true;
                            prodVendido.Cantidad += ventaParaAgregar.Cantidad;
                            prodVendidosAEditar.Add(prodVendido);
                        }
                    }
                }
                if (!exists)
                {
                    ProdVendido nuevoProdVendido = new ProdVendido
                    {
                        Cantidad = ventaParaAgregar.Cantidad,
                        Descripcion = producto.Nombre,
                        Precio = producto.Precio,
                        ProductoId = producto.Id,
                        RegistroVentaId = registroId
                    };
                    prodVendidosAAgregar.Add(nuevoProdVendido);
                    lstProdVendidos.Add(nuevoProdVendido);
                }
                Actualizar();
            }
            addVentas(ventasAAgregar);
            editVentas(ventasAEditar);
            addProdVendidos(prodVendidosAAgregar);
            editProdVendidos(prodVendidosAEditar);
            if (checkBox2.Checked)
            {
                lstProdVendidos.ToList().ForEach(prodVendido =>
                {
                    prodVendido.PedidoId = pedido.Id;
                });
                editProdVendidos(lstProdVendidos);
                updatePedido(pedido, true);
                nuevoRegistroVenta.ClienteId = cliente.Id;
                nuevoRegistroVenta.NombreCliente = cliente.Direccion;
                editRegistroVenta(nuevoRegistroVenta);
            }
            Form1.loadingForm.CloseIt();
            _lstAgregarVentas.Clear();
            LimpiarPantalla();
            Actualizar();
        }

        // reiniciar ventas
        private void ReiniciarVentas_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
        }
        private void ReiniciarVentas_Click(object sender, EventArgs e)
        {
            if (!cbSeguroBorrar.Checked)
            {
                MessageBox.Show("Seleccione si esta seguro para borrar la lista de ventas");
                return;
            }
            Form1.loadingForm.OpenIt();
            deleteVentas(_lstVentas);
            Form1.loadingForm.CloseIt();
            LimpiarPantalla();
            Actualizar();
        }


        ///////////////////////////////REGISTRO DE VENTAS//////////////////////////////////////
        ///
        private void VerRegistro_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            showing = "verReg";
            gbVerRegistro.Visible = true;
            ActualizarGrid2ProdVendidos(new List<ProdVendido>());
        }

        // Ver y deshacer una venta
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try { long.Parse(textBox1.Text); } catch { return; };
                RegistroVenta registroVenta = getRegistroVenta(long.Parse(textBox1.Text));
                if (registroVenta != null)
                {
                    labelFecha.Text = registroVenta.Fecha;
                    labelProductoN.Text = registroVenta.NombreCliente;
                    ActualizarGrid2ProdVendidos(registroVenta.ProdVendido.ToList());
                    decimal importeTotal = 0;
                    foreach (ProdVendido prodVendido in registroVenta.ProdVendido)
                    {
                        importeTotal += prodVendido.Precio;
                    }
                    labelTotal.Text = "Total: $" + importeTotal.ToString();
                    bDeshacerVenta.Enabled = true;
                }
                else
                {
                    labelProductoN.Text = "";
                    labelFecha.Text = "";
                    labelTotal.Text = "";
                    ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                    bDeshacerVenta.Enabled = false;
                }
            }
            else
            {
                labelProductoN.Text = "";
                labelFecha.Text = "";
                labelTotal.Text = "";
                ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                bDeshacerVenta.Enabled = false;
            }
        }
        private void DeshacerVenta_Click(object sender, EventArgs e)
        {
            if (!cbSeguro.Checked)
            {
                MessageBox.Show("Debe confirmar que está seguro de deshacer este registro.");
                return;
            }
            long registroVentaId;
            try { registroVentaId = long.Parse(textBox1.Text); } catch { return; };
            Form1.loadingForm.OpenIt();
            RegistroVenta registro = getRegistroVenta(registroVentaId);
            if (registro == null)
            {
                Form1.loadingForm.CloseIt();
                return;
            }
            deleteRegistroVenta(registro);
            updateVentasDesdeProdVendidos(registro.ProdVendido, false);
            Form1.loadingForm.CloseIt();
            Actualizar();
            LimpiarPantalla();
        }

        ////// Filtrar datos. 
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<RegistroVenta> lFiltrada = new List<RegistroVenta>();
            if (comboBox3.SelectedItem.ToString() == "Hoy")
            {
                textBox2.Text = "";
                textBox2.Visible = false;
                foreach (RegistroVenta rActual in _lstRegistros)
                {
                    if (rActual.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha)) lFiltrada.Add(rActual);
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
            if (comboBox3.SelectedItem.ToString() == "Cliente") FiltrarDatos(textBox2.Text, 'c');
            if (comboBox3.SelectedItem.ToString() == "Fecha") FiltrarDatos(textBox2.Text, 'f');
        }
        private void FiltrarDatos(string texto, char x)
        {
            List<RegistroVenta> ListaFiltrada = new List<RegistroVenta>();
            foreach (RegistroVenta rActual in _lstRegistros)
            {
                if (x == 'c' && rActual.NombreCliente.Contains(texto))
                    ListaFiltrada.Add(rActual);
                if (x == 'f' && rActual.Fecha.Contains(texto))
                    ListaFiltrada.Add(rActual);
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
                if (!todoOk) MessageBox.Show("intervalo incorrecto.");
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
            if (!IntervaloCorrecto()) return;
            Form1.loadingForm.OpenIt();
            var registrosABorrar = new List<RegistroVenta>();
            var ventasABorrar = new List<ProdVendido>();
            foreach (RegistroVenta registroVenta in _lstRegistros)
            {
                if (!SeEncuentraEnIntervalo(registroVenta.Id)) continue;
                registrosABorrar.Add(registroVenta);
                ventasABorrar.AddRange(registroVenta.ProdVendido);
            }
            deleteRegistros(registrosABorrar);
            updateVentasDesdeProdVendidos(ventasABorrar, false);
            Form1.loadingForm.CloseIt();
            Actualizar();
            LimpiarPantalla();
        }
        private void ExportBtn_Click_1(object sender, EventArgs e)
        {
            Actualizar();
            Form1.loadingForm.OpenIt();
            bool success = new Exportar().ExportarVentas(_lstVentas);
            Form1.loadingForm.CloseIt();
            if (success) ExportBtn.Text = "Terminado";
        }
    }
}
