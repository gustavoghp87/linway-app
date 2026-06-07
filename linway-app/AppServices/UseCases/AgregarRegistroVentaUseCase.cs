using AppServices.EntityServices;
using AppServices.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class AgregarRegistroVentaUseCase : IAgregarRegistroVentaUseCase
    {
        private readonly IPedidoServices _pedidoServices;
        private readonly IProductoServices _productoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IRegistroVentaServices _registroVentaServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public AgregarRegistroVentaUseCase(IPedidoServices pedidoServices, IProductoServices productoServices, IProdVendidoServices prodVendidoServices, IRegistroVentaServices registroVentaServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _productoServices = productoServices;
            _prodVendidoServices = prodVendidoServices;
            _registroVentaServices = registroVentaServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(ICollection<Venta> ventas, Cliente cliente, bool enviarAReparto, Reparto reparto)
        {
            Pedido pedido = null;
            if (enviarAReparto)   // enviar a reparto
            {
                pedido = reparto.Pedidos.FirstOrDefault(x => x.ClienteId == cliente.Id);
                if (pedido == null)
                {
                    pedido = PedidoServices.GetNuevoPedido(cliente, reparto);
                }
            }
            // se crea un Registro de Venta (para "Venta particular" o Cliente en Reparto si hay)
            // se recorren sus Ventas en bruto creando Ventas cuando no tienen y sumando cuando sí tienen
            // se recorren sus Ventas en bruto creando ProdVendidos (asociados al Pedido si hay) (nunca se editan ProdVendidos preexistentes porque podrían pertenecer a otras NotaDeEnvio preexistentes)
            // opcionalmente se envía a Reparto, actualizando las etiquetas del Reparto y del Pedido
            //   caso 1: el Reparto no existe, se crea uno
            //   caso 2: el Reparto existe, se agregan ProdVendidos
            var nuevoRegistroVenta = new RegistroVenta
            {   // si se envía a Reparto hay cliente
                ClienteId = cliente != null ? cliente.Id : 634,
                NombreCliente = cliente != null ? cliente.Direccion : "Venta particular",
                Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha)
            };
            _registroVentaServices.Add(nuevoRegistroVenta);
            //
            var prodVendidosAAgregar = new List<ProdVendido>();
            List<Producto> productos = await _productoServices.GetAllAsync();
            foreach (Venta ventaNueva in ventas)
            {
                Producto producto = productos.Find(x => x.Id == ventaNueva.ProductoId);
                var nuevoProdVendido = new ProdVendido
                {
                    Cantidad = ventaNueva.Cantidad,
                    Descripcion = producto.Nombre,
                    Pedido = enviarAReparto ? pedido : null,
                    Precio = producto.Precio,
                    ProductoId = producto.Id,
                    RegistroVenta = nuevoRegistroVenta
                };
                prodVendidosAAgregar.Add(nuevoProdVendido);
            }
            _prodVendidoServices.AddMany(prodVendidosAAgregar);
            await _ventaServices.SumarDesdeProdVendidosAsync(prodVendidosAAgregar);
            //
            if (enviarAReparto)
            {
                if (pedido.Id == 0)
                {
                    await _pedidoServices.AddAsync(pedido);
                }
                else
                {
                    pedido.Entregar = 1;
                    _pedidoServices.Edit(pedido);
                }
            }
            //
            bool guardado = await _savingServices.SaveAsync();
            if (!guardado)
            {
                _savingServices.DiscardChanges();
                return UseCaseResponse.Fail("No se hicieron cambios");
            }
            return UseCaseResponse.Ok();
        }
    }
}
