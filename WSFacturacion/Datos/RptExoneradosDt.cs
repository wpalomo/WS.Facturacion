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
    public class RptExoneradosDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el Detalle de Exonerados
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleExonerados(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, string operador)
        {
            DataSet dsExonerados = new DataSet();
            dsExonerados.DataSetName = "rptExonerados_DetalleExonerados"; 

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 360;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptExonerados_getDetalleExonerados";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = fechahoraDesde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = fechahoraHasta;
                oCmd.Parameters.Add("@idOpe", SqlDbType.VarChar).Value = operador;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsExonerados, "Exonerados");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception)
            {
                throw;
            }

            return dsExonerados;
        }
    }
}
