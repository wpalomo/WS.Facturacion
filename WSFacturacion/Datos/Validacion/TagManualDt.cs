using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Microsoft.SqlServer.Server;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Validacion
{
    public class TagManualDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo Tag Manual
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte, bool puedeVerValInvisible)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "TagManual_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TagManual_GetAnomalias";
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

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda la lsita de anomalias validadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>                
        /// ***********************************************************************************************         

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
                                             new SqlMetaData("TipVal", SqlDbType.Char, 1),                                             
                                             new SqlMetaData("CuentaOriginal", SqlDbType.Int),
                                             new SqlMetaData("CuentaConsolidada", SqlDbType.Int),
                                             new SqlMetaData("NumerConsolidado", SqlDbType.VarChar, 24),
                                             new SqlMetaData("PatenteConsolidada", SqlDbType.VarChar,8),
                                             new SqlMetaData("TipoTarifaConsolidado", SqlDbType.Int),
                                             new SqlMetaData("MontoMovTagCredito", SqlDbType.Money),
                                             new SqlMetaData("MontoMovTagDebito", SqlDbType.Money),
                                             new SqlMetaData("MontoMovRecTagCredito", SqlDbType.Money),
                                             new SqlMetaData("MontoMovRecTagDebito", SqlDbType.Money),
                                             new SqlMetaData("eve_ident", SqlDbType.Int),
                                             new SqlMetaData("FormaPagoConsolidada", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoPagoConsolidado", SqlDbType.Char, 1),
                                             new SqlMetaData("FormaPagoConsolidadaAnterior", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoPagoConsolidadoAnterior", SqlDbType.Char, 1),
                                             new SqlMetaData("CuentaConsolidadaAnterior", SqlDbType.Int),
                                             new SqlMetaData("EstadoAnterior", SqlDbType.Char, 1),
                                             new SqlMetaData("SubFPConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("rec_numev", SqlDbType.Int),
                                             new SqlMetaData("rec_fecha", SqlDbType.DateTime),
                                             new SqlMetaData("EjeAdicionalTabulado", SqlDbType.TinyInt),
                                             new SqlMetaData("EjeAdicionalConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("emitag", SqlDbType.VarChar, 5)
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
                    rec.SetString(7, item.FormaPagoOriginal.MedioPago);
                    rec.SetString(8, item.FormaPagoOriginal.FormaPago);
                    rec.SetByte(9, Convert.ToByte(item.FormaPagoOriginal.SubformaPago));
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
                    if (item.TipoValidacionConsolidado != null) rec.SetString(21, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(21);
                    rec.SetInt32(22, item.CuentaOriginal.Numero);
                    if (item.CuentaConsolidada != null)
                        rec.SetInt32(23, item.CuentaConsolidada.Numero);
                    else
                        rec.SetDBNull(23);
                    string NumerConsolidado = "";
                    if (item.VehiculoConsolidado != null)
                    {
                        if (item.VehiculoConsolidado.Tag != null)
                            NumerConsolidado = item.VehiculoConsolidado.Tag.NumeroTag;
                        else if (item.VehiculoConsolidado.Chip != null)
                            NumerConsolidado = item.VehiculoConsolidado.Chip.NumeroInterno.ToString();
                        else if (item.VehiculoConsolidado.TagOSA != null)
                            NumerConsolidado = item.VehiculoConsolidado.TagOSA.NumeroTag;
                    }
                    if (NumerConsolidado != "") rec.SetString(24, NumerConsolidado); else rec.SetDBNull(24);
                    if (item.VehiculoConsolidado != null) rec.SetString(25, item.VehiculoConsolidado.Patente); else rec.SetDBNull(25);
                    if (item.TipoTarifaConsolidado != null) rec.SetInt32(26, Convert.ToInt32(item.TipoTarifaConsolidado.CodigoTarifa)); else rec.SetDBNull(26);
                    rec.SetSqlMoney(27, item.MontoMovTagCredito);
                    rec.SetSqlMoney(28, item.MontoMovTagDebito);
                    rec.SetSqlMoney(29, item.MontoMovRecTagCredito);
                    rec.SetSqlMoney(30, item.MontoMovRecTagDebito);
                    rec.SetInt32(31, item.EveIdent);
                    rec.SetString(32, item.FormaPagoConsolidada.MedioPago);
                    rec.SetString(33, item.FormaPagoConsolidada.FormaPago);
                    if (item.FormaPagoConsolidadaAnterior != null) rec.SetString(34, item.FormaPagoConsolidadaAnterior.MedioPago); else rec.SetDBNull(34);
                    if (item.FormaPagoConsolidadaAnterior != null) rec.SetString(35, item.FormaPagoConsolidadaAnterior.FormaPago); else rec.SetDBNull(35);
                    //rec.SetString(34, item.FormaPagoConsolidadaOriginal.MedioPago);
                    //rec.SetString(35, item.FormaPagoConsolidadaOriginal.FormaPago);
                    rec.SetInt32(36, item.CuentaOriginal.Numero);
                    if (item.Estado != null) rec.SetString(37, item.Estado); else rec.SetDBNull(37);
                    //rec.SetString(37, item.Estado);
                    if (item.FormaPagoConsolidada.SubformaPago != null) rec.SetByte(38, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago)); else rec.SetDBNull(38);
                    rec.SetInt32(39, item.RecNumev);
                    if (item.FechaRecarga != null) rec.SetDateTime(40, (DateTime)item.FechaRecarga); else rec.SetDBNull(40);
                    //rec.SetDateTime(40, (DateTime)item.FechaRecarga);

                    if (item.EjeAdicionalTabulado != 0) rec.SetByte(41, Convert.ToByte(item.EjeAdicionalTabulado)); else rec.SetDBNull(41);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetByte(42, Convert.ToByte(item.EjeAdicionalConsolidado)); else rec.SetDBNull(42);

                    if (item.VehiculoConsolidado != null)
                    {
                        if (item.VehiculoConsolidado.TagOSA != null) rec.SetString(43, item.VehiculoConsolidado.TagOSA.EmisorTag); else rec.SetDBNull(43);
                    }
                    else
                        rec.SetDBNull(43);

                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TagManual_SetAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaTagManual";
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
