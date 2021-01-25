using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;


namespace linway_app
{
    class DBconnection
    {
        //private readonly string connString;
        private SqlConnection connection;
        readonly SqlConnectionStringBuilder builder;
        private readonly string value;

        public DBconnection() {
            //connString = "Data Source=" + server + ";Initial Catalog=" + database + ";User=" + username + ";Password=" + psw + ";";
            DotNetEnv.Env.Load();
            DotNetEnv.Env.TraversePath().Load();
            builder = new SqlConnectionStringBuilder
            {
                DataSource = Environment.GetEnvironmentVariable("SERVER"),
                UserID = Environment.GetEnvironmentVariable("USERNAME"),
                Password = Environment.GetEnvironmentVariable("PSW"),
                InitialCatalog = Environment.GetEnvironmentVariable("DATABASE")
            };
        }

        public bool ConectarConDB()
        {
            try
            {
                connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                //MessageBox.Show("Conectado a DB");
                return true;
            }
            catch (SqlException exc)
            {
                MessageBox.Show("Falla conectando a la base de datos SQL SERVER: " + exc.Message);
                return false;
            }
        }

        public bool AgregarClienteEnDB (string direccion, int cp, int telefono, string nombre, string cuit, TipoR tipo)
        {
            bool success = ConectarConDB();
            if (!success) return false;
            string sql = "INSERT INTO clientes(nombre, direccion, cuit, cp, telefono, condicion) " +
                "VALUES('" + nombre + "', '" + direccion + "', '" + cuit + "', " + cp +
                ", " + telefono + ", '" + tipo + "');";
            //MessageBox.Show(sql);
            try
            {
                new SqlCommand(sql, connection).ExecuteNonQuery();
                MessageBox.Show("Los datos se guardaron correctamente");
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en SQL SERVER: " + ex.Message);
                return false;
            }
        }

    }
}
