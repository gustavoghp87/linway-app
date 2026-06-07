using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
{
    public partial class FormRepartos : Form  // TODO: usar dos atributos Pedido
    {
        private void TextBox3_TextChanged(object sender, EventArgs ev)
        {
            try
            {
                //var auxList = _lstPedidos.Where(x => x.Entregar == 1).ToList();  comentado
                var auxList = _lstPedidos.ToList();
                var aux = auxList.Find(x => x.Cliente.Direccion.ToLower().Contains(textBox3.Text.ToLower()));
                label30ReposicionarObjetivo.Text = aux != null ? aux.Cliente.Direccion : "No encontrado";
            }
            catch (Exception)
            {
                label30ReposicionarObjetivo.Text = "No encontrado";
            }
        }
        private void TextBox4_TextChanged(object sender, EventArgs ev)
        {
            //var auxList = _lstPedidos.Where(x => x.Entregar == 1).ToList();  comentado
            var auxList = _lstPedidos.ToList();
            var aux = auxList.Find(x => x.Cliente.Direccion.ToLower().Contains(textBox4.Text.ToLower()));
            label31ReposicionarReferencia.Text = aux != null ? aux.Cliente.Direccion : "No encontrado";
        }
        private async void Button14_Click(object sender, EventArgs ev)           //   aceptar
        {
            if (label30ReposicionarObjetivo.Text == "No encontrado" || label30ReposicionarObjetivo.Text == "" || label31ReposicionarReferencia.Text == "No encontrado" || label31ReposicionarReferencia.Text == "")
            {
                return;
            }
            string pedidoAMover = label30ReposicionarObjetivo.Text;
            string pedidoReferencia = label31ReposicionarReferencia.Text;
            string diaReparto = comboBox1ListaDias.Text;
            string nombreReparto = comboBox2ListaRepartos.Text;
            Reparto reparto = _lstDiaRepartos
                .Find(x => x.Dia == diaReparto).Repartos.ToList()
                .Find(x => x.Nombre == nombreReparto);
            Pedido pedido1 = reparto.Pedidos.ToList().Find(x => x.Cliente.Direccion == pedidoAMover);
            Pedido pedido2 = reparto.Pedidos.ToList().Find(x => x.Cliente.Direccion == pedidoReferencia);
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IReposicionarRepartoUseCase>();
                    return await useCase.ExecuteAsync(reparto, pedido1, pedido2);
                },
                "No se pudo reposicionar",
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
