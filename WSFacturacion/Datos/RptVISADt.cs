using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class RptVISADt
    {

        ///*******************************************************************************************************
        /// <summary>
        ///  Obtiene datos para reporte proximos pagos VISA
        /// </summary>
        /// <param name="oConn">Conexion (Consolidado)</param>
        /// <param name="desde">Fecha Desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// <param name="fecha">Agrupacion</param>
        /// <returns>DateSet-RptPeaje_ProximosPagosVISADS</returns>
        ///*******************************************************************************************************
        public static DataSet getProximosPagosVISA(Conexion oConn, DateTime desde, DateTime hasta, string fecha)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ProximosPagosVISADS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptVISA_getProximosPagos";
                oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@Agrupamiento", SqlDbType.Char, 1).Value = fecha;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ProximosPagosVISA");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResultado;
        }


        ///**************************************************************************************************
        /// <summary>
        /// Obtiene los datos para armar el reporte de transitos rechazados/acepados de VISA
        /// </summary>
        /// <param name="conn">Conexion (Consolidado)</param>
        /// <param name="desde">Fecha desde</param>
        /// <param name="hasta">Fecha Hasta</param>
        /// <param name="iEstacion">Estacion</param>
        /// <param name="sDetalle">Detalle</param>
        /// <returns></returns>
        ///**************************************************************************************************
        public static DataSet getTransitosRechazadosVISA(Conexion oConn, DateTime desde, DateTime hasta, int? iEstacion, string sDetalle)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_TransitosRechazadosAceptadosVISADS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptVISA_TransitosRechazadosAceptados";
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@estac", SqlDbType.TinyInt, 1).Value = iEstacion;
                oCmd.Parameters.Add("@Detalle", SqlDbType.Char, 1).Value = sDetalle;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "Transitos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResultado;
        }


        ///**************************************************************************************************
        /// <summary>
        /// Obtiene los datos para el reporte facturacion por programacion de pagamento visa
        /// </summary>
        /// <param name="conn">Conexion (Consolidado)</param>
        /// <param name="desde">FechaDesde</param>
        /// <param name="hasta">FechaHasta</param>
        /// <param name="iEstacion">Estacion</param>
        /// <param name="iIntervaloFacturacion">IntervaloFacturacion</param>
        /// <param name="iIntervaloPagamento">IntervaloPagamento</param>
        /// <param name="PorPlaza">Agrupado por plaza</param>
        /// <returns></returns>
        ///**************************************************************************************************
        public static DataSet getFacturacionPorProgDePagoVISA(Conexion oConn, DateTime desde, DateTime hasta, int? iEstacion, char? iIntervaloFacturacion, char? iIntervaloPagamento, bool PorPlaza)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_FacturacionporprogDePagoVISADS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptVISA_getFacturacionyProximosPagosVISA";

                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt, 1).Value = iEstacion;
                oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char, 1).Value = (PorPlaza == true ? "S" : "N");
                oCmd.Parameters.Add("@IntervaloFacturacion", SqlDbType.Char, 1).Value = iIntervaloFacturacion;
                oCmd.Parameters.Add("@IntervaloPagamento", SqlDbType.Char, 1).Value = iIntervaloPagamento;


                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "Visa");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResultado;
        }


        /// <summary>
        /// Secuencias RO de los archivos importados de VISA.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="iEstacion"></param>
        /// <param name="secuenciaRO"></param>
        /// <returns></returns>
        public static DataSet getSecuenciasRO(Conexion conn, DateTime desde, DateTime hasta, int? iEstacion, int? secuenciaRO)
        {
            DataSet dsRes = new DataSet();
            dsRes.DataSetName = "RptVisa_SecuenciasRO";
            try
            {
                SqlCommand oCmd = new SqlCommand();
                
                oCmd.Connection = conn.conection;
                oCmd.CommandType=CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptVISA_SecuenciasRO";
                oCmd.Parameters.Add(new SqlParameter("JornadaDesde", SqlDbType.DateTime)).Value = desde;
                oCmd.Parameters.Add(new SqlParameter("JornadaHasta", SqlDbType.DateTime)).Value = hasta;
                oCmd.Parameters.Add(new SqlParameter("coest", SqlDbType.TinyInt)).Value = iEstacion;
                oCmd.Parameters.Add(new SqlParameter("NumeroRO", SqlDbType.Int)).Value = secuenciaRO;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter sqa = new SqlDataAdapter(oCmd);
                sqa.Fill(dsRes, "VISA");

                oCmd = null;
                sqa.Dispose();
            }
            catch (Exception ex )
            {
                
                throw ex;
            }
            return dsRes;
        }
    
    }
}
