# gestionBD
## Clase para la gestion de una base de datos con SQLite
## @ Carlos Clemente - mayo/2023

Esta clase tiene varios metodos para la gestion de una base de datos con SQLite.
1. conectar(): funcion principal de la clase (se le debe pasar la ruta local de la base de datos)
2. chequeoDB(): chequea si existe tanto la ruta como la propia base de datos, y en caso contrario lo crea.
3. crearConexion(): funcion para crear una conexion
4. cerrarConexion(): chequea si existe una conexion y si esta abierta antes de cerrarla.
5. consultaSQL(): permite hacer una consulta a la base de datos pasandole como parametro la consulta a realizar.
6. operacionSQL(): funcion para la creacion, modificacion y eliminacion de registros pasandole como parametro la accion a realizar.
