using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Telectronica.Peaje
{
    public class VersionesViasDt
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las versiones de Vias
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">int - Estacion de las versiones de vias</param>
        /// /// <param name="numeroVia">int - Via de las versiones de vias</param>
        /// /// <param name="codigoTipoPrograma">string - Tipos de programas de las versiones de vias</param>
        /// <returns>Lista de Versiones de vias</returns>
        /// ***********************************************************************************************
        public static VersionViaL GetVersionesVias(Conexion oConn, int? numeroEstacion, int? numeroVia, string codigoTipoPrograma)
        {
            VersionViaL versionesVias = new VersionViaL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VersionesVias_GetVersiones";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = numeroEstacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = numeroVia;
                oCmd.Parameters.Add("@codig", SqlDbType.VarChar, 3).Value = codigoTipoPrograma;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    versionesVias.Add(CargarVersionVia(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return versionesVias;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los datos del ODR y Retorna el objeto version via
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static VersionVia CargarVersionVia(System.Data.IDataReader oDR)
        {
            VersionVia versionVia = new VersionVia();

            versionVia.Estacion = Convert.ToInt32(oDR["Estacion"]);
            versionVia.NombreEstacion = Convert.ToString(oDR["NombreEstacion"]);
            versionVia.Via = Convert.ToInt32(oDR["Via"]);
            versionVia.NumeroVersion = Convert.ToString(oDR["NumeroVersion"]);
            versionVia.FechaCambio = (DateTime)(oDR["FechaCambio"]);
            if (oDR["FechaModificacion"] != DBNull.Value)
                versionVia.FechaModificacion = (DateTime)(oDR["FechaModificacion"]);
            versionVia.CodigoTipoPrograma = Convert.ToString(oDR["CodigoTipoPrograma"]);
            versionVia.Descripcion = Convert.ToString(oDR["Descripcion"]);
            versionVia.Archivo = Convert.ToString(oDR["Archivo"]);
            versionVia.Ubicacion = Convert.ToString(oDR["Ubicacion"]);

            return versionVia;
        }

    }
}

