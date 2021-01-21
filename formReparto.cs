using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class formReparto : Form
    {
        const string direccionHDR = "HojaDeReparto.bin";
        const string direccionClientes = "ClientesLinway.bin";
        List<DiasReparto> diasDeReparto = new List<DiasReparto>();
        List<Reparto> listaRepartos = new List<Reparto>();
        List<Reparto> listaRepartos2 = new List<Reparto>();
        List<Destino> listaDestinos = new List<Destino>();

        public formReparto()
        {
            InitializeComponent();
        }

        void cargarHDR()
        {
            if (File.Exists(direccionHDR))
            {
                Stream archivoHDR = File.OpenRead(direccionHDR);
                BinaryFormatter traductor = new BinaryFormatter();
                diasDeReparto = (List<DiasReparto>)traductor.Deserialize(archivoHDR);
                archivoHDR.Close();
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
        void guardarHDR()
        {
            Stream archivoHDR = File.Create(direccionHDR);
            BinaryFormatter traductor = new BinaryFormatter();
            traductor.Serialize(archivoHDR, diasDeReparto);
            archivoHDR.Close();
        }
        void ActualizarHDR(string elDia, string elReparto)
        {
            cargarHDR();
            diasDeReparto.Find(x => x.Dia == elDia).Reparto.Find(x => x.Nombre == elReparto).Destinos = listaDestinos;
        }
        void reCargarHDR(string elDia, string elReparto)
        {
            cargarHDR();
            listaRepartos = diasDeReparto.Find(x => x.Dia == elDia).Reparto;
            listaDestinos = listaRepartos.Find(x => x.Nombre == elReparto).Destinos;
        }
        

        private void formReparto_Load(object sender, EventArgs e)
        {
            cargarHDR();
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 30;
            dataGridView1.Columns[3].Width = 400;
            dataGridView1.Columns[4].Width = 30;
            dataGridView1.Columns[5].Width = 30;
            dataGridView1.Columns[6].Width = 30;
            dataGridView1.Columns[7].Width = 30;
            dataGridView1.Columns[8].Width = 30;
            

            
        }
        private void formReparto_FormClosing(object sender, FormClosingEventArgs e)
        {
            cargarHDR();
            guardarHDR();
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

        private void verDatos(Reparto reparto)
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
                listaDestinos = listaRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)).Destinos;
                verDatos(listaRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)));
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
            cargarHDR();
            if ((textBox1.Text != ""))
            {
                Reparto nReparto = new Reparto(textBox1.Text);
                diasDeReparto.Find(x => x.Dia.Contains(comboBox3.Text)).agregarReparto(nReparto);
            }
            guardarHDR();
            LimpiarPantalla();
            cargarHDR();
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
        private void textBox2_Leave(object sender, EventArgs e)
        {
            List<Cliente> listaClientes = new List<Cliente>();
            if (File.Exists(direccionClientes))
            {
                Stream archivoClientes = File.OpenRead(direccionClientes);
                BinaryFormatter traductor = new BinaryFormatter();
                listaClientes = (List<Cliente>)traductor.Deserialize(archivoClientes);
                archivoClientes.Close();
            }
            if (textBox2.Text == "")
            {
                label8.Text = "no encontrado";
            }
            else
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
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if ((label8.Text != "No encontrado") && (comboBox4.Text != ""))
            {
                reCargarHDR(comboBox4.Text, comboBox5.Text);
                comboBox1.SelectedIndex = comboBox4.SelectedIndex;
                comboBox2.SelectedIndex = comboBox5.SelectedIndex;
                Destino nDestino = new Destino(label8.Text.Substring(0, label8.Text.IndexOf('-')));
                listaRepartos.Find(x => x.Nombre.Contains(comboBox5.Text)).agregarDestino(nDestino);
                guardarHDR();
                LimpiarPantalla();
                cargarHDR();
            }
            else
            {
                MessageBox.Show("Error, verifique los campos");
            }
        }




        ///DATOS PARA FORMULARIO 1
        ///
        public List<Reparto> darRepartos(string dia)
        {
            cargarHDR();
            return diasDeReparto.Find(x => x.Dia.Contains(dia)).Reparto;
        }
        public void cargarAHojaDeReparto(string direc, string dia, string reparto, List<Venta> lVentas)
        {
            cargarHDR();

            listaRepartos = diasDeReparto.Find(x => x.Dia == dia).Reparto;
            listaRepartos.Find(x => x.Nombre == reparto).cargarPorVenta(lVentas, direc.Substring(0, direc.IndexOf('-')));
            guardarHDR();
        }
        public void cargarAHojaDeReparto2(string direc, string dia, string reparto, List<ProdVendido> lVentas)
        {
            cargarHDR();
            listaRepartos = diasDeReparto.Find(x => x.Dia == dia).Reparto;
            listaRepartos.Find(x => x.Nombre == reparto).cargarPorNota(lVentas, direc.Substring(0, direc.IndexOf('-')));
            guardarHDR();
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
            cargarHDR();
            foreach (DiasReparto dActual in diasDeReparto)
            {
                foreach (Reparto rActual in dActual.Reparto)
                {
                    rActual.LimpiarDatos();
                }
            }
            guardarHDR();
            LimpiarPantalla();
            cargarHDR();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        //dia seleciionado
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
                cargarHDR();
                foreach (Reparto rActual in diasDeReparto.Find(x => x.Dia == comboBox6.Text).Reparto)
                {
                    rActual.LimpiarDatos();
                }
                guardarHDR();
                LimpiarPantalla();
                cargarHDR();
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
                cargarHDR();
                comboBox1.SelectedIndex = comboBox8.SelectedIndex;
                comboBox2.SelectedIndex = comboBox7.SelectedIndex;
                diasDeReparto.Find(x => x.Dia == comboBox8.Text).Reparto.Find(x => x.Nombre == comboBox7.Text).LimpiarDatos();
                verDatos(diasDeReparto.Find(x => x.Dia == comboBox8.Text).Reparto.Find(x => x.Nombre == comboBox7.Text));
                dataGridView1.DataSource = listaDestinos.ToArray();
                guardarHDR();
                LimpiarPantalla();
                cargarHDR();
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
                    reCargarHDR(comboBox1.Text, comboBox2.Text);
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
                    guardarHDR();
                    LimpiarPantalla();
                    cargarHDR();
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


        //Exportar a excel.
        private void button12_Click(object sender, EventArgs e)
        {
            //checkBox1.Checked = true;
            ExportarDataGridViewExcel(dataGridView1);
        }
        private void ExportarDataGridViewExcel(DataGridView grd)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                Microsoft.Office.Interop.Excel.Range excelCellrange;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();
                hoja_trabajo =
                    (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                //Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    for (int j = 1; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 3, j] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                }
                hoja_trabajo.Cells[1, 3] = "DIA "+ comboBox1.Text+ " -  RECORRIDO " + comboBox2.Text;
                hoja_trabajo.Cells[1, 3].Font.Bold = true;
                hoja_trabajo.Cells[1, 3].Font.Size = 11;
                hoja_trabajo.Cells[2, 1] = "Dirección";
                hoja_trabajo.Cells[2, 2] = "L";
                hoja_trabajo.Cells[2, 3] = "Productos";
                hoja_trabajo.Cells[2, 4] = "A";
                hoja_trabajo.Cells[2, 5] = "E";
                hoja_trabajo.Cells[2, 6] = "D";
                hoja_trabajo.Cells[2, 7] = "T";
                hoja_trabajo.Cells[2, 8] = "AE";

                
                //Establecer rango de celdas
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[2, 1], hoja_trabajo.Cells[grd.Rows.Count + 4, grd.Columns.Count-1]];
                excelCellrange.Font.Bold = true;

                hoja_trabajo.Cells[grd.Rows.Count + 3, 1] = "TOTALES:";
                hoja_trabajo.Cells[grd.Rows.Count + 3, 2] = label22.Text;
                hoja_trabajo.Cells[grd.Rows.Count + 4, 3] = " TOTAL PESO: " + ((listaRepartos.Find(x => x.Nombre == comboBox2.Text).TotalB * 20) + int.Parse(label22.Text)).ToString() + "                    Total bolsas: "+ label21.Text;
                hoja_trabajo.Cells[grd.Rows.Count + 3, 4] = label14.Text;
                hoja_trabajo.Cells[grd.Rows.Count + 3, 5] = label15.Text;
                hoja_trabajo.Cells[grd.Rows.Count + 3, 6] = label16.Text;
                hoja_trabajo.Cells[grd.Rows.Count + 3, 7] = label17.Text;
                hoja_trabajo.Cells[grd.Rows.Count + 3, 8] = label18.Text;
                /*
                hoja_trabajo.Cells[grd.Rows.Count + 3, 9] = label21.Text;
                hoja_trabajo.Cells[grd.Rows.Count + 3, 9].Font.Bold = true;
                hoja_trabajo.Cells[grd.Rows.Count + 3, 9].EntireColumn.AutoFit();
                 */
                hoja_trabajo.Cells[grd.Rows.Count + 4, 4] = "A";
                hoja_trabajo.Cells[grd.Rows.Count + 4, 5] = "E";
                hoja_trabajo.Cells[grd.Rows.Count + 4, 6] = "D";
                hoja_trabajo.Cells[grd.Rows.Count + 4, 7] = "T";
                hoja_trabajo.Cells[grd.Rows.Count + 4, 8] = "AE";



                //Autoestablecer ancho de columnas
                excelCellrange.EntireColumn.AutoFit();
                //rellenar bordes
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;
                //guardar.
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
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
            reCargarHDR(comboBox10.Text, comboBox9.Text);
            listaDestinos.Remove(listaDestinos.Find(x => x.Direccion == label32.Text));
            dataGridView1.DataSource = listaDestinos.ToArray();
            guardarHDR();
            cargarHDR();
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
        void restarContadores()
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
                reCargarHDR(comboBox1.Text, comboBox2.Text);
                restarContadores();
                verDatos(listaRepartos.Find(x => x.Nombre.Equals(comboBox2.Text)));
                listaDestinos.Find(x => x.Direccion == label36.Text).Limpiar();
                guardarHDR();
                LimpiarPantalla();
                cargarHDR();
            }
            else
            {
                MessageBox.Show("Verifique que los datos sean correctos");
            }
        }


       

        
        //BORRAR REPARTO //

    }
}