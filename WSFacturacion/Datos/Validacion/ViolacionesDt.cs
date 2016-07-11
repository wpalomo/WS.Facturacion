using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Microsoft.SqlServer.Server;

namespace Telectronica.Validacion
{
    public class ViolacionesDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo Violaciones
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte, bool puedeVerValInvisible)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "Violaciones_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                if(codAnomalia == (int)Anomalia.eAnomalia.enmVIOLAC_VIA_CERRADA)
                    oCmd.CommandText = "Validacion.usp_ViolacionesViaCerrada_getAnomalias";
                else
                    oCmd.CommandText = "Validacion.usp_Violaciones_getAnomalias";

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
                                             new SqlMetaData("FormaPagoConsolidada", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoPagoConsolidado", SqlDbType.Char, 1),
                                             new SqlMetaData("dis_subfp", SqlDbType.TinyInt),
                                             new SqlMetaData("TipoTarifaConsolidado", SqlDbType.Int),
                                             new SqlMetaData("PuntoVenta", SqlDbType.VarChar, 4), 
                                             new SqlMetaData("MontoConsolidado", SqlDbType.SmallMoney),
                                             new SqlMetaData("Ticket", SqlDbType.Int),
                                             new SqlMetaData("CategoriaTabulada", SqlDbType.TinyInt),
                                             new SqlMetaData("CategoriaConsolidada", SqlDbType.TinyInt),
                                             new SqlMetaData("Patente", SqlDbType.VarChar, 10), 
                                             new SqlMetaData("CodMarca", SqlDbType.Int),
                                             new SqlMetaData("CodModelo", SqlDbType.Int),
                                             new SqlMetaData("CodColor", SqlDbType.Int),
                                             new SqlMetaData("NombreTitular", SqlDbType.VarChar, 30), 
                                             new SqlMetaData("Autorizado", SqlDbType.Char, 1),
                                             new SqlMetaData("ObservacionInterna", SqlDbType.VarChar, 4000), 
                                             new SqlMetaData("ObservacionExterna", SqlDbType.VarChar, 4000), 
                                             new SqlMetaData("AnomaliaVista", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoDiferencia", SqlDbType.SmallMoney),
                                             new SqlMetaData("dis_ident", SqlDbType.Int),
                                             new SqlMetaData("dov_ident", SqlDbType.Int),
                                             new SqlMetaData("PatenteVieja", SqlDbType.VarChar, 10), 
                                             new SqlMetaData("TipVal", SqlDbType.Char, 1),
                                             new SqlMetaData("CategoriaTicket", SqlDbType.TinyInt),
                                             new SqlMetaData("CuentaConsolidada", SqlDbType.Int),
                                             new SqlMetaData("NumerConsolidado", SqlDbType.VarChar, 24), 
                                             new SqlMetaData("PatenteConsolidada", SqlDbType.VarChar, 8),                                              
                                             new SqlMetaData("MontoMovTagDebito", SqlDbType.Money),
                                             new SqlMetaData("eve_ident", SqlDbType.Int),
                                             new SqlMetaData("FormaPagoConsolidadaAnterior", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoPagoConsolidadoAnterior", SqlDbType.Char, 1),
                                             new SqlMetaData("TipoFranquiciaConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("Movil", SqlDbType.Int),
                                             new SqlMetaData("NombreEmpresa", SqlDbType.VarChar, 30), 
                                             new SqlMetaData("EjeAdicionalTabulado", SqlDbType.Int),
                                             new SqlMetaData("EjeAdicionalConsolidado", SqlDbType.Int),
                                             new SqlMetaData("emitag", SqlDbType.VarChar, 5),
                                             new SqlMetaData("localidad", SqlDbType.VarChar, 30),
                                             new SqlMetaData("EstadoUF", SqlDbType.Char, 3),
                                             new SqlMetaData("ViaSip", SqlDbType.TinyInt),
                                             new SqlMetaData("NumevSIP", SqlDbType.Int),
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
                        rec.SetString(7, item.FormaPagoConsolidada.MedioPago);
                        rec.SetString(8, item.FormaPagoConsolidada.FormaPago);
                        //rec.SetByte(9, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago));
                            rec.SetDBNull(9);
                        rec.SetByte(38, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago));
                    }
                    else
                    {
                        rec.SetDBNull(7);
                        rec.SetDBNull(8);
                        rec.SetDBNull(9);
                        rec.SetDBNull(38);
                    }                    
                    if (item.TipoTarifaConsolidado != null) rec.SetInt32(10, Convert.ToInt32(item.TipoTarifaConsolidado.CodigoTarifa)); else rec.SetDBNull(10);
                    if (item.PuntoVenta != null) rec.SetString(11, item.PuntoVenta); else rec.SetDBNull(11);
                    rec.SetSqlMoney(12, item.MontoConsolidado);
                    if (item.TicketManual != null) rec.SetInt32(13, item.TicketManual.Ticket); else rec.SetDBNull(13);
                    if (item.CategoriaTabulada != null) rec.SetByte(14, item.CategoriaTabulada.Categoria); else rec.SetDBNull(14);
                    if (item.CategoriaConsolidada != null) rec.SetByte(15, item.CategoriaConsolidada.Categoria); else rec.SetDBNull(15);
                    /*
                    if (item.VehiculoOriginal != null)
                    {
                        rec.SetString(16, item.VehiculoOriginal.Patente);
                    }
                    else
                    {*/
                        rec.SetDBNull(16);
                    //}
                    rec.SetString(21, item.Autorizado ? "S" : "N");
                    if (item.ObservacionInterna != null) rec.SetString(22, item.ObservacionInterna); else rec.SetDBNull(22);
                    if (item.ObservacionExterna != null) rec.SetString(23, item.ObservacionExterna); else rec.SetDBNull(23);
                    rec.SetDBNull(24);
                    rec.SetSqlMoney(25, item.MontoDiferencia);
                    if (item.DisIdent != 0) rec.SetInt32(26, item.DisIdent); else rec.SetDBNull(26);
                    if (item.DovIdent != 0) rec.SetInt32(27, item.DovIdent); else rec.SetDBNull(27);
                    //if (item.VehiculoOriginal != null) rec.SetString(28, item.VehiculoOriginal.Patente); else 
                        rec.SetDBNull(28);
                    if (item.TipoValidacionConsolidado != null) rec.SetString(29, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(29);
                    if (item.CategoriaTicketManual != null) rec.SetByte(30, item.CategoriaTicketManual.Categoria); else rec.SetDBNull(30);
                    if (item.CuentaConsolidada != null) rec.SetInt32(31, item.CuentaConsolidada.Numero); else rec.SetDBNull(31);
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
                    if (NumerConsolidado != "" && NumerConsolidado != null) rec.SetString(32, NumerConsolidado); else rec.SetDBNull(32);
                    if (item.VehiculoConsolidado != null)
                    {
                        rec.SetString(33, item.VehiculoConsolidado.Patente);
                        if (item.VehiculoConsolidado.Marca != null) rec.SetInt32(17, item.VehiculoConsolidado.Marca.Codigo); else rec.SetDBNull(17);
                        if (item.VehiculoConsolidado.Modelo != null) rec.SetInt32(18, item.VehiculoConsolidado.Modelo.Codigo); else rec.SetDBNull(18);
                        if (item.VehiculoConsolidado.Color != null) rec.SetInt32(19, item.VehiculoConsolidado.Color.Codigo); else rec.SetDBNull(19);
                        if (item.VehiculoConsolidado.Cliente != null) rec.SetString(20, item.VehiculoConsolidado.Cliente.RazonSocial); else rec.SetDBNull(20);
                    }
                    else
                    {
                        rec.SetDBNull(33);
                        rec.SetDBNull(17);
                        rec.SetDBNull(18);
                        rec.SetDBNull(19);
                        rec.SetDBNull(20);
                    }
                    
                    rec.SetSqlMoney(34, item.MontoMovTagDebito);
                    if (item.EveIdent > 0) rec.SetInt32(35, item.EveIdent); else rec.SetDBNull(35);
                    if (item.FormaPagoConsolidadaAnterior != null)
                    {
                        rec.SetString(36, item.FormaPagoConsolidadaAnterior.MedioPago);
                        rec.SetString(37, item.FormaPagoConsolidadaAnterior.FormaPago);
                    }
                    else
                    {
                        rec.SetDBNull(36);
                        rec.SetDBNull(37);                     
                    }
                    if (item.Movil != null) rec.SetInt32(39, Convert.ToInt32(item.Movil)); else rec.SetDBNull(39);
                    rec.SetString(40, item.NombreEmpresa);

                    if (item.EjeAdicionalTabulado != 0) rec.SetInt32(41, Convert.ToInt32(item.EjeAdicionalTabulado)); else rec.SetDBNull(41);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetInt32(42, Convert.ToInt32(item.EjeAdicionalConsolidado)); else rec.SetDBNull(42);

                    if (item.VehiculoConsolidado != null)
                    {
                        if (item.VehiculoConsolidado.TagOSA != null) rec.SetString(43, item.VehiculoConsolidado.TagOSA.EmisorTag); else rec.SetDBNull(43);
                    }
                    else
                        rec.SetDBNull(43);


                    if (item.ciudad != null)
                    {
                        rec.SetString(44, item.ciudad);
                    }
                    else
                    {
                        rec.SetDBNull(44);
                    }

                    if (item.EstadoUF != null)
                    {
                        rec.SetString(45, item.EstadoUF.Codigo);
                    }
                    else
                    {
                        rec.SetDBNull(45);
                    }

                    if (item.SimuPaso != null)
                    {

                        if (item.SimuPaso.viaSIP != null)
                        {
                            rec.SetByte(46, Convert.ToByte(item.SimuPaso.viaSIP));
                        }
                        else
                        {
                            rec.SetDBNull(46);
                        }

                        if (item.SimuPaso.numevSip != null)
                        {
                            rec.SetInt32(47, item.SimuPaso.numevSip);
                        }
                        else
                        {
                            rec.SetDBNull(47);
                        }
                    }
                    else
                    {
                        rec.SetDBNull(46);
                        rec.SetDBNull(47);
                    }

                    if (item.EjeSuspensoConsolidado != null)
                        rec.SetByte(48, item.EjeSuspensoConsolidado);
                    else
                        rec.SetDBNull(48);

                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Violaciones_setAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaViolacion";
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

        public static void SetNuevasViolVAbierta(Conexion conn, Anomalia anomalia, Estacion estacion, ParteValidacion parte, ViaDefinicion via, int cantidad, CategoriaManual categoria, DateTime fechaDesde, DateTime fechaHasta, string estado, CodigoValidacion codigoValidacion, string usuario, string obsInterna, string obsExterna)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Violaciones_SetNuevasViolsAbierta";
                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = anomalia.Codigo;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@via", SqlDbType.TinyInt).Value = via.NumeroVia;
                oCmd.Parameters.Add("@canti", SqlDbType.SmallInt).Value = cantidad;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = categoria.Categoria;
                oCmd.Parameters.Add("@fechaIni", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@fechaFin", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar,10).Value = usuario;
                oCmd.Parameters.Add("@obsInterna", SqlDbType.VarChar, 4000).Value = obsInterna;
                oCmd.Parameters.Add("@obsExterna", SqlDbType.VarChar, 4000).Value = obsExterna;

                if( !String.IsNullOrEmpty(estado) )
                {
                    oCmd.Parameters.Add("@Estado", SqlDbType.VarChar, 1).Value = estado;
                    oCmd.Parameters.Add("@CodValid", SqlDbType.Int).Value = codigoValidacion.Codigo;
                }

                SqlParameter parOutError = oCmd.Parameters.Add("@causaError", SqlDbType.VarChar, 200);
                parOutError.Direction = ParameterDirection.Output;
                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir(Convert.ToString(parOutError.Value));
                    conn.Rollback();
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void SetNuevasViolVCerrada(Conexion conn, Anomalia anomalia, Estacion estacion, ParteValidacion parte, ViaDefinicion via, int cantidad, CategoriaManual categoria, DateTime fechaDesde, DateTime fechaHasta, string estado, CodigoValidacion codigoValidacion, string usuario, string obsInterna, string obsExterna)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Violaciones_SetNuevasViolsCerrada";
                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = anomalia.Codigo;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@via", SqlDbType.TinyInt).Value = via.NumeroVia;
                oCmd.Parameters.Add("@canti", SqlDbType.SmallInt).Value = cantidad;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = categoria.Categoria;
                oCmd.Parameters.Add("@fechaIni", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@fechaFin", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@obsInterna", SqlDbType.VarChar, 4000).Value = obsInterna;
                oCmd.Parameters.Add("@obsExterna", SqlDbType.VarChar, 4000).Value = obsExterna;

                if (!String.IsNullOrEmpty(estado))
                {
                    oCmd.Parameters.Add("@Estado", SqlDbType.VarChar, 1).Value = estado;
                    oCmd.Parameters.Add("@CodValid", SqlDbType.Int).Value = codigoValidacion.Codigo;
                }

                SqlParameter parOutError = oCmd.Parameters.Add("@causaError", SqlDbType.VarChar, 200);
                parOutError.Direction = ParameterDirection.Output;
                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir(Convert.ToString(parOutError.Value));
                    conn.Rollback();
                    throw new Errores.ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }
    }
}
