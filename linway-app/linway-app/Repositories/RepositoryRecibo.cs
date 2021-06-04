using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryRecibo : IRepository<Recibo>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<Recibo> _entities;
        public RepositoryRecibo(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<Recibo>();
        }
        public bool Add(Recibo recibo)
        {
            try
            {
                _entities.Add(recibo);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(Recibo recibo)
        {
            recibo.Estado = "Eliminado";
            return Edit(recibo);
        }
        public bool Edit(Recibo recibo)
        {
            try
            {
                _entities.Update(recibo);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Recibo Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<Recibo> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
