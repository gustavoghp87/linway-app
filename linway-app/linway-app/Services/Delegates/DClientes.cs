using linway_app.Forms;
using linway_app.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace linway_app.Services.Delegates
{
    public static class DClientes
    {
        public delegate bool DAddCliente(Cliente cliente);
        public delegate bool DAddClientePrimero();
        public delegate void DDeleteCliente(Cliente cliente);
        public delegate void DEditCliente(Cliente cliente);
        public delegate Cliente DGetCliente(long clientId);
        public delegate Cliente DGetClientePorDireccion(string direccion);
        public delegate Cliente DGetClientePorDireccionExacta(string direccion);
        public delegate List<Cliente> DGetClientes();

        public readonly static DAddCliente addCliente
            = new DAddCliente(AddCliente);
        public readonly static DAddClientePrimero addClientePrimero
            = new DAddClientePrimero(AddClientePrimero);
        public readonly static DDeleteCliente deleteCliente
            = new DDeleteCliente(DeleteCliente);
        public readonly static DEditCliente editCliente
            = new DEditCliente(EditCliente);
        public readonly static DGetCliente getCliente
            = new DGetCliente(GetCliente);
        public readonly static DGetClientePorDireccion getClientePorDireccion
            = new DGetClientePorDireccion(GetClientePorDireccion);
        public readonly static DGetClientePorDireccionExacta getClientePorDireccionExacta
            = new DGetClientePorDireccionExacta(GetClientePorDireccionExacta);
        public readonly static DGetClientes getClientes
            = new DGetClientes(GetClientes);

        private static bool AddCliente(Cliente cliente)
        {
            while (cliente.Direccion.Contains("'")) cliente.Direccion = cliente.Direccion.Replace(char.Parse("'"), '"');
            while (cliente.Nombre.Contains("'")) cliente.Nombre = cliente.Nombre.Replace(char.Parse("'"), '"');
            return Form1._servCliente.Add(cliente);
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
            bool response = Form1._servCliente.Delete(cliente);
            if (!response) MessageBox.Show("Falló guardado Cliente en base de datos");
        }
        private static void EditCliente(Cliente cliente)
        {
            bool response = Form1._servCliente.Edit(cliente);
            if (!response) MessageBox.Show("Falló editando Cliente en base de datos");
        }
        private static Cliente GetCliente(long clientId)
        {
            return Form1._servCliente.Get(clientId);
        }
        private static Cliente GetClientePorDireccion(string direccion)
        {
            return GetClientes().Find(x => x.Direccion.ToLower().Contains(direccion.ToLower()));
        }
        private static Cliente GetClientePorDireccionExacta(string direccion)
        {
            return GetClientes().Find(x => x.Direccion.Contains(direccion));
        }
        private static List<Cliente> GetClientes()
        {
            return Form1._servCliente.GetAll();
        }
    }
}
