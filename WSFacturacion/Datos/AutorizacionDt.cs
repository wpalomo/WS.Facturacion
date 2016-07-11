using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class AutorizacionDt
    {


        #region Autorizacion: Clase de Datos de Autorizacion.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Autorizacion definidas
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Autorizacion</returns>
        /// ***********************************************************************************************
        public static AutorizacionL getASRI(Conexion oConn)
        {
            AutorizacionL oAutorizaciones = new AutorizacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_AutorizacionSRI_GetAutorizacionSRI";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oAutorizaciones.Add(CargarASRI(oDR));
                }

                // Cerramos el objeto
                oCmd = null;

                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAutorizaciones;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Genera un elemento vacío
        /// </summary>
        /// <returns>Objeto Autorizacion</returns>
        /// ***********************************************************************************************
        private static Autorizacion Limpiar()
        {
            Autorizacion oAutorizacion = new Autorizacion();

            oAutorizacion.TipoDocumento = "1";
            oAutorizacion.NumeroAutorizacion = "";
            oAutorizacion.FechaInicio = (DateTime) DateTime.Today;
            oAutorizacion.FechaProgramacion = (DateTime) DateTime.Now;
            oAutorizacion.FechaCaducidad = (DateTime) DateTime.Today;

            return oAutorizacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Autorizacion definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>List of Autorizacion</returns>
        /// ***********************************************************************************************
        public static Autorizacion getUnASRI(Conexion oConn, DateTime? dtFecha)
        {
            Autorizacion oAutorizacion = new Autorizacion();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_AutorizacionSRI_GetAutorizacionSRI";

                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = dtFecha;

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    // Esta el registro, pero ahora miramos que este con datos
                    if (oDR["asr_fecha"] != DBNull.Value)
                    {
                        oAutorizacion = CargarASRI(oDR);
                    }
                    else
                    {
                        //No habia datos
                        oAutorizacion = Limpiar();
                    }
                }
                else
                {
                    //No habia datos
                    oAutorizacion = Limpiar();
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAutorizacion;
        }



 
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Autorizacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Autorizacion</param>
        /// <returns>Lista con el elmento Autorizacion de la base de datos</returns>
        /// ***********************************************************************************************
        private static Autorizacion CargarASRI(System.Data.IDataReader oDR)
        {
            Autorizacion oAutorizacion = new Autorizacion();

            oAutorizacion.NumeroAutorizacion = oDR["asr_numau"].ToString();
            oAutorizacion.TipoDocumento = oDR["asr_tipfa"].ToString();

            oAutorizacion.FechaInicio = (DateTime)oDR["asr_fecha"];
            oAutorizacion.FechaProgramacion = (DateTime)oDR["asr_feing"];
            oAutorizacion.FechaCaducidad = (DateTime)oDR["asr_venci"];
 
            return oAutorizacion;


        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Autorizacion en la base de datos
        /// </summary>
        /// <param name="oAutorizacion">Autorizacion  - Objeto con la informacion de la Autorizacion a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static int addSRI(Autorizacion oAutorizacion, Conexion oConn)
        {
            int retval = 0;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_AutorizacionSRI_addAutorizacionSRI";

                oCmd.Parameters.Add("@asr_numau", SqlDbType.VarChar, 10).Value = oAutorizacion.NumeroAutorizacion;
                oCmd.Parameters.Add("@asr_tipfa", SqlDbType.VarChar, 1).Value = oAutorizacion.TipoDocumento;
                oCmd.Parameters.Add("@asr_feing", SqlDbType.DateTime).Value = oAutorizacion.FechaProgramacion;
                oCmd.Parameters.Add("@asr_fecha", SqlDbType.DateTime).Value = oAutorizacion.FechaInicio;
                oCmd.Parameters.Add("@asr_venci", SqlDbType.DateTime).Value = oAutorizacion.FechaCaducidad;


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. -102
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("Este registro ya existe");

                    throw new Telectronica.Errores.WarningException(msg);
                    //throw new ErrorSPException(msg);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Autorizacion en la base de datos
        /// </summary>
        /// <param name="oAutorizacion">Autorizacion  - Objeto con la informacion de la Autorizacion a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static int updASRI(Autorizacion oAutorizacion, Conexion oConn)
        {
            int retval = 0;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_AutorizacionSRI_updAutorizacionSRI";

                oCmd.Parameters.Add("@asr_numau", SqlDbType.VarChar,10).Value = oAutorizacion.NumeroAutorizacion;
                oCmd.Parameters.Add("@asr_tipfa", SqlDbType.VarChar,1).Value = oAutorizacion.TipoDocumento;
                oCmd.Parameters.Add("@asr_feing", SqlDbType.DateTime).Value = oAutorizacion.FechaProgramacion;
                oCmd.Parameters.Add("@asr_fecha", SqlDbType.DateTime).Value = oAutorizacion.FechaInicio;
                oCmd.Parameters.Add("@asr_venci", SqlDbType.DateTime).Value = oAutorizacion.FechaCaducidad;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro de la Autorizacion SRI");

                    throw new Telectronica.Errores.ErrorSPException(msg);
                    //throw new WarningException(msg);  

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Autorización de la base de datos
        /// </summary>
        /// <param name="oAutorizacion">Autorizacion  - Objeto con la informacion de la Autorizacion a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delASRI(Autorizacion oAutorizacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_AutorizacionSRI_delAutorizacionSRI";

                oCmd.Parameters.Add("@asr_feing", SqlDbType.DateTime).Value = oAutorizacion.FechaProgramacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("La Autorización no se puede dar de baja porque está siendo utilizado"));
                throw ex;
            }
            return;
        }




        #endregion


    }
}
