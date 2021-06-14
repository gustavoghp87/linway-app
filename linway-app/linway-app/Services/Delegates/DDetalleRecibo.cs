using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public class DDetalleRecibo
    {
        public delegate void DAddDetalleRecibo(DetalleRecibo detalleRecibo);
        public delegate void DDeleteDetalleRecibo(DetalleRecibo detalleRecibo);
        public delegate List<DetalleRecibo> DGetDetalleRecibo();

        public readonly static DAddDetalleRecibo addDetalleRecibo = new DAddDetalleRecibo(AddDetalleRecibo);
        public readonly static DDeleteDetalleRecibo deleteDetalleRecibo = new DDeleteDetalleRecibo(DeleteDetalleRecibo);
        public readonly static DGetDetalleRecibo getDetalleRecibos = new DGetDetalleRecibo(GetDetalleRecibos);

        private static void AddDetalleRecibo(DetalleRecibo detalleRecibo)
        {
            bool response = Form1._servDetalleRecibo.Add(detalleRecibo);
            if (!response) MessageBox.Show("Algo falló al guardar el Detalle de Recibo en la base de datos");
        }
        private static void DeleteDetalleRecibo(DetalleRecibo detalleRecibo)
        {
            bool response = Form1._servDetalleRecibo.Delete(detalleRecibo);
            if (!response) MessageBox.Show("Algo falló al eliminar el Detalle de Recibo en la base de datos");
        }
        private static List<DetalleRecibo> GetDetalleRecibos()
        {
            return Form1._servDetalleRecibo.GetAll();
        }
    }
}
