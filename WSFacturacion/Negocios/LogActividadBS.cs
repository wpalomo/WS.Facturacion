using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class LogActividadBS
    {

        #region LOG DE ACTIVIDAD: Metodos de la Clase Log de Actividad de un proceso.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el estado de actividad de un determinado proceso.
        /// </summary>
        /// <returns>Lista con la actividad de un determinado proceso</returns>
        /// ***********************************************************************************************
        private static LogActividadL getLogActividad(string tipoProceso)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(true, false);

                    return LogActividadDT.getLogActividad(conn, tipoProceso);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de la actividad de un proceso.
        /// </summary>
        /// <param name="oLogActividad">Objeto con la informacion a actualizar del proceso</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static void updEstadoProceso(LogActividad oLogActividad)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos el estado
                    LogActividadDT.updEstadoProceso(oLogActividad, conn);


                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Determina si se puede iniciar o no la ejecucion del prceso, validando que no este inicado o bien
        /// que se haya iniciado hace mas de N minutos
        /// </summary>
        
        /// ***********************************************************************************************
        public static bool InicioBloqueoProceso(string nombreProceso, out string sCausaInicio)
        {
            bool bPuedeIniciar = false;


            sCausaInicio = "";

            try
            {
                LogActividadL oLogs = null;                     // Variable objeto para consulta de actividad
                LogActividad oLogActividadProceso = null;
                bool estaEjecutandoseProceso = true;


                // No alcanza con chequear que solo haya una instancia, si se abre en dos sesiones diferentes no se detecta.
                // Grabamos y revisamos en la base de datos cuando se inicia y detiene la aplicacion


                // La primera vez NO hay ningun logueo previo, lo contemplamos
                oLogs = LogActividadBS.getLogActividad(nombreProceso);
                if (oLogs.Count > 0)
                {
                    oLogActividadProceso = oLogs[0];
                    estaEjecutandoseProceso = oLogActividadProceso.esEjecutandoProcesoActual;


                    sCausaInicio = "Una instancia de la aplicación se está ejecutando: " + "\n"
                                            + "Terminal: " + oLogActividadProceso.NombreTerminalEjecucionProceso + ", \n"
                                            + "Sesión: " + oLogActividadProceso.NombreSesionEjecucionProceso + ", \n";

                    // Si la ultima vez que ejcuto el proceso fue hace mas de N minutos, igualmente permitimos que lo vuelva a ejecutar
                    if (DateTime.Now - oLogActividadProceso.FechaUltimoLogueo > new TimeSpan(0, 120, 0))
                    {
                        sCausaInicio += "La ultima ejecucion del proceso fue el " + oLogActividadProceso.FechaUltimoLogueo.ToString()
                                     + ", se inicia el proceso de todos modos";

                        estaEjecutandoseProceso = false;
                    }
                
                }
                else
                {
                    estaEjecutandoseProceso = false;
                }



                if (!estaEjecutandoseProceso)
                {

                    // Logueamos cuando se inicia el proceso (la corrida actual)
                    LogActividadBS.updEstadoProceso(new LogActividad(nombreProceso,
                                                                     true,
                                                                     true,
                                                                     null,
                                                                     DateTime.Now,
                                                                     null,
                                                                     null,
                                                                     "Inicio del proceso - Productor ",
                                                                     null));

                    bPuedeIniciar = true;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }


            return bPuedeIniciar;
        }


        /// ***********************************************************************************************
        /// <summary>
        ///   Finaliza el bloqueo del proceso, actualizando en la base de datos el estado del mismo  
        /// </summary>
        /// <param name="nombreProceso">Nombre del proceso a desbloquear</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void FinBloqueoProceso(string nombreProceso, bool ejecucionOK, string resultadoEjecucion)
        {

            try
            {
                // Logueamos cuando se detiene el proceso (la corrida actual)
                LogActividadBS.updEstadoProceso(new LogActividad(nombreProceso,
                                                                 false,
                                                                 ejecucionOK,
                                                                 null,
                                                                 DateTime.Now,
                                                                 null,
                                                                 null,
                                                                 "Fin del proceso. Resultado: " + (ejecucionOK ? "OK" : "MAL - " + resultadoEjecucion),
                                                                 null));


            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /*
         * 
         * 
         *         /// ***********************************************************************************************
                /// <summary>
                /// Determina si se puede iniciar o no la ejecucion del prceso, validando que no este inicado o bien
                /// que se haya iniciado hace mas de N minutos
                /// </summary>
        
                /// ***********************************************************************************************
                public static bool PuedeIniciarProceso(string nombreProceso, out string sCausaInicio)
                {
                    bool bPuedeIniciar = false;


                    sCausaInicio = "";

                    try
                    {
                        LogActividadL oLogs = null;                     // Variable objeto para consulta de actividad
                        LogActividad oLogActividadProceso = null;
                        bool estaEjecutandoseProceso = true;


                        // No alcanza con chequear que solo haya una instancia, si se abre en dos sesiones diferentes no se detecta.
                        // Grabamos y revisamos en la base de datos cuando se inicia y detiene la aplicacion


                        // La primera vez NO hay ningun logueo previo, lo contemplamos
                        oLogs = LogActividadBS.getLogActividad(nombreProceso);
                        if (oLogs.Count > 0)
                        {
                            oLogActividadProceso = oLogs[0];
                            estaEjecutandoseProceso = oLogActividadProceso.esEjecutandoProcesoActual;
                        }
                        else
                        {
                            estaEjecutandoseProceso = false;
                        }



                        if (estaEjecutandoseProceso)
                        {
                            sCausaInicio = "Una instancia de la aplicación se está ejecutando: " + "\n"
                                                    + "Terminal: " + oLogActividadProceso.NombreTerminalEjecucionProceso + ", \n"
                                                    + "Sesión: " + oLogActividadProceso.NombreSesionEjecucionProceso + ", \n";

                            // Si la ultima vez que ejcuto el proceso fue hace mas de N minutos, igualmente permitimos que lo vuelva a ejecutar
                            if (DateTime.Now - oLogActividadProceso.FechaUltimoLogueo > new TimeSpan(0, 120, 0))
                            {
                                sCausaInicio += "La ultima ejecucion del proceso fue el " + oLogActividadProceso.FechaUltimoLogueo.ToString()
                                             + ", se inicia el proceso de todos modos";

                                bPuedeIniciar = true;
                            }

                        }
                        else
                        {
                            bPuedeIniciar = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                    return bPuedeIniciar;
                }


         */
        #endregion

    }
}
