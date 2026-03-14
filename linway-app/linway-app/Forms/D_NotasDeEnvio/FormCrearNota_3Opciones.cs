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
    public partial class FormCrearNota : Form
    {
        private List<Reparto> _repartos;
        private void CheckedChanged(object sender, EventArgs ev)
        {
            if (checkBox3.Checked)       // enviar a hoja de reparto
            {
                label33.Visible = true;
                label34.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
            }
            else
            {
                label33.Visible = false;
                label34.Visible = false;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
            }
        }
        private async void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs ev)
        {
            _repartos = null;
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
            if (repartos == null)
            {
                return;
            }
            _repartos = repartos;
            comboBox3.DataSource = _repartos;
            comboBox3.DisplayMember = "Nombre";
            comboBox3.ValueMember = "Nombre";
        }
        private async void ComboBox3_SelectorDeReparto_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string nombreReparto = comboBox3.Text;
            Pedido pedido = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    Reparto reparto = _repartos.Find(x => x.Nombre == nombreReparto && x.Estado != "Eliminado");
                    Pedido pedido = reparto.Pedidos.ToList().FirstOrDefault(x => x.ClienteId == _cliente.Id && x.Estado != "Eliminado");
                    return pedido;
                },
                "No se pudo buscar el Reparto por nombre",
                null
            );
            _pedido = pedido;  // puede ser null
        }
    }
}
