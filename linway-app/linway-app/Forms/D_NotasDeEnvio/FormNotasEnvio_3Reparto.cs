using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormNotasEnvio : Form
    {
        private NotaDeEnvio _notaDeEnvioAReparto;
        private Reparto _reparto;
        private void Button7_Click(object sender, EventArgs ev)     // limpiar
        {
            _notaDeEnvioAReparto = null;
            _reparto = null;
            comboBox5EnviarAReparto_Reparto.Text = "";
            comboBox5EnviarAReparto_Reparto.DataSource = null;
            comboBox4EnviarAReparto_Dia.Text = "";
            textBox6.Text = "";
            button6.Enabled = false;
            label16.Text = "";
        }
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs ev)
        {
            button6.Enabled = label16.Text != "" && label16.Text != "No encontrado";
            string diaReparto = comboBox4EnviarAReparto_Dia.Text;
            if (diaReparto == "")
            {
                return;
            }
            List<Reparto> repartos = _lstDiaRepartos.Find(x => x.Dia == diaReparto).Repartos.ToList();
            if (repartos == null || repartos.Count == 0)
            {
                comboBox5EnviarAReparto_Reparto.DataSource = null;
                comboBox5EnviarAReparto_Reparto.Text = "";
                return;
            }
            comboBox5EnviarAReparto_Reparto.DataSource = repartos;
            comboBox5EnviarAReparto_Reparto.SelectedIndex = repartos.Count > 0 ? 0 : -1;
            comboBox5EnviarAReparto_Reparto.DisplayMember = "Nombre";
            comboBox5EnviarAReparto_Reparto.ValueMember = "Nombre";
        }
        private void ComboBox5_SelectedIndexChanged(object sender, EventArgs ev)
        {
            var repartoSeleccionado = comboBox5EnviarAReparto_Reparto.SelectedItem;
            _reparto = (Reparto)repartoSeleccionado;
        }
        private void TextBox6_TextChanged(object sender, EventArgs ev)    // búsqueda por id de la nota de envío para agregar a pedido
        {
            _notaDeEnvioAReparto = null;
            string numeroDeNota = textBox6.Text;
            if (numeroDeNota == "")
            {
                label16.Text = "";
                label100Reparto.Text = "";
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
            _notaDeEnvioAReparto = nota;
            label16.Text = nota.Cliente.Direccion;
            button6.Enabled = comboBox5EnviarAReparto_Reparto.Text != "";
            Pedido pedido = nota.ProdVendidos.FirstOrDefault(pv => pv.PedidoId != null)?.Pedido;
            label100Reparto.Text = pedido != null ? $"Reparto {pedido.Reparto.DiaReparto.Dia} {pedido.Reparto.Nombre}" : "En ningún Reparto";
        }
        private async void AgregarPedidoDesdeNota_Click(object sender, EventArgs ev)
        {
            if (_notaDeEnvioAReparto == null || _reparto == null)
            {
                return;
            }
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var servicesContext = ServiceContext.Get(sp);
                    List<ProdVendido> prodVendidos = await servicesContext.ProdVendidoServices.GetAllAsync();
                    Pedido pedidoEnElQueEsta = prodVendidos.FirstOrDefault(pv => pv.NotaDeEnvioId == _notaDeEnvioAReparto.Id)?.Pedido;
                    Pedido pedidoAlQueQuiereIr = _reparto.Pedidos.FirstOrDefault(x => x.ClienteId == _notaDeEnvioAReparto.ClienteId);
                    if (pedidoEnElQueEsta != null && pedidoAlQueQuiereIr != null && pedidoEnElQueEsta.Id == pedidoAlQueQuiereIr.Id)
                    {
                        MessageBox.Show("Esta Nota de Envío ya está en este Reparto");
                        return false;
                    }
                    if (pedidoAlQueQuiereIr == null)
                    {
                        Cliente cliente = await servicesContext.ClienteServices.GetPorIdAsync(_notaDeEnvioAReparto.ClienteId);
                        pedidoAlQueQuiereIr = PedidoServices.GetNuevoPedido(cliente, _reparto);
                    }
                    // prodVendidos
                    List<ProdVendido> prodVendidosDeLaNota = prodVendidos.Where(x => x.NotaDeEnvioId == _notaDeEnvioAReparto.Id).ToList();
                    foreach (ProdVendido prodVendido in prodVendidosDeLaNota)
                    {
                        prodVendido.Pedido = pedidoAlQueQuiereIr;
                    }
                    servicesContext.ProdVendidoServices.EditMany(prodVendidosDeLaNota);
                    // pedido
                    if (pedidoAlQueQuiereIr.Id == 0)
                    {
                        pedidoAlQueQuiereIr.ProdVendidos = prodVendidosDeLaNota;
                    }
                    else
                    {
                        foreach (var pv in prodVendidosDeLaNota)
                        {
                            pedidoAlQueQuiereIr.ProdVendidos.Add(pv);
                        }
                        pedidoAlQueQuiereIr.Entregar = 1;
                        servicesContext.PedidoServices.Edit(pedidoAlQueQuiereIr);
                    }
                    // guardado
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo agregar a reparto",
                this
            );
            if (!logrado)
            {
                return;
            }
            _reparto = null;
            _notaDeEnvioAReparto = null;
            button6.Enabled = false;
            label16.Text = "";
            label100Reparto.Text = "";
            comboBox5EnviarAReparto_Reparto.Text = "";
            comboBox4EnviarAReparto_Dia.Text = "";
            textBox6.Text = "";
        }
    }
}
