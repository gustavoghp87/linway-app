using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public static class DPedido
    {
        public delegate void DAddPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId);

        public readonly static DAddPedidoDesdeNota addPedidoDesdeNota
            = new DAddPedidoDesdeNota(AddPedidoDesdeNota);

        public static void AddPedidoDesdeNota(string diaDeReparto, string nombreReparto, long notaDeEnvioId)
        {
            bool response = Form1._servPedido.AgregarDesdeNota(diaDeReparto, nombreReparto, notaDeEnvioId);
            if (!response) MessageBox.Show("Algo falló al agregar Nota de Envío a la base de datos");
        }

    }
}
