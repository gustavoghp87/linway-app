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
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            textBox6.Text = "";
            textBox7.Text = "";
            //
            comboBox1.Visible = false;
        }
        private void SeleccionarTipo_CheckedChanged(object sender, EventArgs ev)
        {
            comboBox1.Visible = true;
            var elegido = (RadioButton)sender;
            switch (elegido.Text)
            {
                case "Líquido":
                    _tipoAgregar = TipoProducto.Líquido.ToString();
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoLiquido));
                    break;
                case "Polvo":
                    _tipoAgregar = TipoProducto.Polvo.ToString();
                    comboBox1.Visible = true;
                    comboBox1.DataSource = Enum.GetValues(typeof(TipoPolvo));
                    break;
                case "Unidad":
                    _tipoAgregar = TipoProducto.Unidad.ToString();
                    comboBox1.Visible = false;
                    comboBox1.DataSource = null;
                    break;
                case "Saldo":
                    _tipoAgregar = TipoProducto.Saldo.ToString();
                    comboBox1.Visible = true;
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
            return textBox6.Text != "" && textBox7.Text != ""
                && (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked);
        }
        private async void AgregarProducto_Click(object sender, EventArgs ev)
        {
            if (!TodoOKagregarP())
            {
                MessageBox.Show("Verifique los campos.");
                return;
            }
            if (!decimal.TryParse(textBox7.Text, out decimal precio))
            {
                return;
            }
            var nuevoProducto = new Producto
            {
                Nombre = textBox6.Text,
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
