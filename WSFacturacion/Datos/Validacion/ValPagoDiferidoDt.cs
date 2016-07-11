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
    public class ValPagoDiferidoDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo Pagos Diferidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte, bool puedeVerValInvisible)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "PagoDiferido_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_PagoDiferido_getAnomalias";
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

                SqlMetaData[] tvp_definition = { new SqlMetaData("Validada" , SqlDbType.Char, 1),	
                                                 new SqlMetaData("Estado", SqlDbType.Char, 1), 					
                                                 new SqlMetaData("CodigoAceptRechazo", SqlDbType.TinyInt), 	
                                                 new SqlMetaData("eve_ident", SqlDbType.Int), 
								                 new SqlMetaData("Via", SqlDbType.TinyInt), 						
                                                 new SqlMetaData("Bloque", SqlDbType.Int),							
                                                 new SqlMetaData("Fecha", SqlDbType.DateTime), 			    
                                                 new SqlMetaData("Validador", SqlDbType.VarChar, 10),		
								                 new SqlMetaData("FormaPagoOriginal", SqlDbType.Char, 1), 		
                                                 new SqlMetaData("TipoPagoOriginal", SqlDbType.Char, 1), 			
                                                 new SqlMetaData("dis_subfp", SqlDbType.TinyInt),				
                                                 new SqlMetaData("TipoTarifaOriginal", SqlDbType.Int), 
								                 new SqlMetaData("MontoConsolidado", SqlDbType.SmallMoney),		
                                                 new SqlMetaData("CategoriaTabulada", SqlDbType.TinyInt),			
                                                 new SqlMetaData("CategoriaConsolidada", SqlDbType.TinyInt),   
                                                 new SqlMetaData("PatenteConsolidada", SqlDbType.VarChar, 10), 
								                 new SqlMetaData("CodMarca", SqlDbType.Int),						
                                                 new SqlMetaData("CodModelo", SqlDbType.Int),						
                                                 new SqlMetaData("CodColor", SqlDbType.Int),					
                                                 new SqlMetaData("NombreTitular", SqlDbType.VarChar, 30),
								                 new SqlMetaData("CodProvincia", SqlDbType.Int),					
                                                 new SqlMetaData("Localidad", SqlDbType.VarChar, 30),				
                                                 new SqlMetaData("Domicilio", SqlDbType.VarChar, 100),			
                                                 new SqlMetaData("Telefono", SqlDbType.VarChar, 20),
								                 new SqlMetaData("ObservacionInterna", SqlDbType.VarChar, 4000), 
                                                 new SqlMetaData("ObservacionExterna", SqlDbType.VarChar, 4000),	
                                                 new SqlMetaData("AnomaliaVista", SqlDbType.Char, 1),  		
                                                 new SqlMetaData("MontoDiferencia", SqlDbType.SmallMoney), 	
								                 new SqlMetaData("dis_ident", SqlDbType.Int),					
                                                 new SqlMetaData("PatenteOriginal", SqlDbType.VarChar, 10),			
                                                 new SqlMetaData("TipVal", SqlDbType.Char, 1),				   
                                                 new SqlMetaData("CodigoCausa", SqlDbType.TinyInt),
                                                 new SqlMetaData("EjeAdicionalTabulado", SqlDbType.TinyInt),
                                                 new SqlMetaData("EjeAdicionalConsolidado", SqlDbType.TinyInt),

                                               };

                foreach (AnomaliaValidacion item in anomalias)
                {
                    SqlDataRecord rec = new SqlDataRecord(tvp_definition);

                    rec.SetString(0, item.Validada);
                    if (item.Estado != null) rec.SetString(1, item.Estado); else rec.SetDBNull(1);
                    if (item.CodAceptacionRechazo != null) rec.SetByte(2, Convert.ToByte(item.CodAceptacionRechazo.Codigo)); else rec.SetDBNull(2);
                    rec.SetInt32(3, item.EveIdent);
                    rec.SetByte(4, item.Via.NumeroVia);
                    rec.SetInt32(5, item.Bloque);
                    rec.SetDateTime(6, item.Fecha);
                    rec.SetString(7, item.Validador.ID);
                    if (item.FormaPagoConsolidada != null)
                    {
                        if (item.FormaPagoConsolidada.MedioPago != null) rec.SetString(8, item.FormaPagoConsolidada.MedioPago); else rec.SetDBNull(8);
                        if (item.FormaPagoConsolidada.FormaPago != null) rec.SetString(9, item.FormaPagoConsolidada.FormaPago); else rec.SetDBNull(9);
                        if (item.FormaPagoConsolidada.SubformaPago != null) rec.SetByte(10, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago)); else rec.SetDBNull(10);
                    }
                    if (item.TipoTarifa != null) rec.SetInt32(11, Convert.ToInt32(item.TipoTarifa.CodigoTarifa)); else rec.SetDBNull(11);
                    rec.SetSqlMoney(12, item.MontoConsolidado);
                    if (item.CategoriaTabulada != null) rec.SetByte(13, item.CategoriaTabulada.Categoria);
                    if (item.CategoriaConsolidada != null) rec.SetByte(14, item.CategoriaConsolidada.Categoria);
                    if (item.VehiculoConsolidado != null)
                    {
                        rec.SetString(15, item.VehiculoConsolidado.Patente);
                        if (item.VehiculoConsolidado.Marca != null) rec.SetInt32(16, item.VehiculoConsolidado.Marca.Codigo); else rec.SetDBNull(16);
                        if (item.VehiculoConsolidado.Modelo != null) rec.SetInt32(17, item.VehiculoConsolidado.Modelo.Codigo); else rec.SetDBNull(17);
                        if (item.VehiculoConsolidado.Color != null) rec.SetInt32(18, item.VehiculoConsolidado.Color.Codigo); else rec.SetDBNull(18);
                    }

                    if (item.PagoDiferido != null)
                    {
                        rec.SetString(19, item.PagoDiferido.TitularVehiculo);
                        rec.SetInt32(20, (int)item.PagoDiferido.Provincia);
                        rec.SetString(21, item.PagoDiferido.Localidad);
                        rec.SetString(22, item.PagoDiferido.Direccion);
                        rec.SetString(23, item.PagoDiferido.Telefono);
                    }
                    else
                    {
                        rec.SetDBNull(19);
                        rec.SetDBNull(20);
                        rec.SetDBNull(21);
                        rec.SetDBNull(22);
                        rec.SetDBNull(23);
                    }

                    if (item.ObservacionInterna != null) rec.SetString(24, item.ObservacionInterna); else rec.SetDBNull(24);
                    if (item.ObservacionExterna != null) rec.SetString(25, item.ObservacionExterna); else rec.SetDBNull(25);
                    rec.SetDBNull(26);
                    rec.SetSqlMoney(27, item.MontoDiferencia);
                    rec.SetInt32(28, item.DisIdent);
                    if (item.VehiculoOriginal != null) rec.SetString(29, item.VehiculoOriginal.Patente); else rec.SetDBNull(29);
                    if (item.TipoValidacionConsolidado != null) rec.SetString(30, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(30);

                    if (item.PagoDiferido != null) rec.SetByte(31, Convert.ToByte(item.PagoDiferido.Causa.Codigo)); else rec.SetDBNull(31);

                    if (item.EjeAdicionalTabulado != 0) rec.SetByte(32, Convert.ToByte(item.EjeAdicionalTabulado)); else rec.SetDBNull(32);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetByte(33, Convert.ToByte(item.EjeAdicionalConsolidado)); else rec.SetDBNull(33);

                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_PagoDiferido_setAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaPagDif";
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
