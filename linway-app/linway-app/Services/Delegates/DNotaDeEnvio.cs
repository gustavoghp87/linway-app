using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DNotaDeEnvio
    {
        public delegate void DAddNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        public delegate long DAddNotaDeEnvioReturnId(NotaDeEnvio notaDeEnvio);
        public delegate void DDeleteNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        public delegate bool DEditNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        public delegate NotaDeEnvio DEditNotaDeEnvioAgregar(NotaDeEnvio notaDeEnvio, List<ProdVendido> lstProdVendidos);
        public delegate NotaDeEnvio DEditNotaDeEnvioQuitar(NotaDeEnvio notaDeEnvio, ProdVendido nuevoProdVendido);
        public delegate string DExtraerDetalle(List<ProdVendido> lstProdVendidos);
        public delegate decimal DExtraerImporte(List<ProdVendido> lstProdVendidos);
        public delegate NotaDeEnvio DGetNotaDeEnvio(long id);
        public delegate List<NotaDeEnvio> DGetNotaDeEnvios();

        public readonly static DAddNotaDeEnvio addNotaDeEnvio
            = new DAddNotaDeEnvio(AddNotaDeEnvio);
        public readonly static DAddNotaDeEnvioReturnId addNotaDeEnvioReturnId
            = new DAddNotaDeEnvioReturnId(AddNotaDeEnvioReturnId);
        public readonly static DDeleteNotaDeEnvio deleteNotaDeEnvio
            = new DDeleteNotaDeEnvio(DeleteNotaDeEnvio);
        public readonly static DEditNotaDeEnvio editNotaDeEnvio
            = new DEditNotaDeEnvio(EditNotaDeEnvio);
        public readonly static DEditNotaDeEnvioAgregar editNotaDeEnvioAgregar
            = new DEditNotaDeEnvioAgregar(EditNotaDeEnvioAgregar);
        public readonly static DEditNotaDeEnvioQuitar editNotaDeEnvioQuitar
            = new DEditNotaDeEnvioQuitar(EditNotaDeEnvioQuitar);
        public readonly static DExtraerDetalle extraerDetalleDeNotaDeEnvio
            = new DExtraerDetalle(ExtraerDetalleDeNotaDeEnvio);
        public readonly static DExtraerImporte extraerImporteDeNotaDeEnvio
            = new DExtraerImporte(ExtraerImporteDeNotaDeEnvio);
        public readonly static DGetNotaDeEnvio getNotaDeEnvio
            = new DGetNotaDeEnvio(GetNotaDeEnvio);
        public readonly static DGetNotaDeEnvios getNotaDeEnvios
            = new DGetNotaDeEnvios(GetNotaDeEnvios);

        private static long AddAndGetId(NotaDeEnvio notaDeEnvio)
        {
            try
            {
                Form1._servNotaDeEnvio.Add(notaDeEnvio);
                List<NotaDeEnvio> lst = GetNotaDeEnvios();
                return lst.Last().Id;
            }
            catch
            {
                return 0;
            }
        }
        private static void AddNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = Form1._servNotaDeEnvio.Add(notaDeEnvio);
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
            bool response = Form1._servNotaDeEnvio.Delete(notaDeEnvio);
            if (!response) MessageBox.Show("Algo falló al eliminar Nota de Envío de la base de datos");
        }
        private static bool EditNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = Form1._servNotaDeEnvio.Edit(notaDeEnvio);
            if (!response) MessageBox.Show("Algo falló al editar Nota de Envío en base de datos");
            return response;
        }
        private static NotaDeEnvio EditNotaDeEnvioAgregar(NotaDeEnvio notaDeEnvio, List<ProdVendido> lstProdVendidos)
        {
            if (notaDeEnvio.ProdVendidos == null || notaDeEnvio.ProdVendidos.Count == 0)
            {
                notaDeEnvio.ProdVendidos = (ICollection<ProdVendido>)new List<ProdVendido>();
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
                    if (!exists)
                    {
                        notaDeEnvio.ProdVendidos.Add(prodVendidoNuevo);
                    }
                    notaDeEnvio.ImporteTotal += prodVendidoNuevo.Precio * prodVendidoNuevo.Cantidad;
                    notaDeEnvio.Detalle += prodVendidoNuevo.Cantidad.ToString() + "x " + prodVendidoNuevo.Descripcion + ". ";
                }
            }
            bool success = EditNotaDeEnvio(notaDeEnvio);
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
            return Form1._servNotaDeEnvio.Get(id);
        }
        private static List<NotaDeEnvio> GetNotaDeEnvios()
        {
            return Form1._servNotaDeEnvio.GetAll();
        }
    }
}
