using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;

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
        public static readonly Predicate<Producto> isACobrar = IsACobrar;
        public static readonly Predicate<Producto> isBlanqueador = IsBlanqueador;
        public static readonly Predicate<Producto> isLiquido = IsLiquido;
        public static readonly Predicate<Producto> isNegativePrice = IsNegativePrice;
        public static readonly Predicate<Producto> isPolvo = IsPolvo;
        public static readonly Predicate<Producto> isSaldo = IsSaldo;

        private static readonly IServiceBase<Producto> _service = ServicesObjects.ServProducto;

        private static void AddProducto(Producto producto)
        {
            while(producto.Nombre.Contains("'")) producto.Nombre = producto.Nombre.Replace(char.Parse("'"), '"');
            bool response = _service.Add(producto);
            if (!response) Console.WriteLine("Algo falló al guardar Producto en la base de datos");
        }
        private static void DeleteProducto(Producto producto)
        {
            bool response = _service.Delete(producto);
            if (!response) Console.WriteLine("Algo falló al eliminar Producto de la base de datos");
        }
        private static void EditProducto(Producto producto)
        {
            bool response = _service.Edit(producto);
            if (!response) Console.WriteLine("Algo falló al editar Producto en la base de datos");
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
            return GetProductos().Find(x => x.Nombre.ToLower().Contains(nombre.ToLower()) && x.Estado != null && x.Estado != "Eliminado");
        }
        private static Producto GetProductoPorNombreExacto(string nombre)
        {
            return GetProductos().Find(x => x.Nombre.Equals(nombre) && x.Estado != null && x.Estado != "Eliminado");
        }
        private static List<Producto> GetProductos()
        {
            return _service.GetAll();
        }
        private static bool IsACobrar(Producto product)
        {
            return product != null && product.Tipo == TipoProducto.Saldo.ToString()
                && product.SubTipo != null && product.SubTipo == TipoSaldo.ACobrar.ToString();
        }
        private static bool IsBlanqueador(Producto product)
        {
            return product != null
                && product.Tipo == TipoProducto.Polvo.ToString()
                && product.SubTipo != null
                && product.SubTipo == TipoPolvo.Blanqueador.ToString();
        }
        private static bool IsLiquido(Producto product)
        {
            return product != null && product.Tipo == TipoProducto.Líquido.ToString();
        }
        private static bool IsNegativePrice(Producto product)
        {
            return product.Tipo == TipoProducto.Saldo.ToString() && (
                product.SubTipo == TipoSaldo.Bonificacion.ToString()
                || product.SubTipo == TipoSaldo.Devolucion.ToString()
                || product.SubTipo == TipoSaldo.SaldoAFavor.ToString()
            );
        }
        private static bool IsPolvo(Producto product)
        {
            return product != null && product.Tipo == TipoProducto.Polvo.ToString();
        }
        private static bool IsSaldo(Producto product)
        {
            return product != null && product.Tipo == TipoProducto.Saldo.ToString();
        }
    }
}
