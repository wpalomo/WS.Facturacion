using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    public class MonedaBs
    {
        #region MONEDA: Metodos de la Clase Moneda.

        /****************************************************************************
        * POR AHORA LO IMPLEMENTAMOS EN EL VIEWSTATE DE LA PAGINA
        * private const string LISTAMONEDAS = "ListaMonedas";
        * private const string MONEDAREFERENCIA = "MonedaReferencia";
        *****************************************************************************/
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los monedas definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas()
        {
            MonedaL oMonedas = null;
            oMonedas = getMonedas(ConexionBs.getGSToEstacion(), null, null);

            return oMonedas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los monedas definidos. 
        /// </summary>
        /// <param name="codigoMonedas">Int - Permite filtrar por una moneda determinado
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas(int? codigoMonedas)
        {
            return getMonedas(ConexionBs.getGSToEstacion(), codigoMonedas,null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de monedas definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMoneda">Int - Permite filtrar por una Moneda determinada
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas(bool bGST, int? codigoMonedas, bool? referencia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);
                    return MonedaDt.getMonedas(conn, codigoMonedas, referencia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la moneda de referencia
        /// </summary>
        /// <returns>objecto Moneda con los datos de la moneda de referencia</returns>
        /// ***********************************************************************************************
        public static Moneda getMonedaReferencia()
        {
            Moneda oMoneda = null;
            MonedaL oMonedas = getMonedas(ConexionBs.getGSToEstacion(), null, true);

            if (oMonedas.Count > 0)
            {
                oMoneda = oMonedas[0];
            }

            return oMoneda;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Moneda
        /// </summary>
        /// <param name="oMoneda">Monedas - Estructura de la Moneda a insertar
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static void addMonedas(Moneda oMoneda)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos Moneda
                    MonedaDt.addMonedas(oMoneda, conn);

                    if (oMoneda.esMonedaReferencia)
                    {
                        GestionConfiguracionBs.updConfigccoCon_monvi(oMoneda.Codigo, conn);
                    }


                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMoneda(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oMoneda),
                                                           getAuditoriaDescripcion(oMoneda)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);

                    LimpiarCacheMonedas();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Moneda
        /// </summary>
        /// <param name="oMoneda">Monedas - Estructura de la moneda a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updMoneda(Moneda oMoneda)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos Moneda
                    MonedaDt.updMonedas(oMoneda, conn);

                    if (oMoneda.esMonedaReferencia)
                    {
                        GestionConfiguracionBs.updConfigccoCon_monvi(oMoneda.Codigo, conn);
                    }

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMoneda(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oMoneda),
                                                           getAuditoriaDescripcion(oMoneda)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                    LimpiarCacheMonedas();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Moneda
        /// </summary>
        /// <param name="oMoneda">Moneda - Estructura de la Moneda a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delMoneda(Moneda oMoneda, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "MONEDA", 
                                                       new string[] { oMoneda.Codigo.ToString() },
                                                       new string[] { },
                                                       nocheck);
                    
                    //eliminamos la Moneda
                    MonedaDt.delMoneda(oMoneda, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMoneda(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oMoneda),
                                       getAuditoriaDescripcion(oMoneda)),
                                       conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                    LimpiarCacheMonedas();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Limpia de la cache la lista de monedas
        /// </summary>
        /// ***********************************************************************************************
        protected static void LimpiarCacheMonedas()
        {
            //Limpiamos el cache de lista de monedas
            //HttpContext.Current.Cache[LISTAMONEDAS] = null;
            //HttpContext.Current.Cache[MONEDAREFERENCIA] = null;
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaMoneda()
        {
            return "MON";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Moneda oMoneda)
        {
            return oMoneda.Codigo.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Moneda oMoneda)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Moneda", oMoneda.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Simbolo", oMoneda.Simbolo);
            AuditoriaBs.AppendCampo(sb, "Desripción", oMoneda.Desc_Moneda);
            if (oMoneda.esMonedaReferencia)
            {
                AuditoriaBs.AppendCampo(sb, "Es Moneda de la Vía", Utilitarios.Traduccion.getSi());
            }

            return sb.ToString();
        }

        #endregion
        
        #endregion
        
        #region DENOMINACIONES: Metodos de la Clase Denominacion
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Denominaciones definidas, sin filtro
        /// </summary>
        /// <returns>Lista de Denominaciones</returns>
        /// ***********************************************************************************************
        public static DenominacionL getDenominaciones()
        {
            return getDenominaciones(ConexionBs.getGSToEstacion(),null,null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Denominaciones definidos. 
        /// </summary>
        /// <param name="CodMoneda">Codigo de Moneda 
        /// <returns>Lista de DenominacionES</returns>
        /// ***********************************************************************************************
        public static DenominacionL getDenominaciones(int? CodMoneda, int? CodDenominacion)
        {
            return getDenominaciones(ConexionBs.getGSToEstacion(),CodMoneda, CodDenominacion);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Denominaciones definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="oDenominacion">Denominaciones - Estructura de la Denominacion a insertar
        /// <returns>Lista de Denominaciones</returns>
        /// ***********************************************************************************************
        public static DenominacionL getDenominaciones(bool bGST, int? CodMoneda, int? CodDenominacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);
                    return MonedaDt.getDenominacion(conn, CodMoneda, CodDenominacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Denominaciones definidos. 
        /// </summary>
        /// <param name="CodMoneda">Codigo de Moneda 
        /// <returns>Lista de DenominacionES</returns>
        /// ***********************************************************************************************
        public static DenominacionL getDenominaciones(int? CodMoneda)
        {
            return getDenominaciones(ConexionBs.getGSToEstacion(), CodMoneda,null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Denominaciones definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="oDenominacion">Denominaciones - Estructura de la Denominacion a insertar
        /// <returns>Lista de Denominaciones</returns>
        /// ***********************************************************************************************
        public static DenominacionL getDenominaciones(bool bGST, int? CodMoneda)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);
                    return MonedaDt.getDenominacion(conn, CodMoneda, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Denominacion
        /// </summary>
        /// <param name="oDenominacion">Denominaciones - Estructura de la Denominacion a insertar
        /// <returns>Lista de Denominaciones</returns>
        /// ***********************************************************************************************
        public static void addDenominacion(Denominacion oDenominacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos Denominacion
                    MonedaDt.addDenominacion(oDenominacion, conn);
                    
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaoDenominacion(),
                                                           "A",
                                                           getAuditoriaCodigoRegistroDenominacion(oDenominacion),
                                                           getAuditoriaDescripcion(oDenominacion)),
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
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Denominacion
        /// </summary>
        /// <param name="oDenominacion">Denominacion - Estructura de la Denominacion a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updDenominacion(Denominacion oDenominacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos la Denominacion
                    MonedaDt.updDenominacion(oDenominacion, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaoDenominacion(),
                                                           "M",
                                                           getAuditoriaCodigoRegistroDenominacion(oDenominacion),
                                                           getAuditoriaDescripcion(oDenominacion)),
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
        
        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Denominación
        /// </summary>
        /// <param name="oDenominacion">Denominacion - Estructura de la Denominación a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delDenominacion(Denominacion oDenominacion, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "DENOMI", 
                                                       new string[] { oDenominacion.Moneda.Codigo.ToString(), oDenominacion.CodDenominacion.ToString() },
                                                       new string[] { },
                                                       nocheck);

                    //eliminamos la Denominacion
                    MonedaDt.delDenominacion(oDenominacion, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaoDenominacion(),
                                       "B",
                                       getAuditoriaCodigoRegistroDenominacion(oDenominacion),
                                       getAuditoriaDescripcion(oDenominacion)),
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

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaoDenominacion()
        {
            return "DEN";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroDenominacion(Denominacion oDenominacion)
        {
            return oDenominacion.CodDenominacion.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Denominacion oDenominacion)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Moneda", oDenominacion.Moneda.Desc_Moneda);
            AuditoriaBs.AppendCampo(sb, "Descripción", oDenominacion.DescDenominacion);

            return sb.ToString();
        }

        #endregion
        
        #endregion
        
        #region COTIZACIONES: Metodos de negocios de la Clase de COTIZACIONES

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Definicion de cotizaciones realizados para mostrar en la grilla, con los filtros ingresados
        /// </summary>
        /// <param name="codigoEstacion">byte - Codigo de estacion en los que se genero la cotizacion</param>
        /// <param name="fechaInicial">datetime - Fecha inicial de vigencia de la cotizacion</param>
        /// <returns>Lista de Cotizaciones</returns>
        /// ***********************************************************************************************
        public static CotizacionL getCotizacionesCabecera(int? codigoEstacion, DateTime fechaInicial, int? identity)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return MonedaDt.getCotizacionesCabecera(conn, codigoEstacion, fechaInicial, identity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de cotizaciones de una definicion de cotizacion especifica
        /// </summary>
        /// <returns>Lista de cotizaciones de una determinada fecha y estacion</returns>
        /// ***********************************************************************************************
        public static Cotizacion getCotizacionesDetalle(int estacion, int identity)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    // Objeto cabecera
                    Cotizacion oCotizacion = MonedaDt.getCotizacionesCabecera(conn, estacion, null, identity)[0];

                    // Le enlazamos las cotizaciones detalladas
                    oCotizacion.Cotizaciones = MonedaDt.getCotizacionesDetalle(conn, estacion, identity);                    
                    return oCotizacion;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una definicion de cotizacion
        /// </summary>
        /// <param name="oCotizacionL">CotizacionL - Lista que contiene los valores de las cotizaciones a grabar</param>
        /// ***********************************************************************************************
        public static void addCotizacion(Cotizacion oCotizacion)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //En Gestion ó la Estacion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    // Al objeto de cotizacion la estacion en la que estamos insertando lo determina la Bussiness
                    oCotizacion.EstacionOrigen = new Estacion(ConexionBs.getNumeroEstacion(), "");

                    // Al objeto de moneda de referencia lo determina la Bussiness
                    oCotizacion.MonedaReferencia = MonedaBs.getMonedaReferencia();

                    // Insertamos la cabecera
                    MonedaDt.addCotizacionCabecera(oCotizacion, conn);

                    //Grabamos auditoria de la cabecera (para que no confundan tantos datos repetidos)
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCotizacion(),
                                                           "A",
                                                           getAuditoriaCodigoRegistroCAB(oCotizacion),
                                                           getAuditoriaDescripcionCAB(oCotizacion)),
                                                           conn);


                    //Agregamos el detalle de las cotizaciones
                    foreach (CotizacionDetalle oCotizacionDetalle in oCotizacion.Cotizaciones)
                    {
                        MonedaDt.addCotizacionDetalle(oCotizacion.EstacionOrigen.Numero, oCotizacion.Identity, oCotizacionDetalle, conn);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCotizacion(),
                                                               "A",
                                                               getAuditoriaCodigoRegistroCAB(oCotizacion),
                                                               getAuditoriaDescripcionDET(oCotizacionDetalle)),
                                                               conn);
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

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de las cotizaciones a futuro (todavia no estan vigentes). Las borramos y volvemos a insertar
        /// </summary>
        /// <param name="oCotizacion">Cotizacion - Estructura con la cotizacion a insertar, con el detalle
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCotizacion(Cotizacion oCotizacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //En Gestion ó la Estacion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    // Al objeto de cotizacion la estacion en la que estamos insertando lo determina la Bussiness
                    oCotizacion.EstacionOrigen = new Estacion(ConexionBs.getNumeroEstacion(), "");

                    // Eliminamos el detalle para volver a insertarlo. 
                    // No tocamos la cabecera de la cotizacion 
                    MonedaDt.delCotizacionDetalle(oCotizacion, conn);


                    //Grabamos auditoria de la cabecera (para que no confundan tantos datos repetidos)
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCotizacion(),
                                                           "M",
                                                           getAuditoriaCodigoRegistroCAB(oCotizacion),
                                                           getAuditoriaDescripcionCAB(oCotizacion)),
                                                           conn);

                    //Agregamos el detalle de las Cotizaciones
                    foreach (CotizacionDetalle oCotizacionDetalle in oCotizacion.Cotizaciones)
                    {
                        MonedaDt.addCotizacionDetalle(oCotizacion.EstacionOrigen.Numero, oCotizacion.Identity, oCotizacionDetalle, conn);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCotizacion(),
                                                               "M",
                                                               getAuditoriaCodigoRegistroCAB(oCotizacion),
                                                               getAuditoriaDescripcionDET(oCotizacionDetalle)),
                                                               conn);

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

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una definicion de cotizacion completa
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delCotizacion(Cotizacion oCotizacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //En Gestion ó la Estacion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    // Al objeto de cotizacion la estacion en la que estamos insertando lo determina la Bussiness
                    oCotizacion.EstacionOrigen = new Estacion(ConexionBs.getNumeroEstacion(), "");

                    //eliminamos el detalle de las Cotizaciones
                    MonedaDt.delCotizacionDetalle(oCotizacion, conn);

                    //Eliminamos la cabecera de la cotizacion
                    MonedaDt.delCotizacionCabecera(oCotizacion, conn);


                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCotizacion(),
                                                           "B",
                                                           getAuditoriaCodigoRegistroCAB(oCotizacion),
                                                           getAuditoriaDescripcionCAB(oCotizacion)),
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

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaCotizacion()
        {
            return "COT";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado (con la cabecera es suficiente)
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroCAB(Cotizacion oCotizacion)
        {
            return oCotizacion.Identity.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion de la cabecera del registro afectado 
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionCAB(Cotizacion oCotizacion)
        {
            StringBuilder sb = new StringBuilder();

            // Cabecera del registro de auditoria
            AuditoriaBs.AppendCampo(sb, "Número", oCotizacion.Identity.ToString());
            AuditoriaBs.AppendCampo(sb, "Origen", oCotizacion.EstacionOrigen.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Válido Desde", oCotizacion.FechaInicialVigencia.ToString());
            if (oCotizacion.FechaFinalVigencia != null)
            {
                AuditoriaBs.AppendCampo(sb, "Válido Hasta", oCotizacion.FechaFinalVigencia.ToString());
            }

            return sb.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del detalle del registro afectado 
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionDET(CotizacionDetalle oCotizacionDetalle)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Moneda", oCotizacionDetalle.Moneda.Desc_Moneda);
            AuditoriaBs.AppendCampo(sb, "Valor", oCotizacionDetalle.sValorCotizacion);

            return sb.ToString();
        }

        #endregion
        
        #endregion
    }
}
