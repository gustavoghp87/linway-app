using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private void ReiniciarVentas_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            groupBox7.Visible = true;
        }
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
                    ventaServices.DeleteVentas(_lstVentas);
                    return await savingServices.SaveAsync();
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
