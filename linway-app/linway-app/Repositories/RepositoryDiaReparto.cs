using linway_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryDiaReparto : IRepository<DiaReparto>
    {
        private readonly LinwaydbContext _context;
        public RepositoryDiaReparto(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(DiaReparto diaReparto)
        {
            string commandText = $"INSERT INTO DiaReparto(Dia) " +
                                 $"VALUES ('{diaReparto.Dia}')";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(DiaReparto diaReparto)
        {
            try
            {
                _context.DiaReparto.Remove(diaReparto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
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
            return _context.DiaReparto.Find(id);
        }
        public List<DiaReparto> GetAll()
        {
            return _context.DiaReparto.ToList();
        }
    }
}
