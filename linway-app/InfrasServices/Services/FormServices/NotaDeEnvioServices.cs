using linway_app.Services.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class NotaDeEnvioServices: INotaDeEnvioServices
    {
        private readonly IServicesBase<NotaDeEnvio> _services;
        
        public NotaDeEnvioServices(IServicesBase<NotaDeEnvio> services)
        {
            _services = services;
        }
        public void AddNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            _services.Add(notaDeEnvio);
        }
        public void DeleteNotas(ICollection<NotaDeEnvio> notas)
        {
            _services.DeleteMany(notas);
        }

        public void EditNotaDeEnvio(NotaDeEnvio notaDeEnvio)
        {
            _services.Edit(notaDeEnvio);
        }
        public void EditValores(NotaDeEnvio nota)
        {
            nota.ImporteTotal = ExtraerImporteDeNotaDeEnvio(nota.ProdVendidos);
            nota.Detalle = ExtraerDetalleDeNotaDeEnvio(nota.ProdVendidos);
            EditNotaDeEnvio(nota);
        }
        public async Task<NotaDeEnvio> GetNotaDeEnvioPorIdAsync(long id)
        {
            NotaDeEnvio nota = await _services.GetAsync(id);
            return nota;
        }
        public async Task<List<NotaDeEnvio>> GetNotaDeEnviosAsync()
        {
            List<NotaDeEnvio> notas = await _services.GetAllAsync();
            return notas;
        }
        #region static methods
        public static string ExtraerDetalleDeNotaDeEnvio(ICollection<ProdVendido> lstProdVendidos)
        {
            string detalle = "";
            if (lstProdVendidos == null || lstProdVendidos.Count == 0)
            {
                return "";
            }
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                string description = ProdVendidoServices.GetEditedDescripcion(prodVendido.Descripcion);
                detalle += prodVendido.Cantidad.ToString() + "x " + description + ". ";
            }
            return detalle;
        }
        public static decimal ExtraerImporteDeNotaDeEnvio(ICollection<ProdVendido> lstProdVendidos)
        {
            decimal importeTotal = 0;
            if (lstProdVendidos == null || lstProdVendidos.Count == 0)
            {
                return 0;
            }
            foreach (ProdVendido prodVendido in lstProdVendidos)
            {
                importeTotal += prodVendido.Cantidad * prodVendido.Precio;
            }
            return importeTotal;
        }
        #endregion
    }
}
