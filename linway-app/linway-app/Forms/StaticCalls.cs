using linway_app.Models;
using linway_app.Repositories;
using linway_app.Services;
using System.Collections.Generic;

namespace linway_app.Forms
{
    public static class StaticCalls
    {
        public delegate List<Cliente> DGetClientes();
        public delegate Cliente DGetClientePorDireccion(string direccion);
        public delegate Cliente DGetClientePorDireccionExacta(string direccion);
        public delegate bool DAddCliente(Cliente cliente);
        public delegate List<Producto> DGetProductos();

        public readonly static DGetClientes getClientes = new DGetClientes(GetClientes);
        public readonly static DAddCliente addCliente = new DAddCliente(AddCliente);
        public readonly static DGetProductos getProductos = new DGetProductos(GetProductos);
        public readonly static DGetClientePorDireccion getClientePorDireccion
            = new DGetClientePorDireccion(GetClientePorDireccion);
        public readonly static DGetClientePorDireccionExacta getClientePorDireccionExacta
            = new DGetClientePorDireccionExacta(GetClientePorDireccionExacta);

        private static readonly ServicioCliente ServCliente
            = new ServicioCliente(new UnitOfWork(new LinwaydbContext()));
        private static readonly ServicioProducto ServProducto
            = new ServicioProducto(new UnitOfWork(new LinwaydbContext()));
        
        public static List<Cliente> GetClientes()
        {
            return ServCliente.GetAll();
        }
        public static Cliente GetClientePorDireccion(string direccion)
        {
            return GetClientes().Find(x => x.Direccion.ToLower().Contains(direccion.ToLower()));
        }
        public static Cliente GetClientePorDireccionExacta(string direccion)
        {
            return GetClientes().Find(x => x.Direccion.Contains(direccion));
        }
        public static bool AddCliente(Cliente cliente)
        {
            return ServCliente.Add(cliente);
        }

        public static List<Producto> GetProductos()
        {
            return ServProducto.GetAll();
        }
    }
}
