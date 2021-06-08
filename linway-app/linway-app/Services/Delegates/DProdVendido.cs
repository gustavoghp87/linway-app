using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public class DProdVendido
    {
        public delegate void DAgregarProdVendido(ProdVendido prodVendido);
        public delegate void DDeleteProdVendido(ProdVendido prodVendido);
        public delegate void DEditProdVendido(ProdVendido prodVendido);
        public delegate List<ProdVendido> DGetProdVendidos();
        public delegate ProdVendido DGetProdVendidoPorNombre(string descripcion);
        public delegate ProdVendido DGetProdVendidoPorNombreExacto(string descripcion);

        public readonly static DAgregarProdVendido addProdVendido
            = new DAgregarProdVendido(AddProdVendido);
        public readonly static DDeleteProdVendido deleteProdVendido
            = new DDeleteProdVendido(DeleteProdVendido);
        public readonly static DEditProdVendido editProdVendido
            = new DEditProdVendido(EditProdVendido);
        public readonly static DGetProdVendidos getProdVendidos
            = new DGetProdVendidos(GetProdVendidos);
        public readonly static DGetProdVendidoPorNombre getProdVendidoPorNombre
            = new DGetProdVendidoPorNombre(GetProdVendidoPorNombre);
        public readonly static DGetProdVendidoPorNombreExacto getProdVendidoPorNombreExacto
            = new DGetProdVendidoPorNombreExacto(GetProdVendidoPorNombreExacto);

        private static void AddProdVendido(ProdVendido prodVendido)
        {
            bool response = Form1._servProdVendido.Add(prodVendido);
            if (!response) MessageBox.Show("Algo falló al agregar Producto Vendido a la base de datos");
        }
        private static void DeleteProdVendido(ProdVendido prodVendido)
        {
            bool response = Form1._servProdVendido.Delete(prodVendido);
            if (!response) MessageBox.Show("Algo falló al eliminar Producto Vendido de la base de datos");
        }
        private static void EditProdVendido(ProdVendido prodVendido)
        {
            bool response = Form1._servProdVendido.Edit(prodVendido);
            if (!response) MessageBox.Show("Algo falló al editar Producto Vendido en la base de datos");
        }
        private static List<ProdVendido> GetProdVendidos()
        {
            return Form1._servProdVendido.GetAll();
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
