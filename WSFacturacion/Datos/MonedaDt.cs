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
    public class MonedaDt
    {
        #region MonedaDT: Clase de Datos de Moneda.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Monedas definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMoneda">int - Codigo de Moneda a filtrar</param>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas(Conexion oConn, int? codigoMoneda)
        {
            MonedaL oMonedas = new MonedaL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Monedas_GetMoneda";
                oCmd.Parameters.Add("@mon_moned", SqlDbType.TinyInt).Value = codigoMoneda;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oMonedas.Add(CargarMonedas(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oMonedas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Monedas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Monedas</param>
        /// <returns>Lista con el elemento Moneda de la base de datos</returns>
        /// ***********************************************************************************************
        private static Moneda CargarMonedas(System.Data.IDataReader oDR)
        {
            Moneda oMoneda = new Moneda((Int16)oDR["mon_moned"],
                                        oDR["mon_descr"].ToString(),
                                        oDR["mon_simbo"].ToString());

            return oMoneda;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Moneda en la base de datos
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addMonedas(Moneda oMoneda, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesVias_addMensajesVia";

                oCmd.Parameters.Add("mon_descr", SqlDbType.VarChar,30).Value = oMoneda.Desc_Moneda;
                oCmd.Parameters.Add("mon_simbo", SqlDbType.Char,3).Value = oMoneda.Simbolo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("Este Código de Moneda ya existe");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un mensaje de la Via en la base de datos
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updMonedas(Moneda oMoneda, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Monedas_updMoneda";

                oCmd.Parameters.Add("mon_moned", SqlDbType.SmallInt).Value = oMoneda.Codigo;
                oCmd.Parameters.Add("mon_descr", SqlDbType.VarChar,30).Value = oMoneda.Desc_Moneda;
                oCmd.Parameters.Add("mon_simbo", SqlDbType.Char,3).Value = oMoneda.Simbolo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro de la moneda");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Moneda de la base de datos
        /// </summary>
        /// <param name="Monedas">Int - Moneda a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delMoneda(int Moneda, Conexion oConn)
        {   
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Monedas_delMoneda";

                oCmd.Parameters.Add("@men_codig", SqlDbType.TinyInt).Value = Moneda;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro de la Moneda");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void delMoneda(Moneda oMoneda, Conexion oConn)
        {
            delMoneda(oMoneda.Codigo, oConn);
        }

        #endregion
    }
}
