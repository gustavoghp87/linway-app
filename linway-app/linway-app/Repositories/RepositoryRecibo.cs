using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace linway_app.Repositories
{
    public class RepositoryRecibo : IRepository<Recibo>
    {
        private readonly LinwaydbContext _context;

        public RepositoryRecibo(LinwaydbContext context)
        {
            _context = context;
        }

        public bool Add(Recibo t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Recibo t)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Recibo t)
        {
            throw new NotImplementedException();
        }

        public Recibo Get(long id)
        {
            return _context.Recibo.Find(id);
        }

        public List<Recibo> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}
