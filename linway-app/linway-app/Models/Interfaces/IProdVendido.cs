namespace linway_app.Models.Interfaces
{
    public interface IProdVendido
    {
        long Cantidad { get; set; }
        string Descripcion { get; set; }
        long Id { get; set; }
        NotaDeEnvio NotaDeEnvio { get; set; }
        long? NotaDeEnvioId { get; set; }
        Pedido Pedido { get; set; }
        long? PedidoId { get; set; }
        double Precio { get; set; }
        Producto Producto { get; set; }
        long ProductoId { get; set; }
        RegistroVenta RegistroVenta { get; set; }
        long? RegistroVentaId { get; set; }
        string Estado { get; set; }
    }
}