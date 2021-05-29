using linway_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryNotaDeEnvio : IRepository<NotaDeEnvio>
    {
        private readonly LinwaydbContext _context;
        public RepositoryNotaDeEnvio(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(NotaDeEnvio notaDeEnvio)
        {
            string commandText = $"INSERT INTO NotaDeEnvio(ClientId, Fecha, Impresa, Detalle, ImporteTotal) " +
                                 $"VALUES ('{notaDeEnvio.ClientId}', '{notaDeEnvio.Fecha}', {notaDeEnvio.Impresa}, " +
                                 $"'{notaDeEnvio.Detalle}', '{notaDeEnvio.ImporteTotal}')";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(NotaDeEnvio notaDeEnvio)
        {
            try
            {
                _context.NotaDeEnvio.Remove(notaDeEnvio);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(NotaDeEnvio notaDeEnvio)
        {
            try
            {
                _context.NotaDeEnvio.Update(notaDeEnvio);
                _context.SaveChangesAsync();
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
            return _context.NotaDeEnvio.Find(id);
        }
        public List<NotaDeEnvio> GetAll()
        {
            return _context.NotaDeEnvio.ToList();
        }

    }
}
