using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Tesoreria
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Detalle de Recaudacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptDetalleParteJornadaBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los partes de una jornada y todos sus totales
        /// </summary>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">sSentido - Sentido de circulacion</param>
        /// ***********************************************************************************************
        public static DataSet getPartesDetalle(DateTime jornadaDesde, DateTime jornadaHasta,
            int? zona, int? estacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            return getPartesDetalle(null, jornadaDesde, jornadaHasta, zona, estacion, "JORNADA", turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los totales de un parte
        /// </summary>
        /// <param name="parte">int - Numero de Parte</param>
        /// <param name="estacion">int - Codigo de Estacion</param>
        /// <param name="porBloque">bool - Detallado por bloque</param>
        /// ***********************************************************************************************
        public static DataSet getPartesDetalle(int parte, int estacion, bool porBloque)
        {
            return getPartesDetalle((int?) parte, null, null, null, (int?) estacion, porBloque?"BLOQUE":"PARTE", null, null, true, true, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los partes de una jornada o parte y todos sus totales
        /// </summary>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">sSentido - Sentido de circulacion</param>
        /// ***********************************************************************************************
        protected static DataSet getPartesDetalle(int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta,
            int? zona, int? estacion, string tipoAgrupacion, int? turnoDesde, int? turnoHasta,
            bool porEstacion, bool porJornada, string sSentido)
        {
            DataSet dsPartes = null;
            string tipoParte = null;     //NO VALIDAN!!!!!
            // En la plaza con validacion distribuida mostramos partes liquidados sin validar
            // valkidacion  lugar       tipoParte
            // cent         plaza       L
            // cent         Gst         V
            // dist         plaza       V
            // dist         Gst         V
            if (Validacion.ConfigValidacion.ValidacionCentralizada && !ConexionBs.getGSToEstacion())
            {
                tipoParte = "L";
            }

            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                //DataSet inicial
                dsPartes = RptDetalleParteJornadaDt.getPartesAgrupados(conn, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);

                //Totales de Transitos
                RptDetalleParteJornadaDt.getTransitos(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);
                /*
                //Tickets Faltantes
                RptDetalleParteJornadaDt.getTicketsFaltantes(conn, dsPartes, parte, estacion, jornadaDesde, jornadaHasta, zona,
                    turnoDesde, turnoHasta);

                //Tickets Duplicados
                RptDetalleParteJornadaDt.getTicketsDuplicados(conn, dsPartes, parte, estacion, jornadaDesde, jornadaHasta, zona,
                    turnoDesde, turnoHasta);*/

                //Totales de Recaudado
                RptDetalleParteJornadaDt.getRecaudado(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);

                //Resumen Total
                RptDetalleParteJornadaDt.getResumenTotal(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);

                //Tabulado contra Detectado
                RptDetalleParteJornadaDt.getTabuladasDetectadas(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);

                //Anomalias
                RptDetalleParteJornadaDt.getAnomalias(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);

                //Bloques (no lo hago para jornada)
                if (tipoAgrupacion != "JORNADA")
                {
                    RptDetalleParteJornadaDt.getBloques(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);
                }

                //Denominaciones (no lo hago para detalle por bloque)
                if (tipoAgrupacion != "BLOQUE")
                {
                    RptDetalleParteJornadaDt.getDenominaciones(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada);
                }

                //Detalle Cupones
                if (tipoAgrupacion != "BLOQUE")
                {
                    RptDetalleParteJornadaDt.getCuponLiquidacion(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada);
                }

                //Bolsas (solo lo hago para parte)
                if (tipoAgrupacion == "PARTE")
                {
                    RptDetalleParteJornadaDt.getBolsas(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada);
                }
                //Monedas (no lo hago para bloque)
                if (tipoAgrupacion != "BLOQUE")
                {
                    RptDetalleParteJornadaDt.getMonedas(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada);
                }
                //Ventas (no lo hago para bloque)
                if (tipoAgrupacion != "BLOQUE")
                {
                    RptDetalleParteJornadaDt.getVentas(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, sSentido);
                }
                //Resultado (no lo hago para bloque)
                if (tipoAgrupacion != "BLOQUE")
                {
                    RptRecaudacionDt.getPartesDS(conn, dsPartes, "Resultado", parte, jornadaDesde, jornadaHasta,
                                    zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada,
                                    null, tipoParte, "S");
                }

            }

            /*
            //Se traba al ejecutar el siguiente SP
            //Probamos cerrar y reabrir la conexion
            //Facturas por punto de Venta
            if (tipoAgrupacion == "JORNADA")
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    RptDetalleParteJornadaDt.getFacturacionPV(conn, dsPartes, parte, jornadaDesde, jornadaHasta,
                                        zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada);

                }
            }
            
            if (tipoAgrupacion == "JORNADA")
            {
                //Recorro la tabla Resultado 
                //y para cada registro copio RecaudaoNeto + MontoViolaciones a los registros correspondientes de FacturasPorPV

                foreach (DataRow row in dsPartes.Tables["Resultado"].Rows)
                {
                    decimal total = (decimal) row["TotalFacturas"] ;
                    DateTime grpFecha = (DateTime) row["aux_fecha"];
                    int grpEstacion = (int) row["aux_estac"];
                    int grpParte = (int)row["aux_parte"];

                    foreach (DataRow row2 in dsPartes.Tables["FacturasPorPV"].Rows)
                    {
                        if (grpFecha == (DateTime)row2["aux_fecha"]
                            && grpEstacion == (int)row2["est_codig"]
                            && grpParte == (int)row2["aux_parte"])
                        {
                            row2["TotalFacturado"] = total;
                        }
                    }
                }  
                     
            }
            
            */

            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con las Anomalias e Incidencias de un parte
        /// </summary>
        /// <param name="parte">int - Numero de Parte</param>
        /// <param name="estacion">int - Codigo de Estacion</param>
        /// <param name="porBloque">bool - Detallado por bloque</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(int parte, int estacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                //DataSet inicial
                DataSet dsPartes = RptDetalleParteJornadaDt.getAnomaliasEventos(conn, estacion,  parte);

                return dsPartes;
            }
        }

        public static DataSet getCambiosEstado(int parte, int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    //DataSet inicial
                    DataSet dsPartes = RptDetalleParteJornadaDt.getCambiosEstado(conn, estacion, parte);

                    return dsPartes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
