using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Telectronica.Tesoreria;

namespace Telectronica.Tesoreria
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Recaudacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptRecaudacionDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la lista de partes con su recaudacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>

        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
                /// <param name="operador">string - id de Operador</param>
        /// <param name="turno">int? - turno</param>
        /// <param name="tipoParte">string - N sin liquidar
        ///                                  S liquidado
        ///                                  V validado
        ///                                  null todos</param>
        /// ***********************************************************************************************
        public static DataSet getPartes(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta, int? zona, int? estacion, string operador, int? turno, string tipoParte, string sModoParte)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptRecaudacion_PartesDS";
            getPartesDS(oConn, dsPartes, "Partes", null, jornadaDesde, jornadaHasta, zona, estacion, "PARTE", turno, turno, true, true, operador, tipoParte, sModoParte);

            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// lista de partes con su recaudacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se le agregan los datos</param>
        /// <param name="nombreTabla">string - nombre del DataTable a agregar</param>
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
        /// <param name="operador">string - id de Operador</param>
        /// <param name="tipoParte">string - N sin liquidar
        ///                                  S liquidado
        ///                                  V validado
        ///                                  null todos</param>
        /// ***********************************************************************************************
        public static void getPartesDS(Conexion oConn, DataSet dsPartes, string nombreTabla, int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion, string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string operador, string tipoParte)
        {
            getPartesDS(oConn, dsPartes, nombreTabla, parte, jornadaDesde, jornadaHasta, zona, estacion, tipoAgrupacion, turnoDesde, turnoHasta, porEstacion, porJornada, operador, tipoParte, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// lista de partes con su recaudacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se le agregan los datos</param>
        /// <param name="nombreTabla">string - nombre del DataTable a agregar</param>
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
        /// <param name="operador">string - id de Operador</param>
        /// <param name="tipoParte">string - N sin liquidar
        ///                                  S liquidado
        ///                                  V validado
        ///                                  null todos</param>
        /// <param name="sModoParte">string -T Todos
        ///                                  S Sin Modo Mantenimiento
        ///                                  C Con Modo Mantenimiento</param>
        /// ***********************************************************************************************
        public static void getPartesDS(Conexion oConn, DataSet dsPartes, string nombreTabla, int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion, string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string operador, string tipoParte, string sModoParte)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Partes";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.VarChar, 10).Value = tipoAgrupacion;
            oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
            oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char, 1).Value = porEstacion ? "S" : "N";
            oCmd.Parameters.Add("@PorJornada", SqlDbType.Char, 1).Value = porJornada ? "S" : "N";
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = operador;
            oCmd.Parameters.Add("@TipoParte", SqlDbType.Char, 1).Value = tipoParte;
            oCmd.Parameters.Add("@modoParte", SqlDbType.Char, 1).Value = sModoParte;                

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, nombreTabla);

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        public static DataSet getMDA(Conexion oConn,DateTime jornadaDesde, DateTime jornadaHasta, int? estacion, int? turnoDesde, int? turnoHasta, string tipoParte, string sModoParte, bool xEstacion)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptMDA_PartesDS";
            getMdaDS(oConn, dsPartes, "MDA",jornadaDesde, jornadaHasta,  estacion, turnoDesde, turnoHasta, tipoParte, sModoParte,xEstacion);

            return dsPartes;
        }

		public static DataSet getMDAContable(Conexion oConn, DateTime jornadaDesde, DateTime jornadaHasta, int? estacion, string tipoParte,bool xEstacion)
		{
			DataSet dsPartes = new DataSet();
			dsPartes.DataSetName = "RptMDA_PartesDS";
			getMdaContableDS(oConn, dsPartes, "MDA", jornadaDesde, jornadaHasta, estacion, tipoParte,xEstacion);

			return dsPartes;
		}

		public static void getMdaContableDS(Conexion oConn, DataSet dsPartes, string nombreTabla, DateTime? jornadaDesde, DateTime? jornadaHasta, int? estacion, string tipoParte, bool xEstacion)
		{
			// Creamos, cargamos y ejecutamos el comando
			SqlCommand oCmd = new SqlCommand();
			oCmd.Connection = oConn.conection;
			oCmd.Transaction = oConn.transaction;

			oCmd.CommandType = System.Data.CommandType.StoredProcedure;
			oCmd.CommandText = "Peaje.usp_RptGeneralMDA";

			oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
			oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = jornadaDesde;
			oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = jornadaHasta;
			oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char, 1).Value = xEstacion ? "S" : "N";



			oCmd.CommandTimeout = 3000;

			SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
			oDA.Fill(dsPartes, nombreTabla);

			// Cerramos el objeto
			oCmd = null;
			oDA.Dispose();
		}


        public static void getMdaDS(Conexion oConn, DataSet dsPartes,string nombreTabla ,DateTime? jornadaDesde, DateTime? jornadaHasta, int? estacion, int? turnoDesde, int? turnoHasta, string tipoParte, string sModoParte,bool xEstacion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptRelatorioMDA";
            
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = jornadaDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = jornadaHasta;
            oCmd.Parameters.Add("@Turini", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@Turfin", SqlDbType.TinyInt).Value = turnoHasta;
            oCmd.Parameters.Add("@diames", SqlDbType.Char, 1).Value = sModoParte;
            oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char,1).Value = xEstacion ? "S":"N";

            

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, nombreTabla);

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }
    }
}
