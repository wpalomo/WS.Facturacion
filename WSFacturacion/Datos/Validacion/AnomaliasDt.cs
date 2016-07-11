using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class AnomaliasDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de formas de pago
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static FormaPagoValidacionL getForpagAConsolidar(Conexion oConn, int? codAnomalia)
        {
            FormaPagoValidacionL FormaPago = new FormaPagoValidacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.USP_Anomalias_getForpagAConsolidar";
                oCmd.Parameters.Add("@anomalia", SqlDbType.Int).Value = codAnomalia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    FormaPago.Add(CargarFormaPago(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FormaPago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de codigos validacion forma de pago
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla</param>
        /// <returns>el elemento de la base de datos</returns>
        /// ***********************************************************************************************
        private static FormaPagoValidacion CargarFormaPago(SqlDataReader oDR)
        {
            FormaPagoValidacion formaPago = new FormaPagoValidacion();
            formaPago.CodAnomalia = Convert.ToInt16(oDR["fvr_anomal"]);
            formaPago.MedioPago = oDR["fvr_tipop"].ToString();
            formaPago.FormaPago = oDR["fvr_tipbo"].ToString();
            formaPago.Descripcion = oDR["for_descr"].ToString();
            formaPago.TipoTarifa = new TarifaDiferenciada { CodigoTarifa = oDR["fvr_titar"] != DBNull.Value ? Convert.ToInt32(oDR["fvr_titar"]) : 0 };

            return formaPago;
        }
    }
}
