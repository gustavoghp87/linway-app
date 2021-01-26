using System;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;


namespace linway_app
{
    class Importar
    {
        OleDbConnection conn;
        OleDbDataAdapter myDataAdapter;

        public DataTable ImportarExcel()
        {
            string ruta = "";
            OpenFileDialog openfile1 = new OpenFileDialog
            {
                Filter = "Excel Files |*.xls",
                Title = "Seleccione el archivo de Excel"
            };

            if (openfile1.ShowDialog() == DialogResult.OK)
            {
                if (openfile1.FileName.Equals("") == false)
                {
                    ruta = openfile1.FileName;
                }
            }

            //conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ruta + ";Extended Properties='Excel 8.0 Xml;HDR=Yes'");
            conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'");
            conn.Open();
            myDataAdapter = new OleDbDataAdapter("Select * from [HOJA1$]", conn);
            DataSet ds = new DataSet();
            myDataAdapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            
            return dt;
        }
    }
}
