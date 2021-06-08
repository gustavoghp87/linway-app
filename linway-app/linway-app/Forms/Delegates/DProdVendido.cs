using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    public class DProdVendido
    {
        public delegate void DAgregarProdVendido(ProdVendido prodVendido);
        public delegate List<ProdVendido> DGetProdVendidos();
        public delegate ProdVendido DGetProdVendidoPorNombre(string descripcion);
        public delegate ProdVendido DGetProdVendidoPorNombreExacto(string descripcion);
        public delegate void DDeleteProdVendido(ProdVendido prodVendido);

        public readonly static DAgregarProdVendido addProdVendido
            = new DAgregarProdVendido(AddProdVendido);
        public readonly static DGetProdVendidos getProdVendidos
            = new DGetProdVendidos(GetProdVendidos);
        public readonly static DGetProdVendidoPorNombre getProdVendidoPorNombre
            = new DGetProdVendidoPorNombre(GetProdVendidoPorNombre);
        public readonly static DGetProdVendidoPorNombreExacto getProdVendidoPorNombreExacto
            = new DGetProdVendidoPorNombreExacto(GetProdVendidoPorNombreExacto);
        public readonly static DDeleteProdVendido deleteProdVendido
            = new DDeleteProdVendido(DeleteProdVendido);

        private static void AddProdVendido(ProdVendido prodVendido)
        {
            bool response = Form1._servProdVendido.Add(prodVendido);
            if (!response) MessageBox.Show("Algo falló al agregar Producto Vendido a base de datos");
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
        private static void DeleteProdVendido(ProdVendido prodVendido)
        {
            bool response = Form1._servProdVendido.Delete(prodVendido);
            if (!response) MessageBox.Show("Algo falló al eliminar Producto Vendido de la base de datos");
        }
    }
}
