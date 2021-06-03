using linway_app.Excel;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormReparto : Form
    {
        private List<Reparto> _lstRepartos = new List<Reparto>();
        private List<Reparto> _lstRepartosFiltrada = new List<Reparto>();
        private List<Pedido> _lstPedidos = new List<Pedido>();
        private List<DiaReparto> _lstDiaRepartos = new List<DiaReparto>();
        private IServicioCliente _servCliente;
        private IServicioDiaReparto _servDiaReparto;
        private IServicioReparto _servReparto;
        private IServicioPedido _servPedido;

        public FormReparto(IServicioCliente servCliente, IServicioDiaReparto servDiaReparto,
            IServicioReparto servReparto, IServicioPedido servPedido)
        {
            InitializeComponent();
            _servCliente = servCliente;
            _servDiaReparto = servDiaReparto;
            _servReparto = servReparto;
            _servPedido = servPedido;
        }

        private void FormReparto_Load(object sender, EventArgs e)
        {
            Actualizar();
        }
        private void Actualizar()
        {
            CargarDiaRepartos();
            CargarRepartos();
            RenderizarRepartos();
        }
        private void CargarDiaRepartos()
        {
            _lstDiaRepartos = _servDiaReparto.GetAll();
            if (_lstDiaRepartos.Count == 0 || _lstDiaRepartos == null)
            {
                CrearDias();
            }
        }
        private void CrearDias()
        {
            AgregarDiaReparto(new DiaReparto { Dia = "Lunes" });
            AgregarDiaReparto(new DiaReparto { Dia = "Martes" });
            AgregarDiaReparto(new DiaReparto { Dia = "Miercoles" });
            AgregarDiaReparto(new DiaReparto { Dia = "Jueves" });
            AgregarDiaReparto(new DiaReparto { Dia = "Viernes" });
            AgregarDiaReparto(new DiaReparto { Dia = "Sabado" });
            _lstDiaRepartos = _servDiaReparto.GetAll();
        }
        private void CargarRepartos()
        {
            _lstRepartos = _servReparto.GetAll();
        }
        private void RenderizarRepartos()
        {
            dataGridView1.DataSource = _lstPedidos.ToArray();
            dataGridView1.Columns[0].Width = 25;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 30;
            dataGridView1.Columns[3].Width = 400;
            dataGridView1.Columns[4].Width = 30;
            dataGridView1.Columns[5].Width = 30;
            dataGridView1.Columns[6].Width = 30;
            dataGridView1.Columns[7].Width = 30;
            dataGridView1.Columns[8].Width = 60;
        }
        private void AgregarDiaReparto(DiaReparto diaReparto)
        {
            bool response = _servDiaReparto.Add(diaReparto);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Día de Reparto a la base de datos");
        }
        private void AgregarReparto(Reparto reparto)
        {
            bool response = _servReparto.Add(reparto);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Reparto a la base de datos");
        }
        private void AgregarPedido(Pedido pedido)
        {
            bool response = _servPedido.Add(pedido);
            if (!response) MessageBox.Show("Algo falló al agregar nuevo Pedido a la base de datos");
        }
        private List<Cliente> GetClientes()
        {
            return _servCliente.GetAll();
        }
        private void EditarPedido(Pedido pedido)
        {
            bool response = _servPedido.Edit(pedido);
            if (!response) MessageBox.Show("Algo falló al editar el Reparto en la base de datos");
        }
        private void EditarReparto(Reparto reparto)
        {
            bool response = _servReparto.Edit(reparto);
            if (!response) MessageBox.Show("Algo falló al editar el Reparto en la base de datos");
        }
        private void EliminarPedido(Pedido pedido)
        {
            bool response = _servPedido.Delete(pedido);
            if (!response) MessageBox.Show("Algo falló al eliminar el Pedido de la base de datos");
        }
        public void LimpiarReparto(Reparto reparto)
        {
            reparto.Ta = 0;
            reparto.Te = 0;
            reparto.Td = 0;
            reparto.Tt = 0;
            reparto.Tae = 0;
            reparto.TotalB = 0;
            reparto.Tl = 0;
            foreach (Pedido pedido in reparto.Pedidos)
            {
                pedido.Entregar = 0;
                pedido.L = 0;
                pedido.Productos = "";
                pedido.A = 0;
                pedido.E = 0;
                pedido.D = 0;
                pedido.T = 0;
                pedido.Ae = 0;
                EditarPedido(pedido);
            }
            EditarReparto(reparto);
        }



        private void CrearCopiaDeSeguridad_Click(object sender, EventArgs e)
        {
        }
        private void Exportar_Click(object sender, EventArgs e)
        {
            CargarDiaRepartos();
            string dia = comboBox1.Text;
            string nombreReparto = comboBox2.Text;
            if (dia == "" || nombreReparto == "") return;
            DialogResult dialogResult = MessageBox.Show("Exportar " + dia + " - "
                + nombreReparto + " ¿Confirmar?", "Exportar Reparto a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Reparto reparto = _lstDiaRepartos.Find(x => x.Dia == dia)
                    .Reparto.ToList().Find(x => x.Nombre == nombreReparto);
                string litros = label22.Text;
                string bolsas = label21.Text;
                bool success = new Exportar().ExportarAExcel(reparto, dia, litros, bolsas);
                if (success)
                {
                    MessageBox.Show("Terminado");
                }
                else
                {
                    MessageBox.Show("Hubo un error al guardar los cambios.");
                }
            }
        }

        private void ImportarButton_Click(object sender, EventArgs e)
        {
        }

        private void ActualizarHDR(string elDia, string elReparto)
        {
            CargarDiaRepartos();
            _lstDiaRepartos.Find(x => x.Dia == elDia).Reparto.ToList().Find(x => x.Nombre == elReparto).Pedidos = _lstPedidos;
        }

        private void ReCargarHDR(string elDia, string elReparto)
        {
            CargarDiaRepartos();
            _lstRepartos = _lstDiaRepartos.Find(x => x.Dia == elDia).Reparto.ToList();
            _lstPedidos = _lstRepartos.Find(x => x.Nombre == elReparto).Pedidos.ToList();
        }

        private void formReparto_FormClosing(object sender, FormClosingEventArgs e)
        {}

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
            textBox1.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            label8.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            comboBox8.Text = "";
            comboBox9.Text = "";
            comboBox10.Text = "";
            checkBox1.Checked = false;
            label32.Text = "";
            label36.Text = "";
        }


        //____________________Ejegir Hoja de Reparto______________________________

        private void VerDatos(Reparto reparto)
        {
            label14.Text = reparto.Ta.ToString();
            label15.Text = reparto.Te.ToString();
            label16.Text = reparto.Td.ToString();
            label17.Text = reparto.Tt.ToString();
            label18.Text = reparto.Tae.ToString();
            label21.Text = reparto.TotalB.ToString();
            label22.Text = reparto.Tl.ToString();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            label2.Visible = true;
            _lstRepartos = _lstDiaRepartos.Find(x => x.Dia == comboBox1.SelectedItem.ToString()).Reparto.ToList();
            comboBox2.DataSource = _lstRepartos;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
        }
        private void Ver_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                try
                {
                    _lstPedidos = _lstRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)).Pedidos.ToList();
                    VerDatos(_lstRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)));
                }
                catch
                { }
            }
            dataGridView1.DataSource = _lstPedidos.ToArray();
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
            CargarDiaRepartos();
            if (textBox1.Text != "")
            {
                DiaReparto diaRep = _lstDiaRepartos.Find(x => x.Dia.Contains(comboBox3.Text));
                Reparto nuevoReparto = new Reparto
                {
                    Nombre = textBox1.Text,
                    DiaRepartoId = diaRep.Id
                };
                AgregarReparto(nuevoReparto);
            }
            LimpiarPantalla();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }

        //__________________________AGREGAR DESTINO--_____________________
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lstRepartosFiltrada = _lstDiaRepartos.Find(x => x.Dia == comboBox4.SelectedItem.ToString()).Reparto.ToList();
            comboBox5.DataSource = _lstRepartosFiltrada;
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
        private void textBox2_Leave(object sender, EventArgs e)  // Agregar a destino a recorrido
        {
            List<Cliente> listaClientes = GetClientes();
            if (textBox2.Text != "")
            {
                long clientId = long.Parse(textBox2.Text);
                Cliente cliente = listaClientes.Find(x => x.Id == clientId);
                if (cliente != null)
                {
                    label8.Text = cliente.Direccion;
                }
                else
                {
                    label8.Text = "no encontrado";
                }
            }
            else
            {
                label8.Text = "no encontrado";
            }
        }
        private void AgregarDestinoAReparto_btn1_Click(object sender, EventArgs e)
        {
            if (label8.Text != "No encontrado" && comboBox4.Text != "")
            {
                ReCargarHDR(comboBox4.Text, comboBox5.Text);                // día y reparto
                comboBox1.SelectedIndex = comboBox4.SelectedIndex;
                comboBox2.SelectedIndex = comboBox5.SelectedIndex;
                Cliente cliente = GetClientes().Find(x => x.Direccion == label8.Text);
                Reparto reparto = _lstRepartos.Find(x => x.Nombre.Contains(comboBox5.Text));
                if (cliente == null || reparto == null) { MessageBox.Show("Falló algo, código 44"); return; };
                // este agrega Destino sin productos
                Pedido nuevoPedido = new Pedido
                {
                    ClienteId = cliente.Id,
                    Direccion = cliente.Direccion,
                    RepartoId = reparto.Id
                };
                // corroborar que no esté antes de agregar   <----------
                if (_lstPedidos.Exists(x => x.ClienteId == nuevoPedido.ClienteId && x.RepartoId == nuevoPedido.RepartoId))
                {
                    MessageBox.Show("Ese cliente ya estaba en el reparto");
                    return;
                };
                AgregarPedido(nuevoPedido);
                LimpiarPantalla();
            }
            else
            {
                MessageBox.Show("Error, verifique los campos");
            }
        }

        ///DATOS PARA FORMULARIO 1
        public List<Reparto> DarRepartos(string dia)
        {
            CargarDiaRepartos();
            return _lstDiaRepartos.Find(x => x.Dia.Contains(dia)).Reparto.ToList();
        }


        //_LImpiar DATOS_ 
        //todos los dias
        private void todasLuAViToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox4.Visible = true;
        }
        private void LimpiarRepartos_Click(object sender, EventArgs e)
        {
            foreach (DiaReparto diaReparto in _lstDiaRepartos)
            {
                foreach (Reparto reparto in diaReparto.Reparto)
                {
                    LimpiarReparto(reparto);
                    EditarReparto(reparto);
                }
            }
            LimpiarPantalla();
            Actualizar();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }

        //dia seleccionado
        private void diaEspecíficoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox5.Visible = true;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox6.Text != "")
            {
                CargarDiaRepartos();
                string dia = comboBox6.Text;
                foreach (Reparto reparto in _lstDiaRepartos.Find(x => x.Dia == dia).Reparto)
                {
                    LimpiarReparto(reparto);
                }
                LimpiarPantalla();
                CargarDiaRepartos();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un día");
            }
        }

        //reparto seleccionado
        private void repartoSeleccionadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox6.Visible = true;
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dia = comboBox8.SelectedItem.ToString();
            _lstRepartosFiltrada = _lstDiaRepartos.Find(x => x.Dia == dia).Reparto.ToList();
            comboBox7.DataSource = _lstRepartosFiltrada;
            comboBox7.DisplayMember = "Nombre";
            comboBox7.ValueMember = "Nombre";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (comboBox8.Text != "")
            {
                string dia = comboBox8.Text;
                string nombre = comboBox7.Text;
                Reparto reparto = _lstDiaRepartos.Find(x => x.Dia == dia).Reparto.ToList().Find(x => x.Nombre == nombre);
                comboBox1.SelectedIndex = comboBox8.SelectedIndex;
                comboBox2.SelectedIndex = comboBox7.SelectedIndex;
                LimpiarReparto(reparto);
                VerDatos(reparto);
                LimpiarPantalla();
                Actualizar();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un día");
            }
        }

        //__________REPOSICIONAR DESTINO____________________
        private void posicionarDestinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
            label27.Text = "Día " + comboBox1.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (_lstPedidos.Exists(x => x.Direccion.Contains(textBox3.Text)))
            {
                label30.Text = _lstPedidos.Find(x => x.Direccion.Contains(textBox3.Text)).Direccion;
            }
            else
            {
                label30.Text = "No encontrado";
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (_lstPedidos.Exists(x => x.Direccion.Contains(textBox4.Text)))
            {
                label31.Text = _lstPedidos.Find(x => x.Direccion.Contains(textBox4.Text)).Direccion;
            }
            else
            {
                label31.Text = "No encontrado";
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (
                label30.Text != "No encontrado"
                && label31.Text != "No encontrado"
                && label30.Text != ""
                && label31.Text != ""
            )
            {
                if (label30.Text != label31.Text)
                {
                    ReCargarHDR(comboBox1.Text, comboBox2.Text);
                    bool despues;
                    int aMover = _lstPedidos.IndexOf(_lstPedidos.Find(x => x.Direccion == label30.Text));
                    int aDejarAtras = _lstPedidos.IndexOf(_lstPedidos.Find(x => x.Direccion == label31.Text));

                    if (aMover > aDejarAtras)
                    {
                        despues = true;
                    }
                    else
                    {
                        despues = false;
                    }
                    
                    _lstPedidos.Insert(aDejarAtras + 1, _lstPedidos.Find(x => x.Direccion == label30.Text));
                    
                    if (despues)
                    {
                        _lstPedidos.RemoveAt(aMover + 1);
                    }
                    else
                    {
                        _lstPedidos.RemoveAt(aMover);
                    }

                    
                    LimpiarPantalla();
                    CargarDiaRepartos();
                }
                else
                {
                    MessageBox.Show("No pueden ser las mismas direcciones");
                }
            }
            else
            {
                MessageBox.Show("Elija ambas direcciones");
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                List<Pedido> ldFiltrada = new List<Pedido>();
                foreach (Pedido pedido in _lstPedidos)
                {
                    if (pedido.Entregar == 1)
                    {
                        ldFiltrada.Add(pedido);
                    }
                }
                dataGridView1.DataSource = ldFiltrada.ToArray();
            }
            else
            {
                dataGridView1.DataSource = _lstPedidos.ToArray();
            }
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
            if (_lstPedidos.Exists(x => x.Direccion.Contains(textBox5.Text)))
            {
                label32.Text = _lstPedidos.Find(x => x.Direccion.Contains(textBox5.Text)).Direccion;
            }
            else
            {
                label32.Text = "No encontrado";
            }
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            ReCargarHDR(comboBox10.Text, comboBox9.Text);
            Pedido pedido = _lstPedidos.Find(x => x.Direccion == label32.Text);
            EliminarPedido(pedido);
            Actualizar();
            LimpiarPantalla();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private void borrarUnDestinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox8.Visible = true;
        }
        //limpiar un destino
        private void destinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox9.Visible = true;
            label39.Text = "Día " + comboBox1.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (_lstPedidos.Exists(x => x.Direccion.Contains(textBox7.Text)))
            {
                label36.Text = _lstPedidos.Find(x => x.Direccion.Contains(textBox7.Text)).Direccion;
            }
            else
            {
                label36.Text = "No encontrado";
            }
        }
        private void button18_Click(object sender, EventArgs e)
        {
            if (_lstPedidos.Exists(x => x.Direccion == label36.Text))
            {
                string direcCliente = label36.Text;
                ReCargarHDR(comboBox1.Text, comboBox2.Text);
                string nombreRep = comboBox2.Text;
                string direccion = label36.Text;
                Reparto reparto = _lstRepartos.Find(x => x.Nombre == nombreRep);
                Pedido pedido = _lstPedidos.Find(x => x.Direccion == direccion);

                reparto.Tl -= pedido.L;
                reparto.Ta -= pedido.A;
                reparto.TotalB -= pedido.A;
                reparto.Te -= pedido.E;
                reparto.TotalB -= pedido.E;
                reparto.Tt -= pedido.T;
                reparto.TotalB -= pedido.T;
                reparto.Tae -= pedido.Ae;
                reparto.TotalB -= pedido.Ae;
                reparto.Td -= pedido.D;
                reparto.TotalB -= pedido.D;

                VerDatos(reparto);
                EditarReparto(reparto);
                // EliminarPedido(pedido);   ??
                LimpiarReparto(reparto);
                LimpiarPantalla();
                Actualizar();
            }
            else
            {
                MessageBox.Show("Verifique que los datos sean correctos");
            }
        }
    }
}