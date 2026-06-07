using AppServices.Interfaces;
using Models;
using System.Threading.Tasks;

namespace AppServices.UseCases
{
    public class EditarProductoUseCase : IEditarProductoUseCase
    {
        private readonly IProductoServices _productoServices;
        private readonly ISavingServices _savingServices;
        public EditarProductoUseCase(IProductoServices productoServices, ISavingServices savingServices)
        {
            _productoServices = productoServices;
            _savingServices = savingServices;
        }
        public async Task<UseCaseResponse> ExecuteAsync(string nombre, decimal precio, string tipo, string subTipo)
        {
            var producto = new Producto
            {
                Nombre = nombre,
                Precio = precio,
                Tipo = tipo
            };
            if (subTipo != "")
            {
                producto.SubTipo = subTipo;
            }
            _productoServices.Edit(producto);
            //
            bool guardado = await _savingServices.SaveAsync();
            if (!guardado)
            {
                _savingServices.DiscardChanges();
                return UseCaseResponse.Fail("No se hicieron cambios");
            }
            return UseCaseResponse.Ok();
        }
    }
}
