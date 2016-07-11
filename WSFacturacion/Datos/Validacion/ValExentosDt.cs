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
    public class ValExentosDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo Exento
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte, bool puedeVerValInvisible)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "Exentos_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Exentos_GetAnomalias";
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
        /// Devuelve la lista de formas de pago
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static FormaPagoValidacionL getCodFranquiciaForpag(Conexion oConn)
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
                oCmd.CommandText = "Validacion.usp_Exentos_getCodFranquiciaForpag";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    FormaPago.Add(CargarFranquiciaFormaPago(oDR));
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
        private static FormaPagoValidacion CargarFranquiciaFormaPago(SqlDataReader oDR)
        {
            FormaPagoValidacion formaPago = new FormaPagoValidacion();
            formaPago.MedioPago = oDR["cod_tipop"].ToString();
            formaPago.FormaPago = oDR["cod_tipbo"].ToString();
            formaPago.SubformaPago = Convert.ToInt16(oDR["cod_subfp"]);
            formaPago.ExentoDesc = oDR["cod_descr"].ToString();
            return formaPago;
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
                                             new SqlMetaData("FormaPagoConsolidada", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoPagoConsolidado", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoFranquiciaConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("TipoTarifaConsolidado", SqlDbType.Int),
                                             new SqlMetaData("PuntoVenta", SqlDbType.VarChar, 4), 
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
                                             new SqlMetaData("Patente", SqlDbType.VarChar, 8),
                                             new SqlMetaData("Movil", SqlDbType.Int),
                                             new SqlMetaData("CodMarca", SqlDbType.Int),
                                             new SqlMetaData("CodModelo", SqlDbType.Int),
                                             new SqlMetaData("CodColor", SqlDbType.Int),
                                             new SqlMetaData("NombreEmpresa", SqlDbType.VarChar, 30),
                                             new SqlMetaData("PatenteVieja", SqlDbType.VarChar, 8),
                                             new SqlMetaData("TipVal", SqlDbType.Char, 1),
                                             new SqlMetaData("CuentaConsolidada", SqlDbType.Int),
                                             new SqlMetaData("NumerConsolidado", SqlDbType.VarChar, 24),
                                             new SqlMetaData("PatenteConsolidada", SqlDbType.VarChar, 8),
                                             new SqlMetaData("MontoMovTagDebito", SqlDbType.Money),
                                             new SqlMetaData("eve_ident", SqlDbType.Int),
                                             new SqlMetaData("FormaPagoConsolidadaAnterior", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoPagoConsolidadoAnterior", SqlDbType.Char, 1),
                                             new SqlMetaData("EjeAdicionalTabulado", SqlDbType.TinyInt),
                                             new SqlMetaData("EjeAdicionalConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("emitag", SqlDbType.VarChar, 5),
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
                    if (item.FormaPagoConsolidada != null)
                    {
                        if (item.FormaPagoConsolidada.MedioPago != null) rec.SetString(7, item.FormaPagoConsolidada.MedioPago); else rec.SetDBNull(7);
                        if (item.FormaPagoConsolidada.FormaPago != null) rec.SetString(8, item.FormaPagoConsolidada.FormaPago); else rec.SetDBNull(8);
                        if (item.FormaPagoConsolidada.SubformaPago != null) rec.SetByte(9, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago)); else rec.SetDBNull(9);
                    }
                    if (item.TipoTarifaConsolidado != null) rec.SetInt32(10, Convert.ToInt32(item.TipoTarifaConsolidado.CodigoTarifa)); else rec.SetDBNull(10);
                    if (item.PuntoVenta != null) rec.SetString(11, item.PuntoVenta);
                    rec.SetSqlMoney(12, item.MontoConsolidado);
                    if(item.CategoriaTabulada != null) rec.SetByte(13, item.CategoriaTabulada.Categoria);
                    if (item.CategoriaConsolidada != null) rec.SetByte(14, item.CategoriaConsolidada.Categoria);
                    rec.SetString(15, item.ObservacionPeajista);
                    rec.SetString(16, item.ObservacionAnomalia);
                    if (item.ObservacionInterna != null) rec.SetString(17, item.ObservacionInterna); else rec.SetDBNull(17);
                    if (item.ObservacionExterna != null) rec.SetString(18, item.ObservacionExterna); else rec.SetDBNull(18);
                    rec.SetDBNull(19);
                    rec.SetSqlMoney(20, item.MontoDiferencia);
                    rec.SetInt32(21, item.DisIdent);
                    
                    if(item.Movil!= null)  rec.SetInt32(23, Convert.ToInt32(item.Movil)); else rec.SetDBNull(23);

                    /*
                    if (item.VehiculoOriginal != null)
                    {
                        rec.SetString(22, item.VehiculoOriginal.Patente);
                        if (item.VehiculoOriginal.Marca != null) rec.SetInt32(24, item.VehiculoOriginal.Marca.Codigo); else rec.SetDBNull(24);
                        if (item.VehiculoOriginal.Modelo != null) rec.SetInt32(25, item.VehiculoOriginal.Modelo.Codigo); else rec.SetDBNull(25);
                        if (item.VehiculoOriginal.Color != null) rec.SetInt32(26, item.VehiculoOriginal.Color.Codigo); else rec.SetDBNull(26);
                    }*/
                    if (item.NombreEmpresa != null) rec.SetString(27, item.NombreEmpresa);

                    /*
                    if (item.VehiculoOriginal != null) rec.SetString(28, item.VehiculoOriginal.Patente); else rec.SetDBNull(28);
                      */
                    if (item.TipoValidacionConsolidado != null) rec.SetString(29, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(29);
                    
                    if(item.CuentaConsolidada != null) rec.SetInt32(30, item.CuentaConsolidada.Numero); else rec.SetDBNull(30);
                    string NumerConsolidado = "";
                    string formasPago = "XIBQURTC";
                    if (item.VehiculoConsolidado != null
                        //Solo para aceptada y FP exento, tag, chip o violacion
                        && item.Estado == "A"
                        && formasPago.IndexOf(item.FormaPagoConsolidada.Codigo.Substring(0,1)) >= 0)
                        {
                        rec.SetString(32, item.VehiculoConsolidado.Patente);
                        if (item.VehiculoConsolidado.Marca != null) rec.SetInt32(24, item.VehiculoConsolidado.Marca.Codigo); else rec.SetDBNull(24);
                        if (item.VehiculoConsolidado.Modelo != null) rec.SetInt32(25, item.VehiculoConsolidado.Modelo.Codigo); else rec.SetDBNull(25);
                        if (item.VehiculoConsolidado.Color != null) rec.SetInt32(26, item.VehiculoConsolidado.Color.Codigo); else rec.SetDBNull(26);
                            
                        if (item.VehiculoConsolidado.Tag != null)
                            NumerConsolidado = item.VehiculoConsolidado.Tag.NumeroTag;
                        else if (item.VehiculoConsolidado.Chip != null)
                            NumerConsolidado = item.VehiculoConsolidado.Chip.NumeroInterno.ToString();
                        else if (item.VehiculoConsolidado.TagOSA != null)
                            NumerConsolidado = item.VehiculoConsolidado.TagOSA.NumeroTag;
                    }
                    else
                    {
                        rec.SetDBNull(32);
                        rec.SetDBNull(24);
                        rec.SetDBNull(25);
                        rec.SetDBNull(26);
                    }
                    if (NumerConsolidado != "") rec.SetString(31, NumerConsolidado); else rec.SetDBNull(31);
                    rec.SetSqlMoney(33, item.MontoMovTagDebito);
                    rec.SetInt32(34, item.EveIdent);
                    if (item.FormaPagoConsolidadaAnterior != null)
                    {
                        rec.SetString(35, item.FormaPagoConsolidadaAnterior.MedioPago);
                        rec.SetString(36, item.FormaPagoConsolidadaAnterior.FormaPago);
                    }
                    else
                    {
                        rec.SetDBNull(35);
                        rec.SetDBNull(36);
                    }

                    if (item.EjeAdicionalTabulado != 0) rec.SetByte(37, Convert.ToByte(item.EjeAdicionalTabulado)); else rec.SetDBNull(37);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetByte(38, Convert.ToByte(item.EjeAdicionalConsolidado)); else rec.SetDBNull(38);

                    if (item.VehiculoConsolidado != null)
                    {
                        if (item.VehiculoConsolidado.TagOSA != null)
                            rec.SetString(39, item.VehiculoConsolidado.TagOSA.EmisorTag);
                        else
                            rec.SetDBNull(39);
                    }
                    else
                        rec.SetDBNull(39);

                    if (item.EjeSuspensoConsolidado != null)
                        rec.SetByte(40, item.EjeSuspensoConsolidado);
                    else
                        rec.SetDBNull(40);

                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Exentos_setAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaExentos";
                oCmd.Parameters["@anomalias"].Value = tablaAnomalias;

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
