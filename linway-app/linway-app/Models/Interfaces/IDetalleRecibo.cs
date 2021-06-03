namespace linway_app.Models
{
    public interface IDetalleRecibo
    {
        string Detalle { get; set; }
        long Id { get; set; }
        double Importe { get; set; }
        Recibo Recibo { get; set; }
        long ReciboId { get; set; }
        string Estado { get; set; }
    }
}