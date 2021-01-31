using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace linway_app
{
    public partial class FormReparto : Form
    {
        const string direccionHDR = @"Base de datos\HojaDeReparto.bin";
        const string direccionClientes = @"Base de datos\ClientesLinway.bin";
        const string copiaDeSeguridad = "repartos.xlsx";
        List<Reparto> listaRepartos = new List<Reparto>();
        List<Reparto> listaRepartos2 = new List<Reparto>();
        List<Destino> listaDestinos = new List<Destino>();
        List<DiasReparto> diasDeReparto = new List<DiasReparto>();

        public FormReparto()
        {
            InitializeComponent();
        }

        private void formReparto_Load(object sender, EventArgs e)
        {
            CargarHDR();

            dataGridView1.Columns[0].Width = 25;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 30;
            //dataGridView1.Columns[3].Width = 400;
            dataGridView1.Columns[4].Width = 30;
            dataGridView1.Columns[5].Width = 30;
            dataGridView1.Columns[6].Width = 30;
            dataGridView1.Columns[7].Width = 30;
            dataGridView1.Columns[8].Width = 60;
        }

        void CargarHDR()
        {
            if (File.Exists(direccionHDR))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccionHDR);
                    BinaryFormatter traductor = new BinaryFormatter();
                    diasDeReparto = (List<DiasReparto>) traductor.Deserialize(archivo);
                    archivo.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer las hojas de reparto: " + e.Message);
                }
            }
            else
            {
                if (diasDeReparto.Count == 0)
                {
                    diasDeReparto.Add(new DiasReparto("Lunes"));
                    diasDeReparto.Add(new DiasReparto("Martes"));
                    diasDeReparto.Add(new DiasReparto("Miercoles"));
                    diasDeReparto.Add(new DiasReparto("Jueves"));
                    diasDeReparto.Add(new DiasReparto("Viernes"));
                }
            }
            dataGridView1.DataSource = listaDestinos.ToArray();
        }

        void GuardarHDR()
        {
            try
            {
                Stream archivoHDR = File.Create(direccionHDR);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivoHDR, diasDeReparto);
                archivoHDR.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar las hojas de reparto:" + e.Message);
            }
        }

        private void CrearCopiaDeSeguridad_Click(object sender, EventArgs e)
        {
            //checkBox1.Checked = true;
            CargarHDR();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará al actual Excel repartos.xlsx y demorará 15 segundos. ¿Confirmar?", "Exportar Repartos a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool success = new Exportar().ExportarAExcel(diasDeReparto);
                if (success)
                {
                    bCopiaSeguridad.ForeColor = Color.Green;
                    bCopiaSeguridad.Enabled = false;
                    bCopiaSeguridad.Text = "Creacion exitosa";
                }
                else
                {
                    MessageBox.Show("Hubo un error al guardar los cambios.");
                }
            }
        }

        private void ExportarButton_Click(object sender, EventArgs e)
        {
            CargarHDR();
            if (comboBox1.Text == "" || comboBox2.Text == "") return;
            DialogResult dialogResult = MessageBox.Show("Exportar " + comboBox1.Text + " - " + comboBox2.Text + " ¿Confirmar?", "Exportar Reparto a Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Reparto reparto = diasDeReparto.Find(x => x.Dia == comboBox1.Text).Reparto.Find(x => x.Nombre == comboBox2.Text);
                string litros = label22.Text;
                string bolsas = label21.Text;

                bool success = new Exportar().ExportarAExcel(reparto, comboBox1.Text, litros, bolsas);
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
            CargarHDR();
            DialogResult dialogResult = MessageBox.Show("Esta acción reemplazará definitivamente el listado actual de hojas de reparto por el contenido del Excel repartos.xlsx en la carpeta Copias de seguridad. ¿Confirmar?", "Importar Repartos desde Excel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                diasDeReparto.Clear();
                diasDeReparto = new Importar(copiaDeSeguridad).ImportarHojaDeRepartos();
                if (diasDeReparto != null)
                {
                    GuardarHDR();
                }
                else
                {
                    MessageBox.Show("Falló Repartos; cancelado");
                }
                CargarHDR();
                MessageBox.Show("Terminado");
            }
        }

        void ActualizarHDR(string elDia, string elReparto)
        {
            CargarHDR();
            diasDeReparto.Find(x => x.Dia == elDia).Reparto.Find(x => x.Nombre == elReparto).Destinos = listaDestinos;
        }

        void ReCargarHDR(string elDia, string elReparto)
        {
            CargarHDR();
            listaRepartos = diasDeReparto.Find(x => x.Dia == elDia).Reparto;
            listaDestinos = listaRepartos.Find(x => x.Nombre == elReparto).Destinos;
        }

        private void formReparto_FormClosing(object sender, FormClosingEventArgs e)
        {
            CargarHDR();
            GuardarHDR();
        }

        void LimpiarPantalla()
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
            label14.Text = reparto.TA.ToString();
            label15.Text = reparto.TE.ToString();
            label16.Text = reparto.TD.ToString();
            label17.Text = reparto.TT.ToString();
            label18.Text = reparto.TAE.ToString();
            label21.Text = reparto.TotalB.ToString();
            label22.Text = reparto.TL.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            label2.Visible = true;
            listaRepartos = diasDeReparto.Find(x => x.Dia == comboBox1.SelectedItem.ToString()).Reparto;
            comboBox2.DataSource = listaRepartos;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                try
                {
                    listaDestinos = listaRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)).Destinos;
                    VerDatos(listaRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)));
                }
                catch
                { }
            }
            dataGridView1.DataSource = listaDestinos.ToArray();
        }

        // MENUES
        private void agregarRepartoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            gpNuevoReparto.Visible = true;
            comboBox3.DataSource = diasDeReparto;
            comboBox3.DisplayMember = "Dia";
            comboBox3.ValueMember = "Dia";
        }

        private void agregarDestinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox2.Visible = true;
        }

        //_______________AGREGAR Reparto A DIA______________________________-
        private void button2_Click(object sender, EventArgs e)
        {
            CargarHDR();
            if ((textBox1.Text != ""))
            {
                Reparto nReparto = new Reparto(textBox1.Text);
                diasDeReparto.Find(x => x.Dia.Contains(comboBox3.Text)).AgregarReparto(nReparto);
            }
            GuardarHDR();
            LimpiarPantalla();
            CargarHDR();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }

        //__________________________AGREGAR DESTINO--_____________________
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listaRepartos2 = diasDeReparto.Find(x => x.Dia == comboBox4.SelectedItem.ToString()).Reparto;
            comboBox5.DataSource = listaRepartos2;
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
            List<Cliente> listaClientes = new List<Cliente>();

            if (File.Exists(direccionClientes))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccionClientes);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaClientes = (List<Cliente>) traductor.Deserialize(archivo);
                    archivo.Close();
                }
                catch (Exception f)
                {
                    MessageBox.Show("Error al leer archivo de clientes: " + f.Message);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el archivo Clientes en la carpeta");
            }

            if (textBox2.Text != "")
            {
                if (listaClientes.Exists(x => x.Numero == int.Parse(textBox2.Text)))
                {
                    label8.Text = listaClientes.Find(x => x.Numero == int.Parse(textBox2.Text)).Direccion;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if ((label8.Text != "No encontrado") && (comboBox4.Text != ""))
            {
                ReCargarHDR(comboBox4.Text, comboBox5.Text);
                comboBox1.SelectedIndex = comboBox4.SelectedIndex;
                comboBox2.SelectedIndex = comboBox5.SelectedIndex;
                if (label8.Text.Contains("–")) label8.Text = label8.Text.Replace("–", "-");
                Destino nDestino = new Destino(label8.Text.Substring(0, label8.Text.IndexOf('-')));
                listaRepartos.Find(x => x.Nombre.Contains(comboBox5.Text)).AgregarDestino(nDestino);
                GuardarHDR();
                LimpiarPantalla();
                CargarHDR();
            }
            else
            {
                MessageBox.Show("Error, verifique los campos");
            }
        }

        ///DATOS PARA FORMULARIO 1
        public List<Reparto> DarRepartos(string dia)
        {
            CargarHDR();
            return diasDeReparto.Find(x => x.Dia.Contains(dia)).Reparto;
        }

        public void CargarAHojaDeReparto(string direc, string dia, string reparto, List<Venta> lVentas)
        {
            CargarHDR();

            listaRepartos = diasDeReparto.Find(x => x.Dia == dia).Reparto;
            try
            {
                listaRepartos.Find(x => x.Nombre == reparto).CargarPorVenta(lVentas, direc.Substring(0, direc.IndexOf('-')));
                GuardarHDR();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        public void CargarAHojaDeReparto2(string direc, string dia, string reparto, List<ProdVendido> lVentas)
        {
            CargarHDR();
            listaRepartos = diasDeReparto.Find(x => x.Dia == dia).Reparto;
            Reparto elreparto = listaRepartos.Find(x => x.Nombre == reparto);
            if (elreparto == null)
            {
                MessageBox.Show("Falló la lectura de ese reparto");
                return;
            }
            elreparto.CargarPorNota(lVentas, direc.Substring(0, direc.IndexOf('-')));
            GuardarHDR();
        }

        //_LImpiar DATOS_ 
        //todos los dias
        private void todasLuAViToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
            groupBox4.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CargarHDR();
            foreach (DiasReparto dActual in diasDeReparto)
            {
                foreach (Reparto rActual in dActual.Reparto)
                {
                    rActual.LimpiarDatos();
                }
            }
            GuardarHDR();
            LimpiarPantalla();
            CargarHDR();
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
                CargarHDR();
                foreach (Reparto rActual in diasDeReparto.Find(x => x.Dia == comboBox6.Text).Reparto)
                {
                    rActual.LimpiarDatos();
                }
                GuardarHDR();
                LimpiarPantalla();
                CargarHDR();
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
            listaRepartos2 = diasDeReparto.Find(x => x.Dia == comboBox8.SelectedItem.ToString()).Reparto;
            comboBox7.DataSource = listaRepartos2;
            comboBox7.DisplayMember = "Nombre";
            comboBox7.ValueMember = "Nombre";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (comboBox8.Text != "")
            {
                CargarHDR();
                comboBox1.SelectedIndex = comboBox8.SelectedIndex;
                comboBox2.SelectedIndex = comboBox7.SelectedIndex;
                diasDeReparto.Find(x => x.Dia == comboBox8.Text).Reparto.Find(x => x.Nombre == comboBox7.Text).LimpiarDatos();
                VerDatos(diasDeReparto.Find(x => x.Dia == comboBox8.Text).Reparto.Find(x => x.Nombre == comboBox7.Text));
                dataGridView1.DataSource = listaDestinos.ToArray();
                GuardarHDR();
                LimpiarPantalla();
                CargarHDR();
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
            if (listaDestinos.Exists(x => x.Direccion.Contains(textBox3.Text)))
            {
                label30.Text = listaDestinos.Find(x => x.Direccion.Contains(textBox3.Text)).Direccion;
            }
            else
            {
                label30.Text = "No encontrado";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (listaDestinos.Exists(x => x.Direccion.Contains(textBox4.Text)))
            {
                label31.Text = listaDestinos.Find(x => x.Direccion.Contains(textBox4.Text)).Direccion;
            }
            else
            {
                label31.Text = "No encontrado";
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if ((label30.Text != "No encontrado") && (label31.Text != "No encontrado") && (label30.Text != "") && (label31.Text != ""))
            {
                if (label30.Text != label31.Text)
                {
                    ReCargarHDR(comboBox1.Text, comboBox2.Text);
                    bool despues;
                    int aMover = listaDestinos.IndexOf(listaDestinos.Find(x => x.Direccion == label30.Text));
                    int aDejarAtras = listaDestinos.IndexOf(listaDestinos.Find(x => x.Direccion == label31.Text));

                    if (aMover > aDejarAtras)
                    {
                        despues = true;
                    }
                    else
                    {
                        despues = false;
                    }
                    
                    listaDestinos.Insert(listaDestinos.IndexOf(listaDestinos.Find(x => x.Direccion == label31.Text)) + 1, listaDestinos.Find(x => x.Direccion == label30.Text));
                    
                    if (despues)
                    {
                        listaDestinos.RemoveAt(aMover + 1);
                    }
                    else
                    {
                        listaDestinos.RemoveAt(aMover);
                    }

                    GuardarHDR();
                    LimpiarPantalla();
                    CargarHDR();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                List<Destino> ldFiltrada = new List<Destino>();
                foreach (Destino dActual in listaDestinos)
                {
                    if (dActual.Entregar)
                    {
                        ldFiltrada.Add(dActual);
                    }
                }
                dataGridView1.DataSource = ldFiltrada.ToArray();
            }
            else
            {
                dataGridView1.DataSource = listaDestinos.ToArray();

            }
        }


        // BORRAR DESTINO //
        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            listaRepartos2 = diasDeReparto.Find(x => x.Dia == comboBox10.SelectedItem.ToString()).Reparto;
            comboBox9.DataSource = listaRepartos2;
            comboBox9.DisplayMember = "Nombre";
            comboBox9.ValueMember = "Nombre";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (listaDestinos.Exists(x => x.Direccion.Contains(textBox5.Text)))
            {
                label32.Text = listaDestinos.Find(x => x.Direccion.Contains(textBox5.Text)).Direccion;
            }
            else
            {
                label32.Text = "No encontrado";
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            ReCargarHDR(comboBox10.Text, comboBox9.Text);
            listaDestinos.Remove(listaDestinos.Find(x => x.Direccion == label32.Text));
            dataGridView1.DataSource = listaDestinos.ToArray();
            GuardarHDR();
            CargarHDR();
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
            if (listaDestinos.Exists(x => x.Direccion.Contains(textBox7.Text)))
            {
                label36.Text = listaDestinos.Find(x => x.Direccion.Contains(textBox7.Text)).Direccion;
            }
            else
            {
                label36.Text = "No encontrado";
            }
        }

        void RestarContadores()
        {
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TL -= listaDestinos.Find(x => x.Direccion == label36.Text).L;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TA -= listaDestinos.Find(x => x.Direccion == label36.Text).A;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TotalB -= listaDestinos.Find(x => x.Direccion == label36.Text).A;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TE -= listaDestinos.Find(x => x.Direccion == label36.Text).E;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TotalB -= listaDestinos.Find(x => x.Direccion == label36.Text).E;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TT -= listaDestinos.Find(x => x.Direccion == label36.Text).T;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TotalB -= listaDestinos.Find(x => x.Direccion == label36.Text).T;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TAE -= listaDestinos.Find(x => x.Direccion == label36.Text).AE;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TotalB -= listaDestinos.Find(x => x.Direccion == label36.Text).AE;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TD -= listaDestinos.Find(x => x.Direccion == label36.Text).D;
            listaRepartos.Find(x => x.Nombre == comboBox2.Text).TotalB -= listaDestinos.Find(x => x.Direccion == label36.Text).D;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (listaDestinos.Exists(x => x.Direccion == label36.Text))
            {
                ReCargarHDR(comboBox1.Text, comboBox2.Text);
                RestarContadores();
                VerDatos(listaRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)));
                listaDestinos.Find(x => x.Direccion == label36.Text).Limpiar();
                GuardarHDR();
                LimpiarPantalla();
                CargarHDR();
            }
            else
            {
                MessageBox.Show("Verifique que los datos sean correctos");
            }
        }
    }
}