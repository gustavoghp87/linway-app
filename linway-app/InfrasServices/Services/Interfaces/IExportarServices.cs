using Models;
using System.Collections.Generic;

namespace linway_app.Services.Interfaces
{
    public interface IExportarServices
    {
        void ExportarNotas(ICollection<NotaDeEnvio> notas);
        void ExportarReparto(Reparto reparto);
        void ExportarVentas(ICollection<Venta> lstVentas);
    }
}
