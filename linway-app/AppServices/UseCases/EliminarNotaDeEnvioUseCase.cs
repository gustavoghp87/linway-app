using AppServices.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EliminarNotaDeEnvioUseCase : IEliminarNotaDeEnvioUseCase
    {
        private readonly INotaDeEnvioServices _notaDeEnvioServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IRegistroVentaServices _registroVentaServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public EliminarNotaDeEnvioUseCase(INotaDeEnvioServices notaDeEnvioServices, IProdVendidoServices prodVendidoServices, IRegistroVentaServices registroVentaServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _prodVendidoServices = prodVendidoServices;
            _notaDeEnvioServices = notaDeEnvioServices;
            _registroVentaServices = registroVentaServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(ICollection<NotaDeEnvio> notas, bool eliminarDeLosRepartos, bool eliminarRegistros, bool restarDeVentas)
        {
            List<ProdVendido> prodVendidosDeLasNotas = notas.SelectMany(n => n.ProdVendidos).ToList();
            foreach (ProdVendido prod in prodVendidosDeLasNotas)
            {
                prod.NotaDeEnvioId = null;
            }
            //
            if (eliminarDeLosRepartos)
            {
                foreach (ProdVendido prod in prodVendidosDeLasNotas)
                {
                    prod.PedidoId = null;
                }
            }
            //
            if (eliminarRegistros)
            {
                if (restarDeVentas)
                {
                    List<ProdVendido> prodARestar = prodVendidosDeLasNotas.FindAll(x => x.RegistroVentaId != null);
                    await _ventaServices.RestarDesdeProdVendidosAsync(prodARestar);
                }
                //
                List<RegistroVenta> registrosAEliminar = prodVendidosDeLasNotas.Where(pv => pv.RegistroVentaId != null).Select(pv => pv.RegistroVenta).ToList();
                _registroVentaServices.DeleteMany(registrosAEliminar);
                //
                foreach (ProdVendido prod in prodVendidosDeLasNotas)
                {
                    prod.RegistroVentaId = null;
                }
            }
            //
            _prodVendidoServices.EditOrDeleteMany(prodVendidosDeLasNotas);
            //
            _notaDeEnvioServices.DeleteMany(notas);
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
