using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de los Medios de Pago del sistema
    /// </summary>
    ///****************************************************************************************************

    public static class MedioPagoDt
    {

        #region MEDIOPAGOCONFIGURACION: Clase de Datos de Configuracion de Medios de Pagos


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de medios de pagos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Configuracion de medios de pagos</returns>
        /// ***********************************************************************************************
        public static MedioPagoConfiguracionL getMedioPago(Conexion oConn, string TipoMedio, string TipoBoleto)
        {
            MedioPagoConfiguracionL oMedioPagoConfiguracion = new MedioPagoConfiguracionL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ConfiguracionMediosPagos_getConfiguracionMediosPagos";

                oCmd.Parameters.Add("@for_tipop", SqlDbType.Char, 1).Value = TipoMedio;
                oCmd.Parameters.Add("@for_tipbo ", SqlDbType.Char, 1).Value = TipoBoleto;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oMedioPagoConfiguracion.Add(CargarMediosPago(oDR));
                }

                //Si no hay CFGMED cargo un objeto vacio
                /*
                if (oMedioPagoConfiguracion.Count == 0)
                {
                    MedioPagoConfiguracion oMedio = new MedioPagoConfiguracion();

                    FormaPago oFormaPago = new FormaPago(TipoMedio,
                                                         TipoBoleto,
                                                         "");

                    oMedio.FormaDePago = oFormaPago;
                    oMedio.CostoMedioPago = 0;
                    oMedio.CostoReposicionMedioPago = 0;
                    oMedioPagoConfiguracion.Add(oMedio);

                }
                */

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oMedioPagoConfiguracion;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Formas de Configuracion de medios de pagos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de CFGMED</param>
        /// <returns>Lista con el elemento Configuracion de medios de pagos de la base de datos</returns>
        /// ***********************************************************************************************
        private static MedioPagoConfiguracion CargarMediosPago(System.Data.IDataReader oDR)
        {
            MedioPagoConfiguracion oMedioPagoConfiguracion = new MedioPagoConfiguracion();

            FormaPago oFormaPago = new FormaPago(oDR["for_tipop"].ToString(), 
                                                 oDR["for_tipbo"].ToString(),
                                                 oDR["for_descr"].ToString());

            oMedioPagoConfiguracion.FormaDePago = oFormaPago;
            oMedioPagoConfiguracion.CostoMedioPago = Convert.ToDouble(oDR["cfg_cotag"]);
            oMedioPagoConfiguracion.CostoReposicionMedioPago = Convert.ToDouble(oDR["cfg_corep"]); 

            return oMedioPagoConfiguracion;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Configuracion de medios de pagos en la base de datos
        /// </summary>
        /// <param name="oMedioPagoConfiguracion">Medios de pagos - Objeto con la informacion de la configuracion de Estacion a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updMedioPago(MedioPagoConfiguracion oMedioPagoConfiguracion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ConfiguracionMediosPagos_updConfiguracionMediosPago";

                oCmd.Parameters.Add("@cfg_tipop", SqlDbType.Char, 1).Value = oMedioPagoConfiguracion.FormaDePago.Tipo;
                oCmd.Parameters.Add("@cfg_tipbo", SqlDbType.Char, 1).Value = oMedioPagoConfiguracion.FormaDePago.SubTipo;
                oCmd.Parameters.Add("@cfg_cotag", SqlDbType.Money).Value = oMedioPagoConfiguracion.CostoMedioPago;
                oCmd.Parameters.Add("@cfg_corep", SqlDbType.Money).Value = oMedioPagoConfiguracion.CostoReposicionMedioPago;

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
                        msg = Traduccion.Traducir("No existe el registro de Configuración de Medios de Pago");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion

    }
}
