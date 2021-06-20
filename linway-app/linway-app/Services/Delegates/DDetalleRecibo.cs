using linway_app.Forms;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public class DDetalleRecibo
    {
        public readonly static Action<DetalleRecibo> addDetalleRecibo = AddDetalleRecibo;
        public readonly static Action<DetalleRecibo> deleteDetalleRecibo = DeleteDetalleRecibo;
        public readonly static Func<List<DetalleRecibo>> getDetalleRecibo = GetDetalleRecibos;

        private static readonly IServiceBase<DetalleRecibo> _service = Form1._servDetalleRecibo;
        private static void AddDetalleRecibo(DetalleRecibo detalleRecibo)
        {
            bool response = _service.Add(detalleRecibo);
            if (!response) MessageBox.Show("Algo falló al guardar el Detalle de Recibo en la base de datos");
        }
        private static void DeleteDetalleRecibo(DetalleRecibo detalleRecibo)
        {
            bool response = _service.Delete(detalleRecibo);
            if (!response) MessageBox.Show("Algo falló al eliminar el Detalle de Recibo en la base de datos");
        }
        private static List<DetalleRecibo> GetDetalleRecibos()
        {
            return _service.GetAll();
        }
    }
}
