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
    public class LogActividadDT
    {


        #region LOGACTIVIDADDT: Clase de Datos de Log de Actividad de un proceso.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el estado de actividad de un determinado proceso.
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista con la Actividad de un proceso</returns>
        /// ***********************************************************************************************
        public static LogActividadL getLogActividad(Conexion oConn, string tipoProceso)
        {
            LogActividadL oLogActividades = new LogActividadL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_LogActividad_getActividadProceso";
                oCmd.Parameters.Add("@TipoProceso", SqlDbType.Char, 10).Value = tipoProceso;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oLogActividades.Add(CargarLogActividad(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oLogActividades;

        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Log de Actividad de un proceso
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Log de Actividades</param>
        /// <returns>Lista con el elemento Log de Actividad de un proceso de la base de datos</returns>
        /// ***********************************************************************************************
        private static LogActividad CargarLogActividad(System.Data.IDataReader oDR)
        {
            LogActividad oLogActividad = new LogActividad();
                

            oLogActividad.TipoActividad = Convert.ToString(oDR["log_tipo"]);
            oLogActividad.EstadoProcesoActual = Convert.ToString(oDR["log_estad"]);
            oLogActividad.ResultadoUltimoProceso = Convert.ToString(oDR["log_ulest"]);
            oLogActividad.FechaInicioProceso = Util.DbValueToNullable<DateTime>(oDR["log_fecin"]);
            oLogActividad.FechaUltimoLogueo = Util.DbValueToNullable<DateTime>(oDR["log_ulfec"]);
            oLogActividad.NombreTerminalEjecucionProceso = Convert.ToString(oDR["log_iphost"]);
            oLogActividad.NombreSesionEjecucionProceso = Convert.ToString(oDR["log_sesion"]);
            oLogActividad.DescripcionEjecucionUltimoProceso = Convert.ToString(oDR["log_descr"]);
            oLogActividad.ElementosAfectadosPorUltimoProceso = Util.DbValueToNullable<Int32>(oDR["log_canti"]);

            return oLogActividad;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica el estado de la actividad de un determinado proceso 
        /// </summary>
        /// <param name="oUsuario">LogActividad - Objeto con la informacion de actividad de un determinado proceso</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updEstadoProceso(LogActividad logActividad, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_LogActividad_updActividadProceso";

                oCmd.Parameters.Add("@TipoProceso", SqlDbType.Char, 10).Value = logActividad.TipoActividad;
                oCmd.Parameters.Add("@EstadoActual", SqlDbType.Char, 1).Value = logActividad.EstadoProcesoActual;
                oCmd.Parameters.Add("@UltimoEstado", SqlDbType.Char, 1).Value = logActividad.ResultadoUltimoProceso;
                oCmd.Parameters.Add("@FechaInicioProceso", SqlDbType.DateTime).Value = logActividad.FechaInicioProceso;
                oCmd.Parameters.Add("@FechaUltimoProceso", SqlDbType.DateTime).Value = logActividad.FechaUltimoLogueo;
                oCmd.Parameters.Add("@NombreHost", SqlDbType.VarChar, 100).Value = logActividad.NombreTerminalEjecucionProceso;
                oCmd.Parameters.Add("@Sesion", SqlDbType.VarChar, 100).Value = logActividad.NombreSesionEjecucionProceso;
                oCmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 1000).Value = logActividad.DescripcionEjecucionUltimoProceso;
                oCmd.Parameters.Add("@CantidadAfectados", SqlDbType.Int).Value = logActividad.ElementosAfectadosPorUltimoProceso;


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

                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion


    }
}
