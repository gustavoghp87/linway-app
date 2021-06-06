namespace linway_app.Models.Interfaces
{
    public interface IVenta
    {
        long Cantidad { get; set; }
        long Id { get; set; }
        Producto Producto { get; set; }
        long ProductoId { get; set; }
        string Estado { get; set; }
    }
}