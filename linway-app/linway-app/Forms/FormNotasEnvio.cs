﻿using linway_app.Excel;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static linway_app.Services.Delegates.DNotaDeEnvio;
using static linway_app.Services.Delegates.DPedido;
using static linway_app.Services.Delegates.DProducto;
using static linway_app.Services.Delegates.DProdVendido;
using static linway_app.Services.Delegates.DReparto;
using static linway_app.Services.Delegates.DVentas;

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
        private void FormNotas_Load(object sender, EventArgs ev)
        {
            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
        }
        private void ActualizarNotas()
        {
            _lstNotaDeEnvios = getNotaDeEnvios();
            if (_lstNotaDeEnvios == null) return;
            lCantNotas.Text = _lstNotaDeEnvios.Count.ToString() + " notas de envio.";
        }
        private void ActualizarGrid1(ICollection<NotaDeEnvio> lstNotadeEnvios)
        {
            if (lstNotadeEnvios == null) return;
            var grid = new List<ENotaDeEnvio>();
            foreach (NotaDeEnvio nota in lstNotadeEnvios)
            {
                grid.Add(Form1.mapper.Map<ENotaDeEnvio>(nota));
            }
            dataGridView1.DataSource = grid;
            //foreach (var item in dataGridView1.Rows)   muy lento
            //{
            //    ((DataGridViewRow)item).Height = 35;
            //}
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.Columns[2].Width = 170;
            dataGridView1.Columns[3].Width = 320;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;        // lento
            comboBox1.SelectedItem = "Todas ??";
            comboBox3.SelectedItem = "(Seleccionar)";
        }
        private void ActualizarGrid2(ICollection<ProdVendido> lstProdVendidos)
        {
            if (lstProdVendidos == null) return;
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                grid.Add(Form1.mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView2.DataSource = grid;
            dataGridView2.Columns[0].Width = 25;
            dataGridView2.Columns[1].Width = 200;
        }


        //_________________________FILTRAR DATOS_________________________________
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var lFiltrada = new List<NotaDeEnvio>();
            //todas - hoy - impresas- no impresas
            if (comboBox1.SelectedItem.ToString() == "Hoy")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha)) lFiltrada.Add(nota);
                }
                ActualizarGrid1(lFiltrada);
            }
            else if (comboBox1.SelectedItem.ToString() == "Todas")
            {
                label2.Text = "";
                textBox1.Text = "";
                textBox1.Visible = false;
                ActualizarNotas();
                ActualizarGrid1(_lstNotaDeEnvios);
            }
            else if (comboBox1.SelectedItem.ToString() == "Impresas")
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
            else if (comboBox1.SelectedItem.ToString() == "No impresas")
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
            else if (comboBox1.SelectedItem.ToString() == "Cliente")
            {
                label2.Text = "Dirección:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Fecha")
            {
                label2.Text = "Fecha:";
                textBox1.Text = "";
                textBox1.Visible = true;
            }
        }
        void FiltrarDatos(string texto, char x)
        {
            ActualizarNotas();
            var lstFiltrada = new List<NotaDeEnvio>();
            foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
            {
                if (x == 'c' && nota.Cliente != null && nota.Cliente.Direccion != null && nota.Cliente.Direccion.ToLower().Contains(texto.ToLower()))
                    lstFiltrada.Add(nota);
                else if (x == 'f' && nota.Fecha.Contains(texto))
                    lstFiltrada.Add(nota);
            }
            ActualizarGrid1(lstFiltrada);
        }
        private void TextBox1_TextChanged(object sender, EventArgs ev)
        {
            if (comboBox1.SelectedItem.ToString() == "Cliente")
                FiltrarDatos(textBox1.Text, 'c');
            else if (comboBox1.SelectedItem.ToString() == "Fecha")
                FiltrarDatos(textBox1.Text, 'f');
        }

        //_______________GRUPO IMPRIMIR ________________________________________
        private void Button1_Click(object sender, EventArgs ev)
        {
            if (comboBox2.Text == "") return;
            var lstAImprimir = ObtenerListaAImprimir();
            foreach (NotaDeEnvio nota in lstAImprimir)
            {
                var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                form.Rellenar_Datos(nota);
                form.Show();
                form.BringToFront();
            }
            SendToBack();
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back) ev.Handled = true;
        }
        private void TextBox3_KeyPress(object sender, KeyPressEventArgs ev)
        {
            if (!char.IsNumber(ev.KeyChar) && ev.KeyChar != (char)Keys.Back) ev.Handled = true;
        }
        public List<NotaDeEnvio> ObtenerListaAImprimir()
        {
            var listaAImprimir = new List<NotaDeEnvio>();
            if (comboBox2.SelectedItem.ToString() == "No impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 0) listaAImprimir.Add(nota);
                }
            }
            else if (comboBox2.SelectedItem.ToString() == "Hoy")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Fecha == DateTime.Now.ToString(Constants.FormatoDeFecha)) listaAImprimir.Add(nota);
                }
            }
            else if (comboBox2.SelectedItem.ToString() == "Establecer rango" && textBox2.Text != "" && textBox3.Text != "")
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
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
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
            else if (comboBox2.SelectedItem.ToString() == "Establecer rango")
            {
                textBox2.Visible = true;
                textBox3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void TextBox3_TextChanged(object sender, EventArgs ev)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }
        private void TextBox2_TextChanged(object sender, EventArgs ev)
        {
            label7.Text = ObtenerListaAImprimir().Count.ToString();
        }

        
        //_________________________GRUPO BORRAR___________________________________
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs ev)
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
            else if (comboBox3.SelectedItem.ToString() == "Establecer rango")
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
            var listaABorrar = new List<NotaDeEnvio>();
            if (_lstNotaDeEnvios == null || _lstNotaDeEnvios.Count == 0) return listaABorrar;
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
                catch { MessageBox.Show("Rango establecido incorrecto"); }
            }
            else if (comboBox3.SelectedItem.ToString() == "Todas")
            {
                listaABorrar.AddRange(_lstNotaDeEnvios);
            }
            else if (comboBox3.SelectedItem.ToString() == "Impresas")
            {
                foreach (NotaDeEnvio nota in _lstNotaDeEnvios)
                {
                    if (nota.Impresa == 1) listaABorrar.Add(nota);
                }
            }
            return listaABorrar;
        }
        private void TextBox5_TextChanged(object sender, EventArgs ev)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void TextBox4_TextChanged(object sender, EventArgs ev)
        {
            label10.Text = ObtenerListaABorrar().Count.ToString();
        }
        private void Button3_Click(object sender, EventArgs ev)
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
        private void Button5_Click(object sender, EventArgs ev)
        {
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
        }
        private void Button4_Click(object sender, EventArgs ev)
        {
            List<NotaDeEnvio> notas = ObtenerListaABorrar();
            deleteNotas(notas);
            comboBox3.SelectedItem = "(Seleccionar)";
            label11.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button3.Visible = true;
            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
        }

        // enviar a hoja de reparto
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs ev)
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
        private void TextBox6_TextChanged(object sender, EventArgs ev)    // búsqueda por id de la nota de envío para agregar a pedido
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
        private void Button7_Click(object sender, EventArgs ev)     // limpiar
        {
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }
        private void AgregarPedidoDesdeNota_Click(object sender, EventArgs ev)
        {
            try { long.Parse(textBox6.Text); } catch { return; };
            string diaDeReparto = comboBox4.Text;
            string nombreReparto = comboBox5.Text;
            NotaDeEnvio notaDeEnvio = getNotaDeEnvio(long.Parse(textBox6.Text));
            Reparto reparto = getRepartoPorDiaYNombre(diaDeReparto, nombreReparto);
            if (notaDeEnvio == null || reparto == null) return;
            if (notaDeEnvio.ProdVendidos != null)
            {
                ProdVendido prodVendidoEnPedido = notaDeEnvio.ProdVendidos.FirstOrDefault(x => x.PedidoId != null);
                if (prodVendidoEnPedido != null)
                {
                    long currentPedidoId = (long)prodVendidoEnPedido.PedidoId;
                    Pedido currentPedido = getPedido(currentPedidoId);
                    if (currentPedido != null)
                    {
                        bool successCP = cleanPedidos(new List<Pedido>() { currentPedido });
                        if (!successCP)
                        {
                            MessageBox.Show("No se pudo Limpiar Pedido");
                        }
                    }
                }
            }
            long pedidoId = addPedidoIfNotExistsAndReturnId(reparto.Id, notaDeEnvio.ClienteId);
            Pedido pedido = getPedido(pedidoId);
            if (pedidoId == 0 || pedido == null)
            {
                MessageBox.Show("Falló mandar a Pedido");
                return;
            }
            foreach (ProdVendido prodVendido in notaDeEnvio.ProdVendidos)
            {
                prodVendido.PedidoId = pedidoId;
            }
            bool success = editProdVendidos(notaDeEnvio.ProdVendidos);
            if (!success)
            {
                MessageBox.Show("No se pudieron editar Productos Vendidos para incluirlos en el Pedido");
            }
            updatePedido(pedido, true);
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }


        // Agregar o Quitar producto vendido de nota de envio
        private void TextBox7_TextChanged(object sender, EventArgs ev)
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
                    if (notaDeEnvio.Cliente != null && notaDeEnvio.Cliente.Direccion != null)
                    {
                        label18.Text = notaDeEnvio.Cliente.Direccion.ToString() + " - " + notaDeEnvio.ClienteId.ToString();
                    }
                }
                else
                {
                    label18.Text = "No encontrado";
                    label20.Text = "0";
                    _lstProdVendidos.Clear();
                    ActualizarGrid2(_lstProdVendidos);
                }
            }
            else
            {
                label18.Text = "";
                label20.Text = "0";
                _lstProdVendidos.Clear();
                ActualizarGrid2(_lstProdVendidos);
            }
        }

        // Agregar
        private void TextBox9_TextChanged(object sender, EventArgs ev)     // id producto
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
        private void TextBox12_TextChanged(object sender, EventArgs ev)     // producto por nombre
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
        private void TextBox10_TextChanged(object sender, EventArgs ev)     // cantidad a agregar
        {
            if (label25.Text == "No encontrado" || textBox10.Text == "") return;
            try { int.Parse(textBox10.Text); } catch { return; };
            var producto = getProductoPorNombreExacto(label25.Text);
            if (producto == null) return;
            label26.Text = (producto.Precio * int.Parse(textBox10.Text)).ToString();
            button9.Enabled = true;
        }
        private void AgregarProductoVendido_btn9_Click(object sender, EventArgs ev)
        {
            try { long.Parse(textBox7.Text); } catch { return; };
            Producto productoNuevo = getProductoPorNombreExacto(label25.Text);
            if (productoNuevo == null) return;
            try { long.Parse(textBox7.Text); } catch { return; };
            NotaDeEnvio notaDeEnvio = getNotaDeEnvio(long.Parse(textBox7.Text));
            try { int.Parse(textBox10.Text); } catch { return; };

            ProdVendido nuevoProdVendido = new ProdVendido()
            {
                Cantidad = int.Parse(textBox10.Text),
                Descripcion = productoNuevo.Nombre,
                NotaDeEnvioId = notaDeEnvio.Id,
                Precio = isNegativePrice(productoNuevo) ? (-1)*productoNuevo.Precio : productoNuevo.Precio,
                ProductoId = productoNuevo.Id,
                RegistroVentaId = notaDeEnvio.ProdVendidos.First().RegistroVentaId
            };
            Pedido pedido = null;
            if (notaDeEnvio.ProdVendidos != null)
            {
                var prodVendidoEnPedido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.PedidoId != null);
                if (prodVendidoEnPedido != null)
                {
                    pedido = getPedido((long)prodVendidoEnPedido.PedidoId);
                    if (pedido != null) nuevoProdVendido.PedidoId = pedido.Id;
                }
            }
            var existingProdVendido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.Producto.Nombre == productoNuevo.Nombre);
            if (existingProdVendido == null || isSaldo(existingProdVendido.Producto))
            {
                bool successPV = addProdVendidos(new List<ProdVendido>() { nuevoProdVendido });
                if (!successPV) MessageBox.Show("No se pudo agregar Producto Vendido");
            }
            else
            {
                existingProdVendido.Cantidad += nuevoProdVendido.Cantidad;
                bool successPV = editProdVendido(existingProdVendido);
                if (!successPV)
                {
                    MessageBox.Show("No se pudo modificar la cantidad vendida del Producto Vendido");
                }
            }
            _lstProdVendidos.Add(nuevoProdVendido);
            NotaDeEnvio nota = editNoteValues(notaDeEnvio);
            if (nota == null)
            {
                MessageBox.Show("No se pudo modificar la Nota de Envío para que incluya los nuevos detalles y montos");
            }
            bool success = updateVentasDesdeProdVendidos(new List<ProdVendido>() { nuevoProdVendido }, true);
            if (!success)
            {
                MessageBox.Show("No se pudieron actualizar las Ventas");
            }
            if (nuevoProdVendido.PedidoId != null)
            {
                pedido = getPedido((long)nuevoProdVendido.PedidoId);
                Pedido pedidoActualizado = updatePedido(pedido, true);
                if (pedidoActualizado == null)
                {
                    MessageBox.Show("No se pudo actualizar el Pedido");
                }
            }
            else
            {
                MessageBox.Show("Esta nota no estaba en ningún reparto");
            }

            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
            NotaDeEnvio updatedNote = getNotaDeEnvio(long.Parse(textBox7.Text));
            ActualizarGrid2(updatedNote.ProdVendidos.ToList());
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
            textBox12.Text = "";
            textBox11.Visible = false;
            button9.Enabled = false;
        }

        //Quitar
        private void TextBox8_TextChanged(object sender, EventArgs ev)
        {
            if (textBox8.Text != "")
            {
                ProdVendido prodVendido = _lstProdVendidos.Find(x => x.Producto.Nombre.ToLower().Contains(textBox8.Text.ToLower()));
                if (prodVendido != null)
                {
                    label22.Text = prodVendido.Descripcion;
                    button8.Enabled = true;
                }
                else
                {
                    label22.Text = "No encontrado";
                    button8.Enabled = false;
                }
            }
            else
            {
                label22.Text = "";
                button8.Enabled = false;
            }
        }
        private void Quitar_btn8_Click(object sender, EventArgs ev)
        {
            if (label18.Text == "" || label18.Text == "No encontrado" || textBox7.Text == "") return;
            try { long.Parse(textBox7.Text); } catch { return; };
            NotaDeEnvio notaDeEnvio = getNotaDeEnvio(long.Parse(textBox7.Text));
            ProdVendido prodVendido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.Descripcion == label22.Text);
            if (notaDeEnvio == null || prodVendido == null) return;
            if (_lstProdVendidos.Count < 2)
            {
                MessageBox.Show("No se puede quitar el único producto que tiene una nota, hay que eliminarla");
                return;
            }
            notaDeEnvio = editNotaDeEnvioQuitar(notaDeEnvio, prodVendido);
            prodVendido.NotaDeEnvioId = null;
            prodVendido.RegistroVentaId = null;
            prodVendido.PedidoId = null;

            var pedidoId = prodVendido.PedidoId;
            prodVendido.PedidoId = null;
            bool success = editProdVendido(prodVendido);
            if (!success)
            {
                MessageBox.Show("No se pudo modificar Producto Vendido para quitarlo de la Nota");
            }
            success = updateVentasDesdeProdVendidos(new List<ProdVendido>() { prodVendido }, false);
            if (!success)
            {
                MessageBox.Show("No se pudo modificar Producto Vendido para quitarlo de las Ventas");
            }
            if (pedidoId != null)
            {
                Pedido pedido = getPedido((long)pedidoId);
                if (pedido != null)
                {
                    Pedido pedidoActualizado = updatePedido(pedido, true);
                    if (pedidoActualizado == null)
                    {
                        MessageBox.Show("Hubo algún problema al modificar el Pedido");
                    }
                }
            }
            ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
            notaDeEnvio = getNotaDeEnvio(long.Parse(textBox7.Text));
            if (notaDeEnvio == null || notaDeEnvio.ProdVendidos == null) return;
            _lstProdVendidos = notaDeEnvio.ProdVendidos.ToList();
            ActualizarGrid2(_lstProdVendidos);
            label20.Text = notaDeEnvio.ImporteTotal.ToString();
            textBox8.Text = "";
            label22.Text = "";
            button8.Enabled = false;
        }

        private void ExportarAExcel_Btn_Click(object sender, EventArgs ev)
        {
            ICollection<NotaDeEnvio> notas = getNotaDeEnvios();
            bool success = new Exportar().ExportarNotas(notas);
            if (success)
            {
                button2.Text = "TERMINADO";
            }
            else
            {
                MessageBox.Show("No se pudieron exportar Notas");
            }
        }
    }
}
