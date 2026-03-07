using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
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
            if (label25.Text == "No encontrado" || textBox10.Text == "")
            {
                return;
            }
            string nombreDeProducto = label25.Text;
            string cantidadTexto = textBox10.Text;
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
            if (!int.TryParse(textBox10.Text, out int cantidad) || !long.TryParse(textBox7.Text, out long notaDeEnvioId))
            {
                return;
            }
            NotaDeEnvio updatedNote = await UIExecutor.ExecuteAsync(
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
                    Producto productoNuevo = await productoServices.GetProductoPorNombreExactoAsync(nombreDeProducto);
                    NotaDeEnvio notaDeEnvio = await notaDeEnvioServices.GetNotaDeEnvioPorIdAsync(notaDeEnvioId);
                    ProdVendido nuevoProdVendido = new ProdVendido()
                    {
                        Cantidad = int.Parse(textBox10.Text),
                        Descripcion = productoNuevo.Nombre,
                        NotaDeEnvioId = notaDeEnvio.Id,
                        Precio = ProductoServices.IsNegativePrice(productoNuevo) ? (-1) * productoNuevo.Precio : productoNuevo.Precio,
                        ProductoId = productoNuevo.Id,
                        RegistroVentaId = notaDeEnvio.ProdVendidos.First().RegistroVentaId
                    };
                    Pedido pedido = null;
                    if (notaDeEnvio.ProdVendidos != null)
                    {
                        var prodVendidoEnPedido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.PedidoId != null);  // se puede tomar directamente el pedido...
                        if (prodVendidoEnPedido != null)
                        {
                            pedido = await pedidoServices.GetPedidoPorIdAsync((long)prodVendidoEnPedido.PedidoId);
                            if (pedido != null)
                            {
                                nuevoProdVendido.Pedido = pedido;
                            }
                        }
                    }
                    var existingProdVendido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.Producto.Nombre == productoNuevo.Nombre);
                    if (existingProdVendido == null || ProductoServices.IsSaldo(existingProdVendido.Producto))
                    {
                        prodVendidoServices.AddProdVendidos(new List<ProdVendido>() { nuevoProdVendido });
                    }
                    else
                    {
                        existingProdVendido.Cantidad += nuevoProdVendido.Cantidad;
                        prodVendidoServices.EditProdVendido(existingProdVendido);
                    }
                    _lstProdVendidos.Add(nuevoProdVendido);
                    notaDeEnvioServices.EditValores(notaDeEnvio);
                    await ventaServices.UpdateVentasDesdeProdVendidosAsync(new List<ProdVendido>() { nuevoProdVendido }, true);
                    if (nuevoProdVendido.Pedido != null)
                    {
                        //pedido = await getPedido((long)nuevoProdVendido.PedidoId);
                        PedidoServices.ActualizarEtiquetasDePedido(pedido, true);
                        await pedidoServices.AddPedidoAsync(pedido);  // no hay pedido para agregar
                        RepartoServices.ActualizarEtiquetasDeReparto(pedido.Reparto);
                        repartoServices.EditReparto(pedido.Reparto);
                    }
                    else
                    {
                        MessageBox.Show("Esta Nota no estaba en ningún Reparto");
                        savingServices.DiscardChanges();
                        return null;
                    }
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                        return null;
                    }
                    //NotaDeEnvio updatedNote = await notaDeEnvioServices.GetNotaDeEnvio(notaDeEnvioId);
                    return notaDeEnvio;
                },
                "No se pudo realizar",
                this
            );
            if (updatedNote == null)
            {
                return;
            }
            await ActualizarNotas();
            ActualizarGrid1(_lstNotaDeEnvios);
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
