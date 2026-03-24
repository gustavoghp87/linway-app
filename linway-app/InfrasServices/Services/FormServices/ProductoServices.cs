using linway_app.Services.Interfaces;
using Models;
using Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class ProductoServices: IProductoServices
    {
        private readonly IServicesBase<Producto> _services;
        public ProductoServices(IServicesBase<Producto> services)
        {
            _services = services;
        }
        public void AddProducto(Producto producto)
        {
            while (producto.Nombre.Contains("'"))
            {
                producto.Nombre = producto.Nombre.Replace(char.Parse("'"), '"');
            }
            _services.Add(producto);
        }
        public void DeleteProducto(Producto producto)
        {
            producto.Estado = "Eliminado";
            _services.Edit(producto);
            //_services.Delete(producto);
        }
        public void EditProducto(Producto producto)
        {
            _services.Edit(producto);
        }
        public async Task<Producto> GetProductoPorIdAsync(long productId)
        {
            Producto producto = await _services.GetAsync(productId);
            return producto;
        }
        public async Task<Producto> GetProductoPorNombreAsync(string nombre)
        {
            List<Producto> productos = await GetProductosAsync();
            Producto producto = productos.Find(x => x.Nombre.ToLower().Contains(nombre.ToLower()) && x.Estado != "Eliminado");
            return producto;
        }
        public async Task<Producto> GetProductoPorNombreExactoAsync(string nombre)
        {
            List<Producto> productos = await GetProductosAsync();
            Producto producto = productos.Find(x => x.Nombre.Equals(nombre) && x.Estado != "Eliminado");
            return producto;
        }
        public async Task<List<Producto>> GetProductosAsync()
        {
            List<Producto> producto = await _services.GetAllAsync();
            return producto;
        }
        #region static methods
        public static bool IsACobrar(Producto producto)
        {
            bool isACobrar = producto != null
                && producto.Tipo == TipoProducto.Saldo.ToString()
                && producto.SubTipo != null
                && producto.SubTipo == TipoSaldo.ACobrar.ToString();
            return isACobrar;
        }
        public static bool IsBlanqueador(Producto product)
        {
            bool isBlanqueador = product != null
                && product.Tipo == TipoProducto.Polvo.ToString()
                && product.SubTipo != null
                && product.SubTipo == TipoPolvo.Blanqueador.ToString();
            return isBlanqueador;
        }
        public static bool IsLiquido(Producto product)
        {
            bool isLiquido = product != null && product.Tipo == TipoProducto.Líquido.ToString();
            return isLiquido;
        }
        public static bool IsNegativePrice(Producto product)
        {
            bool isNegativePrice = product.Tipo == TipoProducto.Saldo.ToString()
                && (product.SubTipo == TipoSaldo.Bonificacion.ToString()
                    || product.SubTipo == TipoSaldo.Devolucion.ToString()
                    || product.SubTipo == TipoSaldo.SaldoAFavor.ToString()
                );
            return isNegativePrice;
        }
        public static bool IsPolvo(Producto product)
        {
            bool isPolvo = product != null && product.Tipo == TipoProducto.Polvo.ToString();
            return isPolvo;
        }
        public static bool IsProducto(Producto producto)
        {
            bool isProducto = producto.Tipo != TipoProducto.Saldo.ToString();
            return isProducto;
        }
        public static bool IsSaldo(Producto product)
        {
            bool isSaldo = product != null && product.Tipo == TipoProducto.Saldo.ToString();
            return isSaldo;
        }
        #endregion
    }
}
