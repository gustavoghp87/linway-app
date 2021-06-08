﻿using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DProductos
    {
        public delegate List<Producto> DGetProductos();
        public delegate Producto DGetProducto(long productId);
        public delegate Producto DGetProductoPorNombre(string nombre);
        public delegate Producto DGetProductoPorNombreExacto(string nombre);
        public delegate void DAddProducto(Producto Producto);
        public delegate void DEditProducto(Producto producto);
        public delegate void DDeleteProducto(Producto producto);

        public readonly static DGetProductos getProductos = new DGetProductos(GetProductos);
        public readonly static DGetProducto getProducto = new DGetProducto(GetProducto);
        public readonly static DGetProductoPorNombre getProductoPorNombre
            = new DGetProductoPorNombre(GetProductoPorNombre);
        public readonly static DGetProductoPorNombreExacto getProductoPorNombreExacto
            = new DGetProductoPorNombreExacto(GetProductoPorNombreExacto);
        public readonly static DAddProducto addProducto = new DAddProducto(AddProducto);
        public readonly static DEditProducto editProducto = new DEditProducto(EditProducto);
        public readonly static DDeleteProducto deleteProducto = new DDeleteProducto(DeleteProducto);

        private static List<Producto> GetProductos()
        {
            return Form1._servProducto.GetAll();
        }
        private static Producto GetProducto(long productId)
        {
            return Form1._servProducto.Get(productId);
        }
        private static Producto GetProductoPorNombre(string nombre)
        {
            return GetProductos().Find(x => x.Nombre.ToLower().Contains(nombre.ToLower()));
        }
        private static Producto GetProductoPorNombreExacto(string nombre)
        {
            return GetProductos().Find(x => x.Nombre.Equals(nombre));
        }
        private static void AddProducto(Producto producto)
        {
            bool response = Form1._servProducto.Add(producto);
            if (!response) MessageBox.Show("Algo falló al guardar Producto en la base de datos");
        }
        private static void EditProducto(Producto producto)
        {
            bool response = Form1._servProducto.Edit(producto);
            if (!response) MessageBox.Show("Algo falló al editar Producto en la base de datos");
        }
        private static void DeleteProducto(Producto producto)
        {
            bool response = Form1._servProducto.Delete(producto);
            if (!response) MessageBox.Show("Algo falló al eliminar Producto de la base de datos");
        }
    }
}