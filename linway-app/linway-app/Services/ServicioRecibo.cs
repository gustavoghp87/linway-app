using linway_app.Models;

namespace linway_app.Services
{
    class ServicioRecibo
    {
        private IRecibo _recibo;

        public ServicioRecibo(IRecibo recibo)
        {
            _recibo = recibo;
        }

        public void CalcularImporteTotal()
        {
            double subTo = 0;
            if (_recibo.DetalleRecibos != null && _recibo.DetalleRecibos.Count != 0)
                foreach (IDetalleRecibo detalle in _recibo.DetalleRecibos)
                {
                    subTo += detalle.Importe;
                }
            _recibo.ImporteTotal = subTo;
        }
    }
}
