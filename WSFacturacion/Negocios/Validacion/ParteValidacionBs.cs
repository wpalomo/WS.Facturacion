using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data;

namespace Telectronica.Validacion
{
    public class ParteValidacionBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada</param>     
        /// <param name="estadoParte">string - EstadoParte</param>
        /// <param name="estadoValidacion">string - EstadoValidacion</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static ParteValidacionL getPartesValidacion(Estacion estacion, DateTime jornada, string estadoParte, string estadoValidacion,string estadoFallo)
        {
            try
            {
                //Se hace en un Job porque si todos lo hacen a la vez se bloquea
                /*
                //si estamos donde se valida recalculamos la jornada
                if (ConfigValidacion.getEsValidacionLocal(ConexionBs.getGSToEstacion()))
                {
                    //Solo recalculamos si la jornada esta abierta
                    if (!JornadaBs.EstaCerrada(estacion.Numero, jornada))
                    {
                        using (Conexion conn = new Conexion())
                        {
                            //sin transaccion
                            conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                            //calculamos los totales para los partes buscados
                            ParteValidacionDt.setCalcularJornadaInicio(conn, estacion.Numero, jornada, null);
                        }
                    }
                } 
                */
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ParteValidacionDt.getPartesValidacion(conn, estacion, jornada, estadoParte, estadoValidacion,estadoFallo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes
        /// </summary>
        /// <param name="numeroParte">int - numeroParte</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static void setComentarParte(ParteValidacion parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    ParteValidacionDt.setComentarParte(conn, parte);
                

                    //Grabamos auditoria
                    //Auditoria
                    using (Conexion connAud = new Conexion())
                    {
                        //conn.ConectarGSTThenPlaza();
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaComentarParte(),
                                                                "M",
                                                                getAuditoriaCodigoRegistro(parte.Numero),
                                                                getAuditoriaDescripcion(parte.Estacion.Numero, parte.Numero, parte.Jornada, parte.Observaciones)),
                                                                connAud);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaComentarParte()
        {
            return "CPA";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(int numParte)
        {
            return numParte.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(int estacion, int parte, DateTime jornada, string comentario)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Parte:", parte.ToString());
            AuditoriaBs.AppendCampo(sb, "Estación:", estacion.ToString());
            AuditoriaBs.AppendCampo(sb, "JornadaContable:", jornada.ToString("dd/mm/yyyy"));
            AuditoriaBs.AppendCampo(sb, "Comentario:", comentario);

            return sb.ToString();
        }



        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes
        /// </summary>
        /// <param name="numeroParte">int - numeroParte</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static ParteValidacionL getPartesValidacionPorNumero(int numeroParte)
        {
            try
            {
                // Agregar recalcular jornada inicio

                //si estamos donde se valida recalculamos la jornada
                if (ConfigValidacion.getEsValidacionLocal(ConexionBs.getGSToEstacion()))
                {
                    //Solo recalculamos si la jornada esta abierta
                    if (!JornadaBs.EstaCerradaParte(numeroParte))
                    {
                        using (Conexion conn = new Conexion())
                        {
                            //sin transaccion
                            conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                            //calculamos los totales para los partes buscados
                            ParteValidacionDt.setCalcularJornadaInicio(conn, null, null, numeroParte);
                        }
                    }
                } 

                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ParteValidacionDt.getPartesValidacionPorNumero(conn, numeroParte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los tipos de anomalias de un parte
        /// </summary>
        /// <param name="estacion">Estacion - estacion</param>
        /// <param name="parte">int - Parte</param>
        /// <param name="jornada">DateTime - Jornada</param>   
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getTiposAnomalias(Estacion estacion, int parte,char verInvisible, char SoloPendientes)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ParteValidacionDt.getTiposAnomalias(conn, estacion, parte,verInvisible, SoloPendientes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los tipos de anomalias Pendientes de un parte
        /// </summary>
        /// <param name="estacion">Estacion - estacion</param>
        /// <param name="parte">int - Parte</param>
        /// <param name="jornada">DateTime - Jornada</param>   
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getTodasAnomaliasPendientes(Estacion estacion, int parte,char verInvisible)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ParteValidacionDt.getTodasAnomaliasPendientes(conn, estacion, parte,verInvisible);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         */

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias por tipo
        /// </summary>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        //public static DataSet getAnomalias(ParteValidacion parte, int anomalia, bool puedeVerValInvisible, string validador, bool pendientes)
        public static DataSet getAnomalias(ParteValidacion parte, int anomalia, bool puedeVerValInvisible, string validador)
        {
            
            DataSet Anomalias = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    // Si el parte no esta liquidado Llamar al metodo de Datos de validacion ivisible Validacion.usp_Parte_setValidarInvisible
                    if (!parte.EstaLiquidado)
                        ParteValidacionDt.setValidarInvisible(conn, parte, validador, anomalia); 

                    switch (anomalia)
                    {
                        case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_ABIERTA:
                            Anomalias = ViolacionesDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmVIOLAC_SUBE_BARRERA:
                            Anomalias = ViolacionesDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmVIOLAC_QUIEBRE:
                            Anomalias = ViolacionesDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_CERRADA:
                            Anomalias = ViolacionesDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmDACS:
                            Anomalias = DACsDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmEXENTOS:
                            Anomalias = ValExentosDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmSIPS:
                            Anomalias = SIPsDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmCANCELAC_TRANSITO:
                            Anomalias = CancelaTransitoDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;                        
                        case (int)Anomalia.eAnomalia.enmAUTORIZA_PASO:
                            Anomalias = AutorizacionPasoDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmPAGO_DIFERIDO:
                            Anomalias = ValPagoDiferidoDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmTICKET_MANUAL:
                            Anomalias = TicketManualDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;                        
                        case (int)Anomalia.eAnomalia.enmCANCELAC_OTROS:
                            Anomalias = CancelaTransitoDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;                       
                        case (int)Anomalia.eAnomalia.enmDACS_AFAVOR:
                            Anomalias = DACsDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmTAG_MANUAL:
                            Anomalias = TagManualDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmVALES_PREPAGOS:
                            Anomalias = ValValesPrepagosDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        case (int)Anomalia.eAnomalia.enmTRAN_NORMALES:
                            Anomalias = TransitoNormalDt.getAnomalias(conn, anomalia, parte, puedeVerValInvisible);
                            break;
                        default:
                            break;
                    }
                   
                }
                //////peguntamos si solo quiere las anoomalias pendientes
                //if (pendientes)
                //{
                //    DataTable dataTable = Anomalias.Tables[0];
                //    DataView dataView = dataTable.DefaultView;
                //    dataView.RowFilter = "Validada='N'";
                    
                    
                //}
            }
            
            catch (Exception ex)
            {
                throw ex;
            }

            return Anomalias;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de bloques de un parte
        /// </summary>
        /// <returns>Lista de bloques de Partes</returns>
        /// ***********************************************************************************************
        public static DataSet getBloquesParte(ParteValidacion parte, bool? noConsultaEvento)
        {
            DataSet bloques = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    bloques = ParteValidacionDt.getBloquesParte(conn, parte, noConsultaEvento);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de bloques de un parte
        /// </summary>
        /// <returns>Lista de bloques de Partes</returns>
        /// ***********************************************************************************************
        public static DataSet getBloquesParte(ParteValidacion parte)
        {
            return getBloquesParte(parte, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// verifica si un parte esta abierto
        /// </summary>
        /// <param name="estacion">Int - Codigo de la estacion</param>
        /// <param name="parte">Int - Numero de parte</param>
        /// ***********************************************************************************************
        public static bool esParteAbierto(int parte, int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ParteValidacionDt.esParteAbierto(conn, parte, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// verifica el estado de validacion del parte
        /// </summary>
        /// ***********************************************************************************************

        public static void getEstadoValidacion(ParteValidacion parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    ParteValidacionDt.getEstadoValidacion(conn, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static void setValidarInvisible(ParteValidacion parte, string validador)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    ParteValidacionDt.setValidarInvisible(conn, parte,validador);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void SetParteEnValidacion(ParteValidacion parte, string validador)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    ParteValidacionDt.SetParteEnValidacion(conn, parte, validador, ConexionBs.getTerminal());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void SetFinalizarParte(ParteValidacion parte, string validador)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    ParteValidacionDt.SetFinalizarParte(conn, parte, validador, ConexionBs.getTerminal());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> getListaFotos(int idEvento)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ParteValidacionDt.getListaFotos(conn, idEvento);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool EsParteDisponible(ParteValidacion parte, bool liberarParte)
        {

            bool disponible = false;

            if (parte.EstadoValidacion.Estado == "D")
            {
                disponible = true;
            }
            else
                if (!liberarParte)
                {
                    if (parte.EstadoValidacion.Estado == "V")
                    {
                        if (parte.Validador.ID == ConexionBs.getUsuario())
                        {
                        }
                    }
                }

            return disponible;

        }

        ///**********************************************************************************
        /// <summary>
        /// Le setea la reposicion economica al parte por el monto del faltante
        /// </summary>
        /// <param name="ListaPartes"></param>
        ///**********************************************************************************
        public static void setReposicionEconomica(ParteValidacionL ListaPartes)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGST( false);

                    //Por cada parte asignamos el parte
                    foreach (ParteValidacion item in ListaPartes)
                    {
                        if (ParteValidacionDt.setReposicionEconomica(conn, item))
                        {
                            ////Grabamos auditoria
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaReposicion(),
                                                                   "A",
                                                                   getAuditoriaCodigoRegistro(item),
                                                                   getAuditoriaDescripcion(item)),
                                                                   conn);
                        }
                        else
                        {;
                            //Agregamos el parte a la lista de problemas
                            //oPartesMal.Add(oParte);
                        }
                    }

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                    }
                }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }


        ///********************************************************************************
        /// <summary>
        /// Elimina la reposicion economica
        /// </summary>
        /// <param name="ListaPartes"></param>
        ///********************************************************************************
        public static void delReposicionEconomica(ParteValidacionL ListaPartes)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGST(false);

                    //Por cada parte asignamos el parte
                    foreach (ParteValidacion item in ListaPartes)
                    {
                        if (ParteValidacionDt.delReposicionEconomica(conn, item))
                        {
                            ////Grabamos auditoria
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaReposicion(),
                                                                   "B",
                                                                   getAuditoriaCodigoRegistro(item),
                                                                   getAuditoriaDescripcion(item)),
                                                                   conn);
                        }
                        else
                        {
                            ;
                            //Agregamos el parte a la lista de problemas
                            //oPartesMal.Add(oParte);
                        }
                    }

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        ///**********************************************************************************
        /// <summary>
        /// Codigo de la audigoria
        /// </summary>
        /// <returns></returns>
        ///***********************************************************************************
        private static string getAuditoriaCodigoAuditoriaReposicion()
        {
            return "REP";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ParteValidacion oParte)
        {
            return oParte.Numero.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ParteValidacion oParte)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Parte", oParte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Fejor", oParte.Jornada.ToString("yyyyMMdd"));
            AuditoriaBs.AppendCampo(sb, "Faltante", oParte.Faltante.ToString("C"));
            AuditoriaBs.AppendCampo(sb, "Peajista", oParte.Peajista.ID.ToString());
            AuditoriaBs.AppendCampo(sb, "Validador", oParte.Validador.ID.ToString());
            AuditoriaBs.AppendCampo(sb, "Tipo", "E");

            return sb.ToString();
        }
    }
}
