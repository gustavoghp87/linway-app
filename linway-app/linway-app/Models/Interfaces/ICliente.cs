using System.Collections.Generic;

namespace linway_app.Models
{
    public interface ICliente
    {
        string CodigoPostal { get; set; }
        string Cuit { get; set; }
        string Direccion { get; set; }
        long Id { get; set; }
        string Name { get; set; }
        ICollection<NotaDeEnvio> NotaDeEnvio { get; set; }
        ICollection<Pedido> Pedido { get; set; }
        ICollection<Recibo> Recibo { get; set; }
        ICollection<RegistroVenta> RegistroVenta { get; set; }
        string Telefono { get; set; }
        string Tipo { get; set; }
    }
}