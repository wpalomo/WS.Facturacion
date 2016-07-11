using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de Estacion de Peaje
    /// </summary>
    ///****************************************************************************************************
    public static class EstacionBs
    {
        #region ESTACION: Metodos de la Clase de Negocios de la entidad Estaciones de Peaje.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Estaciones definidas. 
        /// </summary>
        /// <returns>Lista de Estaciones</returns>
        /// ***********************************************************************************************
        public static EstacionL getEstaciones()
        {
            return getEstaciones(ConexionBs.getGSToEstacion(), null, null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Estaciones definidas mas el item de "Gestion". 
        /// </summary>
        /// <returns>Lista de Estaciones incluyendo gestion</returns>
        /// ***********************************************************************************************
        public static EstacionL getEstacionesMasGestion()
        {
            EstacionL oEstRetL = new EstacionL();
            EstacionL oEstacionL = new EstacionL();

            // Primer elemento es gestion    
            oEstRetL.Add(new Estacion(0, Traduccion.getTextoGestion()));

            // Levanto la lista de estaciones y la paso a la lista que voy a retornar            
            oEstacionL = getEstaciones();
            oEstRetL.AddRange(oEstacionL);
            return oEstRetL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Estaciones definidas. 
        /// </summary>
        /// <param name="numeroEstacion">Int - Permite filtrar por una estacion determinada
        /// <returns>Lista de Estaciones</returns>
        /// ***********************************************************************************************
        public static EstacionL getEstaciones(int? numeroEstacion)
        {
            return getEstaciones(ConexionBs.getGSToEstacion(), numeroEstacion, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la estacion actual
        /// </summary>
        /// <returns>Estacion Actual</returns>
        /// ***********************************************************************************************
        public static Estacion getEstacionActual()
        {
            if (ConexionBs.getNumeroEstacion() != 0)
                return getEstaciones(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), null)[0];
            else
                return new Estacion(0, Traduccion.getTextoGestion());
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Estaciones de una zona. 
        /// </summary>
        /// <param name="zona">Int - filtra por una zona
        /// <returns>Lista de Estaciones</returns>
        /// ***********************************************************************************************
        public static EstacionL getEstacionesZona(int? zona)
        {
            return getEstaciones(ConexionBs.getGSToEstacion(), null, zona);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Estaciones definidas. Filtra por estacion.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="numeroEstacion">Int - Permite filtrar por una estacion determinada
        /// <returns>Lista de Estaciones</returns>
        /// ***********************************************************************************************
        public static EstacionL getEstaciones(bool bGST, int? numeroEstacion, int? zona)
        {
            bool PudoGST;
            return getEstaciones(bGST, numeroEstacion, zona, out PudoGST);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Estaciones definidas. Filtra por estacion.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="numeroEstacion">Int - Permite filtrar por una estacion determinada
        /// <returns>Lista de Estaciones</returns>
        /// ***********************************************************************************************
        public static EstacionL getEstaciones(bool bGST, int? numeroEstacion, int? zona, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }

                return EstacionDt.getEstaciones(conn, numeroEstacion, zona);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una estacion de peaje.
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto Estacion de Peaje
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addEstacion(Estacion oEstacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que no exista la estacion

                //Agregamos estacion
                EstacionDt.addEstacion(oEstacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEstacion(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oEstacion),
                                                        getAuditoriaDescripcion(oEstacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de una estacion de peaje.
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto Estacion de Peaje
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updEstacion(Estacion oEstacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la estacion

                //Modificamos estacion
                EstacionDt.updEstacion(oEstacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEstacion(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oEstacion),
                                                        getAuditoriaDescripcion(oEstacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Baja de una estacion de peaje.
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto Estacion de Peaje
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delEstacion(Estacion oEstacion, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "ESTACI",
                                                    new string[] { oEstacion.Numero.ToString() },
                                                    new string[] { },
                                                    nocheck);

                //eliminamos la estacion
                EstacionDt.delEstacion(oEstacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEstacion(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oEstacion),
                                                        getAuditoriaDescripcion(oEstacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaEstacion()
        {
            return "EST";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Estacion oEstacion)
        {
            return oEstacion.Numero.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Estacion oEstacion)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre", oEstacion.Nombre);
            AuditoriaBs.AppendCampo(sb, "Sentido", oEstacion.Sentido.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Número de Site", oEstacion.Site.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Permite Retiros Anticipados", Traduccion.getSiNo(oEstacion.esPermiteRetirosAnticipados));
            AuditoriaBs.AppendCampo(sb, "Dirección", oEstacion.Direccion);
            AuditoriaBs.AppendCampo(sb, "Base de Datos", oEstacion.BaseDatos);
            AuditoriaBs.AppendCampo(sb, "Servidor de Datos", oEstacion.ServidorDatos);
            AuditoriaBs.AppendCampo(sb, "Zona", oEstacion.Zona.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Dirección URL", oEstacion.URL);

  
            return sb.ToString();
        }

        #endregion

        #endregion

        #region ZONA: Metodos de la Clase de Negocios de la entidad Zona.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Zonas definidas, sin filtro
        /// </summary>
        /// <returns>Lista de Zonas</returns>
        /// ***********************************************************************************************
        public static ZonaL getZonas()
        {
            return getZonas(ConexionBs.getGSToEstacion(), null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Zonas definidas. 
        /// </summary>
        /// <param name="codigoZona">Int - Permite filtrar por una zona determinada
        /// <returns>Lista de Zonas</returns>
        /// ***********************************************************************************************
        public static ZonaL getZonas(int? codigoZona)
        {
            return getZonas(ConexionBs.getGSToEstacion(), codigoZona);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Zonas definidas
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoZona">Int - Permite filtrar por una zona determinada
        /// <returns>Lista de Zonas</returns>
        /// ***********************************************************************************************
        public static ZonaL getZonas(bool bGST, int? codigoZona)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return EstacionDt.getZonas(conn, codigoZona);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una zona
        /// </summary>
        /// <param name="oZona">Zona - Estructura de la zona a insertar
        /// <returns>Lista de Zonas</returns>
        /// ***********************************************************************************************
        public static void addZona(Zona oZona)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que no exista la Zona

                //Agregamos Zona
                EstacionDt.addZona(oZona, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaZona(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oZona),
                                                        getAuditoriaDescripcion(oZona)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una zona
        /// </summary>
        /// <param name="oZona">Zona - Estructura de la zona a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updZona(Zona oZona)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Zona

                //Modificamos Zona
                EstacionDt.updZona(oZona, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaZona(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oZona),
                                                        getAuditoriaDescripcion(oZona)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una zona
        /// </summary>
        /// <param name="oZona">Zona - Estructura de la zona a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delZona(Zona oZona, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "ZONAS",
                                                    new string[] { oZona.Codigo.ToString() },
                                                    new string[] { },
                                                    nocheck);

                //eliminamos la Zona
                EstacionDt.delZona(oZona, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaZona(),
                                    "B",
                                    getAuditoriaCodigoRegistro(oZona),
                                    getAuditoriaDescripcion(oZona)),
                                    conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaZona()
        {
            return "ZON";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Zona oZona)
        {
            return oZona.Codigo.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Zona oZona)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Descripción", oZona.Descripcion);
            return sb.ToString();
        }

        #endregion

        #endregion

        #region SUBESTACIONES: Metodos de la Clase de Negocios de la entidad Subestaciones.
        
        ///****************************************************************************************************
        /// <summary>
        /// Método encargado de devolver todas las subestaciones
        /// </summary>
        /// <returns></returns>
        ///****************************************************************************************************
        public static SubestacionL GetSubestaciones()
        {
            return GetSubestaciones(null, null);
        }

        ///****************************************************************************************************
        /// <summary>
        /// Método encargado de devolver una subestación dado un código de estación y un sentido
        /// </summary>
        /// <param name="iCodigoEstacion"></param>
        /// <param name="sSentido"></param>
        /// <returns></returns>
        ///****************************************************************************************************
        public static SubestacionL GetSubestaciones(int? iCodigoEstacion, string sSentido)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return (EstacionDt.GetSubestaciones(conn, iCodigoEstacion, sSentido));
            }
        }

        ///****************************************************************************************************
        /// <summary>
        /// Encargado de guardar una subestación en la base de dato
        /// </summary>
        /// <param name="subestacion"></param>
        ///****************************************************************************************************
        public static void UpdSubestacion(Subestacion subestacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                EstacionDt.UpdSubestacion(conn, subestacion);
                
                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaSubestacion(),
                                                        "M",
                                                        getAuditoriaCodigoRegistroSubestacion(subestacion),
                                                        getAuditoriaDescripcionSubestacion(subestacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        ///****************************************************************************************************
        /// <summary>
        /// Método encargado de devolver todas las subestaciones
        /// </summary>
        /// <returns></returns>
        ///****************************************************************************************************
        public static CabeceraTicketL GetCabeceraTicket()
        {
            return GetCabeceraTicket(null, null);
        }

        ///****************************************************************************************************
        /// <summary>
        /// Método encargado de devolver una subestación dado un código de estación y un sentido
        /// </summary>
        /// <param name="iCodigoEstacion"></param>
        /// <param name="sSentido"></param>
        /// <returns></returns>
        ///****************************************************************************************************
        public static CabeceraTicketL GetCabeceraTicket(int? iCodigoEstacion, string sSentido)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return (EstacionDt.GetCabeceraTicket(conn, iCodigoEstacion, sSentido));
            }
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaSubestacion()
        {
            return "SES";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroSubestacion(Subestacion subestacion)
        {
            return subestacion.EstacionNumero + " " + subestacion.SentidoCodigo;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionSubestacion(Subestacion subestacion)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Sentido Cardinal", Convert.ToString(subestacion.CodigoSubEstacion));
            AuditoriaBs.AppendCampo(sb, "Código Subestación", Convert.ToString(subestacion.CodigoSubEstacion));
            AuditoriaBs.AppendCampo(sb, "Código de Sia", Convert.ToString(subestacion.NumeroSia));
            AuditoriaBs.AppendCampo(sb, "Descripción", subestacion.Descripcion);

            return sb.ToString();
        }

        #endregion

        #region CabeceraTicket: Metodos de la Clase de Negocios de la entidad CabeceraTicket.

        ///****************************************************************************************************
        /// <summary>
        /// Encargado de guardar una subestación en la base de dato
        /// </summary>
        /// <param name="subestacion"></param>
        ///****************************************************************************************************
        public static void UpdCabeceraTicket(CabeceraTicket cabeceraTicket)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                EstacionDt.UpdCabeceraTicket(conn, cabeceraTicket);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCabeceraTicket(),
                                                        "M",
                                                        getAuditoriaCodigoRegistroCabeceraTicket(cabeceraTicket),
                                                        getAuditoriaDescripcionCabeceraTicket(cabeceraTicket)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaCabeceraTicket()
        {
            return "SES";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroCabeceraTicket(CabeceraTicket cabeceraTicket)
        {
            return cabeceraTicket.EstacionNumero + " " + cabeceraTicket.SentidoCodigo;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionCabeceraTicket(CabeceraTicket cabeceraTicket)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Código estación", Convert.ToString(cabeceraTicket.EstacionNumero));
            //AuditoriaBs.AppendCampo(sb, "Código de Sia", Convert.ToString(subestacion.NumeroSia));
            AuditoriaBs.AppendCampo(sb, "Descripción", cabeceraTicket.Descripcion);

            return sb.ToString();
        }
        
        #endregion

        #region Sites

        ///****************************************************************************************************
        /// <summary>
        /// Obtiene una lista de objetos Site
        /// </summary>
        /// <returns></returns>
        /// ****************************************************************************************************
        public static SiteL getSites()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return (EstacionDt.getSites(conn, null));
            }
        }

        #endregion

        #region ESTADO DE LA LISTA NEGRA DE LAS VIAS

        /// <summary>
        /// Devuelve una lista con los estados de las listas negras de cada via
        /// </summary>
        /// <param name="iCoest"></param>
        /// <param name="iVia"></param>
        /// <returns></returns>
        public static DataSet GetEstacionEstadosListaNegra()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false, false);
                return EstacionDt.GetEstacionEstadosListaNegra(conn);
            }
        }

        #endregion
    }
}