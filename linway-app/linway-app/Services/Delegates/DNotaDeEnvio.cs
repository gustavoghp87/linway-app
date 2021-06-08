using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DNotaDeEnvio
    {
        public delegate List<NotaDeEnvio> DGetNotaDeEnvios();
        public delegate NotaDeEnvio DGetNotaDeEnvio(long id);
        public delegate long DAddNotaDeEnvioReturnId(NotaDeEnvio notaDeEnvio);
        public delegate void DDeleteNotaDeEnvio(NotaDeEnvio notaDeEnvio);
        public delegate void DEditNotaDeEnvio(NotaDeEnvio notaDeEnvio);

        public readonly static DGetNotaDeEnvios getNotaDeEnvios = new DGetNotaDeEnvios(GetNotaDeEnvios);
        public readonly static DGetNotaDeEnvio getNotaDeEnvio = new DGetNotaDeEnvio(GetNotaDeEnvio);
        public readonly static DAddNotaDeEnvioReturnId addNotaDeEnvioReturnId
            = new DAddNotaDeEnvioReturnId(AddNotaDeEnvioReturnId);
        public readonly static DDeleteNotaDeEnvio deleteNotaDeEnvio = new DDeleteNotaDeEnvio(DeleteNotaDeEnvio);
        public readonly static DEditNotaDeEnvio editNotaDeEnvio = new DEditNotaDeEnvio(EditNotaDeEnvio);

        private static List<NotaDeEnvio> GetNotaDeEnvios()
        {
            return Form1._servNotaDeEnvio.GetAll();
        }
        private static NotaDeEnvio GetNotaDeEnvio(long id)
        {
            return Form1._servNotaDeEnvio.Get(id);
        }
        private static long AddNotaDeEnvioReturnId(NotaDeEnvio notaDeEnvio)
        {
            long response = Form1._servNotaDeEnvio.AddAndGetId(notaDeEnvio);
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
    }
}
