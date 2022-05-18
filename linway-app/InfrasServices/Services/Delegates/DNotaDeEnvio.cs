using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static linway_app.Services.Delegates.DProdVendido;

namespace linway_app.Services.Delegates
{
    public static class DNotaDeEnvio
    {
        public readonly static Action<NotaDeEnvio> addNotaDeEnvio = AddNotaDeEnvio;
        public readonly static Func<NotaDeEnvio, long> addNotaDeEnvioReturnId = AddNotaDeEnvioReturnId;
        public readonly static Action<NotaDeEnvio> deleteNotaDeEnvio = DeleteNotaDeEnvio;
        public readonly static Func<NotaDeEnvio, bool> editNotaDeEnvio = EditNotaDeEnvio;
        public readonly static Func<NotaDeEnvio, NotaDeEnvio> editNotaDeEnvioAgregar = EditNotaDeEnvioAgregar;
        public readonly static Func<NotaDeEnvio, ProdVendido, NotaDeEnvio> editNotaDeEnvioQuitar = EditNotaDeEnvioQuitar;
        public readonly static Func<List<ProdVendido>, string> extraerDetalleDeNotaDeEnvio = ExtraerDetalleDeNotaDeEnvio;
        public readonly static Func<List<ProdVendido>, decimal> extraerImporteDeNotaDeEnvio = ExtraerImporteDeNotaDeEnvio;
        public readonly static Func<long, NotaDeEnvio>  getNotaDeEnvio = GetNotaDeEnvio;
        public readonly static Func<List<NotaDeEnvio>> getNotaDeEnvios = GetNotaDeEnvios;

        private static readonly IServiceBase<NotaDeEnvio> _service = ServicesObjects.ServNotaDeEnvio;
        private static long AddAndGetId(NotaDeEnvio notaDeEnvio)
        {
            try
            {
                _service.Add(notaDeEnvio);
                List<NotaDeEnvio> lst = getNotaDeEnvios();
                return lst.Last().Id;
            }
            catch { return 0; }
        }
        private static void AddNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = _service.Add(notaDeEnvio);
            if (!response) Console.WriteLine("Algo falló al agregar Nota de Envío a base de datos");
        }
        private static long AddNotaDeEnvioReturnId(NotaDeEnvio notaDeEnvio)
        {
            long response = AddAndGetId(notaDeEnvio);
            if (response == 0) Console.WriteLine("Algo falló al agregar Nota de Envío a base de datos");
            return response;
        }
        private static void DeleteNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = _service.Delete(notaDeEnvio);
            if (!response) Console.WriteLine("Algo falló al eliminar Nota de Envío de la base de datos");
        }
        private static bool EditNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = _service.Edit(notaDeEnvio);
            if (!response) Console.WriteLine("Algo falló al editar Nota de Envío en base de datos");
            return response;
        }
        private static NotaDeEnvio EditNotaDeEnvioAgregar(NotaDeEnvio nota)
        {
            if (nota == null || nota.ProdVendidos == null || nota.ProdVendidos.Count == 0) return null;
            nota.ImporteTotal = ExtraerImporteDeNotaDeEnvio(nota.ProdVendidos);
            nota.Detalle = ExtraerDetalleDeNotaDeEnvio(nota.ProdVendidos);
            bool success = editNotaDeEnvio(nota);
            if (!success) return null;
            return nota;
        }
        private static NotaDeEnvio EditNotaDeEnvioQuitar(NotaDeEnvio notaDeEnvio, ProdVendido prodVendidoAEliminar)
        {
            deleteProdVendido(prodVendidoAEliminar);
            var lstAuxiliar = new List<ProdVendido>();
            foreach (ProdVendido prodVendido in notaDeEnvio.ProdVendidos)
            {
                if (prodVendido.ProductoId != prodVendidoAEliminar.ProductoId) lstAuxiliar.Add(prodVendido);
            }
            notaDeEnvio.ProdVendidos = lstAuxiliar;
            notaDeEnvio.ImporteTotal = ExtraerImporteDeNotaDeEnvio(lstAuxiliar);
            notaDeEnvio.Detalle = ExtraerDetalleDeNotaDeEnvio(lstAuxiliar);
            editNotaDeEnvio(notaDeEnvio);
            return notaDeEnvio;
        }
        private static string ExtraerDetalleDeNotaDeEnvio(ICollection<ProdVendido> lstProdVendidos)
        {
            string detalle = "";
            if (lstProdVendidos == null || lstProdVendidos.Count == 0) return "";
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                string description = editDescripcion(prodVendido.Descripcion);
                detalle += prodVendido.Cantidad.ToString() + "x " + description + ". ";
            }
            return detalle;
        }
        private static decimal ExtraerImporteDeNotaDeEnvio(ICollection<ProdVendido> lstProdVendidos)
        {
            decimal importeTotal = 0;
            if (lstProdVendidos == null || lstProdVendidos.Count == 0) return 0;
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                importeTotal += prodVendido.Cantidad * prodVendido.Precio;
            }
            return importeTotal;
        }
        private static NotaDeEnvio GetNotaDeEnvio(long id)
        {
            return _service.Get(id);
        }
        private static List<NotaDeEnvio> GetNotaDeEnvios()
        {
            return _service.GetAll();
        }
    }
}
