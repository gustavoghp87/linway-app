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
    public partial class FormNotasEnvio : Form
    {
        private async void ComboBox4_SelectedIndexChanged(object sender, EventArgs ev)
        {
            button6.Enabled = label16.Text != "" && label16.Text != "No encontrado";
            string dia = comboBox4.Text;
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
            if (repartos == null || repartos.Count == 0)
            {
                comboBox5.DataSource = null;
                comboBox5.Text = "";
                return;
            }
            comboBox5.DataSource = repartos;
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
        }
        private void TextBox6_TextChanged(object sender, EventArgs ev)    // búsqueda por id de la nota de envío para agregar a pedido
        {
            string numeroDeNota = textBox6.Text;
            if (numeroDeNota == "")
            {
                label16.Text = "";
                button6.Enabled = false;
                return;
            }
            if (!long.TryParse(numeroDeNota, out long notaDeEnvioId))
            {
                return;
            }
            NotaDeEnvio nota = _lstNotaDeEnvios.Find(x => x.Id == notaDeEnvioId);
            if (nota == null)
            {
                label16.Text = "No encontrado";
                button6.Enabled = false;
                return;
            }
            label16.Text = nota.Cliente.Direccion;
            button6.Enabled = comboBox5.Text != "";
        }
        private void Button7_Click(object sender, EventArgs ev)     // limpiar
        {
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }
        private async void AgregarPedidoDesdeNota_Click(object sender, EventArgs ev)
        {
            string numeroDeNota = textBox6.Text;
            if (!long.TryParse(numeroDeNota, out long notaDeEnvioId))
            {
                return;
            }
            string diaDeReparto = comboBox4.Text;
            string nombreReparto = comboBox5.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var orquestacionServices = sp.GetRequiredService<IOrquestacionServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    NotaDeEnvio notaDeEnvio = await notaDeEnvioServices.GetNotaDeEnvioPorIdAsync(notaDeEnvioId);
                    Reparto reparto = await orquestacionServices.GetRepartoPorDiaYNombreAsync(diaDeReparto, nombreReparto);
                    if (notaDeEnvio.ProdVendidos != null)
                    {
                        ProdVendido prodVendidoEnPedido = notaDeEnvio.ProdVendidos.FirstOrDefault(x => x.PedidoId != null);
                        if (prodVendidoEnPedido != null)
                        {
                            long currentPedidoId = (long)prodVendidoEnPedido.PedidoId;
                            Pedido currentPedido = await pedidoServices.GetPedido(currentPedidoId);
                            if (currentPedido != null)
                            {
                                pedidoServices.CleanPedidos(new List<Pedido>() { currentPedido });
                            }
                        }
                    }
                    Pedido pedido = await orquestacionServices.GetPedidoPorRepartoYClienteGenerarSiNoExisteAsync(reparto.Id, notaDeEnvio.ClienteId);
                    foreach (ProdVendido prodVendido in notaDeEnvio.ProdVendidos)
                    {
                        prodVendido.Pedido = pedido;
                    }
                    prodVendidoServices.EditProdVendidos(notaDeEnvio.ProdVendidos);
                    await orquestacionServices.UpdatePedidoAsync(pedido, true);
                    return await savingServices.SaveAsync();
                },
                "No se pudo agregar a reparto",
                this
            );
            if (!logrado)
            {
                return;
            }
            comboBox5.Text = "";
            comboBox4.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }
    }
}
