using AppServices.EntityServices;
using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EnviarNotaDeEnvioARepartoUseCase : IEnviarNotaDeEnvioARepartoUseCase
    {
        private readonly IClienteServices _clienteServices;
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly ISavingServices _savingServices;
        public EnviarNotaDeEnvioARepartoUseCase(IClienteServices clienteServices, IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, ISavingServices savingServices)
        {
            _clienteServices = clienteServices;
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(NotaDeEnvio notaDeEnvio, Reparto reparto)
        {
            List<ProdVendido> prodVendidos = await _prodVendidoServices.GetAllAsync();
            Pedido pedidoEnElQueEsta = prodVendidos.FirstOrDefault(pv => pv.NotaDeEnvioId == notaDeEnvio.Id)?.Pedido;
            Pedido pedidoAlQueQuiereIr = reparto.Pedidos.FirstOrDefault(x => x.ClienteId == notaDeEnvio.ClienteId);
            if (pedidoEnElQueEsta != null && pedidoAlQueQuiereIr != null && pedidoEnElQueEsta.Id == pedidoAlQueQuiereIr.Id)
            {
                return UseCaseResponse.Fail("Esta Nota de Envío ya está en este Reparto");
            }
            if (pedidoAlQueQuiereIr == null)
            {
                Cliente cliente = await _clienteServices.GetPorIdAsync(notaDeEnvio.ClienteId);
                pedidoAlQueQuiereIr = PedidoServices.GetNuevoPedido(cliente, reparto);
            }
            // prodVendidos
            List<ProdVendido> prodVendidosDeLaNota = prodVendidos.Where(x => x.NotaDeEnvioId == notaDeEnvio.Id).ToList();
            foreach (ProdVendido prodVendido in prodVendidosDeLaNota)
            {
                prodVendido.Pedido = pedidoAlQueQuiereIr;
            }
            _prodVendidoServices.EditMany(prodVendidosDeLaNota);
            // pedido
            if (pedidoAlQueQuiereIr.Id == 0)
            {
                pedidoAlQueQuiereIr.ProdVendidos = prodVendidosDeLaNota;
            }
            else
            {
                foreach (var pv in prodVendidosDeLaNota)
                {
                    pedidoAlQueQuiereIr.ProdVendidos.Add(pv);
                }
                pedidoAlQueQuiereIr.Entregar = 1;
                _pedidoServices.Edit(pedidoAlQueQuiereIr);
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
