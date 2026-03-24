using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    //
                    List<Reparto> repartosALimpiar = _lstDiaRepartos.SelectMany(x => x.Reparto).ToList();
                    List<ProdVendido> prodVendidosALimpiar = repartosALimpiar.SelectMany(x => x.Pedidos).SelectMany(x => x.ProdVendidos).ToList();
                    //
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosALimpiar);
                    //
                    foreach (Reparto reparto in repartosALimpiar)
                    {
                        foreach (Pedido pedido in reparto.Pedidos)
                        {
                            pedido.ProdVendidos = new List<ProdVendido>();
                            PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, false);
                        }
                        RepartoServices.ActualizarCantidadesDeReparto(reparto);
                    }
                    pedidoServices.EditPedidos(repartosALimpiar.SelectMany(x => x.Pedidos).ToList());
                    repartoServices.EditRepartos(repartosALimpiar);
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    //
                    List<Reparto> repartosALimpiar = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Reparto.ToList();
                    List<ProdVendido> prodVendidosALimpiar = repartosALimpiar.SelectMany(x => x.Pedidos).SelectMany(x => x.ProdVendidos).ToList();
                    //
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosALimpiar);
                    //
                    foreach (var reparto in repartosALimpiar)
                    {
                        foreach (Pedido pedido in reparto.Pedidos)
                        {
                            pedido.ProdVendidos = new List<ProdVendido>();
                            PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, false);
                        }
                        RepartoServices.ActualizarCantidadesDeReparto(reparto);
                    }
                    pedidoServices.EditPedidos(repartosALimpiar.SelectMany(x => x.Pedidos).ToList());
                    repartoServices.EditRepartos(repartosALimpiar);
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
            comboBox7.DataSource = _lstDiaRepartos.Find(x => x.Dia == diaReparto && x.Estado != "Eliminado").Reparto.ToList(); ;
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
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    //
                    Reparto repartoALimpiar = _lstDiaRepartos
                        .Find(x => x.Dia == diaReparto && x.Estado != "Eliminado").Reparto.ToList()
                        .Find(x => x.Nombre == nombreReparto && x.Estado != "Eliminado");
                    List<ProdVendido> prodVendidosALimpiar = repartoALimpiar.Pedidos.SelectMany(x => x.ProdVendidos).ToList();
                    //
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosALimpiar);
                    //
                    foreach (Pedido pedido in repartoALimpiar.Pedidos)
                    {
                        pedido.ProdVendidos = new List<ProdVendido>();
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, false);
                    }
                    RepartoServices.ActualizarCantidadesDeReparto(repartoALimpiar);
                    pedidoServices.EditPedidos(repartoALimpiar.Pedidos);
                    repartoServices.EditReparto(repartoALimpiar);
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
            comboBox1ListaDia.SelectedIndex = comboBox8.SelectedIndex;
            comboBox2.SelectedIndex = comboBox7.SelectedIndex;
            VerDatos(reparto);
            LimpiarPantalla();
            await Actualizar();
        }
        // 4. limpiar un pedido
        private void TextBox7_TextChanged(object sender, EventArgs ev)
        {
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.ToLower().Contains(textBox7.Text.ToLower()));
            label36.Text = pedido != null ? pedido.Direccion : "No encontrado";
        }
        private async void Button18_Click(object sender, EventArgs ev)  // limpiar un pedido
        {
            await ReCargarHDR(comboBox1ListaDia.Text, comboBox2.Text);
            string direccion = label36.Text;
            Reparto reparto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    //
                    Pedido pedidoAEditar = _lstPedidos.Find(x => x.Direccion.Equals(direccion));
                    Reparto repartoAEditar = pedidoAEditar.Reparto;
                    List<ProdVendido> prodVendidosALimpiar = pedidoAEditar.ProdVendidos.ToList();
                    //
                    foreach (var prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosALimpiar);
                    //
                    foreach (var pedido in repartoAEditar.Pedidos)
                    {
                        if (pedido.Id == pedidoAEditar.Id)
                        {
                            pedido.ProdVendidos = new List<ProdVendido>();
                            PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, false);
                            pedidoServices.EditPedido(pedido);
                        }
                    }
                    RepartoServices.ActualizarCantidadesDeReparto(repartoAEditar);
                    repartoServices.EditReparto(repartoAEditar);
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                        return null;
                    }
                    return await repartoServices.GetRepartoPorIdAsync(pedidoAEditar.RepartoId);
                },
                "No se pudo realizar",
                this
            );
            if (reparto == null)
            {
                return;
            }
            VerDatos(reparto);
            LimpiarPantalla();
            await Actualizar();
            await ActualizarCombobox1();
            await UpdateGrid();
        }
    }
}
