using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private async void TextBox7_TextChanged(object sender, EventArgs ev)
        {
            string numeroDeNota = textBox7.Text;
            if (numeroDeNota == "")
            {
                label18.Text = "";
                label20.Text = "0";
                _lstProdVendidos.Clear();
                ActualizarGrid2(_lstProdVendidos);
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
                    NotaDeEnvio notaDeEnvio = await notaDeEnvioServices.GetNotaDeEnvioPorIdAsync(notaDeEnvioId);
                    return notaDeEnvio;
                },
                "No se pudo buscar Nota de Envío",
                null
            );
            if (notaDeEnvio == null || notaDeEnvio.ProdVendidos == null)
            {
                label18.Text = "No encontrado";
                label20.Text = "0";
                _lstProdVendidos.Clear();
                ActualizarGrid2(_lstProdVendidos);
                return;
            }
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
        // Agregar
        private async void TextBox9_TextChanged(object sender, EventArgs ev)     // id producto
        {
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
                    return await productoServices.GetProductoPorIdAsync(productoId);
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
            label25.Text = producto.Nombre;
            if (textBox10.Text != "")
            {
                button9.Enabled = true;
            }
            textBox11.Visible = label25.Text.Contains("actura");
        }
        private async void TextBox12_TextChanged(object sender, EventArgs ev)     // producto por nombre
        {
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
                    return await productoServices.GetProductoPorNombreAsync(nombreDeProducto);
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
            label25.Text = producto.Nombre;
            if (textBox10.Text != "")
            {
                button9.Enabled = true;
            }
            textBox11.Visible = label25.Text.Contains("actura");
            
        }
        private async void TextBox10_TextChanged(object sender, EventArgs ev)     // cantidad a agregar
        {
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
            Producto producto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    return await productoServices.GetProductoPorNombreExactoAsync(nombreDeProducto);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto == null)
            {
                return;
            }
            label26.Text = (producto.Precio * int.Parse(textBox10.Text)).ToString();
            button9.Enabled = true;
        }
        private async void AgregarProductoVendido_btn9_Click(object sender, EventArgs ev)
        {
            string nombreDeProducto = label25.Text;
            string cantidadTexto = textBox10.Text;
            string numeroDeNota = textBox7.Text;
            if (nombreDeProducto == "No encontrado")
            {
                return;
            }
            if (!int.TryParse(textBox10.Text, out int cantidad) || !long.TryParse(numeroDeNota, out long notaDeEnvioId))
            {
                return;
            }
            NotaDeEnvio notaDeEnvio = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    //
                    Producto producto = await productoServices.GetProductoPorNombreExactoAsync(nombreDeProducto);
                    NotaDeEnvio notaDeEnvio = await notaDeEnvioServices.GetNotaDeEnvioPorIdAsync(notaDeEnvioId);
                    var nuevoProdVendido = new ProdVendido()
                    {
                        Cantidad = cantidad,
                        Descripcion = producto.Nombre,
                        NotaDeEnvio = notaDeEnvio,
                        NotaDeEnvioId = notaDeEnvioId,
                        Precio = ProductoServices.IsNegativePrice(producto)
                            ? (-1) * producto.Precio
                            : producto.Precio,
                        Producto = producto,
                        ProductoId = producto.Id,
                        RegistroVentaId = notaDeEnvio.ProdVendidos.FirstOrDefault().RegistroVentaId  // nullable
                    };
                    Pedido pedido = null;
                    // si la nota de envío estaba en algún reparto (mediante sus prod. vendidos) se agregar el prod. vendido nuevo al reparto
                    var prodVendidoEnPedido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.PedidoId != null);
                    if (prodVendidoEnPedido != null)
                    {
                        pedido = await pedidoServices.GetPedidoPorIdAsync((long)prodVendidoEnPedido.PedidoId);
                        nuevoProdVendido.Pedido = pedido;
                    }
                    // si el prod. vendido ya estaba en esta nota de envío, se suma la cantidad y se actualizan las etiquetas
                    var prodVendidos = await prodVendidoServices.GetProdVendidosAsync();
                    var existingProdVendido = prodVendidos.ToList().Find(x => x.NotaDeEnvioId == notaDeEnvioId && x.Producto.Id == producto.Id);
                    if (existingProdVendido == null || ProductoServices.IsSaldo(existingProdVendido.Producto))
                    {
                        prodVendidoServices.AddProdVendido(nuevoProdVendido);
                    }
                    else
                    {
                        existingProdVendido.Cantidad += nuevoProdVendido.Cantidad;
                        prodVendidoServices.EditProdVendido(existingProdVendido);
                    }
                    // se actualiza la nota de envío
                    notaDeEnvio.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(notaDeEnvio.ProdVendidos);
                    notaDeEnvio.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(notaDeEnvio.ProdVendidos);
                    notaDeEnvioServices.EditNotaDeEnvio(notaDeEnvio);
                    // se actualizan las ventas
                    await ventaServices.UpdateVentasDesdeProdVendidosAsync(new List<ProdVendido>() { nuevoProdVendido }, true);
                    // se actualiza el reparto si está la nota en el reparto (mediante sus prod. vendidos)
                    if (pedido != null)
                    {
                        var reparto = await repartoServices.GetRepartoPorIdAsync(pedido.RepartoId);
                        RepartoServices.ActualizarEtiquetasDeReparto(reparto);
                        repartoServices.EditReparto(reparto);
                        PedidoServices.ActualizarEtiquetasDePedido(pedido, pedido.Entregar == 1);
                        pedidoServices.EditPedido(pedido);
                    }
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                        return null;
                    }
                    return notaDeEnvio;
                },
                "No se pudo realizar",
                this
            );
            if (notaDeEnvio == null)
            {
                return;
            }
            await ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
            ActualizarGrid2(notaDeEnvio.ProdVendidos.ToList());
            decimal impTotal = 0;
            _lstProdVendidos = notaDeEnvio.ProdVendidos.ToList();
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
            string nombreDeProducto = textBox8.Text;
            if (nombreDeProducto == "")
            {
                label22.Text = "";
                button8.Enabled = false;
                return;
            }
            ProdVendido prodVendido = _lstProdVendidos.Find(x => x.Producto.Nombre.ToLower().Contains(textBox8.Text.ToLower()));
            if (prodVendido == null)
            {
                label22.Text = "No encontrado";
                button8.Enabled = false;
                return;
            }
            label22.Text = prodVendido.Descripcion;
            button8.Enabled = true;
        }
        private async void Quitar_btn8_Click(object sender, EventArgs ev)
        {
            string nombreDeProducto = label22.Text;
            string numeroDeNota = textBox7.Text;
            if (label18.Text == "" || label18.Text == "No encontrado" || numeroDeNota == "")
            {
                return;
            }
            if (!long.TryParse(textBox7.Text, out long notaDeEnvioId))
            {
                return;
            }
            NotaDeEnvio notaDeEnvio = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    //
                    NotaDeEnvio notaDeEnvio = await notaDeEnvioServices.GetNotaDeEnvioPorIdAsync(notaDeEnvioId);
                    ProdVendido prodVendido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.Descripcion == nombreDeProducto);
                    if (notaDeEnvio == null || prodVendido == null)
                    {
                        return null;
                    }
                    if (_lstProdVendidos.Count < 2)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se puede quitar el único producto que tiene una nota, hay que eliminarla");
                        return null;
                    }
                    prodVendidoServices.DeleteProdVendido(prodVendido);
                    var lstAuxiliar = new List<ProdVendido>();
                    foreach (ProdVendido pv in notaDeEnvio.ProdVendidos)
                    {
                        if (pv.ProductoId != prodVendido.ProductoId)
                        {
                            lstAuxiliar.Add(pv);
                        }
                    }
                    notaDeEnvio.ProdVendidos = lstAuxiliar;
                    notaDeEnvio.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(lstAuxiliar);
                    notaDeEnvio.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(lstAuxiliar);
                    notaDeEnvioServices.EditNotaDeEnvio(notaDeEnvio);
                    prodVendido.NotaDeEnvioId = null;
                    prodVendido.RegistroVentaId = null;
                    prodVendido.PedidoId = null;
                    var pedidoId = prodVendido.PedidoId;
                    prodVendido.PedidoId = null;
                    prodVendidoServices.EditProdVendido(prodVendido);
                    await ventaServices.UpdateVentasDesdeProdVendidosAsync(new List<ProdVendido>() { prodVendido }, false);
                    if (pedidoId != null)
                    {
                        Pedido pedido = await pedidoServices.GetPedidoPorIdAsync((long)pedidoId);
                        if (pedido != null)
                        {
                            PedidoServices.ActualizarEtiquetasDePedido(pedido, true);
                            pedidoServices.EditPedido(pedido);
                            RepartoServices.ActualizarEtiquetasDeReparto(pedido.Reparto);
                            repartoServices.EditReparto(pedido.Reparto);
                        }
                    }
                    //notaDeEnvio = await getNotaDeEnvio(...);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                        return null;
                    }
                    return notaDeEnvio;
                },
                "No se pudo realizar",
                this
            );
            if (notaDeEnvio == null)
            {
                return;
            }
            await ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
            _lstProdVendidos = notaDeEnvio.ProdVendidos.ToList();
            ActualizarGrid2(_lstProdVendidos);
            label20.Text = notaDeEnvio.ImporteTotal.ToString();
            //
            textBox8.TextChanged -= TextBox8_TextChanged;  // evita error de concurrencia de DbContext
            textBox8.Text = "";
            textBox8.TextChanged += TextBox8_TextChanged;
            //
            label22.Text = "";
            button8.Enabled = false;
        }
    }
}
