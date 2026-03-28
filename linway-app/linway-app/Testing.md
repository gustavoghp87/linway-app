

||| TESTING |||

A_Principal
-----------
-Filtrar Productos por nombre
-Filtrar Clientes por dirección
-Que anden los botones que abren y cierran los filtros
-Que se actualicen ambas listas al cerrar otro formulario

B_Clientes
----------
-Agregar cliente nuevo: obligatorios dirección y tipo fiscal; que funcione Limpiar
-Editar cliente: buscar por dirección o por ID, mismos obligatorios que agregar; que funcione Limpiar
-Eliminar: buscar por dirección o por ID; que funcione Estoy Seguro

C_Productos
-----------
-Agregar producto nuevo: obligatorios nombre, precio positivo y tipo
-Editar producto: solo editables precio y tipo, no nombre, mismos obligatorios que agregar
-Eliminar: buscar por nombre o por ID; que funcione Estoy Seguro

D_NotasDeEnvio
--------------
Crear:
-Añadir PV: Buscar cliente por ID o dirección, buscar producto por ID o nombre, cantidad positiva; que limpie al aceptar
-Limpiar lista
-Submit: crea nota de envío para el cliente y los productos vendidos, guarda los productos vendidos, agrega datos a la nota si se va a imprimir
	     se puede revisar en FormNotasDeEnvio si se creó tal cual
-Imprimir: la muestra en pantalla y la marca como imprimida
		   se puede revisar en FormNotasDeEnvio si está marcada como imprimida
-Enviar a listado de ventas: crea RegistroVenta para este cliente; se verifica en FormVentas (izquierda) abajo de todo con fecha de hoy; seleccionar
							 suma en ventas o crea ventas para estos PV; se verifica en FormVenttas (derecha) por nombre de producto, si creó o sumó en cada caso
							 agregar este registro a los PV que se están creando; se verifica en FormVentas (Ver Registro) seleccionando el registro por ID
-Enviar a reparto: si no existía cliente en el reparto, crea pedido vacío
				   luego asocia los PV a ese pedido,
				   actualiza etiqueta y cantidades del pedido; se verifica todo desde FormRepartos seleccionando Dia y Nombre
				   actualiza las cantidades del reparto; se verifica en Repartos - Datos de plantilla, exportando el reparto a excel o mirando la DB
Ver:
-Exportar a Excel: se verifica solo en el Excel
-Imprimir nota de envío 3 modalidades, marca como imprimida, actualiza la lista al cerrar la última
-Lista: 6 filtros
-Enviar a hoja de reparto: muestra si está en algún reparto
						   si ya está en el reparto seleccionado, se cancela
						   si el cliente no existe en el reparto, se crea uno vacío
						   edita los productos vendidos para asociarlos al pedido... lo cual implica quitarlos del pedido anterior si estaban, actualizando cantidades y descrípción, y también cantidades de su reparto si el reparto es otro
						   se verifica todo desde FormRepartos, salvo las cantidades totales de los repartos que se ven exportando a Excel (o mirando DB)
-Agregar producto vendido a nota de envío ... si está en pedido ... ventas

-Quitar producto vendido de nota de envío
-Eliminar notas de envío: 3 modalidades, confirmación


E_Ventas
--------
-Lista izquierda: 4 modalidades
-Lista derecha
-Exportar Ventas a Excel
-1) Crear venta
-2) Borrar todas las ventas
-3) Ver registro
-4) Eliminar registros


F_Reparto
---------
-Lista por reparto (día + nombre): muestra cantidades de reparto, filtro Entregar/Todos, atajo Eliminar Pedido (refresca cantidades)
-Agregar reparto a día (los días son 6 y están fijados), no admite combinación repetida Día-Nombre
-Agregar cliente en reparto o sea crea un pedido vacío, no admite combinación repetida
-Limpiar: (1) todos, (2) repartos de un día, (3) pedidos de un reparto, (4) pedido... r

-Todos los cancelar borran sección


No existe aun eliminar Reparto

G_Recibos
---------
-Imprimir: 3 modalidades, ya cierra el form al abrir los imprimibles, al cerrarlos actualiza esta sección y la lista
-Filtrar: 6 modalidades, muestra abajo la cantidad
-Crear: búsqueda de cliente por ID y por dirección, agrega los detalles de a uno, limpiar solo limpia el detalle que se iba a agregar, cancelar limpia todo
-Eliminar: 3 modalidades, confirmación, tiene que eliminar los detalles en DB también, cancelar limpia todo
