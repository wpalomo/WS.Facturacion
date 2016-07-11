using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Utilitarios;
using Telectronica.Errores;


namespace Telectronica.Peaje
{
    public class CodAutorizacionElectronicaDt
    {

        ///********************************************************************************************
        /// <summary>
        /// Verifica que en el rango total de codcae, no exista ya una asignación hecha.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="contItems"></param>
        /// <param name="cantXVia"></param>
        /// <param name="inicioRango"></param>
        /// <param name="idCae"></param>
        /// <returns></returns>
        ///********************************************************************************************
        public static bool comprobarNuevoVCE(Conexion con, int? contItems, int? cantXVia, int inicioRango, int idCae, int? finalRango, int? identity, string comprobante)
        {
            bool valido = true;

            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Transaction = con.transaction;
                oCmd.Connection = con.conection;
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CAE_compRangoVCE";

                oCmd.Parameters.Add(new SqlParameter("@cantidad", SqlDbType.Int)).Value = contItems;
                oCmd.Parameters.Add(new SqlParameter("@codXvia", SqlDbType.Int)).Value = cantXVia;
                oCmd.Parameters.Add(new SqlParameter("@inicio", SqlDbType.Int)).Value = inicioRango;
                oCmd.Parameters.Add(new SqlParameter("@idCodCae", SqlDbType.Int)).Value = idCae;
                oCmd.Parameters.Add(new SqlParameter("@final", SqlDbType.Int)).Value = finalRango;
                oCmd.Parameters.Add(new SqlParameter("@ident", SqlDbType.Int)).Value = identity;
                oCmd.Parameters.Add(new SqlParameter("@comprobante", SqlDbType.Char, 3)).Value = comprobante;


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    valido = false;
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return valido;
        }

        ///********************************************************************************************
        /// <summary>
        ///  Sugiere el proximo numero de bloque segun el CODCAE seleccionado
        /// </summary>
        /// <param name="?"></param>
        /// <param name="id">codcae identitty</param>
        /// <returns>Numero Inicial Rango</returns>
        ///********************************************************************************************
        public static int getProximoBloque(Conexion con, int id)
        {
            int sugested;
            try
            {
                //SqlDataReader oDR;
                SqlCommand oCmd = new SqlCommand();
                oCmd.Transaction = con.transaction;
                oCmd.Connection = con.conection;

                SqlParameter param = new SqlParameter("@proxBloque", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CAE_proxBloqueVCE";
                oCmd.Parameters.Add(new SqlParameter("@ident", SqlDbType.Int)).Value = id;
                oCmd.Parameters.Add(param);
                //Parametro de retorno

                oCmd.ExecuteNonQuery();

                sugested = Convert.ToInt32(oCmd.Parameters["@proxBloque"].Value);

            }
            catch (Exception e)
            {

                throw e;
            }
            return sugested;
        }

        ///********************************************************************************************
        /// <summary>
        /// Obtiene el detalle de los CAE
        /// </summary>
        /// <param name="con">Conexion</param>
        /// <param name="id">Numero CAE</param>
        /// <returns></returns>
        ///********************************************************************************************
        public static CodAutorizacionElectronica getCodigoCaeDetalle(Conexion con, int id)
        {
            CodAutorizacionElectronica cod = new CodAutorizacionElectronica();
            try
            {
                SqlDataReader oDR;
                SqlCommand oCmd = new SqlCommand();
                oCmd.Transaction = con.transaction;
                oCmd.Connection = con.conection;

                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new SqlParameter("@ident", SqlDbType.Int)).Value = id;
                oCmd.CommandText = "Peaje.usp_CAE_getCODCAE";
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    cod = cargarCodCae(oDR);
                }
                oCmd = null;
                oDR.Close();

            }
            catch (Exception e)
            {

                throw e;
            }
            return cod;
        }


