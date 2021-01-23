using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace linway_app
{
    public partial class FormRecibos : Form
    {
        const string direccionRecibos = "Recibos.bin";
        const string copiaSeguridad = @"Copias de seguridad\Recibos.bin";
        List<DetalleRecibo> listaDetalle = new List<DetalleRecibo>();
        List<Recibo> listaRecibos = new List<Recibo>();
        List<Cliente> listaClientes = new List<Cliente>();
        int primerRecibo = 0;
        int ultimoRecibo;

        public FormRecibos()
        {
            InitializeComponent();
        }

        void GuardarRecibos()
        {
            try
            {
                Stream archivoRecibos = File.Create(direccionRecibos);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivoRecibos, listaRecibos);
                archivoRecibos.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar los recibos:" + e.Message);
            }
        }

        void AbrirRecibos()
        {
            if (File.Exists(direccionRecibos))
            {
                try
                {
                    Stream archivo = File.OpenRead(direccionRecibos);
                    BinaryFormatter traductor = new BinaryFormatter();
                    listaRecibos = (List<Recibo>) traductor.Deserialize(archivo);
                    archivo.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al leer los recibos" + e.Message);
                }
            }
            foreach (Recibo rActual in listaRecibos)
            {
                if (primerRecibo == 0)
                {
                    primerRecibo = rActual.Codigo;
                }
                ultimoRecibo = rActual.Codigo;
            }
            lCantRecibos.Text = listaRecibos.Count.ToString() + " recibos.";
        }

        public void CargarClientes(List<Cliente> clientes)
        {
            this.listaClientes.AddRange(clientes);
        }

        private void formRecibos_Load(object sender, EventArgs e)
        {
            AbrirRecibos();
            dataGridView1.DataSource = listaRecibos.ToArray();
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[3].Width = 60;
            dataGridView1.Columns[4].Width = 30;
            dataGridView2.DataSource = listaDetalle.ToArray();
            dataGridView2.Columns[1].Width = 50;
        }

        private void formRecibos_FormClosing(object sender, FormClosingEventArgs e)
        {
            AbrirRecibos();
            GuardarRecibos();
        }

        // ____________ filtrar datos________________
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Recibo> lFiltrada = new List<Recibo>();
            //todas - hoy - impresas- no impresas

            if (comboBox1.SelectedItem.ToString() == "Hoy")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo rActual in listaRecibos)
                {
                    if (rActual.Fecha == DateTime.Today.ToString().Substring(0, 10))
                    {
                        lFiltrada.Add(rActual);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                dataGridView1.DataSource = listaRecibos.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "Impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo rActual in listaRecibos)
                {
                    if (rActual.Impresa)
                    {
                        lFiltrada.Add(rActual);
                    }
                }
                dataGridView1.DataSource = lFiltrada.ToArray();
            }

            if (comboBox1.SelectedItem.ToString() == "No impresas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (Recibo rActual in listaRecibos)
                {
                    if (!rActual.Impresa)
                    {
                        lFiltrada.Add(rActual);
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
            List<Recibo> ListaFiltrada = new List<Recibo>();

            foreach (Recibo rActual in listaRecibos)
            {
                if (x == 'c')
                {
                    if (rActual.Cliente.Contains(texto))
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

        //_____________grupo imprimir_______________
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                foreach (Recibo rActual in ObtenerListaAImprimir())
                {
                    FormImprimirRecibo vistaPrevia = new FormImprimirRecibo();
                    vistaPrevia.Rellenar_Datos(rActual);
                    vistaPrevia.Show();
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

        List<Recibo> ObtenerListaAImprimir()
        {
            List<Recibo> listaAImprimir = new List<Recibo>();
            if (comboBox2.SelectedItem.ToString() == "No impresas")
            {
                foreach (Recibo rActual in listaRecibos)
                {
                    if (!rActual.Impresa)
                    {
                        listaAImprimir.Add(rActual);
                    }
                }
            }
            if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (Recibo rActual in listaRecibos)
                {
                    if (rActual.Fecha == DateTime.Today.ToString().Substring(0, 10))
                    {
                        listaAImprimir.Add(rActual);
                    }
                }
            }
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                if (comboBox2.SelectedItem.ToString() == "Establecer rango")
                {
                    if ((int.Parse(textBox2.Text) <= int.Parse(textBox3.Text)) && (int.Parse(textBox3.Text) <= ultimoRecibo) && (int.Parse(textBox2.Text) >= primerRecibo))
                    {
                        int j = int.Parse(textBox3.Text);
                        for (int i = int.Parse(textBox2.Text); i <= j; i++)
                        {
                            listaAImprimir.Add(listaRecibos.Find(x => x.Codigo == i));
                        }
                    }

                    else
                    {
                        MessageBox.Show("Rango establecido incorrecto");
                    }
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

        //BORRAR RECIBOS
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox3.SelectedItem.ToString() == "Impresas") || ((comboBox3.SelectedItem.ToString() == "Todas") || (comboBox3.SelectedItem.ToString() == "(Seleccionar)")))
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

        List<Recibo> ObtenerListaABorrar()
        {
            List<Recibo> listaABorrar = new List<Recibo>();

            if (textBox5.Text != "" && textBox4.Text != "")
            {
                if (comboBox3.SelectedItem.ToString() == "Establecer rango")
                {
                    if ((int.Parse(textBox5.Text) <= int.Parse(textBox4.Text)) && (int.Parse(textBox4.Text) <= ultimoRecibo) && (int.Parse(textBox5.Text) >= primerRecibo))
                    {
                        int j = int.Parse(textBox4.Text);
                        for (int i = int.Parse(textBox5.Text); i <= j; i++)
                        {
                            listaABorrar.Add(listaRecibos.Find(x => x.Codigo == i));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Rango establecido incorrecto");
                    }
                }
            }

            if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                listaABorrar.AddRange(listaRecibos);
            }

            if (comboBox3.SelectedItem.ToString() == "Impresas")
            {
                foreach (Recibo rActual in listaRecibos)
                {
                    if (rActual.Impresa)
                    {
                        listaABorrar.Add(rActual);
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
            if ((comboBox3.SelectedItem.ToString() == "Todas") || (comboBox3.SelectedItem.ToString() == "Establecer rango") || (comboBox3.SelectedItem.ToString() == "Impresas"))
            {
                MessageBox.Show("Confirme si desea borrar los recibos seleccionados");
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
            AbrirRecibos();
            foreach (Recibo rActual in ObtenerListaABorrar())
            {
                listaRecibos.Remove(rActual);
            }
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            GuardarRecibos();
            lCantRecibos.Text = listaRecibos.Count.ToString() + " recibos.";
            dataGridView1.DataSource = listaRecibos.ToArray();
        }

        // NUEVO RECIBO
        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                if (listaClientes.Exists(x => x.Numero == int.Parse(textBox6.Text)))
                {
                    label15.Text = listaClientes.Find(x => x.Numero == int.Parse(textBox6.Text)).Direccion;
                    if (listaDetalle.Count != 0)
                    {
                        button6.Enabled = true;
                    }
                }
                else
                {
                    label15.Text = "No encontrado";
                    button6.Enabled = false;
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox7.Enabled = true;
            }
            else
            {
                textBox7.Enabled = false;
            }
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool IsDec = false;
            int nroDec = 0;

            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            for (int i = 0; i < textBox8.Text.Length; i++)
            {
                if (textBox8.Text[i] == ',')
                    IsDec = true;

                if (IsDec && nroDec++ >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 44)
                e.Handled = IsDec;
            else
                e.Handled = true;
        }

        void LimpiarCampos()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            textBox7.Text = "";
            textBox8.Text = "";
        }
        
        private void button8_Click(object sender, EventArgs e)
        {
            listaDetalle.Clear();
            LimpiarCampos();
            button6.Enabled = false;
            label18.Text = "0";
            dataGridView2.DataSource = listaDetalle.ToArray();
        }

        bool algunDetSeleccionado()
        {
            bool seleccionado = false;
            if (radioButton1.Checked)
            {
                seleccionado = true;
            }
            if (radioButton2.Checked)
            {
                seleccionado = true;
            }
            if (radioButton3.Checked)
            {
                seleccionado = true;
            }
            if (radioButton4.Checked)
            {
                seleccionado = true;
            }
            return seleccionado;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((textBox8.Text != "") && (algunDetSeleccionado()))
            {
                if (radioButton1.Checked)
                {
                    listaDetalle.Add(new DetalleRecibo("Saldo a Favor", float.Parse(textBox8.Text) * -1));
                }
                if (radioButton2.Checked)
                {
                    listaDetalle.Add(new DetalleRecibo("Desc. por devol.", float.Parse(textBox8.Text) * -1));
                }
                if (radioButton3.Checked)
                {
                    listaDetalle.Add(new DetalleRecibo("Saldo pendiente", float.Parse(textBox8.Text)));
                }
                if (radioButton4.Checked)
                {
                    listaDetalle.Add(new DetalleRecibo("Factura N° " + textBox7.Text, float.Parse(textBox8.Text)));
                }

                float subTot = 0;
                foreach (DetalleRecibo dActual in listaDetalle)
                {
                    subTot += dActual.importe;
                }
                label18.Text = subTot.ToString();
                LimpiarCampos();
                dataGridView2.DataSource = listaDetalle.ToArray();
                if ((label15.Text != "") && (label15.Text != "No encontrado"))
                {
                    button6.Enabled = true;
                }

            }
            else
            {
                MessageBox.Show("Complete correctamente los campos");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            listaDetalle.Clear();
            textBox6.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            dataGridView2.DataSource = listaDetalle.ToArray();
            label18.Text = "0";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AbrirRecibos();
            this.ultimoRecibo++;
            listaRecibos.Add(new Recibo(ultimoRecibo, label15.Text, listaDetalle));
            LimpiarCampos();
            listaDetalle.Clear();
            textBox6.Text = "";
            label15.Text = "";
            button6.Enabled = false;
            dataGridView2.DataSource = listaDetalle.ToArray();
            label18.Text = "0";
            GuardarRecibos();
            dataGridView1.DataSource = listaRecibos.ToArray();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AbrirRecibos();
            dataGridView1.DataSource = listaRecibos.ToArray();
        }

        //Exportar
        private void bExportar_Click(object sender, EventArgs e)
        {
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
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 3, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                }

                hoja_trabajo.Cells[2, 1] = "Número";
                hoja_trabajo.Cells[2, 2] = "Fecha";
                hoja_trabajo.Cells[2, 3] = "Direccion";
                hoja_trabajo.Cells[2, 4] = "Total";
                hoja_trabajo.Cells[2, 5] = "Impresa";

                //Establecer rango de celdas
                excelCellrange = hoja_trabajo.Range[hoja_trabajo.Cells[2, 1], hoja_trabajo.Cells[grd.Rows.Count + 2, grd.Columns.Count]];
                excelCellrange.Font.Bold = true;

                //Autoestablecer ancho de columnas
                excelCellrange.EntireColumn.AutoFit();
                //rellenar bordes
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;
                //guardar.
                try
                {
                    libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                    libros_trabajo.Close(true);
                    aplicacion.Quit();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error al exportar a Excel:" + e.Message);
                }
            }

        }

        //Copia de seguridad
        private void bCopiaSeguridad_Click(object sender, EventArgs e)
        {
            try
            {
                Stream archivoNotas = File.Create(copiaSeguridad);
                BinaryFormatter traductor = new BinaryFormatter();
                traductor.Serialize(archivoNotas, listaRecibos);
                archivoNotas.Close();
                bCopiaSeguridad.ForeColor = Color.Green;
                bCopiaSeguridad.Enabled = false;
                bCopiaSeguridad.Text = "Creacion exitosa";
            }
            catch (Exception f)
            {
                MessageBox.Show("Error al hacer copia de seguridad de los recibos:" + f.Message);
            }
        }
    }
}
