-- my.ini: bind-address = 198.162.0.82 que es la dirección local del equipo host

CREATE DATABASE IF NOT EXISTS linway;
CREATE USER 'linway'@'%' IDENTIFIED BY 'password';
GRANT ALL PRIVILEGES ON linway.* TO 'linway'@'%';
FLUSH PRIVILEGES;
USE linway;
CREATE TABLE IF NOT EXISTS DiaReparto (
	Id INT NOT NULL AUTO_INCREMENT,
	Dia TEXT NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id)
);
CREATE TABLE IF NOT EXISTS Producto (
	Id INT NOT NULL AUTO_INCREMENT,
	Nombre TEXT NOT NULL,
	Precio REAL NOT NULL,
	Estado TEXT NOT NULL,
	Tipo TEXT NOT NULL,
	SubTipo TEXT,
	PRIMARY KEY (Id)
);
CREATE TABLE IF NOT EXISTS Cliente (
	Id INT NOT NULL AUTO_INCREMENT,
	Direccion TEXT NOT NULL,
	CodigoPostal TEXT,
	Telefono TEXT,
	Nombre TEXT,
	CUIT TEXT,
	Tipo TEXT,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id)
);
-- #########
CREATE TABLE IF NOT EXISTS Venta (
	Id INT NOT NULL AUTO_INCREMENT,
	ProductoId INT NOT NULL,
	Cantidad INT NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);
CREATE TABLE IF NOT EXISTS Reparto (
	Id INT NOT NULL AUTO_INCREMENT,
	Nombre TEXT NOT NULL,
	DiaRepartoId INT NOT NULL,
	TA INT NOT NULL,
	TE INT NOT NULL,
	TD INT NOT NULL,
	TT INT NOT NULL,
	TAE INT NOT NULL,
	TotalB INT NOT NULL,
	TL INT NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (DiaRepartoId) REFERENCES DiaReparto(Id)
);
CREATE TABLE IF NOT EXISTS RegistroVenta (
	Id INT NOT NULL AUTO_INCREMENT,
	ClienteId INT NULL,
	NombreCliente TEXT NOT NULL,
	Fecha TEXT NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ClienteId) REFERENCES Cliente(Id)
);
CREATE TABLE IF NOT EXISTS NotaDeEnvio (
	Id INT NOT NULL AUTO_INCREMENT,
	ClienteId INT NOT NULL,
	Fecha TEXT NOT NULL,
	Impresa INT NOT NULL,
	Detalle TEXT NOT NULL,
	ImporteTotal REAL NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ClienteId) REFERENCES Cliente(Id)
);
-- #########
CREATE TABLE IF NOT EXISTS Recibo (
	Id INT NOT NULL AUTO_INCREMENT,
	ClienteId INT NOT NULL,
	Fecha TEXT NOT NULL,
	Impreso INT NOT NULL,
	DireccionCliente TEXT NOT NULL,
	ImporteTotal REAL NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ClienteId) REFERENCES Cliente(Id)
);
CREATE TABLE IF NOT EXISTS Pedido (
	Id INT NOT NULL AUTO_INCREMENT,
	Direccion TEXT NOT NULL,
	ClienteId INT NOT NULL,
	RepartoId INT NOT NULL,
	Entregar INT NOT NULL,
	L INT NOT NULL,
	A INT NOT NULL,
	E INT NOT NULL,
	D INT NOT NULL,
	T INT NOT NULL,
	AE INT NOT NULL,
	ProductosText TEXT NOT NULL,
	Orden INT NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ClienteId) REFERENCES Cliente(Id),
	FOREIGN KEY (RepartoId) REFERENCES Reparto(Id)
);
-- #########
CREATE TABLE IF NOT EXISTS ProdVendido (
	Id INT NOT NULL AUTO_INCREMENT,
	ProductoId INT NOT NULL,
	NotaDeEnvioId INT,
	RegistroVentaId INT,
	PedidoId INT,
	Cantidad INT NOT NULL,
	Descripcion TEXT NOT NULL,
	Precio REAL NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (PedidoId) REFERENCES Pedido(Id),
	FOREIGN KEY (ProductoId) REFERENCES Producto(Id),
	FOREIGN KEY (RegistroVentaId) REFERENCES RegistroVenta(Id),
	FOREIGN KEY (NotaDeEnvioId) REFERENCES NotaDeEnvio(Id)
);
CREATE TABLE IF NOT EXISTS DetalleRecibo (
	Id INT NOT NULL AUTO_INCREMENT,
	ReciboId INT NOT NULL,
	Detalle TEXT NOT NULL,
	Importe REAL NOT NULL,
	Estado TEXT NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ReciboId) REFERENCES Recibo(Id)
);


DiaReparto (Id, Dia, Estado)
Producto (Id, Nombre, Precio, Estado, Tipo, SubTipo)
Cliente (Id, Direccion, CodigoPostal, Telefono, Nombre, CUIT, Tipo, Estado)
Venta (Id, ProductoId, Cantidad, Estado)
Reparto (Id, Nombre, DiaRepartoId, TA, TE, TD, TT, TAE, TotalB, TL, Estado)
RegistroVenta (Id, ClienteId, NombreCliente, Fecha, Estado)
NotaDeEnvio (Id, ClienteId, Fecha, Impresa, Detalle, ImporteTotal, Estado)
Recibo (Id, ClienteId, Fecha, Impreso, DireccionCliente, ImporteTotal, Estado)
Pedido (Id, Direccion, ClienteId, RepartoId, Entregar, L, A, E, D, T, AE, ProductosText, Orden, Estado)
ProdVendido (Id, ProductoId, NotaDeEnvioId, RegistroVentaId, PedidoId, Cantidad, Descripcion, Precio, Estado)
DetalleRecibo (Id, ReciboId, Detalle, Importe, Estado)