using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        private Producto _productoAAgregar;
        private ProdVendido _prodVendidoAQuitar;
        private int _cantidadAAgregar;
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
            string numeroDeNota = textBox7.Text;
            if (numeroDeNota == "")
            {
                label18.Text = "";
                label20.Text = "0";
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
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    NotaDeEnvio notaDeEnvio = await notaDeEnvioServices.GetPorIdAsync(notaDeEnvioId);
                    return notaDeEnvio;
                },
                "No se pudo buscar Nota de Envío",
                null
            );
            if (notaDeEnvio == null)
            {
                label18.Text = "No encontrado";
                label20.Text = "0";
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
            label20.Text = impTotal.ToString();
            label18.Text = notaDeEnvio.Cliente.Direccion.ToString() + " - " + notaDeEnvio.ClienteId.ToString();
        }
        // Agregar
        private async void TextBox9_TextChanged(object sender, EventArgs ev)     // producto por Id
        {
            _productoAAgregar = null;
            label26.Text = "";
            string numeroDeProducto = textBox9.Text;
            if (numeroDeProducto == "")
            {
                label25.Text = "";
                button9.Enabled = true;
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
                    return await productoServices.GetPorIdAsync(productoId);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label25.Text = "No encontrado";
                button9.Enabled = true;
                return;
            }
            _productoAAgregar = producto;
            label25.Text = producto.Nombre;
            if (textBox10.Text != "")
            {
                button9.Enabled = true;
            }
            textBox11.Visible = label25.Text.Contains("actura");
        }
        private async void TextBox12_TextChanged(object sender, EventArgs ev)     // producto por nombre
        {
            _productoAAgregar = null;
            label26.Text = "";
            string nombreDeProducto = textBox12.Text;
            if (nombreDeProducto == "")
            {
                label25.Text = "";
                button9.Enabled = true;
                return;
            }
            Producto producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    return await productoServices.GetPorNombreAsync(nombreDeProducto);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                label25.Text = "No encontrado";
                button9.Enabled = true;
                return;
            }
            _productoAAgregar = producto;
            label25.Text = producto.Nombre;
            if (textBox10.Text != "")
            {
                button9.Enabled = true;
            }
            textBox11.Visible = label25.Text.Contains("actura");
            
        }
        private void TextBox10_TextChanged(object sender, EventArgs ev)     // cantidad a agregar
        {
            _cantidadAAgregar = 0;
            string nombreDeProducto = label25.Text;
            string cantidadTexto = textBox10.Text;
            if (nombreDeProducto == "No encontrado" || cantidadTexto == "")
            {
                return;
            }
            if (!int.TryParse(textBox10.Text, out int cantidad))
            {
                return;
            }
            _cantidadAAgregar = cantidad;
            if (_productoAAgregar == null)
            {
                return;
            }
            label26.Text = (_productoAAgregar.Precio * _cantidadAAgregar).ToString();
            button9.Enabled = true;
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
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    //
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
                        RegistroVentaId = _notaDeEnvioAModificar.ProdVendidos.FirstOrDefault().RegistroVentaId  // nullable
                    };
                    Pedido pedido = null;
                    // si la nota de envío estaba en algún reparto (mediante sus prod. vendidos) se agregar el prod. vendido nuevo al reparto
                    var prodVendidoEnPedido = _notaDeEnvioAModificar.ProdVendidos.ToList().Find(x => x.PedidoId != null);
                    if (prodVendidoEnPedido != null)
                    {
                        pedido = await pedidoServices.GetPorIdAsync((long)prodVendidoEnPedido.PedidoId);
                        nuevoProdVendido.Pedido = pedido;
                    }
                    // si el prod. vendido ya estaba en esta nota de envío, se suma la cantidad y se actualizan las etiquetas
                    var prodVendidos = await prodVendidoServices.GetAllAsync();
                    var existingProdVendido = prodVendidos.ToList().Find(x => x.NotaDeEnvioId == _notaDeEnvioAModificar.Id && x.Producto.Id == _productoAAgregar.Id);
                    if (existingProdVendido == null || ProductoServices.IsSaldo(existingProdVendido.Producto))
                    {
                        prodVendidoServices.Add(nuevoProdVendido);
                    }
                    else
                    {
                        existingProdVendido.Cantidad += nuevoProdVendido.Cantidad;
                        prodVendidoServices.Edit(existingProdVendido);
                    }
                    // se actualiza la nota de envío
                    _notaDeEnvioAModificar.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(_notaDeEnvioAModificar.ProdVendidos);
                    _notaDeEnvioAModificar.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(_notaDeEnvioAModificar.ProdVendidos);
                    notaDeEnvioServices.Edit(_notaDeEnvioAModificar);
                    // se actualizan las ventas
                    await ventaServices.UpdateDesdeProdVendidosAsync(new List<ProdVendido>() { nuevoProdVendido }, true);
                    // se actualiza el reparto si está la nota en el reparto (mediante sus prod. vendidos)
                    if (pedido != null)
                    {
                        var reparto = await repartoServices.GetPorIdAsync(pedido.RepartoId);
                        RepartoServices.ActualizarCantidadesDeReparto(reparto);
                        repartoServices.Edit(reparto);
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, pedido.Entregar == 1);
                        pedidoServices.Edit(pedido);
                    }
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
            label20.Text = impTotal.ToString();
            label25.Text = "";
            label26.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox11.Visible = false;
            button9.Enabled = false;
            _notaDeEnvioAModificar = null;
            _productoAAgregar = null;
            _cantidadAAgregar = 0;
        }
        //Quitar
        private void TextBox8_TextChanged(object sender, EventArgs ev)  // producto por nombre a quitar
        {
            _prodVendidoAQuitar = null;
            string nombreDeProducto = textBox8QuitarProducto_Nombre.Text;
            if (nombreDeProducto == "")
            {
                label22.Text = "";
                button8.Enabled = false;
                return;
            }
            ProdVendido prodVendido = _lstProdVendidos.Find(x => x.Producto.Nombre.ToLower().Contains(textBox8QuitarProducto_Nombre.Text.ToLower()));
            if (prodVendido == null)
            {
                label22.Text = "No encontrado";
                button8.Enabled = false;
                return;
            }
            _prodVendidoAQuitar = prodVendido;
            label22.Text = prodVendido.Descripcion;
            button8.Enabled = true;
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
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    //
                    _prodVendidoAQuitar.NotaDeEnvioId = null;
                    prodVendidoServices.EditOrDeleteMany(new List<ProdVendido>() { _prodVendidoAQuitar });
                    //
                    var lstAuxiliar = new List<ProdVendido>();
                    foreach (ProdVendido pv in _notaDeEnvioAModificar.ProdVendidos)  // o hacer Remove
                    {
                        if (pv.ProductoId != _prodVendidoAQuitar.ProductoId)
                        {
                            lstAuxiliar.Add(pv);
                        }
                    }
                    _notaDeEnvioAModificar.ProdVendidos = lstAuxiliar;
                    _notaDeEnvioAModificar.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(lstAuxiliar);
                    _notaDeEnvioAModificar.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(lstAuxiliar);
                    notaDeEnvioServices.Edit(_notaDeEnvioAModificar);
                    //
                    await ventaServices.UpdateDesdeProdVendidosAsync(new List<ProdVendido>() { _prodVendidoAQuitar }, false);
                    //
                    if (_prodVendidoAQuitar.PedidoId != null)
                    {
                        Pedido pedido = await pedidoServices.GetPorIdAsync((long)_prodVendidoAQuitar.PedidoId);
                        pedido.ProdVendidos.Remove(_prodVendidoAQuitar);
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, pedido.Entregar == 1);
                        pedidoServices.Edit(pedido);
                        RepartoServices.ActualizarCantidadesDeReparto(pedido.Reparto);
                        repartoServices.Edit(pedido.Reparto);
                    }
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
            label20.Text = _notaDeEnvioAModificar.ImporteTotal.ToString();
            textBox8QuitarProducto_Nombre.Text = "";
            label22.Text = "";
            button8.Enabled = false;
            _notaDeEnvioAModificar = null;
            _prodVendidoAQuitar = null;
        }
    }
}
