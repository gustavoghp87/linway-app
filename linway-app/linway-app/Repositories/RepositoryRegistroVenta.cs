using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryRegistroVenta : IRepository<RegistroVenta>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<RegistroVenta> _entities;
        public RepositoryRegistroVenta(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<RegistroVenta>();
        }
        public bool Add(RegistroVenta registroVenta)
        {
            try
            {
                _context.RegistroVenta.Add(registroVenta);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(RegistroVenta registroVenta)
        {
            registroVenta.Estado = "Eliminado";
            return Edit(registroVenta);
        }
        public bool Edit(RegistroVenta registroVenta)
        {
            try
            {
                _context.RegistroVenta.Update(registroVenta);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public RegistroVenta Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<RegistroVenta> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
