using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarClienteUseCase : IEliminarClienteUseCase
    {
        private readonly IClienteServices _clienteServices;
        private readonly IDetalleReciboServices _detalleReciboServices;
        private readonly INotaDeEnvioServices _notaDeEnvioServices;
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IReciboServices _reciboServices;
        private readonly IRegistroVentaServices _registroVentaServices;
        private readonly ISavingServices _savingServices;
        public EliminarClienteUseCase(IClienteServices clienteServices, IDetalleReciboServices detalleReciboServices, INotaDeEnvioServices notaDeEnvioServices, IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, IReciboServices reciboServices, IRegistroVentaServices registroVentaServices, ISavingServices savingServices)
        {
            _clienteServices = clienteServices;
            _detalleReciboServices = detalleReciboServices;
            _notaDeEnvioServices = notaDeEnvioServices;
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _reciboServices = reciboServices;
            _registroVentaServices = registroVentaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(Cliente cliente)
        {
            {
                // se eliminan sus Recibos con sus Detalles
                List<Recibo> recibos = await _reciboServices.GetAllAsync();
                List<Recibo> recibosDelCliente = recibos.FindAll(r => r.ClienteId == cliente.Id);
                if (recibosDelCliente.Any())
                {
                    _detalleReciboServices.DeleteMany(recibosDelCliente.SelectMany(r => r.DetalleRecibos).ToList());
                    _reciboServices.DeleteMany(recibosDelCliente);
                }
            }
            {
                // se eliminan sus Notas de Envío con sus Productos Vendidos
                List<NotaDeEnvio> notas = await _notaDeEnvioServices.GetAllAsync();
                List<NotaDeEnvio> notasDelCliente = notas.FindAll(n => n.ClienteId == cliente.Id);
                _prodVendidoServices.DeleteMany(notasDelCliente.SelectMany(n => n.ProdVendidos).ToList());
                _notaDeEnvioServices.DeleteMany(notasDelCliente);
            }
            {
                // se eliminan sus Pedidos con sus Productos Vendidos
                List<Pedido> pedidos = await _pedidoServices.GetAllAsync();
                List<Pedido> pedidosDelCliente = pedidos.FindAll(p => p.ClienteId == cliente.Id);
                _prodVendidoServices.DeleteMany(pedidosDelCliente.SelectMany(p => p.ProdVendidos).ToList());
                _pedidoServices.DeleteMany(pedidosDelCliente);
            }
            {
                // se eliminan sus Registros de Venta con sus Productos Vendidos
                List<RegistroVenta> registros = await _registroVentaServices.GetAllAsync();
                List<RegistroVenta> registrosDelCliente = registros.FindAll(r => r.ClienteId == cliente.Id);
                _prodVendidoServices.DeleteMany(registrosDelCliente.SelectMany(r => r.ProdVendidos).ToList());
                _registroVentaServices.DeleteMany(registrosDelCliente);
            }
            //
            _clienteServices.Delete(cliente);
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
