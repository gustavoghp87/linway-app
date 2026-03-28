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
                label33Dia.Visible = true;
                label34Nombre.Visible = true;
                comboBox3NombreDeReparto.Visible = true;
                comboBox4DiaDeReparto.Visible = true;
            }
            else
            {
                label33Dia.Visible = false;
                label34Nombre.Visible = false;
                comboBox3NombreDeReparto.Visible = false;
                comboBox4DiaDeReparto.Visible = false;
            }
        }
        private async void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs ev)
        {
            _repartos = null;
            string diaReparto = comboBox4DiaDeReparto.Text;
            List<Reparto> repartos = await UIExecutor.ExecuteAsync(
                _scope,
                async sp =>
                {
                    var diaRepartoServices = sp.GetRequiredService<IDiaRepartoServices>();
                    List<DiaReparto> lstDiasRep = await diaRepartoServices.GetAllAsync();
                    List<Reparto> repartos = lstDiasRep.Find(x => x.Dia == diaReparto).Repartos.ToList();
                    return repartos;
                },
                "No se pudieron buscar los Repartos por Día",
                null
            );
            if (repartos == null)
            {
                return;
            }
            _repartos = repartos;
            comboBox3NombreDeReparto.DataSource = _repartos;
            comboBox3NombreDeReparto.SelectedIndex = 0;
            comboBox3NombreDeReparto.DisplayMember = "Nombre";
            comboBox3NombreDeReparto.ValueMember = "Nombre";
        }
        private void ComboBox3_SelectorDeReparto_SelectedIndexChanged(object sender, EventArgs ev)
        {
            Reparto reparto = (Reparto)comboBox3NombreDeReparto.SelectedItem;
            _pedido = reparto.Pedidos.ToList().FirstOrDefault(x => x.ClienteId == _cliente.Id);
            _reparto = reparto;
        }
    }
}
