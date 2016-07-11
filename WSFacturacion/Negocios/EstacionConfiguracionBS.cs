using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class EstacionConfiguracionBs
    {
        #region CONFIG: Metodos de la Clase de Negocios de la entidad Config.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Configuracion de Estaciones definidas. 
        /// </summary>
        /// <returns>Lista de Configuracion de Estaciones</returns>
        /// ***********************************************************************************************
        public static EstacionConfiguracion getConfig()
        {
            return getConfig(null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Estaciones definidas
        /// </summary>
        /// <param name="bGST">bool - Permite conectarse a GST o la Estación
        /// <returns>Lista de Configuracion de Estaciones definidas</returns>
        /// ***********************************************************************************************
        public static EstacionConfiguracion getConfig(int? xiCodEst)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);
                    return EstacionConfiguracionDT.getConfig(conn, xiCodEst);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Configuracion de Estaciones definidas
        /// </summary>
        /// <param name="oConfig">EstacionConfiguracion - Estructura de la Configuracion de Estaciones para modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfig(EstacionConfiguracion oConfig)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre con transaccion
                    conn.ConectarPlaza(true);

                    //Modificamos Config
                    EstacionConfiguracionDT.updConfig(oConfig, conn);

                    //Forzamos a cargar nuevamente la estacion
                    ConexionBs.SetNumeroEstacion(oConfig.IdEstacion);
                    
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfiguracionEstacion(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oConfig),
                                                           getAuditoriaDescripcion(oConfig)),
                                                           conn);
                    
                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaConfiguracionEstacion()
        {
            return "CGE";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(EstacionConfiguracion oConfig)
        {
            return oConfig.IdEstacion.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(EstacionConfiguracion oConfig)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Ascendente Hacia", oConfig.SentidoAsc);
            AuditoriaBs.AppendCampo(sb, "Descendente Hacia", oConfig.SentidoDesc);
            if (oConfig.EstacionDecalada == "S")
            {
                AuditoriaBs.AppendCampo(sb, "Nº Via Comienzo Decalado", oConfig.ViaDecalada.ToString());
            }
            AuditoriaBs.AppendCampo(sb, "Tiempo para Evento de Via Inactiva  (min)", oConfig.TiempoViaInactiva.ToString());
            AuditoriaBs.AppendCampo(sb, "Discrepancia para Alarma", oConfig.DiscrepanciaAlarma.ToString());
            AuditoriaBs.AppendCampo(sb, "Cantidad Máxima de Ticket adelantado (asc)", oConfig.TicketAdAsc.ToString());
            AuditoriaBs.AppendCampo(sb, "Cantidad Máxima de Ticket adelantado (desc)", oConfig.TicketAdDesc.ToString());
            AuditoriaBs.AppendCampo(sb, "Primer carril a la izquierda", Traduccion.getSiNo(oConfig.PrimerCarrilalaIzquierda ));
            AuditoriaBs.AppendCampo(sb, "Ascendente hacia arriba", Traduccion.getSiNo(oConfig.AscendenteHaciaArriba));
            AuditoriaBs.AppendCampo(sb, "La estación posee Tesorero", Traduccion.getSiNo(oConfig.esEstacionConTesorero));
            AuditoriaBs.AppendCampo(sb, "Código de Estación de la Antena", oConfig.PlazaAntena);
            AuditoriaBs.AppendCampo(sb, "Código de BEACON de la Antena", oConfig.BeaconId);

            return sb.ToString();
        }

        #endregion

        #endregion

        #region CONFIGALARMA: Metodos de la Clase de Negocios de la entidad ConfiguracionAlarma.
                
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Configuracion de Alarmas definidas. 
        /// </summary>
        /// <returns>Lista de Configuracion de Alarmas</returns>
        /// ***********************************************************************************************
        public static AlarmaConfiguracionL getConfigAlarma()
        {
            return getConfigAlarma(ConexionBs.getGSToEstacion());
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Alarmas definidas
        /// </summary>
        /// <param name="bGST">bool - Permite conectarse a GST o la Alarmas
        /// <returns>Lista de Configuracion de Alarmas definidas</returns>
        /// ***********************************************************************************************
        public static AlarmaConfiguracionL getConfigAlarma(bool bGST)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarPlaza(false);
                return EstacionConfiguracionDT.getConfigAlarma(conn);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Configuracion de Alarmas definidas
        /// </summary>
        /// <param name="oConfig">EstacionConfiguracion - Estructura de la Configuracion de Alarmas para modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfigAlarma(AlarmaConfiguracionL oConfigAlarmaL)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre con transaccion
                conn.ConectarPlaza(true);

                //Modificamos Config

                foreach (AlarmaConfiguracion oConfig in oConfigAlarmaL)
                {
                    EstacionConfiguracionDT.updConfigAlarma(oConfig, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfiguracionAlarma(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(oConfig),
                                                            getAuditoriaDescripcion(oConfig)),
                                                            conn);
                }

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaConfiguracionAlarma()
        {
            return "ALA";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(AlarmaConfiguracion oConfigAlarma)
        {
            return oConfigAlarma.TipoAlarma.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(AlarmaConfiguracion oConfigAlarma)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Duración sirena (seg)", oConfigAlarma.DuracionSirena.ToString());
            AuditoriaBs.AppendCampo(sb, "Duración alarma visual (seg)", oConfigAlarma.DuracionAlarmaVisual.ToString());
            AuditoriaBs.AppendCampo(sb, "Tipo de sonido", oConfigAlarma.TipoSonido.ToString());
            AuditoriaBs.AppendCampo(sb, "Anulación por teclado", oConfigAlarma.AnulacionTeclado.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion

        #region DISPLAY: Metodos de la Clase de Negocios de  VELOCIDAD.

        public static DisplayL getMensajesDisplay()
        {
            return getMensajesDisplay(ConexionBs.getGSToEstacion(), null, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas mensajes Display definidos. 
        /// </summary>
        /// <param name="Codigo">String - Permite filtrar por un mensaje Display
        /// <param name="Sentido">String - Permite filtrar por un mensaje Display
        /// <returns>Lista de Mensajes del Display</returns>
        /// ***********************************************************************************************
        ///    
        public static DisplayL getMensajesDisplay(string Codigo, string Sentido)
        {
            return getMensajesDisplay(ConexionBs.getGSToEstacion(), Codigo, Sentido);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Mensajes del Display
        /// </summary>
        /// <param name="bGST">bool - Permite conectarse a GST o la Estación
        /// <param name="Codigo">String - Permite filtrar por un mensaje Display
        /// <param name="Sentido">String - Permite filtrar por un mensaje Display
        /// <returns>Lista de Mensajes del Display</returns>
        /// ***********************************************************************************************
        public static DisplayL getMensajesDisplay(bool bGST, string Codigo, string Sentido)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarPlaza(false);
                return EstacionConfiguracionDT.getMensajesDisplay(conn, Codigo, Sentido);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de Mensajes del Display
        /// </summary>
        /// <param name="oDisplay">Display - Estructura de Mensajes del Display
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updMensajeDisplay(Display oDisplay)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre con transaccion
                conn.ConectarPlaza(true);

                //Modificamos Config
                EstacionConfiguracionDT.updMensajeDisplay(oDisplay, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensajeDisplay(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oDisplay),
                                                        getAuditoriaDescripcion(oDisplay)),
                                                        conn);
                    
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaMensajeDisplay()
        {
            return "DIS";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Display oDisplay)
        {
            return oDisplay.IdCodigo.ToString() + " " + oDisplay.CodigoSentido.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Display oDisplay)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Activo", oDisplay.Activo.ToString());
            AuditoriaBs.AppendCampo(sb, "Titilante", Traduccion.getSiNo(oDisplay.esTitilante));
            AuditoriaBs.AppendCampo(sb, "Letra Ancha", Traduccion.getSiNo(oDisplay.esLetraAncha));
            //TODO descripcion Velocidad y Efectos
            AuditoriaBs.AppendCampo(sb, "Velocidad", oDisplay.Velocidad.ToString());
            AuditoriaBs.AppendCampo(sb, "Efectos", oDisplay.Efectos.ToString());
            AuditoriaBs.AppendCampo(sb, "Mensaje", oDisplay.Texto.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion

        #region VELOCIDAD: Metodos de la Clase de Negocios de la entidad Display.

        /// <summary>
        /// Devuelve la lista de todos los tipos de Velocidad. 
        /// </summary>
        /// <returns>Lista de tipos de Velocidad</returns>
        /// ***********************************************************************************************
        public static DisplayVelocidadL getDiplayVelocidades()
        {
            DisplayVelocidadL oVelocidadL = new DisplayVelocidadL();
            oVelocidadL = EstacionConfiguracionDT.getVelocidades();

            return oVelocidadL;
        }           

        #endregion

        #region EFECTOS: Metodos de la Clase de Negocios de la entidad efectos del Display.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Efectos. 
        /// </summary>
        /// <returns>Lista de Efectos</returns>
        /// ***********************************************************************************************
        public static DisplayEfectosL getDisplayEfectos()
        {
            DisplayEfectosL oEfectoL = new DisplayEfectosL();
            oEfectoL = EstacionConfiguracionDT.getEfectos();
         
            return oEfectoL;
        }
        
        #endregion

        #region TURNOTRABAJO: Clase de Negocios de los Turnos de Trabajo de la estacion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TODOS los Turnos de trabajo de una estacion.
        /// </summary>
        /// <returns>Lista de Turnos de Trabajo</returns>
        /// ***********************************************************************************************
        public static TurnoTrabajoL getTurnosTrabajo()
        {
            return getTurnosTrabajo(null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TODOS los Turnos de trabajo de una estacion.
        /// </summary>
        /// <param name="Turno">int - Numero de turno. La estacion la toma de variables internas</param>
        /// <returns>Lista de Turnos de Trabajo</returns>
        /// ***********************************************************************************************
        public static TurnoTrabajoL getTurnosTrabajo(byte? Turno)
        {
            return getTurnosTrabajo(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), Turno);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TODOS los Turnos de trabajo de una estacion.
        /// </summary>
        /// <param name="estacion">int - Numero de Estacion </param>
        /// <returns>Lista de Turnos de Trabajo</returns>
        /// ***********************************************************************************************
        public static TurnoTrabajoL getTurnosTrabajoEstacion(int estacion)
        {
            return getTurnosTrabajo(ConexionBs.getGSToEstacion(), estacion, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TODOS los Turnos de trabajo de una estacion.
        /// </summary>
        /// <param name="bConsolidado">bool - Si se trata de una conexion con consolidado o la estacion</param>
        /// <param name="Estacion">int - Numero de estacion de la cual se filtran los turnos. Si no se pasa, trae todos</param>       
        /// <param name="Turno">int - Numero de turno. Si no se pasa, trae todos</param>
        /// <returns>Lista de Turnos de Trabajo</returns>
        /// ***********************************************************************************************
        public static TurnoTrabajoL getTurnosTrabajo(bool bConsolidado, int? Estacion, byte? Turno)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(bConsolidado, false);
                return EstacionConfiguracionDT.getTurnosTrabajo(conn, Estacion, Turno);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el numero del proximo turno a generar 
        /// </summary>
        /// <returns>Numero del Proximo turno a generar</returns>
        /// ***********************************************************************************************
        public static byte getProximoNumeroTurno()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarPlaza(false);
                return EstacionConfiguracionDT.getProximoNumeroTurno(conn, ConexionBs.getNumeroEstacion());
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Turno de Trabajo
        /// </summary>
        /// <param name="oTurnoTrabajo">TurnoTrabajo - Objeto del turno de trabajo a insertar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addTurnoTrabajo(TurnoTrabajo oTurnoTrabajo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la Estacion, con transaccion
                conn.ConectarPlaza(true);

                //Agregamos el Turno de Trabajo
                EstacionConfiguracionDT.addTurnoTrabajo(oTurnoTrabajo, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oTurnoTrabajo),
                                                        getAuditoriaDescripcion(oTurnoTrabajo)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de un Turno de Trabajo.
        /// </summary>
        /// <param name="oTurnoTrabajo">TurnoTrabajo - Objeto del turno de trabajo a modificar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updTurnoTrabajo(TurnoTrabajo oTurnoTrabajo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la Estacion, con transaccion
                conn.ConectarPlaza(true);

                //Modificamos el Turno de Trabajo
                EstacionConfiguracionDT.updTurnoTrabajo(oTurnoTrabajo, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oTurnoTrabajo),
                                                        getAuditoriaDescripcion(oTurnoTrabajo)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Baja de un Turno de Trabajo
        /// </summary>
        /// <param name="oTurnoTrabajo">TurnoTrabajo - Objeto del turno de trabajo a eliminar</param>
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delTurnoTrabajo(TurnoTrabajo oTurnoTrabajo, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la Estacion, con transaccion
                conn.ConectarPlaza(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "TESTUR", 
                                                    new string[] { oTurnoTrabajo.Estacion.Numero.ToString(), oTurnoTrabajo.NumeroTurno.ToString() },
                                                    new string[] { },
                                                    nocheck);

                //Eliminamos el Turno de Trabajo
                EstacionConfiguracionDT.delTurnoTrabajo(oTurnoTrabajo, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oTurnoTrabajo),
                                                        getAuditoriaDescripcion(oTurnoTrabajo)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "TES";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(TurnoTrabajo oTurnoTrabajo)
        {
            return oTurnoTrabajo.NumeroTurno.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(TurnoTrabajo oTurnoTrabajo)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Hora Inicial Turno" + " " + oTurnoTrabajo.NumeroTurno.ToString(), oTurnoTrabajo.HoraString);
            AuditoriaBs.AppendCampo(sb, "Tolerancia Anterior", oTurnoTrabajo.HoraAnterior);
            AuditoriaBs.AppendCampo(sb, "Fecha Anterior", Traduccion.getSiNo(oTurnoTrabajo.DiaAnterior));
            AuditoriaBs.AppendCampo(sb, "Tolerancia Posterior", oTurnoTrabajo.HoraPosterior); 
            AuditoriaBs.AppendCampo(sb, "Fecha Posterior", Traduccion.getSiNo(oTurnoTrabajo.DiaPosterior));
                
            return sb.ToString();
        }

        #endregion

        #endregion
    }
}
