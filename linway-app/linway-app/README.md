
Mejoras de características para próximas versiones:
--------------------------------------------------
-Cambiar sistema de cantidades de líquidos y polvos (2x5, etcétera)
-Eliminar dobles espaciados
-Segundo campo para teléfono
-En los campos de búsqueda por dirección hacer indistinto usar las tildes en las vocales
-Lograr que la app sea reactiva a cambios en la base de datos
-Agregar opción de eliminar reparto
-Lograr que se pueda usar la app sin tener encendida la pc host (base de datos en la nube)
-En repartos reutilizar los 2 combobox en todas las opciones

Mejoras de sistema para próximas versiones:
------------------------------------------
-Separar dirección de localidad
-Migrar de Windows Forms a WPS u otro más moderno
-Ver si eliminar el atributo Pedido.Direccion
-ClienteId no debería ser obligatorio en Registro de Venta (por venta particular cargada desde Ventas)
-Hacer queries de búsqueda a DB en lugar de traer toda una colección y filtrar por Linq
-Las etiquetas de los Repartos y las cantidades de los Pedidos y los Repartos deberían ser calculados dinámicamente ondemand o por procedimiento en la DB para simplificar todo


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


___________________________________Sistema Linway 15__________________________________    marzo 2026
-Cambio de diseño de servicios: scopes por ciclo de vida de formulario, operaciones como bulks
-Panel de carga para operaciones complejas ("Cargando...")
-Corregido comportamiento de asociar Productos Vendidos anteriores a una Nota de Envío nueva
 al usar "Enviar a Reparto" del Formulario de Ventas
-En Formulario de Nota de Envío, en "Agregar a reparto", ahora avisa si la Nota de Envío ya
 estaba en el reparto destino y cancela
-Datos ahora se eliminan realmente
-Eliminar un cliente elimina sus Recibos y sus Detalles, sus Notas, sus Registros de Venta, y los
 Productos Vendidos asociados a estos últimos tres
-Eliminar un producto elimina también sus Ventas, pero no se puede eliminar si tiene Productos Vendidos
 asociados a Registros de Venta, Pedidos o Notas de Envío
-Divididos los Forms en clases parciales por bloque de diseño
-Índices de unicidad en la base de datos
-Se corrieron los siguientes queries:

DROP DATABASE linway;
CREATE DATABASE linway;
USE linway;
SOURCE "C:\Users\g\Desktop\linway_completo.sql";

