using linway_app.Models;
using linway_app.Repositories;
using linway_app.Services;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private List<Venta> _lstVentas = new List<Venta>();
        private List<RegistroVenta> _lstRegistros = new List<RegistroVenta>();
        private List<Venta> _lstAgregarVentas = new List<Venta>();
        private List<Cliente> _lstClientes = new List<Cliente>();
        private readonly List<Producto> _lstProductos = new List<Producto>();
        private readonly IServicioVenta _servVenta;
        private readonly IServicioRegistroVenta _servRegistroVenta;
        private readonly IServicioDiaReparto _servDiaReparto;
        private readonly IServicioReparto _servReparto;
        private readonly IServicioProdVendido _servProdVendido;

        public FormVentas()
        {
            InitializeComponent();
            _servVenta = new ServicioVenta(new UnitOfWork(new LinwaydbContext()));
            _servRegistroVenta = new ServicioRegistroVenta(new UnitOfWork(new LinwaydbContext()));
            _servDiaReparto = new ServicioDiaReparto(new UnitOfWork(new LinwaydbContext()));
            _servReparto = new ServicioReparto(new UnitOfWork(new LinwaydbContext()));
            _servProdVendido = new ServicioProdVendido(new UnitOfWork(new LinwaydbContext()));
        }
        private void FormVentas_Load(object sender, EventArgs e)
        {
            GetVentas();
            GetRegistros();
        }
        public void GetVentas()
        {
            _lstVentas = _servVenta.GetAll();
            if (_lstVentas == null) return;
            dataGridView3.DataSource = _lstVentas.ToArray();
            dataGridView3.Columns[1].Width = 40;
        }
        public void GetRegistros()
        {
            _lstRegistros = _servRegistroVenta.GetAll();
            if (_lstRegistros == null) return;
            dataGridView1.DataSource = _lstRegistros.ToArray();
            dataGridView1.Columns[0].Width = 34;
            dataGridView1.Columns[1].Width = 67;
            label1.Text = "Registro de ventas (" + _lstRegistros.Count.ToString() + ")";
        }
        private DiaReparto GetRepartosPordia(string dia)
        {
            List<DiaReparto> dias = _servDiaReparto.GetAll();
            return dias.Find(x => x.Dia == dia);
        }
        private void AgregarPedidoAReparto(long clientId, string dia, string reparto, List<ProdVendido> lstProdVendidos)
        {
            //bool response = _servReparto.AgregarPedidoARepartoFormVenta(clientId, dia, reparto, lstProdVendidos);
            //if (!response) MessageBox.Show("Algo falló al agregar Pedido al reparto en la base de datos");
        }
        private void AgregarRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = _servRegistroVenta.Add(registroVenta);
            if (!response) MessageBox.Show("Algo falló al agregar Registro de Venta en la base de datos");
        }
        private void AgregarProdVendido(ProdVendido prodVendido)
        {
            bool response = _servProdVendido.Add(prodVendido);
            if (!response) MessageBox.Show("Algo falló al agregar Registro de Venta en la base de datos");
        }

        private void bCopiaSeguridad_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel ventas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Ventas a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //bool success = new Exportar().ExportarAExcel(listaVentas);
                //if (!success)
                //{
                //    MessageBox.Show("Hubo un error al guardar los cambios.");
                //}
            }

            DialogResult dialogResult2 = MessageBox.Show("Esta acción reemplazará al actual Excel registroVentas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Registro de Ventas a Excel", MessageBoxButtons.YesNo);
            if (dialogResult2 == DialogResult.Yes)
            {
                //bool success = new Exportar().ExportarAExcel(listaRegistros);
                //if (success)
                //{
                //    bCopiaSeguridad.ForeColor = Color.Green;
                //    bCopiaSeguridad.Enabled = false;
                //    bCopiaSeguridad.Text = "Creacion exitosa";
                //    MessageBox.Show("Terminados ambos archivos Excel: ventas y registroVentas");
                //}
                //else
                //{
                //    MessageBox.Show("Hubo un error al guardar los cambios.");
                //}
            }
        }

        private void ImportarBtn_Click(object sender, EventArgs e)
        {
            //CargarVentas();
            //CargarRegistros();
            //DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará definitivamente el listado actual de ventas por el contenido del Excel ventas.xlsx, y el registro de ventas por el contenido del Excel registroVentas.xlsx, en la carpeta Copias de seguridad. ¿Confirmar?", "Importar Ventas y Registro de Ventas desde Excel", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    listaVentas.Clear();
            //    listaVentas = new Importar("ventas").ImportarVentas();
            //    if (listaVentas != null)
            //    {
            //        GuardarVentas();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Falló Ventas; cancelado");
            //    }
            //    CargarVentas();

            //    listaRegistros.Clear();
            //    listaRegistros = new Importar("registroVentas").ImportarRegistroVentas();
            //    if (listaRegistros != null)
            //    {
            //        GuardarRegistros();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Falló Registro de Ventas; cancelado");
            //    }
            //    CargarRegistros();
            //}
        }

        private void exportarAExcel_Click(object sender, EventArgs e)
        {}

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

        //Actualizar
        private void ActualizarListas_Click(object sender, EventArgs e)
        {}

        //nueva venta
        private void NuevaVenta_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbNuevaVenta.Visible = true;
            _lstAgregarVentas.Clear();
            dataGridView5.DataSource = _lstAgregarVentas.ToArray();
            dataGridView5.Columns[1].Width = 60;
        }

        private void ReordenarVentas()
        {
            List<Venta> nuevaLista = new List<Venta>();
            foreach (Producto producto in _lstProductos)
            {
                var venta = _lstVentas.Find(x => x.ProductoId.Equals(producto.Id));
                if (venta != null)
                    nuevaLista.Add(venta);
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

        private void textBox12_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox12.Text != "")
            {
                try
                {
                    foreach (Producto producto in _lstProductos)
                    {
                        if (producto.Id == long.Parse(textBox12.Text))
                        {
                            encontrado = true;
                            label28.Text = producto.Nombre;
                            labelPrecio.Text = producto.Precio.ToString();
                        }
                    }
                }
                catch
                {}
            }
            if (!encontrado)
            {
                label28.Text = "No encontrado";
            }
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
            if (_lstAgregarVentas.Count > 0)
            {
                GetVentas();
                double precio = double.Parse(labelPrecio.Text);
                long clienteId = 0;

                List<ProdVendido> lstProdVendidos = null;

                foreach (Venta ventaParaAgregar in _lstAgregarVentas)
                {
                    if (EsProducto(ventaParaAgregar.ProductoId))
                    {
                        ProdVendido nuevoProdVendido = new ProdVendido
                        {
                            Cantidad = ventaParaAgregar.Cantidad,
                            Descripcion = ventaParaAgregar.Producto.Nombre,
                            PedidoId = ventaParaAgregar.Id,
                            Precio = ventaParaAgregar.Producto.Precio,
                            ProductoId = ventaParaAgregar.ProductoId
                        };
                        lstProdVendidos.Add(nuevoProdVendido);

                        bool existe = false;
                        foreach (Venta venta in _lstVentas)
                        {
                            if (venta.ProductoId == ventaParaAgregar.ProductoId)
                            {
                                existe = true;
                                venta.Cantidad += ventaParaAgregar.Cantidad;
                                //EditarVenta(venta);
                            }
                        }
                        if (!existe)
                        {
                            //AgregarVenta(ventaParaAgregar);
                        }
                    }
                }

                if (checkBox2.Checked)   // enviar a reparto o sea agregar o modificar pedido del cliente
                {
                    
                    try {
                        clienteId = long.Parse(textBox19.Text);
                        string dia = comboBox1.Text;
                        string reparto = comboBox2.Text;
                        //AgregarPedidoAReparto(clienteId, dia, reparto);
                    }
                    catch { return; };
                }

                // el nuevo registro va a depender de que corresponda a un cliente dado o no

                RegistroVenta nuevoRegistro = new RegistroVenta
                {
                    ClienteId = clienteId,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    NombreCliente = label20.Text,
                    ProdVendido = lstProdVendidos
                };
                AgregarRegistroVenta(nuevoRegistro);
                foreach (var prodVendido in lstProdVendidos)
                {
                    //prodVendido.RegistroVentaId = nuev
                    AgregarProdVendido(prodVendido);
                }
                ReordenarVentas();
                _lstAgregarVentas.Clear();
                bActualizar.PerformClick();
                LimpiarPantalla();
            }
            else
            {
                MessageBox.Show("No se han ingresado productos");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            _lstAgregarVentas.Clear();
            dataGridView5.DataSource = _lstAgregarVentas.ToArray();
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if ((label28.Text != "No encontrado") && (textBox13.Text != "") && (textBox12.Text != ""))
            {
                Producto prod = _lstProductos.Find(x => x.Nombre == label28.Text);
                if (prod == null) return;
                //Venta nuevaVenta = new Venta(prod.Id, int.Parse(textBox13.Text));
                Venta nuevaVenta = new Venta { ProductoId = prod.Id, Cantidad = int.Parse(textBox13.Text) };
                _lstAgregarVentas.Add(nuevaVenta);
                dataGridView5.DataSource = _lstAgregarVentas.ToArray();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            FormReparto fr = new FormReparto();
            comboBox2.DataSource = GetRepartosPordia(comboBox1.Text);
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
            fr.Close();
        }

        private void textBox19_Leave(object sender, EventArgs e)
        {
            if (textBox19.Text == "")
            {
                label20.Text = "No encontrado";
            }
            else
            {
                if (_lstClientes.Exists(x => x.Id == int.Parse(textBox19.Text)))
                {
                    label20.Text = (_lstClientes.Find(x => x.Id == int.Parse(textBox19.Text)).Direccion);
                }
                else
                {
                    label20.Text = "No encontrado";
                }
            }
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
                _lstVentas.Clear();
                //GuardarVentas();
                LimpiarPantalla();
                GetVentas();
            }
            else
            {
                MessageBox.Show("Seleccione si esta seguro para borrar la lista de ventas");
            }
        }




        ///////////////////////////////REGISTRO DE VENTAS//////////////////////////////////////

        private void VerRegistro_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbVerRegistro.Visible = true;
            dataGridView2.DataSource = new List<ProdVendido>().ToArray();
        }

        
        //Deshacer una venta
        private void TextBox1_Leave(object sender, EventArgs e)
        {
            bool encontrado = false;
            if (textBox1.Text != "")
            {
                foreach (RegistroVenta rActual in _lstRegistros)
                {
                    if (rActual.Id == uint.Parse(textBox1.Text))
                    {
                        encontrado = true;
                        labelFecha.Text = rActual.Fecha;
                        labelCliente.Text = rActual.NombreCliente;
                        //dataGridView2.DataSource = rActual.ProductosVendidos.ToArray();
                        //labelTotal.Text = "Total: $" + rActual.ObtenerTotal().ToString();
                        bDeshacerVenta.Enabled = true;
                    }
                }
            }
            if (!encontrado)
            {
                labelCliente.Text = "NO SE ENCONTRO REGISTRO";
                labelFecha.Text = "XX/XX/XXXX";
                labelTotal.Text = "Total: $XXXX";
                dataGridView2.DataSource = new List<ProdVendido>().ToArray();
                bDeshacerVenta.Enabled = false;
            }
        }

        private void bDeshacerVenta_Click(object sender, EventArgs e)
        {
            if (cbSeguro.Checked)
            {
                RegistroVenta registro = _lstRegistros.Find(x => x.Id == int.Parse(textBox1.Text.ToString()));
                //foreach (ProdVendido prodVendido in registro.ProdVendidos)
                //    if (EsProducto(prodVendido.ProductoId))
                //        foreach (Venta venta in _lstVentas)
                //            if (venta.ProductoId.Equals(prodVendido.ProductoId))
                //                venta.Cantidad -= prodVendido.Cantidad;
                //GuardarVentas();
                _lstRegistros.Remove(registro);
                GetRegistros();
                bActualizar.PerformClick();
                LimpiarPantalla();
            }
            else
            {
                MessageBox.Show("Debe confirmar que esta seguro de deshacer este registro.");
            }
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
                    if (rActual.Fecha == DateTime.Today.ToString().Substring(0, 10))
                    {
                        lFiltrada.Add(rActual);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }
            else if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                textBox2.Text = "";
                textBox2.Visible = false;
                dataGridView1.DataSource = _lstRegistros.ToArray();
            }
            else
            {
                textBox2.Text = "";
                textBox2.Visible = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
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
            dataGridView1.DataSource = ListaFiltrada.ToArray();
        }

        //Borrar registro de ventas
        private void borrarRegistrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gbBorrarReg.Visible = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (bBorrarRegVentas.Enabled)
                bBorrarRegVentas.Enabled = false;
            else
                bBorrarRegVentas.Enabled = true;
        }

        private bool IntervaloCorrecto()
        {
            bool todoOk = false;
            if ((tbDesde.Text != "") & (tbHasta.Text != ""))
            {
                long primero = long.Parse(tbDesde.Text);
                long segundo = long.Parse(tbHasta.Text);
                todoOk = (primero <= segundo);
                if (!todoOk)
                    MessageBox.Show("intervalo incorrecto.");
            }
            else
            {
                MessageBox.Show("Falta llenar algunos campos");
            }
            return todoOk;
        }

        private bool SeEncuentraEnIntervalo(long id)
        {
            long primero = long.Parse(tbDesde.Text);
            long segundo = long.Parse(tbHasta.Text);
            return ((primero <= id) & (segundo >= id));
        }

        private void bBorrarRegVentas_Click(object sender, EventArgs e)
        {
            List<RegistroVenta> registrosABorrar = new List<RegistroVenta>();
            if (IntervaloCorrecto())
            {
                _lstRegistros.RemoveAll(x => SeEncuentraEnIntervalo(x.Id));
                GetRegistros();
                LimpiarPantalla();
                bActualizar.PerformClick();
            }
        }
    }
}
