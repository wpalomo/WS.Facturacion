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
    public class TransitoNormalDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo Transito Normal
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte, bool puedeVerValInvisible)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "TicketManual_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TransitoNormal_getAnomalias";
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
                                             new SqlMetaData("FormaPagoConsolidada", SqlDbType.Char, 1),
                                             new SqlMetaData("dis_tipbo", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoPagoConsolidado", SqlDbType.Char, 1),
                                             new SqlMetaData("dis_subfp", SqlDbType.TinyInt),
                                             new SqlMetaData("SubTipoConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("TipoTarifa", SqlDbType.Int),
                                             new SqlMetaData("TipoTarifaConsolidada", SqlDbType.Int),                                            
                                             new SqlMetaData("MontoConsolidado", SqlDbType.SmallMoney),                                           
                                             new SqlMetaData("CategoriaTabulada", SqlDbType.TinyInt),
                                             new SqlMetaData("CategoriaDetectada", SqlDbType.TinyInt),
                                             new SqlMetaData("CategoriaConsolidada", SqlDbType.TinyInt),
                                             new SqlMetaData("ObservacionPeajista", SqlDbType.VarChar, 255), 
                                             new SqlMetaData("ObservacionAnomalia", SqlDbType.VarChar, 255), 
                                             new SqlMetaData("ObservacionInterna", SqlDbType.VarChar, 4000), 
                                             new SqlMetaData("ObservacionExterna", SqlDbType.VarChar, 4000), 
                                             new SqlMetaData("AnomaliaVista", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoDiferencia", SqlDbType.SmallMoney),
                                             new SqlMetaData("dis_ident", SqlDbType.Int),
                                             new SqlMetaData("DACSeparado", SqlDbType.Char, 1),
                                             new SqlMetaData("TipVal", SqlDbType.Char, 1),
                                             new SqlMetaData("CategoriaSeparada", SqlDbType.TinyInt),
                                             new SqlMetaData("CategoriaTag", SqlDbType.TinyInt),
                                             new SqlMetaData("NumeroCuenta", SqlDbType.Int),
                                             new SqlMetaData("MovTag", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoMovTag", SqlDbType.Money),
                                             new SqlMetaData("MovRecTag", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoMovRecTag", SqlDbType.Money),
                                             new SqlMetaData("eve_ident", SqlDbType.Int),
                                             new SqlMetaData("eve_senti", SqlDbType.Char, 1),
                                             new SqlMetaData("EstadoSeparada", SqlDbType.Char, 1),
                                             new SqlMetaData("numev", SqlDbType.Int),
                                             new SqlMetaData("rec_numev", SqlDbType.Int),
                                             new SqlMetaData("rec_fecha", SqlDbType.DateTime),
                                             new SqlMetaData("EjeAdicionalTabulado", SqlDbType.TinyInt),
                                             new SqlMetaData("EjeAdicionalConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("EjeAdicionalDetectado", SqlDbType.TinyInt),
                                             new SqlMetaData("EjeSuspensoConsolidado", SqlDbType.TinyInt)
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
                    rec.SetString(8, item.FormaPagoConsolidada.MedioPago);
                    rec.SetString(9, item.FormaPagoOriginal.FormaPago);
                    rec.SetString(10, item.FormaPagoConsolidada.FormaPago);
                    if (item.FormaPagoOriginal.SubformaPago != null) rec.SetByte(11, Convert.ToByte(item.FormaPagoOriginal.SubformaPago)); else rec.SetDBNull(11);
                    if (item.FormaPagoConsolidada.SubformaPago != null) rec.SetByte(12, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago)); else rec.SetDBNull(12);
                    if (item.TipoTarifa != null) rec.SetInt32(13, Convert.ToInt32(item.TipoTarifa.CodigoTarifa)); else rec.SetDBNull(13);
                    if (item.TipoTarifaConsolidado != null) rec.SetInt32(14, Convert.ToInt32(item.TipoTarifaConsolidado.CodigoTarifa)); else rec.SetDBNull(14);
                    rec.SetSqlMoney(15, item.MontoConsolidado);
                    if (item.CategoriaTabulada != null) rec.SetByte(16, item.CategoriaTabulada.Categoria);
                    if (item.CategoriaDetectada != null) rec.SetByte(17, item.CategoriaDetectada.Categoria);
                    if (item.CategoriaConsolidada != null) rec.SetByte(18, item.CategoriaConsolidada.Categoria);
                    rec.SetString(19, item.ObservacionPeajista);
                    rec.SetString(20, item.ObservacionAnomalia);
                    if (item.ObservacionInterna != null) rec.SetString(21, item.ObservacionInterna); else rec.SetDBNull(21);
                    if (item.ObservacionExterna != null) rec.SetString(22, item.ObservacionExterna); else rec.SetDBNull(22);
                    rec.SetDBNull(23);
                    rec.SetSqlMoney(24, item.MontoDiferencia);
                    if (item.DisIdent != 0) rec.SetInt32(25, item.DisIdent); else rec.SetDBNull(25);
                    if (item.DACSeparado != null) rec.SetString(26, item.DACSeparado); else rec.SetDBNull(26);
                    if (item.TipoValidacionConsolidado != null) rec.SetString(27, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(27);
                    if (item.CategoriaSeparada != null) rec.SetByte(28, item.CategoriaSeparada.Categoria); else rec.SetDBNull(28);
                    if (item.CategoriaTag != null) rec.SetByte(29, item.CategoriaTag.Categoria); else rec.SetDBNull(29);
                    if (item.CuentaConsolidada != null) rec.SetInt32(30, item.CuentaConsolidada.Numero); else rec.SetDBNull(30);
                    if (item.MovTag != null) rec.SetString(31, item.MovTag); else rec.SetDBNull(31);
                    rec.SetSqlMoney(32, item.MontoMovTag);
                    if (item.MovRecTag != null) rec.SetString(33, item.MovRecTag); else rec.SetDBNull(33);
                    rec.SetSqlMoney(34, item.MontoMovRecTag);
                    rec.SetInt32(35, item.EveIdent);
                    if (item.EveSenti != null) rec.SetString(36, item.EveSenti); else rec.SetDBNull(36);
                    if (item.EstadoSeparada != null) rec.SetSqlString(37, item.EstadoSeparada); else rec.SetDBNull(37);
                    if (item.DisNumev != 0) rec.SetInt32(38, item.DisNumev); else rec.SetDBNull(38);
                    if (item.RecNumev != 0) rec.SetInt32(39, item.RecNumev); else rec.SetDBNull(39);
                    if (item.FechaRecarga != null) rec.SetDateTime(40, (DateTime)item.FechaRecarga); else rec.SetDBNull(40);

                    if (item.EjeAdicionalTabulado != 0) rec.SetByte(41, Convert.ToByte(item.EjeAdicionalTabulado)); else rec.SetDBNull(41);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetByte(42, Convert.ToByte(item.EjeAdicionalConsolidado)); else rec.SetDBNull(42);
                    if (item.EjeAdicionalDetectado != 0) rec.SetByte(43, Convert.ToByte(item.EjeAdicionalDetectado)); else rec.SetDBNull(43);

                    if (item.EjeSuspensoConsolidado != null)
                        rec.SetByte(44, item.EjeSuspensoConsolidado);
                    else
                        rec.SetDBNull(44);


                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TransitoNormal_SetAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaTranNormal";
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
