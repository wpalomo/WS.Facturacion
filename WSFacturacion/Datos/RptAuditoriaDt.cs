using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using System.IO;

namespace Telectronica.Peaje
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Auditoría
    /// </summary>
    /// ***********************************************************************************************
    public class RptAuditoriaDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los registros de auditoría solicitados
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="codigo">int? - Codigo a buscar</param>
        /// <param name="xmlOperaciones">string - XML con los códigos de Tabaud</param>
        /// ***********************************************************************************************
        public static DataSet getLogAuditoria(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta,
                int? zona, int? estacion, string operador, string codigo, AuditoriaCodigoL oOperaciones)
        {
            DataSet dsAudito = new DataSet();
            dsAudito.DataSetName = "RptAuditoria_DS";
            try
            {
                //Pasamos a xml la lista de objetos.
                string xmlOperaciones = "";
                xmlOperaciones = Utilitarios.xmlUtil.SerializeObject(oOperaciones);

                /*
                StreamWriter sw = new StreamWriter("c:\\xml.xml");

                sw.Write(xmlOperaciones);
                sw.Close();
                 */

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RptAuditoria";
                oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = operador;
                oCmd.Parameters.Add("@codig", SqlDbType.VarChar, 50).Value = codigo;
                oCmd.Parameters.Add("@xmlOperaciones", SqlDbType.NText).Value = xmlOperaciones;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsAudito, "Audito");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsAudito;
        }

    }
}
