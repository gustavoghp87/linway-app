using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace linway_app.Services.Delegates
{
    public static class DCliente
    {
        public readonly static Predicate<Cliente> addCliente = AddCliente;
        public readonly static Func<bool> addClientePrimero = AddClientePrimero;
        public readonly static Action<Cliente> deleteCliente = DeleteCliente;
        public readonly static Action<Cliente> editCliente = EditCliente;
        public readonly static Func<long, Cliente> getCliente = GetCliente;
        public readonly static Func<string, Cliente> getClientePorDireccion = GetClientePorDireccion;
        public readonly static Func<string, Cliente> getClientePorDireccionExacta = GetClientePorDireccionExacta;
        public readonly static Func<List<Cliente>> getClientes = GetClientes;

        private static readonly IServiceBase<Cliente> _service = ServicesObjects.ServCliente;

        private static bool AddCliente(Cliente cliente)
        {
            // no permitir direcciones repetidas
            var clientes = getClientes();
            if (clientes == null || clientes.Count == 0) return false;
            if (clientes.Exists(x => x.Direccion == cliente.Direccion)) return false;
            while (cliente.Direccion.Contains("'")) cliente.Direccion = cliente.Direccion.Replace(char.Parse("'"), '"');
            while (cliente.Nombre.Contains("'")) cliente.Nombre = cliente.Nombre.Replace(char.Parse("'"), '"');
            return _service.Add(cliente);
        }
        private static bool AddClientePrimero()
        {
            return addCliente(new Cliente
            {
                Nombre = "Cliente Particular X",
                Direccion = "Cliente Particular X"
            });
        }
        private static void DeleteCliente(Cliente cliente)
        {
            bool response = _service.Delete(cliente);
            if (!response) Console.WriteLine("Falló guardado Cliente en base de datos");
        }
        private static void EditCliente(Cliente cliente)
        {
            bool response = _service.Edit(cliente);
            if (!response) Console.WriteLine("Falló editando Cliente en base de datos");
        }
        private static Cliente GetCliente(long clientId)
        {
            return _service.Get(clientId);
        }
        private static Cliente GetClientePorDireccion(string direccion)
        {
            List<Cliente> clientes = getClientes();
            Cliente cliente = clientes.Find(x => x.Direccion.ToLower().Contains(direccion.ToLower()));
            //List<Cliente> clientesx = clientes.Where(x => x.Direccion.ToLower().Contains(direccion.ToLower())).ToList();
            if (cliente == null)
            {
                List<string> direcciones = DZGeneral.ignorarTildes(direccion);
                if (direcciones == null || direcciones.Count == 0) return null;
                foreach (var direc in direcciones)
                {
                    Cliente cliente1 = clientes.Find(x => x.Direccion.ToLower().Contains(direc));
                    if (cliente1 != null) return cliente1;
                }
            }
            return cliente;
        }
        private static Cliente GetClientePorDireccionExacta(string direccion)
        {
            return getClientes().Find(x => x.Direccion.Contains(direccion));
        }
        private static List<Cliente> GetClientes()
        {
            return _service.GetAll();
        }
    }
}
