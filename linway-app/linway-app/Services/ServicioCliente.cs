using linway_app.Models;
using linway_app.Repositories.Interfaces;
using linway_app.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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
            cliente.Estado = "Eliminado";
            return _unitOfWork.RepoCliente.Edit(cliente);
        }
        public bool Edit(Cliente cliente)
        {
            return _unitOfWork.RepoCliente.Edit(cliente);
        }
        public Cliente Get(long id)
        {
            Cliente cliente = _unitOfWork.RepoCliente.Get(id);
            return cliente == null || cliente.Estado == null || cliente.Estado == "Eliminado"
                ? null : cliente;
        }
        public List<Cliente> GetAll()
        {
            List<Cliente> lst = _unitOfWork.RepoCliente.GetAll();
            lst = (from x
                   in lst                                   
                   where x.Estado != null && x.Estado != "Eliminado"
                   select x).ToList();
            return lst;
        }
    }
}
