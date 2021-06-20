using linway_app.Forms;
using linway_app.Models;
using linway_app.Models.Enums;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DProducto
    {
        public readonly static Action<Producto> addProducto = AddProducto;
        public readonly static Action<Producto> deleteProducto = DeleteProducto;
        public readonly static Action<Producto> editProducto = EditProducto;
        public readonly static Func<Producto, bool> esProducto = EsProducto;
        public readonly static Func<long, Producto> getProducto = GetProducto;
        public readonly static Func<string, Producto> getProductoPorNombre = GetProductoPorNombre;
        public readonly static Func<string, Producto> getProductoPorNombreExacto = GetProductoPorNombreExacto;
        public readonly static Func<List<Producto>> getProductos = GetProductos;

        private static readonly IServiceBase<Producto> _service = Form1._servProducto;
        private static void AddProducto(Producto producto)
        {
            while(producto.Nombre.Contains("'")) producto.Nombre = producto.Nombre.Replace(char.Parse("'"), '"');
            bool response = _service.Add(producto);
            if (!response) MessageBox.Show("Algo falló al guardar Producto en la base de datos");
        }
        private static void DeleteProducto(Producto producto)
        {
            bool response = _service.Delete(producto);
            if (!response) MessageBox.Show("Algo falló al eliminar Producto de la base de datos");
        }
        private static void EditProducto(Producto producto)
        {
            bool response = _service.Edit(producto);
            if (!response) MessageBox.Show("Algo falló al editar Producto en la base de datos");
        }
        private static bool EsProducto(Producto producto)
        {
            return producto.Tipo != TipoProducto.Saldo.ToString();
        }
        private static Producto GetProducto(long productId)
        {
            return _service.Get(productId);
        }
        private static Producto GetProductoPorNombre(string nombre)
        {
            return GetProductos().Find(x => x.Nombre.ToLower().Contains(nombre.ToLower()));
        }
        private static Producto GetProductoPorNombreExacto(string nombre)
        {
            return GetProductos().Find(x => x.Nombre.Equals(nombre));
        }
        private static List<Producto> GetProductos()
        {
            return _service.GetAll();
        }
    }
}
