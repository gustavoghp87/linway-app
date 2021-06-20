using linway_app.Forms;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public class DProdVendido
    {
        public readonly static Action<ProdVendido> addProdVendido = AddProdVendido;
        public readonly static Action<ProdVendido> deleteProdVendido = DeleteProdVendido;
        public readonly static Action<ProdVendido> editProdVendido = EditProdVendido;
        public readonly static Func<List<ProdVendido>> getProdVendido = GetProdVendidos;
        public readonly static Func<string, ProdVendido> getProdVendidoPorNombre = GetProdVendidoPorNombre;
        public readonly static Func<string, ProdVendido> getProdVendidoPorNombreExacto = GetProdVendidoPorNombreExacto;

        private static readonly IServiceBase<ProdVendido> _service = Form1._servProdVendido;
        private static void AddProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Add(prodVendido);
            if (!response) MessageBox.Show("Algo falló al agregar Producto Vendido a la base de datos");
        }
        private static void DeleteProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Delete(prodVendido);
            if (!response) MessageBox.Show("Algo falló al eliminar Producto Vendido de la base de datos");
        }
        private static void EditProdVendido(ProdVendido prodVendido)
        {
            bool response = _service.Edit(prodVendido);
            if (!response) MessageBox.Show("Algo falló al editar Producto Vendido en la base de datos");
        }
        private static List<ProdVendido> GetProdVendidos()
        {
            return _service.GetAll();
        }
        private static ProdVendido GetProdVendidoPorNombre(string descripcion)
        {
            List<ProdVendido> lst = GetProdVendidos();
            return lst.Find(x => x.Descripcion.ToLower().Contains(descripcion.ToLower()));
        }
        private static ProdVendido GetProdVendidoPorNombreExacto(string descripcion)
        {
            List<ProdVendido> lst = GetProdVendidos();
            return lst.Find(x => x.Descripcion.Contains(descripcion));
        }
    }
}
