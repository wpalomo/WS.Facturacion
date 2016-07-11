using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Facturacion
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase de negocios para reportes de clientes
    /// </summary>
    /// ***********************************************************************************************
    public class RptOSAsBs
    {
        public static DataSet getTransitosRechazados(DateTime desde, DateTime hasta, string fecha, int? coest, int? iAdmTag)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptOSAsDt.getTransitosRechazados(conn, desde, hasta, fecha, coest, iAdmTag);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getControlTotales(DateTime desde, DateTime hasta, int? coest)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptOSAsDt.getControlTotales(conn, desde, hasta, coest);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getControlSecuencias(DateTime desde, DateTime hasta, string fecha, int? coest, char? estado, int? iAdmTag)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptOSAsDt.getControlSecuencias(conn, desde, hasta,fecha, coest,estado, iAdmTag);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getProximosPagos(DateTime desde, DateTime hasta, string fecha, int? iAdmTag)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptOSAsDt.getProximosPagos(conn, desde, hasta, fecha, iAdmTag);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Administradora"></param>
        /// <returns></returns>
        public static DataSet getDetalleTransitoPex(DateTime desde, DateTime hasta, int? estacion, string categoria, string estadoPex, string tipoIngreso, string modoMantenimiento, int? iAdmTag, string placa)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                //Eliminamos el tag
                return OSAsDt.getDetalleTransitosPex(conn, desde, hasta, estacion, categoria, estadoPex, tipoIngreso, modoMantenimiento, iAdmTag, placa);
            }

        }

        /***********************************************************************************************************/
        /// <summary>
        /// Arma el Data set para el reporte de Facturacion por programacion de pagamento
        /// </summary>
        /// <param name="desde">Fecha Desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// <param name="iEstacion">Estacion - 0 Todas</param>
        /// <param name="TipoFecha">Tipo de Fecha - J(Jornada) E(Envio)</param>
        /// <param name="iAdmTag">Administradora de Tag</param>
        /// <param name="iIntervaloFacturacion">Intervalo Facturacion (MES/DIA)</param>
        /// <param name="iIntervaloPagamento">Intervalo Pagamento (MES/DIA)</param>
        /// <param name="iTipoTag">Tipo de Tag</param>
        /// <returns>DataSet</returns>
        /***********************************************************************************************************/
        public static DataSet getFacturacionPorProgDePago(DateTime desde, DateTime hasta, int? iEstacion, string TipoFecha, int? iAdmTag, char? iIntervaloFacturacion, char? iIntervaloPagamento, char? iTipoTag, bool PorPlaza)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return OSAsDt.getFacturacionPorProgDePago(conn, desde, hasta, iEstacion, TipoFecha, iAdmTag, iIntervaloFacturacion, iIntervaloPagamento, iTipoTag, PorPlaza);
            }
        }
    }
}
