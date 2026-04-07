using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        // 1. limpiar todos los repartos
        private void Button7_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        private async void LimpiarRepartos_Click(object sender, EventArgs ev)
        {
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    List<Reparto> repartosALimpiar = _lstDiaRepartos.SelectMany(x => x.Repartos).ToList();
                    List<ProdVendido> prodVendidosALimpiar = repartosALimpiar.SelectMany(x => x.Pedidos).SelectMany(x => x.ProdVendidos).ToList();
                    //
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    servicesContext.ProdVendidoServices.EditMany(prodVendidosALimpiar);
                    //
                    foreach (Reparto reparto in repartosALimpiar)
                    {
                        foreach (Pedido pedido in reparto.Pedidos)
                        {
                            pedido.Entregar = 0;
                        }
                    }
                    servicesContext.PedidoServices.EditMany(repartosALimpiar.SelectMany(x => x.Pedidos).ToList());
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudieron limpiar los Repartos o no había nada para limpiar",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
        // 2. limpiar los repartos de un día
        private void Button8_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        private async void Button9_Click(object sender, EventArgs ev)
        {
            string diaReparto = comboBox6.Text;
            if (diaReparto == "")
            {
                MessageBox.Show("Debe seleccionar un día");
                return;
            }
            await Actualizar();
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    List<Reparto> repartosALimpiar = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Repartos.ToList();
                    List<ProdVendido> prodVendidosALimpiar = repartosALimpiar.SelectMany(x => x.Pedidos).SelectMany(x => x.ProdVendidos).ToList();
                    //
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    servicesContext.ProdVendidoServices.EditMany(prodVendidosALimpiar);
                    //
                    foreach (var reparto in repartosALimpiar)
                    {
                        foreach (Pedido pedido in reparto.Pedidos)
                        {
                            pedido.Entregar = 0;
                        }
                    }
                    servicesContext.PedidoServices.EditMany(repartosALimpiar.SelectMany(x => x.Pedidos).ToList());
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudieron limpiar los Repartos",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
        // 3. limpiar un reparto
        private void ComboBox8_SelectedIndexChanged(object sender, EventArgs ev)  // seleccionar
        {
            string diaReparto = comboBox8.SelectedItem.ToString();
            comboBox7.DataSource = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Repartos.ToList(); ;
            comboBox7.DisplayMember = "Nombre";
            comboBox7.ValueMember = "Nombre";
        }
        private async void Button11_Click(object sender, EventArgs ev)
        {
            string diaReparto = comboBox8.Text;
            string nombreReparto = comboBox7.Text;
            if (comboBox8.Text == "")
            {
                MessageBox.Show("Debe seleccionar un día");
                return;
            }
            Reparto reparto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    Reparto repartoALimpiar = _lstDiaRepartos
                        .Find(x => x.Dia == diaReparto).Repartos.ToList()
                        .Find(x => x.Nombre == nombreReparto);
                    List<ProdVendido> prodVendidosALimpiar = repartoALimpiar.Pedidos.SelectMany(x => x.ProdVendidos).ToList();
                    //
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    servicesContext.ProdVendidoServices.EditMany(prodVendidosALimpiar);
                    //
                    foreach (Pedido pedido in repartoALimpiar.Pedidos)
                    {
                        pedido.Entregar = 0;
                    }
                    servicesContext.PedidoServices.EditMany(repartoALimpiar.Pedidos);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                        return null;
                    }
                    return repartoALimpiar;
                },
                "No se pudo realizar",
                this
            );
            if (reparto == null)
            {
                return;
            }
            //comboBox1ListaDias.SelectedIndex = comboBox8.SelectedIndex;
            //comboBox2ListaRepartos.SelectedIndex = comboBox7.SelectedIndex;
            LimpiarPantalla();
            await Actualizar();
        }
        // 4. limpiar un pedido
        private void TextBox7_TextChanged(object sender, EventArgs ev)
        {
            Pedido pedido = _lstPedidos.Find(x => x.Cliente.Direccion.ToLower().Contains(textBox7.Text.ToLower()));
            label36.Text = pedido != null ? pedido.Cliente.Direccion : "No encontrado";
        }
        private async void Button18_Click(object sender, EventArgs ev)  // limpiar un pedido
        {
            await ReCargarHDR(comboBox1ListaDias.Text, comboBox2ListaRepartos.Text);
            string direccion = label36.Text;
            Reparto reparto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    //
                    Pedido pedidoAEditar = _lstPedidos.Find(x => x.Cliente.Direccion.Equals(direccion));
                    Reparto repartoAEditar = pedidoAEditar.Reparto;
                    List<ProdVendido> prodVendidosALimpiar = pedidoAEditar.ProdVendidos.ToList();
                    //
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    servicesContext.ProdVendidoServices.EditMany(prodVendidosALimpiar);
                    //
                    foreach (Pedido pedido in repartoAEditar.Pedidos)
                    {
                        if (pedido.Id == pedidoAEditar.Id)
                        {
                            pedido.Entregar = 0;
                        }
                    }
                    servicesContext.PedidoServices.EditMany(repartoAEditar.Pedidos);
                    //
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                        return null;
                    }
                    return await servicesContext.RepartoServices.GetPorIdAsync(pedidoAEditar.RepartoId);
                },
                "No se pudo realizar",
                this
            );
            if (reparto == null)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
    }
}
