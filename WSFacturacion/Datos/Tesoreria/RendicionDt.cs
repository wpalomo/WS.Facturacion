using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Microsoft.SqlServer.Server;

namespace Telectronica.Tesoreria
{
    #region RENDICION: Clase de Datos de RendicionDt.

    public class RendicionDt
    {
        #region MOVIMIENTOS base de las demas

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader con los datos basicos de un movimiento
        /// </summary>
        /// <param name="movimiento">MovimientoCaja - objeto donde cargar</param>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Retiros</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static void CargarMovimiento(MovimientoCaja oMovimiento, System.Data.IDataReader oDR)
        {
            oMovimiento.Estacion = new Estacion((Byte)oDR["par_estac"], oDR["est_nombr"].ToString());
            oMovimiento.Parte = new Parte((int)oDR["par_parte"], (DateTime)oDR["par_fejor"], (Byte)oDR["par_testu"], oDR["jor_estad"].ToString()=="C");
            oMovimiento.Peajista = new Usuario(oDR["par_id"].ToString(), oDR["Peajista"].ToString());
            oMovimiento.Parte.Estacion = oMovimiento.Estacion;
            oMovimiento.Parte.Peajista = oMovimiento.Peajista;

            if (oDR["moc_numer"] != DBNull.Value)
            {
                oMovimiento.NumeroMovimiento = (int)oDR["moc_numer"];
                oMovimiento.Liquidador = new Usuario(oDR["moc_usumo"].ToString(), oDR["Liquidador"].ToString());
                oMovimiento.FechaHoraIngreso = (DateTime)oDR["moc_fecin"];
                oMovimiento.MontoTotal = Convert.ToDecimal(oDR["moc_efect"]);
            }

            // Levantamos el dato de numero de la cabecera de apropiacion de la bolsa. Puede no estar
            if (oDR["apr_numer"] != DBNull.Value)
            {
                oMovimiento.NumeroApropiacionCabecera = (int)oDR["apr_numer"];
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el movimiento ya fue apropiado que impide la anulacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oMovimiento">MovimientoCaja- Liquidacion, Retiro o Reposicion a Anular</param>
        /// ***********************************************************************************************
        public static bool getBolsaApropiada(Conexion oConn, MovimientoCaja oMovimiento)
        {
            return getBolsaApropiada(oConn, oMovimiento.Parte, oMovimiento.NumeroMovimiento);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el movimiento ya fue depositado que impide la anulacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oMovimiento">MovimientoCaja- Liquidacion, Retiro o Reposicion a Anular</param>
        /// ***********************************************************************************************
        public static bool getBolsaDepositada(Conexion oConn, MovimientoCaja oMovimiento)
        {
            return getBolsaDepositada(oConn, oMovimiento.Parte, oMovimiento.NumeroMovimiento);
        }

        #endregion

        #region Retiros

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Retiros de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int? - Parte</param>
        /// <param name="numeroMovimiento">int? - Numero Movimiento de Caja</param>
        /// <param name="desde">DateTime? - Jornada Desde</param>
        /// <param name="hasta">DateTime? - Jornada Hasta</param>
        /// <param name="operador">string - Operador</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de Retiros</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaRetiroL getRetiros(Conexion oConn, int estacion, int? parte, int? numeroMovimiento, DateTime? desde, DateTime? hasta, string operador, int? turnoDesde, int? turnoHasta)
        {
            MovimientoCajaRetiroL oRetiros = new MovimientoCajaRetiroL();
            
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetRetiros";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numeroMovimiento;
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = desde;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = hasta;
            oCmd.Parameters.Add("@opeid", SqlDbType.VarChar,10).Value = operador;
            oCmd.Parameters.Add("@TurnoDesde", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoHasta", SqlDbType.TinyInt).Value = turnoHasta;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oRetiros.Add(CargarRetiro(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oRetiros;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Prepara el Dataset para mostrar la lista de retiros que fuenron confirmados
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="Retiros"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getDetalleRetirosConfirmados(Conexion oConn, MovimientoCajaRetiroL Retiros)
        {
            DataSet dsConfirmaRetiro = new DataSet();
            dsConfirmaRetiro.DataSetName = "Retiros_DetalleRetirosDS";

            try
            {

                List<SqlDataRecord> NumRet = new List<SqlDataRecord>();
                int? Estacion = null;

                SqlMetaData[] tvp_definition = { new SqlMetaData("n", SqlDbType.Int) };

                foreach (MovimientoCajaRetiro item in Retiros)
                {
                    SqlDataRecord rec = new SqlDataRecord(tvp_definition);
                    rec.SetInt32(0, item.NumeroMovimiento);
                    NumRet.Add(rec);
                    Estacion = item.Estacion.Numero;

                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Rendicion_getDetalleRetiros";

                oCmd.Parameters.Add("@NumerMovimiento", SqlDbType.Structured);
                oCmd.Parameters["@NumerMovimiento"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@NumerMovimiento"].TypeName = "listaRetiros";
                oCmd.Parameters["@NumerMovimiento"].Value = NumRet;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsConfirmaRetiro, "DetalleRetiro");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsConfirmaRetiro;
        }

    
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Retiros
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Retiros</param>
        /// <returns>elemento Retiro</returns>
        /// ***********************************************************************************************
        private static MovimientoCajaRetiro CargarRetiro(System.Data.IDataReader oDR)
        {
            //Cargamos datos basicos
            MovimientoCajaRetiro oRetiro = new MovimientoCajaRetiro();
            CargarMovimiento(oRetiro, oDR);

            //Datos particulares
            oRetiro.Moneda = new Moneda((short)oDR["mor_moned"], oDR["mon_descr"].ToString(), oDR["mon_simbo"].ToString(), "N");
            oRetiro.MontoMoneda = Convert.ToDecimal(oDR["mor_monto"]);
            oRetiro.Bolsa = Util.DbValueToNullable<int>(oDR["moc_sobre"]);
            oRetiro.Precinto  = Util.DbValueToNullable<int>(oDR["moc_preci"]);
            oRetiro.Confirmado = (oDR["moc_confi"].ToString() == "S");

            return oRetiro;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Retiro
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oRetiro">Retiro - Objeto con la informacion del retiro a agregar
        ///                     le asigna el numero de movimiento</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addRetiro(Conexion oConn, MovimientoCajaRetiro oRetiro)
        {
            bool ret = false;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_addRetiro";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRetiro.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oRetiro.Parte.Numero;
            oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oRetiro.Peajista.ID;
            //lo pone el servidor
            //oCmd.Parameters.Add("@horain", SqlDbType.DateTime).Value = oRetiro.FechaHoraIngreso;
            oCmd.Parameters.Add("@login", SqlDbType.VarChar, 10).Value = oRetiro.Liquidador.ID;
            oCmd.Parameters.Add("@moneda", SqlDbType.SmallInt).Value = oRetiro.Moneda.Codigo;
            oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = oRetiro.MontoMoneda;
            oCmd.Parameters.Add("@sobre", SqlDbType.Int).Value = oRetiro.Bolsa;
            oCmd.Parameters.Add("@preci", SqlDbType.Int).Value = oRetiro.Precinto;
                
            SqlParameter parNumero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
            parNumero.Direction = ParameterDirection.Output;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;
            if (parNumero.Value != DBNull.Value)
            {
                oRetiro.NumeroMovimiento = Convert.ToInt32(parNumero.Value);
            }

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte ya está liquidado");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Confirma un Retiro
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oRetiro">Retiro - Objeto con la informacion del retiro a confirmar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updConfRetiro(Conexion oConn, MovimientoCajaRetiro oRetiro)
        {
            bool ret = false;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_updConfRetiro";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRetiro.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oRetiro.Parte.Numero;
            oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oRetiro.Peajista.ID;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oRetiro.NumeroMovimiento;
            oCmd.Parameters.Add("@login", SqlDbType.VarChar, 10).Value = oRetiro.Liquidador.ID;
                
            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al confirmar el retiro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                    //En este caso no generamos error
                }
                else if (retval == -103)
                {
                    msg = Traduccion.Traducir("El Retiro no existe");
                    //En este caso no generamos error
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte ya está liquidado");
                    //En este caso no generamos error
                }
                else
                {
                    throw new ErrorSPException(msg);
                }
            }
            else
            {
                ret = true;
            }

            return ret;
        }
                
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Retiro
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oRetiro">Retiro - Objeto con la informacion del retiro a agregar
        ///                     le asigna el numero de movimiento</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delRetiro(Conexion oConn, MovimientoCajaRetiro oRetiro)
        {
            bool ret = false;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_delRetiro";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRetiro.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oRetiro.Parte.Numero;
            oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oRetiro.Peajista.ID;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value=oRetiro.NumeroMovimiento;


            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte ya está liquidado");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        #endregion

        #region Liquidación

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos generales de la Liquidacion de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oParte">Parte - Parte</param>
        /// <returns>Liquidacion</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaLiquidacion getLiquidacion(Conexion oConn, Parte oParte)
        {
            MovimientoCajaLiquidacion oLiquidacion = null;
            
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetLiquidacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;

            oDR = oCmd.ExecuteReader();
            if (oDR.Read())
            {
                oLiquidacion = CargarLiquidacion(oDR);
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oLiquidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader con los datos de la Liquidacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Liquidaciones</param>
        /// <returns>elemento Liquidacion</returns>
        /// ***********************************************************************************************
        private static MovimientoCajaLiquidacion CargarLiquidacion(System.Data.IDataReader oDR)
        {
            //Cargamos datos basicos
            MovimientoCajaLiquidacion oLiquidacion = new MovimientoCajaLiquidacion();

            CargarMovimiento(oLiquidacion, oDR);

            if (oLiquidacion.NumeroMovimiento > 0)
            {
                //Datos particulares
                oLiquidacion.MontoEfectivo = Convert.ToDecimal(oDR["moc_efect"]);
                oLiquidacion.MontoCheque = Convert.ToDecimal(oDR["moc_chequ"]);
                oLiquidacion.Bolsa = Util.DbValueToNullable<int>(oDR["moc_sobre"]);
                oLiquidacion.Precinto = Util.DbValueToNullable<int>(oDR["moc_preci"]);
                oLiquidacion.Observacion = oDR["moc_obser"].ToString();
                


                oLiquidacion.BolsaCheques = Util.DbValueToNullable<int>(oDR["moc_sobrecheque"]);
                if (oDR["apr_numercheque"] != DBNull.Value)
                {
                    oLiquidacion.NumeroApropiacionChequeCabecera = (int)oDR["apr_numercheque"];
                }

                oLiquidacion.BolsaAbTroco = Util.DbValueToNullable<int>(oDR["moc_sobreAbTroco"]);
                if (oDR["apr_numerAbTroco"] != DBNull.Value)
                {
                    oLiquidacion.NumeroApropiacionAbTrocoCabecera = (int)oDR["apr_numerAbTroco"];
                }

                
            }

            if (oDR["ChequePlaza"] != DBNull.Value)
            {
                oLiquidacion.MontoChequePlaza = Convert.ToDecimal(oDR["ChequePlaza"]);
            }

            oLiquidacion.EsParteSpervisor = (oDR["ParteSupervisor"].ToString() == "S");
            oLiquidacion.MontoFacturadoNetoProv = Convert.ToDecimal(oDR["FactNetoProv"]);
            oLiquidacion.MostroDiferencia = Convert.ToString(oDR["par_mosdf"]);
            oLiquidacion.AbandonoDeTroco = Convert.ToDecimal(oDR["AbandonoTroco"]);

            
            return oLiquidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos del Detalle de la Liquidacion de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name=oParte">Parte - Parte</param>
        /// <returns>LIsta del Detalle de una Liquidacion</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaDetalleL getDetalleLiquidacion(Conexion oConn, Parte oParte)
        {
            MovimientoCajaDetalleL oLiquidacion = new MovimientoCajaDetalleL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetDetalleLiquidacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oLiquidacion.Add(CargarDetalleLiquidacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oLiquidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader con los datos de un detalle de la Liquidacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Detalle de Liquidaciones</param>
        /// <returns>elemento Detalle de Liquidacion</returns>
        /// ***********************************************************************************************
        private static MovimientoCajaDetalle CargarDetalleLiquidacion(System.Data.IDataReader oDR)
        {
            MovimientoCajaDetalle oDetalle = new MovimientoCajaDetalle();

            //Datos particulares
            oDetalle.Moneda = new Moneda((short)oDR["mon_moned"], oDR["mon_descr"].ToString(), oDR["mon_simbo"].ToString(), "N");
            oDetalle.Denominacion = new Denominacion(oDetalle.Moneda, (short)oDR["den_denom"], oDR["den_descr"].ToString(), Convert.ToDecimal(oDR["den_valor"]));
            oDetalle.Cotizacion = (decimal)oDR["cot_cotiz"];
            if (oDR["moi_canti"] == DBNull.Value)
            {
                oDetalle.Cantidad = 0;
            }
            else
            {
                oDetalle.Cantidad = (int)oDR["moi_canti"];
            }

            return oDetalle;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos del Detalle de la Liquidacion de cupones de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name=oParte">Parte - Parte</param>
        /// <returns>LIsta del Detalle de cupones de una Liquidacion</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaDetalleCuponL getDetalleCuponLiquidacion(Conexion oConn, Parte oParte)
        {
            MovimientoCajaDetalleCuponL oLiquidacion = new MovimientoCajaDetalleCuponL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetDetalleCuponLiquidacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oLiquidacion.Add(CargarDetalleCuponLiquidacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oLiquidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader con los datos de un detalle de la Liquidacion de cupones
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Detalle de Liquidaciones</param>
        /// <returns>elemento Detalle de Liquidacion</returns>
        /// ***********************************************************************************************
        private static MovimientoCajaDetalleCupon CargarDetalleCuponLiquidacion(System.Data.IDataReader oDR)
        {
            MovimientoCajaDetalleCupon oDetalle = new MovimientoCajaDetalleCupon();

            //Datos particulares
            oDetalle.tipoCupon = new TipoCupon((short)oDR["tcu_tipva"], oDR["tcu_descr"].ToString());
            oDetalle.categoria = new CategoriaManual((byte)oDR["cat_tarif"], oDR["cat_descr"].ToString());
            oDetalle.cantidad = (oDR["mov_canti"] as int?) ?? 0;
            oDetalle.montoUnitario = (decimal)oDR["tar_valor"];
            oDetalle.montoTotal = oDetalle.cantidad * oDetalle.montoUnitario;
            
            return oDetalle;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos de los Tickets Abortados de la Liquidacion de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name=oParte">Parte - Parte</param>
        /// <returns>Lista de los Tickets Abortados por categoria de una Liquidacion</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaTicketsAbortadosL getTicketsAbortadosLiquidacion(Conexion oConn, Parte oParte)
        {
            MovimientoCajaTicketsAbortadosL oTickets = new MovimientoCajaTicketsAbortadosL();
            
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetTicketsAbortadosLiquidacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTickets.Add(CargarDetalleTicketsAbortados(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTickets;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader con los datos de los tickets Abortados
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Detalle de Liquidaciones</param>
        /// <returns>elemento Tickets Abortados de Liquidacion</returns>
        /// ***********************************************************************************************
        private static MovimientoCajaTicketsAbortados CargarDetalleTicketsAbortados(System.Data.IDataReader oDR)
        {
            MovimientoCajaTicketsAbortados oTickets = new MovimientoCajaTicketsAbortados();

            //Datos particulares
            oTickets.Categoria = new CategoriaManual((byte)oDR["cat_tarif"], oDR["cat_descr"].ToString());
            if (oDR["moc_canti"] == DBNull.Value)
            {
                oTickets.Cantidad = 0;
            }
            else
            {
                oTickets.Cantidad = (int)oDR["moc_canti"];
            }

            return oTickets;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Liquidacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oLiquidacion">MovimientoCajaLiquidacion - Objeto con la informacion de la liquidacion a agregar
        ///                     le asigna el numero de movimiento</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addLiquidacion(Conexion oConn, MovimientoCajaLiquidacion oLiquidacion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_addLiquidacion";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oLiquidacion.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oLiquidacion.Parte.Numero;
            oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oLiquidacion.Peajista.ID;            
            oCmd.Parameters.Add("@visa", SqlDbType.Money).Value = oLiquidacion.MontoVisa;
            oCmd.Parameters.Add("@login", SqlDbType.VarChar, 10).Value = oLiquidacion.Liquidador.ID;
            oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = oLiquidacion.MontoEfectivo;
            oCmd.Parameters.Add("@sobre", SqlDbType.Int).Value = oLiquidacion.Bolsa;
            oCmd.Parameters.Add("@preci", SqlDbType.Int).Value = oLiquidacion.Precinto;
            oCmd.Parameters.Add("@valcc", SqlDbType.Money).Value = oLiquidacion.MontoVales;
            oCmd.Parameters.Add("@cheque", SqlDbType.Money).Value = oLiquidacion.MontoCheque;
            oCmd.Parameters.Add("@motic", SqlDbType.Money).Value = oLiquidacion.MontoTicketManuales;


            SqlParameter parNumero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
            parNumero.Direction = ParameterDirection.Output;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;
            if (parNumero.Value != DBNull.Value)
            {
                oLiquidacion.NumeroMovimiento = Convert.ToInt32(parNumero.Value);
            }

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte ya está liquidado");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un item de una Denominacion a una Liquidacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oDetalle">MovimientoCajaDetalle - Objeto con la informacion del detalle de la liquidacion a agregar</param>
        /// <param name="oLiquidacion">MovimientoCajaLiquidacion - Objeto con la informacion de la liquidacion</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addLiquidacionDetalle(Conexion oConn, MovimientoCajaDetalle oDetalle, MovimientoCajaLiquidacion oLiquidacion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_addLiquidacionDetalle";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oLiquidacion.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oLiquidacion.Parte.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oLiquidacion.NumeroMovimiento;
            oCmd.Parameters.Add("@denom", SqlDbType.SmallInt).Value = oDetalle.Denominacion.CodDenominacion;
            oCmd.Parameters.Add("@moned", SqlDbType.SmallInt).Value = oDetalle.Moneda.Codigo;
            oCmd.Parameters.Add("@canti", SqlDbType.Int).Value = oDetalle.Cantidad;


            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte ya está liquidado");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un item de Cantidad de Tickets Abortados a una Liquidacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oTicket">MovimientoCajaTicketsAbortados - Objeto con la informacion de los tickets abortados de la liquidacion a agregar</param>
        /// <param name="oLiquidacion">MovimientoCajaLiquidacion - Objeto con la informacion de la liquidacion</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addLiquidacionTicketsAbortados(Conexion oConn, MovimientoCajaTicketsAbortados oTicket, MovimientoCajaLiquidacion oLiquidacion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_addLiquidacionTicketsAbortados";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oLiquidacion.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oLiquidacion.Parte.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oLiquidacion.NumeroMovimiento;
            oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = oTicket.Categoria.Categoria;
            oCmd.Parameters.Add("@canti", SqlDbType.Int).Value = oTicket.Cantidad;


            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte ya está liquidado");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Asigna el parte al bloque
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oBloque">Bloque - Objeto con la informacion del bloque a asignar</param>
        /// <param name="oLiquidacion">MovimientoCajaLiquidacion - Objeto con la informacion de la liquidacion</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updBloqueSetParte(Conexion oConn, Bloque oBloque, MovimientoCajaLiquidacion oLiquidacion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Vias.usp_AsignaPartePorBloque";

            oCmd.Parameters.Add("@nuestac", SqlDbType.TinyInt).Value = oLiquidacion.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oLiquidacion.Parte.Numero;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = oBloque.Via.NumeroVia;
            oCmd.Parameters.Add("@nturn", SqlDbType.Int).Value = oBloque.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar,10).Value = oLiquidacion.Peajista.ID;
            oCmd.Parameters.Add("@fecap", SqlDbType.DateTime).Value = oBloque.Apertura;
            oCmd.Parameters.Add("@fecci", SqlDbType.DateTime).Value = oBloque.Cierre;


            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte ya está liquidado");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Asigna el parte a las violaciones a via cerrada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oParte">Parte - Objeto con el parte</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updViolacionesViaCerrada(Conexion oConn, Parte oParte)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_updViolacionesViaCerrada";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al asignar violaciones a via cerrada ") + retval.ToString();
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Marca el parte liquidado como validado si no tiene anomalias pendientes
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oParte">Parte - Objeto con el parte</param>
        /// <param name="validador">string - login del usuario</param>
        /// <param name="terminal">string - IP de la terminal</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updParteFinalizarLiquidacion(Conexion oConn, Parte oParte, string validador, string terminal)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "usp_Parte_SetFinalizarParteEnLiquidacion";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;
            oCmd.Parameters.Add("@host", SqlDbType.VarChar, 30).Value = terminal;
            oCmd.Parameters.Add("@obser", SqlDbType.VarChar).Value = oParte.Observacion;


            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al finalizar la validación del parte.")+"\n"+Traduccion.Traducir("Deberá revalidar el parte.") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no existe");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Liquidacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oParte">Parte - Parte de la liquidacion a anular</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delLiquidacion(Conexion oConn, Parte oParte)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_delLiquidacion";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte no está liquidado");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un detalle por tipo de cupon
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="detalleCupon"></param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addDetalleCupon(Conexion oConn, MovimientoCajaDetalleCupon detalleCupon, MovimientoCajaLiquidacion liquidacion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_addDetalleCupon";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = liquidacion.Estacion.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = liquidacion.NumeroMovimiento;
            oCmd.Parameters.Add("@tipva", SqlDbType.SmallInt).Value = detalleCupon.tipoCupon.Codigo;
            oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = detalleCupon.categoria.Categoria;
            oCmd.Parameters.Add("@canti", SqlDbType.Int).Value = detalleCupon.cantidad;
            oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = detalleCupon.montoTotal;
            
            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un detalle por tipo de cupon
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="detalleCupon"></param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delDetalleCupon(Conexion oConn, MovimientoCajaDetalleCupon detalleCupon, MovimientoCajaLiquidacion liquidacion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_delDetalleCupon";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = liquidacion.Estacion.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = liquidacion.NumeroMovimiento;
            oCmd.Parameters.Add("@tipva", SqlDbType.SmallInt).Value = detalleCupon.tipoCupon.Codigo;
            oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = detalleCupon.categoria.Categoria;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene el monto registrado por la vía para compararlo con lo que se liquida
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="parte"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static decimal getMontoRegistradoPorLavia(Conexion oConn, Parte parte)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            decimal dMontoReg;

            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;
            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getMontoAcumuladoRegPorLaVia";

            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            dMontoReg = Convert.ToDecimal(oCmd.ExecuteScalar());
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al obtener el monto acumulado por la vía") + retval.ToString();
                throw new ErrorSPException(msg);
            }

            return dMontoReg;
        }

        #endregion

        #region Reposición

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Pagos de reposiciones
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="estacion"></param>
        /// <param name="sEstado"></param>
        /// <param name="dtFechaDesde"></param>
        /// <param name="dtFechaHasta"></param>
        /// <param name="iMalote"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ReposicionPedidaL getReposicionesPedidas(Conexion oConn, int? estacion, string sEstado, DateTime? dtFechaDesde, DateTime? dtFechaHasta, int? iMalote)
        {
            ReposicionPedidaL oReposiciones = new ReposicionPedidaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_ReposicionPedida_getReposicionPedida";
            oCmd.Parameters.Add("@Estacion", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Estado", SqlDbType.Char, 1).Value = sEstado;
            oCmd.Parameters.Add("@FechaDesde", SqlDbType.DateTime).Value = dtFechaDesde;
            oCmd.Parameters.Add("@FechaHasta", SqlDbType.DateTime).Value = dtFechaHasta;
            oCmd.Parameters.Add("@Bolsa", SqlDbType.Int).Value = iMalote;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oReposiciones.Add(CargarReposicionesPedidas(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oReposiciones;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga una entidad con los datos traidos de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ReposicionPedida CargarReposicionesPedidas(IDataReader oDR)
        {
            ReposicionPedida reposicion = new ReposicionPedida();

            reposicion.Identity = (int)oDR["IDReposi"];
            reposicion.Estado = new EstadoReposicion(Convert.ToString(oDR["Estado"]));
            reposicion.FechaIngreso = Convert.ToDateTime(oDR["FechaCarga"]);
            reposicion.Pagado = (oDR["IDPago"] != DBNull.Value);
            //reposicion.Sentido = new ViaSentidoCirculacion(Convert.ToString(oDR["SentidoId"]), Convert.ToString(oDR["Sentido"]));
            reposicion.Monto = Convert.ToDecimal(oDR["MontoReponer"]);
            reposicion.Parte = new Parte((int)oDR["Parte"], Convert.ToDateTime(oDR["Fejor"]), (byte)oDR["Turno"]);
            reposicion.Parte.Peajista = new Usuario(Convert.ToString(oDR["OperadorId"]), Convert.ToString(oDR["OperadorNombre"]));
            reposicion.Estacion = new Estacion(Convert.ToInt32(oDR["rep_estac"]), Convert.ToString(oDR["est_nombr"]));
            reposicion.TipoDeReposicion = new TipoDeReposicion(Convert.ToString(oDR["TipoReposicion"]));
            
            if(oDR["rep_estac"] != DBNull.Value)
                reposicion.bolsa = Convert.ToString(oDR["Bolsa"]);
            // Referido al pago de la Reposición

            if (oDR["IDPago"] != DBNull.Value)
            {
                reposicion.PagoReposicion = new MovimientoCajaReposicion();
                reposicion.PagoReposicion.IdPago = (int)(oDR["IDPago"]);
                reposicion.PagoReposicion.NumeroMovimiento = (int)oDR["IDMovimientoCaja"];
                reposicion.PagoReposicion.Estacion = new Estacion(Convert.ToInt32(oDR["rep_estac"]), Convert.ToString(oDR["est_nombr"]));
                reposicion.PagoReposicion.TipoDeReposicion = new TipoDeReposicion(Convert.ToString(oDR["TipoReposicion"]));
                reposicion.PagoReposicion.MontoTotal = Convert.ToDecimal(oDR["MontoReponer"]);                
                reposicion.PagoReposicion.Parte = new Parte((int)oDR["Parte"], Convert.ToDateTime(oDR["Fejor"]), (byte)oDR["Turno"]);
                reposicion.PagoReposicion.Parte.Peajista = new Usuario(Convert.ToString(oDR["OperadorId"]), Convert.ToString(oDR["OperadorNombre"]));
                reposicion.PagoReposicion.Parte.Estacion = reposicion.PagoReposicion.Estacion;
                reposicion.PagoReposicion.Peajista = reposicion.PagoReposicion.Parte.Peajista;
                reposicion.PagoReposicion.FechaHoraIngreso = Convert.ToDateTime(oDR["FechaPago"]);
                reposicion.PagoReposicion.Recibo = (int)oDR["Recibo"];
                reposicion.PagoReposicion.Bolsa = (int)oDR["Malote"];
                reposicion.PagoReposicion.NumeroApropiacionCabecera = (int)oDR["apr_numer"];
            }

            return reposicion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la Reposicion de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oParte">Parte - Parte</param>
        /// <returns>Reporision</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaReposicion getReposicion(Conexion oConn, Parte oParte)
        {
            MovimientoCajaReposicion oReposicion = null;
            ParteReposicionL oPartes = getPartesReposiciones(oConn, oParte.Estacion.Numero, oParte.Numero, oParte.Jornada, oParte.Jornada, oParte.Peajista.ID, oParte.Turno, oParte.Turno);
            
            if (oPartes != null && oPartes.Count > 0)
            {
                oReposicion = oPartes[0].Reposicion;
            }

            return oReposicion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes con su Reposicion de un parte o jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int? - Parte</param>
        /// <param name="desde">DateTime? - Jornada Desde</param>
        /// <param name="hasta">DateTime? - Jornada Hasta</param>
        /// <param name="operador">string - operador</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de Retiros</returns>
        /// ***********************************************************************************************
        public static ParteReposicionL getPartesReposiciones(Conexion oConn, int estacion, int? parte,  DateTime? desde, DateTime? hasta, string operador, int? turnoDesde, int? turnoHasta)
        {
            ParteReposicionL oReposiciones = new ParteReposicionL();
            
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetPartesReposiciones";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = desde;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = hasta;
            oCmd.Parameters.Add("@opeid", SqlDbType.VarChar,10).Value = operador;
            oCmd.Parameters.Add("@TurnoDesde", SqlDbType.TinyInt).Value = turnoDesde;
            oCmd.Parameters.Add("@TurnoHasta", SqlDbType.TinyInt).Value = turnoHasta;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oReposiciones.Add(CargarParteReposicion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oReposiciones;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de ParteReposiciones
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Retiros</param>
        /// <returns>elemento ParteReposicion</returns>
        /// ***********************************************************************************************
        private static ParteReposicion CargarParteReposicion(System.Data.IDataReader oDR)
        {
            //Cargamos datos basicos
            ParteReposicion oParteReposicion = new ParteReposicion();

            oParteReposicion.Parte = new Parte((int)oDR["par_parte"], (DateTime)oDR["par_fejor"], (Byte)oDR["par_testu"], oDR["jor_estad"].ToString() == "C");
            oParteReposicion.Parte.Estacion = new Estacion((byte)oDR["par_estac"], oDR["est_nombr"].ToString());
            oParteReposicion.Parte.Peajista = new Usuario(oDR["par_id"].ToString(), oDR["Peajista"].ToString());
            oParteReposicion.Parte.EstaLiquidado = (oDR["par_liqui"].ToString() == "S");
            oParteReposicion.Parte.EstaValidado = (oDR["pvr_valid"].ToString() == "S");

            if (oDR["moc_numer"] != DBNull.Value)
            {
                oParteReposicion.Reposicion = CargarReposicion(oDR);
            }

            return oParteReposicion;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Reposiciones
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Retiros</param>
        /// <returns>elemento Reposicion</returns>
        /// ***********************************************************************************************
        private static MovimientoCajaReposicion CargarReposicion(System.Data.IDataReader oDR)
        {
            //Cargamos datos basicos
            MovimientoCajaReposicion oReposicion = new MovimientoCajaReposicion();
            CargarMovimiento(oReposicion, oDR);

            //Datos particulares
            oReposicion.Moneda = new Moneda((short)oDR["mor_moned"], oDR["mon_descr"].ToString(), oDR["mon_simbo"].ToString(), "N");
            oReposicion.MontoMoneda = Convert.ToDecimal(oDR["mor_monto"]);
            oReposicion.Bolsa = Util.DbValueToNullable<int>(oDR["moc_sobre"]);
            oReposicion.Precinto = Util.DbValueToNullable<int>(oDR["moc_preci"]);

            return oReposicion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Reposicion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oReposicion">MovimientoCajaReposicion - Objeto con la informacion de la reposicion a agregar
        ///                     le asigna el numero de movimiento</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addReposicion(Conexion oConn, MovimientoCajaReposicion oReposicion, int? numerReposicionPedida)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_addReposicion";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oReposicion.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oReposicion.Parte.Numero;
            oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oReposicion.Peajista.ID;
            //lo pone el servidor
            //oCmd.Parameters.Add("@horain", SqlDbType.DateTime).Value = oReposicion.FechaHoraIngreso;
            oCmd.Parameters.Add("@login", SqlDbType.VarChar, 10).Value = oReposicion.Liquidador.ID;
            oCmd.Parameters.Add("@moneda", SqlDbType.SmallInt).Value = oReposicion.Moneda.Codigo;
            oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = oReposicion.MontoMoneda;
            oCmd.Parameters.Add("@sobre", SqlDbType.Int).Value = oReposicion.Bolsa;
            oCmd.Parameters.Add("@preci", SqlDbType.Int).Value = oReposicion.Precinto;

            if (oReposicion.TipoDeReposicion != null)
            {
                oCmd.Parameters.Add("@tipoRep", SqlDbType.Char, 1).Value = oReposicion.TipoDeReposicion.TipoCodigo;
            }
            oCmd.Parameters.Add("@recibo", SqlDbType.Int).Value = oReposicion.Recibo;
            oCmd.Parameters.Add("@nroReposicion", SqlDbType.Int).Value = numerReposicionPedida;

            SqlParameter parNumero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
            parNumero.Direction = ParameterDirection.Output;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;
            if (parNumero.Value != DBNull.Value)
            {
                oReposicion.NumeroMovimiento = Convert.ToInt32(parNumero.Value);
            }

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte no está liquidado");
                }
                else if (retval == -103)
                {
                    msg = Traduccion.Traducir("El Parte ya tenía repoición");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Reposicion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oReposicion">MovimientoCajaReposicion - Objeto con la informacion de la reposicion a agregar
        ///                     le asigna el numero de movimiento</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delReposicion(Conexion oConn, MovimientoCajaReposicion oReposicion)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_delPagoReposicion";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oReposicion.Estacion.Numero;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oReposicion.Parte.Numero;
            oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oReposicion.Peajista.ID;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oReposicion.NumeroMovimiento;
            oCmd.Parameters.Add("@RepIdent", SqlDbType.Int).Value = oReposicion.IdPago;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("El Parte no es del Peajista");
                }
                else if (retval == -104)
                {
                    msg = Traduccion.Traducir("El Parte no está liquidado");
                }
                throw new ErrorSPException(msg);
            }

            return true;
        }

        #endregion

        #region Bloques

        /// ***********************************************************************************************
        /// <summary>
        /// Lista de bloques que pueden entrar en una liquidacion 
        /// pueden ya tener el mismo parte o no teer parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static BloqueL getBloquesLiquidacion(Conexion oConn, Parte oParte, int? cantBloques)
        {
            BloqueL oBloques = new BloqueL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getBloquesLiquidacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
            oCmd.Parameters.Add("@CantBloques", SqlDbType.Int).Value = cantBloques;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBloques.Add(CargarBloque(oDR));
            }
                
            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oBloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Bloques
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Bloques</param>
        /// <returns>elemento Bloque</returns>
        /// ***********************************************************************************************
        private static Bloque CargarBloque(System.Data.IDataReader oDR)
        {
            Bloque oBloque = new Bloque();
            oBloque.Numero = (int)oDR["tur_nturn"];
            oBloque.Via = new Via((byte)oDR["tur_coest"], (byte)oDR["tur_nuvia"]);
            oBloque.Via.NombreVia = oDR["via_nombr"].ToString();
            oBloque.Apertura = (DateTime)oDR["tur_fecap"];
            oBloque.Cierre = (DateTime)oDR["tur_fecci"];
            oBloque.Peajista = new Usuario(oDR["tur_id"].ToString(), oDR["Peajista"].ToString());
            oBloque.Modo = new ViaModo(oDR["tur_modo"].ToString(), oDR["DescrModo"].ToString());
            oBloque.SentidoCirculacion = new ViaSentidoCirculacion(oDR["tur_senti"].ToString(), oDR["DescrSentido"].ToString());
            oBloque.Autotabulante = oDR["tur_monoc"].ToString();
            oBloque.Identity = (int)oDR["tur_ident"];
            oBloque.VisaIntegrado = (oDR["tur_visa"] as decimal?) ?? 0;

            if( oDR["PeajistaConBloqueConflictivo"] != DBNull.Value )
            {
                oBloque.HayBloqueConflictivo = true;
                oBloque.PeajistaConBloqueConflictivo = oDR["PeajistaConBloqueConflictivo"].ToString();
            }

            if (oDR["Transito"] != DBNull.Value)
            {
                oBloque.Transito = (int)oDR["Transito"];
            }

            return oBloque;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Lista de vias abiertas que impiden la liquidacion 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static BloqueL getViasAbiertas(Conexion oConn, Parte oParte)
        {
            BloqueL oBloques = new BloqueL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getViasAbiertas";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBloques.Add(CargarBloque2(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oBloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Bloques
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Bloques</param>
        /// <returns>elemento Bloque</returns>
        /// ***********************************************************************************************
        private static Bloque CargarBloque2(System.Data.IDataReader oDR)
        {
            Bloque oBloque = new Bloque();
            oBloque.Numero = (int)oDR["via_nturn"];
            oBloque.Via = new Via((byte)oDR["via_coest"], (byte)oDR["via_nuvia"]);
            oBloque.Apertura = (DateTime)oDR["via_fecap"];
            oBloque.Peajista = new Usuario(oDR["via_id"].ToString(), oDR["Peajista"].ToString());

            return oBloque;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los bloques sin liquidar
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iEstacion"></param>
        /// <param name="dtFecha"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static BloqueL getBloquesSinLiquidarPorJornada(Conexion oConn, int? iEstacion, DateTime dtFecha)
        {
            BloqueL oBloques = new BloqueL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_GetBloquesSinLiquidar";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iEstacion;
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = dtFecha;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBloques.Add(CargarBloque3(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oBloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Bloques
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Bloques</param>
        /// <returns>elemento Bloque</returns>
        /// ***********************************************************************************************
        private static Bloque CargarBloque3(IDataReader oDR)
        {
            Bloque oBloque = new Bloque();
            oBloque.Numero = (int)oDR["tur_nturn"];
            oBloque.Apertura = (DateTime)oDR["tur_fecap"];
            oBloque.Cierre = (DateTime)oDR["tur_fecci"];
            oBloque.Peajista = new Usuario(oDR["tur_id"].ToString(), oDR["use_nombr"].ToString());

            return oBloque;
        }

        #endregion

        #region Partes

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si tiene la estacion abierta que impide la liquidacion 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static bool getEstacionAbierta(Conexion oConn, Parte oParte)
        {
            bool bAbierta = false;

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getEstacionAbierta";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            if (oDR.Read())
            {
                bAbierta = true;
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            
            return bAbierta;
        }


        ///*************************************************************************************
        /// <summary>
        /// Elimina la reposicion Financiera que posee un parte especifico, siempre que no se encuentre pagada
        /// </summary>
        /// <param name="conn">Conexion con Gestion</param>
        /// <param name="oParte">Objeto ParteValidacion</param>
        /// <returns>Bool</returns>
        /// ************************************************************************************
        public static bool delReposicionFinanciera(Conexion conn, ReposicionPedida oRep)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Rendicion_delReposicionPedida";

                oCmd.Parameters.Add("@Plaza", SqlDbType.TinyInt).Value = oRep.Estacion.Numero;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oRep.Parte.Numero;
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 1).Value = "F";
                oCmd.Parameters.Add("@Bolsa", SqlDbType.Int, 1).Value = oRep.bolsa; // En la economica no hay bolsa

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro") + retval.ToString();
                    if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                    }
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("La Reposición ya se encuentra paga");
                    }
                    throw new ErrorSPException(msg);
                }
                else
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si tiene una terminal de ventas abierta que impide la liquidacion 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static bool getTerminalAbierta(Conexion oConn, Parte oParte)
        {
            bool bAbierta = false;
            
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getTerminalAbierta";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            if (oDR.Read())
            {
                bAbierta = true;
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return bAbierta;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Ver si tiene retiros sin confirmar que impide la liquidacion 
        /// </summary>
        /// <param name="oConn">Conexion</param>
        /// <param name="oParte">Parte</param>
        /// <returns>Si viene algun retiro devuelvo true si no false</returns>
        /// ***********************************************************************************************
        public static bool getRetirosSinConfirmar(Conexion oConn, Parte oParte)
        {
            
            if (getRetirosSinConfirmar(oConn, oParte.Estacion.Numero, oParte.Jornada, oParte.Turno, oParte.Numero, oParte.Peajista.ID,"N").Count > 0)
                return true;
            else
                return false;
        }
        
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static MovimientoCajaRetiroL getRetirosSinConfirmar(Conexion oConn, int Estacion, DateTime? Jornada, int? Turno, int? Parte, string Peajista,string Estado)
        {
            MovimientoCajaRetiroL oRetiros = new MovimientoCajaRetiroL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getRetirosSinConfirmar";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = Parte;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = Turno;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = Peajista;
            oCmd.Parameters.Add("@Estado", SqlDbType.Char, 1).Value = Estado;
                
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oRetiros.Add(CargarRetiro(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
                
            return oRetiros;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si tiene partes anteriores sin liquidar que impide la liquidacion 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static ParteL getParteAnteriorSinLiquidar(Conexion oConn, Parte oParte)
        {
            ParteL oPartes = new ParteL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getParteAnteriorSinLiquidar";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oPartes.Add(CargarParte(oDR));
            }
                
            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene una lista con partes sin liquidar
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iEstacion"></param>
        /// <param name="dtFecha"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ParteL getPartesSinLiquidarPorJornada(Conexion oConn, int? iEstacion, DateTime dtFechaJornada)
        {
            ParteL oPartes = new ParteL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Partes_GetPartesSinLiquidar";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iEstacion;
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = dtFechaJornada;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oPartes.Add(CargarParteConUsuario(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si tiene partes posteriores ya liquidados que impide anular la liquidacion 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static ParteL getPartePosteriorLiquidado(Conexion oConn, Parte oParte)
        {
            ParteL oPartes = new ParteL();
            
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getPartePosteriorLiquidado";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oPartes.Add(CargarParte(oDR));
            }
                
            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Partes
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <returns>elemento Parte</returns>
        /// ***********************************************************************************************
        private static Parte CargarParteConUsuario(IDataReader oDR)
        {
            Parte parte = CargarParte(oDR);
            parte.Peajista = new Usuario(oDR["par_id"].ToString(), oDR["use_nombr"].ToString());
            return parte;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Partes
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <returns>elemento Parte</returns>
        /// ***********************************************************************************************
        private static Parte CargarParte(IDataReader oDR)
        {
            Parte oParte = new Parte();
            oParte.Numero = (int)oDR["par_parte"];
            oParte.Jornada = (DateTime)oDR["par_fejor"];
            oParte.Estacion = new Estacion((Byte)oDR["par_estac"], "");
            oParte.EstaLiquidado = (oDR["par_liqui"].ToString() == "S");
            oParte.DevolvioFondo = (oDR["par_fondo"].ToString() == "S");
            oParte.Apertura = (DateTime)oDR["par_feape"];
            if (oParte.EstaLiquidado && !(oDR["par_feliq"] is System.DBNull))
            {
                oParte.Liquidacion = (DateTime)oDR["par_feliq"];
            }
            oParte.Turno = (Byte)oDR["par_testu"];
            oParte.Nivel = (Byte)oDR["par_nivel"];
            oParte.Peajista = new Usuario(oDR["par_id"].ToString(), "");
            oParte.ModoMantenimiento = (oDR["par_mante"].ToString() == "S");

            return oParte;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el movimiento ya fue apropiado que impide la anulacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte- Parte a Anular</param>
        /// <param name=numeroMovimiento">int?- Numero de Movimiento (si es null consideramos la liquidacion)</param>
        /// ***********************************************************************************************
        public static bool getBolsaApropiada(Conexion oConn, Parte oParte, int? numeroMovimiento)
        {
            bool ret = false;

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getBolsaApropiada";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numeroMovimiento;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            if (oDR.Read())
            {
                ret = true;
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el movimiento ya fue depositado que impide la anulacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte- Parte a Anular</param>
        /// <param name=numeroMovimiento">int?- Numero de Movimiento (si es null consideramos la liquidacion)</param>
        /// ***********************************************************************************************
        public static bool getBolsaDepositada(Conexion oConn, Parte oParte, int? numeroMovimiento )
        {
            bool ret = false;

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getBolsaDepositada";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numeroMovimiento;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            if(oDR.Read())
            {
                ret = true;
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si ya fue pedido el fallo que impide la anulacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte- Parte a Anular</param>
        /// ***********************************************************************************************
        public static bool getHayFalloPedido(Conexion oConn, Parte oParte)
        {
            bool ret = false;

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getHayFalloPedido";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                
            oDR = oCmd.ExecuteReader();
            if (oDR.Read())
            {
                ret = true;
            }
                
            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Lista de Violaciones a Via Cerrada que entran al parte del supervisor
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name=oParte">Parte - Parte</param>
        /// ***********************************************************************************************
        public static InfoEventoL getViolacionesViaCerrada(Conexion oConn, Parte oParte, int? cantViolaciones)
        {
            InfoEventoL oViolaciones = new InfoEventoL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Rendicion_getViolacionesViaCerrada";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
            oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
            oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
            oCmd.Parameters.Add("@cantViol", SqlDbType.Int).Value = cantViolaciones;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oViolaciones.Add(CargarViolacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oViolaciones;
        }

        #endregion


        #region REPOSICIONES


        ///************************************************************************************************************
        /// <summary>
        /// Genera una reposicion Financiera
        /// </summary>
        /// <param name="oConn">Conexion a la DB</param>
        /// <param name="oReposi">Movimiento de Caja Tipo Reposicion</param>
        /// <returns>true si esta todo OK</returns>
        /// ***********************************************************************************************************
        public static bool addReposicionFinanciera(Conexion oConn, ReposicionPedida oReposi)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Rendicion_setReposicion";

                oCmd.Parameters.Add("@Plaza", SqlDbType.TinyInt).Value = oReposi.Estacion.Numero;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oReposi.Parte.Numero;
                oCmd.Parameters.Add("@LegajoPeajista", SqlDbType.VarChar, 10).Value = oReposi.Peajista;
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 1).Value = "F";
                oCmd.Parameters.Add("@Valor", SqlDbType.Money).Value = oReposi.Monto;
                oCmd.Parameters.Add("@LegajoUsuario", SqlDbType.VarChar, 10).Value = oReposi.Peajista;
                oCmd.Parameters.Add("@Bolsa", SqlDbType.Int).Value = oReposi.bolsa;



                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                    }
                    throw new ErrorSPException(msg);
                }
                else
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }


        #endregion


        #region Eventos

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Carga un elemento DataReader en una Violacion
        // AUTOR ...........: Damian Jachniuk
        // FECHA CREACIÓN ..: 08/07/2010
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oDR - System.Data.IDataReader - Objeto DataReader de la tabla
        //                    Retorna: Lista de InfoEvento: InfoEventoL
        // ----------------------------------------------------------------------------------------------
        private static InfoEvento CargarViolacion(System.Data.IDataReader oDR)
        {
            InfoEvento oInfoEvento = new InfoEvento();

            oInfoEvento.Fecha = Conversiones.edt_DateTime(oDR["dis_fecha"]);
            oInfoEvento.Dac = Conversiones.edt_Str(oDR["dis_dac"]);

            if (Conversiones.edt_Str(oDR["via_sente"]) == "A")
            {
                oInfoEvento.Sentido = "Asc";
            }
            else
            {
                if (Conversiones.edt_Str(oDR["via_sente"]) == "D")
                {
                    oInfoEvento.Sentido = "Des";
                }
                else
                {
                    oInfoEvento.Sentido = Conversiones.edt_Str(oDR["via_sente"]);
                }
            }

            oInfoEvento.Exento = "";

            oInfoEvento.NroTransito = Conversiones.edt_Str(oDR["eve_numtr"]);
            oInfoEvento.Ejes = Conversiones.edt_Str(oDR["dis_ejers"]);
            oInfoEvento.DobleEje = Conversiones.edt_Str(oDR["dis_ejerd"]);
            oInfoEvento.Altura = Conversiones.edt_Str(oDR["dis_altur"]);
            oInfoEvento.Observaciones = Conversiones.edt_Str(oDR["eve_obser"]);
            oInfoEvento.CodEst = Conversiones.edt_Str(oDR["dis_coest"]);
            oInfoEvento.Nuvia = Conversiones.edt_Str(oDR["dis_nuvia"]);
            oInfoEvento.Ident = Conversiones.edt_Str(oDR["dis_numev"]);
            oInfoEvento.IdTran = Conversiones.edt_Str(oDR["dis_numev"]);
            oInfoEvento.CodEve = Conversiones.edt_Str(oDR["eve_codev"]);
            oInfoEvento.FormaPagoInicial = Conversiones.edt_Str(oDR["dis_tipop"]);
            oInfoEvento.SubTipoFormaPago = Conversiones.edt_Str(oDR["dis_tipbo"]);
            oInfoEvento.obsSupervisor = Conversiones.edt_Str(oDR["obs_comen"]);

            return oInfoEvento;
        }

        #endregion
    }

    #endregion
}
