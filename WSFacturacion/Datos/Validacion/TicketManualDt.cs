using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using System.Data.SqlClient;
using Telectronica.Tesoreria;
using Microsoft.SqlServer.Server;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Validacion
{
    public class TicketManualDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo Ticket Manual
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
                oCmd.CommandText = "Validacion.usp_TicketManual_GetAnomalias";
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
        /// retorna una lista de Ticket Manuales
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="ptoVenta">ptoVenta - Punto de venta</param>
        /// <param name="tckInicial">tckInicial - Ticket Manual inicial</param>
        /// <param name="tckFinal">tckFinal - Ticket Manual inicial</param>
        /// <param name="categoria">categoria - Categoria del ticket</param>
        /// ***********************************************************************************************
        public static TicketManualL getTicketManual(Conexion oConn, int tckInicial, int tckFinal, int? categoria, string tipoComprobante)
        {
             TicketManualL tickets = new TicketManualL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TicketManual_getTicket";
               // oCmd.Parameters.Add("@ptovta", SqlDbType.Char, 4).Value = ptoVenta;
                oCmd.Parameters.Add("@TicketInicial", SqlDbType.Int).Value = tckInicial;
                oCmd.Parameters.Add("@TicketFinal", SqlDbType.Int).Value = tckFinal;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = categoria;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = tipoComprobante;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    tickets.Add(CargaTicketManual(oDR));
                }
                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

                return tickets;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TicketManual CargaTicketManual(SqlDataReader oDR)
        {
            TicketManual ticket = new TicketManual();
            ticket.PuntoVenta = Convert.ToString(oDR["PuntoVenta"]);
            if(oDR["Ticket"] != DBNull.Value)
                ticket.Ticket = Convert.ToInt32(oDR["Ticket"]);
            if(oDR["Estacion"] != DBNull.Value)
                ticket.Estacion = new Estacion { Numero = Convert.ToByte(oDR["Estacion"]) };
            if (oDR["Via"] != DBNull.Value)
                ticket.Via = new Via { NumeroVia = Convert.ToByte(oDR["Via"]), NombreVia = Convert.ToString(oDR["ViaNombre"]) };
            if (oDR["Parte"] != DBNull.Value)
                ticket.Parte = new Parte { Numero = Convert.ToInt32(oDR["Parte"]) };
            if (oDR["Peajista"] != DBNull.Value)
                ticket.Cajero = new Usuario { ID = Convert.ToString(oDR["Peajista"]) };
            if (oDR["Fecha"] != DBNull.Value)
                ticket.Fecha = Convert.ToDateTime(oDR["Fecha"]);
            if (oDR["Validador"] != DBNull.Value)
                ticket.Validador = new Usuario { ID = Convert.ToString(oDR["Validador"]) }; 
		
            return ticket;
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
                                             new SqlMetaData("PuntoVenta", SqlDbType.VarChar, 4),
                                             new SqlMetaData("Ticket", SqlDbType.Int),
                                             new SqlMetaData("CategoriaTabulada", SqlDbType.TinyInt),
                                             new SqlMetaData("CategoriaConsolidada", SqlDbType.TinyInt),
                                             new SqlMetaData("CategoriaDetectada", SqlDbType.TinyInt),
                                             new SqlMetaData("CategoriaTicket", SqlDbType.TinyInt),
                                             new SqlMetaData("ObservacionPeajista", SqlDbType.VarChar, 255), 
                                             new SqlMetaData("ObservacionAnomalia", SqlDbType.VarChar, 255), 
                                             new SqlMetaData("ObservacionInterna", SqlDbType.VarChar, 4000), 
                                             new SqlMetaData("ObservacionExterna", SqlDbType.VarChar, 4000), 
                                             new SqlMetaData("AnomaliaVista", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoDiferencia", SqlDbType.SmallMoney),
                                             new SqlMetaData("dis_ident", SqlDbType.Int),
                                             new SqlMetaData("dov_ident", SqlDbType.Int),
                                             new SqlMetaData("eve_ident", SqlDbType.Int),
                                             new SqlMetaData("TipVal", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoTicketManual", SqlDbType.SmallMoney),
                                             new SqlMetaData("SubsidioPatente", SqlDbType.VarChar, 8), 
                                             new SqlMetaData("SubsidioTarjeta", SqlDbType.VarChar, 16), 
                                             new SqlMetaData("EjeAdicionalTabulado", SqlDbType.TinyInt),
                                             new SqlMetaData("EjeAdicionalConsolidado", SqlDbType.TinyInt),
                                             new SqlMetaData("EjeAdicionalDetectado", SqlDbType.TinyInt),
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
                    if (item.TicketManual != null)
                    {
                        rec.SetString(12, String.IsNullOrEmpty(item.TicketManual.PuntoVenta) ? item.PuntoVenta : item.TicketManual.PuntoVenta);
                        rec.SetInt32(13, item.TicketManual.Ticket);
                    }
                    if (item.CategoriaTabulada != null) rec.SetByte(14, item.CategoriaTabulada.Categoria);
                    if (item.CategoriaConsolidada != null) rec.SetByte(15, item.CategoriaConsolidada.Categoria);
                    if (item.CategoriaDetectada != null) rec.SetByte(16, item.CategoriaDetectada.Categoria);
                    if (item.CategoriaTicketManual != null) rec.SetByte(17, item.CategoriaTicketManual.Categoria);
                    rec.SetString(18, item.ObservacionPeajista);
                    rec.SetString(19, item.ObservacionAnomalia);
                    if (item.ObservacionInterna != null) rec.SetString(20, item.ObservacionInterna); else rec.SetDBNull(20);
                    if (item.ObservacionExterna != null) rec.SetString(21, item.ObservacionExterna); else rec.SetDBNull(21);
                    rec.SetDBNull(22);
                    rec.SetSqlMoney(23, item.MontoDiferencia);                                            
                    rec.SetInt32(24, item.DisIdent);                        
                    rec.SetInt32(25,item.DovIdent);                        
                    rec.SetInt32(26,item.EveIdent);
                    if (item.TipoValidacionConsolidado != null) rec.SetString(27, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(27);
                    rec.SetSqlMoney(28,item.MontoTicketManual);    
                    rec.SetDBNull(29);
                    rec.SetDBNull(30);

                    if (item.EjeAdicionalTabulado != 0) rec.SetByte(31, Convert.ToByte(item.EjeAdicionalTabulado)); else rec.SetDBNull(31);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetByte(32, Convert.ToByte(item.EjeAdicionalConsolidado)); else rec.SetDBNull(32);
                    if (item.EjeAdicionalDetectado != 0) rec.SetByte(33, Convert.ToByte(item.EjeAdicionalDetectado)); else rec.SetDBNull(33);

                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TicketManual_setAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaTicketManual";
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

        public static int SetNuevosTicketsManuales(Conexion conn, ParteValidacion parte, Estacion estacion, Anomalia anomalia, ViaDefinicion via, int numeroTicket, int cantidad, CategoriaManual categoria, DateTime fechaIni, DateTime fechaFin, string tipoComprobante, string observInterna, string vista, string iDVal, CodigoValidacion codigoAceptacion)
        {
            int retval = 0;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.USP_TicketManual_SetNuevosTicketsManuales";
                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = anomalia.Codigo;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@Via", SqlDbType.TinyInt).Value = via.NumeroVia;
               // oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = puntoVenta;
                oCmd.Parameters.Add("@ticket", SqlDbType.Int).Value = numeroTicket;
                oCmd.Parameters.Add("@canti ", SqlDbType.SmallInt).Value = cantidad;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = categoria.Categoria;
                oCmd.Parameters.Add("@fechaIni", SqlDbType.DateTime).Value = fechaIni;
                oCmd.Parameters.Add("@fechaFin", SqlDbType.DateTime).Value = fechaFin;
                oCmd.Parameters.Add("@obserInt", SqlDbType.VarChar, 4000).Value = observInterna;
               // oCmd.Parameters.Add("@obserExt", SqlDbType.VarChar,4000).Value = obserExterna;
                oCmd.Parameters.Add("@vista", SqlDbType.Char, 1).Value = vista;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar,10).Value = iDVal;
                oCmd.Parameters.Add("@CodigoAceptacion", SqlDbType.TinyInt).Value = codigoAceptacion.Codigo;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = tipoComprobante;
        //        oCmd.Parameters.Add("@docum", SqlDbType.VarChar, 15).Value = ruc;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 3600;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        public static bool esPuntoVentaCompatible(Conexion conn, Estacion estacion, CategoriaManual categoria, ViaDefinicion via, string tipoComprobante,string puntoVenta)
        {
            int retval;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TicketManual_EsPuntoVentaCompatible";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@Via", SqlDbType.TinyInt).Value = via.NumeroVia;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = categoria.Categoria;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = tipoComprobante;
                oCmd.Parameters.Add("@ptovta", SqlDbType.Char, 4).Value = puntoVenta;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 3600;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retval == 1;
        }



        public static void getNombreYEstadoRuc(Conexion conn, Int64 ruc, out string nombre, out string estado)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_TicketManual_GetNombreYEstadoRuc";
                oCmd.Parameters.Add("@ruc", SqlDbType.BigInt).Value = ruc;
               
                SqlParameter nombreOut = oCmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100);
                nombreOut.Direction = ParameterDirection.Output;
                SqlParameter estadoOut = oCmd.Parameters.Add("@estado", SqlDbType.Char, 2);
                estadoOut.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    conn.Rollback();
                    throw new Errores.ErrorSPException("Ruc no encontrado");
                }

                nombre = Convert.ToString(nombreOut.Value);
                estado = Convert.ToString(estadoOut.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

    }
}
