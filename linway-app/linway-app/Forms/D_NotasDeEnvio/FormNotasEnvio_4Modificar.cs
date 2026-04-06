using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private NotaDeEnvio _notaDeEnvioAModificar;
        private void ActualizarGrid2AgregarProductoANota(ICollection<ProdVendido> lstProdVendidos)
        {
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                grid.Add(Form1.Mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView2.DataSource = grid;
            dataGridView2.Columns[0].Width = 25;
            dataGridView2.Columns[1].Width = 200;
        }
        private async void TextBox7_TextChanged(object sender, EventArgs ev)  // nota de envío por Id
        {
            _notaDeEnvioAModificar = null;
            label2ModificarEstaEnPedido.Text = "";
            label10ModificarEstaEnRV.Text = "";
            label2ModificarAgregarExplicacion.Text = "";
            label29ModificarQuitarExplicacion.Text = "";
            checkBox1ModificarRestarDeVentas.Visible = false;
            //
            string numeroDeNota = textBox7.Text;
            if (numeroDeNota == "")
            {
                label18Modificar_ClienteDireccionId.Text = "";
                label20ModificarImporteTotal.Text = "0";
                _lstProdVendidos.Clear();
                ActualizarGrid2AgregarProductoANota(_lstProdVendidos);
                return;
            }
            if (!long.TryParse(numeroDeNota, out long notaDeEnvioId))
            {
                return;
            }
            NotaDeEnvio notaDeEnvio = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    NotaDeEnvio notaDeEnvio = await servicesContext.NotaDeEnvioServices.GetPorIdAsync(notaDeEnvioId);
                    return notaDeEnvio;
                },
                "No se pudo buscar Nota de Envío",
                null
            );
            if (notaDeEnvio == null)
            {
                label18Modificar_ClienteDireccionId.Text = "No encontrado";
                label20ModificarImporteTotal.Text = "0";
                _lstProdVendidos.Clear();
                ActualizarGrid2AgregarProductoANota(_lstProdVendidos);
                return;
            }
            _notaDeEnvioAModificar = notaDeEnvio;
            _lstProdVendidos = notaDeEnvio.ProdVendidos.ToList();
            ActualizarGrid2AgregarProductoANota(_lstProdVendidos);
            decimal impTotal = 0;
            foreach (ProdVendido nota in _lstProdVendidos)
            {
                impTotal += nota.Cantidad * nota.Precio;
            }
            label20ModificarImporteTotal.Text = impTotal.ToString();
            label18Modificar_ClienteDireccionId.Text = notaDeEnvio.Cliente.Direccion.ToString() + " - " + notaDeEnvio.ClienteId.ToString();
            label2ModificarAgregarExplicacion.Text = "Esta opción agrega un producto a la Nota de Envío seleccionada.";
            label29ModificarQuitarExplicacion.Text = "Esta opción quita un producto de la Nota de Envío seleccionada.";
            //
            Pedido pedido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.PedidoId != null)?.Pedido;
            label2ModificarEstaEnPedido.Text = pedido != null ? $"Reparto: {pedido.Reparto.DiaReparto.Dia} {pedido.Reparto.Nombre}" : "No está en un Reparto";
            if (pedido != null)
            {
                label2ModificarAgregarExplicacion.Text += " También lo agrega al Pedido.";
                label29ModificarQuitarExplicacion.Text += " También lo remueve del Pedido.";
            }
            //
            RegistroVenta registro = notaDeEnvio.ProdVendidos.ToList().Find(x => x.RegistroVentaId != null)?.RegistroVenta;
            label10ModificarEstaEnRV.Text = registro != null ? $"Registro de Venta: {registro.Id}" : "No está en un Registro de Venta";
            checkBox1ModificarRestarDeVentas.Visible = registro != null;
            if (registro != null)
            {
                label2ModificarAgregarExplicacion.Text += " Y también lo agrega al Registro de Venta y a Ventas.";
                label29ModificarQuitarExplicacion.Text += " Y también lo remueve del Registro de Venta. Se resta de Ventas opcionalmente.";
            }
        }
    }

    public partial class FormNotasEnvio : Form  // Agregar un producto a la nota de envío
    {
        private Producto _productoAAgregar;
        private int _cantidadAAgregar;
        private async void TextBox9_TextChanged(object sender, EventArgs ev)     // producto por Id
        {
            _productoAAgregar = null;
            label26ModificarAgregarImporte.Text = "";
            string numeroDeProducto = textBox9.Text;
            if (numeroDeProducto == "")
            {
                label25ModificarAgregarProductoNombre.Text = "";
                button9ModificarAgregar.Enabled = true;
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
                    var servicesContext = ServiceContext.Get(sp);
                    return await servicesContext.ProductoServices.GetPorIdAsync(productoId);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label25ModificarAgregarProductoNombre.Text = "No encontrado";
                button9ModificarAgregar.Enabled = true;
                return;
            }
            _productoAAgregar = producto;
            label25ModificarAgregarProductoNombre.Text = producto.Nombre;
            if (textBox10ModificarAgregarProductoCantidad.Text != "")
            {
                button9ModificarAgregar.Enabled = true;
            }
            textBox11.Visible = label25ModificarAgregarProductoNombre.Text.Contains("actura");
        }
        private async void TextBox12_TextChanged(object sender, EventArgs ev)     // producto por nombre
        {
            _productoAAgregar = null;
            label26ModificarAgregarImporte.Text = "";
            string nombreDeProducto = textBox12.Text;
            if (nombreDeProducto == "")
            {
                label25ModificarAgregarProductoNombre.Text = "";
                button9ModificarAgregar.Enabled = true;
                return;
            }
            Producto producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var servicesContext = ServiceContext.Get(sp);
                    return await servicesContext.ProductoServices.GetPorNombreAsync(nombreDeProducto);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label25ModificarAgregarProductoNombre.Text = "No encontrado";
                button9ModificarAgregar.Enabled = true;
                return;
            }
            _productoAAgregar = producto;
            label25ModificarAgregarProductoNombre.Text = producto.Nombre;
            if (textBox10ModificarAgregarProductoCantidad.Text != "")
            {
                button9ModificarAgregar.Enabled = true;
            }
            textBox11.Visible = label25ModificarAgregarProductoNombre.Text.Contains("actura");
            
        }
        private void TextBox10_TextChanged(object sender, EventArgs ev)     // cantidad a agregar
        {
            _cantidadAAgregar = 0;
            string nombreDeProducto = label25ModificarAgregarProductoNombre.Text;
            string cantidadTexto = textBox10ModificarAgregarProductoCantidad.Text;
            if (nombreDeProducto == "No encontrado" || cantidadTexto == "")
            {
                return;
            }
            if (!int.TryParse(textBox10ModificarAgregarProductoCantidad.Text, out int cantidad))
            {
                return;
            }
            _cantidadAAgregar = cantidad;
            if (_productoAAgregar == null)
            {
                return;
            }
            label26ModificarAgregarImporte.Text = (_productoAAgregar.Precio * _cantidadAAgregar).ToString();
            button9ModificarAgregar.Enabled = true;
        }
        private async void AgregarProductoVendido_btn9_Click(object sender, EventArgs ev)
        {
            if (_notaDeEnvioAModificar == null || _productoAAgregar == null || _cantidadAAgregar == 0)
            {
                return;
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    var registroDeVentaId = _notaDeEnvioAModificar.ProdVendidos.FirstOrDefault().RegistroVentaId;
                    var nuevoProdVendido = new ProdVendido()
                    {
                        Cantidad = _cantidadAAgregar,
                        Descripcion = _productoAAgregar.Nombre,
                        NotaDeEnvio = _notaDeEnvioAModificar,
                        NotaDeEnvioId = _notaDeEnvioAModificar.Id,
                        Precio = ProductoServices.IsNegativePrice(_productoAAgregar)
                            ? (-1) * _productoAAgregar.Precio
                            : _productoAAgregar.Precio,
                        Producto = _productoAAgregar,
                        ProductoId = _productoAAgregar.Id,
                        RegistroVentaId = registroDeVentaId  // nullable
                    };
                    Pedido pedido = null;
                    // si la nota de envío estaba en algún reparto (mediante sus prod. vendidos) se agregar el prod. vendido nuevo al reparto
                    var prodVendidoEnPedido = _notaDeEnvioAModificar.ProdVendidos.ToList().Find(x => x.PedidoId != null);
                    if (prodVendidoEnPedido != null)
                    {
                        pedido = await servicesContext.PedidoServices.GetPorIdAsync((long)prodVendidoEnPedido.PedidoId);
                        nuevoProdVendido.Pedido = pedido;
                    }
                    // si el prod. vendido ya estaba en esta nota de envío, se suma la cantidad y se actualizan las etiquetas
                    var prodVendidos = await servicesContext.ProdVendidoServices.GetAllAsync();
                    var existingProdVendido = prodVendidos.ToList().Find(x => x.NotaDeEnvioId == _notaDeEnvioAModificar.Id && x.Producto.Id == _productoAAgregar.Id);
                    if (existingProdVendido == null || ProductoServices.IsSaldo(existingProdVendido.Producto))
                    {
                        servicesContext.ProdVendidoServices.Add(nuevoProdVendido);
                    }
                    else
                    {
                        existingProdVendido.Cantidad += nuevoProdVendido.Cantidad;
                        servicesContext.ProdVendidoServices.Edit(existingProdVendido);
                    }
                    // se actualiza la nota de envío
                    _notaDeEnvioAModificar.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(_notaDeEnvioAModificar.ProdVendidos);
                    _notaDeEnvioAModificar.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(_notaDeEnvioAModificar.ProdVendidos);
                    servicesContext.NotaDeEnvioServices.Edit(_notaDeEnvioAModificar);
                    // se actualizan las ventas
                    if (registroDeVentaId != null)
                    {
                        await servicesContext.VentaServices.SumarDesdeProdVendidosAsync(nuevoProdVendido);
                    }
                    // se actualiza el reparto si está la nota en el reparto (mediante sus prod. vendidos)
                    if (pedido != null)
                    {
                        var reparto = await servicesContext.RepartoServices.GetPorIdAsync(pedido.RepartoId);
                        RepartoServices.ActualizarCantidadesDeReparto(reparto);
                        servicesContext.RepartoServices.Edit(reparto);
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, pedido.Entregar == 1);
                        servicesContext.PedidoServices.Edit(pedido);
                    }
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo realizar",
                this
            );
            if (!logrado)
            {
                return;
            }
            await ActualizarNotas();
            EventoCombobox1ListaModalidad();
            ActualizarGrid2AgregarProductoANota(_notaDeEnvioAModificar.ProdVendidos.ToList());
            decimal impTotal = 0;
            _lstProdVendidos = _notaDeEnvioAModificar.ProdVendidos.ToList();
            foreach (ProdVendido prodVend in _lstProdVendidos)
            {
                impTotal += prodVend.Cantidad * prodVend.Precio;
            }
            label20ModificarImporteTotal.Text = impTotal.ToString();
            label25ModificarAgregarProductoNombre.Text = "";
            label26ModificarAgregarImporte.Text = "";
            textBox9.Text = "";
            textBox10ModificarAgregarProductoCantidad.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox11.Visible = false;
            button9ModificarAgregar.Enabled = false;
            _notaDeEnvioAModificar = null;
            _productoAAgregar = null;
            _cantidadAAgregar = 0;
            label2ModificarAgregarExplicacion.Text = "";
            label29ModificarQuitarExplicacion.Text = "";
        }
    }

    public partial class FormNotasEnvio : Form  // Quitar un producto de la nota de envío
    {
        private ProdVendido _prodVendidoAQuitar;
        private void TextBox8_TextChanged(object sender, EventArgs ev)  // producto por nombre a quitar
        {
            _prodVendidoAQuitar = null;
            string nombreDeProducto = textBox8QuitarProducto_Nombre.Text;
            if (nombreDeProducto == "")
            {
                label22ModificarQuitarProductoIdNombre.Text = "";
                button8ModificarQuitar.Enabled = false;
                return;
            }
            ProdVendido prodVendido = _lstProdVendidos.Find(x => x.Producto.Nombre.ToLower().Contains(textBox8QuitarProducto_Nombre.Text.ToLower()));
            if (prodVendido == null)
            {
                label22ModificarQuitarProductoIdNombre.Text = "No encontrado";
                button8ModificarQuitar.Enabled = false;
                return;
            }
            _prodVendidoAQuitar = prodVendido;
            label22ModificarQuitarProductoIdNombre.Text = $"{prodVendido.ProductoId} {prodVendido.Descripcion}";
            button8ModificarQuitar.Enabled = true;
        }
        private async void Quitar_btn8_Click(object sender, EventArgs ev)
        {
            if (_notaDeEnvioAModificar == null || _prodVendidoAQuitar == null)
            {
                return;
            }
            if (_lstProdVendidos.Count < 2)
            {
                MessageBox.Show("No se puede quitar el único producto que tiene una nota, hay que eliminarla");
                return;
            }
            bool restarDeVentas = checkBox1ModificarRestarDeVentas.Checked;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    if (_prodVendidoAQuitar.RegistroVentaId != null && restarDeVentas)
                    {
                        await servicesContext.VentaServices.RestarDesdeProdVendidosAsync(_prodVendidoAQuitar);
                    }
                    //
                    if (_prodVendidoAQuitar.PedidoId != null)
                    {
                        Pedido pedido = await servicesContext.PedidoServices.GetPorIdAsync((long)_prodVendidoAQuitar.PedidoId);
                        pedido.ProdVendidos.Remove(_prodVendidoAQuitar);
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, pedido.Entregar == 1);
                        servicesContext.PedidoServices.Edit(pedido);
                        RepartoServices.ActualizarCantidadesDeReparto(pedido.Reparto);
                        servicesContext.RepartoServices.Edit(pedido.Reparto);
                    }
                    //
                    _notaDeEnvioAModificar.ProdVendidos.Remove(_prodVendidoAQuitar);
                    _notaDeEnvioAModificar.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(_notaDeEnvioAModificar.ProdVendidos);
                    _notaDeEnvioAModificar.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(_notaDeEnvioAModificar.ProdVendidos);
                    servicesContext.NotaDeEnvioServices.Edit(_notaDeEnvioAModificar);
                    //
                    // nada para actualizar en RV
                    //
                    servicesContext.ProdVendidoServices.Delete(_prodVendidoAQuitar);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo realizar",
                this
            );
            if (!logrado)
            {
                return;
            }
            await ActualizarNotas();
            EventoCombobox1ListaModalidad();
            _lstProdVendidos = _notaDeEnvioAModificar.ProdVendidos.ToList();
            ActualizarGrid2AgregarProductoANota(_lstProdVendidos);
            label20ModificarImporteTotal.Text = _notaDeEnvioAModificar.ImporteTotal.ToString();
            textBox8QuitarProducto_Nombre.Text = "";
            checkBox1ModificarRestarDeVentas.Visible = false;
            label22ModificarQuitarProductoIdNombre.Text = "";
            button8ModificarQuitar.Enabled = false;
            _notaDeEnvioAModificar = null;
            _prodVendidoAQuitar = null;
            label2ModificarAgregarExplicacion.Text = "";
            label29ModificarQuitarExplicacion.Text = "";
        }
    }
}
