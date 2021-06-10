namespace linway_app.Models.Interfaces
{
    public interface IDetalleRecibo
    {
        string Detalle { get; set; }
        decimal Importe { get; set; }
        Recibo Recibo { get; set; }
        long ReciboId { get; set; }
    }
}