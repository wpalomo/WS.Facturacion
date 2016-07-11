using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;
using Telectronica.Tesoreria;


namespace Telectronica.Facturacion
{
    public class ValePrepagoDT
    {

        #region VALEPREPAGO_VENTA: Clase de datos de Venta de Vales Prepagos

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Vales Prepagos NO facturadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <returns>Lista de Vales Prepagos pendientes</returns>
        /// ***********************************************************************************************
        public static ValePrepagoVentaL getVentaValesNoFacturadas(Conexion oConn, int parte)
        {
            return getVentaValesNoFacturadas(oConn, parte, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Vales Prepagos NO facturadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <param name="cliente">Cliente - Cliente</param>
        /// <returns>Lista de Vales Prepagos pendientes</returns>
        /// ***********************************************************************************************
        public static ValePrepagoVentaL getVentaValesNoFacturadas(Conexion oConn, int parte, Cliente cliente)
        {            
            ValePrepagoVentaL oVales = new ValePrepagoVentaL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_getNoFacturados";
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
                if (cliente != null)
                    oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente.NumeroCliente;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVales.Add(CargarVentaVale(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVales;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Vales Prepagos Vendidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="cliente">int - Numero de cliente al que se le personalizaron los vales</param>
        /// <param name="serieDesde">int - Numero de serie inicial</param>
        /// <param name="serieHasta">int - Numero de serie final</param>
        /// <param name="categoria">byte - Categoria de la personalizacion</param>
        /// <returns>Lista de Vales Prepagos vendidos</returns>
        /// ***********************************************************************************************
        public static ValePrepagoVentaL getVentaValesVendidos(Conexion oConn,   int cliente,
                                                              int serieDesde,   int serieHasta,
                                                              byte categoria)
        {
            ValePrepagoVentaL oVales = new ValePrepagoVentaL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_getValesVendidos";
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;
                oCmd.Parameters.Add("@SerDe", SqlDbType.Int).Value = serieDesde;
                oCmd.Parameters.Add("@SerHa", SqlDbType.Int).Value = serieHasta;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = categoria;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVales.Add(CargarVentaVale(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVales;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Vales
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Venta de Vales</param>
        /// <returns>Elemento de ventas de vales Chips de la base de datos</returns>
        /// ***********************************************************************************************
        private static ValePrepagoVenta CargarVentaVale(System.Data.IDataReader oDR)
        {
            // Creamos objetos auxiliares para componer el definitivo
            TipoCuenta oAuxTipoCuenta = new TipoCuenta((int)oDR["cta_tipcu"], oDR["tic_descr"].ToString());
            
            AgrupacionCuenta oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["cta_subfp"], oDR["ctg_descr"].ToString(), null, null, null, null,null,null);

            Cliente oAuxCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());

            Cuenta oAuxCuenta = new Cuenta((int)oDR["dvn_cuent"],
                                           oAuxTipoCuenta,
                                           Util.DbValueToNullable<DateTime>(oDR["cta_feegr"]),
                                           oAuxAgrupacion,
                                           oDR["cta_descr"].ToString(),
                                           oDR["cta_delet"].ToString(),
                                           oAuxCliente);

            Parte oAuxParte = new Parte((int)oDR["dvn_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);

            CategoriaManual oCategoria = new CategoriaManual((byte)oDR["dvn_categ"], "");

            ValePrepagoVenta oVale = new ValePrepagoVenta((int)oDR["dvn_ident"],
                                                new Estacion((byte)oDR["dvn_coest"], ""),
                                                (DateTime)oDR["dvn_fecha"],
                                                oAuxCuenta,
                                                (int)oDR["dvn_insde"],
                                                (int)oDR["dvn_insha"],
                                                oCategoria,
                                                null,
                                                oAuxParte, 
                                                (decimal)oDR["dvn_monto"],
                                                oDR["dvn_anula"].ToString(),
                                                oDR["tii_descr"].ToString(),
                                                new TarifaDiferenciada((byte) oDR["tit_titar"],oDR["tit_descr"].ToString()),
                                                oAuxCliente
                                                );
                                                
                                                
            return oVale;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Ventas de Vales facturadas en una factura
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oFactura">Factura - Factura</param>
        /// <returns>Lista de Items de Ventas de Vales de una Factura</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getVentaValesFacturadas(Conexion oConn, Factura oFactura)
        {
            FacturaItemL oVentas = new FacturaItemL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_getFacturados";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = oFactura.PuntoVenta;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVentas.Add(CargarItemVentaVale(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVentas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem (ValePrepagoVenta)
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Venta de Vales</param>
        /// <returns>Objeto Item de Factura correspondiente a una venta de vales</returns>
        /// ***********************************************************************************************
        private static FacturaItem CargarItemVentaVale(System.Data.IDataReader oDR)
        {
            FacturaItem oItem = FacturacionDt.CargarItem(oDR);

            oItem.Operacion = (Operacion)CargarVentaVale(oDR);

            return oItem;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Graba los registros de la venta de vales prepagos en la base de datos. Aun no esta activa, hay un segundo paso para eso
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oVale">ValePrepago - Venta de vale a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarVentaValePrepago(Conexion oConn, ValePrepagoVenta oVale)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_Guardar";
                oCmd.Parameters.Add("@Coest", SqlDbType.Int).Value = oVale.Estacion.Numero;
                oCmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = oVale.FechaOperacion;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = oVale.Cuenta.Cliente.NumeroCliente;
                oCmd.Parameters.Add("@SerDe", SqlDbType.Int).Value = oVale.SerieInicial;
                oCmd.Parameters.Add("@SerHa", SqlDbType.Int).Value = oVale.SerieFinal;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oVale.Categoria.Categoria;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oVale.Monto;
                oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oVale.Cuenta.Numero;
                oCmd.Parameters.Add("@Titar", SqlDbType.TinyInt).Value = oVale.Cuenta.Agrupacion.TipoTarifa.CodigoTarifa;
                oCmd.Parameters.Add("@SubFp", SqlDbType.TinyInt).Value = oVale.Cuenta.Agrupacion.SubTipoCuenta;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oVale.Parte.Numero;
                oCmd.Parameters.Add("@Tipcu", SqlDbType.Int).Value = oVale.Cuenta.TipoCuenta.CodigoTipoCuenta;
                
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
        /// Elimina la venta del vale (no facturada) de la base de datos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oVale">ValePrepago - Objeto de la venta de vale a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularVentaVale(Conexion oConn, ValePrepagoVenta oVale)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_AnularVentaVales";
                oCmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = oVale.Identity;

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
        /// Graba los datos de la factura en el registro de una Venta de Vales (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una Venta de Vales)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool FacturarVenta(Conexion oConn, FacturaItem oItem)
        {
            return FacturarVenta(oConn, oItem, false, false);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Anula los datos de la factura en el registro de una Venta de Vales (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una Venta de Vales)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool AnularFacturaVenta(Conexion oConn, FacturaItem oItem, bool porNC)
        {
            return FacturarVenta(oConn, oItem, !porNC, porNC);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba los datos de la factura en el registro de una Venta de Vales (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una Venta de Vales)</param>
        /// <param name="anular">bool - true para anular la factura</param>
        /// <param name="anularNC">bool - true para anular la factura por Nota de Credito</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static bool FacturarVenta(Conexion oConn, FacturaItem oItem, bool anular, bool anularNC)
        {
            bool bRet = false;
            try
            {
                ValePrepagoVenta oVenta = oItem.Operacion.ValePrepagoVenta;
                if (oVenta == null)
                    throw new InvalidCastException("No es una Venta de Vales");

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_FacturarItem";
                if (!anular)
                {
                    oCmd.Parameters.Add("@Factu", SqlDbType.Int).Value = oItem.NumeroFactura;
                    oCmd.Parameters.Add("@ItmId", SqlDbType.Int).Value = oItem.Identity;
                }
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oVenta.FechaOperacion;
                oCmd.Parameters.Add("@Ident", SqlDbType.VarChar, 8).Value = oVenta.Identity;
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

        /// ***********************************************************************************************
        /// <summary>
        /// Graba en la tabla de maestros la venta realizada. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oVenta">ValePrepagoVenta - Objeto de la venta que contiene la informacion a grabar</param>
        /// <param name="serie">int - Numero del Vale a Grabar</param>
        /// <param name="numeroVale">string - Codigo de Barras (sin Checksum)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GrabarEnMaestro(Conexion oConn, ValePrepagoVenta oVenta, int serie, string numeroVale)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_GrabaVales";
                oCmd.Parameters.Add("@Serie", SqlDbType.Int).Value = serie;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = oVenta.Cliente.NumeroCliente;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oVenta.Categoria.Categoria;
                oCmd.Parameters.Add("@NVale", SqlDbType.VarChar,18).Value = numeroVale;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oVenta.Estacion.Numero;

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
        /// Elimina en la tabla de maestros la venta realizada. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oVenta">ValePrepagoVenta - Objeto de la venta que contiene la informacion a eliminar</param>
        /// <param name="serie">int - Numero del Vale a Grabar</param>
        /// <param name="numeroVale">string - Codigo de Barras (sin Checksum)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EliminarEnMaestro(Conexion oConn, ValePrepagoVenta oVenta, int serie, string numeroVale)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_EliminarVales";
                oCmd.Parameters.Add("sVale", SqlDbType.VarChar, 18).Value = numeroVale;

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
        #endregion


        #region VALEPREPAGO_PERSONALIZACION: Clase de datos de Personalizacio de Vales Prepagos

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Personalizaciones de Vales Prepagos 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="cliente">int - Numero de cliente al que se le personalizaron los vales</param>
        /// <param name="serieDesde">int - Numero de serie inicial</param>
        /// <param name="serieHasta">int - Numero de serie final</param>
        /// <param name="categoria">byte - Categoria de la personalizacion</param>
        /// <returns>Lista de Vales Prepagos personalizados</returns>
        /// ***********************************************************************************************
        public static ValePrepagoPersonalizacionL getPersonalizacionValesPrepagos(  Conexion oConn,       int cliente,          
                                                                                    int serieDesde,       int serieHasta,      
                                                                                    byte categoria)
        {
            ValePrepagoPersonalizacionL oVales = new ValePrepagoPersonalizacionL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_getPersonalizacionesValidVenta";
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;
                oCmd.Parameters.Add("@SerDe", SqlDbType.Int).Value = serieDesde;
                oCmd.Parameters.Add("@SerHa", SqlDbType.Int).Value = serieHasta;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = categoria;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVales.Add(CargarPersonalizacionVale(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVales;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Vales personalizados
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Vales personalizados</param>
        /// <returns>Elemento de personalizacion de vales de la base de datos</returns>
        /// ***********************************************************************************************
        private static ValePrepagoPersonalizacion CargarPersonalizacionVale(System.Data.IDataReader oDR)
        {
            // Creamos objetos auxiliares para componer el definitivo
            TipoCuenta oAuxTipoCuenta = new TipoCuenta((int)oDR["cta_tipcu"], oDR["tic_descr"].ToString());

            TarifaDiferenciada oAuxTarifaDiferenciada = new TarifaDiferenciada((byte)oDR["tit_titar"], oDR["tit_descr"].ToString(), (float)oDR["tit_porce"]);

            AgrupacionCuenta oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["cta_subfp"], oDR["ctg_descr"].ToString(), oAuxTarifaDiferenciada, null, null, null,null, null);

            Cliente oAuxCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());

            Cuenta oAuxCuenta = new Cuenta((int)oDR["per_cuent"],
                                           oAuxTipoCuenta,
                                           Util.DbValueToNullable<DateTime>(oDR["cta_feegr"]),
                                           oAuxAgrupacion,
                                           oDR["cta_descr"].ToString(),
                                           oDR["cta_delet"].ToString(),
                                           oAuxCliente);

            CategoriaManual oCategoria = new CategoriaManual((byte)oDR["per_categ"], "");

            ValePrepagoPersonalizacion oVale = new ValePrepagoPersonalizacion((int)oDR["per_inlot"],
                                                                              new Zona((byte)oDR["per_zona"], ""), 
                                                                              (DateTime)oDR["per_dtFec"],
                                                                              oAuxCuenta, 
                                                                              new Usuario(oDR["per_usuar"].ToString(), ""),
                                                                              (int)oDR["per_insde"],
                                                                              (int)oDR["per_insha"],
                                                                              oCategoria,
                                                                              (byte)oDR["per_inHoj"],
                                                                              oDR["per_stAnu"].ToString()
                                                                             );

            return oVale;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de habilitaciones de un lote con su precio para ser vendido 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="lote">int - Numero de lote que se consultar la habilitacion</param>
        /// <returns>Habilitacion de los vales con su precio para ser vendidos</returns>
        /// ***********************************************************************************************
        public static ValePrepagoHabilitacionL getHabilitacionVentaVale(Conexion oConn, int cliente,
                                                                                    int serieDesde, int serieHasta,
                                                                                    byte categoria)
        {
            ValePrepagoHabilitacionL oVales = new ValePrepagoHabilitacionL();

            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_getHabilitacionVentaVale";
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;
                oCmd.Parameters.Add("@SerDe", SqlDbType.Int).Value = serieDesde;
                oCmd.Parameters.Add("@SerHa", SqlDbType.Int).Value = serieHasta;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = categoria;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVales.Add(CargarHabilitacionVale(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVales;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en el objeto de habilitacion de Vales
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Habilitacion de Vales</param>
        /// <returns>Elemento de Habilitacion vales prepagos de la base de datos</returns>
        /// ***********************************************************************************************
        private static ValePrepagoHabilitacion CargarHabilitacionVale(System.Data.IDataReader oDR)
        {
            ValePrepagoHabilitacion oVale = new ValePrepagoHabilitacion((int)oDR["pra_inLot"],
                                                                        new Estacion((byte)oDR["pra_estha"], oDR["est_nombr"].ToString()),
                                                                        (decimal)oDR["tar_valor"],
                                                                        oDR["pra_habil"].ToString());


            return oVale;
        }

        #endregion


        #region VALEPREPAGO_LISTANEGRA: Clase de datos de Lista Negra de Vales Prepagos

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de vales prepagos en lista negra que coinciden con los parametros
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="cliente">int - Numero de cliente al que se le personalizaron los vales</param>
        /// <param name="serieDesde">int - Numero de serie inicial</param>
        /// <param name="serieHasta">int - Numero de serie final</param>
        /// <param name="categoria">byte - Categoria de la personalizacion</param>
        /// <returns>Lista de Vales Prepagos en Lista Negra</returns>
        /// ***********************************************************************************************
        public static ValePrepagoListaNegraL getListaNegraValesPrepagos(Conexion oConn, int cliente,
                                                                        int serieDesde, int serieHasta,
                                                                        byte categoria)
        {
            ValePrepagoListaNegraL oVales = new ValePrepagoListaNegraL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VentaVales_getListaNegra";
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;
                oCmd.Parameters.Add("@SerDe", SqlDbType.Int).Value = serieDesde;
                oCmd.Parameters.Add("@SerHa", SqlDbType.Int).Value = serieHasta;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = categoria;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVales.Add(CargarListaNegraVale(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVales;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Lista Negra de Vales 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Lista Negra de Vales</param>
        /// <returns>Elemento de Lista Negra de vales de la base de datos</returns>
        /// ***********************************************************************************************
        private static ValePrepagoListaNegra CargarListaNegraVale(System.Data.IDataReader oDR)
        {
            Cliente oAuxCliente = new Cliente((int)oDR["lin_inCli"], "", "");

            CategoriaManual oCategoria = new CategoriaManual((byte)oDR["lin_inCat"], "");

            ValePrepagoListaNegra oVale = new ValePrepagoListaNegra((int)oDR["lin_ident"], 
                                                                    (DateTime)oDR["lin_dtFec"],
                                                                    DateTime.Now,   // Util.DbValueToNullable < DateTime >(oDR["lin_dtRec"]), EL SERVICIO NO PERMITE DATETIME NULLABLE !! 
                                                                    oAuxCliente,
                                                                    (int)oDR["lin_inSDe"],
                                                                    (int)oDR["lin_inSHa"],
                                                                    oCategoria,
                                                                    oDR["lin_stEst"].ToString(),
                                                                    oDR["lin_comen"].ToString()
                                                                   );

            return oVale;
        }

        #endregion

    }

}
