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
    /// Clase que trae los datos para Reportes de Detalle de Recaudacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptDetalleParteJornadaDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de los partes agrupados por parte, bloque o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
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
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static DataSet getPartesAgrupados(Conexion oConn, 
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptRecaudacion_PartesDetalleDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_PartesAgrupados";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.VarChar, 10).Value = tipoAgrupacion;
            oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
            oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char, 1).Value = porEstacion?"S":"N";
            oCmd.Parameters.Add("@PorJornada", SqlDbType.Char, 1).Value = porJornada ? "S" : "N";
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Partes");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de los transitos de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static void getTransitos(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Transitos";
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
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }
                
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de los transitos de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// ***********************************************************************************************
        public static void getFacturacionPV(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Facturacion_getTotalFacturasPorPV";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value =  jornadaDesde;

            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.VarChar, 10).Value = tipoAgrupacion;
            oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
            oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char, 1).Value = porEstacion ? "S" : "N";
            oCmd.Parameters.Add("@PorJornada", SqlDbType.Char, 1).Value = porJornada ? "S" : "N";

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "FacturasPorPV");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }
       
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de los tickets faltantes
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>       
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>     
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// ***********************************************************************************************
        public static void getTicketsFaltantes(Conexion oConn, DataSet dsPartes,
            int? parte, int? estacion, DateTime? jornadaDesde, DateTime? jornadaHasta,
            int? zona, int? turnoDesde, int? turnoHasta)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "usp_CrtlValidacion_GetTicketsFaltantes";                
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornadaDesde;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;  
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
            oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "TicketsFaltantes");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de los tickets duplicados
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>       
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>     
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// ***********************************************************************************************
        public static void getTicketsDuplicados(Conexion oConn, DataSet dsPartes,
            int? parte, int? estacion, DateTime? jornadaDesde, DateTime? jornadaHasta,
            int? zona, int? turnoDesde, int? turnoHasta)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "usp_CrtlValidacion_GetTicketsDuplicados";
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornadaDesde;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
            oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "TicketsDuplicados");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de la Recaudacion por categoria y FP de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static void getRecaudado(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Recaudado";
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
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Recaudado");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de la Recaudacion por categoria y FP de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static void getResumenTotal(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_ResumenTotal";
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
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "ResumenTotal");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de las tabuladas y detectadas de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static void getTabuladasDetectadas(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_TabuladasDetectadas";
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
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "TabuladasDetectadas");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de las anomalias de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static void getAnomalias(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Anomalias";
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
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Anomalias");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de los bloques de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static void getBloques(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Bloques";
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
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Bloques");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de las liquidaciones por denominacion de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// ***********************************************************************************************
        public static void getDenominaciones(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Denominaciones";
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

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Denominaciones");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los retiros de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="numeroMovimiento">int? - Numero de Movimiento</param>
        /// <param name="desde">DateTime? - Jornada Desde</param>
        /// <param name="hasta">DateTime? - Jornada Hasta</param>
        /// <param name="operador">string - Operador</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// ***********************************************************************************************
        public static void getRetiros(Conexion oConn, DataSet dsLiquidacion,
            int? estacion, int? parte, int? numeroMovimiento, DateTime? desde, DateTime? hasta, string operador,
            int? turnoDesde, int? turnoHasta)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetRetiros";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numeroMovimiento;
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = desde;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = hasta;
            oCmd.Parameters.Add("@opeid", SqlDbType.VarChar, 10).Value = operador;
            oCmd.Parameters.Add("@TurnoDesde", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoHasta", SqlDbType.TinyInt).Value = turnoHasta;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsLiquidacion, "Retiros");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de las liquidaciones por bolsa y moneda de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// ***********************************************************************************************
        public static void getBolsas(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Bolsas";
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

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Bolsas");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de los totales por moneda, cotizacion y tipo de movimiento de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// ***********************************************************************************************
        public static void getMonedas(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Monedas";
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

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Monedas");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de los totales de ventasde un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// <param name="sSentido">string - Sentido de Circulación</param>
        /// ***********************************************************************************************
        public static void getVentas(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada, string sSentido)
         {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_Ventas";
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
            oCmd.Parameters.Add("@Senti", SqlDbType.VarChar, 24).Value = sSentido;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Ventas");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de las anomalias e incidencias de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomaliasEventos(Conexion oConn, int estacion, int parte)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptRecaudacion_AnomaliasEventosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_AnomaliasEventos";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Anomalias");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de los cambios de estado de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// ***********************************************************************************************
        public static DataSet getCambiosEstado(Conexion oConn, int estacion, int parte)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptRecaudacion_CambiosEstadoDS";
            
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_CambiosEstado";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;

            oCmd.CommandTimeout = 300;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Anomalias");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a un DataSet los datos de las liquidaciones por movimiento de cupones de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="dsPartes">DataSet - DataSet al que se agregan los datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="tipoAgrupacion">string - JORNADA Agrupa por toda la jornada
        ///                                       PARTE Agrupa por cada parte
        ///                                       BLOQUE Agrupa por el bloque
        ///                                  </param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="porEstacion">bool - detallado por Estacion</param>
        /// <param name="porJornada">bool - detallado por Jornada</param>
        /// ***********************************************************************************************
        public static void getCuponLiquidacion(Conexion oConn, DataSet dsPartes,
            int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion,
            string tipoAgrupacion, int? turnoDesde, int? turnoHasta, bool porEstacion, bool porJornada)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_RptLiquidacion_Cupones";
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

            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Cupones");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }
    }
}
