using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Validacion
{
    public class AutorizacionPasoDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo autorizacion paso
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte, bool puedeVerValInvisible)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "AutorizacionPaso_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_AutorizacionPaso_GetAnomalias";
                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@VerInvisible", SqlDbType.Char).Value = puedeVerValInvisible ? "S" : "N";

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsAnomalias, "Anomalias");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsAnomalias;
        }

        /// <summary>
        /// Guarda los cambios en la anomalia
        /// </summary>
        /// <param name="oConn">Conexion</param>
        /// <param name="codAnomalia">tipo anomalia</param>
        /// <param name="estacion">estacion</param>
        /// <param name="parte">parte</param>
        /// <param name="jornada">jornada</param>
        /// <param name="anomalias">anomalias a guardar</param>
        /// <param name="validador">validador de las anomalias</param>
        public static void setAnomaliasValidadas(Conexion oConn, int codAnomalia, int estacion, int parte, DateTime jornada, AnomaliaValidacionL anomalias, string validador)
        {
            try
            {
                List<SqlDataRecord> tablaAnomalias = new List<SqlDataRecord>();

                SqlMetaData[] tvp_definition = { new SqlMetaData("Validada", SqlDbType.Char, 1),
                                                 new SqlMetaData("Estado", SqlDbType.Char, 1),
                                                 new SqlMetaData("CodigoAceptRechazo", SqlDbType.TinyInt),
                                                 new SqlMetaData("Via", SqlDbType.TinyInt),
                                                 new SqlMetaData("Bloque", SqlDbType.Int),
                                                 new SqlMetaData("Fecha", SqlDbType.DateTime),
                                                 new SqlMetaData("Validador", SqlDbType.VarChar, 10), 
                                                 new SqlMetaData("dis_tipop", SqlDbType.Char, 1),
                                                 new SqlMetaData("dis_tipbo", SqlDbType.Char, 1),
                                                 new SqlMetaData("dis_subfp", SqlDbType.TinyInt),
                                                 new SqlMetaData("TipoTarifa", SqlDbType.Int),
                                                 new SqlMetaData("MontoConsolidado", SqlDbType.SmallMoney),
                                                 new SqlMetaData("CategoriaTabulada", SqlDbType.TinyInt),
                                                 new SqlMetaData("CategoriaConsolidada", SqlDbType.TinyInt),
                                                 new SqlMetaData("ObservacionPeajista", SqlDbType.VarChar, 255), 
                                                 new SqlMetaData("ObservacionAnomalia", SqlDbType.VarChar, 255), 
                                                 new SqlMetaData("ObservacionInterna", SqlDbType.VarChar, 4000), 
                                                 new SqlMetaData("ObservacionExterna", SqlDbType.VarChar, 4000),  
   								                 new SqlMetaData("AnomaliaVista", SqlDbType.Char, 1),
                                                 new SqlMetaData("MontoDiferencia", SqlDbType.SmallMoney),
                                                 new SqlMetaData("dis_ident", SqlDbType.Int),
                                                 new SqlMetaData("eve_ident", SqlDbType.Int),
                                                 new SqlMetaData("TipVal", SqlDbType.Char, 1),	
							 	                 new SqlMetaData("EjeAdicionalTabulado", SqlDbType.TinyInt),
                                                 new SqlMetaData("EjeAdicionalConsolidado", SqlDbType.TinyInt),
                                             };


                foreach (AnomaliaValidacion item in anomalias)
                {
                    SqlDataRecord rec = new SqlDataRecord(tvp_definition);

                    rec.SetString(0, item.Validada);
                    if (item.Estado != null) rec.SetString(1, item.Estado); else rec.SetDBNull(1);
                    if (item.CodAceptacionRechazo != null) rec.SetByte(2, Convert.ToByte(item.CodAceptacionRechazo.Codigo)); else rec.SetDBNull(2);
                    rec.SetByte(3, item.Via.NumeroVia);
                    rec.SetInt32(4, item.Bloque);
                    rec.SetDateTime(5, item.Fecha);
                    rec.SetString(6, item.Validador.ID);
                    rec.SetString(7, item.FormaPagoConsolidada.MedioPago);
                    rec.SetString(8, item.FormaPagoConsolidada.FormaPago);
                    rec.SetByte(9, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago));
                    if (item.TipoTarifa != null) rec.SetInt32(10, Convert.ToInt32(item.TipoTarifa.CodigoTarifa)); else rec.SetDBNull(10);
                    rec.SetSqlMoney(11, item.MontoConsolidado);
                    rec.SetByte(12, item.CategoriaTabulada.Categoria);
                    rec.SetByte(13, item.CategoriaConsolidada.Categoria);
                    rec.SetString(14, item.ObservacionPeajista);
                    rec.SetString(15, item.ObservacionAnomalia);
                    if (item.ObservacionInterna != null) rec.SetString(16, item.ObservacionInterna); else rec.SetDBNull(16);
                    if (item.ObservacionExterna != null) rec.SetString(17, item.ObservacionExterna); else rec.SetDBNull(17);
                    rec.SetDBNull(18);
                    rec.SetSqlMoney(19, item.MontoDiferencia);
                    rec.SetInt32(20, item.DisIdent);
                    rec.SetInt32(21, item.EveIdent);
                    if (item.TipoValidacionConsolidado != null) rec.SetString(22, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(22);

                    if (item.EjeAdicionalTabulado != 0) rec.SetByte(23, Convert.ToByte(item.EjeAdicionalTabulado)); else rec.SetDBNull(23);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetByte(24, Convert.ToByte(item.EjeAdicionalConsolidado)); else rec.SetDBNull(24);

                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_AutPas_SetAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaAutPas";
                oCmd.Parameters["@anomalias"].Value = tablaAnomalias;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al guardar la validación") + retval.ToString();

                    throw new ErrorSPException(msg);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
