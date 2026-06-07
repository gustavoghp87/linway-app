using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormRepartos : Form
    {
        private void Button12_Click(object sender, EventArgs e)
        {
            LimpiarPantalla();
        }
        private async void EliminarReparto_Button3_Click(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                MessageBox.Show("Debe marcar la casilla de confirmación para eliminar el reparto");
                return;
            }
            string diaReparto = comboBox1ListaDias.Text;
            string nombreReparto = comboBox2ListaRepartos.Text;
            if (diaReparto == "" || nombreReparto == "")
            {
                MessageBox.Show("Debe seleccionar un Día de Reparto y un Reparto para eliminar");
                return;
            }
            Reparto reparto = _lstDiaRepartos
                .Find(x => x.Dia == diaReparto).Repartos.ToList()
                .Find(x => x.Nombre == nombreReparto);
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IEliminarRepartoUseCase>();
                    return await useCase.ExecuteAsync(reparto);
                },
                "No se pudo realizar",
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
            LimpiarPantalla();
            comboBox1ListaDias.SelectedIndex = 0;
            ActualizarCombobox1();
        }
    }
}
