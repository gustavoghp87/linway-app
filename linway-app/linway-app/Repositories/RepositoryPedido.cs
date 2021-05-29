using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryPedido : IRepository<Pedido>
    {
        private readonly LinwaydbContext _context;
        public RepositoryPedido(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(Pedido pedido)
        {
            string commandText = $"INSERT INTO Pedido(Direccion, ClienteId, RepartoId, Entregar, L, A, E, D, T, Ae, Productos) " +
                                 $"VALUES ('{pedido.Direccion}', {pedido.ClienteId}, {pedido.RepartoId}, {pedido.Entregar}, " +
                                          $"'{pedido.L}', '{pedido.A}', '{pedido.E}', '{pedido.D}', '{pedido.T}', '{pedido.Ae}', " +
                                          $"'{pedido.Productos}')";
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(Pedido pedido)
        {
            try
            {
                _context.Pedido.Remove(pedido);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(Pedido pedido)
        {
            try
            {
                _context.Pedido.Update(pedido);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public Pedido Get(long id)
        {
            return _context.Pedido.Find(id);
        }
        public List<Pedido> GetAll()
        {
            return _context.Pedido.ToList();
        }
    }
}