--
RENAME TABLE Cliente TO Clientes;
RENAME TABLE DetalleRecibo TO DetalleRecibos;
RENAME TABLE DiaReparto TO DiaRepartos;
RENAME TABLE NotaDeEnvio TO NotaDeEnvios;
RENAME TABLE Pedido TO Pedidos;
RENAME TABLE Producto TO Productos;
RENAME TABLE ProdVendido TO ProdVendidos;
RENAME TABLE Recibo TO Recibos;
RENAME TABLE RegistroVenta TO RegistroVentas;
RENAME TABLE Reparto TO Repartos;
RENAME TABLE Venta TO Ventas;
--
DELETE FROM DiaRepartos WHERE Id = 7;
ALTER TABLE Repartos MODIFY COLUMN Nombre VARCHAR(40) NOT NULL;
ALTER TABLE Repartos ADD CONSTRAINT UC_DiaReparto_Nombre UNIQUE (DiaRepartoId, Nombre);
--
UPDATE ProdVendidos AS pv JOIN NotaDeEnvios AS n ON n.Id = pv.NotaDeEnvioId SET pv.NotaDeEnvioId = NULL WHERE n.Estado = 'Eliminado';
UPDATE ProdVendidos AS pv JOIN Pedidos AS p ON p.Id = pv.PedidoId SET pv.PedidoId = NULL WHERE p.Estado = 'Eliminado';
UPDATE ProdVendidos AS pv JOIN RegistroVentas AS r ON r.Id = pv.RegistroVentaId SET pv.RegistroVentaId = NULL WHERE r.Estado = 'Eliminado';
DELETE FROM ProdVendidos WHERE NotaDeEnvioId IS NULL AND PedidoId IS NULL AND RegistroVentaId IS NULL;
DELETE FROM ProdVendidos WHERE Estado = "Eliminado";
ALTER TABLE ProdVendidos DROP COLUMN Estado;
--
DELETE FROM DetalleRecibos WHERE Estado = "Eliminado";
ALTER TABLE DetalleRecibos DROP COLUMN Estado;
--
DELETE FROM Recibos WHERE Estado = "Eliminado";
ALTER TABLE Recibos DROP COLUMN Estado;
--
DELETE FROM Ventas WHERE Estado = "Eliminado";
ALTER TABLE Ventas DROP COLUMN Estado;
DELETE v FROM Ventas v INNER JOIN Productos p ON v.ProductoId = p.Id WHERE p.Estado = 'Eliminado';
--
DELETE FROM Pedidos WHERE Estado = "Eliminado";
ALTER TABLE Pedidos DROP COLUMN Estado;
--
DELETE FROM RegistroVentas WHERE Estado = "Eliminado";
ALTER TABLE RegistroVentas DROP COLUMN Estado;
--
DELETE FROM NotaDeEnvios WHERE Estado = "Eliminado";
ALTER TABLE NotaDeEnvios DROP COLUMN Estado;
--
ALTER TABLE Repartos DROP COLUMN Estado;
--
ALTER TABLE DiaRepartos DROP COLUMN Estado;
--
DELETE FROM Clientes WHERE Id = 435;
UPDATE Clientes SET Direccion = CONCAT(Direccion, " ELIMINADO ", Id) WHERE Estado = "Eliminado";
UPDATE Pedidos p JOIN Clientes c ON c.Id = p.ClienteId SET p.Direccion = c.Direccion;
ALTER TABLE Clientes MODIFY COLUMN Direccion VARCHAR(80) NOT NULL;
ALTER TABLE Pedidos MODIFY COLUMN Direccion VARCHAR(80) NOT NULL;
ALTER TABLE RegistroVentas MODIFY COLUMN NombreCliente VARCHAR(80) NOT NULL;
ALTER TABLE Recibos MODIFY COLUMN DireccionCliente VARCHAR(80) NOT NULL;
ALTER TABLE Clientes ADD UNIQUE INDEX idx_direccion (Direccion);
--
UPDATE Productos SET Nombre = CONCAT("ELIMINADO ", Id, " - ", Nombre) WHERE Estado = "Eliminado";
ALTER TABLE Productos MODIFY COLUMN Nombre VARCHAR(60) NOT NULL;
ALTER TABLE Productos ADD UNIQUE INDEX idx_nombre (Nombre);
--
DELETE dr FROM DetalleRecibos dr INNER JOIN Recibos r ON r.Id = dr.ReciboId INNER JOIN Clientes c ON c.Id = r.ClienteId WHERE c.Estado = 'Eliminado';
DELETE r FROM Recibos r INNER JOIN Clientes c ON c.Id = r.ClienteId WHERE c.Estado = 'Eliminado';
DELETE pv FROM ProdVendidos pv INNER JOIN NotaDeEnvios ne ON ne.Id = pv.NotaDeEnvioId INNER JOIN Clientes c ON c.Id = ne.ClienteId WHERE c.Estado = 'Eliminado';
DELETE pv FROM ProdVendidos pv INNER JOIN RegistroVentas rv ON rv.Id = pv.RegistroVentaId INNER JOIN Clientes c ON c.Id = rv.ClienteId WHERE c.Estado = 'Eliminado';
DELETE pv FROM ProdVendidos pv INNER JOIN Pedidos pe ON pe.Id = pv.PedidoId INNER JOIN Clientes c ON c.Id = pe.ClienteId WHERE c.Estado = 'Eliminado';
DELETE rv FROM RegistroVentas rv INNER JOIN Clientes c ON c.Id = rv.ClienteId WHERE c.Estado = 'Eliminado';
DELETE ne FROM NotaDeEnvios ne INNER JOIN Clientes c ON c.Id = ne.ClienteId WHERE c.Estado = 'Eliminado';
DELETE p FROM Pedidos p INNER JOIN Clientes c ON c.Id = p.ClienteId WHERE c.Estado = 'Eliminado';
UPDATE Repartos r
    JOIN (
        SELECT 
            RepartoId,
            SUM(A)  AS Ta,
            SUM(E)  AS Te,
            SUM(D)  AS Td,
            SUM(T)  AS Tt,
            SUM(Ae) AS Tae,
            SUM(L)  AS Tl,
            SUM(A + E + T + Ae + D) AS TotalB
        FROM Pedidos
        GROUP BY RepartoId
    ) p ON p.RepartoId = r.Id
    SET 
        r.Ta      = p.Ta,
        r.Te      = p.Te,
        r.Td      = p.Td,
        r.Tt      = p.Tt,
        r.Tae     = p.Tae,
        r.Tl      = p.Tl,
        r.TotalB  = p.TotalB
