using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormProductos : Form
    {
        private Producto _productoAEliminar;
        private async void TextBox21_TextChanged(object sender, EventArgs ev)  // por id
        {
            _productoAEliminar = null;
            cbSeguroBorrar.Checked = false;
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
                label46EliminarProductoNombre.Text = "No encontrado";
                button22Eliminar.Enabled = false;
                return;
            }
            _productoAEliminar = producto;
            label46EliminarProductoNombre.Text = producto.Nombre;
            button22Eliminar.Enabled = true;
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)  // por nombre
        {
            _productoAEliminar = null;
            cbSeguroBorrar.Checked = false;
            string nombreDeProducto = textBox1EliminarProductoNombre.Text;
            if (nombreDeProducto == "")
            {
                label46EliminarProductoNombre.Text = "";
                button22Eliminar.Enabled = false;
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
                label46EliminarProductoNombre.Text = "No encontrado";
                button22Eliminar.Enabled = false;
                return;
            }
            _productoAEliminar = producto;
            label46EliminarProductoNombre.Text = producto.Nombre;
            button22Eliminar.Enabled = true;
        }
        private async Task ValidarSiSePuedeEliminarAsync(IServiceProvider sp)
        {
            var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
            List<ProdVendido> prodVendidos = await prodVendidoServices.GetAllAsync();
            List<ProdVendido> prodVendidosDelProducto = prodVendidos.FindAll(pv => pv.ProductoId == _productoAEliminar.Id);
            if (prodVendidosDelProducto.Any(pv => pv.NotaDeEnvioId != null))
            {
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene Notas de Envío asociadas");
            }
            if (prodVendidosDelProducto.Any(pv => pv.RegistroVentaId != null))
            {
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene Registros de Venta asociados");
            }
            if (prodVendidosDelProducto.Any(pv => pv.PedidoId != null))
            {
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene Pedidos asociados");
            }
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
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    await ValidarSiSePuedeEliminarAsync(sp);
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    //
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    List<Venta> ventas = await ventaServices.GetAllAsync();
                    Venta venta = ventas.Find(v => v.ProductoId == _productoAEliminar.Id);
                    if (venta != null)
                    {
                        ventaServices.DeleteMany(new List<Venta> { venta });
                    }
                    //
                    productoServices.Delete(_productoAEliminar);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo eliminar el Producto",
                this
            );
            if (!logrado)
            {
                return;
            }
            button22Eliminar.Enabled = false;
            textBox21EliminarProductoNumero.Text = "";
            textBox1EliminarProductoNombre.Text = "";
            label46EliminarProductoNombre.Text = "";
            cbSeguroBorrar.Checked = false;
        }
    }
}
