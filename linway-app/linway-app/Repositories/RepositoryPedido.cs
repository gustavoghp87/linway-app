using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryPedido : IRepository<Pedido>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<Pedido> _entities;
        public RepositoryPedido(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<Pedido>();
        }
        public bool Add(Pedido pedido)
        {
            try
            {
                _context.Pedido.Add(pedido);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            //string commandText = $"INSERT INTO Pedido(Direccion, ClienteId, RepartoId, Entregar, L, A, E, D, T, Ae, Productos, Estado) " +
            //                     $"VALUES ('{pedido.Direccion}', {pedido.ClienteId}, {pedido.RepartoId}, {pedido.Entregar}, " +
            //                              $"'{pedido.L}', '{pedido.A}', '{pedido.E}', '{pedido.D}', '{pedido.T}', '{pedido.Ae}', " +
            //                              $"'{pedido.Productos}', 'Activo')";
            //return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(Pedido pedido)
        {
            pedido.Estado = "Eliminado";
            return Edit(pedido);
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
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<Pedido> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
