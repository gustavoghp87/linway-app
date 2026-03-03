using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private void VerRegistro_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            showing = "verReg";
            gbVerRegistro.Visible = true;
            ActualizarGrid2ProdVendidos(new List<ProdVendido>());
        }
        // Ver y deshacer una venta
        private async void TextBox1_TextChanged(object sender, EventArgs ev)
        {
            string numeroDeRegistroVenta = textBox1.Text;
            if (numeroDeRegistroVenta == "")
            {
                labelProductoN.Text = "";
                labelFecha.Text = "";
                labelTotal.Text = "";
                ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                bDeshacerVenta.Enabled = false;
                return;
            }
            if (!long.TryParse(numeroDeRegistroVenta, out long registroVentaId))
            {
                return;
            }
            RegistroVenta registroVenta = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    return await registroVentaServices.GetRegistroVentaPorIdAsync(registroVentaId);
                },
                "No se pudo buscar el Registro de Venta",
                null
            );
            if (registroVenta == null)
            {
                labelProductoN.Text = "";
                labelFecha.Text = "";
                labelTotal.Text = "";
                ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                bDeshacerVenta.Enabled = false;
                return;
            }
            labelFecha.Text = registroVenta.Fecha;
            labelProductoN.Text = registroVenta.NombreCliente;
            ActualizarGrid2ProdVendidos(registroVenta.ProdVendido.ToList());
            decimal importeTotal = 0;
            foreach (ProdVendido prodVendido in registroVenta.ProdVendido)
            {
                importeTotal += prodVendido.Precio;
            }
            labelTotal.Text = "Total: $" + importeTotal.ToString();
            bDeshacerVenta.Enabled = true;
        }
        private async void DeshacerVenta_Click(object sender, EventArgs ev)
        {
            if (!cbSeguro.Checked)
            {
                MessageBox.Show("Debe confirmar que está seguro de deshacer este registro.");
                return;
            }
            ;
            if (!long.TryParse(textBox1.Text, out long registroVentaId))
            {
                return;
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    RegistroVenta registro = await registroVentaServices.GetRegistroVentaPorIdAsync(registroVentaId);
                    await orquestacionServices.UpdateVentasDesdeProdVendidosAsync(registro.ProdVendido, false);
                    return await savingServices.SaveAsync();
                },
                "No se pudo eliminar el Registro de Venta",
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
