using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DRecibo
    {
        public delegate bool DAddRecibo(Recibo recibo);
        public delegate long DAddReciboReturnId(Recibo recibo);
        public delegate void DDeleteRecibo(Recibo recibo);
        public delegate void DEditRecibo(Recibo recibo);
        public delegate Recibo DGetRecibo(long reciboId);
        public delegate List<Recibo> DGetRecibos();
        public delegate decimal DCalcularTotalRecibo(Recibo recibo);

        public readonly static DAddRecibo addRecibo = new DAddRecibo(AddRecibo);
        public readonly static DAddReciboReturnId addReciboReturnId = new DAddReciboReturnId(AddReciboReturnId);
        public readonly static DDeleteRecibo deleteRecibo = new DDeleteRecibo(DeleteRecibo);
        public readonly static DEditRecibo editRecibo = new DEditRecibo(EditRecibo);
        public readonly static DGetRecibo getRecibo = new DGetRecibo(GetRecibo);
        public readonly static DGetRecibos getRecibos = new DGetRecibos(GetRecibos);
        public readonly static DCalcularTotalRecibo calcularTotalRecibo = new DCalcularTotalRecibo(CalcularTotalRecibo);

        private static bool AddRecibo(Recibo recibo)
        {
            bool response = Form1._servRecibo.Add(recibo);
            if (!response) MessageBox.Show("Algo falló al guardar el Recibo en la base de datos");
            return response;
        }
        private static long AddReciboReturnId(Recibo recibo)
        {
            bool response = AddRecibo(recibo);
            if (!response) return 0;
            var response1 = GetRecibos();
            var last = response1[response1.Count - 1];
            return last.Id;
        }
        private static void DeleteRecibo(Recibo recibo)
        {
            bool response = Form1._servRecibo.Delete(recibo);
            if (!response) MessageBox.Show("Algo falló al eliminar el Recibo de la base de datos");
        }
        private static void EditRecibo(Recibo recibo)
        {
            bool response = Form1._servRecibo.Edit(recibo);
            if (!response) MessageBox.Show("Algo falló al editar el Recibo de la base de datos");
        }
        private static Recibo GetRecibo(long reciboId)
        {
            return Form1._servRecibo.Get(reciboId);
        }
        private static List<Recibo> GetRecibos()
        {
            return Form1._servRecibo.GetAll();
        }
        private static decimal CalcularTotalRecibo(Recibo recibo)
        {
            decimal subTo = 0;
            if (recibo.DetalleRecibos != null && recibo.DetalleRecibos.Count != 0)
                foreach (DetalleRecibo detalle in recibo.DetalleRecibos)
                {
                    subTo += detalle.Importe;
                }
            return subTo;
        }
    }
}
