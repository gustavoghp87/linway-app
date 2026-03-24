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
        private Pedido _pedidoAEliminar;
        private async void ComboBox10_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string diaReparto = comboBox10.SelectedItem.ToString();
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    return lstDiasRep.Find(x => x.Dia == diaReparto && x.Estado != "Eliminado").Reparto.ToList();
                },
                "No se pudieron buscar los Repartos",
                null
            );
            if (repartos == null)
            {
                return;
            }
            comboBox9.SelectedIndexChanged -= ComboBox9_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox9.DataSource = repartos;
            comboBox9.SelectedIndexChanged += ComboBox9_SelectedIndexChanged;
            comboBox9.DisplayMember = "Nombre";
            comboBox9.ValueMember = "Nombre";
            label32.Text = "";
            textBox5.Text = "";
        }
        private async void ComboBox9_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var reparto = (Reparto)comboBox9.SelectedItem;
            reparto = await UIExecutor.ExecuteAsync(    // este paso extra evita que traiga pedidos eliminados
                _scope,
                async sp => {
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    return await repartoServices.GetRepartoPorIdAsync(reparto.Id);
                },
                "No se pudo buscar Reparto",
                null
            );
            if (reparto == null || reparto.Pedidos == null || reparto.Pedidos.Count == 0)
            {
                return;
            }
            _lstPedidos = reparto.Pedidos.ToList();
            label32.Text = "";
            textBox5.Text = "";
        }
        private void TextBox5_TextChanged(object sender, EventArgs ev)
        {
            if (_lstPedidos == null || _lstPedidos.Count == 0)
            {
                label32.Text = "Reparto vacío";
                _pedidoAEliminar = null;
                return;
            }
            string direccion = textBox5.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(direccion))
            {
                label32.Text = "";
                _pedidoAEliminar = null;
                return;
            }
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.Trim().ToLower().Contains(direccion));
            if (pedido == null)
            {
                label32.Text = "No encontrado";
                _pedidoAEliminar = null;
                return;
            }
            label32.Text = pedido.Direccion;
            _pedidoAEliminar = pedido;
        }
        private void Button15_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        private async void Button16_Click(object sender, EventArgs ev)
        {
            await ReCargarHDR(comboBox10.Text, comboBox9.Text);
            if (_pedidoAEliminar == null)
            {
                return;
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    //var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();  comentado
                    //List<ProdVendido> prodVendidos = await prodVendidoServices.GetProdVendidos();
                    //_pedidoAEliminar.ProdVendidos = prodVendidos.Where(x => x.PedidoId == _pedidoAEliminar.Id).ToList();
                    //if (_pedidoAEliminar.ProdVendidos != null && _pedidoAEliminar.ProdVendidos.Count > 0)
                    //{
                    //    foreach (ProdVendido prodVendido in _pedidoAEliminar.ProdVendidos)
                    //    {
                    //        prodVendido.PedidoId = null;  innecesario, Entity Framework ya lo hace
                    //    }
                    //    prodVendidoServices.EditProdVendidos(_pedidoAEliminar.ProdVendidos);
                    //}
                    pedidoServices.DeletePedido(_pedidoAEliminar);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
            await Actualizar();
            LimpiarPantalla();
            await ActualizarCombobox1();
            await UpdateGrid();
        }
    }
}
