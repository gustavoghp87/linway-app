using linway_app.Services.Interfaces;
using Models;
using NPOI.POIFS.FileSystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace linway_app.Services.FormServices
{
    public class ClienteServices: IClienteServices
    {
        private readonly IServicesBase<Cliente> _services;
        public ClienteServices(IServicesBase<Cliente> services)
        {
            _services = services;
        }
        public async Task AddClienteAsync(Cliente cliente)
        {
            cliente.Direccion = cliente.Direccion.Trim();
            {   // no permitir comillas simples
                while (cliente.Direccion.Contains("'"))
                {
                    cliente.Direccion = cliente.Direccion.Replace(char.Parse("'"), '"');
                }
                while (cliente.Nombre.Contains("'"))
                {
                    cliente.Nombre = cliente.Nombre.Replace(char.Parse("'"), '"');
                }
            }
            {   // no permitir direcciones repetidas (no se hace por DB porque se permiten repetidos entre clientes eliminados)
                var clientes = await GetClientesAsync();
                if (clientes.Exists(x => x.Direccion == cliente.Direccion))
                {
                    throw new Exception($"Ya existe un cliente con esta dirección: {cliente.Direccion}");
                }
            }
            _services.Add(cliente);
        }
        //public async Task AddClientePrimeroAsync()
        //{
        //    var cliente = new Cliente
        //    {
        //        Nombre = "Cliente Particular X",
        //        Direccion = "Cliente Particular X"
        //    };
        //    await AddClienteAsync(cliente);
        //}
        public void DeleteCliente(Cliente cliente)
        {
            cliente.Estado = "Eliminado";
            _services.Edit(cliente);
            //_services.Delete(cliente);
        }
        public void EditCliente(Cliente cliente)
        {
            _services.Edit(cliente);
        }
        public async Task<Cliente> GetClientePorIdAsync(long clientId)
        {
            Cliente cliente = await _services.GetAsync(clientId);
            return cliente;
        }
        public async Task<Cliente> GetClientePorDireccionAsync(string direccion)
        {
            List<Cliente> clientes = await GetClientesAsync();
            Cliente cliente = clientes.Find(x => x.Direccion.ToLower().Contains(direccion.ToLower()));
            if (cliente == null)
            {
                List<string> direcciones = Helpers.IgnorarTildes(direccion);
                if (direcciones == null || direcciones.Count == 0)
                {
                    return null;
                }
                foreach (var direc in direcciones)
                {
                    Cliente cliente1 = clientes.Find(x => x.Direccion.ToLower().Contains(direc));
                    if (cliente1 != null)
                    {
                        return cliente1;
                    }
                }
            }
            return cliente;
        }
        public async Task<Cliente> GetClientePorDireccionExactaAsync(string direccion)
        {
            List<Cliente> clientes = await GetClientesAsync();
            Cliente cliente = clientes.Find(x => x.Direccion.Contains(direccion));
            return cliente;
        }
        public async Task<List<Cliente>> GetClientesAsync()
        {
            List<Cliente> clientes = await _services.GetAllAsync();
            return clientes;
        }
    }
}
