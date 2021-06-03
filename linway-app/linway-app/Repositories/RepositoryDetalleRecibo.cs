using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryDetalleRecibo : IRepository<DetalleRecibo>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<DetalleRecibo> _entities;
        public RepositoryDetalleRecibo(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<DetalleRecibo>();
        }
        public bool Add(DetalleRecibo detalleRecibo)
        {
            try
            {
                _context.DetalleRecibo.Add(detalleRecibo);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(DetalleRecibo detalleRecibo)
        {
            detalleRecibo.Estado = "Eliminado";
            return Edit(detalleRecibo);
        }
        public bool Edit(DetalleRecibo detalleRecibo)
        {
            try
            {
                _context.DetalleRecibo.Update(detalleRecibo);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public DetalleRecibo Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<DetalleRecibo> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
