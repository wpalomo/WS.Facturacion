using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Detalle de Exonerados
    /// </summary>
    /// ***********************************************************************************************
    public class RptViolacionesDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el Detalle de Violaciones
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleViolaciones(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion,string Placa)
        {
            DataSet dsViolaciones = new DataSet();
            dsViolaciones.DataSetName = "rptViolaciones_DetalleViolaciones"; 

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 360;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptViolaciones_getDetalleViolaciones";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
             //   oCmd.Parameters.Add("@agrupacion", SqlDbType.Char, 1).Value = agrupacion;
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = fechahoraDesde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = fechahoraHasta;
                oCmd.Parameters.Add("@Placa", SqlDbType.VarChar).Value = Placa;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsViolaciones, "Violaciones");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception)
            {
                throw;
            }

            return dsViolaciones;
        }




        ///*********************************************************************************************************
        /// <summary>
        /// Arma el dataset con el total de transitos filtrando solo las violaciones, Agrupados por jornada
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="fechahoraDesde"></param>
        /// <param name="fechahoraHasta"></param>
        /// <param name="zona"></param>
        /// <param name="estacion"></param>
        /// <param name="Placa"></param>
        /// <returns></returns>
        ///*********************************************************************************************************
        public static DataSet getEstadisticoViolaciones(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
        int? zona, int? estacion, int? nuvia,string agrupacion, int? fragmentacion, int? horacorte)
        {
            DataSet dsViolaciones = new DataSet();
            dsViolaciones.DataSetName = "rptViolaciones_EstadisticoViolacionesDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 360;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstViolacionesSentido";
                oCmd.Parameters.Add("@agrupacion", SqlDbType.Char, 1).Value = agrupacion;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Desde", SqlDbType.DateTime).Value = fechahoraDesde;
                oCmd.Parameters.Add("@Hasta", SqlDbType.DateTime).Value = fechahoraHasta;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = nuvia;
                oCmd.Parameters.Add("@wfragm", SqlDbType.Int).Value = fragmentacion;
                oCmd.Parameters.Add("@horacorte", SqlDbType.Int).Value = horacorte;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsViolaciones, "Violaciones");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception)
            {
                throw;
            }

            return dsViolaciones;
        }

    }
}

