using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryDetalleRecibo : IRepository<DetalleRecibo>
    {
        private readonly LinwaydbContext _context;
        public RepositoryDetalleRecibo(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(DetalleRecibo detalleRecibo)
        {
            string commandText = $"INSERT INTO DetalleRecibo(ReciboId, Detalle, Importe) " +
                                 $"VALUES ({detalleRecibo.ReciboId}, '{detalleRecibo.Detalle}', '{detalleRecibo.Importe}')";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(DetalleRecibo detalleRecibo)
        {
            try
            {
                _context.DetalleRecibo.Remove(detalleRecibo);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(DetalleRecibo detalleRecibo)
        {
            try
            {
                string commandText = $"UPDATE DetalleRecibo " +
                                     $"SET ReciboId='{detalleRecibo.ReciboId}', Detalle='{detalleRecibo.Detalle}', " +
                                          $"Importe='{detalleRecibo.Importe}' " +
                                     $"WHERE Id={detalleRecibo.Id}";
                return SQLiteCommands.Execute(commandText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public DetalleRecibo Get(long id)
        {
            return _context.DetalleRecibo.Find(id);
        }
        public List<DetalleRecibo> GetAll()
        {
            return _context.DetalleRecibo.ToList();
        }
    }
}
