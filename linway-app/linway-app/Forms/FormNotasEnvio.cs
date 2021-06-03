using linway_app.Models;
using linway_app.Services;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private List<NotaDeEnvio> _lstNotaDeEnvios;
        private List<ProdVendido> _lstProdVendidos;
        private List<Producto> _lstProductos;
        private readonly IServicioNotaDeEnvio _servNotaDeEnvio;
        private readonly IServicioProdVendido _servProdVendido;
        private readonly IServicioProducto _servProducto;
        private readonly IServicioCliente _servCliente;
        private readonly IServicioPedido _servPedido;
        private readonly IServicioDiaReparto _servDiaReparto;

        public FormNotasEnvio(IServicioNotaDeEnvio servNotaDeEnvio, IServicioCliente servCliente, IServicioProducto servProducto,
            IServicioDiaReparto servDiaReparto, IServicioProdVendido servProdVendido, IServicioPedido servPedido)
        {
            InitializeComponent();
            _lstNotaDeEnvios = new List<NotaDeEnvio>();
            _lstProdVendidos = new List<ProdVendido>();
            _lstProductos = new List<Producto>();
            
            _servNotaDeEnvio = servNotaDeEnvio;
            _servProdVendido = servProdVendido;
            _servProducto = servProducto;
            _servCliente = servCliente;
            _servPedido = servPedido;
            _servDiaReparto = servDiaReparto;
        }
        private void FormNotas_Load(object sender, EventArgs e)
        {
            GetNotas();
            GetProductos();
            if (_lstNotaDeEnvios != null)
            {
                dataGridView1.DataSource = _lstNotaDeEnvios.ToArray();
                dataGridView1.Columns[0].Width = 20;
                dataGridView1.Columns[1].Width = 30;
                dataGridView1.Columns[2].Width = 40;
                dataGridView1.Columns[3].Width = 30;
                dataGridView1.Columns[4].Width = 150;
                dataGridView1.Columns[5].Width = 40;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;

            }
            comboBox1.SelectedItem = "Todas";
            comboBox3.SelectedItem = "(Seleccionar)";
            
        }
        private void ActualizarGrid2(List<ProdVendido> lstProdVendidos)
        {
            dataGridView2.DataSource = _lstProdVendidos.ToArray();
            dataGridView2.Columns[0].Width = 30;
            //dataGridView2.Columns[1].Width = 30;
            dataGridView2.Columns[2].Width = 40;
        }
        private void GetNotas()
        {
            var notas = _servNotaDeEnvio.GetAll();
            if (notas != null)
            {
                _lstNotaDeEnvios = notas;
                lCantNotas.Text = _lstNotaDeEnvios.Count.ToString() + " notas de envio.";
            }
        }
        private void GuardarNota(NotaDeEnvio nota)
        {
            bool response = _servNotaDeEnvio.Add(nota);
            if (!response) MessageBox.Show("Algo falló al guardar Nota de Envío en la base de datos");
        }
        private void EliminarNotaDeEnvio(NotaDeEnvio nota)
        {
            bool response = _servNotaDeEnvio.Delete(nota);
            if (!response) MessageBox.Show("Algo falló al eliminar Nota de Envío de la base de datos");
            GetNotas();
        }
        private void GetProdVendidos()
        {
            _lstProdVendidos = _servProdVendido.GetAll();
        }
        private void AgregarProductoVendido(ProdVendido nuevoPV)
        {
            bool response = _servProdVendido.Add(nuevoPV);
            if (!response) MessageBox.Show("Algo falló al agregar Nota de Envío a la base de datos");
        }
        private void GetProductos()
        {
            _lstProductos = _servProducto.GetAll();
        }
        private Cliente GetClientePorDireccion(string direccion)
        {
            List<Cliente> lstClientes = _servCliente.GetAll();
            Cliente cliente = lstClientes.Find(x => x.Direccion.ToLower().Contains(direccion.ToLower()));
            return cliente;
        }
        private void AgregarPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            bool response = _servPedido.AgregarDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
            if (!response) MessageBox.Show("Algo falló al agregar Nota de Envío a la base de datos");
        }
        private DiaReparto GetDiaDeReparto(string diaDeReparto)
        {
            List<DiaReparto> dias = _servDiaReparto.GetAll();
            return dias.Find(x => x.Dia == diaDeReparto);
        }

        private void CopiaSeguridad_Click(object sender, EventArgs e)         // quitar diálogo, llevar a Exportar
        {
            //GetNotas();
            //DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel notas.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Notas de Envío a Excel", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
                //bool success = new Exportar().ExportarAExcel(notasEnvio);
                //if (success)
                //{
                //    bCopiaSeguridad.ForeColor = Color.Green;
                //    bCopiaSeguridad.Enabled = false;
                //    bCopiaSeguridad.Text = "Creacion exitosa";
                //}
                //else
                //{
                //    MessageBox.Show("Hubo un error al guardar los cambios.");
                //}
            //}
        }

        public void ImportarNotasDeEnvio_Click(object sender, EventArgs e)
        {}

        public void RecibirProductos(List<Producto> productos)
        {
            //listaProductos.AddRange(productos);
        }


        //_________________________FILTRAR DATOS_________________________________
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<NotaDeEnvio> lFiltrada = new List<NotaDeEnvio>();
            //todas - hoy - impresas- no impresas
            if (comboBox1.SelectedItem.ToString() == "Hoy") // cambio dps de este, 21/01/2021
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Today.ToString().Substring(0, 10))
                    {
                        lFiltrada.Add(nota);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                dataGridView1.DataSource = _lstNotaDeEnvios.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "Impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 1)
                    {
                        lFiltrada.Add(nota);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "No impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 0)
                    {
                        lFiltrada.Add(nota);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "Cliente")
            {
                label2.Text = "Dirección:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }

            if (comboBox1.SelectedItem.ToString() == "Fecha")
            {
                label2.Text = "Fecha:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }
        }

        void FiltrarDatos(string texto, char x)
        {
            List<NotaDeEnvio> ListaFiltrada = new List<NotaDeEnvio>();

            foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
            {
                if (x == 'c')
                {
                    if (nota.Client != null && nota.Client.Name.Contains(texto))
                    {
                        ListaFiltrada.Add(nota);
                    }
                }

                if (x == 'f')
                {
                    if (nota.Fecha.Contains(texto))
                    {
                        ListaFiltrada.Add(nota);
                    }
                }

            }
            dataGridView1.DataSource = ListaFiltrada.ToArray();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Cliente")
            {
                FiltrarDatos(textBox1.Text, 'c');
            }
            if (comboBox1.SelectedItem.ToString() == "Fecha")
            {
                FiltrarDatos(textBox1.Text, 'f');
            }
        }

        //_______________GRUPO IMPRIMIR ________________________________________
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "") return;
            foreach (NotaDeEnvio nota in ObtenerListaAImprimir())
            {
                try
                {
                    var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                    form.Rellenar_Datos(nota);
                    form.Show();
                }
                catch (Exception g)
                {
                    MessageBox.Show("Error al generar vista previa, rellenar los datos o mostrar la vista previa: " + g.Message);
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        public List<NotaDeEnvio> ObtenerListaAImprimir()
        {
            List<NotaDeEnvio> listaAImprimir = new List<NotaDeEnvio>();
            if (comboBox2.SelectedItem.ToString() == "No impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 0)
                    {
                        listaAImprimir.Add(nota);
                    }
                }
            }

            if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString("yy-MM-dd"))
                    {
                        listaAImprimir.Add(nota);
                    }
                }
            }

            if (comboBox2.SelectedItem.ToString() == "Establecer rango" && textBox2.Text != "" && textBox3.Text != "")
            {
                try
                {
                    int j = int.Parse(textBox3.Text);
                    for (int i = int.Parse(textBox2.Text); i <= j; i++)
                    {
                        listaAImprimir.Add(_lstNotaDeEnvios.Find(x => x.Id == i));
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Rango establecido incorrecto");
                }
            }
            return listaAImprimir;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox2.SelectedItem.ToString() == "No impresas") || ((comboBox2.SelectedItem.ToString() == "Hoy")))
            {
                textBox2.Visible = false;
                textBox3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox3.Text = "";
                textBox2.Text = "";
            }
            if (comboBox2.SelectedItem.ToString() == "Establecer rango")
            {
                textBox2.Visible = true;
                textBox3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }

        //Actualizar y Cerrar
        private void button2_Click(object sender, EventArgs e)
        {
            GetNotas();
            dataGridView1.DataSource = _lstNotaDeEnvios.ToArray();
        }

        private void FormNotas_FormClosing(object sender, FormClosingEventArgs e)
        {
            //GetNotas();
            //GuardarNotas();
        }

        //_________________________GRUPO BORRAR___________________________________
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Impresas"
                || comboBox3.SelectedItem.ToString() == "Todas"
                || comboBox3.SelectedItem.ToString() == "(Seleccionar)"
            )
            {
                textBox4.Visible = false;
                textBox5.Visible = false;
                label13.Visible = false;
                label12.Visible = false;
                textBox4.Text = "";
                textBox5.Text = "";
            }
            if (comboBox3.SelectedItem.ToString() == "Establecer rango")
            {
                textBox4.Visible = true;
                textBox5.Visible = true;
                label13.Visible = true;
                label12.Visible = true;
            }
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }

        public List<NotaDeEnvio> ObtenerListaABorrar()
        {
            List<NotaDeEnvio> listaABorrar = new List<NotaDeEnvio>();

            if (comboBox3.SelectedItem.ToString() == "Establecer rango" && textBox5.Text != "" && textBox4.Text != "")
            {
                try
                {
                    int j = int.Parse(textBox4.Text);
                    for (int i = int.Parse(textBox5.Text); i <= j; i++)
                    {
                        listaABorrar.Add(_lstNotaDeEnvios.Find(x => x.Id == i));
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Rango establecido incorrecto");
                }
            }
            if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                listaABorrar.AddRange(_lstNotaDeEnvios);
            }
            if (comboBox3.SelectedItem.ToString() == "Impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 1)
                    {
                        listaABorrar.Add(nota);
                    }
                }
            }
            return listaABorrar;
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Todas"
                || comboBox3.SelectedItem.ToString() == "Establecer rango"
                || comboBox3.SelectedItem.ToString() == "Impresas"
            )
            {
                MessageBox.Show("Confirme si desea borrar las notas de envio seleccionadas");
                label11.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button3.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetNotas();
            foreach (NotaDeEnvio nota in ObtenerListaABorrar())
            {
                EliminarNotaDeEnvio(nota);
            }
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            dataGridView1.DataSource = _lstNotaDeEnvios.ToArray();
        }

        //enviar a hoja de reparto
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((label16.Text != "") && (label16.Text != "No encontrado"))
            {
                button6.Enabled = true;
            }
            else
            {
                button6.Enabled = false;
            }
            comboBox5.DataSource = GetDiaDeReparto(comboBox4.Text);
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                try
                {
                    long id = int.Parse(textBox6.Text);
                    if (_lstNotaDeEnvios.Exists(x => x.Id == id))
                    {
                        label16.Text = _lstNotaDeEnvios.Find(x => x.Id == id).Client.Direccion;
                        if (comboBox5.Text != "")
                        {
                            button6.Enabled = true;
                        }
                        else
                        {
                            button6.Enabled = false;
                        }
                    }
                    else
                    {
                        label16.Text = "No encontrado";
                        button6.Enabled = false;
                    }
                }
                catch
                {
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }

        private void AgregarPedidoDesdeNota_Click(object sender, EventArgs e)
        {
            string diaDeReparto = comboBox4.Text;
            string nombreReparto = comboBox5.Text;
            long notaDeEnvioId = long.Parse(textBox6.Text);
            AgregarPedidoDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }


        //Modificar nota de envio
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != "")
            {
                try
                {
                    long id = long.Parse(textBox7.Text);
                    NotaDeEnvio notaDeEnvio = _lstNotaDeEnvios.Find(x => x.Id == id);
                    if (notaDeEnvio != null)
                    {
                        _lstProdVendidos = notaDeEnvio.ProdVendidos.ToList();
                        ActualizarGrid2(_lstProdVendidos);
                        double impTotal = 0;
                        foreach (ProdVendido nota in _lstProdVendidos)
                        {
                            impTotal += nota.Precio;
                        }
                        label20.Text = impTotal.ToString();
                        button10.Enabled = true;

                        if (notaDeEnvio.Client != null && notaDeEnvio.Client.Direccion != null)
                        {
                            label18.Text = notaDeEnvio.Client.Direccion.ToString() + " - " + notaDeEnvio.ClientId.ToString();
                        }
                    }
                    else
                    {
                        label18.Text = "No encontrado";
                        label20.Text = "0";
                        button10.Enabled = false;
                        _lstProdVendidos.Clear();
                        ActualizarGrid2(_lstProdVendidos);
                    }
                }
                catch {}
            }
            else
            {
                label18.Text = "";
                label20.Text = "0";
                button10.Enabled = false;
                _lstProdVendidos.Clear();
                ActualizarGrid2(_lstProdVendidos);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (_lstProdVendidos.Count != 0)
            {
                GetNotas();
                NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == long.Parse(textBox7.Text));
                nota = ServicioNotaDeEnvio.Modificar(nota, _lstProdVendidos);
                GuardarNota(nota);
                textBox7.Text = "";
                button10.Enabled = false;
                label18.Text = "";
                label20.Text = "0";
                _lstProdVendidos.Clear();
                dataGridView2.DataSource = _lstProdVendidos.ToArray();
                dataGridView1.DataSource = _lstNotaDeEnvios.ToArray();
            }
            else
            {
                MessageBox.Show("La nota de envío debe tener al menos un producto");
            }
        }

        //Quitar
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                if (_lstProdVendidos.Exists(x => x.Descripcion.Contains(textBox8.Text)))
                {
                    label22.Text = _lstProdVendidos.Find(x => x.Descripcion.Contains(textBox8.Text)).Descripcion;
                    button8.Enabled = true;
                    button10.Enabled = true;
                }
                else
                {
                    label22.Text = "No encontrado";
                    button8.Enabled = false;
                    button10.Enabled = false;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (label18.Text != "" && label18.Text != "No encontrado" && textBox7.Text != "")
            {
                _lstProdVendidos.Remove(_lstProdVendidos.Find(x => x.Descripcion == label22.Text));
                double impTotal = 0;
                foreach (ProdVendido producto in _lstProdVendidos)
                {
                    impTotal += producto.Precio;
                }
                label20.Text = impTotal.ToString();
                textBox8.Text = "";
                label22.Text = "";
                button8.Enabled = false;
                ActualizarGrid2(_lstProdVendidos);
            }
        }

        //agregar
        private void textBox9_Leave(object sender, EventArgs e)
        {
            if (textBox9.Text != "")
            {
                Producto producto = _lstProductos.Find(x => x.Id == int.Parse(textBox9.Text));
                if (producto != null)
                {
                    label25.Text = producto.Nombre;
                    if (textBox10.Text != "")
                    {
                        button9.Enabled = true;
                    }
                    if (label25.Text.Contains("actura"))
                    {
                        textBox11.Visible = true;
                    }
                    else
                    {
                        textBox11.Visible = false;
                    }
                }
                else
                {
                    label25.Text = "No encontrado";
                    button9.Enabled = true;
                }
            }
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            if ((textBox9.Text != "") && (label25.Text != "No encontrado"))
            {
                if (textBox10.Text != "")
                {
                    label26.Text = (_lstProductos.Find(x => x.Nombre == label25.Text).Precio * int.Parse(textBox10.Text)).ToString();
                    button9.Enabled = true;
                }
            }
        }

        private void AgregarProductoVendido_btn9_Click(object sender, EventArgs e)
        {
            ProdVendido nuevoPV;
            Producto prod = _lstProductos.Find(x => x.Nombre == label25.Text);
            if (prod.Nombre.Contains("pendiente"))
                nuevoPV = new ProdVendido
                {
                    ProductoId = prod.Id,
                    Descripcion = prod.Nombre,
                    Cantidad = 1,
                    Precio = prod.Precio
                };
            else if ((prod.Nombre.Contains("favor")) || (prod.Nombre.Contains("devoluci")) || (prod.Nombre.Contains("BONIF")))
                nuevoPV = new ProdVendido
                {
                    ProductoId = prod.Id,
                    Descripcion = prod.Nombre,
                    Cantidad = 1,
                    Precio = prod.Precio * -1
                };
            else if ((prod.Nombre.Contains("actura")))
                nuevoPV = new ProdVendido
                {
                    ProductoId = prod.Id,
                    Descripcion = prod.Nombre + textBox11.Text,
                    Cantidad = 1,
                    Precio = prod.Precio
                };
            else
                nuevoPV = new ProdVendido
                {
                    ProductoId = prod.Id, Descripcion = prod.Nombre, Cantidad = int.Parse(textBox10.Text), Precio = prod.Precio
                };
            AgregarProductoVendido(nuevoPV);
            ActualizarGrid2(_lstProdVendidos);
            double impTotal = 0;
            foreach (ProdVendido prodVendido in _lstProdVendidos)
            {
                impTotal += prodVendido.Precio;
            }
            label20.Text = impTotal.ToString();
            label25.Text = "";
            label26.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox11.Visible = false;
            button9.Enabled = false;
        }

        private void bExportar_Click(object sender, EventArgs e)
        {
        }
    }
}
