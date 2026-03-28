using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
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
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    ventaServices.DeleteMany(_lstVentas);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudieron eliminar las Ventas",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
    }
}
