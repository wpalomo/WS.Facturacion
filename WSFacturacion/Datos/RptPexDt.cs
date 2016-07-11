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
    public class RptPexDt
    {

        public static DataSet getTransitosRechazados(Conexion oConn, DateTime desde, DateTime hasta, string fecha, int? coest, string status)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptPeaje_TransitosRechazadosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getTransitosRechazados";
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = coest;
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@fecha", SqlDbType.Char).Value = fecha;
                oCmd.Parameters.Add("@status", SqlDbType.Char).Value = status;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "ListaTransitos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getControlTotales(Conexion oConn, DateTime desde, DateTime hasta, int? coest)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ControlTotalesDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getControlTotales";
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = coest;
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ControlTotales");

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

        public static DataSet getControlSecuencias(Conexion oConn, DateTime desde, DateTime hasta, string fecha, int? coest, char? estado)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ControlSecuenciasDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getControlSecuencias";                
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@fecha", SqlDbType.Char).Value = fecha;
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = coest;
                oCmd.Parameters.Add("@estado", SqlDbType.Char).Value = estado;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ControlSecuencias");

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

        public static DataSet getProximosPagos(Conexion oConn, DateTime? desde, DateTime? hasta, string fecha, int? nroSecuenciaInicio, int? nroSecuenciaFin)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ProximosPagosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getProximosPagos";
                oCmd.Parameters.Add("@fecha", SqlDbType.Char).Value = fecha;
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@nroSecuenciaIni", SqlDbType.Int).Value = nroSecuenciaInicio;
                oCmd.Parameters.Add("@nroSecuenciaFin", SqlDbType.Int).Value = nroSecuenciaFin;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ProximosPagos");

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

        public static DataSet getProximosPagos(Conexion oConn, string fecha, int? nroSecuenciaInicio, int? nroSecuenciaFin)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ProximosPagosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptPex_getProximosPagosPorNroSecuencia";
                oCmd.Parameters.Add("@fecha", SqlDbType.Char).Value = fecha;
                oCmd.Parameters.Add("@nroSecuenciaIni", SqlDbType.Int).Value = nroSecuenciaInicio;
                oCmd.Parameters.Add("@nroSecuenciaFin", SqlDbType.Int).Value = nroSecuenciaFin;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ProximosPagos");

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

        public static DataSet getReenvioTransitos(Conexion oConn, DateTime? desde, DateTime? hasta, int? zona, int? estacion)
        {
            DataSet dsResultado = new DataSet();
            dsResultado.DataSetName = "RptPeaje_ReenvioTransitosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptOSAs_getReenvioTransitos";
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@zona", SqlDbType.Int).Value = zona;
                oCmd.Parameters.Add("@estacion", SqlDbType.Int).Value = estacion;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsResultado, "ReenvioTransitos");

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
    }
}
