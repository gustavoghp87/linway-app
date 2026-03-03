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
    public partial class FormRepartos : Form
    {
        private void TodasLuAVi_ToolStripMenuItem_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
            ActualizarGrid(new List<Pedido>());
            groupBox4.Visible = true;
        }
        private async void LimpiarRepartos_Click(object sender, EventArgs ev)
        {
            var repartosALimpiar = new List<Reparto>();
            foreach (DiaReparto diaReparto in _lstDiaRepartos)
            {
                foreach (Reparto reparto in diaReparto.Reparto)
                {
                    repartosALimpiar.Add(reparto);
                }
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    orquestacionServices.CleanRepartos(repartosALimpiar);
                    return await savingServices.SaveAsync();
                },
                "No se pudieron limpiar los Repartos",
                this
            );
            if (!logrado)
            {
                MessageBox.Show("Algo falló al limpiar Repartos");
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
        private void Button7_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        // 2. limpiar los repartos de un dia
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
        private async void Button9_Click(object sender, EventArgs ev)
        {
            string dia = comboBox6.Text;
            if (dia == "")
            {
                MessageBox.Show("Debe seleccionar un día");
                return;
            }
            await Actualizar();
            List<Reparto> lstRepartos = _lstDiaRepartos?.Find(x => x.Dia == dia)?.Reparto?.ToList();
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    orquestacionServices.CleanRepartos(lstRepartos);
                    return await savingServices.SaveAsync();
                },
                "No se pudieron limpiar los Repartos",
                this
            );
            if (!logrado)
            {
                MessageBox.Show("Algo falló al limpiar Repartos");
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
            string dia = comboBox8.SelectedItem.ToString();
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    return await orquestacionServices.GetRepartosPorDiaAsync(dia);
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
        private async void Button11_Click(object sender, EventArgs ev)
        {
            string dia = comboBox8.Text;
            string nombre = comboBox7.Text;
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
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    Reparto reparto = await orquestacionServices.GetRepartoPorDiaYNombreAsync(dia, nombre);
                    if (reparto == null)
                    {
                        throw new Exception("No se pudo encontrar el Reparto");
                    }
                    orquestacionServices.CleanRepartos(new List<Reparto>() { reparto });
                    bool logrado = await savingServices.SaveAsync();
                    if (!logrado)
                    {
                        MessageBox.Show("No habían cambios para hacer");
                        return null;
                    }
                    return reparto;
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
        private async void Button18_Click(object sender, EventArgs ev)
        {
            await ReCargarHDR(comboBox1.Text, comboBox2.Text);
            string direccion = label36.Text;
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.Equals(direccion));
            if (pedido == null)
            {
                MessageBox.Show("Verifique que los datos sean correctos");
                return;
            }
            var prodVendidosAEditar = new List<ProdVendido>();
            pedido.ProdVendidos.ToList().ForEach(prodVendido =>
            {
                prodVendido.PedidoId = null;
                prodVendidosAEditar.Add(prodVendido);
            });
            Reparto reparto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    //
                    prodVendidoServices.EditProdVendidos(prodVendidosAEditar);
                    pedidoServices.CleanPedidos(new List<Pedido>() { pedido });
                    await savingServices.SaveAsync();
                    return await repartoServices.GetRepartoPorIdAsync(pedido.RepartoId);
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
        }
    }
}
