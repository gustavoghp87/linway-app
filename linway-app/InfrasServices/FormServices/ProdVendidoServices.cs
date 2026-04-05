using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class ProdVendidoServices: IProdVendidoServices
    {
        private readonly IServicesBase<ProdVendido> _services;
        public ProdVendidoServices(IServicesBase<ProdVendido> services)
        {
            _services = services;
        }
        public void Add(ProdVendido prodVendido)
        {
            _services.Add(prodVendido);
        }
        public void AddMany(ICollection<ProdVendido> prodVendidos)
        {
            _services.AddMany(prodVendidos);
        }
        public void Delete(ProdVendido prodVendido)
        {
            _services.Delete(prodVendido);
        }
        public void DeleteMany(ICollection<ProdVendido> prodVendidos)
        {
            _services.DeleteMany(prodVendidos);
        }
        public void EditOrDeleteMany(List<ProdVendido> prodVendidos)
        {
            var prodVendidosAEliminar = new List<ProdVendido>();
            var prodVendidosAEditar = new List<ProdVendido>();
            foreach (var prodVendido in prodVendidos)
            {
                if (prodVendido.NotaDeEnvioId == null && prodVendido.RegistroVentaId == null && prodVendido.PedidoId == null)
                {
                    prodVendidosAEliminar.Add(prodVendido);
                }
                else
                {
                    prodVendidosAEditar.Add(prodVendido);
                }
            }
            _services.DeleteMany(prodVendidosAEliminar);
            _services.EditMany(prodVendidosAEditar);
        }
        public void Edit(ProdVendido prodVendido)
        {
            _services.Edit(prodVendido);;
        }
        public void EditMany(ICollection<ProdVendido> prodVendidos)
        {
            _services.EditMany(prodVendidos);
        }
        public async Task<List<ProdVendido>> GetAllAsync()
        {
            List<ProdVendido> prodVendidos = await _services.GetAllAsync();
            return prodVendidos;
        }
        #region static methods
        public static string GetEditedDescripcion(string description)
        {
            if (description.Contains("-"))
            {
                description = description.Substring(0, description.IndexOf("-") - 1);
            };
            if (description.Contains("."))
            {
                description = description.Substring(0, description.IndexOf(".") - 1);
            };
            return description;
        }
        #endregion
    }
}
