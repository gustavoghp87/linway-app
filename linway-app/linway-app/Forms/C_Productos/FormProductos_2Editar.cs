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
        private bool _liberado = false;
        private bool _liberado2 = false;
        private string _tipoMod = "";
        private string _subTipoMod = "";
        private Producto _productoAEditar;
        private void LimpiarEditar_Click(object sender, EventArgs ev)
        {
            _productoAEditar = null;
            label19.Text = "";
            textBox2.Text = "";
            textBox8.Text = "";
            textBox9PrecioProdNuevo.Text = "";
            textBox21EliminarProductoNumero.Text = "";
            //
            comboBox2.Visible = false;
            comboBox3.Visible = false;
        }
        private void CargarDatosAModificar(Producto producto)
        {
            if (producto == null)
            {
                label19.Text = "No encontrado";
                textBox9PrecioProdNuevo.Text = "";
                _subTipoMod = "";
                comboBox3.Visible = false;
                comboBox2.Visible = false;
                return;
            }
            _liberado = false;
            _liberado2 = false;
            label19.Text = producto.Nombre;
            textBox9PrecioProdNuevo.Text = producto.Precio.ToString();
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
            _productoAEditar = null;
            string numeroDeProducto = textBox8.Text;
            if (numeroDeProducto == "")
            {
                label19.Text = "";
                textBox9PrecioProdNuevo.Text = "";
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
                    return await productoServices.GetPorIdAsync(productoId);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto != null)
            {
                _productoAEditar = producto;
            }
            CargarDatosAModificar(producto);
        }
        private async void TextBox2_TextChanged(object sender, EventArgs ev)  // buscar por nombre
        {
            _productoAEditar = null;
            string nombreDeProducto = textBox2.Text;
            if (nombreDeProducto == "")
            {
                label19.Text = "";
                textBox9PrecioProdNuevo.Text = "";
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
                    return await productoServices.GetPorNombreAsync(nombreDeProducto);
                },
                "No se pudo buscar el Producto",
                null
            );
            if (producto != null)
            {
                _productoAEditar = producto;
            }
            CargarDatosAModificar(producto);
        }
        bool TodoOKmodificarP()
        {
            bool subtipoVisibleButEmpty = comboBox2.Visible && comboBox2.Text == "";
            return _productoAEditar != null && textBox9PrecioProdNuevo.Text != "" && !subtipoVisibleButEmpty;
        }
        private async void EditarProducto_Click(object sender, EventArgs ev)
        {
            if (!TodoOKmodificarP())
            {
                MessageBox.Show("Verifique que se hayan llenado los campos correctamente");
                return;
            }
            if (!decimal.TryParse(textBox9PrecioProdNuevo.Text, out decimal precio) || precio <= 0)
            {
                return;
            }
            string tipo = comboBox3.Text;
            string subtipo = comboBox2.Visible && comboBox2.Text != "" ? comboBox2.Text : "";
            _productoAEditar.Precio = precio;
            _productoAEditar.Tipo = tipo;
            _productoAEditar.SubTipo = subtipo;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    productoServices.Edit(_productoAEditar);
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
            _productoAEditar = null;
            button6.PerformClick();
        }
    }
}
