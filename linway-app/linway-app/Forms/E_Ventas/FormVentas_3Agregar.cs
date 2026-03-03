using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private void NuevaVenta_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            showing = "agregarReg";
            gbNuevaVenta.Visible = true;
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
        }
        private async void InputProductoId_TextChanged(object sender, EventArgs ev)
        {
            string numeroDeProducto = textBox12.Text;
            if (numeroDeProducto == "")
            {
                label28.Text = "";
                labelPrecio.Text = "";
                return;
            }
            if (!long.TryParse(numeroDeProducto, out long productoId))
            {
                return;
            }
            Producto producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    return await productoServices.GetProductoPorIdAsync(productoId);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label28.Text = "No encontrado";
                labelPrecio.Text = "";
                return;
            }
            label28.Text = producto.Nombre;
            labelPrecio.Text = producto.Precio.ToString();
        }
        private async void TextBox4_TextChanged(object sender, EventArgs ev)
        {
            string nombreDeProducto = textBox4.Text;
            if (textBox4.Text != "")
            {
                label28.Text = "";
                labelPrecio.Text = "";
                return;
            }
            Producto producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    return await productoServices.GetProductoPorNombreAsync(nombreDeProducto);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label28.Text = "No encontrado";
                labelPrecio.Text = "";
                return;
            }
            label28.Text = producto.Nombre;
            labelPrecio.Text = producto.Precio.ToString();
        }
        private void Limpiar_Click(object sender, EventArgs ev)
        {
            _lstAgregarVentas.Clear();
            ActualizarGrid5(_lstAgregarVentas);
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
        }
        private async void Anyadir_Click(object sender, EventArgs ev)
        {
            if(!int.TryParse(textBox13.Text, out int cantidad))
            {
                return;
            }
            string nombreDeProducto = label28.Text;
            Producto producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    return await productoServices.GetProductoPorNombreAsync(nombreDeProducto);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null) return;
            var nuevaVenta = new Venta
            {
                ProductoId = producto.Id,
                Cantidad = cantidad,
                Producto = producto      // dejar para que cargue el nombre en el grid
            };
            _lstAgregarVentas.Add(nuevaVenta);
            ActualizarGrid5(_lstAgregarVentas);
            label28.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox4.Text = "";
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs ev)        //enviar HDR
        {
            bool enviarAReparto = checkBox2.Checked;
            if (enviarAReparto)
            {
                label6.Visible = true;
                label7.Visible = true;
                comboBox2.Visible = true;
                comboBox1.Visible = true;
                label15.Visible = true;
                label20.Text = "";
                textBox19.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                label6.Visible = false;
                label7.Visible = false;
                comboBox2.Visible = false;
                comboBox1.Visible = false;
                label15.Visible = false;
                label20.Text = "";
                textBox19.Visible = false;
                textBox3.Visible = false;
                textBox19.Text = "";
                textBox3.Text = "";
            }
        }
        private async void AgregarAReparto_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string dia = comboBox1.Text;
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    return await orquestacionServices.GetRepartosPorDiaAsync(dia);
                },
                "No se pudieron buscar los Repartos por Día",
                null
            );
            if (repartos == null)
            {
                return;
            }
            comboBox2.DataSource = repartos;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Nombre";
        }
        private async void TextBox19_TextChanged(object sender, EventArgs ev)
        {
            string numeroDeCliente = textBox19.Text;
            if (numeroDeCliente == "")
            {
                label20.Text = "";
                return;
            }
            if (!long.TryParse(numeroDeCliente, out long clienteId))
            {
                return;
            }
            Cliente cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetClientePorIdAsync(clienteId);
                },
                "No se pudo buscar el cliente",
                null
            );
            if (cliente == null)
            {
                label20.Text = "No encontrado";
                return;
            }
            label20.Text = cliente.Direccion;
        }
        private async void TextBox3_TextChanged(object sender, EventArgs ev)
        {
            string direccion = textBox3.Text;
            if (direccion == "")
            {
                label20.Text = "";
                return;
            }
            Cliente cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetClientePorDireccionAsync(direccion);
                },
                "No se pudo buscar el cliente",
                null
            );
            if (cliente == null)
            {
                label20.Text = "No encontrado";
                return;
            }
            label20.Text = cliente.Direccion;
        }
        private async void AgregarVenta_Click(object sender, EventArgs ev)
        {
            //var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            if (_lstAgregarVentas == null || _lstAgregarVentas.Count == 0)
            {
                MessageBox.Show("No se han ingresado productos");
                return;
            }
            //Console.WriteLine($"[{stopwatch.ElapsedMilliseconds} ms] Punto 1");
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    //
                    string dia = comboBox1.Text;
                    string nombre = comboBox2.Text;
                    string direccionCliente = label20.Text;
                    Cliente cliente = null;
                    Reparto reparto = null;
                    Pedido pedido = null;
                    bool enviarAReparto = checkBox2.Checked;
                    if (enviarAReparto)   // enviar a reparto
                    {
                        if (direccionCliente == "" || dia == "" || nombre == "")
                        {
                            MessageBox.Show("Faltan el cliente o el reparto");
                            return false;
                        }
                        cliente = await clienteServices.GetClientePorDireccionExactaAsync(label20.Text);
                        reparto = await orquestacionServices.GetRepartoPorDiaYNombreAsync(dia, nombre);
                        if (cliente == null || reparto == null)
                        {
                            MessageBox.Show("Falló cliente o reparto");
                            return false;
                        }
                        pedido = await orquestacionServices.GetPedidoPorRepartoYClienteGenerarSiNoExisteAsync(reparto.Id, cliente.Id);
                        if (pedido == null)
                        {
                            MessageBox.Show("Falló pedido");
                            return false;
                        }
                    }
                    // generar un RegistroVenta con "Venta particular"y devuelve un id para usar en los ProdVendido
                    // generar un Venta por cada ítem en _lstAgregarVentas, no necesita precios
                    // generar un ProdVendido por cada ítem en _lstAgregarVentas con el id de (1)
                    ActualizarGrid3Ventas();
                    var nuevoRegistroVenta = new RegistroVenta
                    {   // hacer o editar registros (no puedo editar porque no tengo cliente, se modifica si hay "enviar a hdr")
                        ClienteId = 634,
                        NombreCliente = "Venta particular",
                        Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha)
                    };
                    var ventasAAgregar = new List<Venta>();
                    var ventasAEditar = new List<Venta>();
                    var prodVendidosAAgregar = new List<ProdVendido>();
                    var prodVendidosAEditar = new List<ProdVendido>();
                    int counter = 0;
                    List<Venta> lstVentas = await ventaServices.GetVentasAsync();
                    List<Producto> productos = await productoServices.GetProductosAsync();
                    List<ProdVendido> prodVendidos = enviarAReparto ? await prodVendidoServices.GetProdVendidos() : null;
                    foreach (Venta ventaParaAgregar in _lstAgregarVentas)
                    {
                        counter++;
                        bool exists = false;
                        foreach (Venta venta in lstVentas)
                        {
                            if (venta.ProductoId != ventaParaAgregar.ProductoId)
                            {
                                continue;
                            }
                            exists = true;
                            venta.Cantidad += ventaParaAgregar.Cantidad;
                            ventasAEditar.Add(venta);
                        }
                        if (!exists)
                        {
                            ventasAAgregar.Add(ventaParaAgregar);
                        }
                        Producto producto = productos.Find(x => x.Id == ventaParaAgregar.ProductoId);
                        if (producto == null)
                        {
                            MessageBox.Show("Falló Producto");
                            return false;
                        }
                        exists = false;
                        if (enviarAReparto && pedido.Id != 0)  // pedido.Id 0 implica que se acaba de crear
                        {
                            foreach (ProdVendido prodVendido in prodVendidos)
                            {
                                if (prodVendido.ProductoId == producto.Id && prodVendido.PedidoId == pedido.Id)
                                {
                                    exists = true;
                                    prodVendido.Cantidad += ventaParaAgregar.Cantidad;
                                    prodVendidosAEditar.Add(prodVendido);
                                    break;
                                }
                            }
                        }
                        if (!exists)
                        {
                            var nuevoProdVendido = new ProdVendido
                            {
                                Cantidad = ventaParaAgregar.Cantidad,
                                Descripcion = producto.Nombre,
                                Precio = producto.Precio,
                                ProductoId = producto.Id,
                                RegistroVenta = nuevoRegistroVenta
                            };
                            prodVendidosAAgregar.Add(nuevoProdVendido);
                        }
                        //Actualizar();  comentado
                    }
                    ventaServices.AddVentas(ventasAAgregar);
                    ventaServices.EditVentas(ventasAEditar);
                    if (enviarAReparto)
                    {
                        foreach (ProdVendido pv in prodVendidosAAgregar)
                        {
                            pv.Pedido = pedido;
                        }
                        prodVendidoServices.AddProdVendidos(prodVendidosAAgregar);
                        if (pedido.Id == 0)
                        {
                            pedidoServices.AddPedido(pedido);
                        } else
                        {
                            await orquestacionServices.UpdatePedidoAsync(pedido, true);
                        }
                        nuevoRegistroVenta.ClienteId = cliente.Id;
                        nuevoRegistroVenta.NombreCliente = cliente.Direccion;
                    }
                    else
                    {
                        prodVendidoServices.AddProdVendidos(prodVendidosAAgregar);
                        prodVendidoServices.EditProdVendidos(prodVendidosAEditar);
                    }
                    registroVentaServices.AddRegistroVenta(nuevoRegistroVenta);
                    return await savingServices.SaveAsync();
                },
                "No se pudo realizar",
                this
            );
            if (!logrado)
            {
                return;
            }
            _lstAgregarVentas.Clear();
            LimpiarPantalla();
            await Actualizar();
            //Console.WriteLine($"[{stopwatch.ElapsedMilliseconds} ms] Punto 8");
        }
    }
}
