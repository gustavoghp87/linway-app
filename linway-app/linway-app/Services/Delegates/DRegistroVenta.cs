﻿using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DRegistroVenta
    {
        public delegate bool DAddRegistroVenta(RegistroVenta registroVenta);
        public delegate long DAddRegistroVentaReturnId(RegistroVenta registroVenta);
        public delegate void DDeleteRegistroVenta(RegistroVenta registroVenta);
        public delegate RegistroVenta DGetRegistroVenta(long registroVentaId);
        public delegate List<RegistroVenta> DGetRegistroVentas();
        public delegate void DEditRegistroVentas(RegistroVenta registroVenta);

        public readonly static DAddRegistroVenta addRegistroVenta
            = new DAddRegistroVenta(AddRegistroVenta);
        public readonly static DAddRegistroVentaReturnId addRegistroVentaReturnId
            = new DAddRegistroVentaReturnId(AddRegistroVentaReturnId);
        public readonly static DDeleteRegistroVenta deleteRegistroVenta
            = new DDeleteRegistroVenta(DeleteRegistroVenta);
        public readonly static DGetRegistroVenta getRegistroVenta
            = new DGetRegistroVenta(GetRegistroVenta);
        public readonly static DGetRegistroVentas getRegistroVentas
            = new DGetRegistroVentas(GetRegistroVentas);
        public readonly static DEditRegistroVentas editRegistroVenta
            = new DEditRegistroVentas(EditRegistroVenta);

        private static bool AddRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = Form1._servRegistroVenta.Add(registroVenta);
            if (!response) MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
            return response;
        }
        private static long AddRegistroVentaReturnId(RegistroVenta registroVenta)
        {
            try
            {
                bool response = AddRegistroVenta(registroVenta);
                if (!response)
                {
                    MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
                    return 0;
                }
                var lst = GetRegistroVentas();
                return lst.Last().Id;
            }
            catch
            {
                MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
                return 0;
            }
        }
        private static void DeleteRegistroVenta(RegistroVenta registroVenta)
        {
            bool success = Form1._servRegistroVenta.Delete(registroVenta);
            if (!success) MessageBox.Show("Algo falló al eliminar Registro de Venta a base de datos");
        }
        private static RegistroVenta GetRegistroVenta(long registroVentaId)
        {
            return Form1._servRegistroVenta.Get(registroVentaId);
        }
        private static List<RegistroVenta> GetRegistroVentas()
        {
            return Form1._servRegistroVenta.GetAll();
        }
        private static void EditRegistroVenta(RegistroVenta registroVenta)
        {
            bool response = Form1._servRegistroVenta.Edit(registroVenta);
            if (!response) MessageBox.Show("Algo falló al editar Registro de Venta en la base de datos");
        }
    }
}
