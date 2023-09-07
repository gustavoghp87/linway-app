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
        public readonly static Predicate<Producto> addProducto = AddProducto;
        public readonly static Predicate<Producto> deleteProducto = DeleteProducto;
        public readonly static Predicate<Producto> editProducto = EditProducto;
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

        private static bool AddProducto(Producto producto)
        {
            while(producto.Nombre.Contains("'")) producto.Nombre = producto.Nombre.Replace(char.Parse("'"), '"');
            bool success = _service.Add(producto);
            return success;
        }
        private static bool DeleteProducto(Producto producto)
        {
            bool success = _service.Delete(producto);
            return success;
        }
        private static bool EditProducto(Producto producto)
        {
            bool success = _service.Edit(producto);
            return success;
        }
        private static bool EsProducto(Producto producto)
        {
            bool esProducto = producto.Tipo != TipoProducto.Saldo.ToString();
            return esProducto;
        }
        private static Producto GetProducto(long productId)
        {
            Producto producto = _service.Get(productId);
            return producto;
        }
        private static Producto GetProductoPorNombre(string nombre)
        {
            Producto producto = GetProductos().Find(x => x.Nombre.ToLower().Contains(nombre.ToLower()) && x.Estado != null && x.Estado != "Eliminado");
            return producto;
        }
        private static Producto GetProductoPorNombreExacto(string nombre)
        {
            Producto producto = GetProductos().Find(x => x.Nombre.Equals(nombre) && x.Estado != null && x.Estado != "Eliminado");
            return producto;
        }
        private static List<Producto> GetProductos()
        {
            List<Producto> producto = _service.GetAll();
            return producto;
        }
        private static bool IsACobrar(Producto producto)
        {
            bool isACobrar = producto != null
                && producto.Tipo == TipoProducto.Saldo.ToString()
                && producto.SubTipo != null
                && producto.SubTipo == TipoSaldo.ACobrar.ToString();
            return isACobrar;
        }
        private static bool IsBlanqueador(Producto product)
        {
            bool isBlanqueador = product != null
                && product.Tipo == TipoProducto.Polvo.ToString()
                && product.SubTipo != null
                && product.SubTipo == TipoPolvo.Blanqueador.ToString();
            return isBlanqueador;
        }
        private static bool IsLiquido(Producto product)
        {
            bool isLiquido = product != null && product.Tipo == TipoProducto.Líquido.ToString();
            return isLiquido;
        }
        private static bool IsNegativePrice(Producto product)
        {
            bool isNegativePrice = product.Tipo == TipoProducto.Saldo.ToString()
                && (product.SubTipo == TipoSaldo.Bonificacion.ToString()
                    || product.SubTipo == TipoSaldo.Devolucion.ToString()
                    || product.SubTipo == TipoSaldo.SaldoAFavor.ToString()
                );
            return isNegativePrice;
        }
        private static bool IsPolvo(Producto product)
        {
            bool isPolvo = product != null && product.Tipo == TipoProducto.Polvo.ToString();
            return isPolvo;
        }
        private static bool IsSaldo(Producto product)
        {
            bool isSaldo = product != null && product.Tipo == TipoProducto.Saldo.ToString();
            return isSaldo;
        }
    }
}
