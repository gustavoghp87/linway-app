using linway_app.Models;
using linway_app.Services;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServicioProdVendido _servProdVendido;

        public FormCrearNota(
            IServicioNotaDeEnvio servNotaDeEnvio, IServicioCliente servCliente, IServicioProducto servProducto,
            IServicioVenta servVenta, IServicioRegistroVenta servRegistroVenta, IServicioDiaReparto servDiaReparto,
            IServicioReparto servReparto, IServicioProdVendido servProdVendido
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
            _servProdVendido = servProdVendido;
        }
        private void FormCrearNota_Load(object sender, EventArgs e)
        {
            //if (_lstProdVendidos == null || _lstProdVendidos.Count == 0) return;
            //dataGridView4.DataSource = _lstProdVendidos.ToArray();
            //dataGridView4.Columns[0].Visible = false;
            //dataGridView4.Columns[1].Width = 30;
            //dataGridView4.Columns[2].Width = 40;
            //dataGridView4.Columns[3].Width = 25;
            //dataGridView4.Columns[4].Visible = true;
            //dataGridView4.Columns[5].Width = 30;
            //dataGridView4.Columns[6].Visible = false;
            //dataGridView4.Columns[7].Visible = false;
            Actualizar();
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
            dataGridView4.Columns[12].Visible = false;
        }
        private long AgregarNota(NotaDeEnvio notaDeEnvio)
        {
            long response = _servNotaDeEnvio.AddAndGetId(notaDeEnvio);
            if (response == 0) MessageBox.Show("Algo falló al agregar Nota de Envío a base de datos");
            return response;
        }
        private Cliente GetCliente(long id)
        {
            return _servCliente.Get(id);
        }
        private Cliente GetClientePorDire(string dire)
        {
            var lst = _servCliente.GetAll();
            if (lst == null || lst.Count == 0) return null;
            var cliente = lst.Find(x => x.Direccion.ToLower().Contains(dire.ToLower()));
            return cliente;
        }
        private Cliente GetClientePorDireExacta(string dire)
        {
            var lst = _servCliente.GetAll();
            if (lst == null || lst.Count == 0) return null;
            var cliente = lst.Find(x => x.Direccion.Equals(dire));
            return cliente;
        }
        private Producto GetProducto(long id)
        {
            return _servProducto.Get(id);
        }
        private Producto GetProductoPorNombre(string nombre)
        {
            var lst = _servProducto.GetAll();
            if (lst == null || lst.Count == 0) return null;
            var producto = lst.Find(x => x.Nombre.ToLower().Contains(nombre.ToLower()));
            return producto;
        }
        private Producto GetProductoPorNombreExacto(string nombre)
        {
            var lst = _servProducto.GetAll();
            if (lst == null || lst.Count == 0) return null;
            var producto = lst.Find(x => x.Nombre.Equals(nombre));
            return producto;
        }
        private List<Venta> GetVentas()
        {
            return _servVenta.GetAll();
        }
        private long AgregarRegistroVenta(RegistroVenta registroVenta)
        {
            long response = _servRegistroVenta.AddAndGetId(registroVenta);
            if (response == 0) MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
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
            bool response = _servReparto.AgregarPedidoAReparto(cliente.Id, reparto.DiaReparto.Dia, reparto.Nombre, _lstProdVendidos);
            if (!response) MessageBox.Show("Algo falló al agregar Pedido a Reparto en base de datos");
        }
        private void AgregarProductoVendido(ProdVendido prodVendido)
        {
            bool response = _servProdVendido.Add(prodVendido);
            if (!response) MessageBox.Show("Algo falló al agregar Producto Vendido a base de datos");
        }
        private void SoloNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }


        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (textBox15.Text != "")
            {
                Cliente cliente = GetCliente(long.Parse(textBox15.Text));
                if (cliente == null)
                {
                    label36.Text = "No encontrado";
                    labelClienteId.Text = "";
                    return;
                }
                label36.Text = cliente.Direccion;
                labelClienteId.Text = cliente.Id.ToString();
            }
            else
            {
                label36.Text = "";
                labelClienteId.Text = "";
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Cliente cliente = GetClientePorDire(textBox1.Text);
                if (cliente == null)
                {
                    label36.Text = "No encontrado";
                    labelClienteId.Text = "";
                    return;
                }
                label36.Text = cliente.Direccion;
                labelClienteId.Text = cliente.Id.ToString();
            }
            else
            {
                label36.Text = "";
                labelClienteId.Text = "";
            }
        }
        private void CargarCliente_Leave(object sender, EventArgs e)
        {}
        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            if (textBox16.Text != "")
            {
                try { long.Parse(textBox16.Text); } catch { };
                var producto = GetProducto(long.Parse(textBox16.Text));
                if (producto == null)
                {
                    label38.Text = "No encontrado";
                    labelProductoId.Text = "";
                    return;
                }
                label38.Text = producto.Nombre;
                labelProductoId.Text = producto.Id.ToString();
                if (label38.Text.Contains("actura")) textBox20.Visible = true;
                else textBox20.Visible = false;
            }
            else
            {
                label38.Text = "";
                labelProductoId.Text = "";
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)  // prod x nombr
        {
            if (textBox2.Text != "")
            {
                var producto = GetProductoPorNombre(textBox2.Text);
                if (producto == null)
                {
                    label38.Text = "No encontrado";
                    labelProductoId.Text = "";
                    return;
                }
                label38.Text = producto.Nombre;
                labelProductoId.Text = producto.Id.ToString();
                if (label38.Text.Contains("actura")) textBox20.Visible = true;
                else textBox20.Visible = false;
            }
            else
            {
                label38.Text = "";
                labelProductoId.Text = "";
            }
        }
        private void CargarProducto_Leave(object sender, EventArgs e)
        {}
        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (labelProductoId.Text != "")
            {
                try { long.Parse(labelProductoId.Text); } catch { return; };
                Producto producto = GetProducto(long.Parse(labelProductoId.Text));
                if (producto == null || labelProductoId.Text == "" || textBox17.Text == "")
                {
                    label40.Text = "";
                    return;
                }
                label40.Text = (producto.Precio * long.Parse(textBox17.Text)).ToString();   // subtotal
            }
            else
            {
                label40.Text = "";
            }
        }
        private void CargarSubtotal_Leave(object sender, EventArgs e)
        {}
        private void LimpiarLista_Click(object sender, EventArgs e)
        {
            _lstProdVendidos.Clear();
            dataGridView4.DataSource = _lstProdVendidos.ToArray();
            label42.Text = "";
        }


        private void NuevoProdVendidos_btn13_Click(object sender, EventArgs e)
        {
            if (labelClienteId.Text != "" && labelProductoId.Text != "")
            {
                try { long.Parse(labelProductoId.Text); } catch { return; };
                var producto = GetProducto(long.Parse(labelProductoId.Text));
                var cantidad = long.Parse(labelProductoId.Text);

                for (int i = 0; i < cantidad; i++)
                {
                    ProdVendido nuevoPV = new ProdVendido
                    {
                        ProductoId = producto.Id,
                        Descripcion = label38.Text,
                        Cantidad = 1,
                        Precio = producto.Precio,
                        Estado = "Activo"
                    };
                    if (producto.Nombre.Contains("pendiente"))
                    {

                    }
                    else if (producto.Nombre.Contains("favor")
                          || producto.Nombre.Contains("devoluci")
                          || producto.Nombre.Contains("BONIF")
                    )
                        nuevoPV.Precio = producto.Precio * -1;
                    else if (producto.Nombre.Contains("actura"))
                        nuevoPV.Descripcion = label38.Text + textBox20.Text;
                    else
                        nuevoPV.Cantidad = int.Parse(textBox17.Text);
                    _lstProdVendidos.Add(nuevoPV);
                }

                double impTotal = 0;
                foreach (ProdVendido prodVendido in _lstProdVendidos)
                {
                    impTotal += prodVendido.Precio;
                }
                label42.Text = impTotal.ToString();

                textBox20.Text = "";
                textBox20.Visible = false;
                textBox16.Text = "";
                textBox17.Text = "";
                textBox2.Text = "";
                label38.Text = "";
                label40.Text = "";
                labelProductoId.Text = "";
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
            if (labelClienteId.Text != "" && labelClienteId.Text != "No encontrado")
            {
                var cliente = GetClientePorDireExacta(label36.Text);
                if (cliente == null) return;

                NotaDeEnvio nuevaNota = new NotaDeEnvio
                {
                    ClientId = cliente.Id,
                    Client = cliente,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Impresa = 0,
                    Detalle = ServicioNotaDeEnvio.ExtraerDetalle(_lstProdVendidos),
                    ImporteTotal = ServicioNotaDeEnvio.ExtraerImporte(_lstProdVendidos),
                    Estado = "Activo"
                };
                long notaId = AgregarNota(nuevaNota);
                if (notaId == 0)
                {
                    MessageBox.Show("Falló al procesar Nota nueva");
                    return;
                }
                foreach (var prodVendido in _lstProdVendidos)
                {
                    prodVendido.NotaDeEnvioId = notaId;
                    AgregarProductoVendido(prodVendido);
                }

                if (checkBox4.Checked)      // agregar productos vendidos a lista de registros y a lista de ventas
                {
                    RegistroVenta nuevoRegistro = new RegistroVenta
                    {
                        ClienteId = cliente.Id,
                        Cliente = cliente,
                        Fecha = DateTime.Now.ToString(),
                        NombreCliente = cliente.Direccion,
                        Estado = "Activo"
                    };
                    long registroId = AgregarRegistroVenta(nuevoRegistro);
                    foreach (var prodVendido in _lstProdVendidos)
                    {
                        prodVendido.RegistroVentaId = registroId;
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

                foreach (var prodVendido in _lstProdVendidos)
                {
                    prodVendido.NotaDeEnvioId = notaId;
                    AgregarProductoVendido(prodVendido);
                }

                if (checkBox1.Checked)      // imprimir checkbox
                {
                    var form = Program.GetConfig().GetRequiredService<FormImprimirNota>();
                    form.Rellenar_Datos(nuevaNota);
                    form.Show();
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
