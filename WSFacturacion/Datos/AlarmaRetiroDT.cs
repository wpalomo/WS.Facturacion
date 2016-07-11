using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class AlarmaRetiroDT
    {


        #region AlarmaRetiroDT ALARMARETIRODT: Clase de Datos de Alarmas de Retiro.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Alarmas de retiro vigente.
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Alarmas</returns>
        /// ***********************************************************************************************
        public static AlarmaRetiroL getAlarmasRetiro(Conexion oConn)
        {
            AlarmaRetiroL oAlarmas = new AlarmaRetiroL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Retiros_GetAlarmasRetiros";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oAlarmas.Add(CargarAlarmasRetiro(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAlarmas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de alarmas de retiros
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Cantidad de alarmas de retiro </returns>
        /// ***********************************************************************************************
        public static int getHayAlarmasRetiro(Conexion oConn)
        {
            int retorno = 0;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Retiros_GetAlarmasRetiros";

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                retorno = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retorno;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Alarmas de retiro
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Alarmas de retiro</param>
        /// <returns>Lista con el elemento Alarmas de retiro de la base de datos</returns>
        /// ***********************************************************************************************
        private static AlarmaRetiro CargarAlarmasRetiro(System.Data.IDataReader oDR)
        {
            AlarmaRetiro oAlarmaRetiro = new AlarmaRetiro(Convert.ToInt16(oDR["estacion"]),
                                                            Convert.ToInt16(oDR["via"]), 
                                                            Convert.ToInt32(oDR["parte"]), 
                                                            Convert.ToDateTime(oDR["jornada"]),
                                                            Convert.ToInt16(oDR["turno"]),
                                                            Convert.ToString(oDR["id"]),
                                                            Convert.ToString(oDR["peajista"]),
                                                            Convert.ToDecimal(oDR["recaudoNeto"]),
                                                            Convert.ToDecimal(oDR["minimo"]));

            return oAlarmaRetiro;
        }



        #endregion




    }
}
