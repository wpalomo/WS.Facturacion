using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class JornadaBs
    {
         #region Cierre Jornada


        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene la primera jornada a cerrar
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// ***********************************************************************************************
        public static DateTime getJornadaACerrar(int? estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return JornadaDt.getJornadaACerrar(conn, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas en un periodo
        /// </summary>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="estado">String - estado jornada</param>
        /// <param name="porEstacion">bool - Por Estacion</param>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************
        public static JornadaL getJornadasCierre(int? estacion, DateTime jornadaDesde, DateTime jornadaHasta, string estado)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    //return JornadaDt.getJornadasCierre(conn, estacion, jornadaDesde, jornadaHasta, estado, !ConfigValidacion.ValidacionCentralizada);
                    //AJC: En VPR vamos a mostrar por estacion aun si es centralizada la validacion
                    return JornadaDt.getJornadasCierre(conn, estacion, jornadaDesde, jornadaHasta, estado, ConfigValidacion.CierreJornadaPorEstacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// cierra una jornada
        /// </summary>
        /// <param name="plaza"></param>
        /// <param name="estacion"></param>
        /// <param name="fechaCierre"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ProblemasCierreJornadaL setCierreJornada(Estacion estacion, DateTime fechaJornada, string modo)
        {
            ProblemasCierreJornadaL oProblemas = new ProblemasCierreJornadaL();
            bool facturoFallos = false;

            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    oProblemas = getProblemasCierreJornada(estacion.Numero, fechaJornada, modo);

                    

                    // si no hay problemas 
                    if (oProblemas.Count == 0)
                    {
                        //Creamos las notas debito y credito - VER COMO FUNCIONARA ESTO
                        //facturoFallos = ValPartesDt.setFacturasValidacion(conn, estacion.Numero, fechaJornada, ConexionBs.getUsuario());

                        //Cerramos jornada
                        JornadaDt.setCierreJornada(conn, ConexionBs.getUsuario(), estacion.Numero, fechaJornada, ConexionBs.getTerminal());


                        //Auditoria
                        using (Conexion connAud = new Conexion())
                        {
                            //conn.ConectarGSTThenPlaza();
                            connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                            // Ahora tenemos que grabar la auditoria:
                            //TODO si es a via cerrada grabar otro codigo
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCerrarJornada(),
                                                "A",
                                                getAuditoriaCodigoRegistro(estacion),
                                                getAuditoriaDescripcion(estacion, fechaJornada) +
                                                    (modo.Equals("M") ? " - CIERRE FORZADO MANUALMENTE " : "") +
                                                    (facturoFallos ? "- Generó Facturas" : "")
                                                ),
                                                connAud);

                        }

                    }

                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oProblemas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="fechaJornada"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static string getAuditoriaDescripcion(Estacion estacion, DateTime fechaJornada)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estación", estacion.Nombre);
            AuditoriaBs.AppendCampo(sb, "Fecha Jornada", fechaJornada.ToString("dd/MM/yyyy"));

            return sb.ToString();
        }

        /// ***********************************************************************************************
        /// ***********************************************************************************************
        private static string getAuditoriaCodigoAuditoriaCerrarJornada()
        {
            return "CRJ";
        }


        /// ***********************************************************************************************
        /// ***********************************************************************************************
        private static string getAuditoriaCodigoRegistro(Estacion estacion)
        {
            return estacion.Numero.ToString();
        }

        #endregion


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas de un periodo
        /// </summary>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************
        public static JornadaL getJornadas(int? estacion, DateTime jornadaDesde, DateTime jornadaHasta)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidado(ConexionBs.getGSToEstacion(), false);

                    return getJornadas(conn, estacion, jornadaDesde, jornadaHasta);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas de un periodo
        /// </summary>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************
        public static JornadaL getJornadaParte(int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return JornadaDt.getJornadaParte(conn, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el estado de una jornada
        /// </summary>
        /// <param name="conn">Conexion - Conexion a la base de datos</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada</param>
        /// <returns>Estado de una jornada</returns>
        /// ***********************************************************************************************
        public static string GetEstadoJornada(int estacion, DateTime jornada)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return JornadaDt.GetEstadoJornada(conn, estacion, jornada);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas de un periodo
        /// </summary>
        /// <param name="conn">Conexion - Conexion a la base de datos</param>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************
        public static JornadaL getJornadas(Conexion conn, int? estacion, DateTime jornadaDesde, DateTime jornadaHasta)
        {
            try
            {
                return JornadaDt.getJornadas(conn, estacion, jornadaDesde, jornadaHasta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Revisa si una jornada estaba cerrada
        /// </summary>
        /// <param name="jornada">DateTime - Jornada</param>
        /// <returns>true si estaba cerrada</returns>
        /// ***********************************************************************************************
        public static bool EstaCerrada(int? estacion, DateTime jornada)
        {
            bool ret = false;
            JornadaL oJornadas = getJornadas(estacion, jornada, jornada);
            if (oJornadas.Count > 0)
            {
                if (oJornadas[0].Status == Jornada.enmStatus.enmCerrada)
                {
                    ret = true;
                }
            }

            return ret;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Consulta si una jornada esta cerrada    
        /// </summary>
        /// <param name="jornada"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool EstaCerrada(DateTime jornada)
        {
            return EstaCerrada(ConexionBs.getNumeroEstacion(), jornada);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Revisa si una jornada de un parte estaba cerrada
        /// </summary>
        /// <param name="jornada">DateTime - Jornada</param>
        /// <returns>true si estaba cerrada</returns>
        /// ***********************************************************************************************
        public static bool EstaCerradaParte(int parte)
        {
            bool ret = false;
            JornadaL oJornadas = getJornadaParte(parte);
            if (oJornadas.Count > 0)
            {
                if (oJornadas[0].Status == Jornada.enmStatus.enmCerrada)
                {
                    ret = true;
                }
            }

            return ret;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Verifica que una jornada no esté cerrada
        /// En caso de estarlo genera una excepcion
        /// </summary>
        /// <param name="conn">Conexion - Conexion a la base de datos</param>
        /// <param name="jornada">DateTime - Jornada</param>
        /// ***********************************************************************************************
        public static void VerificaJornadaAbierta(Conexion conn, DateTime jornada)
        {
            JornadaL oJornadas = getJornadas(conn, ConexionBs.getNumeroEstacion(), jornada, jornada);
            if (oJornadas.Count > 0)
            {
                if (oJornadas[0].Status == Jornada.enmStatus.enmCerrada)
                {
                    throw new ErrorJornadaCerrada(Traduccion.Traducir( "La jornada se encuentra cerrada"));
                }
            }

            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Esta interfaseada
        /// </summary>
        /// <param name="jornada">DateTime - Jornada</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void setInterfaceJornada(DateTime jornada)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {

                   conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                   JornadaDt.setInterfaceJornada(conn, ConexionBs.getNumeroEstacion(), jornada);

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
        /// Verifica la proxima jornada a cerrar, segun la que ya se cerro
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="oProblemas"></param>
        /// <param name="jornada"></param>
        /// <param name="estacion"></param>
        /// ***********************************************************************************************
        private static void getVerificacionFechaACerrar(Conexion conn, ProblemasCierreJornadaL oProblemas, DateTime jornada, int? estacion)
        {
            //no se puede cerrar una jornada con fecha posterior a la actual
            if( jornada > DateTime.Now)
                oProblemas.Add( new ProblemasCierreJornada(Traduccion.Traducir("No se puede cerrar una jornada con fecha posterior a la actual"),"") );
                

            //Debe ser igual a la proxima jornada a cerrar
            DateTime primerJornadaACerrar = JornadaDt.getJornadaACerrar(conn, estacion);
            if(!primerJornadaACerrar.Equals(jornada))
                oProblemas.Add(new ProblemasCierreJornada(string.Format(Traduccion.Traducir("La proxima jornada a cerrar debe ser {0}"),primerJornadaACerrar.ToShortDateString() ) , ""));


            if (JornadaBs.EstaCerrada(estacion, jornada))
                oProblemas.Add(new ProblemasCierreJornada(Traduccion.Traducir("La jornada ya se encontraba cerrada."), ""));
        
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Reabre una jornada
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="fechaJornada"></param>
        /// ***********************************************************************************************
        public static void setAbrirJornada(int estacion, DateTime fechaJornada)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    //TODO marcar todo factur_valid como anulada 

                    
                    //JornadaDt.MarcarAnuladasFacturasDeJornada(conn, estacion, fechaJornada);
                    JornadaDt.setAbrirJornada(conn, ConexionBs.getUsuario(), estacion, fechaJornada, ConexionBs.getTerminal());

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
        /// Verifica la reapertura de una jornada previamente cerrada
        /// </summary>
        /// <param name="fechaJornada"></param>
        /// ***********************************************************************************************
        public static void VerificarFechaReapertura(DateTime fechaJornada)
        {
            //TODO realizar validacion

            //DateTime dteFechaLimite  = Date - oConfiguracion.getDatosConfig(rsError)![con_dreap]
            //if (fechaJornada < dteFechaLimite)
            //    throw new ErrorJornadaCerrada(Traduccion.Traducir("La jornada a reabrir es anterior al plazo permitido."));


        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los logs de una estacion y jornada determinada
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="jornada"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static LogL getLogsJornadas(int estacion, DateTime jornada)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return JornadaDt.getLogsJornadas(conn, estacion, jornada);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los problemas del cierre de una jornada
        /// </summary>
        /// <param name="estacion">Estacion a cerrar</param>
        /// <param name="jornada">Jornada a cerrar</param>
        /// <param name="modo">Modo en que se realiza el cierre (manual o Forzado)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ProblemasCierreJornadaL getProblemasCierreJornada(int? estacion, DateTime jornada, string modo)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return getProblemasCierreJornada(conn, estacion, jornada, modo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de problemas por los cuales NO se puede cerrar la jornada
        /// </summary>
        /// <param name="conn">Objeto de conexion</param>
        /// <param name="estacion">Numero de estacion a chequear</param>
        /// <param name="jornada">Jornada que se esta deseando cerrar</param>
        /// <param name="modo">Modo en que se realiza el cierre (manual o Forzado)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ProblemasCierreJornadaL getProblemasCierreJornada(Conexion conn, int? estacion, DateTime jornada, string modo)
        {

            // Lista de problemas de cierre
            ProblemasCierreJornadaL oProblemas = new ProblemasCierreJornadaL();



            // Verifica el estado de la jornada y la proxima jornada a cerrar
            // Se verifica igual, aunque el modo sea "M" (cierre forzado)
            getVerificacionFechaACerrar(conn, oProblemas, jornada, estacion);


            // Las siguientes validaciones las realiza si el modo NO es M (cierre forzado)
            if (!modo.Equals("M"))
            {
                // Verifica partes sin liquidar
                JornadaDt.getPartesSinLiquidar(conn, oProblemas, estacion, jornada);


                // Verifica Bloques sin liquidar
                JornadaDt.getBloquesSinLiquidar(conn, oProblemas, estacion, jornada);


                // Verifica Anomalias sin valida
                JornadaDt.getAnomaliasSinValidar(conn, oProblemas, estacion, jornada);


                // Verifica que el parte no se encuentre validandose actualmente
                JornadaDt.getPartesValidandose(conn, oProblemas, estacion, jornada);


                // Verifica diferencias entre lo calculado en Partes_valid y el valor de cada parte indivudual
                JornadaDt.getVerificarDiferencias(conn, oProblemas, estacion, jornada);


                JornadaDt.getVerificarReposicionesNoPedidas(conn, oProblemas, estacion, jornada);

                //Verifica que no exista reposiciones con valores de reposicion incoherentes.
                JornadaDt.getVerificarReposicionesMalPedidas(conn, oProblemas, estacion, jornada);


                //JornadaDt.getVerificarDiferenciasFallo(conn, oProblemas, estacion, jornada);


                //JornadaDt.getVerificarFalloPendiente(conn, oProblemas, estacion, jornada);


                // Verifica que no haya tickets faltantes
                //JornadaDt.GetTicketsFaltantes(conn, oProblemas, estacion, jornada);


                // Verifica que no haya tickets duplicados
                //JornadaDt.GetTicketsDuplicados(conn, oProblemas, estacion, jornada);


                // Verifica que no haya registros de facturas rechazadas por SUNAT
                //JornadaDt.GetFacturasRechazadasSUNAT(conn, oProblemas, estacion, jornada);

            }


            return oProblemas;

        }


        public static bool esFechaDeJornada(Conexion conn, DateTime fechaDesde, DateTime fechaHasta, DateTime jornada, Estacion estacion)
        {
            return JornadaDt.esFechaDeJornada(conn, fechaDesde, fechaHasta, jornada, estacion);
        }
    }
}
