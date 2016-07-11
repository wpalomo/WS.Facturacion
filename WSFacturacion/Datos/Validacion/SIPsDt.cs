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
    public class SIPsDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// lista de anomalias de tipo SIP
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>        
        /// <param name="codAnomalia">int - Codigo de la anomalia</param>
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getAnomalias(Conexion oConn, int codAnomalia, ParteValidacion parte, bool puedeVerValInvisible)
        {
            DataSet dsAnomalias = new DataSet();
            dsAnomalias.DataSetName = "SIPs_AnomaliasDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Sips_getAnomalias";
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
                                             new SqlMetaData("PuntoVenta", SqlDbType.VarChar, 4), 
                                             new SqlMetaData("MontoConsolidado", SqlDbType.SmallMoney),
                                             new SqlMetaData("Ticket", SqlDbType.Int),
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
                                             new SqlMetaData("NumeroCuenta", SqlDbType.Int),
                                             new SqlMetaData("MovTag", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoMovTag", SqlDbType.Money),
                                             new SqlMetaData("MovRecTag", SqlDbType.Char, 1),
                                             new SqlMetaData("MontoMovRecTag", SqlDbType.Money),
                                             new SqlMetaData("eve_ident", SqlDbType.Int),
                                             new SqlMetaData("rec_numev", SqlDbType.Int),
                                             new SqlMetaData("rec_fecha", SqlDbType.DateTime),
                                             new SqlMetaData("TipoRecarga", SqlDbType.Char, 1),
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
                    rec.SetInt32(4,item.Bloque);
                    rec.SetDateTime(5, item.Fecha);
                    rec.SetString(6, item.Validador.ID);
                    rec.SetString(7, item.FormaPagoConsolidada.MedioPago);
                    rec.SetString(8, item.FormaPagoConsolidada.FormaPago);
                    rec.SetByte(9, Convert.ToByte(item.FormaPagoConsolidada.SubformaPago));
                    if (item.TipoTarifa != null) rec.SetInt32(10, Convert.ToInt32(item.TipoTarifa.CodigoTarifa)); else rec.SetDBNull(10);
                    rec.SetString(11, item.PuntoVenta);
                    rec.SetSqlMoney(12, item.MontoConsolidado);
                    if (item.TicketManual != null) rec.SetInt32(13, item.TicketManual.Ticket); else rec.SetDBNull(13);
                    rec.SetByte(14, item.CategoriaTabulada.Categoria);
                    rec.SetByte(15, item.CategoriaConsolidada.Categoria);
                    rec.SetString(16, item.ObservacionPeajista);
                    rec.SetString(17, item.ObservacionAnomalia);
                    if (item.ObservacionInterna != null) rec.SetString(18, item.ObservacionInterna); else rec.SetDBNull(18);
                    if (item.ObservacionExterna != null) rec.SetString(19, item.ObservacionExterna); else rec.SetDBNull(19);
                    rec.SetDBNull(20);
                    rec.SetSqlMoney(21, item.MontoDiferencia);
                    rec.SetInt32(22, item.DisIdent);
                    if (item.TipoValidacionConsolidado != null) rec.SetString(23, item.TipoValidacionConsolidado.Codigo); else rec.SetDBNull(23);
                    if (item.CuentaConsolidada != null) rec.SetInt32(24, item.CuentaConsolidada.Numero); else rec.SetDBNull(24);
                    if (item.MovTag != null) rec.SetString(25, item.MovTag); else rec.SetDBNull(25);
                    rec.SetSqlMoney(26, item.MontoMovTag);
                    if (item.MovRecTag != null) rec.SetString(27, item.MovRecTag); else rec.SetDBNull(27);
                    rec.SetSqlMoney(28, item.MontoMovRecTag);
                    rec.SetInt32(29, item.EveIdent);
                    rec.SetInt32(30, item.RecNumev);
                    if (item.FechaRecarga != null) rec.SetDateTime(31, (DateTime)item.FechaRecarga); else rec.SetDBNull(31);
                    if (item.TipoRecarga != null) rec.SetString(32, item.TipoRecarga.ToString()); else rec.SetDBNull(32);

                    if (item.EjeAdicionalTabulado != 0) rec.SetByte(33, Convert.ToByte(item.EjeAdicionalTabulado)); else rec.SetDBNull(33);
                    if (item.EjeAdicionalConsolidado != 0) rec.SetByte(34, Convert.ToByte(item.EjeAdicionalConsolidado)); else rec.SetDBNull(34);

                    tablaAnomalias.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Sips_setAnomaliasValidadas";

                oCmd.Parameters.Add("@anomalia", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@anomalias", SqlDbType.Structured);
                oCmd.Parameters["@anomalias"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@anomalias"].TypeName = "listaSips";
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


        public static SimulacionDePasoL GetSIPs(Conexion oConn, int estacion, DateTime fecha, int Tolerancia)
        {
            SimulacionDePasoL oSIPs = new SimulacionDePasoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Sips_getSips";
                oCmd.Parameters.Add("@coestViolac", SqlDbType.TinyInt).Value = estacion;
                //oCmd.Parameters.Add("@nuviaViolac", SqlDbType.TinyInt).Value = Anomalia.Via.NumeroVia;
                //oCmd.Parameters.Add("@eventoViolac", SqlDbType.Int).Value = Anomalia.EveIdent;
                oCmd.Parameters.Add("@fechaViolacion", SqlDbType.DateTime).Value = fecha;
                oCmd.Parameters.Add("@ToleranciaViolacion", SqlDbType.Int).Value = Tolerancia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oSIPs.Add(CargarSIPs(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oSIPs;
        }

        private static SimulacionDePaso CargarSIPs(System.Data.IDataReader oDR)
        {
            SimulacionDePaso oSIP = new SimulacionDePaso();


            if (oDR["Fecha"] != DBNull.Value)
                oSIP.FechaSIP = Convert.ToDateTime(oDR["Fecha"]);

            if (oDR["Via"] != DBNull.Value)
                oSIP.viaSIP = Convert.ToInt32(oDR["Via"]);

            oSIP.forpag = new FormaPago(oDR["for_tipop"].ToString(), oDR["for_tipbo"].ToString(), oDR["for_descr"].ToString());

            oSIP.Categ = new CategoriaManual(Convert.ToByte(oDR["cat_tarif"]), oDR["cat_descr"].ToString());

            if (oDR["tra_ticke"] != DBNull.Value)
                oSIP.ticket = Convert.ToInt32(oDR["tra_ticke"]);

            if (oDR["dis_parte"] != DBNull.Value)
                oSIP.parte = Convert.ToInt32(oDR["dis_parte"]);

            oSIP.tipoSimulacion = oDR["TipoSip"].ToString();

            oSIP.observacionPlaza = oDR["obs_comen"].ToString();

            oSIP.utilizado = (oDR["Utilizado"].ToString() == "S" ? true : false);

            oSIP.numevSip = Convert.ToInt32(oDR["EventoSip"]);

            if (oDR["FechaViolac"] != DBNull.Value)
                oSIP.FechaViol = Convert.ToDateTime(oDR["FechaViolac"]);

            if (oDR["ViaViolac"] != DBNull.Value)
                oSIP.viaViol = Convert.ToInt32(oDR["ViaViolac"]);

            if (oDR["ParteViolac"] != DBNull.Value)
                oSIP.parteViol = Convert.ToInt32(oDR["ParteViolac"]);

            oSIP.Placa = oDR["Placa"].ToString();
            
            return oSIP;

        }

    }
}