;
DELETE FROM Clientes WHERE Estado = "Eliminado";
ALTER TABLE Clientes DROP COLUMN Estado;
--
DELETE pr FROM Productos pr LEFT JOIN ProdVendidos pv ON pv.ProductoId = pr.Id WHERE pv.ProductoId = NULL AND pr.Estado = 'Eliminado';
DELETE FROM Productos
    WHERE Estado = 'Eliminado'
        AND Id IN (
            SELECT Id FROM (
                SELECT pr.Id
                FROM Productos pr
                LEFT JOIN ProdVendidos pv ON pv.ProductoId = pr.Id
                LEFT JOIN NotaDeEnvios ne ON ne.Id = pv.NotaDeEnvioId
                LEFT JOIN RegistroVentas rv ON rv.Id = pv.RegistroVentaId
                WHERE pr.Estado = 'Eliminado'
                GROUP BY pr.Id
                HAVING COUNT(ne.Id) = 0 AND COUNT(rv.Id) = 0
            ) AS sub
        )
;
ALTER TABLE Productos DROP COLUMN Estado;
--
UPDATE ProdVendidos SET NotaDeEnvioId = NULL WHERE NotaDeEnvioId = 26931;
DELETE FROM NotaDeEnvios WHERE ID = 26931;
UPDATE ProdVendidos SET RegistroVentaId = NULL WHERE RegistroVentaId IN (46436, 56618);
DELETE FROM RegistroVentas WHERE Id IN (46436, 56618);
DELETE FROM ProdVendidos WHERE NotaDeEnvioId IS NULL AND PedidoId IS NULL AND RegistroVentaId IS NULL;
DELETE FROM Productos WHERE Id IN (52, 263, 264);
SELECT pr.Id, pr.Nombre, COUNT(pv.Id) AS CantidadVendida, COUNT(ne.Id) AS CantidadNotas, COUNT(pe.Id) AS CantidadPedidos, COUNT(rv.Id) AS CantidadVentas FROM Productos pr LEFT JOIN ProdVendidos pv ON pv.ProductoId = pr.Id LEFT JOIN NotaDeEnvios ne ON ne.Id = pv.NotaDeEnvioId LEFT JOIN Pedidos pe ON pe.Id = pv.PedidoId LEFT JOIN RegistroVentas rv ON rv.Id = pv.RegistroVentaId WHERE pr.Nombre LIKE "ELIMINADO %" GROUP BY pr.Id ORDER BY pr.Id;
--
DELETE FROM Pedidos WHERE Id IN (355, 460, 529, 544, 547, 1118, 1886, 1969, 2217);
ALTER TABLE Pedidos ADD UNIQUE KEY (RepartoId, ClienteId);
--


___________________________________Sistema Linway 14__________________________________    junio 2023
-Cambia base de datos de sqlite a MySQL



___________________________________Sistema Linway 13__________________________________    mayo 2022

-Corregido reposicionar en reparto
-Corregidos problemas de velocidad al modificar muchos datos a la vez, por ejemplo limpiar
 reparto
-Corregida modificación de nota de envío que no era permanente
-Ahora al modificar una nota de envío se actualizan automáticamente: pedido, reparto,
 venta, y registro de venta
-Imagen de "Cargando..."


___________________________________Sistema Linway 12__________________________________    octubre 2021

Junio:
-Terminado el proyecto de tipificar los productos
-En agregar productos, ahora el tipo de producto es obligatorio
-En agregar clientes, ahora solo son obligatorios Dirección y Tipo de  contribuyente
-Se simplificaron los formularios de clientes y productos, que se abren con botón único y
 no tienen partes deshabilitadas
-Migración de sistema de datos en listas de objetos en binarios con librería
 System.Runtime.Serialization.Formatters.Binary a base de datos relacional en archivo único
 SQLite, usando ORM Microsoft.EntityFramework.Core.SQLite
-Consecuentemente, ahora a ningún dato se lo elimina de la base de datos sino que pasa
 a ser invisible a la aplicación
-Se eliminó la llave de seguridad de la aplicación
-Remoción de importación desde Excel, y también exportación excepto para repartos y ventas
-La vía alternativa a modificado de datos es abrir linway-db.db con la aplicación
 Browser for SQLite
-Para planillas Excel, cambio de librería de Microsoft por la de Apache que es más rápida
 y no necesita de Office
-Comienza implementación de principios SOLID
-Agrandados algunos formularios
-Remoción de todos los botones de Actualizar (actualización automática)
-Agregadas opciones de buscar clientes por dirección y productos por nombre
-Ignoradas mayúsculas/minúsculas en las búsquedas
-Cambiados los _leave por _textChanged para que la aplicación ejecute sin abandonar el
  campo de escritura


