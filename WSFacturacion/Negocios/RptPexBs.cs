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
    /// Clase de negocios para reportes de clientes
    /// </summary>
    /// ***********************************************************************************************
    public class RptPexBs
    {
        public static DataSet getTransitosRechazados(DateTime desde, DateTime hasta, string fecha, int? coest, string status)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptPexDt.getTransitosRechazados(conn, desde, hasta, fecha, coest, status);
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
                    return RptPexDt.getControlTotales(conn, desde, hasta, coest);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getControlSecuencias(DateTime desde, DateTime hasta, string fecha, int? coest, char? estado)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptPexDt.getControlSecuencias(conn, desde, hasta,fecha, coest,estado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getProximosPagos(DateTime? desde, DateTime? hasta, string fecha, int? nroSecuenciaInicio, int? nroSecuenciaFin)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    DataSet ds = new DataSet();
                    //if (nroSecuenciaInicio == null)
                    //{
                        ds = RptPexDt.getProximosPagos(conn, desde, hasta, fecha, nroSecuenciaInicio, nroSecuenciaFin);
                    //}
                    //else
                    //{
                    //    ds = RptPexDt.getProximosPagos(conn, fecha, nroSecuenciaInicio, nroSecuenciaFin);
                    //}
                    return ds;                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getReenvioTransitos(DateTime desde, DateTime hasta, int? zona, int? estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarConsolidado(false);
                    return RptPexDt.getReenvioTransitos(conn, desde, hasta, zona, estacion);
                    return new DataSet();
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
        public static DataSet getDetalleTransitoPex(DateTime desde, DateTime hasta, int? estacion, string categoria, string estadoPex, string tipoIngreso, string modoMantenimiento, string placa, string emisor, string tag)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                //Eliminamos el tag
                return PexDt.getDetalleTransitosPex(conn, desde, hasta, estacion, categoria, estadoPex, tipoIngreso, modoMantenimiento, placa, emisor, tag);       
            }

        }
    }
}
