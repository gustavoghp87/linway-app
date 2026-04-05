using linway_app.PresentationHelpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
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
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    var prodVendidosDelReparto = new List<ProdVendido>();
                    foreach (Pedido pedido in reparto.Pedidos)
                    {
                        foreach (ProdVendido prodVendido in pedido.ProdVendidos)
                        {
                            prodVendido.PedidoId = null;
                        }
                        prodVendidosDelReparto.AddRange(pedido.ProdVendidos);
                    }
                    servicesContext.ProdVendidoServices.EditOrDeleteMany(prodVendidosDelReparto);
                    //
                    servicesContext.PedidoServices.DeleteMany(reparto.Pedidos);
                    //
                    servicesContext.RepartoServices.Delete(reparto);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo realizar",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            comboBox1ListaDias.SelectedIndex = 0;
            ActualizarCombobox1();
        }
    }
}
