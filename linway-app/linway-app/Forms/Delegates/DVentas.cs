using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public static class DVentas
    {
        public delegate List<Venta> DGetNotadeEnvios();
        public delegate void DEditVenta(Venta venta);
        public delegate void DAddVenta(Venta venta);

        public readonly static DGetNotadeEnvios getVentas = new DGetNotadeEnvios(GetVentas);
        public readonly static DEditVenta editVenta = new DEditVenta(EditVenta);
        public readonly static DAddVenta addVenta = new DAddVenta(AddVenta);

        private static List<Venta> GetVentas()
        {
            return Form1._servVenta.GetAll();
        }
        private static void EditVenta(Venta venta)
        {
            bool response = Form1._servVenta.Edit(venta);
            if (!response) MessageBox.Show("Algo falló al editar Venta en base de datos");
        }
        private static void AddVenta(Venta venta)
        {
            bool response = Form1._servVenta.Add(venta);
            if (!response) MessageBox.Show("Algo falló al agregar Venta a base de datos");
        }
    }
}
