using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Utilitarios;
using System.Web;

namespace Telectronica.Peaje
{
    public class EventoDt
    {
        #region CLAVEEVENTO

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Codigos de Evento
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigo">int? - Codigo del evento para la busqueda</param>
        /// <param name="estacion">int? - Estacion donde esta habilitado</param>
        /// <param name="usuario">string - usuario que tiene el codigo habilitado</param>
        /// <returns>Lista de Codigos de Evento</returns>
        /// ***********************************************************************************************
        public static ClaveEventoL getClavesEvento(Conexion oConn, int? codigo, int? estacion, string usuario)
        {
            ClaveEventoL oClaves = new ClaveEventoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Evento_getClaeve";
            oCmd.Parameters.Add("@codev", SqlDbType.SmallInt).Value = codigo;
            oCmd.Parameters.Add("@coest", SqlDbType.SmallInt).Value = estacion;
            oCmd.Parameters.Add("@usuario", SqlDbType.VarChar,10).Value = usuario;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oClaves.Add(CargarClaeve(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oClaves;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto ClaveEvento
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Codigos de evento</param>
        /// <returns>Objeto ClaveEvento con los datos</returns>
        /// ***********************************************************************************************
        private static ClaveEvento CargarClaeve(System.Data.IDataReader oDR)
        {
            ClaveEvento oClave = new ClaveEvento();
            oClave.Codigo = (Int16) oDR["cla_codev"];
            oClave.Descripcion = oDR["cla_descr"].ToString();
            oClave.Tipo = oDR["cla_tipom"].ToString();
            oClave.TipoEvento = new TipoEvento();
            oClave.TipoEvento.Tipo = oDR["cla_tipom"].ToString();
            oClave.TipoEvento.Descripcion = oDR["tcl_descr"].ToString();
            return oClave;
        }

        #endregion

        #region TipoEvento

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de Evento
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xTipo">string? - Tipo del evento para la busqueda</param>
        /// <returns>Lista de Tipos de Evento</returns>
        /// ***********************************************************************************************
        public static TipoEventoL getTiposEvento(Conexion oConn, string xTipo, int? xiCodEst)
        {
            TipoEventoL oTiposEvento = new TipoEventoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Evento_GetClaeve_Tipos";

            if (xTipo == "")
            {
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 10).Value = null;
            }
            else
            {
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 10).Value = xTipo;
            }

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiCodEst;
            oCmd.Parameters.Add("@usuario", SqlDbType.VarChar, 10).Value = (string)HttpContext.Current.Session["Permisos_Usuario"];

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTiposEvento.Add(CargarTipoEvento(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTiposEvento;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto TipoEvento
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Tipo de Evento</param>
        /// <returns>Objeto TipoEvento con los datos</returns>
        /// ***********************************************************************************************
        private static TipoEvento CargarTipoEvento(System.Data.IDataReader oDR)
        {
            TipoEvento otipoEvento = new TipoEvento();
            otipoEvento.Tipo = oDR["cla_tipom"].ToString();
            otipoEvento.Descripcion = oDR["tcl_descr"].ToString();

            return otipoEvento;
        }

        #endregion
    }
}
