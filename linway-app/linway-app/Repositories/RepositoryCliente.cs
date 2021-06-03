using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryCliente : IRepository<Cliente>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<Cliente> _entities;
        public RepositoryCliente(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<Cliente>();
        }
        public bool Add(Cliente cliente)
        {
            try
            {
                _context.Cliente.Add(cliente);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(Cliente cliente)
        {
            cliente.Estado = "Eliminado";
            return Edit(cliente);
        }
        public bool Edit(Cliente cliente)
        {
            try
            {
                _context.Cliente.Update(cliente);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Cliente Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<Cliente> GetAll()
        {
            var resp = _entities;
            if (resp == null) return null;
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
