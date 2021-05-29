using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System.Collections.Generic;

namespace linway_app.Services
{
    public class ServicioCliente : IServicioCliente
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicioCliente(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Add(Cliente cliente)
        {
            return _unitOfWork.RepoCliente.Add(cliente);
        }
        public bool Delete(Cliente cliente)
        {
            return _unitOfWork.RepoCliente.Delete(cliente);
        }
        public bool Edit(Cliente cliente)
        {
            return _unitOfWork.RepoCliente.Edit(cliente);
        }
        public Cliente Get(long id)
        {
            return _unitOfWork.RepoCliente.Get(id);
        }
        public List<Cliente> GetAll()
        {
            return _unitOfWork.RepoCliente.GetAll();
        }
    }
}
