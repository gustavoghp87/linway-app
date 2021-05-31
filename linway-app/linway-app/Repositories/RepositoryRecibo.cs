using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryRecibo : IRepository<Recibo>
    {
        private readonly LinwaydbContext _context;
        public RepositoryRecibo(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(Recibo recibo)
        {
            string commandText = $"INSERT INTO Recibo(ClienteId, DireccionCliente, Fecha, ImporteTotal, Impresa) " +
                                 $"VALUES ({recibo.ClienteId}, '{recibo.DireccionCliente}', '{recibo.Fecha}', " +
                                        $"'{recibo.ImporteTotal}', {recibo.Impresa})";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(Recibo recibo)
        {
            try
            {
                _context.Recibo.Remove(recibo);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Recibo recibo)
        {
            try
            {
                _context.Recibo.Update(recibo);
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
            return _context.Recibo.Find(id);
        }
        public List<Recibo> GetAll()
        {
            return _context.Recibo.ToList();
        }
    }
}
