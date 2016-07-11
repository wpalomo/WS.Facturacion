using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Telectronica.Peaje
{

    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de Tarifas
    /// </summary>
    ///****************************************************************************************************

    public static class TarifaBs
    {

        #region TARIFA_DIFERENCIADA: Clase de Negocios de Tarifas Diferenciadas definidas


            ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de Tarifas
    /// </summary>
    ///****************************************************************************************************

        public static decimal getTarifa(int estacion, DateTime fecha, int categoria, int titari)
        {
             try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return TarifaDt.getTarifa(conn, estacion, fecha, categoria, titari, null);
                }
            }
             catch (Exception ex)
             {
                 throw ex;
             }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tarifas diferenciadas definidas. No recibe parametros, invoca al mismo 
        /// metodo pero con valor NULL para que retorne todos los registros
        /// </summary>
        /// <returns>Lista de Tarifas Diferenciadas</returns>
        /// ***********************************************************************************************
        public static TarifaDiferenciadaL getTarifasDiferenciadas()
        {
            return getTarifasDiferenciadas(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tarifas diferenciadas definidas. 
        /// </summary>
        /// <param name="codigoTarifa">int - Codigo de tarifa diferenciada por la cual filtrar la busqueda</param>
        /// <returns>Lista de Tarifas Diferenciadas</returns>
        /// ***********************************************************************************************
        public static TarifaDiferenciadaL getTarifasDiferenciadas(int? codigoTarifa)
        {
            return getTarifasDiferenciadas(ConexionBs.getGSToEstacion(), codigoTarifa);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tarifas diferenciadas definidas.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoTarifa">int - Codigo de tarifa diferenciada por la cual filtrar la busqueda</param>
        /// <returns>Lista de Tarifas Diferenciadas</returns>
        /// ***********************************************************************************************
        public static TarifaDiferenciadaL getTarifasDiferenciadas(bool bGST, int? codigoTarifa)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return TarifaDt.getTarifasDiferenciadas(conn, codigoTarifa);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Tarifa Diferenciada
        /// </summary>
        /// <param name="bGST">TarifaDiferenciada - Objeto Tarifa Diferenciada
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addTarifaDiferenciada(TarifaDiferenciada oTarifaDiferenciada)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que no exista la tarifa diferenciada

                    //Agregamos la Tarifa Diferenciada
                    TarifaDt.addTarifaDiferenciada(oTarifaDiferenciada, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oTarifaDiferenciada),
                                                           getAuditoriaDescripcion(oTarifaDiferenciada)),
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
        /// Actualizacion de una tarifa diferenciada.
        /// </summary>
        /// <param name="oTarifaDiferenciada">TarifaDiferenciada - Objeto de la Tarifa Diferenciada
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updTarifaDiferenciada(TarifaDiferenciada oTarifaDiferenciada)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos la Tarifa Diferenciada
                    TarifaDt.updTarifaDiferenciada(oTarifaDiferenciada, conn);

                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                       "M",
                                       getAuditoriaCodigoRegistro(oTarifaDiferenciada),
                                       getAuditoriaDescripcion(oTarifaDiferenciada)),
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
        /// Baja de una Tarifa Diferenciada
        /// </summary>
        /// <param name="oTarifaDiferenciada">TarifaDiferenciada - Objeto de la Tarifa Diferenciada
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delTarifaDiferenciada(TarifaDiferenciada oTarifaDiferenciada, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "TITARI", 
                                                       new string[] { oTarifaDiferenciada.CodigoTarifa.ToString() }, 
                                                       new string[] { "TARIFADET" }, 
                                                       nocheck);

                    // Eliminamos la Tarifa Diferenciada
                    TarifaDt.delTarifaDiferenciada(oTarifaDiferenciada, conn);

                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oTarifaDiferenciada),
                                                           getAuditoriaDescripcion(oTarifaDiferenciada)),
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
            private static string getAuditoriaCodigoAuditoria()
            {
                return "TIT";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(TarifaDiferenciada oTarifaDiferenciada)
            {
                return oTarifaDiferenciada.CodigoTarifa.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(TarifaDiferenciada oTarifaDiferenciada)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Descripción", oTarifaDiferenciada.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Porcentaje de Pago",oTarifaDiferenciada.Porcentaje.ToString());

                return sb.ToString();
            }

            #endregion


        #endregion


        #region TARIFAS: Metodos de negocios de la Clase de TARIFAS.

            public static DataSet getRptTarifasDetalle()
            {
                try
                {
                    using (Conexion conn = new Conexion())
                    {
                        //sin transaccion
                        conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                        return TarifaDt.getRptTarifasDetalle(conn);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Cambios de tarifas realizados para mostrar en la grilla, con los filtros ingresados
        /// </summary>
        /// <param name="codigoEstacion">byte - Codigo de estacion de la que deseo conocer los cambios de tarifa</param>
        /// <param name="fechaInicial">datetime - Fecha inicial de vigencia del cambio de tarifa</param>
        /// <returns>Lista de Cambios de tarifa realizados</returns>
        /// ***********************************************************************************************
        public static TarifaL getTarifasCabecera(int? codigoEstacion, DateTime fechaInicial, int? identity)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return TarifaDt.getTarifasCabecera(conn, codigoEstacion, fechaInicial, identity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la tarifa vigente
        /// </summary>
        /// <param name="codigoEstacion">byte - Codigo de estacion de la que deseo conocer los cambios de tarifa</param>
        /// <param name="fechaVigencia">datetime - Fecha de vigencia del cambio de tarifa</param>
        /// <returns>Cambio de tarifa vigente</returns>
        /// ***********************************************************************************************
        public static Tarifa getTarifaVigente(Conexion conn, int? codigoEstacion, DateTime fechaVigencia)
        {
            Tarifa oTarifa = null;
            //Traemos la lista de tarifas que no terminaron ordenadas por fecha de inicio
             TarifaL oTarifas = TarifaDt.getTarifasCabecera(conn, codigoEstacion, fechaVigencia, null);
            //Estan ordenadas, la primera es la vigente
             foreach (Tarifa oTar in oTarifas)
             {
                 if (oTar.FechaInicialVigencia <= fechaVigencia)
                 {
                     oTarifa = oTar;
                     break;
                 }
             }
             return oTarifa;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tarifas de un cambio de tarifa especifico
        /// </summary>
        /// <returns>Lista de tarifas de una determinada fecha y estacion</returns>
        /// ***********************************************************************************************
        public static Tarifa getTarifasDetalle(int identity)
        {

            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    // Objeto cabecera
                    Tarifa oTarifaCabecera = TarifaDt.getTarifasCabecera(conn, null, null, identity)[0];



                    // Rescatamos el detalle de tarifas de la cabecera
                    TarifaDetalleL oTarifaDetalleL = TarifaDt.getTarifasDetalle(conn, identity);


                    // Le enlazamos las tarifas detalladas 
                    oTarifaCabecera.TarifasDetalle = oTarifaDetalleL;


                    return oTarifaCabecera;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna una lista de detalle de tarifas con su habilitacion.
        /// </summary>
        /// <returns>Lista de tarifas de detalle con su estado de habilitacion</returns>
        /// ***********************************************************************************************
        public static TarifaDetalleL getTarifasDetalleHabilitacion()
        {
            try
            {
                // Creamos una lista vacia de detalle de tarifas
                TarifaDetalleL oTarifaDetalleHabilL = null;


                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);


                    // Ahora creamos una lista de detalles de tarifa con su habilitacion
                    oTarifaDetalleHabilL = TarifaDt.getHabilitacionesTarifasDetalle(conn);

                }


                return oTarifaDetalleHabilL; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un cambio de tarifa
        /// </summary>
        /// <param name="oTarifaL">TarifaL - Lista que contiene los valores de las tarifas a grabar</param>
        /// <returns>Lista de Tarifas</returns>
        /// ***********************************************************************************************
        public static void addCambioTarifa(Tarifa oTarifa)
        {
            try
            {
                
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    // Insertamos la cabecera
                    TarifaDt.addTarifaCabecera(oTarifa, conn);

                    //Grabamos auditoria de la cabecera (para que no confundan tantos datos repetidos)
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTarifa(),
                                                           "A",
                                                           getAuditoriaCodigoRegistroCAB(oTarifa),
                                                           getAuditoriaDescripcionCAB(oTarifa)),
                                                           conn);
    

                    //Agregamos el detalle de las tarifas
                    foreach (TarifaDetalle oTarifaDetalle in oTarifa.TarifasDetalle)
                    {
                        TarifaDt.addTarifaDetalle(oTarifa.Identity, oTarifaDetalle, conn);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTarifa(),
                                                               "A",
                                                               getAuditoriaCodigoRegistroCAB(oTarifa),
                                                               getAuditoriaDescripcionDET(oTarifaDetalle)),
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
        /// Modificacion de las tarifas a futuro (todavia no estan vigentes). Las borramos y volvemos a insertar
        /// </summary>
        /// <param name="oTarifa">BandaHoraria - Estructura con la banda horaria a insertar, con el detalle
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCambioTarifa(Tarifa oTarifa)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //No tocamos la cabecera del cambio de tarifa
                    TarifaDt.updTarifaCabecera(oTarifa, conn);

                    // Eliminamos el detalle para volver a insertarlo. 
                    TarifaDt.delTarifaDetalle(oTarifa, conn);


                    //Grabamos auditoria de la cabecera (para que no confundan tantos datos repetidos)
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTarifa(),
                                                           "M",
                                                           getAuditoriaCodigoRegistroCAB(oTarifa),
                                                           getAuditoriaDescripcionCAB(oTarifa)),
                                                           conn);

                    //Agregamos el detalle de las Tarifas
                    foreach (TarifaDetalle oTarifaDetalle in oTarifa.TarifasDetalle)
                    {
                        TarifaDt.addTarifaDetalle(oTarifa.Identity, oTarifaDetalle, conn);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTarifa(),
                                                               "M",
                                                               getAuditoriaCodigoRegistroCAB(oTarifa),
                                                               getAuditoriaDescripcionDET(oTarifaDetalle)),
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
        /// Eliminacion de un cambio de tarifa completo
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delCambioTarifa(Tarifa oTarifa)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //eliminamos el detalle de las tarifas
                    TarifaDt.delTarifaDetalle(oTarifa, conn);

                    //Eliminamos la cabecera del cambio de tarifa
                    TarifaDt.delTarifaCabecera(oTarifa, conn);


                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTarifa(),
                                                           "B",
                                                           getAuditoriaCodigoRegistroCAB(oTarifa),
                                                           getAuditoriaDescripcionCAB(oTarifa)),
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
            private static string getAuditoriaCodigoAuditoriaTarifa()
            {
                return "CAM";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado (con la cabecera es suficiente)
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistroCAB(Tarifa oTarifa)
            {
                return oTarifa.Identity.ToString();
            }

            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion de la cabecera del registro afectado 
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcionCAB(Tarifa oTarifa)
            {
                StringBuilder sb = new StringBuilder();

                // Cabecera del registro de auditoria
                AuditoriaBs.AppendCampo(sb, "Estación", oTarifa.Estacion.Nombre);
                AuditoriaBs.AppendCampo(sb, "Válido Desde", oTarifa.FechaInicialVigencia.ToString());
                if (oTarifa.FechaFinalVigencia != null)
                    AuditoriaBs.AppendCampo(sb, "Válido Hasta", oTarifa.FechaFinalVigencia.ToString());
                AuditoriaBs.AppendCampo(sb, "IVA %", oTarifa.PorcentajeIva.ToString("F02"));

                return sb.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del detalle del registro afectado 
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcionDET(TarifaDetalle oTarifaDetalle)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Categoría", oTarifaDetalle.Categoria.Descripcion.ToString());
                AuditoriaBs.AppendCampo(sb, "Tipo de Tarifa", oTarifaDetalle.TarifaDiferenciada.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Tipo Día", oTarifaDetalle.TipoDia.DescripcionCorta);
                AuditoriaBs.AppendCampo(sb, "Valor", oTarifaDetalle.sValorTarifa);

                return sb.ToString();
            }

            #endregion


        #endregion

        
        #region AUTORIZACION_PASO: Clase de Negocios de Autorizaciones de Paso definidas


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Autorizaciones de paso definidas. No recibe parametros, invoca al mismo 
        /// metodo pero con valor NULL para que retorne todos los registros
        /// </summary>
        /// <returns>Lista de  Autorizaciones de paso</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoL getAutorizacionesPaso()
        {
            return getAutorizacionesPaso(ConexionBs.getGSToEstacion(), null, null, null, null, null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tarifas diferenciadas definidas (se usa en el filtro, los sentidos se pasan en NULL). 
        /// </summary>
        /// <param name="estacionOrigen">int? - Estacion de origen para el filtrado</param>
        /// <param name="categoria">byte? - Categoria a filtrar</param>
        /// <param name="estacionDestino">int? - Estacion de destino para el filtrado</param>
        /// <returns>Lista de Autorizaciones de Paso</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoL getAutorizacionesPaso(int? estacionOrigen, byte? categoria, int? estacionDestino)
        {
            return getAutorizacionesPaso(ConexionBs.getGSToEstacion(), estacionOrigen, null, categoria, estacionDestino, null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tarifas diferenciadas definidas. 
        /// </summary>
        /// <param name="estacionOrigen">int? - Estacion de origen para el filtrado</param>
        /// <param name="sentidoOrigen">string - Sentido de origen para el filtrado</param>
        /// <param name="categoria">byte? - Categoria a filtrar</param>
        /// <param name="estacionDestino">int? - Estacion de destino para el filtrado</param>
        /// <param name="sentidoDestino">string - Sentido de destino para el filtrado</param>
        /// <returns>Lista de Autorizaciones de Paso</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoL getAutorizacionesPaso(int? estacionOrigen, string sentidoOrigen,
                                                                    byte? categoria, int? estacionDestino, string sentidoDestino)
        {
            return getAutorizacionesPaso(ConexionBs.getGSToEstacion(), estacionOrigen, sentidoOrigen, 
                                         categoria, estacionDestino, sentidoDestino);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tarifas diferenciadas definidas.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="estacionOrigen">int? - Estacion de origen para el filtrado</param>
        /// <param name="sentidoOrigen">string - Sentido de origen para el filtrado</param>
        /// <param name="categoria">byte? - Categoria a filtrar</param>
        /// <param name="estacionDestino">int? - Estacion de destino para el filtrado</param>
        /// <param name="sentidoDestino">string - Sentido de destino para el filtrado</param>
        /// <returns>Lista de Autorizaciones de Paso</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoL getAutorizacionesPaso(bool bGST, int? estacionOrigen, string sentidoOrigen, 
                                                                    byte? categoria, int? estacionDestino, string sentidoDestino)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return TarifaDt.getAutorizacionesPaso(conn, estacionOrigen, sentidoOrigen, categoria, estacionDestino, sentidoDestino);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Autorizacion de Paso
        /// </summary>
        /// <param name="oTarifaAutorizacionPaso">TarifaAutorizacionPaso - Datos de la autorizacion de paso a insertar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addAutorizacionPaso(TarifaAutorizacionPaso oTarifaAutorizacionPaso)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);


                    //Agregamos la Autorizacion de paso
                    TarifaDt.addAutorizacionPaso(oTarifaAutorizacionPaso, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAutorizacionPaso(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oTarifaAutorizacionPaso),
                                                           getAuditoriaDescripcion(oTarifaAutorizacionPaso)),
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
        /// Actualizacion de una Autorizacion de paso
        /// </summary>
        /// <param name="oTarifaAutorizacionPaso">TarifaAutorizacionPaso - Datos de la autorizacion de paso a modificar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updAutorizacionPaso(TarifaAutorizacionPaso oTarifaAutorizacionPaso)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos la Tarifa Diferenciada
                    TarifaDt.updAutorizacionPaso(oTarifaAutorizacionPaso, conn);

                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAutorizacionPaso(),
                                             "M",
                                             getAuditoriaCodigoRegistro(oTarifaAutorizacionPaso),
                                             getAuditoriaDescripcion(oTarifaAutorizacionPaso)),
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
        /// Baja de una Autorizacion de paso
        /// </summary>
        /// <param name="oTarifaAutorizacionPaso">TarifaAutorizacionPaso - Datos de la autorizacion de paso a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delAutorizacionPaso(TarifaAutorizacionPaso oTarifaAutorizacionPaso)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);


                    // Eliminamos la Autorizacion de paso
                    TarifaDt.delAutorizacionPaso(oTarifaAutorizacionPaso, conn);

                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAutorizacionPaso(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oTarifaAutorizacionPaso),
                                                           getAuditoriaDescripcion(oTarifaAutorizacionPaso)),
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
            private static string getAuditoriaCodigoAuditoriaAutorizacionPaso()
            {
                return "AUT";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(TarifaAutorizacionPaso oTarifaAutorizacionPaso)
            {
                return oTarifaAutorizacionPaso.EstacionOrigen.Numero.ToString() + "-" +
                       oTarifaAutorizacionPaso.EstacionDestino.Numero.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(TarifaAutorizacionPaso oTarifaAutorizacionPaso)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Estación Origen", oTarifaAutorizacionPaso.EstacionOrigen.Nombre);
                AuditoriaBs.AppendCampo(sb, "Sentido Origen", oTarifaAutorizacionPaso.SentidoOrigen.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Categoría", oTarifaAutorizacionPaso.Categoria.Descripcion.ToString());
                AuditoriaBs.AppendCampo(sb, "Estación Destino", oTarifaAutorizacionPaso.EstacionDestino.Nombre);
                AuditoriaBs.AppendCampo(sb, "Sentido Destino", oTarifaAutorizacionPaso.SentidoDestino.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Forma de Descuento", oTarifaAutorizacionPaso.FormaDescuento.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Lapso (minutos)", oTarifaAutorizacionPaso.MinutosVigencia.ToString());
                if (oTarifaAutorizacionPaso.TipoTarifa != null)
                {
                    AuditoriaBs.AppendCampo(sb, "Tipo de Tarifa", oTarifaAutorizacionPaso.TipoTarifa.Descripcion);                        
                }
                
                return sb.ToString();
            }

            #endregion


        #endregion


        #region TARIFAAUTORIZACIONPASOFORMADESCUENTO: Las posibles formas de pago que se pueden configurar en la autorizacion de paso

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las formas de descuento que se pueden utilizar en una autorizacion de paso
        /// </summary>
        /// <returns>Lista de formas de descuento</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoFormaDescuentoL getFormasDescuentoAutorizacionPaso()
        {
            TarifaAutorizacionPasoFormaDescuentoL oFormasDescuento = new TarifaAutorizacionPasoFormaDescuentoL();
            oFormasDescuento = TarifaDt.getTarifasAutorizacionPasoFormaDescuento();


            return oFormasDescuento;
        }           

        #endregion
    }
}
