using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DCliente;
using static linway_app.Services.Delegates.DDiaReparto;
using static linway_app.Services.Delegates.DPedido;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DReparto;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private Pedido _pedidoAEliminar;
        private List<Pedido> _lstPedidos;
        private List<DiaReparto> _lstDiaRepartos;
        public FormRepartos()
        {
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
            if (lstPedidos == null) return;
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
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        }
        private void ReCargarHDR(string elDia, string elReparto)
        {
            Actualizar();
            long repartoId = getRepartoPorDiaYNombre(elDia, elReparto).Id;
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
            if (dialogResult == DialogResult.Yes)
            {
                success = exportReparto(dia, nombreReparto);
            }
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
            var lstRepartos = getRepartosPorDia(comboBox1.SelectedItem.ToString());
            comboBox2.DataSource = lstRepartos.Count > 0 ? lstRepartos : null;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
        }
        private void UpdateGrid()
        {
            if (comboBox1.Text == "") return;
            Reparto reparto = getRepartoPorDiaYNombre(comboBox1.SelectedItem.ToString(), comboBox2.Text);
            if (reparto == null) return;
            if (checkBox1.Checked)
            {
                _lstPedidos = getPedidosPorRepartoId(reparto.Id).Where(x => x.Entregar == 1).ToList();
            }
            else
            {
                _lstPedidos = getPedidosPorRepartoId(reparto.Id).ToList();
            }
            VerDatos(reparto);
            ActualizarGrid(_lstPedidos);
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
            if (textBox1.Text == "") return;
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
            LimpiarPantalla();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }

        //__________________________AGREGAR DESTINO--_____________________
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.DataSource = getRepartosPorDia(comboBox4.SelectedItem.ToString());
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)  // Agregar destino a recorrido
        {
            if (textBox2.Text == "")
            {
                label8.Text = "No encontrado";
                return;
            }
            try { long.Parse(textBox2.Text); } catch { return; };
            Cliente cliente = getCliente(long.Parse(textBox2.Text));
            if (cliente != null) label8.Text = cliente.Direccion;
            else label8.Text = "No encontrado";
        }
        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                label8.Text = "No encontrado";
                return;
            }
            Cliente cliente = getClientePorDireccion(textBox6.Text);
            if (cliente != null) label8.Text = cliente.Direccion;
            else label8.Text = "No encontrado";
        }
        private void AgregarDestinoAReparto_btn1_Click(object sender, EventArgs e)
        {
            if (label8.Text == "No encontrado" || comboBox4.Text == "")
            {
                MessageBox.Show("Error, verificar los campos");
                return;
            }
            ReCargarHDR(comboBox4.Text, comboBox5.Text);                // día y reparto
            comboBox1.SelectedIndex = comboBox4.SelectedIndex;
            comboBox2.SelectedIndex = comboBox5.SelectedIndex;
            Cliente cliente = getClientePorDireccionExacta(label8.Text);
            Reparto reparto = getRepartoPorDiaYNombre(comboBox4.Text, comboBox5.Text);
            if (cliente == null || reparto == null)
            {
                MessageBox.Show("Falló algo, código 44");
                return;
            };
            if (_lstPedidos.Exists(x => x.Id == cliente.Id && x.RepartoId == reparto.Id))
            {
                MessageBox.Show("Ese cliente ya estaba en el reparto");
                return;
            };
            long pedidoId = addPedidoIfNotExistsAndReturnId(reparto.Id, cliente.Id);
            Pedido pedido = getPedido(pedidoId);
            if (pedido == null)
            {
                MessageBox.Show("Falló poner pedido para entregar");
            }
            else
            {
                pedido.Entregar = 1;
                editPedidos(new List<Pedido>() { pedido });
            }
            LimpiarPantalla();
            Actualizar();
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
            if (comboBox6.Text == "")
            {
                MessageBox.Show("Debe seleccionar un día");
                return;
            }
            Actualizar();
            try
            {
                List<Reparto> lstRepartos = _lstDiaRepartos.Find(x => x.Dia == comboBox6.Text).Reparto.ToList();
                cleanRepartos(lstRepartos);
                LimpiarPantalla();
                Actualizar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falló: ", ex.Message);
            }
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
            comboBox7.DataSource = getRepartosPorDia(comboBox8.SelectedItem.ToString());
            comboBox7.DisplayMember = "Nombre";
            comboBox7.ValueMember = "Nombre";
        }
        private void Button11_Click(object sender, EventArgs e)
        {
            if (comboBox8.Text == "")
            {
                MessageBox.Show("Debe seleccionar un día");
                return;
            }
            string dia = comboBox8.Text;
            string nombre = comboBox7.Text;
            Reparto reparto = getRepartoPorDiaYNombre(dia, nombre);
            comboBox1.SelectedIndex = comboBox8.SelectedIndex;
            comboBox2.SelectedIndex = comboBox7.SelectedIndex;
            cleanRepartos(new List<Reparto>() { reparto });
            VerDatos(reparto);
            LimpiarPantalla();
            Actualizar();
        }

        // 4. limpiar un pedido
        private void Destino_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //LimpiarPantalla();
            //ActualizarGrid(new List<Pedido>());
            groupBox9.Visible = true;
            label39.Text = "Día " + comboBox1.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void TextBox7_TextChanged(object sender, EventArgs e)
        {
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.ToLower().Contains(textBox7.Text.ToLower()));
            if (pedido != null) label36.Text = pedido.Direccion;
            else label36.Text = "No encontrado";
        }
        private void Button18_Click(object sender, EventArgs e)
        {
            ReCargarHDR(comboBox1.Text, comboBox2.Text);
            string direccion = label36.Text;
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.Equals(direccion));
            if (pedido == null)
            {
                MessageBox.Show("Verifique que los datos sean correctos");
                return;
            }
            var prodVendidosAEditar = new List<ProdVendido>();
            pedido.ProdVendidos.ToList().ForEach(prodVendido =>
            {
                prodVendido.PedidoId = null;
                prodVendidosAEditar.Add(prodVendido);
            });
            editProdVendidos(prodVendidosAEditar);
            cleanPedidos(new List<Pedido>() { pedido });
            VerDatos(getReparto(pedido.RepartoId));
            LimpiarPantalla();
            Actualizar();
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
            try
            {
                var auxList = _lstPedidos.Where(x => x.Entregar == 1).ToList();
                var aux = auxList.Find(x => x.Direccion.ToLower().Contains(textBox3.Text.ToLower()));
                if (aux == null) label30.Text = "No encontrado";
                else label30.Text = aux.Direccion;
            }
            catch (Exception)
            {
                label30.Text = "No encontrado";
            }
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
            if (label30.Text == "No encontrado" || label30.Text == "" || label31.Text == "No encontrado" || label31.Text == "")
                return;
            try
            {
                string pedidoAMover = label30.Text;
                string pedidoReferencia = label31.Text;
                Reparto reparto = getRepartoPorDiaYNombre(comboBox1.Text, comboBox2.Text);
                Pedido pedido1 = reparto.Pedidos.ToList().Find(x => x.Direccion == pedidoAMover && x.Estado != "Eliminado");
                Pedido pedido2 = reparto.Pedidos.ToList().Find(x => x.Direccion == pedidoReferencia && x.Estado != "Eliminado");
                long order1 = pedido1.Orden;
                long order2 = pedido2.Orden;
                if (order2 == order1)
                {
                    return;
                }
                var pedidosAEditar = new List<Pedido>();
                if (order2 > order1)
                {
                    foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                    {
                        if (pedido.Orden > order1 && pedido.Orden < order2)
                        {
                            pedido.Orden -= 1;
                            pedidosAEditar.Add(pedido);
                        }
                    }
                    pedido1.Orden = pedido2.Orden;
                    pedido2.Orden -= 1;
                    pedidosAEditar.Add(pedido1);
                    pedidosAEditar.Add(pedido2);
                }
                else
                {
                    foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                    {
                        if (pedido.Orden < order1 && pedido.Orden > order2)
                        {
                            pedido.Orden += 1;
                            pedidosAEditar.Add(pedido);
                        }
                    }
                    pedido1.Orden = pedido2.Orden + 1;
                    pedidosAEditar.Add(pedido1);
                    pedidosAEditar.Add(pedido2);
                }
                editPedidos(pedidosAEditar);
                LimpiarPantalla();
                Actualizar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Algo falló: " + ex.Message, "No se pudo reposicionar");
            }
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                Actualizar();
                return;
            }
            var ldFiltrada = new List<Pedido>();
            foreach (Pedido pedido in _lstPedidos)
            {
                if (pedido.Entregar == 1) ldFiltrada.Add(pedido);
            }
            ActualizarGrid(ldFiltrada);
        }


        // BORRAR PEDIDO //
        private void ComboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox9.DataSource = getRepartosPorDia(comboBox10.SelectedItem.ToString());
                comboBox9.DisplayMember = "Nombre";
                comboBox9.ValueMember = "Nombre";
            }
            catch (Exception) { }
            label32.Text = "";
            textBox5.Text = "";
        }
        private void ComboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var reparto = (Reparto)comboBox9.SelectedItem;
                reparto = getReparto(reparto.Id);    // este paso extra evita que traiga pedidos eliminados
                if (reparto == null || reparto.Pedidos == null || reparto.Pedidos.Count == 0) return;
                _lstPedidos = reparto.Pedidos.ToList();
            }
            catch (Exception) { }
            label32.Text = "";
            textBox5.Text = "";
        }
        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            if (_lstPedidos == null || _lstPedidos.Count == 0)
            {
                label32.Text = "Reparto vacío";
                _pedidoAEliminar = null;
                return;
            }
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.Trim().ToLower().Contains(textBox5.Text.Trim().ToLower()));
            if (pedido == null)
            {
                label32.Text = "No encontrado";
                _pedidoAEliminar = null;
            }
            else
            {
                label32.Text = pedido.Direccion;
                _pedidoAEliminar = pedido;
            }
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            ReCargarHDR(comboBox10.Text, comboBox9.Text);
            if (_pedidoAEliminar == null) return;
            _pedidoAEliminar = getPedido(_pedidoAEliminar.Id);
            if (_pedidoAEliminar == null) return;
            List<ProdVendido> prodVendidos = getProdVendidos();
            if (prodVendidos != null && prodVendidos.Count != 0)
            {
                _pedidoAEliminar.ProdVendidos = prodVendidos.Where(x => x.PedidoId == _pedidoAEliminar.Id).ToList();
                if (_pedidoAEliminar.ProdVendidos != null && _pedidoAEliminar.ProdVendidos.Count != 0)
                {
                    foreach (ProdVendido prodVendido in _pedidoAEliminar.ProdVendidos)
                    {
                        prodVendido.PedidoId = null;
                    }
                    editProdVendidos(_pedidoAEliminar.ProdVendidos);
                }
            }
            deletePedido(_pedidoAEliminar);
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