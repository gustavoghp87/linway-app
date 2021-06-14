using linway_app.Models;
using linway_app.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DNotaDeEnvio;
using static linway_app.Services.Delegates.DPedido;
using static linway_app.Services.Delegates.DProductos;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DReparto;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private List<NotaDeEnvio> _lstNotaDeEnvios;
        private List<ProdVendido> _lstProdVendidos;
        public FormNotasEnvio()
        {
            InitializeComponent();
            _lstNotaDeEnvios = new List<NotaDeEnvio>();
            _lstProdVendidos = new List<ProdVendido>();
        }
        private void FormNotas_Load(object sender, EventArgs e)
        {
            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
        }
        private void ActualizarNotas()
        {
            _lstNotaDeEnvios = getNotaDeEnvios();
            if (_lstNotaDeEnvios != null) lCantNotas.Text = _lstNotaDeEnvios.Count.ToString() + " notas de envio.";
        }
        private void ActualizarGrid1(List<NotaDeEnvio> lstNotadeEnvios)
        {
            if (lstNotadeEnvios != null)
            {
                List<ENotaDeEnvio> grid = new List<ENotaDeEnvio>();
                foreach (NotaDeEnvio nota in lstNotadeEnvios)
                {
                    grid.Add(Form1._mapper.Map<ENotaDeEnvio>(nota));
                }
                dataGridView1.DataSource = grid;
                dataGridView1.Columns[0].Width = 20;
                dataGridView1.Columns[1].Width = 70;
                dataGridView1.Columns[2].Width = 170;
                dataGridView1.Columns[3].Width = 350;
            }
            comboBox1.SelectedItem = "Todas ??";
            comboBox3.SelectedItem = "(Seleccionar)";
        }
        private void ActualizarGrid2(List<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos != null)
            {
                List<EProdVendido> grid = new List<EProdVendido>();
                foreach (ProdVendido prodVendido in lstProdVendidos)
                {
                    grid.Add(Form1._mapper.Map<EProdVendido>(prodVendido));
                }
                dataGridView2.DataSource = grid;
                dataGridView2.Columns[0].Width = 25;
                dataGridView2.Columns[1].Width = 200;
            }
        }


        //_________________________FILTRAR DATOS_________________________________
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<NotaDeEnvio> lFiltrada = new List<NotaDeEnvio>();
            //todas - hoy - impresas- no impresas
            if (comboBox1.SelectedItem.ToString() == "Hoy")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.UtcNow.ToString("yyyy-MM-dd")) lFiltrada.Add(nota);
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
                    if (nota.Impresa == 1) lFiltrada.Add(nota);
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
                    if (nota.Impresa == 0) lFiltrada.Add(nota);
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
        void FiltrarDatos(string texto, char x)
        {
            ActualizarNotas();
            List<NotaDeEnvio> lstFiltrada = new List<NotaDeEnvio>();
            foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
            {
                if (x == 'c' && nota.Cliente != null
                    && nota.Cliente.Direccion != null
                    && nota.Cliente.Direccion.ToLower().Contains(texto.ToLower())
                ) lstFiltrada.Add(nota);
                if (x == 'f' && nota.Fecha.Contains(texto)) lstFiltrada.Add(nota);
            }
            ActualizarGrid1(lstFiltrada);
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Cliente") FiltrarDatos(textBox1.Text, 'c');
            if (comboBox1.SelectedItem.ToString() == "Fecha") FiltrarDatos(textBox1.Text, 'f');
        }

        //_______________GRUPO IMPRIMIR ________________________________________
        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "") return;
            foreach (NotaDeEnvio nota in ObtenerListaAImprimir())
            {
                var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nota);
                form.Show();
                Close();
            }
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
        }
        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
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
                    if (nota.Impresa == 0) listaAImprimir.Add(nota);
                }
            }
            if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString("yy-MM-dd")) listaAImprimir.Add(nota);
                }
            }
            if (
                comboBox2.SelectedItem.ToString() == "Establecer rango"
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
                catch { MessageBox.Show("Rango establecido incorrecto"); }
            }
            return listaAImprimir;
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
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
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }

        
        //_________________________GRUPO BORRAR___________________________________
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
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
            if (
                comboBox3.SelectedItem.ToString() == "Establecer rango"
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
                catch { MessageBox.Show("Rango establecido incorrecto"); }
            }
            if (comboBox3.SelectedItem.ToString() == "Todas") listaABorrar.AddRange(_lstNotaDeEnvios);
            if (comboBox3.SelectedItem.ToString() == "Impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 1) listaABorrar.Add(nota);
                }
            }
            return listaABorrar;
        }
        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            if (
                comboBox3.SelectedItem.ToString() == "Todas"
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
        private void Button5_Click(object sender, EventArgs e)
        {
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }
        private void Button4_Click(object sender, EventArgs e)
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
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (label16.Text != "" && label16.Text != "No encontrado") button6.Enabled = true;
            else button6.Enabled = false;
            List<Reparto> lstRepartos = getRepartosPorDia(comboBox4.Text);
            if (lstRepartos != null && lstRepartos.Count != 0)
            {
                comboBox5.DataSource = lstRepartos;
                comboBox5.DisplayMember = "Nombre";
                comboBox5.ValueMember = "Nombre";
            }
            else
            {
                comboBox5.DataSource = null;
                comboBox5.Text = "";
            }
        }
        private void TextBox6_TextChanged(object sender, EventArgs e)    // búsqueda por id de la nota de envío para agregar a pedido
        {
            if (textBox6.Text != "")
            {
                try { long.Parse(textBox6.Text); } catch { return; };
                long id = long.Parse(textBox6.Text);
                NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == id);
                if (nota != null)
                {
                    label16.Text = nota.Cliente.Direccion;
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
        private void Button7_Click(object sender, EventArgs e)     // limpiar
        {
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }
        private void AgregarPedidoDesdeNota_Click(object sender, EventArgs e)
        {
            try { long.Parse(textBox6.Text); } catch { return; };
            string diaDeReparto = comboBox4.Text;
            string nombreReparto = comboBox5.Text;
            long notaDeEnvioId = long.Parse(textBox6.Text);
            addPedidoDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }


        // Agregar o Quitar producto vendido de nota de envio
        private void TextBox7_TextChanged(object sender, EventArgs e)
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
                    decimal impTotal = 0;
                    foreach (ProdVendido nota in _lstProdVendidos)
                    {
                        impTotal += nota.Cantidad * nota.Precio;
                    }
                    label20.Text = impTotal.ToString();
                    button10.Enabled = true;
                    if (notaDeEnvio.Cliente != null && notaDeEnvio.Cliente.Direccion != null)
                    {
                        label18.Text = notaDeEnvio.Cliente.Direccion.ToString() +
                            " - " + notaDeEnvio.ClienteId.ToString();
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
        // Agregar
        private void TextBox9_TextChanged(object sender, EventArgs e)     // id producto
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
        private void TextBox12_TextChanged(object sender, EventArgs e)     // producto por nombre
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
        private void TextBox10_TextChanged(object sender, EventArgs e)     // cantidad a agregar
        {
            if (label25.Text != "No encontrado" && textBox10.Text != "")
            {
                try { int.Parse(textBox10.Text); } catch { return; };
                label26.Text = (getProductoPorNombreExacto(label25.Text).Precio * int.Parse(textBox10.Text)).ToString();
                button9.Enabled = true;
            }
        }
        private void AgregarProductoVendido_btn9_Click(object sender, EventArgs e)
        {
            try { long.Parse(textBox7.Text); } catch { return; };
            ProdVendido prodVendido = getProdVendidoPorNombreExacto(label25.Text);
            if (prodVendido == null) return;
            try { long.Parse(textBox7.Text); } catch { return; };
            NotaDeEnvio notaDeEnvio = getNotaDeEnvio(long.Parse(textBox7.Text));
            _lstProdVendidos.Add(prodVendido);





            //ProdVendido nuevoPV = new ProdVendido();
            //nuevoPV.NotaDeEnvioId = getNotaDeEnvio(long.Parse(textBox7.Text)).Id;
            //nuevoPV.Descripcion = prod.Nombre;
            //nuevoPV.Cantidad = 1;
            //nuevoPV.Precio = prod.Precio;
            //nuevoPV.ProductoId = prod.Id;
            //if (prod == null) return;
            //if (prod.Nombre.Contains("pendiente")) { }
            ////
            //else if (
            //    prod.Nombre.Contains("favor")
            //    || prod.Nombre.Contains("devoluci")
            //    || prod.Nombre.Contains("BONIF")
            //)
            //    nuevoPV.Precio = prod.Precio * -1;
            //else if ((prod.Nombre.Contains("actura")))
            //    nuevoPV.Descripcion = prod.Nombre + textBox11.Text;
            //else
            //{
            //    try { int.Parse(textBox10.Text); } catch { return; };
            //    nuevoPV.Cantidad = int.Parse(textBox10.Text);
            //}







            notaDeEnvio = editNotaDeEnvioAgregar(notaDeEnvio, _lstProdVendidos);
            ActualizarGrid2(_lstProdVendidos);
            decimal impTotal = 0;
            foreach (ProdVendido prodVend in _lstProdVendidos)
            {
                impTotal += prodVend.Cantidad * prodVend.Precio;
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
        //Quitar
        private void TextBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                ProdVendido prodVendido = _lstProdVendidos.Find(x => x.Producto.Nombre.ToLower().Contains(textBox8.Text.ToLower()));
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
        private void Button8_Click(object sender, EventArgs e)
        {
            if (label18.Text != "" && label18.Text != "No encontrado" && textBox7.Text != "")
            {
                try { long.Parse(textBox7.Text); } catch { return; };
                NotaDeEnvio notaDeEnvio = getNotaDeEnvio(long.Parse(textBox7.Text));
                ProdVendido prodVendido = getProdVendidoPorNombre(label22.Text);
                if (notaDeEnvio == null || prodVendido == null) return;
                notaDeEnvio = editNotaDeEnvioQuitar(notaDeEnvio, prodVendido);
                prodVendido.NotaDeEnvioId = null;
                editProdVendido(prodVendido);
                ActualizarNotas();
                ActualizarGrid1(_lstNotaDeEnvios);
                ActualizarGrid2(notaDeEnvio.ProdVendidos.ToList());
                label20.Text = notaDeEnvio.ImporteTotal.ToString();
                textBox8.Text = "";
                label22.Text = "";
                button8.Enabled = false;
            }
        }

        // MODIFICAR BTN
        private void Button10_Click(object sender, EventArgs e)
        {
            if (_lstProdVendidos.Count != 0)
            {
                NotaDeEnvio nota = getNotaDeEnvio(long.Parse(textBox7.Text));
                if (nota == null) return;
                nota = editNotaDeEnvioAgregar(nota, _lstProdVendidos);
                addNotaDeEnvio(nota);
                ActualizarNotas();
                ActualizarGrid1(_lstNotaDeEnvios);
                _lstProdVendidos.Clear();
                ActualizarGrid2(_lstProdVendidos);
                textBox7.Text = "";
                button10.Enabled = false;
                label18.Text = "";
                label20.Text = "0";
            }
            else MessageBox.Show("La nota de envío debe tener al menos un producto");
        }
    }
}
