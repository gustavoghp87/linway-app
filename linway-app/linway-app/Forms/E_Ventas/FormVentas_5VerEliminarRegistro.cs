using AppLinway.PresentationHelpers;
using AppServices.DTOs;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppLinway.Forms
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
            checkBox3EliminarRegistroRestarDeVentas.Checked = false;
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
            var registroVenta = await UIExecutor.ExecuteAsync(
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
        private async void DeshacerVenta_Click(object sender, EventArgs ev)  // elimina registro, quitándolo de pedido y nota de envío, y opcionalmente resetea contador de ventas
        {
            if (!cbSeguro.Checked)
            {
                MessageBox.Show("Debe confirmar que está seguro de eliminar el Registro y reiniciar el contador de Ventas.");
                return;
            }
            bool restarDeVentas = checkBox3EliminarRegistroRestarDeVentas.Checked;
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IEliminarRegistroVentaUseCase>();
                    return await useCase.ExecuteAsync(_registroVerEliminar, restarDeVentas);
                },
                "No se pudo eliminar el Registro de Venta",
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
            await Actualizar();
            LimpiarPantalla();
        }
    }
}
