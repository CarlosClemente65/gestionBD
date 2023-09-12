/* Nombre de la biblioteca: gestionBD
 * Version: 1.0.0
 * Descripcion: Esta libreria recopila los metodos necesarios para la conexion y gestion de una base de datos con SQLite
 * Licencia: libre uso
 * Desarrollador: Carlos Clemente Rodriguez
 * Fecha desarrollo: mayo 2023
 */
using System;
using System.Data;
using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;

namespace gestionBD
{
    public class conectar
    {
        /* Variables para gestionar las conexiones
            - dbDir contiene la ruta donde esta alojada la base de datos.
            - dbName contiene el nombre de la base de datos
            - dbPath es la ruta completa
            - conexion se define como una conexion a una DB de SQLite
         */
        string dbPath;
        private SQLiteConnection conexion;

        public conectar(string rutaBD)
        {
            // Se deben pasar como parametro la ruta completa de la base de datos
            dbPath = rutaBD;
        }


        public bool chequeoBD()
        {
            bool resultado = false;
            // Funcion que permite chequear si existe la base de datos y en caso contrario poder crearla
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(dbPath)))
                {
                    //Crea el diretorio si no existe
                    Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
                }

                if (!File.Exists(dbPath))
                {
                    //Crea el fichero de la base de datos si no existe
                    SQLiteConnection.CreateFile(dbPath);
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar la base de datos. " + ex.Message);
            }
            return resultado;
        }

        public SQLiteConnection crearConexion()
        {
            // Funcion para crear una conexion con la base de datos
            try
            {
                conexion = new SQLiteConnection("Data Source=" + dbPath + ";Version=3");
                conexion.Open();
                return conexion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la conexión con la base de datos." + ex.Message);
            }
        }

        public void cerrarConexion()
        {
            // Chequea si se ha creado la conexion y si no se ha cerrado para en ese caso cerrarla
            if (conexion != null && conexion.State != ConnectionState.Closed)
            {
                conexion.Close();
            }
        }

        public DataTable consultaSQL(string sql)
        {
            // Metodo para obtener consultas de la base de datos y devolver un dataTable
            DataTable dt = new DataTable();
            conectar conexion = new conectar(dbPath);

            try
            {
                // Ejecucion del comando SQL que se haya pasado como parametro mediante un using para liberar recursos al finalizar
                using (SQLiteCommand cmd = new SQLiteCommand(sql, crearConexion()))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la consulta a la base de datos: " + ex.Message);
            }
            finally
            {
                cerrarConexion();
            }
            return dt;
        }

        public bool operacionSQL(string sql, Dictionary<string, object> parametros = null)
        {
            //Metodo para creacion, modificacion y eliminacion de registros de la BD mediante el comando que se pase como parametro
            //Devuelve true o false si se ha podido gestionar algun registro.
            //conectar conexion = new conectar(dbPath);
            {
                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, crearConexion()))
                    {
                        if (parametros != null)
                        {
                            foreach (var parametro in parametros)
                            {
                                cmd.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al ejecutar el alta o modificacion en la base de datos: " + ex.Message);
                }
                finally
                {
                    cerrarConexion();
                }
            }
        }
    }
}
