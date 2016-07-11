using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Validacion;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de las Formas de Pago definidas en el sistema
    /// </summary>
    ///****************************************************************************************************

    public static class FormaPagoDt
    {

        #region FORMAPAGO: Clase de Datos de las Formas de Pago


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formas de Pago definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Tipo">string - Forma de pago (o tipo) por la cual filtrar</param>
        /// <param name="SubTipo">string - Subforma de pago (o subtipo) por la cual filtrar</param>
        /// <returns>Lista de Formas de Pago</returns>
        /// ***********************************************************************************************
        public static FormaPagoL getFormasPago(Conexion oConn,
                                               string Tipo,
                                               string SubTipo)
        {
            FormaPagoL oFormasPago = new FormaPagoL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FormasPago_getFormasPago";
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 1).Value = Tipo;
                oCmd.Parameters.Add("@SubTipo", SqlDbType.Char, 1).Value = SubTipo;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oFormasPago.Add(CargarFormaPago(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oFormasPago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formas de Pago definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Tipo">string - Forma de pago (o tipo) por la cual filtrar</param>
        /// <param name="SubTipo">string - Subforma de pago (o subtipo) por la cual filtrar</param>
        /// <returns>Lista de Formas de Pago</returns>
        /// ***********************************************************************************************
        public static CausaSupervisionL getCodigosAceptacionSupervicionRemota(Conexion oConn)
        {
            CausaSupervisionL oAnomalias = new CausaSupervisionL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_getCodAceptacionDefectoForpag";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oAnomalias.Add(CargarAnomalia(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAnomalias;
        }

        private static CausaSupervision CargarAnomalia(System.Data.IDataReader oDR)
        {
            CausaSupervision oCausa = new CausaSupervision();
            oCausa.Codigo = Convert.ToByte(oDR["cod_codig"]);
            oCausa.Descripcion = oDR["cod_descr"].ToString();
            oCausa.Anomal = Convert.ToByte(oDR["cod_anomal"]);

            return oCausa;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Formas de Pago
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Formas de Pago</param>
        /// <returns>Lista con el elemento FormaPago de la base de datos</returns>
        /// ***********************************************************************************************
        private static FormaPago CargarFormaPago(System.Data.IDataReader oDR)
        {
            FormaPago oFormaPago = new FormaPago();
            oFormaPago.Tipo = oDR["for_tipop"].ToString();
            oFormaPago.SubTipo = oDR["for_tipbo"].ToString();
            oFormaPago.Descripcion = oDR["for_descr"].ToString();
            oFormaPago.Iniciales = oDR["for_inici"].ToString();
            oFormaPago.NombreCorto = oDR["for_corto"].ToString();
            oFormaPago.Automatica = oDR["for_autom"].ToString();
            oFormaPago.CobraVia = oDR["for_cobra"].ToString();


            return oFormaPago;
        }


        #endregion

        #region TIPOPAGO: Clase de Datos de los Tipos de Pago


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de Pago asociados a Tag o Chip
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Tipos de Pago</returns>
        /// ***********************************************************************************************
        public static TipoPagoL getTiposAutomaticos(Conexion oConn)
        {
            TipoPagoL oTipoPago = new TipoPagoL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FormasPago_getTiposAutomaticos";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTipoPago.Add(CargarTipoPago(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTipoPago;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Tipos de Pago
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Formas de Pago</param>
        /// <returns>elemento TipoPago de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoPago CargarTipoPago(System.Data.IDataReader oDR)
        {
            TipoPago oTipoPago = new TipoPago();
            oTipoPago.Codigo = oDR["tic_tipbo"].ToString();
            oTipoPago.Descripcion = oDR["tic_descr"].ToString();


            return oTipoPago;
        }


        #endregion

    }
}
