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
        public void AddProdVendido(ProdVendido prodVendido)
        {
            _services.Add(prodVendido);
        }
        public void AddProdVendidos(ICollection<ProdVendido> prodVendidos)
        {
            _services.AddMany(prodVendidos);
        }
        public void DeleteProdVendido(ProdVendido prodVendido)
        {
            _services.Delete(prodVendido);
        }
        public void EditProdVendido(ProdVendido prodVendido)
        {
            _services.Edit(prodVendido);;
        }
        public void EditProdVendidos(ICollection<ProdVendido> prodVendidos)
        {
            _services.EditMany(prodVendidos);
        }
        public async Task<List<ProdVendido>> GetProdVendidosAsync()
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
