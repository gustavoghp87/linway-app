﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;


namespace linway_app
{
    class Importar
    {
        OleDbConnection conn;
        OleDbDataAdapter MyDataAdapter;
        DataTable dt;

        public void ImportarExcel(DataGridView dgv, string nombreHoja)
        {
            string ruta = "";
            try
            {
                OpenFileDialog openfile1 = new OpenFileDialog
                {
                    Filter = "Excel Files |*.xls",
                    Title = "Seleccione el archivo de Excel"
                };
                if (openfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openfile1.FileName.Equals("") == false)
                    {
                        ruta = openfile1.FileName;
                    }
                }

                //conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ruta + ";Extended Properties='Excel 8.0 Xml;HDR=Yes'");
                conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'");
                MyDataAdapter = new OleDbDataAdapter("Select * from [" + openfile1.FileName + "]", conn);
                dt = new DataTable();
                MyDataAdapter.Fill(dt);
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}