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
        private void TodasLuAVi_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox4.Visible = true;
        }
        private async void LimpiarRepartos_Click(object sender, EventArgs ev)  // limpiar todos los repartos
        {
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    List<Reparto> repartosALimpiar = _lstDiaRepartos.SelectMany(x => x.Reparto).ToList();
                    List<Pedido> pedidosALimpiar = repartosALimpiar.SelectMany(x => x.Pedidos).ToList();
                    List<ProdVendido> prodVendidosALimpiar = pedidosALimpiar.SelectMany(x => x.ProdVendidos).ToList();
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosALimpiar);
                    foreach (Reparto reparto in repartosALimpiar)
                    {
                        foreach (Pedido pedido in reparto.Pedidos)
                        {
                            PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, false);
                        }
                        RepartoServices.ActualizarCantidadesDeReparto(reparto);
                    }
                    repartoServices.EditRepartos(repartosALimpiar);
                    pedidoServices.EditPedidos(pedidosALimpiar);
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
        private void Button7_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        // 2. limpiar los repartos de un día
        private void DiaEspecífico_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox5.Visible = true;
        }
        private void Button8_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        private async void Button9_Click(object sender, EventArgs ev)  // limpiar los repartos de un dia
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
                    List<Reparto> repartosALimpiar = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Reparto.ToList();
                    List<Pedido> pedidosALimpiar = repartosALimpiar.SelectMany(x => x.Pedidos).ToList();
                    List<ProdVendido> prodVendidosALimpiar = pedidosALimpiar.SelectMany(x => x.ProdVendidos).ToList();
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosALimpiar);
                    foreach (var reparto in repartosALimpiar)
                    {
                        RepartoServices.ActualizarCantidadesDeReparto(reparto);
                    }
                    repartoServices.EditRepartos(repartosALimpiar);
                    foreach (Pedido pedido in pedidosALimpiar)
                    {
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, false);
                    }
                    pedidoServices.EditPedidos(pedidosALimpiar);
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
        private void RepartoSeleccionado_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox6.Visible = true;
        }
        private async void ComboBox8_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string diaReparto = comboBox8.SelectedItem.ToString();
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    return lstDiasRep.Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList();
                },
                "No se pudieron buscar los Repartos por Día",
                null
            );
            if (repartos == null)
            {
                return;
            }
            comboBox7.DataSource = repartos;
            comboBox7.DisplayMember = "Nombre";
            comboBox7.ValueMember = "Nombre";
        }
        private async void Button11_Click(object sender, EventArgs ev)  // limpiar un reparto
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
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    Reparto repartoALimpiar = lstDiasRep
                        .Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList()
                        .Find(x => x.Nombre == nombreReparto && x.Estado != null && x.Estado != "Eliminado") ?? throw new Exception("No se pudo encontrar el Reparto");
                    List<Pedido> pedidosALimpiar = repartoALimpiar.Pedidos.ToList();
                    List<ProdVendido> prodVendidosALimpiar = pedidosALimpiar.SelectMany(x => x.ProdVendidos).ToList();
                    foreach (ProdVendido prodVendido in prodVendidosALimpiar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosALimpiar);
                    RepartoServices.ActualizarCantidadesDeReparto(repartoALimpiar);
                    repartoServices.EditReparto(repartoALimpiar);
                    foreach (Pedido pedido in pedidosALimpiar)
                    {
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, false);
                    }
                    pedidoServices.EditPedidos(pedidosALimpiar);
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
            comboBox1.SelectedIndex = comboBox8.SelectedIndex;
            comboBox2.SelectedIndex = comboBox7.SelectedIndex;
            VerDatos(reparto);
            LimpiarPantalla();
            await Actualizar();
        }
        // 4. limpiar un pedido
        private void Destino_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            //LimpiarPantalla();  // comentado
            //ActualizarGrid(new List<Pedido>());  // comentado
            groupBox9.Visible = true;
            label39.Text = "Día " + comboBox1.Text + " -> Reparto: " + comboBox2.Text;
        }
        private void TextBox7_TextChanged(object sender, EventArgs ev)
        {
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.ToLower().Contains(textBox7.Text.ToLower()));
            label36.Text = pedido != null ? pedido.Direccion : "No encontrado";
        }
        private async void Button18_Click(object sender, EventArgs ev)  // limpiar un pedido
        {
            await ReCargarHDR(comboBox1.Text, comboBox2.Text);
            string direccion = label36.Text;
            Reparto reparto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();

                    // no hay que quitar las referencias de los productos vendidos a los pedidos?
                    Pedido pedidoAEditar = _lstPedidos.Find(x => x.Direccion.Equals(direccion));
                    if (pedidoAEditar == null)
                    {
                        MessageBox.Show("Verifique que los datos sean correctos");
                        return null;
                    }
                    var prodVendidosAEditar = pedidoAEditar.ProdVendidos;
                    foreach (var prodVendido in prodVendidosAEditar)
                    {
                        prodVendido.PedidoId = null;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosAEditar);
                    PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedidoAEditar, false);
                    pedidoServices.EditPedido(pedidoAEditar);
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
