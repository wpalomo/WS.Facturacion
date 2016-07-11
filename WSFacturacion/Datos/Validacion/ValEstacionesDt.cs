using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data.SqlClient;
using System.Data;

namespace Telectronica.Validacion
{
    public class ValEstacionesDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve true si esta habilitada la categoria para una forma de pago
        /// </summary>
        /// ***********************************************************************************************
        public static bool getCategFormaPagoHabil(Conexion oConn, string tipop, string tipbo, int categoria)
        {
            bool habil = false;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ValEstaciones_getCategFormaPagoHabil";
                oCmd.Parameters.Add("@tipop", SqlDbType.Char, 1).Value = tipop;
                oCmd.Parameters.Add("@tipbo", SqlDbType.Char, 1).Value = tipbo;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = categoria;
                
                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    habil = true;
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return habil;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Codigos de Validacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Monedas</param>
        /// <returns>Lista con el elemento de la base de datos</returns>
        /// ***********************************************************************************************
        private static CodigoValidacion CargarCodigosValidacion(SqlDataReader oDR)
        {
            CodigoValidacion oCodigoValidacion = new CodigoValidacion(oDR["Tipo"].ToString(), Convert.ToInt16(oDR["Código"]), oDR["Descripción"].ToString(), null);
            return oCodigoValidacion;
        }

        public static CodigoValidacionL getCodigosAceptacionRechazo(Conexion oConn)
        {
            CodigoValidacionL codigosValidacion = new CodigoValidacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Estaciones_getCodigosAceptacionRechazo";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    codigosValidacion.Add(CargarCodigosValidacion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codigosValidacion;
        }
    }
}
