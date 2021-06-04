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
                _entities.Add(cliente);
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
                using (var context = new LinwaydbContext())
                {
                    context.Cliente.Update(cliente);
                    context.SaveChangesAsync();
                    return true;
                }
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
            using (var context = new LinwaydbContext())
            {
                var resp = context.Cliente;
                if (resp == null) return null;
                var lstSinFiltr = resp.ToList();
                var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
                return lst;
            }
        }
    }
}
