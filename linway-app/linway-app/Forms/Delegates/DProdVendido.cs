using linway_app.Models;
using System.Windows.Forms;

namespace linway_app.Forms.Delegates
{
    class DProdVendido
    {
        public delegate void DAgregarProdVendido(ProdVendido prodVendido);

        public readonly static DAgregarProdVendido addProdVendido
            = new DAgregarProdVendido(AddProdVendido);

        private static void AddProdVendido(ProdVendido prodVendido)
        {
            bool response = Form1._servProdVendido.Add(prodVendido);
            if (!response) MessageBox.Show("Algo falló al agregar Producto Vendido a base de datos");
        }
    }
}
