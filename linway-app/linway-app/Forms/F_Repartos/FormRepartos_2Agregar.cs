using linway_app.PresentationHelpers;
using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
            if (textBox1.Text == "")
            {
                return;
            }
            string diaReparto = comboBox3.Text;
            DiaReparto diaRep = _lstDiaRepartos.Find(x => x.Dia.Contains(diaReparto));
            Reparto nuevoReparto = new Reparto
            {
                Nombre = textBox1.Text,
                DiaRepartoId = diaRep.Id,
                Ta = 0,
                Tae = 0,
                Td = 0,
                Te = 0,
                Tl = 0,
                TotalB = 0,
                Tt = 0
            };
            var success = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var savingServices = sp.GetRequiredService<ISavingServices>();
                    var repartoServices = sp.GetRequiredService<IRepartoServices>();
                    repartoServices.AddReparto(nuevoReparto);
                    bool guardado = await savingServices.SaveAsync();
                    if (!guardado)
                    {
                        savingServices.DiscardChanges();
                        MessageBox.Show("No se hicieron cambios");
                    }
                    return guardado;
                },
                "No se pudo agregar el Reparto",
                this
            );
            if (!success)
            {
                MessageBox.Show("No se pudo agregar Reparto");
                return;
            }
            await Actualizar();
            LimpiarPantalla();
            await ActualizarCombobox1();
            await UpdateGrid();
        }
        private void Button5_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
    }
}
