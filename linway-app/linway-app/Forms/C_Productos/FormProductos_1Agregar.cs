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
        private string _tipoAgregar = TipoProducto.Líquido.ToString();
        private string _subTipoAgregar = "";
        private void LimpiarAgregar_Click(object sender, EventArgs ev)
        {
            radioButton1AgregarLiquido.Checked = false;
            radioButton2Agregar.Checked = false;
            radioButton3.Checked = false;
            radioButton4AgregarSaldo.Checked = false;
            textBox6AgregarNombre.Text = "";
            textBox7AgregarPrecio.Text = "";
            //
            comboBox1.Visible = false;
        }
        private void SeleccionarTipo_CheckedChanged(object sender, EventArgs ev)
        {
            var elegido = (RadioButton)sender;
            switch (elegido.Text)
            {
                case "Líquido":
                    comboBox1.Visible = true;
                    _tipoAgregar = TipoProducto.Líquido.ToString();
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoLiquido));
                    break;
                case "Polvo":
                    comboBox1.Visible = true;
                    _tipoAgregar = TipoProducto.Polvo.ToString();
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoPolvo));
                    break;
                case "Unidad":
                    comboBox1.Visible = false;
                    _tipoAgregar = TipoProducto.Unidad.ToString();
                    comboBox1.DataSource = null;
                    break;
                case "Saldo":
                    comboBox1.Visible = true;
                    _tipoAgregar = TipoProducto.Saldo.ToString();
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoSaldo));
                    break;
            }
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs ev)
        {
            _subTipoAgregar = comboBox1.SelectedItem?.ToString() ?? "";
        }
        private bool TodoOKagregarP()
        {
            return textBox6AgregarNombre.Text != "" && textBox7AgregarPrecio.Text != ""
                && (radioButton1AgregarLiquido.Checked || radioButton2Agregar.Checked || radioButton3.Checked || radioButton4AgregarSaldo.Checked);
        }
        private async void AgregarProducto_Click(object sender, EventArgs ev)
        {
            if (!TodoOKagregarP())
            {
                MessageBox.Show("Verifique los campos.");
                return;
            }
            if (!decimal.TryParse(textBox7AgregarPrecio.Text, out decimal precio) || precio <= 0)
            {
                return;
            }
            var nuevoProducto = new Producto
            {
                Nombre = textBox6AgregarNombre.Text,
                Precio = precio,
                Tipo = _tipoAgregar.ToString()
            };
            if (_subTipoAgregar != "")
            {
                nuevoProducto.SubTipo = _subTipoAgregar;
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var productoServices = sp.GetRequiredService<IProductoServices>();
                    productoServices.AddProducto(nuevoProducto);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo agregar Producto",
                this
            );
            if (!logrado)
            {
                return;
            }
            limpiarBtn.PerformClick();
        }
    }
}
