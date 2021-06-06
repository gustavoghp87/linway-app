using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    class ServicioNotaDeEnvio : IServicioNotaDeEnvio
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioNotaDeEnvio(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(NotaDeEnvio notaDeEnvio)
        {
            return _unitOfWork.RepoNotaDeEnvio.Add(notaDeEnvio);
        }
        public bool Delete(NotaDeEnvio notaDeEnvio)
        {
            notaDeEnvio.Estado = "Eliminado";
            return _unitOfWork.RepoNotaDeEnvio.Edit(notaDeEnvio);
        }
        public bool Edit(NotaDeEnvio notaDeEnvio)
        {
            return _unitOfWork.RepoNotaDeEnvio.Edit(notaDeEnvio);
        }
        public NotaDeEnvio Get(long id)
        {
            NotaDeEnvio notaDeEnvio = _unitOfWork.RepoNotaDeEnvio.Get(id);
            return notaDeEnvio == null || notaDeEnvio.Estado == null || notaDeEnvio.Estado == "Eliminado"
                ? null : notaDeEnvio;
        }
        public List<NotaDeEnvio> GetAll()
        {
            List<NotaDeEnvio> lst = _unitOfWork.RepoNotaDeEnvio.GetAll();
            lst = (from x
                   in lst
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }

        public long AddAndGetId(NotaDeEnvio notaDeEnvio)
        {
            try
            {
                Add(notaDeEnvio);
                List<NotaDeEnvio> lst = GetAll();
                return lst.Last().Id;
            }
            catch
            {
                return 0;
            }
        }
        public static double ExtraerImporte(List<ProdVendido> lstProdVendidos)
        {
            double subTo = 0;
            if (lstProdVendidos != null && lstProdVendidos.Count != 0)
            {
                foreach (ProdVendido prodVendido in lstProdVendidos)
                {
                    subTo += prodVendido.Precio;
                }
            }
            return subTo;
        }
        public static string ExtraerDetalle(List<ProdVendido> lstProdVendidos)
        {
            string detalle = "";
            if (lstProdVendidos != null && lstProdVendidos.Count != 0)
            {
                foreach (ProdVendido prodVendido in lstProdVendidos)
                {
                    detalle = detalle + prodVendido.Cantidad.ToString() + "x " + prodVendido.Descripcion + ". ";
                }
            }
            return detalle;
        }
        public static NotaDeEnvio Modificar(NotaDeEnvio notaDeEnvio, List<ProdVendido> listaP)
        {
            notaDeEnvio.ProdVendidos = listaP;
            double subTo = 0;
            string deta = "";
            foreach (ProdVendido vActual in listaP)
            {
                subTo += vActual.Precio;
                deta = deta + vActual.Cantidad.ToString() + "x " + vActual.Descripcion + ". ";
            }
            notaDeEnvio.ImporteTotal = subTo;
            notaDeEnvio.Detalle = deta;
            notaDeEnvio.Impresa = 0;
            return notaDeEnvio;
        }
    }
}
