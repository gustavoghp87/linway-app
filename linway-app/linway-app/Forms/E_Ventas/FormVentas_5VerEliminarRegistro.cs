using linway_app.PresentationHelpers;
using linway_app.Services.FormServices;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormVentas : Form
    {
        private RegistroVenta _registroVerEliminar;
        private void ActualizarGrid2ProdVendidos(ICollection<ProdVendido> lstProdVendidos)
        {
            var grid = new List<EProdVendido>();
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                grid.Add(Form1.Mapper.Map<EProdVendido>(prodVendido));
            }
            dataGridView2.DataSource = grid;
            dataGridView2.Columns[0].Width = 28;
            dataGridView2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridView2.Columns[1].Width = 150;
            dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
        }
        private async void TextBox1_TextChanged(object sender, EventArgs ev)  // registro por Id
        {
            _registroVerEliminar = null;
            string numeroDeRegistroVenta = textBox1.Text;
            if (numeroDeRegistroVenta == "")
            {
                labelProductoN.Text = "";
                labelFecha.Text = "";
                labelTotal.Text = "";
                ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                bDeshacerVenta.Enabled = false;
                return;
            }
            if (!long.TryParse(numeroDeRegistroVenta, out long registroVentaId))
            {
                return;
            }
            RegistroVenta registroVenta = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    return await registroVentaServices.GetPorIdAsync(registroVentaId);
                },
                "No se pudo buscar el Registro de Venta",
                null
            );
            if (registroVenta == null)
            {
                labelProductoN.Text = "";
                labelFecha.Text = "";
                labelTotal.Text = "";
                ActualizarGrid2ProdVendidos(new List<ProdVendido>());
                bDeshacerVenta.Enabled = false;
                return;
            }
            _registroVerEliminar = registroVenta;
            labelFecha.Text = registroVenta.Fecha;
            labelProductoN.Text = registroVenta.NombreCliente;
            ActualizarGrid2ProdVendidos(registroVenta.ProdVendidos.ToList());
            decimal importeTotal = 0;
            foreach (ProdVendido prodVendido in registroVenta.ProdVendidos)
            {
                importeTotal += prodVendido.Precio;
            }
            labelTotal.Text = "Total: $" + importeTotal.ToString();
            bDeshacerVenta.Enabled = true;
        }
        private async void DeshacerVenta_Click(object sender, EventArgs ev)  // elimina registro y resetea contador de ventas
        {
            if (!cbSeguro.Checked)
            {
                MessageBox.Show("Debe confirmar que está seguro de eliminar el Registro y reiniciar el contador de Ventas.");
                return;
            }
            ;
            bool logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var pedidoServices = sp.GetRequiredService<IPedidoServices>();
                    var prodVendidoServices = sp.GetRequiredService<IProdVendidoServices>();
                    var registroVentaServices = sp.GetRequiredService<IRegistroVentaServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    var ventaServices = sp.GetRequiredService<IVentaServices>();
                    //
                    await ventaServices.UpdateDesdeProdVendidosAsync(_registroVerEliminar.ProdVendidos, false);
                    //
                    var pedido = _registroVerEliminar.ProdVendidos.FirstOrDefault(pv => pv.PedidoId != null)?.Pedido;
                    if (pedido != null)
                    {
                        var prodVendidos = pedido.ProdVendidos.Where(pv => pv.RegistroVentaId != _registroVerEliminar.Id).ToList();
                        pedido.ProdVendidos = prodVendidos;
                        PedidoServices.ActualizarCantidadesYDescripcionDePedido(pedido, pedido.Entregar == 1);
                        pedidoServices.Edit(pedido);
                        //
                        Reparto reparto = await repartoServices.GetPorIdAsync(pedido.RepartoId);
                        reparto.Pedidos.Remove(pedido);
                        RepartoServices.ActualizarCantidadesDeReparto(reparto);
                        repartoServices.Edit(reparto);
                    }
                    //
                    foreach (ProdVendido pv in _registroVerEliminar.ProdVendidos)
                    {
                        pv.RegistroVentaId = null;
                        pv.PedidoId = null;
                    }
                    prodVendidoServices.EditOrDeleteMany(_registroVerEliminar.ProdVendidos.ToList());
                    //
                    registroVentaServices.Delete(_registroVerEliminar);
                    //
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo eliminar el Registro de Venta",
                this
            );
            if (!logrado)
            {
                return;
            }
            await Actualizar();
            LimpiarPantalla();
        }
    }
}
