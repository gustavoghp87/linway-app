using AppLinway.PresentationHelpers;
using AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Windows.Forms;

namespace AppLinway.Forms
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
                DiaRepartoId = diaRep.Id
            };
            var resultado = await UIExecutor.ExecuteAsync(
                _scope,
                async sp => {
                    var useCase = _scope.ServiceProvider.GetRequiredService<IAgregarRepartoUseCase>();
                    return await useCase.ExecuteAsync(nuevoReparto, _lstDiaRepartos);
                },
                "No se pudo agregar el Reparto",
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
            LimpiarPantalla();
            await Actualizar();
        }
        private void Button5_Click(object sender, EventArgs ev)
        {
            LimpiarPantalla();
        }
    }
}
