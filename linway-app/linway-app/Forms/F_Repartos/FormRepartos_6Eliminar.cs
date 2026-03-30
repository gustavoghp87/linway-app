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
        private void ComboBox10_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string diaReparto = comboBox10Repartos_Dia.SelectedItem.ToString();
            List<Reparto> repartos = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Repartos.ToList();
            if (repartos == null)
            {
                return;
            }
            comboBox9Repartos_Nombre.DataSource = repartos;
            comboBox9Repartos_Nombre.DisplayMember = "Nombre";
            comboBox9Repartos_Nombre.ValueMember = "Nombre";
            label32EliminarPedido_Direccion.Text = "";
            textBox5EliminarPedido_Direccion.Text = "";
        }
        private async void ComboBox9_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var reparto = comboBox9Repartos_Nombre.SelectedItem as Reparto;
            if (reparto == null)
            {
                return;
            }
            _lstPedidos = reparto.Pedidos.ToList();
            label32EliminarPedido_Direccion.Text = "";
            textBox5EliminarPedido_Direccion.Text = "";
        }
        private void TextBox5_TextChanged(object sender, EventArgs ev)  // pedido por dirección del cliente
        {
            if (_lstPedidos == null || _lstPedidos.Count == 0)
            {
                label32EliminarPedido_Direccion.Text = "Reparto vacío";
                _pedidoAEliminar = null;
                return;
            }
            string direccion = textBox5EliminarPedido_Direccion.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(direccion))
            {
                label32EliminarPedido_Direccion.Text = "";
                _pedidoAEliminar = null;
                return;
            }
            Pedido pedido = _lstPedidos.Find(x => x.Direccion.Trim().ToLower().Contains(direccion));
            if (pedido == null)
            {
                label32EliminarPedido_Direccion.Text = "No encontrado";
                _pedidoAEliminar = null;
                return;
            }
            label32EliminarPedido_Direccion.Text = pedido.Direccion;
            _pedidoAEliminar = pedido;
        }
        private void Button15_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
        private async void Button16_Click(object sender, EventArgs ev)
        {
            await ReCargarHDR(comboBox10Repartos_Dia.Text, comboBox9Repartos_Nombre.Text);
            if (_pedidoAEliminar == null)
            {
                return;
            }
            bool logrado = await EliminarPedidoAsync(_pedidoAEliminar);
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
    }
}
