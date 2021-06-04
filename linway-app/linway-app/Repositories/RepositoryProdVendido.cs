using linway_app.Models;
using linway_app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryProdVendido : IRepository<ProdVendido>
    {
        private readonly LinwaydbContext _context;
        private readonly DbSet<ProdVendido> _entities;
        public RepositoryProdVendido(LinwaydbContext context)
        {
            _context = context;
            _entities = context.Set<ProdVendido>();
        }
        public bool Add(ProdVendido prodVendido)
        {
            try
            {
                _entities.Add(prodVendido);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            //string p1 = "INSERT INTO ProdVendido(ProductoId, Cantidad, Descripcion, Precio, Estado";
            //string p2 = $"VALUES ({prodVendido.ProductoId}, {prodVendido.Cantidad}, '{prodVendido.Descripcion}', " +
            //    $"'{prodVendido.Precio}', 'Activo'";
            //if (prodVendido.NotaDeEnvioId != null)
            //{
            //    p1 += $", NotaDeEnvioId";
            //    p2 += $", {prodVendido.NotaDeEnvioId}";
            //}
            //if (prodVendido.RegistroVentaId != null)
            //{
            //    p1 += $", RegistroVentaId";
            //    p2 += $", {prodVendido.RegistroVentaId}";
            //}
            //if (prodVendido.PedidoId != null)
            //{
            //    p1 += $", PedidoId";
            //    p2 += $", {prodVendido.PedidoId}";
            //}
            //p1 += ") ";
            //p2 += ")";
            //string commandText = p1 + p2;
            //return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(ProdVendido prodVendido)
        {
            prodVendido.Estado = "Eliminado";
            return Edit(prodVendido);
        }
        public bool Edit(ProdVendido prodVendido)
        {
            try
            {
                _entities.Update(prodVendido);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public ProdVendido Get(long id)
        {
            var response = _entities.Find(id);
            if (response == null || response.Estado == null || response.Estado == "Eliminado") return null;
            return response;
        }
        public List<ProdVendido> GetAll()
        {
            var lstSinFiltr = _entities.ToList();
            var lst = lstSinFiltr.Where(x => x.Estado != null && x.Estado != "Eliminado").ToList();
            return lst;
        }
    }
}
