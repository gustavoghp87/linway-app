using linway_app.Models;
using linway_app.Repositories;
using linway_app.Services;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormCrearNota : Form
    {
        private readonly List<ProdVendido> _lstProdVendidos;
        private readonly IServicioNotaDeEnvio _servNotaDeEnvio;
        private readonly IServicioCliente _servCliente;
        private readonly IServicioProducto _servProducto;
        private readonly IServicioVenta _servVenta;
        private readonly IServicioRegistroVenta _servRegistroVenta;
        private readonly IServicioDiaReparto _servDiaReparto;
        private readonly IServicioReparto _servReparto;
        public FormCrearNota(
            IServicioNotaDeEnvio servNotaDeEnvio, IServicioCliente servCliente, IServicioProducto servProducto,
            IServicioVenta servVenta, IServicioRegistroVenta servRegistroVenta, IServicioDiaReparto servDiaReparto,
            IServicioReparto servReparto
        )
        {
            InitializeComponent();
            _lstProdVendidos = new List<ProdVendido>();
            _servNotaDeEnvio = servNotaDeEnvio;
            _servCliente = servCliente;
            _servProducto = servProducto;
            _servVenta = servVenta;
            _servRegistroVenta = servRegistroVenta;
            _servDiaReparto = servDiaReparto;
            _servReparto = servReparto;
        }
        private void FormCrearNota_Load(object sender, EventArgs e)
        {
            if (_lstProdVendidos == null || _lstProdVendidos.Count == 0) return;
            dataGridView4.DataSource = _lstProdVendidos.ToArray();
            dataGridView4.Columns[0].Visible = false;
            dataGridView4.Columns[1].Width = 30;
            dataGridView4.Columns[2].Width = 40;
            dataGridView4.Columns[3].Width = 25;
            dataGridView4.Columns[4].Visible = true;
            dataGridView4.Columns[5].Width = 30;
            dataGridView4.Columns[6].Visible = false;
            dataGridView4.Columns[7].Visible = false;
        }
        private void Actualizar()
        {
            if (_lstProdVendidos == null || _lstProdVendidos.Count == 0) return;
            dataGridView4.DataSource = _lstProdVendidos.ToArray();
            dataGridView4.Columns[0].Visible = false;
            dataGridView4.Columns[1].Width = 30;
            dataGridView4.Columns[2].Visible = false;
            dataGridView4.Columns[3].Visible = false;
            dataGridView4.Columns[4].Visible = false;
            dataGridView4.Columns[5].Width = 30;
            dataGridView4.Columns[6].Visible = true;
            dataGridView4.Columns[7].Visible = true;
            dataGridView4.Columns[8].Visible = false;
            dataGridView4.Columns[9].Visible = false;
            dataGridView4.Columns[10].Visible = false;
            dataGridView4.Columns[11].Visible = false;
            //dataGridView4.Columns[12].Visible = false;
        }

        private List<NotaDeEnvio> GetNotas()
        {
            return _servNotaDeEnvio.GetAll();
        }
        private bool AgregarNota(NotaDeEnvio notaDeEnvio)
        {
            bool response = _servNotaDeEnvio.Add(notaDeEnvio);
            if (!response) MessageBox.Show("Algo falló al agregar Nota de Envío a base de datos");
            return response;
        }
        private Cliente GetCliente(long id)
        {
            return _servCliente.Get(id);
        }
        private Producto GetProducto(long id)
        {
            return _servProducto.Get(id);
        }
        private List<Venta> GetVentas()
        {
            return _servVenta.GetAll();
        }
        private bool AgregarRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = _servRegistroVenta.Add(registroVenta);
            if (!response) MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
            return response;
        }
        private bool EditarVenta(Venta venta)
        {
            bool response = _servVenta.Edit(venta);
            if (!response) MessageBox.Show("Algo falló al editar Venta en base de datos");
            return response;
        }
        private bool AgregarVenta(Venta venta)
        {
            bool response = _servVenta.Add(venta);
            if (!response) MessageBox.Show("Algo falló al agregar Venta a base de datos");
            return response;
        }
        private List<Reparto> GetRepartosPorDia(string diaReparto)
        {
            List<DiaReparto> lstDiasRep = _servDiaReparto.GetAll();
            List<Reparto> lstRepartos = lstDiasRep.Find(x => x.Dia == diaReparto).Reparto.ToList();
            return lstRepartos;
        }
        private void AgregarPedidoARepartoPorNota(Reparto reparto, Cliente cliente)
        {
            bool response = _servReparto.AgregarPedidoARepartoFormNota(reparto, cliente, _lstProdVendidos);
            if (!response) MessageBox.Show("Algo falló al agregar Pedido a Reparto en base de datos");
        }

        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
        private void CargarCliente_Leave(object sender, EventArgs e)
        {
            if (textBox15.Text != "")
            {
                Cliente cliente = GetCliente(long.Parse(textBox15.Text));
                if (cliente == null)
                {
                    label36.Text = "No encontrado";
                    return;
                }
                label36.Text = cliente.Direccion;
            }
        }
        private void CargarProducto_Leave(object sender, EventArgs e)
        {
            if (textBox16.Text != "")
            {
                var producto = GetProducto(long.Parse(textBox16.Text));
                if (producto == null)
                {
                    label38.Text = "No encontrado";
                    return;
                }
                label38.Text = producto.Nombre;
                if (label38.Text.Contains("actura")) textBox20.Visible = true;
                else textBox20.Visible = false;
            }
        }
        private void CargarSubtotal_Leave(object sender, EventArgs e)
        {
            if ((label38.Text != "No encontrado") && (textBox16.Text != "") && (textBox17.Text != ""))
            {
                Producto producto = GetProducto(long.Parse(textBox16.Text));
                if (producto == null) return;
                label40.Text = ( producto.Precio * int.Parse(textBox17.Text) ).ToString();   // subtotal
            }
        }
        private void LimpiarLista_Click(object sender, EventArgs e)
        {
            _lstProdVendidos.Clear();
            dataGridView4.DataSource = _lstProdVendidos.ToArray();
            label42.Text = "";
        }
        private void NuevoProdVendido_btn13_Click(object sender, EventArgs e)
        {
            if ((label38.Text != "No encontrado") && (textBox16.Text != "") && (textBox17.Text != "") && (label40.Text != ""))
            {
                Producto producto = GetProducto(long.Parse(textBox16.Text));
                ProdVendido nuevoPV;
                if (producto.Nombre.Contains("pendiente"))
                    nuevoPV = new ProdVendido
                    {
                        ProductoId = producto.Id,
                        Descripcion = label38.Text,
                        Cantidad = 1,
                        Precio = producto.Precio
                    };
                else if (producto.Nombre.Contains("favor") || producto.Nombre.Contains("devoluci") || producto.Nombre.Contains("BONIF"))
                    nuevoPV = new ProdVendido
                    {
                        ProductoId = producto.Id,
                        Descripcion = label38.Text,
                        Cantidad = 1,
                        Precio = producto.Precio * -1
                    };
                else if (producto.Nombre.Contains("actura"))
                    nuevoPV = new ProdVendido
                    {
                        ProductoId = producto.Id,
                        Descripcion = label38.Text + textBox20.Text,
                        Cantidad = 1,
                        Precio = producto.Precio
                    };
                else
                    nuevoPV = new ProdVendido
                    {
                        ProductoId = producto.Id,
                        Descripcion = label38.Text,
                        Cantidad = int.Parse(textBox17.Text),
                        Precio = producto.Precio
                    };
                _lstProdVendidos.Add(nuevoPV);
                double impTotal = 0;
                foreach (ProdVendido vActual in _lstProdVendidos)
                {
                    impTotal += vActual.Precio;
                }
                textBox20.Text = "";
                textBox20.Visible = false;
                label42.Text = impTotal.ToString();
                textBox16.Text = "";
                textBox17.Text = "";
                label38.Text = "";
                label40.Text = "";
                Actualizar(); // dataGridView4.DataSource = _lstProdVendidos.ToArray();
            }
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)       // enviar a hoja de reparto checkbox
            {
                label33.Visible = true;
                label34.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
            }
            else
            {
                label33.Visible = false;
                label34.Visible = false;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
            }
        }

        private bool EsProducto(string nombre)
        {
            bool es = true;
            if ((nombre.Contains("pendiente")) || (nombre.Contains("favor")) || (nombre.Contains("actura"))
                || (nombre.Contains("evoluc")) || (nombre.Contains("cobrar") || (nombre.Contains("BONIFI")))
            )
            {
                es = false;
            }
            return es;
        }

        private void ConfirmarCrearNota_Click(object sender, EventArgs e)
        {
            if ((label36.Text != "No encontrado") && (textBox15.Text != ""))
            {
                var cliente = GetCliente(long.Parse(textBox15.Text));
                if (cliente == null) return;

                NotaDeEnvio nuevaNota = new NotaDeEnvio
                {
                    ProdVendidos = _lstProdVendidos,
                    ClientId = cliente.Id,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Impresa = 0,
                    Detalle = ServicioNotaDeEnvio.ExtraerDetalle(_lstProdVendidos),
                    ImporteTotal = ServicioNotaDeEnvio.ExtraerImporte(_lstProdVendidos)
                };
                bool response = AgregarNota(nuevaNota);
                if (!response) return;

                if (checkBox4.Checked)      // agregar productos vendidos a lista de registros y a lista de ventas
                {
                    RegistroVenta nuevoRegistro = new RegistroVenta()
                    {
                        Cliente = cliente,
                        ClienteId = cliente.Id,
                        Fecha = DateTime.Now.ToString(),
                        NombreCliente = cliente.Direccion,
                        ProdVendido = _lstProdVendidos
                    };
                    AgregarRegistroVenta(nuevoRegistro);


                    foreach (var prodVendido in _lstProdVendidos)
                    {
                        if (EsProducto(prodVendido.Producto.Nombre))
                        {
                            List<Venta> lstVentas = GetVentas();
                            bool exists = false;
                            foreach (var venta in lstVentas)
                            {
                                if (venta.ProductoId == prodVendido.ProductoId)
                                {
                                    exists = true;
                                    venta.Cantidad += prodVendido.Cantidad;
                                    EditarVenta(venta);
                                }
                            }
                            if (!exists)
                            {
                                Venta nuevaVenta = new Venta
                                {
                                    ProductoId = prodVendido.ProductoId,
                                    Cantidad = prodVendido.Cantidad,
                                    Producto = GetProducto(prodVendido.ProductoId)
                                };
                                AgregarVenta(nuevaVenta);
                            }
                        }
                    }
                }

                if (checkBox1.Checked)      // imprimir checkbox
                {
                    FormImprimirNota formimprimir = new FormImprimirNota();
                    formimprimir.Rellenar_Datos(nuevaNota);
                    formimprimir.Show();
                }

                if (checkBox3.Checked)     // enviar a hoja de reparto como pedido nuevo para X reparto
                {
                    List<Reparto> repartos = GetRepartosPorDia(comboBox4.Text);
                    Reparto reparto = repartos.Find(x => x.Nombre == comboBox3.Text);
                    AgregarPedidoARepartoPorNota(reparto, cliente);
                }
                Close();
            }
            else
            {
                MessageBox.Show("Verifique los campos");
            }
        }

        private void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs e)
        {
            string diaDeReparto = comboBox4.Text;
            List<Reparto> repartos = GetRepartosPorDia(diaDeReparto);  // partiendo del día seleccionado, buscar sus repartos
            comboBox3.DataSource = repartos;                           // combobox "día", necesito la lista de dias de rep
            comboBox3.DisplayMember = "Nombre";
            comboBox3.ValueMember = "Nombre";
        }

        private void CerrarBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
