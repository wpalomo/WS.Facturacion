using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class JornadaDt
    {
        #region JORNADA: Clase de Datos de JornadaDt.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas en un periodo
        /// Las jornadas que nunca se cerraron no aparecen
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************
        public static JornadaL getJornadas(Conexion oConn,
                                int? estacion, DateTime jornadaDesde, DateTime jornadaHasta)
        {
            JornadaL oJornadas = new JornadaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetJornadas";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;

                oCmd.CommandTimeout = 5000;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oJornadas.Add(CargarJornada(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oJornadas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas de un parte
        /// Las jornadas que nunca se cerraron no aparecen
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Parte</param>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************
        public static JornadaL getJornadaParte(Conexion oConn,
                                int parte)
        {
            JornadaL oJornadas = new JornadaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetJornadaParte";
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;

                oCmd.CommandTimeout = 5000;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oJornadas.Add(CargarJornada(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oJornadas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Jornadas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Jornadas</param>
        /// <returns>elemento Jornada</returns>
        /// ***********************************************************************************************
        internal static Jornada CargarJornada(System.Data.IDataReader oDR)
        {
            Jornada oJornada = new Jornada();
            if (oDR["est_codig"] != DBNull.Value)
                oJornada.Estacion = new Estacion((Byte)oDR["est_codig"], oDR["est_nombr"].ToString());
            oJornada.Fecha = (DateTime)oDR["jor_fejor"];
            if (oDR["jor_idval"] != DBNull.Value)
                oJornada.Validador = new Usuario(oDR["jor_idval"].ToString(), oDR["use_nombr"].ToString());
            oJornada.Status = (oDR["jor_estad"].ToString() == "C") ? Jornada.enmStatus.enmCerrada :
                              ((oDR["jor_estad"].ToString() == "R") ? Jornada.enmStatus.enmReabierta :  Jornada.enmStatus.enmAbierta);
            oJornada.FechaModificacion = (DateTime)oDR["jor_fecmo"];
            oJornada.Terminal = oDR["jor_host"].ToString();
            if( oDR["jor_numre"]  != DBNull.Value )
                oJornada.Reaperturas = (int) oDR["jor_numre"];

            return oJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Logs de jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Logs</param>
        /// <returns>elemento Log</returns>
        /// ***********************************************************************************************
        private static Log CargarLog(System.Data.IDataReader oDR)
        {
            Log oLog = new Log();
            oLog.numero = (int)oDR["log_nummo"];
            oLog.estacion = oDR["log_coest"].ToString();
            oLog.fechaJornada = (DateTime)oDR["log_fejor"];
            oLog.tipo = oDR["log_tipmo"].ToString(); ;
            oLog.fecha = (DateTime)oDR["log_fecha"];
            oLog.usuario = oDR["log_idval"].ToString();
            oLog.host = oDR["log_host"].ToString();

            return oLog;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Partes sin Liquidar</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarPartesSinLiquidar(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["estacion"]);
            oCierreJornada.parte = (int)oDR["parte"];
            oCierreJornada.problema = "Parte sin Liquidar";
            oCierreJornada.peajista = oDR["cajero"].ToString();
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = "";

            return oCierreJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Bloques sin Liquidar</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarBloquesSinLiquidar(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["estacion"]);
            oCierreJornada.parte = null;
            oCierreJornada.problema = "Bloque sin Liquidar";
            oCierreJornada.peajista = oDR["cajero"].ToString();
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = "Nro. Bloque: " + oDR["Bloque"] + " Vía: " + oDR["Via"] + " Fecha Apertura: " + oDR["fechaAper"] + " Fecha cierre: " + oDR["fechaCierre"];

            return oCierreJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Anomalias sin Liquidar</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarAnomaliasSinLiquidar(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["estacion"]);
            oCierreJornada.parte = (int) oDR["parte"];
            oCierreJornada.problema = "Anomalias sin validar";
            oCierreJornada.peajista = oDR["cajero"].ToString();
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = "Parte visto: " + oDR["visto"] + " Anomalías sin validar: " + oDR["CantAnomalias"] + " Anomalía: " + oDR["Anomalia"];

            return oCierreJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de los partes que se estan validando</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarPartesValidandose(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["estacion"]);
            oCierreJornada.parte = (int) oDR["parte"];
            oCierreJornada.problema = Traduccion.Traducir("Partes validandose");
            oCierreJornada.peajista = oDR["cajero"].ToString();
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = "Validador: " + oDR["Validador"] + " Terminal: " + oDR["Terminal"];

            return oCierreJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con las diferencias de totales del parte</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarVerificarDiferencias(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["estacion"]);
            oCierreJornada.parte = (int) oDR["parte"];
            oCierreJornada.problema = Traduccion.Traducir("Se encontraron diferencias");
            oCierreJornada.peajista = oDR["cajero"].ToString();
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = Traduccion.Traducir("Para solucionar este problema se debe revalidar el Parte");

            return oCierreJornada;
        }


        private static ProblemasCierreJornada CargarVerificarReposiciones(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["Estacion"]);
            oCierreJornada.parte = (int)oDR["parte"];
            oCierreJornada.problema = Traduccion.Traducir("El Parte posee una reposicion incorrecta.");
            oCierreJornada.peajista = null;
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = Traduccion.Traducir("Para solucionar este problema se debe anular y volver a pedir la reposicion.");

            return oCierreJornada;
        }


        private static ProblemasCierreJornada CargarVerificarFallos(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["Estacion"]);
            oCierreJornada.parte = (int) oDR["parte"];
            oCierreJornada.problema = Traduccion.Traducir("El Parte no posee reposicion economida por el faltante ");
            oCierreJornada.peajista = null;
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = Traduccion.Traducir("Para solucionar este problema se deve pedir la reposicion del parte");

            return oCierreJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con los tickets faltantes</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarTicketsFaltantes(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["NumeroEstacion"]);
            oCierreJornada.parte = (int) oDR["parte"];
            oCierreJornada.problema = "Tickets Faltantes";
            oCierreJornada.peajista = oDR["cajero"].ToString();
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = oDR["descr"].ToString();

            return oCierreJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con los tickets duplicados</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarTicketsDuplicados(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["NumeroEstacion"]);
            oCierreJornada.parte = (int) oDR["parte"];
            oCierreJornada.problema = "Tickets Duplicados";
            oCierreJornada.peajista = oDR["cajero"].ToString();
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = oDR["descr"].ToString();

            return oCierreJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Problemas de Cierre de Jornada
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader dcon las facturas rechazadas de SUNAT</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornada CargarFacturasRechazadasSUNAT(System.Data.IDataReader oDR)
        {
            ProblemasCierreJornada oCierreJornada = new ProblemasCierreJornada();
            oCierreJornada.estacion = Convert.ToInt32(oDR["estacion"]);
            oCierreJornada.parte = null;
            oCierreJornada.problema = "Facturas Rechazadas por SUNAT";
            oCierreJornada.peajista = null;
            oCierreJornada.tipo = "E";
            oCierreJornada.datos = "Archivo: " + oDR["ArchivoXML"] + ", Respuesta SUNAT: " + oDR["CodigoRespuesta"] + " - " + oDR["DetalleRespuesta"];

            return oCierreJornada;
        }

       
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica el registor de jornada si esta interfaseada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada </param>
        /// ***********************************************************************************************
        public static void setInterfaceJornada(Conexion oConn, int? estacion, DateTime jornada)
        {
            try
            { 
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_updJornadaInterfaseContable";
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro de jornada") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("No existe el registro");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el estado de una jornada
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="estacion">Estacion a consultar</param>
        /// <param name="jornada">Jornada a consultar</param>
        /// <returns>elemento ProblemasCierreJornada</returns>
        /// ***********************************************************************************************
        public static string GetEstadoJornada(Conexion oConn, int estacion, DateTime jornada)
        {
            string status = "";
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetEstadoJornada";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    status = oDR["Estado"].ToString();
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        #endregion



        #region Cierre Jornada

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas en un periodo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="estado">String - estado jornada</param>
        /// <param name="porEstacion">bool - Por Estacion</param>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************
        public static JornadaL getJornadasCierre(Conexion oConn,
                                int? estacion, DateTime jornadaDesde, DateTime jornadaHasta, string estado, bool porEstacion)
        {
            JornadaL oJornadas = new JornadaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_CierreJornada_GetJornadas";
                if(estacion > 0)
                    oCmd.Parameters.Add("@Estacion", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@Estado", SqlDbType.Char, 1).Value = estado;
                oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char, 1).Value = porEstacion ? "S" : "N";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oJornadas.Add(CargarJornadaCierre(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oJornadas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Jornadas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Jornadas</param>
        /// <returns>elemento Jornada</returns>
        /// ***********************************************************************************************
        private static Jornada CargarJornadaCierre(System.Data.IDataReader oDR)
        {
            Jornada oJornada = new Jornada();
            oJornada.Fecha = (DateTime)oDR["JornadaContable"];
            oJornada.Estacion = new Estacion(Convert.ToByte(oDR["Estacion"]), oDR["PlazaDescr"].ToString());
            if (oDR["FechaCierre"] != DBNull.Value)
                oJornada.FechaModificacion = (DateTime)oDR["FechaCierre"];
            oJornada.Validador = new Usuario(oDR["CodValidador"].ToString(), oDR["Validador"].ToString());
            oJornada.Status = (oDR["EstadoCodig"].ToString() == "C") ? Jornada.enmStatus.enmCerrada :
                              ((oDR["EstadoCodig"].ToString() == "R") ? Jornada.enmStatus.enmReabierta : Jornada.enmStatus.enmAbierta);
            return oJornada;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene la primera jornada a cerrar
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// ***********************************************************************************************
        public static DateTime getJornadaACerrar(Conexion oConn, int? estacion)
        {
            DateTime fecha = DateTime.Today;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetJornadaACerrar";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    fecha = Util.DbValueToNullable<DateTime>(oDR["jornada"]) == null ? DateTime.Today : Convert.ToDateTime(oDR["jornada"]);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

                return fecha;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Realiza el cierre de una jornada
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="usuario">Usuario que realiza el cierre</param>
        /// <param name="estacion">Estacion a consultar</param>
        /// <param name="fechaJornada">Jornada a cerrar</param>
        /// <param name="host">Terminal desde donde se cierra la jornada</param>
        /// <returns>Codigo de resultado del cierre</returns>
        /// ***********************************************************************************************
        public static int setCierreJornada(Conexion oConn, string usuario, int estacion, DateTime fechaJornada, string host)
        {
            int retval = 0;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_SetCierreJornada";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = fechaJornada;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar,10).Value = usuario;
                oCmd.Parameters.Add("@host", SqlDbType.VarChar, 30).Value = host;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. -102
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("Error de Parametrización");
                    if (retval == -103)
                        msg = Traduccion.Traducir("La jornada ya se encontraba cerrada");

                    throw new Telectronica.Errores.ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        #endregion

        public static int setAbrirJornada(Conexion oConn, string usuario, int estacion, DateTime fechaJornada, string host)
        {
            int retval = 0;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_SetAperturaJornada";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = fechaJornada;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@host", SqlDbType.VarChar, 30).Value = host;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. -102
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("Error de Parametrización");
                    if (retval == -103)
                        msg = Traduccion.Traducir("La jornada ya se encontraba cerrada");
                    if (retval == -104)
                        msg = Traduccion.Traducir("La jornada ya fue procesada contablemente");

                    throw new Telectronica.Errores.ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }


        public static LogL getLogsJornadas(Conexion oConn, int estacion, DateTime jornada)
        {
            LogL oLogs = new LogL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetLogs";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oLogs.Add(CargarLog(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oLogs;
        }

        public static bool getPartesSinLiquidar(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
	            oCmd.CommandTimeout = 600;
                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetPartesSinLiquidar";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarPartesSinLiquidar(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool getBloquesSinLiquidar(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
				oCmd.CommandTimeout = 600;
                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetBloquesSinLiquidar";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarBloquesSinLiquidar(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool getAnomaliasSinValidar(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
				oCmd.CommandTimeout = 600;
	            

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetAnomaliasSinValidar";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarAnomaliasSinLiquidar(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        public static bool getPartesValidandose(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
				oCmd.CommandTimeout = 600;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetPartesValidandose";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarPartesValidandose(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool getVerificarDiferencias(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
				oCmd.CommandTimeout = 600;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetVerificarDiferencias";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarVerificarDiferencias(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool getVerificarReposicionesNoPedidas(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
				oCmd.CommandTimeout = 600;


                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetReposicionEconomicaPedida";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarVerificarFallos(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        


        public static bool GetTicketsFaltantes(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_CrtlValidacion_GetTicketsFaltantes";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarTicketsFaltantes(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        public static bool GetTicketsDuplicados(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_CrtlValidacion_GetTicketsDuplicados";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarTicketsDuplicados(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Consulta las facturas rechazadas por la SUNAT y las retorna en 
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="oProblemas">Lista de problemas del cierre de jornada</param>
        /// <param name="estacion">Estacion a consultar</param>
        /// <param name="jornada">Jornada a consultar</param>
        /// <returns>Lista de problemas de cierre de jornada</returns>
        /// ***********************************************************************************************
        public static bool GetFacturasRechazadasSUNAT(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetFacturasRechazadasSUNAT";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarFacturasRechazadasSUNAT(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        public static bool esFechaDeJornada(Conexion conn, DateTime fechaDesde, DateTime fechaHasta, DateTime jornada, Estacion estacion)
        {
            bool res = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_IsFechaDeJornada";
                oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = estacion.Numero;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                res = retval == 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        public static void MarcarAnuladasFacturasDeJornada(Conexion conn, int estacion, DateTime fechaJornada)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = conn.conection;
            oCmd.Transaction = conn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Validacion.usp_Jornada_MarcarAnuladasFacturas";
            oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = estacion;
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = fechaJornada;

            oCmd.ExecuteNonQuery();
        }



        public static void getVerificarReposicionesMalPedidas(Conexion oConn, ProblemasCierreJornadaL oProblemas, int? estacion, DateTime jornada)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 600;


                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Jornada_GetReposicionEconomicaErronea";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oProblemas.Add(CargarVerificarReposiciones(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ;
        }
    }
}
