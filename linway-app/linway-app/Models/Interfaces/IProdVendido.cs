namespace linway_app.Models.Interfaces
{
    public interface IProdVendido
    {
        long Cantidad { get; set; }
        string Descripcion { get; set; }
        NotaDeEnvio NotaDeEnvio { get; set; }
        long? NotaDeEnvioId { get; set; }
        Pedido Pedido { get; set; }
        long? PedidoId { get; set; }
        decimal Precio { get; set; }
        Producto Producto { get; set; }
        long ProductoId { get; set; }
        RegistroVenta RegistroVenta { get; set; }
        long? RegistroVentaId { get; set; }
    }
}