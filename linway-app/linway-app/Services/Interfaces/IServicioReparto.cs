﻿using linway_app.Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IServicioReparto
    {
        bool Add(Reparto reparto);
        //void AgregarRepartoPorNota(Reparto reparto, List<ProdVendido> lstProdVendidos, string dire);
        //void AgregarRepartoPorVenta(Reparto reparto, List<Venta> lstVentas, string dire);
        bool Delete(Reparto reparto);
        bool Edit(Reparto reparto);
        Reparto Get(long id);
        List<Reparto> GetAll();
        void LimpiarDatos(Reparto reparto);
        bool AgregarPedidoAReparto(long clientId, string dia, string repartoNombre, List<ProdVendido> lstProdVendidos);
    }
}