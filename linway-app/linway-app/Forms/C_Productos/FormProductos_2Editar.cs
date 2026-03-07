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
        private void LimpiarEditar_Click(object sender, EventArgs ev)
        {
            label19.Text = "";
            textBox2.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox21.Text = "";
            //
            comboBox2.Visible = false;
            comboBox3.Visible = false;
        }
        private void CargarDatosAModificar(Producto producto)
        {
            if (producto == null)
            {
                label19.Text = "No encontrado";
                textBox9.Text = "";
                _subTipoMod = "";
                comboBox3.Visible = false;
                comboBox2.Visible = false;
                return;
            }
            _liberado = false;
            _liberado2 = false;
            label19.Text = producto.Nombre;
            textBox9.Text = producto.Precio.ToString();
            _tipoMod = producto.Tipo;
            if (producto.SubTipo != null)
            {
                _subTipoMod = producto.SubTipo;
                comboBox2.Visible = true;
            }
            else
            {
                _subTipoMod = "";
            }
            comboBox3.Visible = true;
            comboBox3.DataSource = Enum.GetValues(typeof(TipoProducto));
        }
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var tipoElegido = (ComboBox)sender;
            if (_tipoMod == null)
            {
                return;
            }
            if (_tipoMod != "" && !_liberado)
            {
                tipoElegido.Text = _tipoMod.ToString();
                _tipoMod = "";
            }
            switch (tipoElegido.Text)
            {
                case "Líquido":
                    _tipoMod = TipoProducto.Líquido.ToString();
                    comboBox2.Visible = true;
                    comboBox2.DataSource = Enum.GetValues(typeof(TipoLiquido));
                    break;
                case "Polvo":
                    _tipoMod = TipoProducto.Polvo.ToString();
                    comboBox2.Visible = true;
                    comboBox2.DataSource = Enum.GetValues(typeof(TipoPolvo));
                    break;
                case "Unidad":
                    _tipoMod = TipoProducto.Unidad.ToString();
                    comboBox2.Visible = false;
                    comboBox2.DataSource = null;
                    break;
                case "Saldo":
                    _tipoMod = TipoProducto.Saldo.ToString();
                    comboBox2.Visible = true;
                    comboBox2.DataSource = Enum.GetValues(typeof(TipoSaldo));
                    break;
            }
            comboBox2.Text = _subTipoMod;
            _liberado = true;
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs ev)
        {
            if (_subTipoMod == null)
            {
                return;
            }
            var tipoElegido = (ComboBox)sender;
            if (_subTipoMod != "" && !_liberado2)
            {
                tipoElegido.Text = _subTipoMod;
                _liberado2 = true;
            }
        }
        private async void TextBox8_TextChanged(object sender, EventArgs ev)   // buscar por id
        {
            string numeroDeProducto = textBox8.Text;
            if (numeroDeProducto == "")
            {
                label19.Text = "";
                textBox9.Text = "";
                _tipoMod = "";
                _subTipoMod = "";
                comboBox3.Visible = false;
                comboBox2.Visible = false;
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
            CargarDatosAModificar(producto);
        }
        private async void TextBox2_TextChanged(object sender, EventArgs ev)  // buscar por nombre
        {
            string nombreDeProducto = textBox2.Text;
            if (nombreDeProducto == "")
            {
                label19.Text = "";
                textBox9.Text = "";
                _tipoMod = "";
                _subTipoMod = "";
                comboBox3.Visible = false;
                comboBox2.Visible = false;
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
            CargarDatosAModificar(producto);
        }
        bool TodoOKmodificarP()
        {
            bool subtipoVisibleButEmpty = comboBox2.Visible && comboBox2.Text == "";
            return label19.Text != "No encontrado" && label19.Text != "" && textBox9.Text != "" && !subtipoVisibleButEmpty;
        }
        private async void EditarProducto_Click(object sender, EventArgs ev)
        {
            if (!TodoOKmodificarP())
            {
                MessageBox.Show("Verifique que se hayan llenado los campos correctamente");
                return;
            }
            if (!decimal.TryParse(textBox9.Text, out decimal precio))
            {
                return;
            }
            string tipo = comboBox3.Text;
            string subtipo = comboBox2.Visible && comboBox2.Text != "" ? comboBox2.Text : "";
            string nombreDeProducto = label19.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    Producto producto = await productoServices.GetProductoPorNombreExactoAsync(nombreDeProducto);
                    if (producto == null)
                    {
                        return false;  // nunca debería pasar
                    }
                    
                    producto.Precio = precio;
                    producto.Tipo = tipo;
                    producto.SubTipo = subtipo;
                    productoServices.EditProducto(producto);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo editar el Producto",
                this
            );
            if (!logrado)
            {
                return;
            }
            button6.PerformClick();
        }
    }
}
