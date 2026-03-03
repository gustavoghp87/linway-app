using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Enums;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormCrearNota : Form
    {
        private async void TextBox15_TextChanged(object sender, EventArgs ev)  // cliente por número
        {
            string numeroDeCliente = textBox15.Text;
            textBox1.Text = "";
            if (numeroDeCliente == "")
            {
                label36.Text = "";
                labelClienteId.Text = "";
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
                label36.Text = "No encontrado";
                labelClienteId.Text = "";
                return;
            }
            label36.Text = cliente.Direccion;
            labelClienteId.Text = cliente.Id.ToString();
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)  // cliente por dirección
        {
            string direccion = textBox1.Text;
            if (direccion == "")
            {
                label36.Text = "";
                labelClienteId.Text = "";
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
                label36.Text = "No encontrado";
                labelClienteId.Text = "";
                return;
            }
            label36.Text = cliente.Direccion;
            labelClienteId.Text = cliente.Id.ToString();
        }
        private async void TextBox16_TextChanged(object sender, EventArgs ev)  // producto por número
        {
            string numeroDeProducto = textBox16.Text;
            if (numeroDeProducto == "")
            {
                label38.Text = "";
                labelProductoId.Text = "";
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
                label38.Text = "No encontrado";
                labelProductoId.Text = "";
                return;
            }
            label38.Text = producto.Nombre;
            labelProductoId.Text = producto.Id.ToString();
            textBox20.Visible = label38.Text.Contains("actura");
        }
        private async void TextBox2_TextChanged(object sender, EventArgs ev)  // producto por nombre
        {
            string nombreDeProducto = textBox2.Text;
            if (nombreDeProducto == "")
            {
                label38.Text = "";
                labelProductoId.Text = "";
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
                label38.Text = "No encontrado";
                labelProductoId.Text = "";
                return;
            }
            label38.Text = producto.Nombre;
            labelProductoId.Text = producto.Id.ToString();
            textBox20.Visible = label38.Text.Contains("actura");
        }
        private async void TextBox17_TextChanged(object sender, EventArgs ev)  // cantidad
        {
            string numeroDeProducto = labelProductoId.Text;
            if (numeroDeProducto == "" || !int.TryParse(textBox17.Text, out int cantidad))
            {
                label40.Text = "";
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
                label40.Text = "";
                return;
            }
            label40.Text = (producto.Precio * cantidad).ToString();   // subtotal
        }
        private async void AnyadirProdVendidos_Click(object sender, EventArgs ev)
        {
            if (labelClienteId.Text == "" || labelProductoId.Text == "")
            {
                return;
            }
            string numeroDeProducto = labelProductoId.Text;
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
                return;
            }
            if (!int.TryParse(textBox17.Text, out int cantidad))
            {
                return;
            }
            bool exists = false;
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                if (exists || ProductoServices.IsSaldo(prodVendido.Producto))
                {
                    continue;
                }
                if (prodVendido.ProductoId == producto.Id)
                {
                    exists = true;
                    prodVendido.Cantidad += cantidad;
                }
            }
            if (!exists)
            {
                var nuevoPV = new ProdVendido
                {
                    Cantidad = cantidad,
                    Descripcion = label38.Text,
                    Precio = producto.Precio,
                    ProductoId = producto.Id
                };
                if (producto.Tipo == TipoProducto.Saldo.ToString() && producto.SubTipo != TipoSaldo.SaldoPendiente.ToString())
                {
                    if (ProductoServices.IsNegativePrice(producto))
                    {
                        nuevoPV.Precio = producto.Precio * -1;
                    }
                    else if (producto.SubTipo == TipoSaldo.AFacturar.ToString())
                    {
                        nuevoPV.Descripcion = label38.Text + textBox20.Text;
                    }
                }
                _lstProdVendidosAAgregar.Add(nuevoPV);
                _lstProductosAAgregar.Add(producto);
            }
            decimal impTotal = 0;
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                impTotal += prodVendido.Precio * prodVendido.Cantidad;
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
            ActualizarGrid();
        }
    }
}
