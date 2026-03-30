using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormCrearNota : Form
    {
        private void CheckedChanged(object sender, EventArgs ev)  // enviar a hoja de reparto
        {
            bool enviarA_HDR = checkBox3EnviarA_HDR.Checked;
            if (enviarA_HDR)
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
        private void EnviarA_HDR_SelectedIndexChanged(object sender, EventArgs ev)
        {
            string diaReparto = comboBox4DiaDeReparto.Text;
            List<Reparto> repartos = _lstDias.Find(x => x.Dia == diaReparto).Repartos.ToList();
            comboBox3NombreDeReparto.DataSource = repartos.Count > 0 ? repartos : null;
            comboBox3NombreDeReparto.SelectedIndex = repartos.Count > 0 ? 0 : -1;
            comboBox3NombreDeReparto.DisplayMember = "Nombre";
            comboBox3NombreDeReparto.ValueMember = "Nombre";
        }
        private void ComboBox3_SelectorDeReparto_SelectedIndexChanged(object sender, EventArgs ev)
        {
            Reparto reparto = (Reparto)comboBox3NombreDeReparto.SelectedItem;
            _reparto = reparto;
            EstablecerReparto();
        }
    }
}
