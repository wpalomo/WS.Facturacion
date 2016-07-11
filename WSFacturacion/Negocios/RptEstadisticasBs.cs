using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Estadisticas de Transito
    /// </summary>
    /// ***********************************************************************************************
    public class RptEstadisticasBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Sentido y Porcentaje
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstSentidoPorcentajes(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP,string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstSentidoPorcentajes(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP,sModo);
            }
        }
                
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Sentido y Forma de Pago
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos">string  - incluye o no exentos</param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstSentidoFPago(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac,string sIncluirFP,string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstSentidoFPago(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP,sModo);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Sentido y Categoria
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstSentidoCategorias(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria,string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstSentidoCategorias(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP, sCategoria,sModo);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Volumen Diario Clasificado
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstVolumenDiario(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstVolumenDiario(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP, sCategoria, sModo);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Flujo Diario Clasificado
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstFlujoDiario(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstFlujoDiario(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP, sCategoria, sModo);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Ingresos Plaza Clasificado
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstIngresosPlaza(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstIngresosPlaza(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP, sCategoria, sModo);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Ingresos Mensuales
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstIngresosMensuales(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstIngresosMensuales(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP, sCategoria, sModo);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Espacial
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstEspacial(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria,string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstEspacial(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP, sCategoria,sModo);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Temporal
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstTemporal(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP,string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstTemporal(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac, sIncluirFP,sModo);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Categoria y Vias
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sModoMantenim">string - Indica si se agregan o no los partes en modo mantenimiento</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstCategoriaVias(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, string sModoMantenim, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria,string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstCategoriaVias(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sModoMantenim, sIncluirExentos, sIncluirViolac, sIncluirFP, sCategoria,sModo);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Categoria y Equivalente
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="fragmentacion">string - Dia o Mes</param>
        /// ***********************************************************************************************
        public static DataSet getEstCategoriaEquivalente(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, string fragmentacion, string sIncluirExentos)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptEstadisticasDt.getEstCategoriaEquivalente(conn, fechahoraDesde, fechahoraHasta, zona, estacion, fragmentacion, sIncluirExentos);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Categoria y Forma de Pago
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstCategoriaFPago(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, out Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac,string sIncluirFP, string sCategoria,string sModo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DateTime dtHoraCorte = new DateTime();
                dtHoraCorte = RptEstadisticasDt.getHoraCorte(conn, zona, estacion);

                if (dtHoraCorte != null)
                {
                    HoraCorte = Convert.ToInt16(dtHoraCorte.Hour);
                }
                else
                {
                    HoraCorte = 0;
                }

                return RptEstadisticasDt.getEstCategoriaFPago(conn, fechahoraDesde, fechahoraHasta, zona, estacion, via, agrupacion, fragmentacion, HoraCorte, sIncluirExentos, sIncluirViolac,sIncluirFP, sCategoria,sModo);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica de Tabuladas y Detectadas
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="operador">string - Operador</param>
        /// <param name="formaPago">FormaPago - formaPago</param>
        /// <param name="string">string - sTipoDiscrepancia</param>
        /// ***********************************************************************************************
        public static DataSet getTabuladasDetectadas(DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? estacion, int? via, string operador, FormaPago formaPago, string sTipoDiscrepancia)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptEstadisticasDt.getTabuladasDetectadas(conn, fechahoraDesde, fechahoraHasta, estacion, via, operador, formaPago, sTipoDiscrepancia);
            }
        }

                
        ///**********************************************************************************************************
        /// <summary>
        /// Obtiene el detalle de los transitos con ejes suspenso Detallado
        /// </summary>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="estacion"></param>
        /// <param name="via"></param>
        /// <returns></returns>
        ///**********************************************************************************************************
        public static DataSet getEstadDetalladaEjesSuspensos(DateTime fechahoraDesde, DateTime fechahoraHasta, int? estacion, int? via, string modo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptEstadisticasDt.getEstadDetalladaEjesSuspensos(conn, fechahoraDesde, fechahoraHasta, estacion, via, modo);
            }
        }


        ///**********************************************************************************************************
        /// <summary>
        /// Obtiene estadistico de ejes suspensos
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="estacion"></param>
        /// <param name="via"></param>
        /// <returns></returns>
        ///**********************************************************************************************************
        public static DataSet getEstadisticoEjesSuspensos(DateTime fechahoraDesde, DateTime fechahoraHasta, int? estacion, int? via, string modo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptEstadisticasDt.getEstadisticoEjesSuspensos(conn, fechahoraDesde, fechahoraHasta, estacion, via, modo);
            }
        }

        public static DataSet getEstadisticoExentos(DateTime fechahoraDesde, DateTime fechahoraHasta, int? estacion,
            int? via, string cliente, string placa, bool detallado,bool porEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                int nClient;
                
                Int32.TryParse(cliente,out nClient);
                
                return RptEstadisticasDt.getEstadisticoExentos(conn,fechahoraDesde,fechahoraHasta,estacion,via,nClient,placa,detallado,porEstacion);

            }
        }

        public static DataSet getEstadistico915(DateTime desde, DateTime hasta, int? estacion,int? admtag)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                
                return RptEstadisticasDt.getEstadistico915(conn, desde, hasta, estacion,admtag);

            }
        }

        public static DataSet ANTT_getEstCategoriaDia(DateTime desde, DateTime hasta, int? estacion, string sIncluyeExentos, string sIncluirViolac, string sIncluirFP)
        {

            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                return RptEstadisticasDt.ANNT_getEstCaegoriaDia(conn,desde, hasta, estacion, sIncluyeExentos, sIncluirViolac, sIncluirFP);

            }
        }
    }
}

