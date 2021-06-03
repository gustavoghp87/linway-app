using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryReparto : IRepository<Reparto>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<Reparto> _entities;
        public RepositoryReparto(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<Reparto>();
        }
        public bool Add(Reparto reparto)
        {
            try
            {
                _context.Reparto.Add(reparto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(Reparto reparto)
        {
            reparto.Estado = "Eliminado";
            return Edit(reparto);
        }
        public bool Edit(Reparto reparto)
        {
            try
            {
                _context.Reparto.Update(reparto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Reparto Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<Reparto> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
