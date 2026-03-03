using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Enums;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormProductos : Form
    {
        private async void TextBox21_TextChanged(object sender, EventArgs ev)  // por id
        {
            string numeroDeProducto = textBox21.Text;
            if (numeroDeProducto == "")
            {
                label46.Text = "";
                button22.Enabled = false;
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
                label46.Text = "No encontrado";
                button22.Enabled = false;
                return;
            }
            label46.Text = producto.Nombre;
            button22.Enabled = true;
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)
        {
            string nombreDeProducto = textBox1.Text;
            if (nombreDeProducto == "")
            {
                label46.Text = "";
                button22.Enabled = false;
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
                label46.Text = "No encontrado";
                button22.Enabled = false;
                return;
            }
            label46.Text = producto.Nombre;
            button22.Enabled = true;
        }
        private async void Eliminar_Click(object sender, EventArgs ev)
        {
            if (!cbSeguroBorrar.Checked)
            {
                MessageBox.Show("Tilde si esta seguro para borrar el producto");
                return;
            }
            string nombreDeProducto = label46.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    Producto producto = await productoServices.GetProductoPorNombreExactoAsync(nombreDeProducto);
                    productoServices.DeleteProducto(producto);
                    return await savingServices.SaveAsync();
                },
                "No se pudo eliminar el Producto",
                this
            );
            if (!logrado)
            {
                return;
            }
            button22.Enabled = false;
            textBox21.Text = "";
            textBox1.Text = "";
            label46.Text = "";
            cbSeguroBorrar.Checked = false; 
        }
    }
}
