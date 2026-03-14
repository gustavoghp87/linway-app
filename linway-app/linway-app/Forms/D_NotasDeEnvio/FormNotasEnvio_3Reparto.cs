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
    public partial class FormNotasEnvio : Form
    {
        private NotaDeEnvio _notaDeEnvioAReparto;
        private Reparto _reparto;
        private async void ComboBox4_SelectedIndexChanged(object sender, EventArgs ev)
        {
            button6.Enabled = label16.Text != "" && label16.Text != "No encontrado";
            string diaReparto = comboBox4.Text;
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
            if (repartos == null || repartos.Count == 0)
            {
                comboBox5.SelectedIndexChanged -= ComboBox5_SelectedIndexChanged;  // evita error de concurrencia de DbContext
                comboBox5.DataSource = null;
                comboBox5.Text = "";
                comboBox5.SelectedIndexChanged += ComboBox5_SelectedIndexChanged;
                return;
            }
            comboBox5.SelectedIndexChanged -= ComboBox5_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox5.DataSource = repartos;
            comboBox5.DisplayMember = "Nombre";
            comboBox5.ValueMember = "Nombre";
            comboBox5.SelectedIndexChanged += ComboBox5_SelectedIndexChanged;
        }
        private async void ComboBox5_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string nombreReparto = comboBox5.Text;
            Reparto reparto = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    List<Reparto> lstRepartos = await repartoServices.GetRepartosAsync();
                    return lstRepartos.Find(x => x.Nombre == nombreReparto && x.Estado != "Eliminado");
                },
                "No se pudieron buscar los Repartos por Nombre",
                null
            );
            _reparto = reparto;
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
            button6.Enabled = comboBox5.Text != "";
            Pedido pedido = nota.ProdVendidos.FirstOrDefault(pv => pv.PedidoId != null && pv.Pedido.Estado != "Eliminado")?.Pedido;
            label100Reparto.Text = pedido != null ? $"Reparto {pedido.Reparto.DiaReparto.Dia} {pedido.Reparto.Nombre}" : "En ningún Reparto";
        }
        private void Button7_Click(object sender, EventArgs ev)     // limpiar
        {
            _notaDeEnvioAReparto = null;
            _reparto = null;
            //
            comboBox5.SelectedIndexChanged -= ComboBox5_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox5.Text = "";
            comboBox5.SelectedIndexChanged += ComboBox5_SelectedIndexChanged;
            //
            comboBox4.SelectedIndexChanged -= ComboBox4_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox4.Text = "";
            comboBox4.SelectedIndexChanged += ComboBox4_SelectedIndexChanged;
            //
            textBox6.TextChanged -= TextBox6_TextChanged;  // evita error de concurrencia de DbContext
            textBox6.Text = "";
            textBox6.TextChanged += TextBox6_TextChanged;
            //
            button6.Enabled = false;
            label16.Text = "";
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
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    List<ProdVendido> prodVendidos = await prodVendidoServices.GetProdVendidosAsync();
                    Pedido pedidoEnElQueEsta = prodVendidos.FirstOrDefault(pv => pv.NotaDeEnvioId == _notaDeEnvioAReparto.Id && pv.Estado != "Eliminado")?.Pedido;
                    Pedido pedidoAlQueQuiereIr = _reparto.Pedidos.FirstOrDefault(x => x.ClienteId == _notaDeEnvioAReparto.ClienteId && x.Estado != "Eliminado");
                    if (pedidoEnElQueEsta != null && pedidoAlQueQuiereIr != null && pedidoEnElQueEsta.Id == pedidoAlQueQuiereIr.Id)
                    {
                        MessageBox.Show("Esta Nota de Envío ya está en este Reparto");
                        return false;
                    }
                    if (pedidoAlQueQuiereIr == null)
                    {
                        Cliente cliente = await clienteServices.GetClientePorIdAsync(_notaDeEnvioAReparto.ClienteId);
                        pedidoAlQueQuiereIr = PedidoServices.CrearPedido(cliente, _reparto);
                    }
                    // prodVendidos
                    List<ProdVendido> prodVendidosDeLaNota = prodVendidos.Where(x => x.NotaDeEnvioId == _notaDeEnvioAReparto.Id).ToList();
                    foreach (ProdVendido prodVendido in prodVendidosDeLaNota)
                    {
                        prodVendido.Pedido = pedidoAlQueQuiereIr;
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosDeLaNota);
                    // pedido
                    if (pedidoAlQueQuiereIr.Id == 0)
                    {
                        pedidoAlQueQuiereIr.ProdVendidos = prodVendidosDeLaNota;
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedidoAlQueQuiereIr, true);
                        await pedidoServices.AddPedidoAsync(pedidoAlQueQuiereIr);
                    }
                    else
                    {
                        foreach (var pv in prodVendidosDeLaNota)
                        {
                            pedidoAlQueQuiereIr.ProdVendidos.Add(pv);
                        }
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedidoAlQueQuiereIr, true);
                        pedidoServices.EditPedido(pedidoAlQueQuiereIr);
                    }
                    if (pedidoEnElQueEsta != null)
                    {
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedidoEnElQueEsta, true);
                        pedidoServices.EditPedido(pedidoEnElQueEsta);
                        Reparto repartoEnElQueEsta = pedidoEnElQueEsta.Reparto;
                        if (repartoEnElQueEsta.Id != _reparto.Id)
                        {
                            RepartoServices.ActualizarCantidadesDeReparto(repartoEnElQueEsta);
                            repartoServices.EditReparto(repartoEnElQueEsta);
                        }
                    }
                    // reparto
                    RepartoServices.ActualizarCantidadesDeReparto(_reparto);
                    repartoServices.EditReparto(_reparto);
                    // guardado
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
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
            //
            comboBox5.SelectedIndexChanged -= ComboBox4_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox5.Text = "";
            comboBox5.SelectedIndexChanged += ComboBox4_SelectedIndexChanged;
            //
            comboBox4.SelectedIndexChanged -= ComboBox4_SelectedIndexChanged;  // evita error de concurrencia de DbContext
            comboBox4.Text = "";
            comboBox4.SelectedIndexChanged += ComboBox4_SelectedIndexChanged;
            //
            textBox6.TextChanged -= TextBox6_TextChanged;  // evita error de concurrencia de DbContext
            textBox6.Text = "";
            textBox6.TextChanged += TextBox6_TextChanged;
        }
    }
}
