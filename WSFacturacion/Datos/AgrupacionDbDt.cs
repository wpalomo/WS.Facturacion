using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;
using Telectronica.Errores;


namespace Telectronica.Peaje
{
    public class AgrupacionDbDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Agrupaciones definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Agrupaciones definidas</returns>
        /// ***********************************************************************************************
        public static AgrupacionDbL getAgrupaciones(Conexion oConn,int? codigo)
        {
            AgrupacionDbL oAgrupaciones = new AgrupacionDbL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AgrupacionesDb_GetAgrupaciones";
            oCmd.Parameters.Add("@Codig", SqlDbType.TinyInt).Value = codigo;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oAgrupaciones.Add(CargarAgrupacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oAgrupaciones;
        
        }

        public static void addAgrupacionDb( Conexion oConn, AgrupacionDb oAgrupacionDb)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AgrupacionesDb_addAgrupacion";

            oCmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = oAgrupacionDb.BaseDatos;
            oCmd.Parameters.Add("@Servi", SqlDbType.VarChar, 50).Value = oAgrupacionDb.ServidorDatos;
            oCmd.Parameters.Add("@Url", SqlDbType.VarChar, 50).Value = oAgrupacionDb.URL;
            oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 50).Value = oAgrupacionDb.Descripcion;
            

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("Este numero de estación ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este número de Estación fue eliminado");
                }

                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una AgrupacionDb de peaje en la base de datos
        /// </summary>
        /// <param name="oEstacion">AgrupacionDb - Objeto con la informacion de la agrupacion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updAgrupacionDb(Conexion oConn, AgrupacionDb oAgrupacionDb)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AgrupacionesDb_updAgrupacion";
           
            oCmd.Parameters.Add("@Codig", SqlDbType.TinyInt).Value = oAgrupacionDb.Codigo;
            oCmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = oAgrupacionDb.BaseDatos;
            oCmd.Parameters.Add("@Servi", SqlDbType.VarChar, 50).Value = oAgrupacionDb.ServidorDatos;
            oCmd.Parameters.Add("@Url", SqlDbType.VarChar, 50).Value = oAgrupacionDb.URL;
            oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 50).Value = oAgrupacionDb.Descripcion;
           

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro de la estación");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una agrupacion de base de datos en la base de datos, invocando a otro metodo que lo hace efectivo
        /// </summary>
        /// <param name="oEstacion">AgrupacionDb - Objeto de agrupacion a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delAgrupacionDb(Conexion oConn, AgrupacionDb oAgrupacionDb)
        {
            delAgrupacionDb(oConn, oAgrupacionDb.Codigo);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una AgrupacionDb de peaje en la base de datos
        /// </summary>
        /// <param name="estacion">int - Numero de agrupacion a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delAgrupacionDb(Conexion oConn, int agrupacion)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_AgrupacionesDb_delAgrupacion";

                oCmd.Parameters.Add("@Codig", SqlDbType.TinyInt).Value = agrupacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la estación");
                    }

                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
            }
        }


        private static AgrupacionDb CargarAgrupacion(SqlDataReader oDr)
        {
            AgrupacionDb oAgrupacion = new AgrupacionDb();
            oAgrupacion.Codigo = Convert.ToInt32(oDr["adb_codig"]);
            oAgrupacion.BaseDatos = oDr["adb_nomdb"].ToString();
            oAgrupacion.ServidorDatos = oDr["adb_servi"].ToString();
            oAgrupacion.URL = oDr["adb_url"].ToString();
            oAgrupacion.Descripcion = oDr["adb_descr"].ToString();

            return oAgrupacion;
        }
    }
}
