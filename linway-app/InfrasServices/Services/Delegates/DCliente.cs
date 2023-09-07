using InfrasServices.Services;
using linway_app.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using static linway_app.Services.Delegates.DPedido;

namespace linway_app.Services.Delegates
{
    public static class DCliente
    {
        public readonly static Predicate<Cliente> addCliente = AddCliente;
        public readonly static Func<bool> addClientePrimero = AddClientePrimero;
        public readonly static Predicate<Cliente> deleteCliente = DeleteCliente;
        public readonly static Predicate<Cliente> editCliente = EditCliente;
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
            bool success = _service.Add(cliente);
            return success;
        }
        private static bool AddClientePrimero()
        {
            Cliente cliente = new Cliente
            {
                Nombre = "Cliente Particular X",
                Direccion = "Cliente Particular X"
            };
            bool success = addCliente(cliente);
            return success;
        }
        private static bool DeleteCliente(Cliente cliente)
        {
            bool success = _service.Delete(cliente);
            return success;
        }
        private static bool EditCliente(Cliente cliente)
        {
            bool success = _service.Edit(cliente);
            if (success)
            {
                success = editDireccionClienteEnPedidos(cliente);
            }
            return success;
        }
        private static Cliente GetCliente(long clientId)
        {
            Cliente cliente = _service.Get(clientId);
            return cliente;
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
            Cliente cliente = getClientes().Find(x => x.Direccion.Contains(direccion));
            return cliente;
        }
        private static List<Cliente> GetClientes()
        {
            List<Cliente> clientes = _service.GetAll();
            return clientes;
        }
    }
}
