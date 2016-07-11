using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Configuration;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Facturacion
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase de negocios para reportes de VISA
    /// </summary>
    /// ***********************************************************************************************
    public class RptVISABs
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene datos para reporte proximos pagos VISA
        /// </summary>
        /// <param name="desde">Fecha Desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// <param name="fecha">Agrupacion</param>
        /// <returns>DateSet-RptPeaje_ProximosPagosVISADS</returns>
        /// ***********************************************************************************************
        public static DataSet getProximosPagosVISA(DateTime desde, DateTime hasta, string fecha)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptVISADt.getProximosPagosVISA(conn, desde, hasta, fecha);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///**************************************************************************************************
        /// <summary>
        /// Obtiene los datos para armar el reporte de transitos rechazados/acepados de VISA
        /// </summary>
        /// <param name="desde">Fecha Desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// <param name="iEstacion">Estacion</param>
        /// <param name="sDetalle">Detalle del reporte</param>
        /// <returns></returns>
        ///**************************************************************************************************
        public static DataSet getTransitosRechazadosVISA(DateTime desde, DateTime hasta, int? iEstacion, string sDetalle)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptVISADt.getTransitosRechazadosVISA(conn,desde,hasta,iEstacion,sDetalle);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///**************************************************************************************************
        /// <summary>
        /// Obtiene los datos para el reporte facturacion por programacion de pagamento visa
        /// </summary>
        /// <param name="desde">Fecha desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// <param name="iEstacion">Estacion</param>
        /// <param name="iIntervaloFacturacion">Intervalo Facturacion</param>
        /// <param name="iIntervaloPagamento">Intervalo Pago</param>
        /// <param name="PorPlaza">Agrupado por plaza</param>
        /// <returns></returns>
        ///**************************************************************************************************
        public static DataSet getFacturacionPorProgDePagoVISA(DateTime desde, DateTime hasta, int? iEstacion, char? iIntervaloFacturacion, char? iIntervaloPagamento, bool PorPlaza)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptVISADt.getFacturacionPorProgDePagoVISA(conn, desde, hasta, iEstacion, iIntervaloFacturacion, iIntervaloPagamento, PorPlaza);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Reporte de secuencias RO importadas.
        /// </summary>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="iEstacion"></param>
        /// <param name="secuenciaRO"></param>
        /// <returns></returns>
        public static DataSet getSecuenciasRO(DateTime desde, DateTime hasta, int? iEstacion, int? secuenciaRO)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidado(false);
                    return RptVISADt.getSecuenciasRO(conn, desde, hasta, iEstacion, secuenciaRO);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
