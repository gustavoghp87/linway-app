using linway_app.Forms;
using linway_app.Models;
using linway_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DCliente
    {
        public readonly static Func<Cliente, bool> addCliente = AddCliente;
        public readonly static Func<bool> addClientePrimero = AddClientePrimero;
        public readonly static Action<Cliente> deleteCliente = DeleteCliente;
        public readonly static Action<Cliente> editCliente = EditCliente;
        public readonly static Func<long, Cliente> getCliente = GetCliente;
        public readonly static Func<string, Cliente> getClientePorDireccion = GetClientePorDireccion;
        public readonly static Func<string, Cliente> getClientePorDireccionExacta = GetClientePorDireccionExacta;
        public readonly static Func<List<Cliente>> getClientes = GetClientes;
        public readonly static Func<string, string> invertirFecha = InvertirFecha;

        private static readonly IServiceBase<Cliente> _service = Form1._servCliente;
        private static bool AddCliente(Cliente cliente)
        {
            while (cliente.Direccion.Contains("'")) cliente.Direccion = cliente.Direccion.Replace(char.Parse("'"), '"');
            while (cliente.Nombre.Contains("'")) cliente.Nombre = cliente.Nombre.Replace(char.Parse("'"), '"');
            return _service.Add(cliente);
        }
        private static bool AddClientePrimero()
        {
            Cliente cliente = new Cliente();
            cliente.Nombre = "Cliente Particular X";
            cliente.Direccion = "Cliente Particular X";
            return addCliente(cliente);
        }
        private static void DeleteCliente(Cliente cliente)
        {
            bool response = _service.Delete(cliente);
            if (!response) MessageBox.Show("Falló guardado Cliente en base de datos");
        }
        private static void EditCliente(Cliente cliente)
        {
            bool response = _service.Edit(cliente);
            if (!response) MessageBox.Show("Falló editando Cliente en base de datos");
        }
        private static Cliente GetCliente(long clientId)
        {
            return _service.Get(clientId);
        }
        private static Cliente GetClientePorDireccion(string direccion)
        {
            return getClientes().Find(x => x.Direccion.ToLower().Contains(direccion.ToLower()));
        }
        private static Cliente GetClientePorDireccionExacta(string direccion)
        {
            return getClientes().Find(x => x.Direccion.Contains(direccion));
        }
        private static List<Cliente> GetClientes()
        {
            return _service.GetAll();
        }
        private static string InvertirFecha(string fecha)
        {
            if (!fecha.Contains("-")) return fecha;
            var array = fecha.Split('-');
            if (array.Length != 3) return fecha;
            return array[2] + "-" + array[1] + "-" + array[0];
        }
    }
}
