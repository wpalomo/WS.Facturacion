using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace Telectronica.Validacion
{
    public class EventoValidacionDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Contexto de una anomalia
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        
        /// ***********************************************************************************************
        public static DataSet getContextoAnomalia(Conexion oConn, int estacion, int via, DateTime fechaDesde, DateTime fechaHasta, string grupo)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "Contexto_AnomaliaDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_EventoValidacion_getContextoAnomalia";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Via", SqlDbType.TinyInt).Value = via;
                oCmd.Parameters.Add("@dteAnterior", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@dtePosterior", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@perfil", SqlDbType.TinyInt).Value = 0;
                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar,20).Value = grupo;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsAnomalias, "Contexto");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsAnomalias;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Contexto de una anomalia
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        

        /// ***********************************************************************************************
        public static int getBloquePorFechaParte(Conexion conn, Estacion estacion, ViaDefinicion via, DateTime fechaIni, DateTime fechaFin, ParteValidacion parte)
        {
            int retval = 0;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Eventos_getBloquePorFechaParte";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@Via", SqlDbType.TinyInt).Value = via.NumeroVia;
                oCmd.Parameters.Add("@fechaIni", SqlDbType.DateTime).Value = fechaIni;
                oCmd.Parameters.Add("@fechaFin", SqlDbType.DateTime).Value = fechaFin;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 3600;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Lista de Transitos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        

        /// ***********************************************************************************************
        public static DataSet getTransitos(Conexion oConn, int estacion, List<int> vias, int parte, DateTime fechaDesde, DateTime fechaHasta, int perfil)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "Transitos_AnomaliaDS";
            try
            {
                List<SqlDataRecord> listaVias = new List<SqlDataRecord>();

                SqlMetaData[] tvp_definition = { new SqlMetaData("via", SqlDbType.Int) };

                foreach (int item in vias)
                {
                    SqlDataRecord rec = new SqlDataRecord(tvp_definition);
                    rec.SetInt32(0, item);
                    listaVias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_EventoValidacion_getTransitos";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@vias", SqlDbType.Structured);
                oCmd.Parameters["@vias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@vias"].TypeName = "listaVias";
                oCmd.Parameters["@vias"].Value = listaVias;
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "Transitos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        /// <summary>
        /// Dataset de historia de rechazos de una placa o tag en particular.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="placa"></param>
        /// <param name="ntag"></param>
        /// <returns></returns>
        public static DataSet getHistoriaRechazos(Conexion con, string placa, DateTime fecha, string ntag, string emisor)
        {
            DataSet historia = new DataSet();
            historia.DataSetName = "Contexto_HistoriaDS";

            try
            {
                
                SqlCommand oCmd = new SqlCommand();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_OSAS_getHistoriaRechazos";
                oCmd.Transaction = con.transaction;
                oCmd.Connection = con.conection;
                oCmd.CommandTimeout = 3600;
                oCmd.Parameters.Add(new SqlParameter("paten",SqlDbType.VarChar,20)).Value = placa;
                oCmd.Parameters.Add(new SqlParameter("ntag", SqlDbType.VarChar, 30)).Value = ntag;
                oCmd.Parameters.Add(new SqlParameter("fecha",SqlDbType.DateTime)).Value = fecha;
                oCmd.Parameters.Add(new SqlParameter("emisor", SqlDbType.VarChar, 5)).Value = emisor;

                SqlDataAdapter Sqa = new SqlDataAdapter(oCmd);
                Sqa.Fill(historia,"Historia" );

                oCmd = null;
                Sqa.Dispose();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

            return historia;

        }
    }
}
