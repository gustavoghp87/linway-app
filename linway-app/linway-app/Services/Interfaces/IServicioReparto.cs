using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services
{
    interface IServicioReparto
    {
        bool Add(Reparto reparto);
        //void AgregarRepartoPorNota(Reparto reparto, List<ProdVendido> lstProdVendidos, string dire);
        //void AgregarRepartoPorVenta(Reparto reparto, List<Venta> lstVentas, string dire);
        bool Delete(Reparto reparto);
        bool Edit(Reparto reparto);
        Reparto Get(long id);
        List<Reparto> GetAll();
        void LimpiarDatos(Reparto reparto);
        bool AgregarPedidoARepartoPorNota(Reparto reparto, Cliente cliente, List<ProdVendido> lstProdVendidos);
    }
}