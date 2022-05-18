using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DDiaReparto;
using static linway_app.Services.Delegates.DPedido;
using static linway_app.Services.Delegates.DReparto;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private List<Reparto> _lstRepartos;
        private List<Reparto> _lstRepartosFiltrada;
        private List<Pedido> _lstPedidos;
        private List<DiaReparto> _lstDiaRepartos;
        public FormRepartos()
        {
            _lstRepartos = new List<Reparto>();
            _lstRepartosFiltrada = new List<Reparto>();
            _lstPedidos = new List<Pedido>();
            _lstDiaRepartos = new List<DiaReparto>();
            InitializeComponent();
        }
        private void FormReparto_Load(object sender, EventArgs e)
        {
            Actualizar();
            checkBox1.Checked = true;
        }
        private void Actualizar()
        {
            _lstDiaRepartos = getDiaRepartos();
            if (_lstDiaRepartos == null || _lstDiaRepartos.Count == 0) CrearDias();
            UpdateGrid();
        }
        private void CrearDias()
        {
            var nuevoDia = new DiaReparto();
            string[] dias = new string[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            foreach (string dia in dias)
            {
                nuevoDia.Dia = dia;
                addDiaReparto(nuevoDia);
            }
            _lstDiaRepartos = getDiaRepartos();
            if (_lstDiaRepartos == null || _lstDiaRepartos.Count == 0)
            {
                MessageBox.Show("Algo falla con la base de datos");
                Close();
            }
        }
        private void ActualizarGrid(ICollection<Pedido> lstPedidos)
        {
            if (lstPedidos != null)
            {
                var grid1 = new List<EPedido>();
                foreach (Pedido pedido in lstPedidos)
                {
                    grid1.Add(Form1.mapper.Map<EPedido>(pedido));
                }
                grid1 = grid1.OrderBy(x => x.Orden).ToList();
                dataGridView1.DataSource = grid1;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Width = 37;
                dataGridView1.Columns[2].Width = 230;
                dataGridView1.Columns[3].Width = 320;
                dataGridView1.Columns[4].Width = 53;
                dataGridView1.Columns[5].Width = 30;
                dataGridView1.Columns[6].Width = 30;
                dataGridView1.Columns[7].Width = 30;
                dataGridView1.Columns[8].Width = 30;
                dataGridView1.Columns[9].Width = 30;
                dataGridView1.Columns[11].Visible = false;      // orden
            }
        }
        private void ReCargarHDR(string elDia, string elReparto)
        {
            Actualizar();
            _lstRepartos = getRepartosPorDia(elDia);
            long repartoId = _lstRepartos.Find(x => x.Nombre == elReparto).Id;
            _lstPedidos = getPedidosPorRepartoId(repartoId).OrderBy(x => x.Orden).ToList();
        }
        private void Exportar_Click(object sender, EventArgs e)
        {
            string dia = comboBox1.Text;
            string nombreReparto = comboBox2.Text;
            if (dia == "" || nombreReparto == "") return;
            bool success = false;
            DialogResult dialogResult = MessageBox.Show("Exportar " + dia + " - "
                + nombreReparto + " ¿Confirmar?", "Exportar Reparto a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) success = exportReparto(dia, nombreReparto);
            if (success) exportarButton.Text = "Terminado";
        }
        private void LimpiarPantalla()
        {
            gpNuevoReparto.Visible = false;
            groupBox2.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox9.Visible = false;
            label30.Text = "";
            label31.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            textBox1.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            label8.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            comboBox8.Text = "";
            comboBox9.Text = "";
            comboBox10.Text = "";
            //checkBox1.Checked = false;
            label32.Text = "";
            label36.Text = "";
        }


        //____________________Ejegir Hoja de Reparto______________________________
        private void VerDatos(Reparto reparto)
        {
            if (reparto == null) return;
            label14.Text = reparto.Ta.ToString();
            label15.Text = reparto.Te.ToString();
            label16.Text = reparto.Td.ToString();
            label17.Text = reparto.Tt.ToString();
            label18.Text = reparto.Tae.ToString();
            label21.Text = reparto.TotalB.ToString();
            label22.Text = reparto.Tl.ToString() + " litros";
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            label2.Visible = true;
            ActualizarGrid(new List<Pedido>());
            if (_lstDiaRepartos.Count == 0) return;
            _lstRepartos = getRepartosPorDia(comboBox1.SelectedItem.ToString());
            comboBox2.DataSource = _lstRepartos.Count > 0 ? _lstRepartos : null;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
        }
        private void UpdateGrid()
        {
            if (comboBox1.Text != "")
            {
                Reparto reparto = _lstRepartos.Find(x => x.Nombre.Equals(comboBox2.Text));
                if (reparto == null) return;
                if (checkBox1.Checked)
                    _lstPedidos = reparto.Pedidos.Where(x => x.Entregar == 1).ToList();
                else
                    _lstPedidos = reparto.Pedidos.ToList();
                VerDatos(reparto);
                ActualizarGrid(_lstPedidos);
            }
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        // MENUES
        private void AgregarReparto_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gpNuevoReparto.Visible = true;
            comboBox3.DataSource = _lstDiaRepartos;
            comboBox3.DisplayMember = "Dia";
            comboBox3.ValueMember = "Dia";
        }

        private void AgregarDestino_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox2.Visible = true;
        }

        //_______________AGREGAR Reparto A DIA______________________________-
        private void AgregarRepartoADIA_btn2_Click(object sender, EventArgs e)
        {
            Actualizar();
            if (textBox1.Text != "")
            {
                DiaReparto diaRep = _lstDiaRepartos.Find(x => x.Dia.Contains(comboBox3.Text));
                Reparto nuevoReparto = new Reparto
                {
                    Nombre = textBox1.Text,
                    DiaRepartoId = diaRep.Id,
                    Ta = 0,
                    Tae = 0,
                    Td = 0,
                    Te = 0,
                    Tl = 0,
                    TotalB = 0,
                    Tt = 0
                };
                addReparto(nuevoReparto);
            }
            LimpiarPantalla();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }

        //__________________________AGREGAR DESTINO--_____________________
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lstRepartosFiltrada = _lstDiaRepartos.Find(x => x.Dia == comboBox4.SelectedItem.ToString()).Reparto.ToList();
            comboBox5.DataSource = _lstRepartosFiltrada;
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)  // Agregar destino a recorrido
        {
            if (textBox2.Text != "")
            {
                try { long.Parse(textBox2.Text); } catch { return; };
                Cliente cliente = getCliente(long.Parse(textBox2.Text));
                if (cliente != null) label8.Text = cliente.Direccion;
                else label8.Text = "no encontrado";
            }
            else label8.Text = "no encontrado";
        }
        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                Cliente cliente = getClientePorDireccion(textBox6.Text);
                if (cliente != null) label8.Text = cliente.Direccion;
                else label8.Text = "no encontrado";
            }
            else label8.Text = "no encontrado";
        }
        private void AgregarDestinoAReparto_btn1_Click(object sender, EventArgs e)
        {
            if (label8.Text != "No encontrado" && comboBox4.Text != "")
            {
                ReCargarHDR(comboBox4.Text, comboBox5.Text);                // día y reparto
                comboBox1.SelectedIndex = comboBox4.SelectedIndex;
                comboBox2.SelectedIndex = comboBox5.SelectedIndex;
                Cliente cliente = getClientePorDireccionExacta(label8.Text);
                Reparto reparto = _lstRepartos.Find(x => x.Nombre.Equals(comboBox5.Text));
                if (cliente == null || reparto == null) { MessageBox.Show("Falló algo, código 44"); return; };
                // este agrega Destino sin productos
                Pedido nuevoPedido = new Pedido
                {
                    ClienteId = cliente.Id,
                    Direccion = cliente.Direccion,
                    RepartoId = reparto.Id,
                    ProductosText = "",
                    Entregar = 0,
                    A = 0,
                    Ae = 0,
                    D = 0,
                    E = 0,
                    L = 0,
                    T = 0
                };
                // corroborar que no esté antes de agregar   <----------
                if (_lstPedidos.Exists(x => x.ClienteId == nuevoPedido.ClienteId && x.RepartoId == nuevoPedido.RepartoId))
                {
                    MessageBox.Show("Ese cliente ya estaba en el reparto");
                    return;
                };
                addPedido(nuevoPedido);
                LimpiarPantalla();
                Actualizar();
            }
            else MessageBox.Show("Error, verifique los campos");
        }


        //_LImpiar DATOS_ 
        // 1. limpiar los repartos de todos los dias
        private void TodasLuAVi_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox4.Visible = true;
        }
        private void LimpiarRepartos_Click(object sender, EventArgs e)
        {
            Form1.loadingForm.OpenIt();
            var repartosALimpiar = new List<Reparto>();
            foreach (DiaReparto diaReparto in _lstDiaRepartos)
            {
                foreach (Reparto reparto in diaReparto.Reparto)
                {
                    repartosALimpiar.Add(reparto);
                }
            }
            cleanRepartos(repartosALimpiar);
            LimpiarPantalla();
            Actualizar();
            Form1.loadingForm.CloseIt();
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }

        // 2. limpiar los repartos de un dia
        private void DiaEspecífico_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox5.Visible = true;
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            if (comboBox6.Text != "")
            {
                Form1.loadingForm.OpenIt();
                Actualizar();
                try
                {
                    List<Reparto> lstRepartos = _lstDiaRepartos.Find(x => x.Dia == comboBox6.Text).Reparto.ToList();
                    cleanRepartos(lstRepartos);
                    LimpiarPantalla();
                    Actualizar();
                }
                catch { return; }
                Form1.loadingForm.CloseIt();
            }
            else MessageBox.Show("Debe seleccionar un día");
        }

        // 3. limpiar un reparto
        private void RepartoSeleccionado_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox6.Visible = true;
        }
        private void ComboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dia = comboBox8.SelectedItem.ToString();
            _lstRepartosFiltrada = _lstDiaRepartos.Find(x => x.Dia == dia).Reparto.ToList();
            comboBox7.DataSource = _lstRepartosFiltrada;
            comboBox7.DisplayMember = "Nombre";
            comboBox7.ValueMember = "Nombre";
        }
        private void Button11_Click(object sender, EventArgs e)
        {
            if (comboBox8.Text != "")
            {
                Form1.loadingForm.OpenIt();
                string dia = comboBox8.Text;
                string nombre = comboBox7.Text;
                Reparto reparto = _lstDiaRepartos
                    .Find(x => x.Dia == dia).Reparto.ToList()
                    .Find(x => x.Nombre == nombre);
                comboBox1.SelectedIndex = comboBox8.SelectedIndex;
                comboBox2.SelectedIndex = comboBox7.SelectedIndex;
                cleanRepartos(new List<Reparto>() { reparto });
                VerDatos(reparto);
                LimpiarPantalla();
                Actualizar();
                Form1.loadingForm.CloseIt();
            }
            else MessageBox.Show("Debe seleccionar un día");
        }

        // 4. limpiar un pedido
        private void Destino_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox9.Visible = true;
            label39.Text = "Día " + comboBox1.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void TextBox7_TextChanged(object sender, EventArgs e)
        {
            Pedido pedido = _lstPedidos
                .Find(x => x.Direccion.ToLower().Contains(textBox7.Text.ToLower()) && x.Estado != null && x.Estado != "Eliminado");
            if (pedido != null) label36.Text = pedido.Direccion;
            else label36.Text = "No encontrado";
        }
        private void Button18_Click(object sender, EventArgs e)
        {
            ReCargarHDR(comboBox1.Text, comboBox2.Text);
            //string direcCliente = label36.Text;
            //string nombreRep = comboBox2.Text;
            string direccion = label36.Text;
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.Equals(direccion));
            if (pedido != null)
            {
                Form1.loadingForm.OpenIt();
                Reparto reparto = getReparto(pedido.RepartoId);
                substractPedidoAReparto(reparto, pedido);
                cleanPedidos(new List<Pedido>() { pedido });
                VerDatos(getReparto(pedido.RepartoId));
                LimpiarPantalla();
                Actualizar();
                Form1.loadingForm.CloseIt();
            }
            else MessageBox.Show("Verifique que los datos sean correctos");
        }


        //__________REPOSICIONAR DESTINO____________________
        private void PosicionarDestino_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
            label27.Text = "Día " + comboBox1.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            var auxList = _lstPedidos.Where(x => x.Entregar == 1).ToList();
            var aux = auxList.Find(x => x.Direccion.ToLower().Contains(textBox3.Text.ToLower()));
            if (aux == null) label30.Text = "No encontrado";
            else label30.Text = aux.Direccion;
        }
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            var auxList = _lstPedidos.Where(x => x.Entregar == 1).ToList();
            var aux = auxList.Find(x => x.Direccion.ToLower().Contains(textBox4.Text.ToLower()));
            if (aux == null) label31.Text = "No encontrado";
            else label31.Text = aux.Direccion;
        }
        private void Button14_Click(object sender, EventArgs e)           //   aceptar
        {
            if (
                label30.Text != "No encontrado" && label30.Text != "" &&
                label31.Text != "No encontrado" && label31.Text != ""
            )
            {
                try
                {
                    Form1.loadingForm.OpenIt();
                    string pedidoAMover = label30.Text;
                    string pedidoReferencia = label31.Text;
                    Reparto reparto = _lstRepartos.Find(x => x.DiaReparto.Dia == comboBox1.Text && x.Nombre == comboBox2.Text);
                    Pedido pedido1 = reparto.Pedidos.ToList().Find(x => x.Direccion == pedidoAMover);
                    Pedido pedido2 = reparto.Pedidos.ToList().Find(x => x.Direccion == pedidoReferencia);
                    long order1 = pedido1.Orden;
                    long order2 = pedido2.Orden;
                    if (order2 == order1) return;
                    if (order2 > order1)
                    {
                        foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                        {
                            if (pedido.Orden > order1 && pedido.Orden < order2)
                            {
                                pedido.Orden -= 1;
                                editPedido(pedido);
                            }
                        }
                        pedido1.Orden = pedido2.Orden;
                        pedido2.Orden -= 1;
                        editPedido(pedido1);
                        editPedido(pedido2);
                    }
                    else
                    {
                        foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                        {
                            if (pedido.Orden < order1 && pedido.Orden > order2)
                            {
                                pedido.Orden += 1;
                                editPedido(pedido);
                            }
                        }
                        pedido1.Orden = pedido2.Orden + 1;
                        editPedido(pedido1);
                        editPedido(pedido2);
                    }
                    LimpiarPantalla();
                    Actualizar();
                    //_lstPedidos = getPedidosPorRepartoId(pedido1.RepartoId);
                    //ActualizarGrid(_lstPedidos);
                    Form1.loadingForm.CloseIt();
                }
                catch (Exception ex) { MessageBox.Show("Algo falló: " + ex.Message, "No se pudo reposicionar"); }
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                List<Pedido> ldFiltrada = new List<Pedido>();
                foreach (Pedido pedido in _lstPedidos)
                {
                    if (pedido.Entregar == 1) ldFiltrada.Add(pedido);
                }
                ActualizarGrid(ldFiltrada);
            }
            else Actualizar();
        }


        // BORRAR DESTINO //
        private void ComboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lstRepartosFiltrada = _lstDiaRepartos.Find(x => x.Dia == comboBox10.SelectedItem.ToString()).Reparto.ToList();
            comboBox9.DataSource = _lstRepartosFiltrada;
            comboBox9.DisplayMember = "Nombre";
            comboBox9.ValueMember = "Nombre";
        }
        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                label32.Text = _lstPedidos.Find(x => x.Direccion.Contains(textBox5.Text)).Direccion;
            }
            catch { label32.Text = "No encontrado"; }
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            ReCargarHDR(comboBox10.Text, comboBox9.Text);
            Pedido pedido = getPedidoPorDireccion(label32.Text);
            if (pedido == null) return;
            deletePedido(pedido);
            Actualizar();
            LimpiarPantalla();
        }
        private void Button15_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void BorrarUnDestino_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox8.Visible = true;
        }
    }
}