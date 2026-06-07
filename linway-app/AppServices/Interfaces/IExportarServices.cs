using Models;
using System.Collections.Generic;

namespace AppServices.Interfaces
{
    public interface IExportarServices
    {
        void ExportarNotas(ICollection<NotaDeEnvio> notas);
        void ExportarReparto(Reparto reparto);
        void ExportarVentas(ICollection<Venta> lstVentas);
    }
}
