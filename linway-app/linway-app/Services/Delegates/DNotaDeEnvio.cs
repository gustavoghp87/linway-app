using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DNotaDeEnvio
    {
        public delegate long DAddNotaDeEnvioReturnId(NotaDeEnvio notaDeEnvio);
        public delegate void DDeleteNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        public delegate void DEditNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        public delegate NotaDeEnvio DGetNotaDeEnvio(long id);
        public delegate List<NotaDeEnvio> DGetNotaDeEnvios();
        public delegate double DExtraerImporte(List<ProdVendido> lstProdVendidos);
        public delegate string DExtraerDetalle(List<ProdVendido> lstProdVendidos);
        public delegate NotaDeEnvio DModificar(NotaDeEnvio notaDeEnvio, List<ProdVendido> lstProdVendidos);

        public readonly static DAddNotaDeEnvioReturnId addNotaDeEnvioReturnId
            = new DAddNotaDeEnvioReturnId(AddNotaDeEnvioReturnId);
        public readonly static DDeleteNotaDeEnvio deleteNotaDeEnvio
            = new DDeleteNotaDeEnvio(DeleteNotaDeEnvio);
        public readonly static DEditNotaDeEnvio editNotaDeEnvio
            = new DEditNotaDeEnvio(EditNotaDeEnvio);
        public readonly static DGetNotaDeEnvio getNotaDeEnvio
            = new DGetNotaDeEnvio(GetNotaDeEnvio);
        public readonly static DGetNotaDeEnvios getNotaDeEnvios
            = new DGetNotaDeEnvios(GetNotaDeEnvios);
        public readonly static DExtraerImporte extraerImporteDeNotaDeEnvio
            = new DExtraerImporte(ExtraerImporteDeNotaDeEnvio);
        public readonly static DExtraerDetalle extraerDetalleDeNotaDeEnvio
            = new DExtraerDetalle(ExtraerDetalleDeNotaDeEnvio);
        public readonly static DModificar modificarNotaDeEnvio
            = new DModificar(ModificarNotaDeEnvio);

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
        private static void EditNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            bool response = Form1._servNotaDeEnvio.Edit(notaDeEnvio);
            if (!response) MessageBox.Show("Algo falló al editar Nota de Envío en base de datos");
        }
        private static NotaDeEnvio GetNotaDeEnvio(long id)
        {
            return Form1._servNotaDeEnvio.Get(id);
        }
        private static List<NotaDeEnvio> GetNotaDeEnvios()
        {
            return Form1._servNotaDeEnvio.GetAll();
        }

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
        private static double ExtraerImporteDeNotaDeEnvio(List<ProdVendido> lstProdVendidos)
        {
            double subTo = 0;
            if (lstProdVendidos != null && lstProdVendidos.Count != 0)
            {
                foreach (ProdVendido prodVendido in lstProdVendidos)
                {
                    subTo += prodVendido.Precio;
                }
            }
            return subTo;
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
        private static NotaDeEnvio ModificarNotaDeEnvio(NotaDeEnvio notaDeEnvio, List<ProdVendido> lstProdVendidos)
        {
            notaDeEnvio.ProdVendidos = lstProdVendidos;
            double subTo = 0;
            string deta = "";
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                subTo += prodVendido.Precio;
                deta = deta + prodVendido.Cantidad.ToString() + "x " + prodVendido.Descripcion + ". ";
            }
            notaDeEnvio.ImporteTotal = subTo;
            notaDeEnvio.Detalle = deta;
            notaDeEnvio.Impresa = 0;
            return notaDeEnvio;
        }
    }
}
