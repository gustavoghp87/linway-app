import sqlite3
import pandas as pd

connection = sqlite3.connect('linway-db.db')
cursor = connection.cursor()

data = pd.read_excel('clientes.xls')
for i in data.index:
	Id      	 = str(data['Codigo'][i])
	Direccion    = str(data['Direccion - Localidad'][i])
	CodigoPostal = str(data['CP'][i])
	Telefono     = str(data['Telefono'][i])
	Nombre       = str(data['Nombre y Apellido'][i])
	CUIT         = str(data['CUIT'][i])
	Tipo         = str(data['Tipo'][i])
	Estado       = "Activo"
	if (str(data['CP'][i]) == "nan"):
		CodigoPostal = ""
	if (str(data['Telefono'][i]) == "nan"):
		Telefono = ""
	if (str(data['Nombre y Apellido'][i]) == "nan"):
		Nombre = ""
	if (str(data['CUIT'][i]) == "nan"):
		CUIT = ""
	print("\n", Direccion, "\n")
	cursor.execute(f"INSERT INTO Cliente(Direccion, CodigoPostal, Telefono, Nombre, CUIT, Tipo, Estado) VALUES ('{Direccion}', '{CodigoPostal}', '{Telefono}', '{Nombre}', '{CUIT}', '{Tipo}', '{Estado}')")

data = pd.read_excel('productos.xls')
for i in data.index:
	Id      	 = str(data['Codigo'][i])
	Nombre       = str(data['Producto'][i])
	Precio       = str(data['Precio'][i])
	Tipo         = str(data['Tipo'][i])
	SubTipo      = str(data['SubTipo'][i])
	Estado       = "Activo"
	if (str(data['SubTipo'][i]) == "nan"):
		SubTipo = ""

	print("\n", Nombre, "\n", Tipo, " ", SubTipo)
	cursor.execute(f"INSERT INTO Producto(Nombre, Precio, Tipo, SubTipo, Estado) VALUES ('{Nombre}', '{Precio}', '{Tipo}', '{SubTipo}', '{Estado}')")

connection.commit()
connection.close()
