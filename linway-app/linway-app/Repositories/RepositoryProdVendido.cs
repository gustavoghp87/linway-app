using linway_app.Models;
using linway_app.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linway_app.Repositories
{
    public class RepositoryProdVendido : IRepository<ProdVendido>
    {
        private readonly LinwaydbContext _context;
        public RepositoryProdVendido(LinwaydbContext context)
        {
            _context = context;
        }
        public bool Add(ProdVendido prodVendido)
        {
            string p1 = "INSERT INTO ProdVendido(ProductoId, Cantidad, Descripcion, Precio";
            string p2 = $"VALUES ({prodVendido.ProductoId}, {prodVendido.Cantidad}, '{prodVendido.Descripcion}','{prodVendido.Precio}'";
            if (prodVendido.NotaDeEnvioId != null)
            {
                p1 += $", NotaDeEnvioId";
                p2 += $", {prodVendido.NotaDeEnvioId}";
            }
            if (prodVendido.RegistroVentaId != null)
            {
                p1 += $", RegistroVentaId";
                p2 += $", {prodVendido.RegistroVentaId}";
            }
            if (prodVendido.PedidoId != null)
            {
                p1 += $", PedidoId";
                p2 += $", {prodVendido.PedidoId}";
            }
            p1 += ") ";
            p2 += ")";
            string commandText = p1 + p2;
            return SQLiteCommands.Execute(commandText);
        }
        public bool Delete(ProdVendido prodVendido)
        {
            try
            {
                _context.ProdVendido.Remove(prodVendido);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Edit(ProdVendido prodVendido)
        {
            try
            {
                _context.ProdVendido.Update(prodVendido);
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
            return _context.ProdVendido.Find(id);
        }
        public List<ProdVendido> GetAll()
        {
            return _context.ProdVendido.ToList();
        }
    }
}