Octubre:
-Generación automática de backup en OneDrive al abrir la app por primera vez en el día
-Lectura de la base de datos con path absoluto para poder usar varias instancias de la app
 a la vez
-Las carpetas de la solución fueron convertidas en proyectos .NET


______________________________________________________________________________________




_____________________Actualizacion Sistema Linway 10 __________________________________   febrero 2021

-Corrección de agregar ventas a registro de ventas desde creación de notas de envío
-Agregada función de exportar e importar desde hoja excel de todos los datos de la 
  aplicación para tener una manera alternativa de modificarlos
-Agrandados algunos cuadros
-Comienza versionado con sistema git
-Código en https://github.com/gustavoghp87/linway-app

______________________________________________________________________________________




_____________________Actualizacion Sistema Linway 9 __________________________________

Objetivo: 
- Quitar importancia al formulario principal y que solo sea informativo.
- Que solo utilize los archivos para mostrar clientes y productos
- Sera mas seguro en cuanto a la perdida de datos importantes.
- crear copias de seguridad.
- Utilizar mas formularios
- refactorizar codigo y "limpiarlo"


Registro de cambios:
- No existe mas el archivo DatosGenerales.bin ni la Clase.
- Crear nota de envio abre un formulario aparte y ya no lo hace desde el formulario principal.
- Formulario de Recibos ya no utiliza el archivo de clientes. 
- Formulario de Notas De Envio ya no utiliza el archivo de productos
- Se creo en formulario de Recibos un boton para crear copia de seguridad.
- Se creo en formulario de Notas de envio un boton para crear Copia de seguridad.
- Se creo formulario de clientes desde el cual se pueden agregar/modificar/borrar clientes 
  y crear copia de seguridad.
- Se creo formulario de Productos desde el cual se pueden agregar/modificar/borrar productos
  y crear copia de seguridad.
- Formularios de Recibos y Notas de envio ahora muestran la cantidad de Recibos/Notas de envio que hay.
- Se creo nuevo formulario de ventas desde el cual se puede operar lo relacionado con las ventas.
- Desde formulario de ventas se puede ver el registro de ventas, del cual se puede ver cada venta	
  realizada y deshacer la venta. 
- nuevo archivo RegistroVentas.bin.

+ Formulario principal no usa mas archivos.



FALTA;
- Otro formulario para modificar listado de ventas antes de exportar.
- Tipo Producto nuevo -> (*)
- Panel informacion en Form1 ( Cant clientes, productos, notificaciones de errores, etc).
-Advertencias




------ Sistema linway ACTUALIZACION 8 ------

** En formulario de reparto el boton de chequeo para ver todos los destinos o solo los que se entregan 
   Tambien funciona para el boton "Exportar";
	- Si esta tildado se exportaran a Excel solo los destinos con reparto
	- Si no se exportaran todos los destinos. 
---------------------------------------------




------ Sistema linway ACTUALIZACION 7 ------

** Se modifico el boton exportar en formulario de notas de envio. antes decia "d"
** Ahora se pueden exportar todos los clientes.
** Se agrego la opcion de agregar por ventas a hoja de reparto el producto "a cobrar"
	- este no se agrega en la lista de ventas
	- modifica la hoja de reparto del destino a cobrar 
---------------------------------------------




------ Sistema linway ACTUALIZACION 5 ------

** Se agrego boton "Exportar" a formulario de Recibos
 
---------------------------------------------




------ Sistema linway ACTUALIZACION 4 ------

** Ahora se permiten crean notas de envío con importe negativo 
 
---------------------------------------------





--------------ACTUALIZACION 3 -----------------------

1- Se borro el Boton "Actualizar" del formulario de hoja de reparto por mal funcionamiento.

2- Se añadio la opcion de Limpiar Datos de un solo Destino en la hoja de reparto.

3- Posible solucion a errores en guardar cambios en hoja de reparto

-----------------------------------------------------





--Actualizacion 2 Sistema linway--

1- Se añadio boton "Actualizar" al formulario de hojas de reparto


2- Se corrigio posible error por el cual no se actualizaria bien el archivo de 
   las hojas de reparto.




   

--Actualizacion 1 Sistema linway--

1- En la hoja de reparto se sacó la "x" que aparece en varios productos al
   verse en cada renglon de la hoja de reparto. Por ejemplo:

             Antes:   "20x Softel blanco".
             Ahora:   "20 Softel blanco".


2- Se corrigio posible error por el cual no se actualizaria bien el archivo de 
   las notas de envio al usarse con varias computadoras.
