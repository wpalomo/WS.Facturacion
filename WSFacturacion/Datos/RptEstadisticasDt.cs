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
    /// Clase que trae los datos para Reportes de Estadisticas de Transito
    /// </summary>
    /// ***********************************************************************************************
    public class RptEstadisticasDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Sentido y Porcentaje
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstSentidoPorcentajes(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta, 
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP,string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_SentidoPorcentajesDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstSentidoPorcentajes";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }

        /// <summary>
        /// Estadistico 915MHZ para presentar a sem parar
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="fechaHoraDesde">desde</param>
        /// <param name="fechaHoraHasta">hasta</param>
        /// <param name="estacion">Estacion</param>
        /// <returns>Dataset</returns>
        public static DataSet getEstadistico915(Conexion oConn, DateTime fechaHoraDesde, DateTime fechaHoraHasta, int? estacion, int? admtag)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_Tag915";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_915";
            
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = fechaHoraDesde;
            oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = fechaHoraHasta;
            oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = admtag;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Datos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Sentido y Forma dePago
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstSentidoFPago(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac,string sIncluirFP,string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_SentidoFPagoDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstSentidoFPago";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por Sentido y Categoria
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstSentidoCategorias(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria,string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_SentidoCategoriasDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstSentidoCategorias";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Volumen Diario
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstVolumenDiario(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_VolumenDiarioDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstVolumenDiario";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica flujo Diario
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstFlujoDiario(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_FlujoDiarioDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstFlujoDiario";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica flujo Diario
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstIngresosPlaza(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_IngresosPlazaDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstIngresosPlaza";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Ingresos Mensuales
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstIngresosMensuales(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria, string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_IngresosMensualesDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstIngresosMensuales";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por categoria y Vias
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sModoMantenim">string - Indica si se agregan o no los partes en modo mantenimiento</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstCategoriaVias(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sModoMantenim, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria,string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_CategoriaViasDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstCategoriaVias";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@modoMante", SqlDbType.Char, 1).Value = sModoMantenim;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Espacial
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// <param name="sCategoria"></param>
        /// ***********************************************************************************************
        public static DataSet getEstEspacial(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria,string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_EspacialDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstEspacial";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica Temporal
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstTemporal(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP,string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_TemporalDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstTemporal";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType.SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por categoria y FPago
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="agrupacion">string - V via
        ///                                  E estación
        ///                                  Z Zona
        ///                                  null concesión</param>
        /// <param name="fragmentacion">int - Minutos de fragmentacion</param>
        /// <param name="sIncluirExentos"></param>
        /// <param name="sIncluirViolac"></param>
        /// ***********************************************************************************************
        public static DataSet getEstCategoriaFPago(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, int? via, string agrupacion, int fragmentacion, Int16 HoraCorte, string sIncluirExentos, string sIncluirViolac, string sIncluirFP, string sCategoria,string sModo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_CategoriaFPagoDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstCategoriaFPago";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@Agrupacion", SqlDbType.Char, 1).Value = agrupacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@wfragm", SqlDbType. SmallInt).Value = fragmentacion;
            oCmd.Parameters.Add("@horacorte", SqlDbType.TinyInt).Value = HoraCorte;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.VarChar, 1).Value = sIncluirExentos;
            oCmd.Parameters.Add("@IncluirViolac", SqlDbType.VarChar, 1).Value = sIncluirViolac;
            oCmd.Parameters.Add("@IncluirForpag", SqlDbType.VarChar, 1).Value = sIncluirFP;
            oCmd.Parameters.Add("@TipoCategoria", SqlDbType.VarChar, 1).Value = sCategoria;
            oCmd.Parameters.Add("@Modo", SqlDbType.Char, 1).Value = sModo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();
            

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica de Tabuladas y Detectadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="operador">string - Operador</param>
        /// <param name="formaDePago">FormaPago - formaDePago</param>
        /// <param name="string">string - sTipoDiscrepancia</param>
        /// ***********************************************************************************************
        public static DataSet getTabuladasDetectadas(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? estacion, int? via, string operador, FormaPago formaDePago, string sTipoDiscrepancia)
        {
            DataSet dsResultados = new DataSet();
            dsResultados.DataSetName = "RptEstadisticas_TabuladasDetectadasDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getTabuladasDetectadas";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@opeid", SqlDbType.VarChar, 10).Value = operador;
            oCmd.Parameters.Add("@tipop", SqlDbType.Char, 1).Value = formaDePago.Tipo;
            oCmd.Parameters.Add("@tipbo", SqlDbType.Char, 1).Value = formaDePago.SubTipo;
            oCmd.Parameters.Add("@tdisc", SqlDbType.Char, 1).Value = sTipoDiscrepancia;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsResultados, "TabuladasDetectadas");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsResultados;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la estadistica por categoria y equivalentes
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="fragmentacion">string - Dia o Mes</param>
        /// <param name="sIncluirExentos">string - S o N</param>
        /// ***********************************************************************************************
        public static DataSet getEstCategoriaEquivalente(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? zona, int? estacion, string fragmentacion, string sIncluirExentos)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_CategoriaEquivalenteDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEstCategoriaEquivalente";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@wfragm", SqlDbType.Char,1).Value = fragmentacion;
            oCmd.Parameters.Add("@IncluirExentos", SqlDbType.Char, 1).Value = sIncluirExentos;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }



        public static DateTime getHoraCorte(Conexion oConn, int? zona, int? estacion)
        {
            DateTime HoraCorte = new DateTime();
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_HoraCorteDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getHoraCorte";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "HoraCorte");

            if (dsPartes.Tables[0].Rows.Count > 0)
            {
                HoraCorte = Convert.ToDateTime(dsPartes.Tables[0].Rows[0][0]);
            }
            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return HoraCorte;
        }



        ///********************************************************************************************************
        /// <summary>
        /// Saca el detalle de los transitos con ejes suspensos
        /// </summary>
        /// <param name="oConn">Conexion</param>
        /// <param name="fechahoraDesde">Fecha de Inicio</param>
        /// <param name="fechahoraHasta">Fecha de Finalizacion</param>
        /// <param name="estacion">Numero de estacion</param>
        /// <param name="via">Numero de via</param>
        /// <returns>DataSet - Resultados</returns>
        ///********************************************************************************************************
        public static DataSet getEstadDetalladaEjesSuspensos(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? estacion, int? via, string modo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_EjesSuspensosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getDetalleEjesLevantados";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@modo", SqlDbType.Char,1).Value = modo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }


        ///********************************************************************************************************
        /// <summary>
        /// Saca el estadistico de ejes suspensos
        /// </summary>
        /// <param name="oConn">Conexion</param>
        /// <param name="fechahoraDesde">Fecha de Inicio</param>
        /// <param name="fechahoraHasta">Fecha de Finalizacion</param>
        /// <param name="estacion">Numero de estacion</param>
        /// <param name="via">Numero de via</param>
        /// <returns>DataSet - Resultados</returns>
        ///********************************************************************************************************
        public static DataSet getEstadisticoEjesSuspensos(Conexion oConn, DateTime fechahoraDesde, DateTime fechahoraHasta,
                int? estacion, int? via, string modo)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_EjesSuspensosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptEstadisticas_getEjesLevantados";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@modo", SqlDbType.Char, 1).Value = modo;

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Transitos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }


        public static DataSet getEstadisticoExentos(Conexion oConn, DateTime FechaDesde, DateTime FechaHasta,
            int? Estacion, int? via, int cliente, string placa, bool detallado,bool porEstacion)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_ExentosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptExentos_getEstadisticaClientes";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;
            oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = FechaDesde;
            oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = FechaHasta;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            if (cliente != 0)
            {
                oCmd.Parameters.Add("@client", SqlDbType.Int).Value = cliente;
            }
            oCmd.Parameters.Add("@placa", SqlDbType.VarChar, 10).Value = placa;
            if (detallado){oCmd.Parameters.Add("@detallado", SqlDbType.Char, 1).Value = 'S';}
            if (porEstacion) { oCmd.Parameters.Add("@PorEstacion", SqlDbType.Char, 1).Value = 'S'; }

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Exentos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }


        public static DataSet ANNT_getEstCaegoriaDia(Conexion oConn,DateTime desde, DateTime hasta, int? estacion, string sIncluyeExentos, string sIncluirViolac, string sIncluirFP)
        {
            DataSet dsPartes = new DataSet();
            dsPartes.DataSetName = "RptEstadisticas_Dataset";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.Reportes_ANTT_FluxoDiario";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
            oCmd.Parameters.Add("@incluirExentos", SqlDbType.Char,1).Value = sIncluyeExentos;
            oCmd.Parameters.Add("@incluirViolac", SqlDbType.Char, 1).Value = sIncluirViolac;
            //oCmd.Parameters.Add("@incluirViolac", SqlDbType.Char, 1).Value = sIncluirFP;

            

            oCmd.CommandTimeout = Utilitarios.Util.getSpTimeOut();

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPartes, "Registros");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPartes;
        }
    }



}
