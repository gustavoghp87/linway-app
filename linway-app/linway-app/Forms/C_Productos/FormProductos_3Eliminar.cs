using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using Models;
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
        private async void TextBox21_TextChanged(object sender, EventArgs ev)  // producto por id
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
                    var servicesContext = ServiceContext.Get(sp);
                    return await servicesContext.ProductoServices.GetPorIdAsync(productoId);
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
        private async void TextBox1_TextChanged(object sender, EventArgs ev)  // producto por nombre
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
                    var servicesContext = ServiceContext.Get(sp);
                    return await servicesContext.ProductoServices.GetPorNombreAsync(nombreDeProducto);
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
        private async Task ValidarSiSePuedeEliminarAsync(IProdVendidoServices prodVendidoServices)
        {
            List<ProdVendido> prodVendidos = await prodVendidoServices.GetAllAsync();
            List<ProdVendido> prodVendidosDelProducto = prodVendidos.FindAll(pv => pv.ProductoId == _productoAEliminar.Id);
            string excepcion = "";
            if (prodVendidosDelProducto.Any(pv => pv.NotaDeEnvioId != null))
            {
                excepcion += "\n\n";
                excepcion += $"No se puede eliminar el producto porque tiene las siguientes Notas de Envío asociadas: ";
                excepcion += string.Join(", ", prodVendidosDelProducto.Where(pv => pv.NotaDeEnvioId != null).Select(pv => pv.NotaDeEnvioId));
            }
            if (prodVendidosDelProducto.Any(pv => pv.PedidoId != null))
            {
                excepcion += "\n\n";
                excepcion += $"No se puede eliminar el producto porque tiene los siguientes Pedidos asociados: ";
                excepcion += string.Join(", ", prodVendidosDelProducto.Where(pv => pv.PedidoId != null).Select(pv => $"{pv.Pedido.Reparto.DiaReparto.Dia} {pv.Pedido.Reparto.Nombre}"));
            }
            if (prodVendidosDelProducto.Any(pv => pv.RegistroVentaId != null))
            {
                excepcion += "\n\n";
                excepcion += $"No se puede eliminar el producto porque tiene los siguientes Registros de Venta asociados: ";
                excepcion += string.Join(", ", prodVendidosDelProducto.Where(pv => pv.RegistroVentaId != null).Select(pv => pv.RegistroVentaId));
            }
            if (excepcion.Length > 0)
            {
                throw new InvalidOperationException(excepcion);
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
                    var servicesContext = ServiceContext.Get(sp);
                    await ValidarSiSePuedeEliminarAsync(servicesContext.ProdVendidoServices);
                    //
                    List<Venta> ventas = await servicesContext.VentaServices.GetAllAsync();
                    Venta venta = ventas.Find(v => v.ProductoId == _productoAEliminar.Id);
                    if (venta != null)
                    {
                        servicesContext.VentaServices.DeleteMany(new List<Venta> { venta });
                    }
                    //
                    servicesContext.ProductoServices.Delete(_productoAEliminar);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
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
