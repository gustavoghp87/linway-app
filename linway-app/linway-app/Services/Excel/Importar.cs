using linway_app.Models;
using linway_app.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace linway_app.Excel
{
    public class Importar
    {
        readonly string archivo;
        readonly string extension = "xls";
        //string extension = "xlsx";
        readonly OleDbConnection conn;
        readonly OleDbDataAdapter myDataAdapter;
        readonly DataSet ds;
        readonly DataTable dt;

        public Importar(string archivo)
        {
            try
            {
                this.archivo = archivo + "." + extension;
                string ruta = @"Copias de seguridad/" + archivo;
                //conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ruta + ";Extended Properties='Excel 8.0 Xml;HDR=Yes'");
                conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'");
                conn.Open();
                myDataAdapter = new OleDbDataAdapter("Select * from [HOJA1$]", conn);
                ds = new DataSet();
                myDataAdapter.Fill(ds);
                dt = ds.Tables[0];
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Falló algo en la lectura de " + archivo + ": " + e.Message);
            }
        }

        public List<Cliente> ImportarClientes()
        {
            try
            {
                List<Cliente> listaClientes = new List<Cliente>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[0].ToString() == "") continue;
                    //MessageBox.Show(dt.Rows[i].ItemArray[0].ToString() + " | " + dt.Rows[i].ItemArray[1].ToString() + " | " + dt.Rows[i].ItemArray[2].ToString() + " | " + dt.Rows[i].ItemArray[3].ToString() + " | " + dt.Rows[4].ItemArray[0].ToString() + " | " + dt.Rows[i].ItemArray[5].ToString() + " | " + dt.Rows[i].ItemArray[6].ToString());
                    int id = Int32.Parse(dt.Rows[i].ItemArray[0].ToString());
                    string direccion = dt.Rows[i].ItemArray[1].ToString();
                    if (direccion.Contains("–")) direccion = direccion.Replace("–", "-");
                    string codigoPostal = dt.Rows[i].ItemArray[2].ToString();
                    string telefono = dt.Rows[i].ItemArray[3].ToString();
                    string nombre = dt.Rows[i].ItemArray[4].ToString();
                    string cuit = dt.Rows[i].ItemArray[5].ToString();
                    TipoR tipo;
                    if (dt.Rows[i].ItemArray[6].ToString() == "Inscripto")
                    {
                        tipo = TipoR.Inscripto;
                    }
                    else
                    {
                        tipo = TipoR.Monotributo;
                    }
                    //listaClientes.Add(new Cliente(id, direccion, codigoPostal, telefono, nombre, cuit, tipo));
                    listaClientes.Add(new Cliente {
                        Id = id,
                        Direccion = direccion,
                        CodigoPostal = codigoPostal,
                        Telefono = telefono,
                        Nombre = nombre,
                        Cuit = cuit,
                        Tipo = tipo.ToString()
                    });
                }
                return listaClientes;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error importando el Excel " + archivo + ": " + exc.Message);
                return null;
            }
        }

        public List<Producto> ImportarProductos()
        {
            try
            {
                if (dt.Rows.Count < 1) return null;
                List<Producto> listaProductos = new List<Producto>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[0].ToString() == "") continue;
                    //MessageBox.Show(i.ToString() + " - " + dt.Rows[i].ItemArray[0].ToString() + " - " + dt.Rows[i].ItemArray[1].ToString() + " - " + dt.Rows[i].ItemArray[2].ToString());
                    int id = Int32.Parse(dt.Rows[i].ItemArray[0].ToString());
                    string nombre = dt.Rows[i].ItemArray[1].ToString();
                    decimal precio = decimal.Parse(dt.Rows[i].ItemArray[2].ToString());
                    //listaProductos.Add(new Producto(id, nombre, precio));
                    listaProductos.Add(new Producto {
                        Id = id,
                        Nombre = nombre,
                        Precio = precio
                    });
                }
                return listaProductos;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error importando el Excel " + archivo + ": " + exc.Message);
                return null;
            }
        }

        public List<NotaDeEnvio> ImportarNotas()
        {
            try
            {
                List<NotaDeEnvio> notasEnvio = new List<NotaDeEnvio>();
                // if (laNota.Cliente.Contains("–")) laNota.Cliente = laNota.Cliente.Replace("–", "-");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[0].ToString() == "") continue;
                    int Id = Int32.Parse(dt.Rows[i].ItemArray[0].ToString());
                    string fecha = dt.Rows[i].ItemArray[1].ToString();
                    string clie = dt.Rows[i].ItemArray[2].ToString();
                    //char[] separators = new char[] { '.', ' ' };
                    string[] productos = dt.Rows[i].ItemArray[3].ToString().Split('.');
                    string[] precios = dt.Rows[i].ItemArray[3].ToString().Split('&');
                    if (productos.Length == 0) MessageBox.Show("Es cero: " + i.ToString());
                    List<ProdVendido> listaVendidos = new List<ProdVendido>();
                    int j = 0;
                    while (j<productos.Length)
                    {
                        try
                        {
                            int cantidad = Int32.Parse(productos[j].Substring(0, productos[j].IndexOf('x')));
                            string detalle = productos[j].Split('x')[1].Trim();
                            float precio = float.Parse(precios[j+1]);
                            //MessageBox.Show(producto);
                            //MessageBox.Show(producto.Substring(0, producto.IndexOf('x')));
                            //MessageBox.Show(producto.Split('x')[1].Trim());
                            //* listaVendidos.Add(new ProdVendido(detalle, cantidad, precio));
                        }
                        catch
                        {
                            //MessageBox.Show("No hay x: " + producto + "... en el " + i.ToString());
                            //j++;
                        }   
                        j++;
                    }
                    float total = float.Parse(dt.Rows[i].ItemArray[4].ToString());
                    bool impresa = dt.Rows[i].ItemArray[5].ToString().ToLower() == "si" || dt.Rows[i].ItemArray[5].ToString().ToLower() == "sí";
                    //MessageBox.Show(Codigo + " " + fecha + " " + clie + " " + dt.Rows[i].ItemArray[4].ToString() + " - " + dt.Rows[i].ItemArray[5].ToString());
                    try
                    {
                        if (clie.Contains("–")) clie = clie.Replace("–", "-");
                        // notasEnvio.Add(new NotaDeEnvio(Codigo, clie, listaVendidos, impresa, fecha, total));
                        //* notasEnvio.Add(new NotaDeEnvio
                        //* {
                        //*     Id = Id, Cliente = clie, listaVendidos, impresa, fecha, total
                        //* });
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Error generando nota de envío: " + exc.Message);
                    }
                }
                return notasEnvio;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error importando el Excel " + archivo + ": " + exc.Message);
                return null;
            }
        }

        public List<Venta> ImportarVentas()
        {
            List<Venta> listaVentas = new List<Venta>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[0].ToString() != "")
                    {
                        //MessageBox.Show(dt.Rows[i].ItemArray[0].ToString() + " " + dt.Rows[i].ItemArray[1].ToString());
                        string Producto = dt.Rows[i].ItemArray[0].ToString();
                        int Cantidad = int.Parse(dt.Rows[i].ItemArray[1].ToString());
                        //* Venta venta = new Venta(Producto);
                        //* venta.RealizarVenta(Cantidad);
                        //* listaVentas.Add(venta);
                    }
                }
                return listaVentas;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error importando el Excel " + archivo + ": " + exc.Message);
                return null;
            }
        }

        public List<RegistroVenta> ImportarRegistroVentas()
        {
            List<RegistroVenta> listaRegistro = new List<RegistroVenta>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[0].ToString() != "")
                    {
                        int id = int.Parse(dt.Rows[i].ItemArray[0].ToString());
                        string fecha = dt.Rows[i].ItemArray[1].ToString();
                        string cliente = dt.Rows[i].ItemArray[2].ToString();
                        if (cliente.Contains("–")) cliente = cliente.Replace("–", "-");
                        //* listaRegistro.Add(new RegistroVenta(id, fecha, cliente));
                    }
                }
                return listaRegistro;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error importando el Excel " + archivo + ": " + exc.Message);
                return null;
            }
        }

        public List<DiaReparto> ImportarHojaDeRepartos()
        {
            List<DiaReparto> diasRepartos = new List<DiaReparto>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[0].ToString() == "") continue;
                    string Dia = dt.Rows[i].ItemArray[0].ToString();
                    if (Dia == "") break;
                    string Nombre = dt.Rows[i].ItemArray[1].ToString();
                    if (Nombre == "") continue;
                    string Direccion = dt.Rows[i].ItemArray[2].ToString();
                    if (Direccion.Contains("–")) Direccion = Direccion.Replace("–", "-");
                    string Productos = dt.Rows[i].ItemArray[3].ToString();
                    bool Entregar = dt.Rows[i].ItemArray[4].ToString().ToLower() == "si" || dt.Rows[i].ItemArray[4].ToString().ToLower() == "sí";
                    int L = int.Parse(dt.Rows[i].ItemArray[5].ToString());
                    int A = int.Parse(dt.Rows[i].ItemArray[6].ToString());
                    int E = int.Parse(dt.Rows[i].ItemArray[7].ToString());
                    int D = int.Parse(dt.Rows[i].ItemArray[8].ToString());
                    int T = int.Parse(dt.Rows[i].ItemArray[9].ToString());
                    int AE = int.Parse(dt.Rows[i].ItemArray[10].ToString());
                    bool existeDia = false;
                    bool existeNombre = false;
                    //* Destino nuevoDestino = new Destino(Direccion, Productos, Entregar, L, A, E, D, T, AE);
                    foreach (DiaReparto dia in diasRepartos)
                    {
                        if (dia.Dia == Dia)
                        {
                            existeDia = true;
                            //foreach (Reparto reparto in dia.Repartos)
                            //{
                            //    if (reparto.Nombre == Nombre)
                            //    {
                            //        existeNombre = true;
                            //        //* reparto.Destinos.Add(nuevoDestino);
                            //    }
                            //}

                            if (!existeNombre)
                            {
                                //* Reparto nuevoReparto = new Reparto(Nombre);
                                //* nuevoReparto.Destinos.Add(nuevoDestino);
                                //* dia.Repartos.Add(nuevoReparto);
                            }
                        }
                    }

                    if (!existeDia)
                    {
                        //* Reparto reparto = new Reparto(Nombre);
                        //* reparto.Destinos.Add(nuevoDestino);

                        DiaReparto diaNuevo = new DiaReparto { Dia = Dia } ;
                        //* diaNuevo.Repartos.Add(reparto);
                        diasRepartos.Add(diaNuevo);
                    }
                    //MessageBox.Show(Dia + " - " + Nombre + " - " + Direccion + " - " + Productos + " - " + Entregar.ToString());
                }
                return diasRepartos;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error importando el Excel " + archivo + ": " + exc.Message);
                return null;
            }
        }

        public List<Recibo> ImportarRecibos()
        {
            List<Recibo> listaRecibos = new List<Recibo>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[0].ToString() == "") continue;
                    int Numero = Int32.Parse(dt.Rows[i].ItemArray[0].ToString());
                    string Fecha = dt.Rows[i].ItemArray[1].ToString();
                    string Direccion = dt.Rows[i].ItemArray[2].ToString();
                    //float Total = float.Parse(dt.Rows[i].ItemArray[3].ToString());
                    bool Impresa = dt.Rows[i].ItemArray[4].ToString().ToLower() == "si" || dt.Rows[i].ItemArray[4].ToString().ToLower() == "sí";

                    List<DetalleRecibo> listaDetalles = new List<DetalleRecibo>();
                    string detalles = dt.Rows[i].ItemArray[5].ToString();
                    string[] pares = detalles.Split('|');
                    foreach (string par in pares)
                    {
                        if (par == "" || par == " " || par == "  ") continue;
                        string[] productos = par.Split(':');
                        string detalle = productos[0];
                        float valor = float.Parse(productos[1]);
                        //* listaDetalles.Add(new DetalleRecibo(detalle, valor));
                    }
                    //* listaRecibos.Add(new Recibo(Numero, Direccion, listaDetalles, Fecha, Impresa));
                }
                return listaRecibos;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error importando el Excel " + archivo + ": " + exc.Message);
                return null;
            }
        }
    }
}
