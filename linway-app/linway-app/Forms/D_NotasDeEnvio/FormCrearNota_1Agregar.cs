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
        private int _cantidad;
        private async void TextBox15_TextChanged(object sender, EventArgs ev)  // cliente por número
        {
            _cliente = null;
            string numeroDeCliente = textBox15ClienteNumeroBusqueda.Text;
            textBox1ClienteDireccionBusqueda.Text = "";
            if (numeroDeCliente == "")
            {
                label36ClienteDireccion.Text = "";
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
                    return await clienteServices.GetPorIdAsync(clienteId);
                },
                "No se pudo buscar el cliente",
                null
            );
            if (cliente == null)
            {
                label36ClienteDireccion.Text = "No encontrado";
                labelClienteId.Text = "";
                return;
            }
            _cliente = cliente;
            label36ClienteDireccion.Text = cliente.Direccion;
            labelClienteId.Text = cliente.Id.ToString();
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)  // cliente por dirección
        {
            _cliente = null;
            string direccion = textBox1ClienteDireccionBusqueda.Text;
            if (direccion == "")
            {
                label36ClienteDireccion.Text = "";
                labelClienteId.Text = "";
                return;
            }
            Cliente cliente = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    return await clienteServices.GetPorDireccionAsync(direccion);
                },
                "No se pudo buscar el cliente",
                null
            );
            if (cliente == null)
            {
                label36ClienteDireccion.Text = "No encontrado";
                labelClienteId.Text = "";
                return;
            }
            _cliente = cliente;
            label36ClienteDireccion.Text = cliente.Direccion;
            labelClienteId.Text = cliente.Id.ToString();
        }
        private async void TextBox16_TextChanged(object sender, EventArgs ev)  // producto por número
        {
            _producto = null;
            label40ProductoSubtotal.Text = "";
            string numeroDeProducto = textBox16ProductoNombreBusqueda.Text;
            if (numeroDeProducto == "")
            {
                label38ProductoNombre.Text = "";
                labelProductoNumero.Text = "";
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
                label38ProductoNombre.Text = "No encontrado";
                labelProductoNumero.Text = "";
                return;
            }
            _producto = producto;
            label38ProductoNombre.Text = _producto.Nombre;
            labelProductoNumero.Text = _producto.Id.ToString();
            textBox20Factura.Visible = label38ProductoNombre.Text.Contains("actura");
            label40ProductoSubtotal.Text = _cantidad > 0 ? (_producto.Precio * _cantidad).ToString() : "";   // subtotal
        }
        private async void TextBox2_TextChanged(object sender, EventArgs ev)  // producto por nombre
        {
            _producto = null;
            label40ProductoSubtotal.Text = "";
            string nombreDeProducto = textBox2ProductoNombreBusqueda.Text;
            if (nombreDeProducto == "")
            {
                label38ProductoNombre.Text = "";
                labelProductoNumero.Text = "";
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
                label38ProductoNombre.Text = "No encontrado";
                labelProductoNumero.Text = "";
                return;
            }
            _producto = producto;
            label38ProductoNombre.Text = _producto.Nombre;
            labelProductoNumero.Text = _producto.Id.ToString();
            textBox20Factura.Visible = label38ProductoNombre.Text.Contains("actura");
            label40ProductoSubtotal.Text = _cantidad > 0 ? (_producto.Precio * _cantidad).ToString() : "";   // subtotal
        }
        private async void TextBox17_TextChanged(object sender, EventArgs ev)  // cantidad
        {
            label40ProductoSubtotal.Text = "";
            string numeroDeProducto = labelProductoNumero.Text;
            if (numeroDeProducto == "" || !int.TryParse(textBox17ProductoCantidad.Text, out int cantidad))
            {
                return;
            }
            _cantidad = cantidad;
            label40ProductoSubtotal.Text = _producto != null ? (_producto.Precio * _cantidad).ToString() : "";   // subtotal
        }
        private async void AnyadirProdVendidos_Click(object sender, EventArgs ev)
        {
            if (_cliente == null || _producto == null || _cantidad == 0)
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
                if (prodVendido.ProductoId == _producto.Id)
                {
                    exists = true;
                    prodVendido.Cantidad += _cantidad;
                }
            }
            if (!exists)
            {
                var nuevoPV = new ProdVendido
                {
                    Cantidad = _cantidad,
                    Descripcion = label38ProductoNombre.Text,
                    Precio = _producto.Precio,
                    ProductoId = _producto.Id,
                    Producto = _producto
                };
                if (_producto.Tipo == TipoProducto.Saldo.ToString() && _producto.SubTipo != TipoSaldo.SaldoPendiente.ToString())
                {
                    if (ProductoServices.IsNegativePrice(_producto))
                    {
                        nuevoPV.Precio = _producto.Precio * -1;
                    }
                    else if (_producto.SubTipo == TipoSaldo.AFacturar.ToString())
                    {
                        nuevoPV.Descripcion = label38ProductoNombre.Text + textBox20Factura.Text;
                    }
                }
                _lstProdVendidosAAgregar.Add(nuevoPV);
                _lstProductosAAgregar.Add(_producto);
            }
            decimal impTotal = 0;
            foreach (ProdVendido prodVendido in _lstProdVendidosAAgregar)
            {
                impTotal += prodVendido.Precio * prodVendido.Cantidad;
            }
            _producto = null;
            _cantidad = 0;
            label42ImporteTotal.Text = impTotal.ToString();
            textBox20Factura.Text = "";
            textBox20Factura.Visible = false;
            textBox16ProductoNombreBusqueda.Text = "";
            textBox17ProductoCantidad.Text = "";
            textBox2ProductoNombreBusqueda.Text = "";
            label38ProductoNombre.Text = "";
            label40ProductoSubtotal.Text = "";
            labelProductoNumero.Text = "";
            ActualizarGrid();
        }
    }
}
