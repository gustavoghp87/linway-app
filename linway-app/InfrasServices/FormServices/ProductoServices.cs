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
        public void Add(Producto producto)
        {
            while (producto.Nombre.Contains("'"))
            {
                producto.Nombre = producto.Nombre.Replace(char.Parse("'"), '"');
            }
            _services.Add(producto);
        }
        public void Delete(Producto producto)
        {
            _services.Delete(producto);
        }
        public void Edit(Producto producto)
        {
            _services.Edit(producto);
        }
        public async Task<Producto> GetPorIdAsync(long productId)
        {
            Producto producto = await _services.GetAsync(productId);
            return producto;
        }
        public async Task<Producto> GetPorNombreAsync(string nombre)
        {
            List<Producto> productos = await GetAllAsync();
            Producto producto = productos.Find(x => x.Nombre.ToLower().Contains(nombre.ToLower()));
            return producto;
        }
        public async Task<Producto> GetPorNombreExactoAsync(string nombre)
        {
            List<Producto> productos = await GetAllAsync();
            Producto producto = productos.Find(x => x.Nombre.Equals(nombre));
            return producto;
        }
        public async Task<List<Producto>> GetAllAsync()
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
        public static bool IsBlanqueador(Producto producto)
        {
            bool isBlanqueador = producto != null
                && producto.Tipo == TipoProducto.Polvo.ToString()
                && producto.SubTipo != null
                && producto.SubTipo == TipoPolvo.Blanqueador.ToString();
            return isBlanqueador;
        }
        public static bool IsLiquido(Producto producto)
        {
            bool isLiquido = producto != null && producto.Tipo == TipoProducto.Líquido.ToString();
            return isLiquido;
        }
        public static bool IsNegativePrice(Producto producto)
        {
            bool isNegativePrice = producto.Tipo == TipoProducto.Saldo.ToString()
                && (producto.SubTipo == TipoSaldo.Bonificacion.ToString()
                    || producto.SubTipo == TipoSaldo.Devolucion.ToString()
                    || producto.SubTipo == TipoSaldo.SaldoAFavor.ToString()
                );
            return isNegativePrice;
        }
        public static bool IsPolvo(Producto producto)
        {
            bool isPolvo = producto != null && producto.Tipo == TipoProducto.Polvo.ToString();
            return isPolvo;
        }
        public static bool IsProducto(Producto producto)
        {
            bool isProducto = producto.Tipo != TipoProducto.Saldo.ToString();
            return isProducto;
        }
        public static bool IsSaldo(Producto producto)
        {
            bool isSaldo = producto != null && producto.Tipo == TipoProducto.Saldo.ToString();
            return isSaldo;
        }
        #endregion
    }
}
