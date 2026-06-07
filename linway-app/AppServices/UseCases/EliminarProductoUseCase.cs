using AppServices.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarProductoUseCase : IEliminarProductoUseCase
    {
        private readonly IProductoServices _productoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public EliminarProductoUseCase(IProductoServices productoServices, IProdVendidoServices prodVendidoServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _productoServices = productoServices;
            _prodVendidoServices = prodVendidoServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Producto productoAEliminar)
        {
            await ValidarSiSePuedeEliminarAsync(productoAEliminar.Id);
            //
            List<Venta> ventas = await _ventaServices.GetAllAsync();
            Venta venta = ventas.Find(v => v.ProductoId == productoAEliminar.Id);
            if (venta != null)
            {
                _ventaServices.Delete(venta);
            }
            //
            _productoServices.Delete(productoAEliminar);
            //
            bool guardado = await _savingServices.SaveAsync();
            if (!guardado)
            {
                _savingServices.DiscardChanges();
                return UseCaseResponse.Fail("No se hicieron cambios");
            }
            return UseCaseResponse.Ok();
        }
        private async Task ValidarSiSePuedeEliminarAsync(long productoId)
        {
            List<ProdVendido> prodVendidos = await _prodVendidoServices.GetAllAsync();
            List<ProdVendido> prodVendidosDelProducto = prodVendidos.FindAll(pv => pv.ProductoId == productoId);
            string excepcion = "";
            if (prodVendidosDelProducto.Any(pv => pv.NotaDeEnvioId != null))
            {
                excepcion += "\n\n";
                excepcion += $"No se puede eliminar el producto porque tiene las siguientes Notas de Envío asociadas: ";
                excepcion += string.Join(", ", prodVendidosDelProducto.Where(pv => pv.NotaDeEnvioId != null).Select(pv => pv.NotaDeEnvioId));
            }
            if (prodVendidosDelProducto.Any(pv => pv.PedidoId != null))
            {
                excepcion += "\n\n";
                excepcion += $"No se puede eliminar el producto porque tiene los siguientes Pedidos asociados: ";
                excepcion += string.Join(", ", prodVendidosDelProducto.Where(pv => pv.PedidoId != null).Select(pv => $"{pv.Pedido.Reparto.DiaReparto.Dia} {pv.Pedido.Reparto.Nombre}"));
            }
            if (prodVendidosDelProducto.Any(pv => pv.RegistroVentaId != null))
            {
                excepcion += "\n\n";
                excepcion += $"No se puede eliminar el producto porque tiene los siguientes Registros de Venta asociados: ";
                excepcion += string.Join(", ", prodVendidosDelProducto.Where(pv => pv.RegistroVentaId != null).Select(pv => pv.RegistroVentaId));
            }
            if (excepcion.Length > 0)
            {
                throw new InvalidOperationException(excepcion);
            }
        }
    }
}
