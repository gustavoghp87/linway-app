using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormProductos : Form
    {
        private Producto _productoAEliminar;
        private void CargarEtiquetas()
        {
            label46EliminarProductoNombre.Text = $"{_productoAEliminar.Id} - {_productoAEliminar.Nombre}";
            var tieneVentas = _productoAEliminar.Ventas.Any();
            var tieneNotas = _productoAEliminar.ProdVendidos.Any(x => x.NotaDeEnvioId != null);
            var tienePedidos = _productoAEliminar.ProdVendidos.Any(x => x.PedidoId != null);
            var tieneRegistros = _productoAEliminar.ProdVendidos.Any(x => x.RegistroVentaId != null);
            if (tieneNotas || tienePedidos || tieneRegistros)
            {
                label2EliminarExplicacion.Text = "No se puede eliminar este Producto porque tiene Notas, Pedidos y/o Registros asociados. Intentar eliminar para ver cuáles son.";
            }
            else if (tieneVentas)
            {
                label2EliminarExplicacion.Text = "Al eliminarse este Producto, se van a eliminar también sus Ventas";
            }
            else
            {
                label2EliminarExplicacion.Text = "Este Producto no tiene Notas, Pedidos, Registros ni Ventas asociados.";
            }
            button22Eliminar.Enabled = true;
        }
        private async void TextBox21_TextChanged(object sender, EventArgs ev)  // producto por id
        {
            _productoAEliminar = null;
            cbSeguroBorrar.Checked = false;
            label2EliminarExplicacion.Text = "";
            string numeroDeProducto = textBox21EliminarProductoNumero.Text;
            if (numeroDeProducto == "")
            {
                label46EliminarProductoNombre.Text = "";
                button22Eliminar.Enabled = false;
                return;
            }
            if (!long.TryParse(numeroDeProducto, out long productoId))
            {
                return;
            }
            var producto = await UIExecutor.ExecuteAsync(
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
                label46EliminarProductoNombre.Text = "No encontrado";
                button22Eliminar.Enabled = false;
                return;
            }
            _productoAEliminar = producto;
            CargarEtiquetas();
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)  // producto por nombre
        {
            _productoAEliminar = null;
            cbSeguroBorrar.Checked = false;
            label2EliminarExplicacion.Text = "";
            string nombreDeProducto = textBox1EliminarProductoNombre.Text;
            if (nombreDeProducto == "")
            {
                label46EliminarProductoNombre.Text = "";
                button22Eliminar.Enabled = false;
                return;
            }
            var producto = await UIExecutor.ExecuteAsync(
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
                label46EliminarProductoNombre.Text = "No encontrado";
                button22Eliminar.Enabled = false;
                return;
            }
            _productoAEliminar = producto;
            CargarEtiquetas();
        }
        private async void Eliminar_Click(object sender, EventArgs ev)
        {
            if (_productoAEliminar == null)
            {
                MessageBox.Show("No hay un producto seleccionado para eliminar");
                return;
            }
            if (!cbSeguroBorrar.Checked)
            {
                MessageBox.Show("Tilde si esta seguro para borrar el producto");
                return;
            }
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IEliminarProductoUseCase>();
                    return await useCase.ExecuteAsync(_productoAEliminar);
                },
                "No se pudo eliminar el Producto",
                this
            );
            if (resultado == null || !resultado.Success)
            {
                if (resultado?.ErrorMessage != null)
                {
                    MessageBox.Show(resultado.ErrorMessage);
                }
                return;
            }
            button22Eliminar.Enabled = false;
            textBox21EliminarProductoNumero.Text = "";
            textBox1EliminarProductoNombre.Text = "";
            label46EliminarProductoNombre.Text = "";
            label2EliminarExplicacion.Text = "";
            cbSeguroBorrar.Checked = false;
        }
    }
}
