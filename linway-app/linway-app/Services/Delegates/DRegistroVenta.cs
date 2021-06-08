﻿using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DRegistroVenta
    {
        public delegate long DAddRegistroVenta(RegistroVenta registroVenta);
        public delegate List<RegistroVenta> DGetRegistroVentas();
        public delegate void DEditRegistroVentas(RegistroVenta registroVenta);

        public readonly static DAddRegistroVenta addRegistroVentaReturnId
            = new DAddRegistroVenta(AddRegistroVentaReturnId);
        public readonly static DGetRegistroVentas getRegistroVentas
            = new DGetRegistroVentas(GetRegistroVentas);
        public readonly static DEditRegistroVentas editRegistroVenta
            = new DEditRegistroVentas(EditRegistroVenta);

        private static long AddRegistroVentaReturnId(RegistroVenta registroVenta)
        {
            long response = Form1._servRegistroVenta.AddAndGetId(registroVenta);
            if (response == 0) MessageBox.Show("Algo falló al agregar Registro de Venta a base de datos");
            return response;
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