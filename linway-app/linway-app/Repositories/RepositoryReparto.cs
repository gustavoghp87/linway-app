using linway_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryReparto : IRepository<Reparto>
    {
        private readonly LinwaydbContext _context;

        public RepositoryReparto(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(Reparto reparto)
        {
            string commandText = $"INSERT INTO Reparto(Nombre, DiaRepartoId, Ta, Te, Td, Tt, Tae, TotalB, Tl) " +
                                 $"VALUES ('{reparto.Nombre}', {reparto.DiaRepartoId}, '{reparto.Ta}', '{reparto.Te}', '{reparto.Td}', " +
                                 $"{reparto.Tt}, '{reparto.Tae}', '{reparto.TotalB}', '{reparto.Tl}')";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(Reparto reparto)
        {
            try
            {
                _context.Reparto.Remove(reparto);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
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
            return _context.Reparto.Find(id);
        }
        public List<Reparto> GetAll()
        {
            return _context.Reparto.ToList();
        }
    }
}
