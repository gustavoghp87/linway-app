using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private void CheckBox1_CheckedChanged(object sender, EventArgs ev)
        {
            bBorrarRegVentas.Enabled = !bBorrarRegVentas.Enabled;
        }
        private bool IntervaloCorrecto()
        {
            try
            {
                bool todoOk = false;
                long primero = long.Parse(tbDesde.Text);
                long segundo = tbHasta.Text != "" ? long.Parse(tbHasta.Text) : primero;
                todoOk = (primero <= segundo);
                if (!todoOk)
                {
                    MessageBox.Show("intervalo incorrecto.");
                }
                return todoOk;
            }
            catch
            {
                MessageBox.Show("Falta llenar algunos campos");
                return false;
            }
        }
        private bool SeEncuentraEnIntervalo(long id)
        {
            try
            {
                long primero = long.Parse(tbDesde.Text);
                long segundo = long.Parse(tbHasta.Text);
                bool seEncuentraEnIntervalo = primero <= id && segundo >= id;
                return seEncuentraEnIntervalo;
            }
            catch {
                return false;
            }
        }
        private async void BorrarRegVentas_Click(object sender, EventArgs ev)
        {
            if (!IntervaloCorrecto())
            {
                return;
            }
            var registrosABorrar = new List<RegistroVenta>();
            var prodVendidosAEditarOEliminar = new List<ProdVendido>();
            foreach (RegistroVenta registroVenta in _lstRegistros)
            {
                if (!SeEncuentraEnIntervalo(registroVenta.Id))
                {
                    continue;
                }
                registrosABorrar.Add(registroVenta);
                prodVendidosAEditarOEliminar.AddRange(registroVenta.ProdVendidos);
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    //
                    foreach (ProdVendido pv in prodVendidosAEditarOEliminar)
                    {
                        pv.RegistroVentaId = null;
                    }
                    prodVendidoServices.EditOrDelete(prodVendidosAEditarOEliminar);
                    //
                    registroVentaServices.DeleteMany(registrosABorrar);
                    //
                    await ventaServices.UpdateDesdeProdVendidosAsync(prodVendidosAEditarOEliminar, false);  // resta de las ventas
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudieron eliminar los Registros de Venta",
                this
            );
            if (!logrado)
            {
                return;
            }
            await Actualizar();
            LimpiarPantalla();
        }
    }
}
