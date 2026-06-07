using AppServices.EntityServices;
using AppServices.Interfaces;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class AgregarProdVendidoANotaDeEnvioUseCase : IAgregarProdVendidoANotaDeEnvioUseCase
    {
        private readonly INotaDeEnvioServices _notaDeEnvioServices;
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public AgregarProdVendidoANotaDeEnvioUseCase(INotaDeEnvioServices notaDeEnvioServices, IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _notaDeEnvioServices = notaDeEnvioServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(NotaDeEnvio notaDeEnvio, Producto _productoAAgregar, int _cantidadAAgregar)
        {
            var registroDeVentaId = notaDeEnvio.ProdVendidos.FirstOrDefault().RegistroVentaId;
            var nuevoProdVendido = new ProdVendido()
            {
                Cantidad = _cantidadAAgregar,
                Descripcion = _productoAAgregar.Nombre,
                NotaDeEnvio = notaDeEnvio,
                NotaDeEnvioId = notaDeEnvio.Id,
                Precio = ProductoServices.IsNegativePrice(_productoAAgregar)
                    ? (-1) * _productoAAgregar.Precio
                    : _productoAAgregar.Precio,
                Producto = _productoAAgregar,
                ProductoId = _productoAAgregar.Id,
                RegistroVentaId = registroDeVentaId  // nullable
            };
            Pedido pedido = null;
            // si la nota de envío estaba en algún reparto (mediante sus prod. vendidos) se agregar el prod. vendido nuevo al reparto
            var prodVendidoEnPedido = notaDeEnvio.ProdVendidos.ToList().Find(x => x.PedidoId != null);
            if (prodVendidoEnPedido != null)
            {
                pedido = await _pedidoServices.GetPorIdAsync((long)prodVendidoEnPedido.PedidoId);
                nuevoProdVendido.Pedido = pedido;
            }
            // si el prod. vendido ya estaba en esta nota de envío, se suma la cantidad y se actualizan las etiquetas
            var prodVendidos = await _prodVendidoServices.GetAllAsync();
            var existingProdVendido = prodVendidos.ToList().Find(x => x.NotaDeEnvioId == notaDeEnvio.Id && x.Producto.Id == _productoAAgregar.Id);
            if (existingProdVendido == null || ProductoServices.IsSaldo(existingProdVendido.Producto))
            {
                _prodVendidoServices.Add(nuevoProdVendido);
            }
            else
            {
                existingProdVendido.Cantidad += nuevoProdVendido.Cantidad;
                _prodVendidoServices.Edit(existingProdVendido);
            }
            // se actualizan las ventas
            if (registroDeVentaId != null)
            {
                await _ventaServices.SumarDesdeProdVendidosAsync(nuevoProdVendido);
            }
            // se actualiza la nota de envío
            notaDeEnvio.ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(notaDeEnvio.ProdVendidos);
            notaDeEnvio.Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(notaDeEnvio.ProdVendidos);
            _notaDeEnvioServices.Edit(notaDeEnvio);
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
