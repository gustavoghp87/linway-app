using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DVentas
    {
        public delegate void DAddVenta(Venta venta);
        public delegate void DDeleteVenta(Venta venta);
        public delegate void DEditVenta(Venta venta);
        public delegate Venta DGetVenta(long ventaId);
        public delegate List<Venta> DGetVentas();

        public readonly static DAddVenta addVenta = new DAddVenta(AddVenta);
        public readonly static DDeleteVenta deleteVenta = new DDeleteVenta(DeleteVenta);
        public readonly static DEditVenta editVenta = new DEditVenta(EditVenta);
        public readonly static DGetVenta getVenta = new DGetVenta(GetVenta);
        public readonly static DGetVentas getVentas = new DGetVentas(GetVentas);

        private static void AddVenta(Venta venta)
        {
            bool response = Form1._servVenta.Add(venta);
            if (!response) MessageBox.Show("Algo falló al agregar Venta a la base de datos");
        }
        private static void DeleteVenta(Venta venta)
        {
            bool response = Form1._servVenta.Delete(venta);
            if (!response) MessageBox.Show("Algo falló al eliminar Venta de la base de datos");
        }
        private static void EditVenta(Venta venta)
        {
            bool response = Form1._servVenta.Edit(venta);
            if (!response) MessageBox.Show("Algo falló al editar Venta en la base de datos");
        }
        private static Venta GetVenta(long ventaId)
        {
            return Form1._servVenta.Get(ventaId);
        }
        private static List<Venta> GetVentas()
        {
            return Form1._servVenta.GetAll();
        }
    }
}
