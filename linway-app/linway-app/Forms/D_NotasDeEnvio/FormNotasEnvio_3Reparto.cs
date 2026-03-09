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
            string numeroDeNota = textBox6.Text;
            if (!long.TryParse(numeroDeNota, out long notaDeEnvioId))
            {
                return;
            }
            string diaReparto = comboBox4.Text;
            string nombreReparto = comboBox5.Text;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var clienteServices = sp.GetRequiredService<IClienteServices>();
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    var notaDeEnvioServices = sp.GetRequiredService<INotaDeEnvioServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    NotaDeEnvio notaDeEnvio = await notaDeEnvioServices.GetNotaDeEnvioPorIdAsync(notaDeEnvioId);
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetDiaRepartosAsync();
                    Reparto reparto = lstDiasRep
                        .Find(x => x.Dia == diaReparto && x.Estado != null && x.Estado != "Eliminado").Reparto.ToList()
                        .Find(x => x.Nombre == nombreReparto && x.Estado != null && x.Estado != "Eliminado");
                    List<Pedido> pedidos = await pedidoServices.GetPedidosAsync();  // evita error de concurrencia de DbContext
                    List<ProdVendido> prodVendidos = await prodVendidoServices.GetProdVendidosAsync();

                    // si ya está en un reparto
                    Pedido pedidoEnElQueEsta = pedidos.ToList().FirstOrDefault(x => x.ProdVendidos.ToList().Exists(pv => pv.NotaDeEnvioId == notaDeEnvioId && x.Estado != "Eliminado"));
                    //prodVendidos.FirstOrDefault
                    Pedido pedidoAlQueQuiereIr = pedidos.ToList().FirstOrDefault(x => x.ClienteId == notaDeEnvio.ClienteId && x.Estado != "Eliminado");
                    if (pedidos.Exists(x => x.Id == notaDeEnvioId))
                    {
                        MessageBox.Show("Esta Nota de Envío ya está en este Reparto");
                        return false;
                    }
                    
                    Cliente cliente = await clienteServices.GetClientePorIdAsync(notaDeEnvio.ClienteId);
                    Pedido pedido = pedidoAlQueQuiereIr;
                    bool existiaPedido = pedido != null;
                    if (!existiaPedido)
                    {
                        pedido = new Pedido()
                        {
                            Cliente = cliente,
                            Direccion = cliente.Direccion,
                            Reparto = reparto,
                            Entregar = 1,
                            Estado = "Activo",
                            ProductosText = "",
                            L = 0,
                            A = 0,
                            Ae = 0,
                            D = 0,
                            E = 0,
                            T = 0
                        };
                    }
                    
                    var prodVendidosAActualizar = new List<ProdVendido>();
                    foreach (ProdVendido prodVendido in prodVendidos.Where(x => x.NotaDeEnvioId == notaDeEnvio.Id))
                    {
                        prodVendido.Pedido = pedido;
                        prodVendidosAActualizar.Add(prodVendido);
                    }
                    prodVendidoServices.EditProdVendidos(prodVendidosAActualizar);
                    RepartoServices.ActualizarEtiquetasDeReparto(reparto);
                    repartoServices.EditReparto(reparto);
                    if (existiaPedido)
                    {
                        PedidoServices.ActualizarEtiquetasDePedido(pedido, true);
                        pedidoServices.EditPedido(pedido);
                    }
                    else
                    {
                        await pedidoServices.AddPedidoAsync(pedido);
                    }
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
            comboBox5.Text = "";
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
    }
}
