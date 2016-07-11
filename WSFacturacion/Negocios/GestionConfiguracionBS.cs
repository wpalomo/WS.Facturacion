using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class GestionConfiguracionBs
    {
        #region OBSERVACION: Metodos de la Clase de Negocios de la entidad observacion.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Observaciones definidas, sin parametros. 
        /// </summary>
        /// <returns>Lista de Observaciones</returns>
        /// ***********************************************************************************************
        public static ObservacionL getObservaciones()
        {
            return getObservaciones(ConexionBs.getGSToEstacion(), null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Observaciones definidas. 
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Observacion determinada
        /// <returns>Lista de Observaciones</returns>
        /// ***********************************************************************************************
        public static ObservacionL getObservaciones(int? codigo)
        {
            return getObservaciones(ConexionBs.getGSToEstacion(), codigo);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Observaciones definidas
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Obsdervacion determinada
        /// <returns>Lista de Observaciones</returns>
        /// ***********************************************************************************************
        public static ObservacionL getObservaciones(bool bGST, int? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);

                return GestionConfiguracionDt.getObservaciones(conn, codigo);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Observacion
        /// </summary>
        /// <param name="oObservacion">Observacion - Estructura de la Observacion para dar de alta
                /// <returns>Lista de Observaciones</returns>
        /// ***********************************************************************************************
        public static void addObservacion(Observacion oObservacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que no exista la Observacion

                //Agregamos Observacion
                GestionConfiguracionDt.addObservacion(oObservacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oObservacion),
                                                        getAuditoriaDescripcion(oObservacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Observacion
        /// </summary>
        /// <param name="oObservacion">Observacion - Estructura de la Observacion para modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updObservacion(Observacion oObservacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Observacion

                //Modificamos Observacion
                GestionConfiguracionDt.updObservacion(oObservacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oObservacion),
                                                        getAuditoriaDescripcion(oObservacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Observacion
        /// </summary>
        /// <param name="bGST">Int - Observacion a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delObservacion(Observacion oObservacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verficamos que haya registros para la Observacion

                //eliminamos la estacion
                GestionConfiguracionDt.delObservacion(oObservacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oObservacion),
                                                        getAuditoriaDescripcion(oObservacion)),
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
            return "OBS";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Observacion oObservacion)
        {
            return oObservacion.Codigo.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Observacion oObservacion)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Descripción",oObservacion.Descripcion);
            return sb.ToString();
        }

        #endregion
        
        #endregion

        #region CONFIGCCO: Metodos de la Clase de Negocios de la entidad Configcco.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Configcco definidas. 
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Configcco determinada
        /// <returns>Lista de Configcco</returns>
        /// ***********************************************************************************************
        public static GestionConfiguracion getConfigcco()
        {
            return getConfigcco(ConexionBs.getGSToEstacion());
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configcco definidas
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Configcco determinada
        /// <returns>Lista de Configcco</returns>
        /// ***********************************************************************************************
        public static GestionConfiguracion getConfigcco(bool bGST)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return GestionConfiguracionDt.getConfigcco(conn);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Configcco
        /// </summary>
        /// <param name="bGST">Int - Configcco a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfigcco(GestionConfiguracion oConfigcco)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Configcco

                //Modificamos Configcco
                GestionConfiguracionDt.updConfigcco(oConfigcco, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfigCCO(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oConfigcco),
                                                        getAuditoriaDescripcion(oConfigcco)),
                                                        conn);
                
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
       }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de la moneda de referencia que utiliza la via.
        /// </summary>
        /// <param name="IntMoneda">Int16 - Codigo de moneda
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfigccoCon_monvi(Int16 IntMoneda, Conexion oConn)
        {
            //Modificamos Configcco
            GestionConfiguracionDt.updConfigccoCon_monvi(IntMoneda, oConn);
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaConfigCCO()
        {
            return "CGC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(GestionConfiguracion oConfigcco)
        {
            return oConfigcco.IdReplicacion.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(GestionConfiguracion oConfigcco)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Dirección URL", oConfigcco.DireccionURL.ToString());
            AuditoriaBs.AppendCampo(sb, "Cobros de Ejes Levantados", Traduccion.getSiNo(oConfigcco.CobraEixos));
            AuditoriaBs.AppendCampo(sb, "Cantidad de Ejes para Foto Lateral", oConfigcco.EjesParaFotoAVI.ToString());
            AuditoriaBs.AppendCampo(sb, "Código de País en la Antena", oConfigcco.PaisAntena);
            AuditoriaBs.AppendCampo(sb, "Código de Emisor de la Antena", oConfigcco.ConcesionarioAntena);
            AuditoriaBs.AppendCampo(sb, "Tiempo de Espera de las Alarmas del Cliente Gráfico (sec)", oConfigcco.TiempoAlarmaClienteGrafico.ToString());

            return sb.ToString();
        }

        #endregion
        
        #endregion
        
        #region CFGTRIB: Metodos de la Clase de Negocios de la entidad CFGTRIB.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las CFGTRIB definidas. 
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Configcco determinada
        /// <returns>Lista de CFGTRIB</returns>
        /// ***********************************************************************************************
        public static ConfiguracionTributaria getConfigtrb()
        {
            return getConfigtrb(ConexionBs.getGSToEstacion());
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de CFGTRIB definidas
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una CFGTRIB determinada
        /// <returns>Lista de CFGTRIB</returns>
        /// ***********************************************************************************************
        public static ConfiguracionTributaria getConfigtrb(bool bGST)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return GestionConfiguracionDt.getConfigtrb(conn);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una CFGTRIB
        /// </summary>
        /// <param name="bGST">Int - CFGTRIB a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfigtrb(ConfiguracionTributaria oConfigtrb)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Configtrb

                //Modificamos Configtrb
                GestionConfiguracionDt.updConfigtrb(oConfigtrb, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfigTrb(),
                                                        "M",
                                                        getAuditoriaCodigoRegistroTrb(oConfigtrb),
                                                        getAuditoriaDescripcionTrb(oConfigtrb)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaConfigTrb()
        {
            return "TRB";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroTrb(ConfiguracionTributaria oConfigtrb)
        {
            return oConfigtrb.IdReplicacion.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionTrb(ConfiguracionTributaria oConfigtrb)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Razón Social", oConfigtrb.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "RUC", oConfigtrb.RUC);
            AuditoriaBs.AppendCampo(sb, "Dirección Matriz", oConfigtrb.Direccion);
            AuditoriaBs.AppendCampo(sb, "Contribuyente Especial", oConfigtrb.ContribuyenteEspecial);
            if (oConfigtrb.FechaContribuyenteEspecial != null)
            {
                AuditoriaBs.AppendCampo(sb, "Fecha de Resolución", ((DateTime)oConfigtrb.FechaContribuyenteEspecial).ToShortDateString());
            }

            return sb.ToString();
        }

        #endregion
        
        #endregion

        #region CAUSAS de CIERRE: Metodos de la clase Causas de cierre de Vía

        /// <summary>
        /// Mostrar listado de Causas de cierre
        /// </summary>
        /// <returns></returns>
        public static causaCierreL getCausasCierre()
        {
            return getCausasCierre(null);
        }

        public static causaCierreL getCausasCierre(int? codci)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return GestionConfiguracionDt.getCausasCierre(conn, codci);
            }
        }
        
        /// <summary>
        /// ALTA de Causa de cierre
        /// </summary>
        /// <param name="oCausaCierre"></param>
        public static void addCausaCierre(CausaCierre oCausaCierre)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que no exista la Zona

                //Agregamos Causa de Cierre
                GestionConfiguracionDt.addCausaCierre(oCausaCierre, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCierre(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oCausaCierre),
                                                        getAuditoriaDescripcion(oCausaCierre)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// <summary>
        /// MODIFICACION de Causa de cierre
        /// </summary>
        /// <param name="oCausaCierre"></param>
        public static void updCausaCierre(CausaCierre oCausaCierre)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Causa

                //Modificamos Causa
                GestionConfiguracionDt.updCausaCierre(oCausaCierre, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCierre(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oCausaCierre),
                                                        getAuditoriaDescripcion(oCausaCierre)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// <summary>
        /// BAJA de Causa de Cierre
        /// </summary>
        /// <param name="oCausaCierre"></param>
        /// <param name="nocheck"></param>
        public static void delCausaCierre(CausaCierre oCausaCierre, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "Causas Cierre",
                                                    new string[] { oCausaCierre.Codigo.ToString() },
                                                    new string[] { },
                                                    nocheck);

                //eliminamos la Causa
                GestionConfiguracionDt.delCausaCierre(oCausaCierre, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCierre(),
                                    "B",
                                    getAuditoriaCodigoRegistro(oCausaCierre),
                                    getAuditoriaDescripcion(oCausaCierre)),
                                    conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaCierre()
        {
            return "CAU";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(CausaCierre oCausaCierre)
        {
            return oCausaCierre.Codigo.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(CausaCierre oCausaCierre)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Descripción", oCausaCierre.Descripcion);
            return sb.ToString();
        }

        #endregion

        #region SIP: Metodos de los Códigos de Simulación de Paso

        /*
        // Mostrar listado de los códigos de Simulación de Paso
        public static SimulacionDePasoL getSimulacionDePaso()
        {
            return getSimulacionDePaso(null);
        }
        
        public static SimulacionDePasoL getSimulacionDePaso(int? codci)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return GestionConfiguracionDt.getSimulacionDePaso(conn, codci);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //ALTA de Simulacion de Paso
        public static void addSimulacionDePaso(SimulacionDePaso oSimulacionDePaso)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que no exista la Zona

                    //Agregamos Código de SIP
                    GestionConfiguracionDt.addSimulacionDePaso(oSimulacionDePaso, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaSip(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oSimulacionDePaso),
                                                           getAuditoriaDescripcion(oSimulacionDePaso)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //MODIFICACION de Simulación de Paso

        public static void updSimulacionDePaso(SimulacionDePaso oSimulacionDePaso)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que ya exista la Simulacion de paso

                    //Modificamos Simulacion de Paso
                    GestionConfiguracionDt.updSimulacionDePaso(oSimulacionDePaso, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaSip(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oSimulacionDePaso),
                                                           getAuditoriaDescripcion(oSimulacionDePaso)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //BAJA de SIP
        public static void delSimulacionDePaso(SimulacionDePaso oSimulacionDePaso, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "Simulacion de Paso",
                                                       new string[] { oSimulacionDePaso.Codigo.ToString() },
                                                       new string[] { },
                                                       nocheck);

                    //eliminamos el codigo de SIP
                    GestionConfiguracionDt.delSimulacionDePaso(oSimulacionDePaso, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaSip(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oSimulacionDePaso),
                                       getAuditoriaDescripcion(oSimulacionDePaso)),
                                       conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaSip()
        {
            return "SIP";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(SimulacionDePaso oSimulacionDePaso)
        {
            return oSimulacionDePaso.Codigo.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(SimulacionDePaso oSimulacionDePaso)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Descripción", oSimulacionDePaso.Descripcion);

            return sb.ToString();
        }
        */

        #endregion

    }
}
