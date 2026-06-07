using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
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
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var useCase = _scope.ServiceProvider.GetRequiredService<ILimpiarRepartosUseCase>();
                    return await useCase.ExecuteAsync(_lstDiaRepartos);
                },
                "No se pudieron limpiar los Repartos o no había nada para limpiar",
                this
            );
            if (resultado == null || !resultado.Success)
            {
                if (resultado?.ErrorMessage != null)
                {
                    MessageBox.Show(resultado.ErrorMessage);
                }
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
            List<Reparto> repartosALimpiar = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Repartos.ToList();
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var useCase = _scope.ServiceProvider.GetRequiredService<ILimpiarDiaRepartoUseCase>();
                    return await useCase.ExecuteAsync(repartosALimpiar);
                },
                "No se pudieron limpiar los Repartos",
                this
            );
            if (resultado == null || !resultado.Success)
            {
                if (resultado?.ErrorMessage != null)
                {
                    MessageBox.Show(resultado.ErrorMessage);
                }
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
            Reparto repartoALimpiar = _lstDiaRepartos
                .Find(x => x.Dia == diaReparto).Repartos.ToList()
                .Find(x => x.Nombre == nombreReparto);
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var useCase = _scope.ServiceProvider.GetRequiredService<ILimpiarRepartoUseCase>();
                    return await useCase.ExecuteAsync(repartoALimpiar);
                },
                "No se pudo realizar",
                this
            );
            if (resultado == null || !resultado.Success)
            {
                if (resultado?.ErrorMessage != null)
                {
                    MessageBox.Show(resultado.ErrorMessage);
                }
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
            Pedido pedidoAEditar = _lstPedidos.Find(x => x.Cliente.Direccion.Equals(direccion));
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<ILimpiarPedidoUseCase>();
                    return await useCase.ExecuteAsync(pedidoAEditar);
                },
                "No se pudo realizar",
                this
            );
            if (resultado == null || !resultado.Success)
            {
                if (resultado?.ErrorMessage != null)
                {
                    MessageBox.Show(resultado.ErrorMessage);
                }
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
    }
}
