using linway_app.Forms;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DNotaDeEnvio
    {
        public readonly static Action<NotaDeEnvio> addNotaDeEnvio = AddNotaDeEnvio;
        public readonly static Func<NotaDeEnvio, long> addNotaDeEnvioReturnId = AddNotaDeEnvioReturnId;
        public readonly static Action<NotaDeEnvio> deleteNotaDeEnvio = DeleteNotaDeEnvio;
        public readonly static Func<NotaDeEnvio, bool> editNotaDeEnvio = EditNotaDeEnvio;
        public readonly static Func<NotaDeEnvio, List<ProdVendido>, NotaDeEnvio> editNotaDeEnvioAgregar = EditNotaDeEnvioAgregar;
        public readonly static Func<NotaDeEnvio, ProdVendido, NotaDeEnvio> editNotaDeEnvioQuitar = EditNotaDeEnvioQuitar;
        public readonly static Func<List<ProdVendido>, string> extraerDetalleDeNotaDeEnvio = ExtraerDetalleDeNotaDeEnvio;
        public readonly static Func<List<ProdVendido>, decimal> extraerImporteDeNotaDeEnvio = ExtraerImporteDeNotaDeEnvio;
        public readonly static Func<long, NotaDeEnvio>  getNotaDeEnvio = GetNotaDeEnvio;
        public readonly static Func<List<NotaDeEnvio>> getNotaDeEnvios = GetNotaDeEnvios;

        private static readonly IServiceBase<NotaDeEnvio> _service = Form1._servNotaDeEnvio;
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
            if (!response) MessageBox.Show("Algo falló al agregar Nota de Envío a base de datos");
        }
        private static long AddNotaDeEnvioReturnId(NotaDeEnvio notaDeEnvio)
        {
            long response = AddAndGetId(notaDeEnvio);
            if (response == 0) MessageBox.Show("Algo falló al agregar Nota de Envío a base de datos");
            return response;
        }
        private static void DeleteNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = _service.Delete(notaDeEnvio);
            if (!response) MessageBox.Show("Algo falló al eliminar Nota de Envío de la base de datos");
        }
        private static bool EditNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = _service.Edit(notaDeEnvio);
            if (!response) MessageBox.Show("Algo falló al editar Nota de Envío en base de datos");
            return response;
        }
        private static NotaDeEnvio EditNotaDeEnvioAgregar(NotaDeEnvio notaDeEnvio, List<ProdVendido> lstProdVendidos)
        {
            if (notaDeEnvio.ProdVendidos == null || notaDeEnvio.ProdVendidos.Count == 0)
            {
                notaDeEnvio.ProdVendidos = new List<ProdVendido>();
                notaDeEnvio.ProdVendidos.ToList().AddRange((IEnumerable<ProdVendido>)lstProdVendidos);
                notaDeEnvio.ImporteTotal = ExtraerImporteDeNotaDeEnvio(lstProdVendidos);
                notaDeEnvio.Detalle = ExtraerDetalleDeNotaDeEnvio(lstProdVendidos);
            }
            else
            {
                notaDeEnvio.ImporteTotal = 0;
                notaDeEnvio.Detalle = "";
                foreach (ProdVendido prodVendidoNuevo in lstProdVendidos)
                {
                    bool exists = false;
                    foreach (ProdVendido prodVendidoNDC in notaDeEnvio.ProdVendidos)
                    {
                        if (prodVendidoNDC.Id == prodVendidoNuevo.ProductoId)
                        {
                            exists = true;
                            prodVendidoNDC.Cantidad += prodVendidoNuevo.Cantidad;
                        }
                    }
                    if (!exists) notaDeEnvio.ProdVendidos.Add(prodVendidoNuevo);
                    notaDeEnvio.ImporteTotal += prodVendidoNuevo.Precio * prodVendidoNuevo.Cantidad;
                    notaDeEnvio.Detalle += prodVendidoNuevo.Cantidad.ToString() + "x " + prodVendidoNuevo.Descripcion + ". ";
                }
            }
            bool success = editNotaDeEnvio(notaDeEnvio);
            if (!success) return null;
            return notaDeEnvio;
        }
        private static NotaDeEnvio EditNotaDeEnvioQuitar(NotaDeEnvio notaDeEnvio, ProdVendido nuevoProdVendido)
        {
            List<ProdVendido> lstAuxiliar = new List<ProdVendido>();
            foreach (ProdVendido prodVendido in notaDeEnvio.ProdVendidos)
            {
                if (prodVendido.Id != nuevoProdVendido.Id) lstAuxiliar.Add(prodVendido);
            }
            notaDeEnvio.ProdVendidos = lstAuxiliar;
            notaDeEnvio.ImporteTotal = ExtraerImporteDeNotaDeEnvio(lstAuxiliar);
            notaDeEnvio.Detalle = ExtraerDetalleDeNotaDeEnvio(lstAuxiliar);
            editNotaDeEnvio(notaDeEnvio);
            return notaDeEnvio;
        }
        private static string ExtraerDetalleDeNotaDeEnvio(List<ProdVendido> lstProdVendidos)
        {
            string detalle = "";
            if (lstProdVendidos != null && lstProdVendidos.Count != 0)
            {
                foreach (ProdVendido prodVendido in lstProdVendidos)
                {
                    detalle = detalle + prodVendido.Cantidad.ToString() + "x " + prodVendido.Descripcion + ". ";
                }
            }
            return detalle;
        }
        private static decimal ExtraerImporteDeNotaDeEnvio(List<ProdVendido> lstProdVendidos)
        {
            decimal importeTotal = 0;
            if (lstProdVendidos != null && lstProdVendidos.Count != 0)
            {
                foreach (ProdVendido prodVendido in lstProdVendidos)
                {
                    importeTotal += prodVendido.Cantidad * prodVendido.Precio;
                }
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
