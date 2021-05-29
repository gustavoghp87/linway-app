using linway_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryRegistroVenta : IRepository<RegistroVenta>
    {
        private readonly LinwaydbContext _context;
        public RepositoryRegistroVenta(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(RegistroVenta registroVenta)
        {
            string commandText = $"INSERT INTO RegistroVenta(ClienteId, NombreCliente, Fecha) " +
                                 $"VALUES ({registroVenta.ClienteId}, '{registroVenta.NombreCliente}', '{registroVenta.Fecha}')";

            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(RegistroVenta registroVenta)
        {
            try
            {
                _context.RegistroVenta.Remove(registroVenta);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
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
            return _context.RegistroVenta.Find(id);
        }
        public List<RegistroVenta> GetAll()
        {
            return _context.RegistroVenta.ToList();
        }
    }
}
