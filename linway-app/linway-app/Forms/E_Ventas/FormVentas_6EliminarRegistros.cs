using linway_app.PresentationHelpers;
using Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private List<RegistroVenta> _registrosAEliminar = new List<RegistroVenta>();
        private void CheckBox1_CheckedChanged(object sender, EventArgs ev)
        {
            bBorrarRegVentas.Enabled = !bBorrarRegVentas.Enabled;
        }
        private void CargarListaDeRegistrosAEliminar()
        {
            _registrosAEliminar.Clear();
            if (long.TryParse(tbDesde.Text, out long menor))
            {
                if (tbHasta.Text == "" || !long.TryParse(tbHasta.Text, out long mayor))
                {
                    mayor = menor;
                }
                _registrosAEliminar = _lstRegistros.FindAll(r => menor <= r.Id && mayor >= r.Id);
            }
            checkBox1.Text = $"Estoy seguro de borrar los registros \r\nseleccionados. Seleccionados: {_registrosAEliminar.Count}";
        }
        private void EliminarRegistrosRango_TextChanged(object sender, EventArgs ev)
        {
            CargarListaDeRegistrosAEliminar();
        }
        private async void BorrarRegVentas_Click(object sender, EventArgs ev)
        {
            CargarListaDeRegistrosAEliminar();
            if (_registrosAEliminar.Count == 0)
            {
                return;
            }
            var prodVendidosAEditarOEliminar = new List<ProdVendido>();
            foreach (RegistroVenta registroVenta in _registrosAEliminar)
            {
                prodVendidosAEditarOEliminar.AddRange(registroVenta.ProdVendidos);
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    foreach (ProdVendido pv in prodVendidosAEditarOEliminar)
                    {
                        pv.RegistroVentaId = null;
                    }
                    servicesContext.ProdVendidoServices.EditOrDeleteMany(prodVendidosAEditarOEliminar);
                    //
                    servicesContext.RegistroVentaServices.DeleteMany(_registrosAEliminar);
                    //
                    await servicesContext.VentaServices.RestarDesdeProdVendidosAsync(prodVendidosAEditarOEliminar);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
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
