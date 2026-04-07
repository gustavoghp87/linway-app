using linway_app.PresentationHelpers;
using Models;
using System;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class FormRepartos : Form
    {
        private async void AgregarRepartoADIA_btn2_Click(object sender, EventArgs ev)
        {
            await Actualizar();
            if (textBox1AgregarRepartoNombre.Text == "")
            {
                return;
            }
            string diaReparto = comboBox3.Text;
            DiaReparto diaRep = _lstDiaRepartos.Find(x => x.Dia.Contains(diaReparto));
            var nuevoReparto = new Reparto
            {
                Nombre = textBox1AgregarRepartoNombre.Text,
                DiaReparto = diaRep,
                DiaRepartoId = diaRep.Id,
                //Ta = 0,
                //Tae = 0,
                //Td = 0,
                //Te = 0,
                //Tl = 0,
                //TotalB = 0,
                //Tt = 0
            };
            var logrado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var servicesContext = ServiceContext.Get(sp);
                    servicesContext.RepartoServices.Add(nuevoReparto, _lstDiaRepartos);
                    bool guardado = await servicesContext.SavingServices.SaveAsync();
                    if (!guardado)
                    {
                        servicesContext.SavingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo agregar el Reparto",
                this
            );
            if (!logrado)
            {
                return;
            }
            LimpiarPantalla();
            await Actualizar();
        }
        private void Button5_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
    }
}
