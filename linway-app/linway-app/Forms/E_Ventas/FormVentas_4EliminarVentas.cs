using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormVentas : Form
    {
        private async void ReiniciarVentas_Click(object sender, EventArgs ev)
        {
            if (!cbSeguroBorrar.Checked)
            {
                MessageBox.Show("Seleccione si esta seguro para borrar la lista de ventas");
                return;
            }
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IEliminarVentasUseCase>();
                    return await useCase.ExecuteAsync(_lstVentas);
                },
                "No se pudieron eliminar las Ventas",
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
            await Actualizar();
        }
    }
}
