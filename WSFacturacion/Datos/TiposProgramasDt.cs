using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Telectronica.Peaje
{
    public class TiposProgramasDt
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tipos de programas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoTipoPrograma">string - Codigo del tipo de programa, NULL para todos</param>
        /// <returns>Lista de Tipos de Programas</returns>
        /// ***********************************************************************************************
        public static TipoProgramaL GetTiposProgramas(Conexion oConn, string codigoTipoPrograma)
        {
            TipoProgramaL tiposProgramas = new TipoProgramaL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_TiposProgramas_GetTiposProgramas";
                oCmd.Parameters.Add("@codig", SqlDbType.VarChar, 3).Value = codigoTipoPrograma;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    tiposProgramas.Add(CargarTipoPrograma(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tiposProgramas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los datos del ODR y Retorna el objeto tipo de programa
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static TipoPrograma CargarTipoPrograma(System.Data.IDataReader oDR)
        {
            TipoPrograma tipoPrograma = new TipoPrograma();

            tipoPrograma.CodigoTipoPrograma = Convert.ToString(oDR["CodigoTipoPrograma"]);
            tipoPrograma.Descripcion = Convert.ToString(oDR["Descripcion"]);
            tipoPrograma.Archivo = Convert.ToString(oDR["Archivo"]);
            tipoPrograma.Ubicacion = Convert.ToString(oDR["Ubicacion"]);

            return tipoPrograma;
        }

    }
}

