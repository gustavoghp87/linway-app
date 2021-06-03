using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryDiaReparto : IRepository<DiaReparto>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<DiaReparto> _entities;
        public RepositoryDiaReparto(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<DiaReparto>();
        }
        public bool Add(DiaReparto diaReparto)
        {
            try
            {
                _context.DiaReparto.Add(diaReparto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(DiaReparto diaReparto)
        {
            return true;
        }
        public bool Edit(DiaReparto diaReparto)
        {
            try
            {
                _context.DiaReparto.Update(diaReparto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public DiaReparto Get(long id)
        {
            var response = _entities.Find(id);
            return response;
        }
        public List<DiaReparto> GetAll()
        {
            return _entities.ToList();
        }
    }
}
