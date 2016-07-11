using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de Vias
    /// </summary>
    ///****************************************************************************************************
    public class ViaBs
    {
        #region DEFINICION: Definición de las vías de la estación

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la cantidad de carriles para la estación indicada
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 15/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - string? - Permite filrar por código de estación
        // ----------------------------------------------------------------------------------------------

        /// <summary>
        /// Devuelve una lista de carriles
        /// </summary>
        /// <param name="xiEstacion"></param>
        /// <returns></returns>
        public static CarrilL getCarriles(int? xiEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.getCarriles(conn, xiEstacion);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las vias de una estacion
        /// </summary>
        /// <returns>Lista de Vias</returns>
        /// ***********************************************************************************************
        public static ViaDefinicionL getVias()
        {
            return getVias(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las vias de la estacion actual. 
        /// </summary>
        /// <param name="numeroVia">Int - Permite filtrar por una Via determinada
        /// <returns>Lista de Vias</returns>
        /// ***********************************************************************************************
        public static ViaDefinicionL getVias(int? numeroVia)
        {
            return getVias(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), numeroVia);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las vias de una estacion. 
        /// </summary>
        /// <param name="estacion">Int - Estacion
        /// <returns>Lista de Vias</returns>
        /// ***********************************************************************************************
        public static ViaDefinicionL getViasEstacion(int estacion)
        {
            return getVias(ConexionBs.getGSToEstacion(), estacion, null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Vias definidas
        /// </summary>
        /// <param name="bConsolidado">bool - Indica si se debe conectar con Consolidado o Estacion</param>
        /// <param name="numeroEstacion">Int - Permite filtrar por una estacion
        /// <param name="numeroVia">Int - Permite filtrar por una Via determinada
        /// <returns>Lista de Colores</returns>
        /// ***********************************************************************************************
        public static ViaDefinicionL getVias(bool bConsolidado, int numeroEstacion, int? numeroVia)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(bConsolidado, false);

                return ViaDt.getVias(conn, numeroEstacion, numeroVia);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Via
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Estructura de la Via a insertar
        /// ***********************************************************************************************
        public static void addVia(ViaDefinicion oViaDefinicion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la Plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verificamos punto de venta no esxiste
                ListaPuntosVenta oPV =VerificaPuntoVenta(conn, oViaDefinicion.PuntoVenta, "VIA", oViaDefinicion.NumeroVia);
                if( oPV != null )
                {
                    string msg = string.Format("El punto de venta ya está siendo usado por la {0} {1}",
                        (oPV.Origen == "VIA") ? "Vía" : "Impresora de Facturación", oPV.Numvia);
                    throw new Telectronica.Errores.ErrorSPException(msg);
                }

                //Agregamos la Via
                ViaDt.addVia(oViaDefinicion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oViaDefinicion),
                                                        getAuditoriaDescripcion(oViaDefinicion)),
                                                        conn);

                //Grabo OK, ya habiamos hecho COMMIT
                conn.Finalizar(true);
            }

            //Fuera de la transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarPlaza(false);

                //Agregamos el login de Sql para la via insertada 
                string sPassword = Encriptado.EncriptarPassword(oViaDefinicion.UsuarioSQL.ToUpper());

                ViaDt.addLoginSql(oViaDefinicion.UsuarioSQL, sPassword, conn);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Via
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Estructura de la Via a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updVia(ViaDefinicion oViaDefinicion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verificamos punto de venta no esxiste
                ListaPuntosVenta oPV = VerificaPuntoVenta(conn, oViaDefinicion.PuntoVenta, "VIA", oViaDefinicion.NumeroVia);
                if (oPV != null)
                {
                    string msg = string.Format("El punto de venta ya está siendo usado por la {0} {1}",
                        (oPV.Origen == "VIA") ? "Vía" : "Impresora de Facturación", oPV.Numvia);
                    throw new Telectronica.Errores.ErrorSPException(msg);
                }

                //Modificamos la Via
                ViaDt.updVia(oViaDefinicion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oViaDefinicion),
                                                        getAuditoriaDescripcion(oViaDefinicion)),
                                                        conn);

                //Grabo OK 
                conn.Finalizar(true);
            }

            //Fuera de la transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarPlaza(false);

                //Actualizamos el login de Sql para la via insertada 
                string sPassword = Encriptado.EncriptarPassword(oViaDefinicion.UsuarioSQL.ToUpper());

                ViaDt.addLoginSql(oViaDefinicion.UsuarioSQL, sPassword, conn);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Via
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Estructura de la Via a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delVia(ViaDefinicion oViaDefinicion, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "VIADEF", 
                                                    new string[] { oViaDefinicion.Estacion.Numero.ToString(), oViaDefinicion.NumeroVia.ToString() },
                                                    new string[]  { "COMANDOS", "DISJUS", "EVENTO", "HISVIA", "SQLMAL", "TOHORA_VALID", "TOHORA", "TRANSITOS", "TRCCTE", "TRASUB", "TURNOS", "VERTRA", "VIAANO", "VIADIS", "VIAEST", "VIASXX", "VIATRA", "VIAVER", "MENREC", "HABEXE" },
                                                    nocheck);

                //Eliminamos la Via
                ViaDt.delVia(oViaDefinicion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                    "B",
                                    getAuditoriaCodigoRegistro(oViaDefinicion),
                                    getAuditoriaDescripcion(oViaDefinicion)),
                                    conn);

                //Grabo OK 
                conn.Finalizar(true);
            }
            //Fuera de la transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarPlaza(false);

                //Eliminamos el login de Sql para la via insertada (fuera de la transaccion)                   
                ViaDt.delLoginSql(oViaDefinicion.UsuarioSQL, conn);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Método encargado de retornar un objeto del tipo ListaPuntosVenta
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="puntoventa"></param>
        /// <param name="origen"></param>
        /// <param name="via"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ListaPuntosVenta VerificaPuntoVenta(Conexion conn, string puntoventa, string origen, int via)
        {
            ListaPuntosVenta oRet = null;
            if (puntoventa != null && puntoventa.Trim() != "")
            {
                int pv = int.Parse(puntoventa);
                ListaPuntosVentaL oPVs = InterfaceDT.getInterfacesPuntosVenta(conn, ConexionBs.getNumeroEstacion());
                foreach (ListaPuntosVenta oPV in oPVs)
                {
                    if (!oPV.Deleted && int.Parse(oPV.PuntoVenta) == pv
                        && (oPV.Origen != origen || oPV.Numvia != via))
                    {
                        oRet = oPV;
                        break;
                    }
                }
            }
            return oRet;
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "VIA";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ViaDefinicion oViaDefinicion)
        {
            return oViaDefinicion.NumeroVia.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ViaDefinicion oViaDefinicion)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre Corto", oViaDefinicion.NombreVia.ToString());
            AuditoriaBs.AppendCampo(sb, "Nombre Largo", oViaDefinicion.NombreViaLargo.ToString());
            AuditoriaBs.AppendCampo(sb, "Carril", oViaDefinicion.Carril.ToString());
            AuditoriaBs.AppendCampo(sb, "Modelo", oViaDefinicion.Modelo.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Sentido", oViaDefinicion.SentidoCirculacion.Descripcion);
            //AuditoriaBs.AppendCampo(sb, "Autotabulante", Traduccion.getSiNo(oViaDefinicion.esAutotabulante));
            AuditoriaBs.AppendCampo(sb, "Contador de Ejes", oViaDefinicion.DetectorEjes.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Detector Ruedas Dobles", oViaDefinicion.RuedasDuales.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Sensor de Altura", Traduccion.getSiNo(oViaDefinicion.esSensorAltura));
            AuditoriaBs.AppendCampo(sb, "Tarjeta Magnética", Traduccion.getSiNo(oViaDefinicion.esLectograbador));
            AuditoriaBs.AppendCampo(sb, "Telepeaje", Traduccion.getSiNo(oViaDefinicion.esTelepeaje));
            AuditoriaBs.AppendCampo(sb, "Tarjeta Chip", Traduccion.getSiNo(oViaDefinicion.esTarjetaChip));
            AuditoriaBs.AppendCampo(sb, "VisaCash", Traduccion.getSiNo(oViaDefinicion.esVisaCash));
            AuditoriaBs.AppendCampo(sb, "Recibe Clearing", Traduccion.getSiNo(oViaDefinicion.esAceptaClearing));
            AuditoriaBs.AppendCampo(sb, "Imprime Ticket Clearing", Traduccion.getSiNo(oViaDefinicion.esImprimeClearing));
            AuditoriaBs.AppendCampo(sb, "Recibe Venta Anticipada", Traduccion.getSiNo(oViaDefinicion.esAceptaVentaAnticipada));
            AuditoriaBs.AppendCampo(sb, "Imprime Venta Anticipada", Traduccion.getSiNo(oViaDefinicion.esImprimeVentaAnticipada));
            AuditoriaBs.AppendCampo(sb, "Cámara Lateral", oViaDefinicion.VideoCamara1.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Cámara Frontal", oViaDefinicion.VideoCamara2.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Directorio de Video", oViaDefinicion.PathArchivosVideo);
            AuditoriaBs.AppendCampo(sb, "Distancia entre Sensores", oViaDefinicion.DistanciaSensores.ToString());
            AuditoriaBs.AppendCampo(sb, "Punto de Venta", oViaDefinicion.PuntoVenta.ToString());
            AuditoriaBs.AppendCampo(sb, "Habilita Exentos", Traduccion.getSiNo(oViaDefinicion.CobroExento));
            AuditoriaBs.AppendCampo(sb, "Habilita Modo Comboio", Traduccion.getSiNo(oViaDefinicion.ModoComboio));
            AuditoriaBs.AppendCampo(sb, "Sensor de Ejes Levantados", Traduccion.getSiNo(oViaDefinicion.ConSensoresEixos));
            AuditoriaBs.AppendCampo(sb, "Directorio de Fotos", oViaDefinicion.PathFotos);

            if (oViaDefinicion.ViaControladora != null)
            {
                AuditoriaBs.AppendCampo(sb, "Controlada por la Vía", oViaDefinicion.ViaControladora.ToString());
            }

            AuditoriaBs.AppendCampo(sb, "Número de Vía en la Antena", oViaDefinicion.ViaAntena);
            
            return sb.ToString();
        }

        #endregion

        #endregion
                
        #region ESTADO: Estado de las vías (Todo lo relacionado al ONLINE)

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de la cantidad de vias en cada estado
        /// </summary>
        /// <param name="bConsolidado">bool - Indica si se debe conectar con Consolidado o Estacion</param>
        /// <param name="numeroEstacion">Int - Permite filtrar por una estacion
        /// <returns>Lista de ViasTotales</returns>
        /// ***********************************************************************************************
        public static ViasTotalesL getViasTotales()
        {
            return getViasTotales(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion());
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de carriles con su estado actual
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 17/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  bGST - bool - Indica si se debe conectar con Gestion o Estacion
        //                                  xiCodEst - int - numero de estacion
        //                                  xiCarril - int - numero de carril
        //                    Retorna: Lista de CarrilStatus: CarrilStatusL
        // ----------------------------------------------------------------------------------------------
        public static CarrilStatusL getEstadoEstacion(bool bGST, int xiCodEst, int? xiCarril, bool xbAscendenteHaciaArriba)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bGST, false);
                return ViaDt.getEstadoEstacion(conn, xiCodEst, xiCarril, xbAscendenteHaciaArriba);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de la cantidad de vias en cada estado
        /// </summary>
        /// <param name="bConsolidado">bool - Indica si se debe conectar con Consolidado o Estacion</param>
        /// <param name="numeroEstacion">Int - Permite filtrar por una estacion
        /// <returns>Lista de ViasTotales</returns>
        /// ***********************************************************************************************
        public static ViasTotalesL getViasTotales(bool bConsolidado, int numeroEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(bConsolidado, false);
                return ViaDt.getViasTotales(conn, numeroEstacion);
            }
        }

        #endregion
        
        #region MODELOSVIA: Modelos en los que se puede tipificar una vía

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista modelos de via
        /// </summary>
        /// <returns>Lista de Todos los Modelos de Via existentes</returns>
        /// ***********************************************************************************************
        public static ViaModeloL getModelosVia()
        {
            return getModelosVia(false, null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista modelos de via
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <param name="ModeloVia">string - Modelo de via a filtrar</param>
        /// <returns>Lista de Modelos de Via existentes</returns>
        /// ***********************************************************************************************
        public static ViaModeloL getModelosVia(bool bConsolidado, string ModeloVia)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(bConsolidado, false);
                return ViaDt.getModelosVia(conn, ModeloVia);
            }
        }
        
        #endregion
        
        #region VIADETECTOREJES: Tipos de detectores de ejes que puede tener una vía

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los tipos de detectores de ejes. 
        /// </summary>
        /// <returns>Lista de tipos de detectores de ejes</returns>
        /// ***********************************************************************************************
        public static ViaDetectorEjeL getTiposDetectoresEjes()
        {
            return ViaDt.getTiposDetectorEjes();
        }           

        #endregion
        
        #region VIARUEDASDUALES: Tipos de detectores de ruedas duales que puede tener una vía

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los tipos de detectores de ruedas duales. 
        /// </summary>
        /// <returns>Lista de tipos de detectores de ruedas duales</returns>
        /// ***********************************************************************************************
        public static ViaRuedasDualesL getTiposRuedasDuales()
        {
            return ViaDt.getTiposRuedasDuales();
        }

        #endregion
        
        #region VIASENTIDOCIRCULACION: Los posibles sentidos de circulacion que puede tener una vía
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los posibles sentidos de circulacion de una via recuperados de la
        /// base de datos y se le agrega un elemento mas que representa la bidireccionalidad
        /// </summary>
        /// <returns>Lista de sentidos de circulacion</returns>
        /// ***********************************************************************************************
        public static ViaSentidoCirculacionL getSentidosCirculacionCompleto()
        {
            var sentidos = getSentidosCirculacion();
            var sentidoAmbos = new ViaSentidoCirculacion
            { 
                Codigo = null, 
                Descripcion = Traduccion.Traducir("Todos")
            };
            sentidos.Insert(0, sentidoAmbos);
            return sentidos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los posibles sentidos de circulacion de una via recuperados de la
        /// base de datos.
        /// </summary>
        /// <returns>Lista de sentidos de circulacion</returns>
        /// ***********************************************************************************************
        public static ViaSentidoCirculacionL getSentidosCirculacion()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.getSentidosCirculacion(conn, null);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los sentidos de circulación posibles para las vía de una estación
        /// </summary>
        /// <param name="iEstacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ViaSentidoCirculacionL getSentidosCirculacionPosible()
        {
            return getSentidosCirculacionPosible(ConexionBs.getNumeroEstacion());
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los sentidos de circulación posibles para las vía de una estación
        /// </summary>
        /// <param name="iEstacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ViaSentidoCirculacionL getSentidosCirculacionPosible(int? iEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.getSentidoCirculacionPosible(conn, iEstacion);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los sentidos Ascendente y Descendente
        /// base de datos.
        /// </summary>
        /// <returns>Lista de sentidos de circulacion</returns>
        /// ***********************************************************************************************
        public static SentidoCirculacionL getSentidos()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.getSentidos(conn);
            }
        }

        #endregion
        
        #region VIAVIDEO: Tipos de aplicaciones que se les puede dar a las cámaras de video (Video, Foto o Ninguna)

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las aplicaciones que se le pueden dar a las camaras de video
        /// </summary>
        /// <returns>Lista de aplicaciones de las camaras de video</returns>
        /// ***********************************************************************************************
        public static ViaVideoL getTiposUsoCamaras()
        {
            return ViaDt.getTiposUsoCamaras();
        }           

        #endregion
        
        #region MODOSVIA

        /// <summary>
        /// Devuelve todos los modelos de vía
        /// </summary>
        /// <returns></returns>
        public static ViaModoL getViaModos()
        {
            using (Conexion conn = new Conexion())
            {                  
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.getViaModos(conn, null);
            }
        }

        #endregion

        #region ViaEst

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de todas las vías
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 11/08/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...:
        // ----------------------------------------------------------------------------------------------
        public static ViaL getViasEstacion()
        {
            return getViasEstacion(null);
        }
        
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de todas las vías
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 11/08/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - string? - Permite filrar por código de estación
        // ----------------------------------------------------------------------------------------------
        public static ViaL getViasEstacion(int? xiEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.getViasEstacion(conn, xiEstacion);
            }
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve un objeto Via que representa a Todas o Ninguna, segun la descripción
        /// </summary>
        /// <param name="sDescr"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        private static Via GetViaGenerica(string sDescr)
        {
            return new Via
            {
                NumeroVia = byte.MaxValue,
                NombreVia = Traduccion.Traducir(sDescr)
            };
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de todas las vías para los comandos
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 22/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - string? - Permite filrar por código de estación
        // ----------------------------------------------------------------------------------------------
        public static ViaComandoL getViasComandos(int? xiEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.getViasComandos(conn, xiEstacion);
            }
        }

        #endregion

        #region ERROR DE LA VIA: Metodos de la Clase Error de la Via.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los errores de vía
        /// </summary>
        /// <returns>Lista de Errores</returns>
        /// ***********************************************************************************************
        public static AlarmasL getErrores(string xsTipo, string xsStatus, string xsVias, DateTime dtFechaHoraDesde, DateTime dtFechaHoraHasta)
        {
            return getErrores( xsTipo,  xsStatus,  xsVias,  dtFechaHoraDesde,  dtFechaHoraHasta,ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Errores
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMensaje">Int - Permite filtrar por un mensaje determinada
        /// <param name="soloNoConfirmados">char - Permite filtrar entre alarmas no confirmadas
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static AlarmasL getErrores(string xsTipo, string xsStatus, string xsVias, DateTime dtFechaHoraDesde, DateTime dtFechaHoraHasta,bool bGST, int? codigoError)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                ViaL lListaVias = new ViaL();

                // PASAR DE LA LISTA DE STRING A LA LISTA DE OBJETOS CORRESPONDIENTES

                // VIAS
                if (xsVias != null)
                {

                    string[] auxVias = xsVias.Split(',');

                    if (auxVias[0] != "")
                    {
                        foreach (string word in auxVias)
                        {
                            lListaVias.Add(new Via(ConexionBs.getNumeroEstacion(), Convert.ToByte(word), ""));
                        }
                    }
                }
                else
                {
                    lListaVias = null;
                }
                return ViaDt.getErrores(xsTipo, xsStatus, lListaVias, dtFechaHoraDesde, dtFechaHoraHasta, conn, codigoError);
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad errores sin confirmar 
        /// </summary>
        /// <returns>Int</returns>
        /// ***********************************************************************************************
        public static int GetHayErrorSinConfirmar()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ViaDt.GetHayErrorSinConfirmar(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Otros

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Error Recibido
        /// </summary>
        /// <param name="oMensaje">Alarmas - Estructura de la Alarma a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delAlarmaRecibidaVia(Alarmas oAlarma, string Usuario)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(false, true);

                    //eliminamos la Auditoria
                    ViaDt.delAlarmaRecibidoVia(oAlarma,Usuario, conn);


                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Devuelve el directorio de fotos de una vía
        /// </summary>
        /// <param name="iCodEst"></param>
        /// <param name="iVia"></param>
        /// <returns></returns>
        public static string GetPhotoPath(int iCodEst, int iVia, bool Siguio,string Nomfo)
        {
            using (Conexion conn = new Conexion())
            {                  
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ViaDt.GetPhotoPath(conn, iCodEst, iVia, Siguio, Nomfo);
            }
        }

        /// <summary>
        /// Devuelve todos los modos posibles para una via
        /// </summary>
        /// <returns></returns>
        public static ModoDelParteL GetModosPosiblesVia()
        {
            ModoDelParteL modos = new ModoDelParteL();

            ModoDelParte modo1 = new ModoDelParte("T", Traduccion.Traducir("Todos"));
            ModoDelParte modo2 = new ModoDelParte("C", Traduccion.Traducir("Solo Modo Mantenimiento"));
            ModoDelParte modo3 = new ModoDelParte("S", Traduccion.Traducir("Sin Modo Mantenimiento"));

            modos.Add(modo1);
            modos.Add(modo2);
            modos.Add(modo3);

            return modos;
        }

        #endregion


        #region SUPERVICION REMOTA

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Via de Supervision
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Estructura de la Via a insertar
        /// ***********************************************************************************************
        public static void addViaSupervision(ViaInformacion oViaInfo)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Plaza, con transaccion
                    conn.ConectarPlaza(true, false);

                    //Agregamos la Via
                    ViaDt.addViaSupervision(oViaInfo, conn);

                    //Grabo OK, ya habiamos hecho COMMIT
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
        /// Devuelve la lista de todas las vias de una estacion y un punto de venta. 
        /// </summary>
        /// <param name="estacion">Int - Estacion</param>
        /// <param name="ptoVenta">Int - Punto de venta</param>
        /// <returns>Lista de Vias</returns>
        /// ***********************************************************************************************
        public static ViaInformacionL getViasSupervision(int? estacion, string id)
        {
            return getViasSupervision(ConexionBs.getGSToEstacion(), estacion, null, id);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Vias definidas
        /// </summary>
        /// <param name="bConsolidado">bool - Indica si se debe conectar con Consolidado o Estacion</param>
        /// <param name="numeroEstacion">Int - Permite filtrar por una estacion
        /// <param name="numeroVia">Int - Permite filtrar por una Via determinada
        /// <returns>Lista de Colores</returns>
        /// ***********************************************************************************************
        public static ViaInformacionL getViasSupervision(bool bConsolidado, int? numeroEstacion, int? numeroVia, string id)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false, false);

                    return ViaDt.getViasSupervision(conn, numeroEstacion, numeroVia, id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Via
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Estructura de la Via a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delViaSupervision(ViaInformacion oViaInfo)
        {
            delViaSupervision(oViaInfo, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Via
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Estructura de la Via a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delViaSupervision(ViaInformacion oViaInfo, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true,false);

                    //Eliminamos la Via
                    ViaDt.delViaSupervision(oViaInfo, conn);

                    conn.Finalizar(true);
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
    }
}
