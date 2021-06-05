using linway_app.Models;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public static class DRegistroVenta
    {
        public delegate long DAddRegistroVenta(RegistroVenta registroVenta);

        public readonly static DAddRegistroVenta addRegistroVenta = new DAddRegistroVenta(AddRegistroVenta);

        private static long AddRegistroVenta(RegistroVenta registroVenta)
        {
            long response = Form1._servRegistroVenta.AddAndGetId(registroVenta);
            if (response == 0) MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
            return response;
        }
    }
}