        ///********************************************************************************************
        /// <summary>
        /// Elimina CAE
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oCod">Codigo CAE</param>
        ///********************************************************************************************
        public static void delCodigoCae(Conexion oConn, CodAutorizacionElectronica oCod)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CAE_delCODCAE";
                oCmd.Parameters.Add("@ident", SqlDbType.Int).Value = oCod.Identity;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection
                    .ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                    {
                        msg = Traduccion.Traducir("El Codigo CAE ya ha sido asignado a una VIA, No puede Eliminarse.");
                    }
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                    {
                        msg = Traduccion.Traducir("El Nuevo rango provisto ya se encuentra en uso, seleccione un rango válido");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        ///***************************************************************************************
        /// <summary>
        /// Modifica Codigo CAE
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oCod">Codigo CAE</param>
        ///***************************************************************************************
        public static void updCodigoCae(Conexion oConn, CodAutorizacionElectronica oCod)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CAE_updCODCAE";
                oCmd.Parameters.Add("@codigo", SqlDbType.VarChar, 11).Value = oCod.Codigo;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = oCod.Tipo;
                if (oCod.Fecha.Year > 1)
                {
                    oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oCod.Fecha;
                }
                oCmd.Parameters.Add("@venc", SqlDbType.DateTime).Value = oCod.Vencimiento;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 2).Value = oCod.Serie;
                oCmd.Parameters.Add("@numin", SqlDbType.Int).Value = oCod.NumeroInicial;
                oCmd.Parameters.Add("@numfi", SqlDbType.Int).Value = oCod.NumeroFinal;
                oCmd.Parameters.Add("@comprobante", SqlDbType.Char, 3).Value = oCod.Comprobante;
                oCmd.Parameters.Add("@ident", SqlDbType.Int).Value = oCod.Identity;
                oCmd.Parameters.Add("@tipoReferencia", SqlDbType.Char, 1).Value = oCod.TipoReferencia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error ") + retval.ToString();
                    if (retval == -104)
                    {
                        msg = Traduccion.Traducir("El Codigo CAE ya ha sido asignado, No puede modificarse.");
                    }
                    else if (retval == -105)
                    {
                        msg = Traduccion.Traducir("El Nuevo rango provisto ya se encuentra en uso, seleccione un rango válido");
                    }

                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return;
        }


        ///*********************************************************************************************
        /// <summary>
        /// Setea nuevo codigo cae
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oCod">Codigo CAE</param>
        ///*********************************************************************************************
        public static void addCodigoCae(Conexion oConn, CodAutorizacionElectronica oCod)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CAE_setCODCAE";
                oCmd.Parameters.Add("@codigo", SqlDbType.VarChar, 11).Value = oCod.Codigo;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = oCod.Tipo;

                if (oCod.Fecha.Year > 1)
                {
                    oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oCod.Fecha;
                }

                oCmd.Parameters.Add("@venc", SqlDbType.DateTime).Value = oCod.Vencimiento;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 2).Value = oCod.Serie;
                oCmd.Parameters.Add("@numin", SqlDbType.Int).Value = oCod.NumeroInicial;
                oCmd.Parameters.Add("@numfi", SqlDbType.Int).Value = oCod.NumeroFinal;
                oCmd.Parameters.Add("@comprobante", SqlDbType.Char, 3).Value = oCod.Comprobante;
                oCmd.Parameters.Add("@tipoReferencia", SqlDbType.Char, 1).Value = oCod.TipoReferencia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Ya existe un Codigo CAE dentro del rango Provisto ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                    {
                        msg = Traduccion.Traducir("Ya existe un Codigo CAE dentro del rango Provisto ");
                    }
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                    {
                        msg = Traduccion.Traducir("Este Codigo fue eliminado");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return;
        }


        ///*************************************************************************************************************
        /// <summary>
        /// Lista de codigos cae para el DROPDOWN de la busqueda, trae lo codigos sin  repetirse
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        ///*************************************************************************************************************
        public static List<string> getCodigosCaeDiferente(Conexion oConn)
        {
            List<string> codigosL = new List<string>();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CAE_getCODCAEdiferente";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    codigosL.Add(Convert.ToString(oDR["cod_codig"]));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codigosL;
        }


        ///********************************************************************************************
        /// <summary>
        /// Obtiene los codigo de cae
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        ///********************************************************************************************       
        public static CodAutorizacionElectronicaL getCodigosCae(Conexion oConn)
        {
            CodAutorizacionElectronicaL codigosL = new CodAutorizacionElectronicaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CAE_getCODCAE";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    codigosL.Add(cargarCodCae(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codigosL;
        }




        ///**********************************************************************************
        /// <summary>
        /// Carga CODCAE
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        ///**********************************************************************************
        private static CodAutorizacionElectronica cargarCodCae(System.Data.IDataReader oDR)
        {
            CodAutorizacionElectronica codcae = new CodAutorizacionElectronica();
            try
            {
                codcae.Codigo = Convert.ToString(oDR["cod_codig"]);
                codcae.Fecha = Convert.ToDateTime(oDR["cod_fecha"]);
                codcae.NumeroFinal = Convert.ToInt32(oDR["cod_numfi"]);
                codcae.NumeroInicial = Convert.ToInt32(oDR["cod_numin"]);
                codcae.Serie = oDR["cod_serie"].ToString().Trim();
                codcae.Tipo = Convert.ToChar(oDR["cod_tipo"]);
                codcae.Vencimiento = Convert.ToDateTime(oDR["cod_venc"]);
                codcae.Identity = Convert.ToInt32(oDR["cod_ident"]);
                codcae.Comprobante = Convert.ToString(oDR["cod_tipdo"]);
                codcae.Descripcion = codcae.Comprobante + "-" + codcae.Serie + "-" + codcae.Codigo.ToString() + "-" + codcae.sTipo;

                if (oDR["cod_tipref"] != DBNull.Value)
                    codcae.TipoReferencia = Convert.ToChar(oDR["cod_tipref"]);
                else
                    codcae.TipoReferencia = null;




                return codcae;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
