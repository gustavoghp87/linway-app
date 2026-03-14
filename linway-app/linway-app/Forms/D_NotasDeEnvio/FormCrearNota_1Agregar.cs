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
            _cliente = cliente;
            label36.Text = cliente.Direccion;
            labelClienteId.Text = cliente.Id.ToString();
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)  // cliente por dirección
        {
            _cliente = null;
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
            _cliente = cliente;
            label36.Text = cliente.Direccion;
            labelClienteId.Text = cliente.Id.ToString();
        }
        private async void TextBox16_TextChanged(object sender, EventArgs ev)  // producto por número
        {
            _producto = null;
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
            _producto = producto;
            label38.Text = _producto.Nombre;
            labelProductoId.Text = _producto.Id.ToString();
            textBox20.Visible = label38.Text.Contains("actura");
        }
        private async void TextBox2_TextChanged(object sender, EventArgs ev)  // producto por nombre
        {
            _producto = null;
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
            _producto = producto;
            label38.Text = _producto.Nombre;
            labelProductoId.Text = _producto.Id.ToString();
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
            _cantidad = cantidad;
            if (_producto == null)
            {
                label40.Text = "";
                return;
            }
            label40.Text = (_producto.Precio * _cantidad).ToString();   // subtotal
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
                    Descripcion = label38.Text,
                    Precio = _producto.Precio,
                    ProductoId = _producto.Id
                };
                if (_producto.Tipo == TipoProducto.Saldo.ToString() && _producto.SubTipo != TipoSaldo.SaldoPendiente.ToString())
                {
                    if (ProductoServices.IsNegativePrice(_producto))
                    {
                        nuevoPV.Precio = _producto.Precio * -1;
                    }
                    else if (_producto.SubTipo == TipoSaldo.AFacturar.ToString())
                    {
                        nuevoPV.Descripcion = label38.Text + textBox20.Text;
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
            _cliente = null;
            _producto = null;
            _cantidad = 0;
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
