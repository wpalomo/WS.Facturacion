using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Utilitarios;

namespace Telectronica.Facturacion
{
    public class PrepagoDT
    {

        #region RECARGA_SUPERVISION: Clase de datos de Recargas de Supervision

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Recargas de Supervision NO facturadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <returns>Lista de Recargas pendientes</returns>
        /// ***********************************************************************************************
        public static RecargaSupervisionL getRecargasNoFacturadas(Conexion oConn, int parte)
        {
            return getRecargasNoFacturadas(oConn, parte, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Recargas de Supervision NO facturadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <param name="cliente">Cliente - cliente por el cual buscar las operaciones pendientes</param>
        /// <returns>Lista de Recargas pendientes</returns>
        /// ***********************************************************************************************
        public static RecargaSupervisionL getRecargasNoFacturadas(Conexion oConn, int parte, Cliente cliente)
        {
            RecargaSupervisionL oRecargas = new RecargaSupervisionL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Prepagos_getNoFacturados";
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
                if (cliente != null)
                    oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente.NumeroCliente;
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oRecargas.Add(CargarRecarga(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oRecargas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Recargas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Recargas</param>
        /// <returns>Lista con los elementos de recargas de la base de datos</returns>
        /// ***********************************************************************************************
        private static RecargaSupervision CargarRecarga(System.Data.IDataReader oDR)
        {
            // Creamos objetos auxiliares para componer el definitivo
            TipoCuenta oAuxTipoCuenta = new TipoCuenta((int)oDR["cta_tipcu"], oDR["tic_descr"].ToString());

            AgrupacionCuenta oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["cta_subfp"], oDR["ctg_descr"].ToString(), null, null, null, null,null,null);

            Cliente oAuxCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());

            Cuenta oAuxCuenta = new Cuenta((int)oDR["rec_cuent"],
                                           oAuxTipoCuenta,
                                           Util.DbValueToNullable<DateTime>(oDR["cta_feegr"]),
                                           oAuxAgrupacion,
                                           oDR["cta_descr"].ToString(),
                                           oDR["cta_delet"].ToString(),
                                           oAuxCliente);

            Parte oAuxParte = new Parte((int)oDR["rec_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);

            RecargaSupervision oRecarga = new RecargaSupervision((int)oDR["rec_ident"],
                                                                    new Estacion((byte)oDR["rec_coest"], ""),
                                                                    (DateTime)oDR["rec_fecha"],
                                                                    oAuxCuenta,
                                                                    null,
                                                                    oAuxParte,
                                                                    (decimal)oDR["rec_monto"],
                                                                    oDR["rec_anula"].ToString(),
                                                                    oDR["tii_descr"].ToString()
                                                                 );


            return oRecarga;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Recargas facturadas en una factura
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oFactura">Factura - Factura</param>
        /// <returns>Lista de Items de Recargas de una Factura</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getRecargasFacturadas(Conexion oConn, Factura oFactura)
        {
            FacturaItemL oRecargas = new FacturaItemL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Prepagos_getFacturados";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = oFactura.PuntoVenta;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oRecargas.Add(CargarItemRecarga(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oRecargas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem (RecargaSupervision)
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Recargas</param>
        /// <returns>Objeto Item de Factura correspondiente a una recarga</returns>
        /// ***********************************************************************************************
        private static FacturaItem CargarItemRecarga(System.Data.IDataReader oDR)
        {
            FacturaItem oItem = FacturacionDt.CargarItem(oDR);

            oItem.Operacion = (Operacion)CargarRecarga(oDR);

            return oItem;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Graba el registro de la recarga en la base de datos. Aun no esta activa, hay un segundo paso para eso
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oRecarga">RecargaSupervision - Objeto de la recarga que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarRecarga(Conexion oConn, RecargaSupervision oRecarga)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Prepagos_Guardar";
                oCmd.Parameters.Add("Coest", SqlDbType.Int).Value = oRecarga.Estacion.Numero;
                oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oRecarga.Cuenta.Numero;
                oCmd.Parameters.Add("@Saldo", SqlDbType.Money).Value = oRecarga.Monto;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oRecarga.Parte.Numero;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina la recarga (no facturada) de la base de datos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oRecarga">RecargaSupervision - Objeto de la recarga que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularRecarga(Conexion oConn, RecargaSupervision oRecarga)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Prepagos_Anular";
                oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oRecarga.Cuenta.Numero;
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = oRecarga.Identity;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba los datos de la factura en el registro de una Recarga (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una Recarga)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool FacturarRecarga(Conexion oConn, FacturaItem oItem)
        {
            return FacturarRecarga(oConn, oItem, false, false);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Anula los datos de la factura en el registro de una Recarga (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una Recarga)</param>
        /// <param name="porNC">bool - True si se anula por Nota de Credito</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool AnularFacturaRecarga(Conexion oConn, FacturaItem oItem, bool porNC)
        {
            return FacturarRecarga(oConn, oItem, !porNC, porNC);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Graba los datos de la factura en el registro de una Recarga (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una Recarga)</param>
        /// <param name="anular">bool - true para anular la factura</param>
        /// <param name="anularNC">bool - true para anular la factura por Nota de Credito</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static bool FacturarRecarga(Conexion oConn, FacturaItem oItem, bool anular, bool anularNC)
        {
            bool bRet = false;
            try
            {
                RecargaSupervision oRecarga = oItem.Operacion.RecargaSupervision;
                if (oRecarga == null)
                    throw new InvalidCastException("No es una Recarga de Prepagos");

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Prepagos_FacturarItem";
                if (!anular)
                {
                    oCmd.Parameters.Add("@Factu", SqlDbType.Int).Value = oItem.NumeroFactura;
                    oCmd.Parameters.Add("@ItmId", SqlDbType.Int).Value = oItem.Identity;
                }
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = oRecarga.Identity;
                oCmd.Parameters.Add("@anula", SqlDbType.Char, 1).Value = anular ? "S" : "N";
                oCmd.Parameters.Add("@notcr", SqlDbType.Char, 1).Value = anularNC ? "S" : "N";

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No se encontró el registro de la operación");
                    throw new ErrorSPException(msg);
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bRet;
        }

        #endregion

        #region SALDO_PREPAGO: Clase de datos de saldos de prepagos

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el saldo que tiene el cliente para la zona de la estacion que interesa
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea averiguar el saldo</param>
        /// <param name="estacion">int - Numero de estacion de la que se desea saber el saldo</param>
        /// <returns>Objeto con el saldo prepago del cliente</returns>
        /// ***********************************************************************************************
        public static SaldoPrepagoL getSaldoCliente(Conexion oConn, int numeroCliente, int estacion)
        {
            SaldoPrepagoL oPrepago = new SaldoPrepagoL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                oCmd.CommandText = "usp_Clientes_getSaldoPorZona";
                oCmd.Parameters.Add("@Numcl", SqlDbType.Int).Value = numeroCliente;
                oCmd.Parameters.Add("@Estaci", SqlDbType.TinyInt).Value = estacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oPrepago.Add(CargarSaldoPrepago(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oPrepago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve todos los registros de saldo que tiene un cliente para las diferentes zonas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea averiguar el saldo</param>
        /// <returns>Lista de los saldos prepagos del cliente</returns>
        /// ***********************************************************************************************
        public static SaldoPrepagoL getSaldosCliente(Conexion oConn, int numeroCliente)
        {
            SaldoPrepagoL oPrepagoL = new SaldoPrepagoL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                oCmd.CommandText = "usp_Clientes_getSaldos";
                oCmd.Parameters.Add("@Numcl", SqlDbType.Int).Value = numeroCliente;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oPrepagoL.Add(CargarSaldoPrepago(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oPrepagoL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Saldos Prepagos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla</param>
        /// <returns>Objeto Prepago</returns>
        /// ***********************************************************************************************
        private static SaldoPrepago CargarSaldoPrepago(System.Data.IDataReader oDR)
        {
            SaldoPrepago oPrepago = new SaldoPrepago();

            oPrepago.Zona = new Zona((byte)oDR["zon_zona"], oDR["zon_descr"].ToString());

            if (oDR["pre_numcl"] != DBNull.Value)
            {
                //oPrepago.Cliente = new Cliente((int)oDR["pre_numcl"], "", "");
                oPrepago.Saldo = (decimal)oDR["pre_saldo"];
                oPrepago.GiroRojo = (decimal)oDR["pre_grojo"];
                if (oDR["pre_feult"] != DBNull.Value)
                    oPrepago.FechaUltimoMovimiento = (DateTime)oDR["pre_feult"];
            }

            return oPrepago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba un movimiento de Recarga en la cuenta corriente para actualizar el Saldo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oRecarga">RecargaSupervision - Movimiento de Recarga</param>
        /// <param name="bAnular">bool - true para anular</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GrabarMovimientoPrepago(Conexion oConn, RecargaSupervision oRecarga, bool bAnular)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Vias.usp_GrabMovcta";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRecarga.Estacion.Numero;
                oCmd.Parameters.Add("@cuent", SqlDbType.Int).Value = oRecarga.Cuenta.Numero;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now;
                oCmd.Parameters.Add("@codtm", SqlDbType.TinyInt).Value = bAnular ? SaldoPrepago.enmTipo.enmRecargaAnulada : SaldoPrepago.enmTipo.enmRecarga;
                oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = bAnular ? (-oRecarga.Monto) : oRecarga.Monto;
                //oCmd.Parameters.Add("@eveid", SqlDbType.Int).Value = null;
                oCmd.Parameters.Add("@recid", SqlDbType.Int).Value = oRecarga.Identity;
                oCmd.Parameters.Add("@feope", SqlDbType.DateTime).Value = oRecarga.FechaOperacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No se encontró el registro de la operación");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion

        #region RECARGAS_POSIBLES: Clase de datos de recargas posibles

        private static string ArmaIDRecarga(string CodigoEstacion, string TipoDeCuenta, string Agrupacion, string CodigoRecarga)
        {
            string sCodigoRecarga = "";
            sCodigoRecarga = CodigoEstacion + ",";
            sCodigoRecarga += TipoDeCuenta  + ",";
            sCodigoRecarga += Agrupacion + ",";
            sCodigoRecarga += CodigoRecarga;

            return sCodigoRecarga;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Recargas Posibles
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla</param>
        /// <returns>Objeto RecargaPosible</returns>
        /// ***********************************************************************************************
        private static RecargaPosible CargarRecargaPosible(System.Data.IDataReader oDR)
        {
            //string CodigoRecarga = "";
            //CodigoRecarga = Convert.ToString(oDR["rpm_coest"]) + ",";
            //CodigoRecarga += Convert.ToString(oDR["rpm_tipcu"]) + ",";
            //CodigoRecarga += Convert.ToString(oDR["rpm_subfp"]) + ",";
            //CodigoRecarga += Convert.ToString(oDR["rpm_codig"]);

            RecargaPosible oRecarga = new RecargaPosible();
            TipoCuenta oTipoCuenta = new TipoCuenta(Convert.ToInt32(oDR["tic_codig"]), Convert.ToString(oDR["tic_descr"]));
            AgrupacionCuenta oAgrupacion = new AgrupacionCuenta(oTipoCuenta, Convert.ToInt16(oDR["ctg_subfp"]), Convert.ToString(oDR["ctg_descr"]));

            oRecarga.Estacion = new Estacion(Convert.ToInt32(oDR["rpm_coest"]), Convert.ToString(oDR["est_nombr"]));
            oRecarga.TipoCuenta = new TipoCuenta(Convert.ToInt32(oDR["rpm_tipcu"]), Convert.ToString(oDR["tic_descr"]));
            oRecarga.Agrupacion = oAgrupacion;

            oRecarga.EnVia = (oDR["rpm_envia"].ToString().Equals("S") );
            oRecarga.Monto = (decimal)oDR["rpm_monto"];

            oRecarga.ID = ArmaIDRecarga(Convert.ToString(oDR["rpm_coest"]), Convert.ToString(oDR["rpm_tipcu"]), Convert.ToString(oDR["rpm_subfp"]), Convert.ToString(oDR["rpm_codig"]));

            return oRecarga;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene la lista de recargas posibles, filtrando por los parametros indicados
        /// </summary>
        /// <param name="oConn">Conexion de la base de datos</param>
        /// <param name="estacion">Id de la estacion por la cual filtrar, null = todas</param>
        /// <param name="TipoCuenta">Id del tipo de cuenta por la cual filtrar, null = todas</param>
        /// <param name="Agrupacion">Id de la agrupacion por la cual filtrar, null = todas</param>
        /// <returns>Lista de recargas posibles</returns>
        /// ***********************************************************************************************
        public static RecargaPosibleL getRecargasPosibles(Conexion oConn, int? estacion, int? TipoCuenta, int? Agrupacion)
        {
            RecargaPosibleL oRecargaPosibleL = new RecargaPosibleL();

            try
            {
                oRecargaPosibleL = getRecargasPosibles(oConn, estacion, TipoCuenta, Agrupacion, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oRecargaPosibleL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene la lista de recargas posibles, filtrando por los parametros indicados
        /// </summary>
        /// <param name="oConn">Conexion de la base de datos</param>
        /// <param name="estacion">Id de la estacion por la cual filtrar, null = todas</param>
        /// <param name="TipoCuenta">Id del tipo de cuenta por la cual filtrar, null = todas</param>
        /// <param name="Agrupacion">Id de la agrupacion por la cual filtrar, null = todas</param>
        /// <param name="RecargaID">Id de la recarga, null = todas</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static RecargaPosibleL getRecargasPosibles(Conexion oConn, int? estacion, int? TipoCuenta, int? Agrupacion, int? RecargaID)
        {
            RecargaPosibleL oRecargaPosibleL = new RecargaPosibleL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                oCmd.CommandText = "Facturacion.usp_RecargaPosible_getRecargasPosibles";

                // Asignamos los parametros
                oCmd.Parameters.Add("@estacion", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("@TipoCuenta", SqlDbType.Int).Value = TipoCuenta;
                oCmd.Parameters.Add("@Agrupacion", SqlDbType.Int).Value = Agrupacion;
                oCmd.Parameters.Add("@RecargaID", SqlDbType.Int).Value = RecargaID;

                // Ejecutamos la consulta
                oDR = oCmd.ExecuteReader();

                // Cargamos la lista, recorriendo los registros
                while (oDR.Read())
                    oRecargaPosibleL.Add(CargarRecargaPosible(oDR));


                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oRecargaPosibleL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una posible recarga
        /// </summary>
        /// <param name="oUsuario">oPrepagoConfiguracion - Objeto con la informacion de la configuracion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updRecargaPosible(RecargaPosible oRecargaPosible, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_RecargaPosible_updRecargasPosibles";

                oCmd.Parameters.Add("@Estacion", SqlDbType.Int).Value =Convert.ToInt32(oRecargaPosible.Estacion.Numero);
                oCmd.Parameters.Add("@TipoCuenta", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.TipoCuenta.CodigoTipoCuenta);
                oCmd.Parameters.Add("@Agrupacion", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.Agrupacion.SubTipoCuenta);
                oCmd.Parameters.Add("@RecargaID", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.ID.ToString().Split(',')[3]) ;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = Convert.ToDouble(oRecargaPosible.Monto);
                oCmd.Parameters.Add("@EnVia", SqlDbType.Char).Value = oRecargaPosible.EnVia ? 'S' : 'N';

                SqlParameter parRetVal = oCmd.Parameters.Add("@nRet", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro del usuario");
                    throw new ErrorSPException(msg);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una posible recarga
        /// </summary>
        /// <param name="oUsuario">oRecargaPosible - Objeto con la informacion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addRecargaPosible(RecargaPosible oRecargaPosible, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_RecargaPosible_addRecargasPosibles";

                oCmd.Parameters.Add("@Estacion", SqlDbType.Int).Value = Convert.ToInt32( oRecargaPosible.Estacion.Numero);
                oCmd.Parameters.Add("@TipoCuenta", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.TipoCuenta.CodigoTipoCuenta);
                oCmd.Parameters.Add("@Agrupacion", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.Agrupacion.SubTipoCuenta);
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = Convert.ToDouble(oRecargaPosible.Monto);
                oCmd.Parameters.Add("@EnVia", SqlDbType.Char).Value = oRecargaPosible.EnVia ? 'S' : 'N';
                oCmd.Parameters.Add("@RecargaID", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Agregamos el ID de la recarga:
                string Id = Convert.ToString(oCmd.Parameters["@RecargaID"].Value);
                oRecargaPosible.ID = ArmaIDRecarga(oRecargaPosible.Estacion.Numero.ToString(), oRecargaPosible.TipoCuenta.CodigoTipoCuenta.ToString(), oRecargaPosible.Agrupacion.SubTipoCuenta.ToString(), Id);

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                 if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro del usuario");
                    throw new ErrorSPException(msg);
                }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una posible recarga
        /// </summary>
        /// <param name="oUsuario">oPrepagoConfiguracion - Objeto con la informacion de la recarga a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delRecargaPosible(RecargaPosible oRecargaPosible, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_RecargaPosible_delRecargasPosibles";

                oCmd.Parameters.Add("@Estacion", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.Estacion.Numero);
                oCmd.Parameters.Add("@TipoCuenta", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.TipoCuenta.CodigoTipoCuenta);
                oCmd.Parameters.Add("@Agrupacion", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.Agrupacion.SubTipoCuenta);
                oCmd.Parameters.Add("@RecargaID", SqlDbType.Int).Value = Convert.ToInt32(oRecargaPosible.ID.ToString().Split(',')[3]);

                SqlParameter parRetVal = oCmd.Parameters.Add("@nRet", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro de la recarga");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion

        #region CONFIGURACION_PREPAGOS: Clase de datos para la configuracion de prepagos

        /// ***********************************************************************************************
        /// <summary>
        /// Cargamos un objeto de tipo PrepagoConfiguracion desde un objeto de tipo IDataReader
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static PrepagoConfiguracion CargarPrepagoConfiguracion(System.Data.IDataReader oDR)
        {
            PrepagoConfiguracion oPrepagoConfiguracion = new PrepagoConfiguracion();

            //oPrepagoConfiguracion.ID = Convert.ToString(oDR["cfg_ident"]);
            oPrepagoConfiguracion.Habilitado = (oDR["cfg_cobvi"].ToString() == "S");

            return oPrepagoConfiguracion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtenemos una lista de objetos de tipo PrepagoConfiguracion 
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static PrepagoConfiguracionL getPrepagosConfiguraciones(Conexion oConn)
        {
            PrepagoConfiguracionL oPrepagoConfiguracion = new PrepagoConfiguracionL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                oCmd.CommandText = "Facturacion.usp_PrepagoConfiguracion_getPrepagoConfiguracion";

                // Ejecutamos la consulta
                oDR = oCmd.ExecuteReader();

                // Cargamos la lista, recorriendo los registros
                while (oDR.Read())
                    oPrepagoConfiguracion.Add(CargarPrepagoConfiguracion(oDR));

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oPrepagoConfiguracion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la configuracion de los prepagos en la base de datos
        /// </summary>
        /// <param name="oUsuario">oPrepagoConfiguracion - Objeto con la informacion de la configuracion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updPrepagoConfiguracion(PrepagoConfiguracion oPrepagoConfiguracion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_PrepagoConfiguracion_setPrepagoConfiguracion";

                oCmd.Parameters.Add("@cfg_cobvi", SqlDbType.Char).Value = oPrepagoConfiguracion.Habilitado ? "S" : "N";

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro del usuario");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion

    }
}
