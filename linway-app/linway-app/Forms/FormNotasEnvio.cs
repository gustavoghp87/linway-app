using linway_app.Models;
using linway_app.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Forms.Delegates.DDiaReparto;
using static linway_app.Forms.Delegates.DNotaDeEnvio;
using static linway_app.Forms.Delegates.DPedido;
using static linway_app.Forms.Delegates.DProductos;
using static linway_app.Forms.Delegates.DProdVendido;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private List<NotaDeEnvio> _lstNotaDeEnvios;
        private List<ProdVendido> _lstProdVendidos;
        private List<Producto> _lstProductos;

        public FormNotasEnvio()
        {
            InitializeComponent();
            _lstNotaDeEnvios = new List<NotaDeEnvio>();
            _lstProdVendidos = new List<ProdVendido>();
            _lstProductos = new List<Producto>();
        }
        private void FormNotas_Load(object sender, EventArgs e)
        {
            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
            _lstProductos = getProductos();
        }
        private void ActualizarNotas()
        {
            _lstNotaDeEnvios = getNotaDeEnvios();
            if (_lstNotaDeEnvios != null)
            {
                lCantNotas.Text = _lstNotaDeEnvios.Count.ToString() + " notas de envio.";
            }
        }
        private void ActualizarGrid1(List<NotaDeEnvio> lstNotadeEnvios)
        {
            ActualizarNotas();
            dataGridView1.DataSource = lstNotadeEnvios.ToArray();
            if (lstNotadeEnvios != null)
            {
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
            comboBox1.SelectedItem = "Todas ??";
            comboBox3.SelectedItem = "(Seleccionar)";
        }
        private void ActualizarGrid2(List<ProdVendido> lstProdVendidos)
        {
            dataGridView2.DataSource = lstProdVendidos.ToArray();
            if (lstProdVendidos != null)
            {
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[2].Visible = false;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[8].Visible = false;
                dataGridView2.Columns[9].Visible = false;
                dataGridView2.Columns[10].Visible = false;
                dataGridView2.Columns[11].Visible = false;
                dataGridView2.Columns[12].Visible = false;
            }
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
                ActualizarGrid1(lFiltrada);
            }

            if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                ActualizarNotas();
                ActualizarGrid1(_lstNotaDeEnvios);
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
                ActualizarGrid1(lFiltrada);
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
                ActualizarGrid1(lFiltrada);
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

        void FiltrarDatos(string texto, char x)                             // c de cliente
        {
            ActualizarNotas();
            List<NotaDeEnvio> lstFiltrada = new List<NotaDeEnvio>();
            foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
            {
                if (x == 'c')
                {
                    if (nota.Client != null
                        && nota.Client.Direccion != null
                        && nota.Client.Direccion.ToLower().Contains(texto.ToLower())
                    )
                    {
                        lstFiltrada.Add(nota);
                    }
                }
                if (x == 'f')
                {
                    if (nota.Fecha.Contains(texto))
                    {
                        lstFiltrada.Add(nota);
                    }
                }

            }
            ActualizarGrid1(lstFiltrada);
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
                    Close();
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

            if (comboBox2.SelectedItem.ToString() == "Establecer rango"
                && textBox2.Text != ""
                && textBox3.Text != ""
            )
            {
                try
                {
                    int j = int.Parse(textBox3.Text);
                    for (int i = int.Parse(textBox2.Text); i <= j; i++)
                    {
                        NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == i);
                        if (nota != null) listaAImprimir.Add(nota);
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
            if (comboBox2.SelectedItem.ToString() == "No impresas" || comboBox2.SelectedItem.ToString() == "Hoy")
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
            ActualizarNotas();
            List<NotaDeEnvio> listaABorrar = new List<NotaDeEnvio>();

            if (comboBox3.SelectedItem.ToString() == "Establecer rango"
                && textBox5.Text != ""
                && textBox4.Text != ""
            )
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
            foreach (NotaDeEnvio nota in ObtenerListaABorrar())
            {
                deleteNotaDeEnvio(nota);
            }
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
        }

        //enviar a hoja de reparto
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (label16.Text != "" && label16.Text != "No encontrado")
            {
                button6.Enabled = true;
            }
            else
            {
                button6.Enabled = false;
            }
            getRepartosPorDia(comboBox4.Text);
            comboBox5.DataSource = getRepartosPorDia(comboBox4.Text);
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
        }
        private void textBox6_TextChanged(object sender, EventArgs e)    // búsqueda por id de la nota de envío para agregar a pedido
        {
            if (textBox6.Text != "")
            {
                try { long.Parse(textBox6.Text); } catch { return; };
                long id = long.Parse(textBox6.Text);
                NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == id);
                if (nota != null)
                {
                    label16.Text = nota.Client.Direccion;
                    if (comboBox5.Text != "") button6.Enabled = true;
                    else button6.Enabled = false;
                }
                else
                {
                    label16.Text = "No encontrado";
                    button6.Enabled = false;
                }
            }
            else
            {
                label16.Text = "";
                button6.Enabled = false;
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
            try { long.Parse(textBox6.Text); } catch { return; };
            long notaDeEnvioId = long.Parse(textBox6.Text);
            addPedidoDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
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
                try { long.Parse(textBox7.Text); } catch { return; };
                NotaDeEnvio notaDeEnvio = getNotaDeEnvio(long.Parse(textBox7.Text));
                if (notaDeEnvio != null)
                {
                    if (notaDeEnvio.ProdVendidos == null) return;
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
                        label18.Text = notaDeEnvio.Client.Direccion.ToString() +
                            " - " + notaDeEnvio.ClientId.ToString();
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
                NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == long.Parse(textBox7.Text));
                nota = ServicioNotaDeEnvio.Modificar(nota, _lstProdVendidos);
                addNotaDeEnvioReturnId(nota);
                ActualizarNotas();
                ActualizarGrid1(_lstNotaDeEnvios);
                _lstProdVendidos.Clear();
                ActualizarGrid2(_lstProdVendidos);
                textBox7.Text = "";
                button10.Enabled = false;
                label18.Text = "";
                label20.Text = "0";
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
                ProdVendido prodVendido = getProdVendidoPorNombre(textBox8.Text);
                if (prodVendido != null)
                {
                    label22.Text = prodVendido.Descripcion;
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
            else
            {
                label22.Text = "";
                button8.Enabled = false;
                button10.Enabled = false;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (label18.Text != "" && label18.Text != "No encontrado" && textBox7.Text != "")
            {
                ProdVendido prodVendido = getProdVendidoPorNombreExacto(label22.Text);
                deleteProdVendido(prodVendido);
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
        private void textBox9_TextChanged(object sender, EventArgs e)     // id producto
        {
            label26.Text = "";
            if (textBox9.Text != "")
            {
                try { long.Parse(textBox9.Text); } catch { return; };
                Producto producto = getProducto(long.Parse(textBox9.Text));
                if (producto != null)
                {
                    label25.Text = producto.Nombre;
                    if (textBox10.Text != "") button9.Enabled = true;
                    if (label25.Text.Contains("actura")) textBox11.Visible = true;
                    else textBox11.Visible = false;
                }
                else
                {
                    label25.Text = "No encontrado";
                    button9.Enabled = true;
                }
            }
            else
            {
                label25.Text = "";
                button9.Enabled = true;
            }
        }
        private void textBox12_TextChanged(object sender, EventArgs e)     // producto por nombre
        {
            label26.Text = "";
            if (textBox12.Text != "")
            {
                Producto producto = getProductoPorNombre(textBox12.Text);
                if (producto != null)
                {
                    label25.Text = producto.Nombre;
                    if (textBox10.Text != "") button9.Enabled = true;
                    if (label25.Text.Contains("actura")) textBox11.Visible = true;
                    else textBox11.Visible = false;
                }
                else
                {
                    label25.Text = "No encontrado";
                    button9.Enabled = true;
                }
            }
            else
            {
                label25.Text = "";
                button9.Enabled = true;
            }
        }
        private void textBox10_TextChanged(object sender, EventArgs e)     // cantidad a agregar
        {
            if (label25.Text != "No encontrado" && textBox10.Text != "")
            {
                try { int.Parse(textBox10.Text); } catch { return; };
                label26.Text = (getProductoPorNombreExacto(label25.Text).Precio * int.Parse(textBox10.Text))
                    .ToString();
                button9.Enabled = true;
            }
        }
        private void AgregarProductoVendido_btn9_Click(object sender, EventArgs e)
        {
            try { long.Parse(textBox7.Text); } catch { return; };
            Producto prod = getProductoPorNombreExacto(label25.Text);
            ProdVendido nuevoPV = new ProdVendido();
            nuevoPV.NotaDeEnvioId = getNotaDeEnvio(long.Parse(textBox7.Text)).Id;
            nuevoPV.Descripcion = prod.Nombre;
            nuevoPV.Cantidad = 1;
            nuevoPV.Precio = prod.Precio;
            nuevoPV.Estado = "Activo";
            nuevoPV.ProductoId = prod.Id;
            if (prod == null) return;
            if (prod.Nombre.Contains("pendiente")) { }
            //
            else if (prod.Nombre.Contains("favor")
                    || prod.Nombre.Contains("devoluci")
                    || prod.Nombre.Contains("BONIF")
            )
                nuevoPV.Precio = prod.Precio * -1;
            else if ((prod.Nombre.Contains("actura")))
                nuevoPV.Descripcion = prod.Nombre + textBox11.Text;
            else
            {
                try { int.Parse(textBox10.Text); } catch { return; };
                nuevoPV.Cantidad = int.Parse(textBox10.Text);
            }
            addProdVendido(nuevoPV);
            _lstProdVendidos = getNotaDeEnvio(long.Parse(textBox7.Text)).ProdVendidos.ToList();
            ActualizarGrid2(_lstProdVendidos);
            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
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
    }
}
