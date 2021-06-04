using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryNotaDeEnvio : IRepository<NotaDeEnvio>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<NotaDeEnvio> _entities;
        public RepositoryNotaDeEnvio(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<NotaDeEnvio>();
        }
        public bool Add(NotaDeEnvio notaDeEnvio)
        {
            try
            {
                _entities.Add(notaDeEnvio);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Delete(NotaDeEnvio notaDeEnvio)
        {
            notaDeEnvio.Estado = "Eliminado";
            return Edit(notaDeEnvio);
        }
        public bool Edit(NotaDeEnvio notaDeEnvio)
        {
            try
            {
                _entities.Update(notaDeEnvio);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public NotaDeEnvio Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<NotaDeEnvio> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }

    }
}
