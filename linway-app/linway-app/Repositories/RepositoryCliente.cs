using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryCliente : IRepository<Cliente>
    {
        private readonly LinwaydbContext _context;
        public RepositoryCliente(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(Cliente cliente)
        {
            string commandText = $"INSERT INTO Cliente(Direccion, CodigoPostal, Telefono, Name, CUIT, Tipo) " +
                                 $"VALUES ('{cliente.Direccion}', '{cliente.CodigoPostal}', '{cliente.Telefono}', " +
                                         $"'{cliente.Name}', '{cliente.Cuit}', '{cliente.Tipo}')";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(Cliente cliente)
        {
            try
            {
                _context.Cliente.Remove(cliente);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Cliente cliente)
        {
            try
            {
                _context.Cliente.Update(cliente);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Cliente Get(long id)
        {
            return _context.Cliente.Find(id);
        }
        public List<Cliente> GetAll()
        {
            return _context.Cliente.ToList();
        }
    }
}
