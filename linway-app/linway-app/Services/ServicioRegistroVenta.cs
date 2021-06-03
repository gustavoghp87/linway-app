using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Services
{
    public class ServicioRegistroVenta : IServicioRegistroVenta
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioRegistroVenta(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(RegistroVenta registroVenta)
        {
            return _unitOfWork.RepoRegistroVenta.Add(registroVenta);
        }
        public bool Delete(RegistroVenta registroVenta)
        {
            return _unitOfWork.RepoRegistroVenta.Delete(registroVenta);
        }
        public bool Edit(RegistroVenta registroVenta)
        {
            return _unitOfWork.RepoRegistroVenta.Edit(registroVenta);
        }
        public RegistroVenta Get(long id)
        {
            return _unitOfWork.RepoRegistroVenta.Get(id);
        }
        public List<RegistroVenta> GetAll()
        {
            return _unitOfWork.RepoRegistroVenta.GetAll();
        }

        public long AddAndGetId(RegistroVenta registroVenta)
        {
            bool response = Add(registroVenta);
            if (!response) return 0;
            var lst = GetAll();
            return lst.Last().Id;
        }
        public bool ModificarClienteId(long clienteId, RegistroVenta registroVenta)
        {
            registroVenta.ClienteId = clienteId;
            bool response = Edit(registroVenta);
            return response;
        }
    }
}
