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
        private void TextBox3_TextChanged(object sender, EventArgs ev)
        {
            try
            {
                //var auxList = _lstPedidos.Where(x => x.Entregar == 1).ToList();  comentado
                var auxList = _lstPedidos.ToList();
                var aux = auxList.Find(x => x.Direccion.ToLower().Contains(textBox3.Text.ToLower()));
                label30.Text = aux != null ? aux.Direccion : "No encontrado";
            }
            catch (Exception)
            {
                label30.Text = "No encontrado";
            }
        }
        private void TextBox4_TextChanged(object sender, EventArgs ev)
        {
            //var auxList = _lstPedidos.Where(x => x.Entregar == 1).ToList();  comentado
            var auxList = _lstPedidos.ToList();
            var aux = auxList.Find(x => x.Direccion.ToLower().Contains(textBox4.Text.ToLower()));
            label31.Text = aux != null ? aux.Direccion : "No encontrado";
        }
        private async void Button14_Click(object sender, EventArgs ev)           //   aceptar
        {
            if (label30.Text == "No encontrado" || label30.Text == "" || label31.Text == "No encontrado" || label31.Text == "")
            {
                return;
            }
            string pedidoAMover = label30.Text;
            string pedidoReferencia = label31.Text;
            string diaReparto = comboBox1ListaDias.Text;
            string nombreReparto = comboBox2ListaRepartos.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    //
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetAllAsync();
                    Reparto reparto = lstDiasRep
                        .Find(x => x.Dia == diaReparto).Repartos.ToList()
                        .Find(x => x.Nombre == nombreReparto);
                    Pedido pedido1 = reparto.Pedidos.ToList().Find(x => x.Direccion == pedidoAMover);
                    Pedido pedido2 = reparto.Pedidos.ToList().Find(x => x.Direccion == pedidoReferencia);
                    long order1 = pedido1.Orden;
                    long order2 = pedido2.Orden;
                    if (order2 == order1)
                    {
                        return false;
                    }
                    var pedidosAEditar = new List<Pedido>();
                    if (order2 > order1)
                    {
                        //foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                        foreach (Pedido pedido in reparto.Pedidos)
                        {
                            if (pedido.Orden > order1 && pedido.Orden < order2)
                            {
                                pedido.Orden -= 1;
                                pedidosAEditar.Add(pedido);
                            }
                        }
                        pedido1.Orden = pedido2.Orden;
                        pedido2.Orden -= 1;
                        pedidosAEditar.Add(pedido1);
                        pedidosAEditar.Add(pedido2);
                    }
                    else
                    {
                        //foreach (Pedido pedido in reparto.Pedidos.Where(x => x.Entregar == 1))
                        foreach (Pedido pedido in reparto.Pedidos)
                        {
                            if (pedido.Orden < order1 && pedido.Orden > order2)
                            {
                                pedido.Orden += 1;
                                pedidosAEditar.Add(pedido);
                            }
                        }
                        pedido1.Orden = pedido2.Orden + 1;
                        pedidosAEditar.Add(pedido1);
                        pedidosAEditar.Add(pedido2);
                    }
                    pedidoServices.EditMany(pedidosAEditar);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo reposicionar",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
    }
}
