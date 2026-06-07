using AppServices.EntityServices;
using AppServices.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class AgregarNotaDeEnvioUseCase : IAgregarNotaDeEnvioUseCase
    {
        private readonly INotaDeEnvioServices _notaDeEnvioServices;
        private readonly IPedidoServices _pedidoServices;
        private readonly IProdVendidoServices _prodVendidoServices;
        private readonly IRegistroVentaServices _registroVentaServices;
        private readonly IVentaServices _ventaServices;
        private readonly ISavingServices _savingServices;
        public AgregarNotaDeEnvioUseCase(INotaDeEnvioServices notaDeEnvioServices, IPedidoServices pedidoServices, IProdVendidoServices prodVendidoServices, IRegistroVentaServices registroVentaServices, IVentaServices ventaServices, ISavingServices savingServices)
        {
            _notaDeEnvioServices = notaDeEnvioServices;
            _pedidoServices = pedidoServices;
            _prodVendidoServices = prodVendidoServices;
            _registroVentaServices = registroVentaServices;
            _ventaServices = ventaServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse<NotaDeEnvio>> ExecuteAsync(Cliente cliente, ICollection<Producto> productos, ICollection<ProdVendido> prodVendidos, Pedido pedido, Reparto reparto, bool agregarProdVendidosARegistrosYVentas, bool enviarAHojaDeReparto, bool imprimir)
        {
            var nuevaNota = new NotaDeEnvio
            {
                Cliente = cliente,
                ClienteId = cliente.Id,
                Detalle = NotaDeEnvioServices.ExtraerDetalleDeNotaDeEnvio(prodVendidos),
                Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                ImporteTotal = NotaDeEnvioServices.ExtraerImporteDeNotaDeEnvio(prodVendidos),
                Impresa = 0
                //ProdVendidos = _lstProdVendidosAAgregar
            };
            _notaDeEnvioServices.Add(nuevaNota);
            foreach (ProdVendido prodVendido in prodVendidos)
            {
                prodVendido.NotaDeEnvio = nuevaNota;
                prodVendido.NotaDeEnvioId = nuevaNota.Id;
            }
            if (agregarProdVendidosARegistrosYVentas)
            {
                var nuevoRegistro = new RegistroVenta
                {
                    ClienteId = cliente.Id,
                    Fecha = DateTime.Now.ToString(Constants.FormatoDeFecha),
                    NombreCliente = cliente.Direccion
                };
                _registroVentaServices.Add(nuevoRegistro);
                foreach (var prodVendido in prodVendidos)
                {
                    prodVendido.RegistroVenta = nuevoRegistro;
                    prodVendido.RegistroVentaId = nuevoRegistro.Id;
                    //_lstProdVendidosAAgregar.Find(x => x.Id == prodVendido.Id).RegistroVenta = nuevoRegistro;  // para que el siguiente checkbox no pise los cambios
                }
                await _ventaServices.SumarDesdeProdVendidosAsync(prodVendidos);
            }
            if (enviarAHojaDeReparto)
            {
                bool existiaPedido = pedido != null;
                if (!existiaPedido)
                {
                    pedido = PedidoServices.GetNuevoPedido(cliente, reparto);
                }
                pedido.Entregar = 1;
                foreach (ProdVendido prodVendido in prodVendidos)
                {
                    prodVendido.Pedido = pedido;
                    pedido.ProdVendidos.Add(prodVendido);
                }
                if (existiaPedido)
                {
                    _pedidoServices.Edit(pedido);
                }
                else
                {
                    await _pedidoServices.AddAsync(pedido);
                }
                // reparto
                foreach (var p in reparto.Pedidos)
                {
                    if (p.Id == pedido.Id)
                    {
                        p.ProdVendidos = pedido.ProdVendidos;
                    }
                }
            }
            //
            _prodVendidoServices.AddMany(prodVendidos);
            //
            if (imprimir)
            {
                foreach (ProdVendido prodVendido in prodVendidos)
                {
                    prodVendido.Producto = productos.ToList().Find(p => p.Id == prodVendido.ProductoId);
                }
            }
            //
            bool guardado = await _savingServices.SaveAsync();
            if (!guardado)
            {
                _savingServices.DiscardChanges();
                return UseCaseResponse<NotaDeEnvio>.Fail("No se hicieron cambios");
            }
            //return nuevaNota;
            return new UseCaseResponse<NotaDeEnvio>()
            {
                Success = true,
                Data = nuevaNota
            };
        }
    }
}
