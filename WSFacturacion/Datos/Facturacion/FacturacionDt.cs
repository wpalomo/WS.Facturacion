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
    public class FacturacionDt
    {

        #region FALLOS: Clase de Datos de Fallos

        public static bool FacturarFallo(Conexion oConn, FacturaItem oItem)
        {
            return FacturarFallo(oConn, oItem, false, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Anula los datos de la factura en el registro de un Fallo (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una Fallo)</param>
        /// <param name="porNC">bool - True si se anula por Nota de Credito</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool AnularFacturaFallo(Conexion oConn, FacturaItem oItem, bool porNC)
        {
            return FacturarFallo(oConn, oItem, !porNC, porNC);
        }

        private static bool FacturarFallo(Conexion oConn, FacturaItem oItem, bool anular, bool anularNC)
        {
            bool bRet = false;
            try
            {
                Fallos oFallo = oItem.Operacion.Fallo;
                if (oFallo == null)
                    throw new InvalidCastException("No es un Fallo");

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Fallos_FacturarItem";

                // Los campos de facturacion se establecen a NULL cuando se anula la operacion.
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFallo.Estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oFallo.Parte;
                oCmd.Parameters.Add("@tipitm", SqlDbType.Int).Value = oFallo.TipoItem;

                if (anular)
                {
                    oCmd.Parameters.Add("@factu", SqlDbType.Int).Value = null;
                    oCmd.Parameters.Add("@itmid", SqlDbType.Int).Value = null;
                    oCmd.Parameters.Add("@pendi", SqlDbType.Char, 1).Value = "S";
                    oCmd.Parameters.Add("@anula", SqlDbType.Char, 1).Value = "S";
                }
                else
                {
                    oCmd.Parameters.Add("@factu", SqlDbType.Int).Value = oItem.NumeroFactura;
                    oCmd.Parameters.Add("@itmid", SqlDbType.Int).Value = oItem.Identity;
                    oCmd.Parameters.Add("@pendi", SqlDbType.Char, 1).Value = "N";
                    oCmd.Parameters.Add("@anula", SqlDbType.Char, 1).Value = "N";
                }

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


        /// <summary>
        /// Obtiene la lista de los fallos no facturados para el cliente en el parte indicado.
        /// </summary>
        /// <param name="oConn">Conexion a la base de datos</param>
        /// <param name="parte">Numero de parte filtrado</param>
        /// <param name="cliente">Cliente filtrado</param>
        /// <param name="tipo">Operacion.enmTipo - Tipo (Fallo o Violacion)</param>
        /// <returns></returns>
        public static FallosL getFallosNoFacturados(Conexion oConn, int parte, Cliente cliente, Operacion.enmTipo tipo)
        {
            FallosL oFallos = new FallosL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_FallosAnteriores_getNoFacturados";
                oCmd.Parameters.Add("@tipitm", SqlDbType.Int).Value = tipo;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                    oFallos.Add(CargarFallo(oDR, tipo));

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oFallos;
        }

        private static Fallos CargarFallo(System.Data.IDataReader oDR, Operacion.enmTipo tipo)
        {
            Fallos oFallos = new Fallos();

            //Parte oAuxParte = new Parte((int)oDR[Pre + "parte"], (DateTime)oDR[Pre + "fecha"], (byte)oDR["par_testu"]);
            oFallos.TipoItem = tipo;
            oFallos.Estacion = Convert.ToInt16(oDR["fpd_coest"]);
            oFallos.Parte = Convert.ToInt32(oDR["fpd_parte"]);
            oFallos.identity = Convert.ToInt32(oDR["fpd_ident"]);
            oFallos.MontoAReponer = Convert.ToDecimal(oDR["fpd_morep"]);
            oFallos.PendienteFacturacion = 'S';
            oFallos.PendienteNC = 'S';
            if (tipo == Operacion.enmTipo.enmFallos)
            {
                oFallos.MontoAFacturar = Convert.ToDecimal(oDR["fpd_mofac"]);
                oFallos.MontoAAnularPorNC = Convert.ToDecimal(oDR["fpd_moanu"]);

                if (!(oDR["fpg_numer"] is  DBNull))
                    oFallos.ClaveRegistroMocaja = Convert.ToInt32(oDR["fpg_numer"]);

                if (!(oDR["fpg_itmid"] is DBNull))
                    oFallos.IdDeItmFC = Convert.ToInt32(oDR["fpg_itmid"]);

                if (!(oDR["fpg_factu"] is DBNull))
                    oFallos.NumeroFactura = Convert.ToInt32(oDR["fpg_factu"]);

                if (!(oDR["fpg_itmnc"] is DBNull))
                    oFallos.IdDeItmNC = Convert.ToInt32(oDR["fpg_itmnc"]);

                if (!(oDR["fpg_ncnum"] is DBNull))
                    oFallos.NumeroNC = Convert.ToInt32(oDR["fpg_ncnum"]);

                if(!(oDR["fpg_pendi"] is DBNull ))
                    oFallos.PendienteFacturacion = Convert.ToChar(oDR["fpg_pendi"]);
                if (!(oDR["fpg_pennc"] is DBNull))
                    oFallos.PendienteNC = Convert.ToChar(oDR["fpg_pennc"]);
            }
            else
            {
                oFallos.MontoAFacturar = Convert.ToDecimal(oDR["fpd_movio"]);
                oFallos.MontoAAnularPorNC = 0;

                if (!(oDR["fpg_itmidv"] is DBNull))
                    oFallos.IdDeItmFC = Convert.ToInt32(oDR["fpg_itmidv"]);

                if (!(oDR["fpg_factuv"] is DBNull))
                    oFallos.NumeroFactura = Convert.ToInt32(oDR["fpg_factuv"]);

                if (!(oDR["fpg_pendiv"] is DBNull))
                    oFallos.PendienteFacturacion = Convert.ToChar(oDR["fpg_pendiv"]);
            }
            oFallos.FechaPedido = Convert.ToDateTime(oDR["fpd_fecha"]);
            oFallos.Anulado = Convert.ToChar(oDR["fpd_anula"]);

            oFallos.oEstacion = new Estacion(Convert.ToInt16(oDR["fpd_coest"]), Convert.ToString(oDR["est_nombr"]));
            
            oFallos.DescripcionVenta = Convert.ToString(oDR["DescripcionVenta"]);


            oFallos.Supervisor = Convert.ToString(oDR["fpg_supid"]);

            oFallos.oSupervisor = new Usuario(Convert.ToString(oDR["fpg_supid"]), Convert.ToString(oDR["use_nombr"]));


            return oFallos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Fallos facturados en una factura
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oFactura">Factura - Factura</param>
        /// <param name="tipo">Operacion.enmTipo - Tipo (Fallo o Violacion)</param>
        /// <returns>Lista de Items de de una Factura</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getFallosFacturados(Conexion oConn, Factura oFactura, Operacion.enmTipo tipo)
        {
            FacturaItemL oFallos = new FacturaItemL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_FallosAnteriores_getFacturados";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = oFactura.PuntoVenta;
                oCmd.Parameters.Add("tipitm", SqlDbType.Int).Value = tipo;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oFallos.Add(CargarItemFallo(oDR, tipo));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oFallos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem (Fallo)
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Fallos</param>
        /// <returns>Objeto Item de Factura correspondiente a un fallo</returns>
        /// ***********************************************************************************************
        private static FacturaItem CargarItemFallo(System.Data.IDataReader oDR, Operacion.enmTipo tipo)
        {
            FacturaItem oItem = FacturacionDt.CargarItem(oDR);

            oItem.Operacion = (Operacion)CargarFallo(oDR, tipo);

            return oItem;
        }

        #endregion

        #region FACTURAS: Clase de Datos de Facturas


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Facturas de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="parte">int - Numero de Parte</param>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getFacturasParte(Conexion oConn,
                                               byte estacion, int parte)
        {
            return getFacturas(oConn, estacion, null, null, null, parte, null, null, null, null, null, false, null);

        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Facturas 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">byte? - Numero de Estacion</param>
        /// <param name="tipoFactura">string - Tipo de Factura</param>
        /// <param name="serie">string - Serie de la Factura</param>
        /// <param name="numero">int? - Numero de la Factura</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="cliente">string - Nombre del Cliente (parcial)</param>
        /// <param name="excluirParte">int? - Numero de Parte a Excluir</param>
        /// <param name="ultimas">int? - Cantidad de Facturas a Traer</param>
        /// <param name="desde">DateTime? - Fecha Desde</param>
        /// <param name="hasta">DateTime? - Fecha Hasta</param>
        /// <param name="impagas">bool - true para traer facturas impagas</param>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getFacturas(Conexion oConn,
                                               byte? estacion, string tipoFactura, string serie, int? numero,
                                               int? parte,
                                               string cliente, int? excluirParte, int? ultimas, DateTime? desde, DateTime? hasta,
                                               bool impagas, string puntoVenta)
        {
            FacturaL oFacturas = new FacturaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getFacturas";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = tipoFactura;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = serie;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = numero;
                oCmd.Parameters.Add("Parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("Cliente", SqlDbType.VarChar, 50).Value = cliente;
                oCmd.Parameters.Add("ExcluirParte", SqlDbType.Int).Value = excluirParte;
                oCmd.Parameters.Add("Ultimas", SqlDbType.Int).Value = ultimas;
                oCmd.Parameters.Add("FechaDesde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("FechaHasta", SqlDbType.DateTime).Value = hasta;
                if (impagas)
                    oCmd.Parameters.Add("ACuenta", SqlDbType.Char, 1).Value = "S";
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = puntoVenta;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oFacturas.Add(CargarFactura(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oFacturas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una factura por el numero
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="tipoFactura">string - Tipo de Factura</param>
        /// <param name="serie">string - Serie de la Factura</param>
        /// <param name="numero">int - Numero de la Factura</param>
        /// <returns>Objeto Factura</returns>
        /// ***********************************************************************************************
        public static Factura getFactura(Conexion oConn,
                                               byte estacion, string tipoFactura, string serie, int numero, string puntoVenta)
        {
            Factura oFactura = null;
            FacturaL oFacturas = getFacturas(oConn, estacion, tipoFactura, serie, numero, null, null, null, null, null, null, false, puntoVenta);
            if (oFacturas.Count > 0)
                oFactura = oFacturas[0];

            return oFactura;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Factura
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Facturas</param>
        /// <returns>Objeto Factura con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static Factura CargarFactura(System.Data.IDataReader oDR)
        {
            Factura oFactura = new Factura();
            oFactura.Identity = (int)oDR["fac_ident"];
            oFactura.NumeroFactura = (int)oDR["fac_numer"];
            oFactura.Parte = new Parte((int)oDR["fac_parte"], DateTime.Today, 0);
            oFactura.Estacion = new Estacion((byte)oDR["fac_coest"], "");
            oFactura.Parte.Estacion = oFactura.Estacion;
            oFactura.Cliente = new Cliente((int)oDR["fac_numcl"], oDR["fac_nombr"].ToString(), oDR["fac_domic"].ToString());
            oFactura.Cliente.Domicilio = oDR["fac_domic"].ToString();
            oFactura.Cliente.Telefono = oDR["fac_telef"].ToString();
            oFactura.Cliente.Localidad = oDR["fac_local"].ToString();
            if (oDR["fac_provi"] != DBNull.Value)
                oFactura.Cliente.Provincia = new Provincia((int)oDR["fac_provi"], oDR["pro_descr"].ToString());
            oFactura.Cliente.TipoDocumento = new TipoDocumento(0, oDR["fac_tidoc"].ToString());
            oFactura.Cliente.NumeroDocumento = oDR["fac_docum"].ToString();
            if (oDR["fac_tiiva"] != DBNull.Value)
                oFactura.Cliente.TipoIVA = new TipoIVA((byte)oDR["fac_tiiva"], oDR["tip_descr"].ToString());


            oFactura.FechaGeneracion = (DateTime)oDR["fac_fecha"];
            oFactura.MontoTotal = (decimal)oDR["fac_monto"];
            oFactura.MontoNeto = (decimal)oDR["fac_ineto"];
            oFactura.MontoIVA = (decimal)oDR["fac_iva"];
            if (oDR["fac_reten"] != DBNull.Value)
                oFactura.MontoRetencion = (decimal)oDR["fac_reten"];
            if (oDR["fac_reteb"] != DBNull.Value)
                oFactura.MontoRetencionBienes = (decimal)oDR["fac_reteb"];
            //TODO Obtener de la Tabla
            oFactura.MontoACobrar = oFactura.MontoTotal - oFactura.MontoRetencion - oFactura.MontoRetencionBienes;

            oFactura.Observacion = oDR["fac_obser"].ToString();
            oFactura.PuntoVenta = oDR["fac_puvta"].ToString();
            oFactura.Serie = oDR["fac_serie"].ToString();
            oFactura.TipoFactura = oDR["fac_tipfa"].ToString();
            oFactura.TipoFacturaDescr = oDR["tif_descr"].ToString();

            oFactura.Anulada = (oDR["fac_anula"].ToString() == "S");
            oFactura.CobroACuenta = (oDR["fac_acuent"].ToString() == "S");
            oFactura.FacturaReemplazoTicket = (oDR["fac_reemp"].ToString() == "S");
            oFactura.NotaCredito = (oDR["fac_notcr"].ToString() == "S");

            //Si tiene un cobro a cuenta registrado está cobrada
            oFactura.Cobrada = (oDR["cob_monto"] != DBNull.Value && (decimal)oDR["cob_monto"] > 0);
            
            //Dmitriy: 14/04/2011
            oFactura.Establecimiento = (int)oDR["fac_estab"];
            if (oDR["fac_numausri"] != DBNull.Value)
                oFactura.NumeroAutorizacionSRI = oDR["fac_numausri"].ToString();

            oFactura.FechaInicioSRI = (DateTime)oDR["fac_fechasri"];
            oFactura.FechaCaducidadSRI = (DateTime)oDR["fac_vencisri"];
           
            return oFactura;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la factura para imprimir
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="tipoFactura">string - Tipo de Factura</param>
        /// <param name="serie">string - Serie de la Factura</param>
        /// <param name="numero">int - Numero de la Factura</param>
        /// <returns>DataSet con todo el detalle de la Factura</returns>
        /// ***********************************************************************************************
        public static DataSet getFacturaDetalle(Conexion oConn,
                                               byte estacion, string tipoFactura, string serie, string puvta, int numero)
        {
            DataSet dsFactura = new DataSet();
            dsFactura.DataSetName = "Factura_DetalleDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getFacturaDetalle";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = tipoFactura;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = serie;
                oCmd.Parameters.Add("puvta", SqlDbType.VarChar).Value = puvta;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = numero;


                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsFactura, "DetalleFactura");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsFactura;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Clientes a los que falta Facturar
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de Parte</param>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientesNoFacturados(Conexion oConn,
                                                int parte, char IncluyeFallos)
        {
            ClienteL oClientes = new ClienteL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getClientesNoFacturados";
                oCmd.Parameters.Add("Parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("IncluyeFallos", SqlDbType.Char, 1).Value = IncluyeFallos;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oClientes.Add(CargarCliente(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oClientes;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Cliente
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Clientes</param>
        /// <returns>Objeto Cliente con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static Cliente CargarCliente(System.Data.IDataReader oDR)
        {
            Cliente oCliente = null;
            if (oDR["cli_numcl"] != DBNull.Value)
            {
                oCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());
                oCliente.Email = oDR["cli_email"].ToString();
                oCliente.Expediente = oDR["cli_exped"].ToString();
                oCliente.Localidad = oDR["cli_local"].ToString();
                oCliente.Provincia = new Provincia((int)oDR["cli_provi"], oDR["pro_descr"].ToString());
                oCliente.Telefono = oDR["cli_telef"].ToString();
                oCliente.TipoDocumento = new TipoDocumento((int)oDR["cli_tidoc"], oDR["tid_descr"].ToString());
                oCliente.NumeroDocumento = oDR["cli_docum"].ToString();
            }
            else
            {
                oCliente = new Cliente(0, Traduccion.Traducir("CONSUMIDOR FINAL"), null);
            }


            return oCliente;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el proximo numero de factura
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Numero de Estacion</param>
        /// <param name="tipoFactura">TipoFactura - Tipo de Estacion</param>
        /// <param name="impresora">int - Numero de Impresora</param>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static int getNumeroFactura(Conexion oConn,
                                                int estacion, TipoFactura tipoFactura, int impresora)
        {
            int numero = 1;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getNumeracionFactura";
                oCmd.Parameters.Add("coest", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("impresora", SqlDbType.Int).Value = impresora;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = tipoFactura.Codigo;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    numero = (int)oDR["nmv_numfa"] + 1;
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return numero;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem
        /// Publico porque lo usan las distintas ventas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de ItmFac</param>
        /// <returns>Objeto FacturaItem con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        public static FacturaItem CargarItem(System.Data.IDataReader oDR)
        {
            FacturaItem oItem = new FacturaItem();
            oItem.Identity = (int)oDR["itm_ident"];
            oItem.NumeroFactura = (int)oDR["itm_factu"];
            oItem.Estacion = new Estacion((byte)oDR["itm_coest"], "");
            oItem.PuntoVenta = oDR["itm_puvta"].ToString();
            oItem.Serie = oDR["itm_serie"].ToString();
            oItem.TipoFactura = oDR["itm_tipfa"].ToString();


            return oItem;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Registra una nueva venta
        /// Actualiza los ultimos numeros de factura
        /// y los totales vendidos (por si hay cierre Z)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oTerminal">TerminalFacturacion - Objeto con la informacion de la terminal de facturacion</param>
        /// <param name="oFactura">Factura - Objeto con la informacion de la factura a agregar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addVenta(Conexion oConn, TerminalFacturacion oTerminal, Factura oFactura)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_RegistrarVenta";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("@terminal", SqlDbType.Int).Value = oTerminal.Numero;
                oCmd.Parameters.Add("@seriePR", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("@factuPR", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("@Impor", SqlDbType.Money).Value = oFactura.MontoTotal;
                oCmd.Parameters.Add("@TotIva", SqlDbType.Money).Value = oFactura.MontoIVA;
                oCmd.Parameters.Add("@impresora", SqlDbType.Int).Value = oTerminal.ImpresoraFacturacion.Codigo;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;

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
        /// ***********************************************************************************************
        /// <summary>
        /// Registra una nueva venta
        /// Actualiza los ultimos numeros de nota credito
        /// y los totales vendidos (por si hay cierre Z)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oTerminal">TerminalFacturacion - Objeto con la informacion de la terminal de facturacion</param>
        /// <param name="oNotaCredito">NotaCredito - Objeto con la informacion de la nota de credito a agregar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addVenta(Conexion oConn, TerminalFacturacion oTerminal, NotaCredito oNotaCredito)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_RegistrarVenta";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oNotaCredito.Estacion.Numero;
                oCmd.Parameters.Add("@terminal", SqlDbType.Int).Value = oTerminal.Numero;
                oCmd.Parameters.Add("@seriePR", SqlDbType.Char, 1).Value = oNotaCredito.Serie;
                oCmd.Parameters.Add("@factuPR", SqlDbType.Int).Value = oNotaCredito.NumeroNC;
                oCmd.Parameters.Add("@Impor", SqlDbType.Money).Value = oNotaCredito.MontoTotal;
                oCmd.Parameters.Add("@TotIva", SqlDbType.Money).Value = oNotaCredito.MontoIVA;
                oCmd.Parameters.Add("@impresora", SqlDbType.Int).Value = oTerminal.ImpresoraFacturacion.Codigo;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = "C";           // Codigo de comprobante de Nota de Credito

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

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Factura (Cabecera)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oFactura">Factura - Objeto con la informacion de la factura a agregar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addFactura(Conexion oConn, Factura oFactura)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_GuardaFactura";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("@seriePR", SqlDbType.Char, 1).Value = oFactura.SeriePreimpresa;
                oCmd.Parameters.Add("@numerPR", SqlDbType.Int).Value = oFactura.NumeroFacturaPreimpresa;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oFactura.PuntoVenta;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oFactura.Parte.Numero;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oFactura.FechaGeneracion;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = oFactura.Cliente.NumeroCliente;
              
                // El cliente puede ser nulo:
                if (oFactura.Cliente.TipoDocumento != null)
                    oCmd.Parameters.Add("@Tidoc", SqlDbType.VarChar, 10).Value = oFactura.Cliente.TipoDocumento.Descripcion; //En la factura se guarda la descripcion
                else
                    oCmd.Parameters.Add("@Tidoc", SqlDbType.VarChar, 10).Value = null;

                //Dmitriy: 15/04/2011
                oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = oFactura.Cliente.NumeroDocumento;
                oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 30).Value = oFactura.Cliente.RazonSocial;
                oCmd.Parameters.Add("@Domic", SqlDbType.VarChar, 50).Value = oFactura.Cliente.Domicilio;
                oCmd.Parameters.Add("@Telef", SqlDbType.VarChar, 15).Value = oFactura.Cliente.Telefono;
                //oCmd.Parameters.Add("@Anula", SqlDbType.Char, 1).Value = oFactura.Anulada;
                oCmd.Parameters.Add("@Obser", SqlDbType.VarChar, 3000).Value = oFactura.Observacion;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oFactura.MontoTotal;
                oCmd.Parameters.Add("@Ineto", SqlDbType.Money).Value = oFactura.MontoNeto;
                oCmd.Parameters.Add("@Iva", SqlDbType.Money).Value = oFactura.MontoIVA;
                oCmd.Parameters.Add("@Reemp", SqlDbType.Char, 1).Value = oFactura.FacturaReemplazoTicket ? "S" : "N";
                oCmd.Parameters.Add("@Reten", SqlDbType.Money).Value = oFactura.MontoRetencion;
                oCmd.Parameters.Add("@ReteB", SqlDbType.Money).Value = oFactura.MontoRetencionBienes;
                oCmd.Parameters.Add("@ACuenta", SqlDbType.Char, 1).Value = oFactura.CobroACuenta ? "S" : "N";

                // El cliente puede ser nulo:
                if (oFactura.Cliente.Provincia != null)
                    oCmd.Parameters.Add("@Provi", SqlDbType.Int).Value = oFactura.Cliente.Provincia.Codigo;
                else
                    oCmd.Parameters.Add("@Provi", SqlDbType.Int).Value = null;

                oCmd.Parameters.Add("@Local", SqlDbType.VarChar, 30).Value = oFactura.Cliente.Localidad;

                    // El cliente puede ser nulo:
                if (oFactura.Cliente.TipoIVA != null)
                    oCmd.Parameters.Add("@TiIva", SqlDbType.TinyInt).Value = oFactura.Cliente.TipoIVA.Codigo;
                else
                    oCmd.Parameters.Add("@TiIva", SqlDbType.TinyInt).Value = null;


                /******
                 *  Dmitriy: 15/04/2011
                 * */
                //@Establ,        /* numero de establecimiento */
				//@Numausri ,     /* Numero de Autorizacion SRI */
				//@Vencisri ,	  /* Fecha de vencimiento de la SRI */
				//@Fechasri 	  /* Fecha de inicio del cambio de Autorizacion */
               
                if(oFactura.Establecimiento!= null)
                    oCmd.Parameters.Add("@Estab",  SqlDbType.TinyInt).Value = oFactura.Establecimiento;                         // numero de establecimiento 
                else
                    oCmd.Parameters.Add("@Estab", SqlDbType.TinyInt).Value = null;                        
             
                if(oFactura.NumeroAutorizacionSRI != null)
                    oCmd.Parameters.Add("@Numausri", SqlDbType.VarChar , 10).Value =  oFactura.NumeroAutorizacionSRI;		    // Numero de Autorizacion SRI 
                else
                    oCmd.Parameters.Add("@Numausri", SqlDbType.VarChar, 10).Value = null;		    

                if(oFactura.FechaInicioSRI != null)
                    oCmd.Parameters.Add("@Vencisri", SqlDbType.DateTime).Value = oFactura.FechaCaducidadSRI;		            // Fecha de vencimiento de la SRI 
                else
                    oCmd.Parameters.Add("@Vencisri", SqlDbType.DateTime).Value = null;		
                if(oFactura.FechaCaducidadSRI != null)
                    oCmd.Parameters.Add("@Fechasri", SqlDbType.DateTime).Value = oFactura.FechaInicioSRI;	                    // Fecha de inicio del cambio de Autorizacion 
                else
                    oCmd.Parameters.Add("@Fechasri", SqlDbType.DateTime).Value = null;	                  
                
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

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Item de la Factura (detalle)
        /// Le asigna el Identity al item
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oItem">FacturaItem - Objeto con la informacion del item de la factura a agregar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addFacturaItem(Conexion oConn, FacturaItem oItem)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_GuardaItems";

                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oItem.TipoFactura;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oItem.PuntoVenta;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 1).Value = oItem.Serie;
                oCmd.Parameters.Add("@factu", SqlDbType.Int).Value = oItem.NumeroFactura;
                oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 50).Value = oItem.DescripcionVenta;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oItem.Monto;
                oCmd.Parameters.Add("@Tiven", SqlDbType.Int).Value = (int)oItem.TipoOperacion;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oItem.Estacion.Numero;
                oCmd.Parameters.Add("@Canti", SqlDbType.TinyInt).Value = oItem.Cantidad;
                oCmd.Parameters.Add("@MontoUni", SqlDbType.Money).Value = oItem.MontoUnitario;

                SqlParameter parIdentity = oCmd.Parameters.Add("@nIdent", SqlDbType.Int);
                parIdentity.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;
                if (parIdentity.Value != DBNull.Value)
                    oItem.Identity = Convert.ToInt32(parIdentity.Value);

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

        /// ***********************************************************************************************
        /// <summary>
        /// Anula una Factura 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oFactura">Factura - Objeto con la informacion de la factura a anular</param>
        /// <param name="porNC">bool - true si es por NC</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool anularFactura(Conexion oConn, Factura oFactura, bool porNC)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_Anular";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("@factur", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oFactura.PuntoVenta;
                oCmd.Parameters.Add("@notcr", SqlDbType.Char, 1).Value = porNC ? "S" : "N";


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

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Cobros A Cuenta de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="parte">int - Numero de Parte</param>
        /// <returns>Lista de Cobros A Cuenta</returns>
        /// ***********************************************************************************************
        public static CobroACuentaL getCobrosACuentaParte(Conexion oConn,
                                               byte estacion, int parte)
        {
            CobroACuentaL oCobros = new CobroACuentaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getCobrosACuentaPorParte";
                oCmd.Parameters.Add("Parte", SqlDbType.Int).Value = parte;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCobros.Add(CargarCobroACuenta(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCobros;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto CobroACuenta
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Cobros A Cuenta</param>
        /// <returns>Objeto Cobro A Cuenta con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static CobroACuenta CargarCobroACuenta(System.Data.IDataReader oDR)
        {
            CobroACuenta oCobro = new CobroACuenta();
            oCobro.FechaCobro = (DateTime)oDR["cob_fecha"];
            oCobro.Monto = (decimal)oDR["cob_monto"];
            if (oDR["cob_reten"] != DBNull.Value)
                oCobro.MontoRetencion = (decimal)oDR["cob_reten"];
            if (oDR["cob_reteb"] != DBNull.Value)
                oCobro.MontoRetencionBienes = (decimal)oDR["cob_reteb"];
            oCobro.Parte = new Parte((int)oDR["cob_parte"], DateTime.Today, 0);
            oCobro.Factura = CargarFactura(oDR);


            return oCobro;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Facturas a las que falta Cobrar
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getFacturasPendientes(Conexion oConn)
        {
            return getFacturas(oConn, null, null, null, null, null, null, null, null, null, null, true, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Cobro a Cuenta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCobroACuenta">CobroACuenta - Objeto con la informacion del Cobro A Cuenta</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addCobroACuenta(Conexion oConn, CobroACuenta oCobroACuenta)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_CobroFacturaCuenta";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oCobroACuenta.Factura.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oCobroACuenta.Factura.TipoFactura;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 1).Value = oCobroACuenta.Factura.Serie;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oCobroACuenta.Factura.NumeroFactura;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oCobroACuenta.Factura.PuntoVenta;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oCobroACuenta.Parte.Numero;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oCobroACuenta.FechaCobro;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oCobroACuenta.Monto;
                oCmd.Parameters.Add("@Reten", SqlDbType.Money).Value = oCobroACuenta.MontoRetencion;
                oCmd.Parameters.Add("@RetenB", SqlDbType.Money).Value = oCobroACuenta.MontoRetencionBienes;

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

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Cobro a Cuenta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCobroACuenta">CobroACuenta - Objeto con la informacion del Cobro A Cuenta</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delCobroACuenta(Conexion oConn, CobroACuenta oCobroACuenta)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_AnularCobroFacturaCuenta";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oCobroACuenta.Factura.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oCobroACuenta.Factura.TipoFactura;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 1).Value = oCobroACuenta.Factura.Serie;
                oCmd.Parameters.Add("@factu", SqlDbType.Int).Value = oCobroACuenta.Factura.NumeroFactura;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oCobroACuenta.Factura.PuntoVenta;

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
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Notas de Credito de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="parte">int - Numero de Parte</param>
        /// <returns>Lista de Notas de Credito</returns>
        /// ***********************************************************************************************
        public static NotaCreditoL getNotasCreditoParte(Conexion oConn,
                                               byte estacion, int parte)
        {
            return getNotasCredito(oConn, estacion, null, null, null, parte);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una Nota de Credito 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="serie">string - Serie de la NC</param>
        /// <param name="puntoventa">string - Punto de Venta de la NC</param>
        /// <param name="numero">int - Numero de la NC</param>
        /// <returns>Objeto Nota de Credito</returns>
        /// ***********************************************************************************************
        public static NotaCredito getNotaCredito(Conexion oConn,
                                               byte estacion, string serie, string puntoventa, int numero)
        {
            NotaCredito oNota = null;
            NotaCreditoL oNotas = getNotasCredito(oConn, estacion, serie, puntoventa, numero, null);
            if (oNotas.Count > 0)
                oNota = oNotas[0];

            return oNota;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Notas de Credito de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte? - Numero de Estacion</param>
        /// <param name="serie">string - Serie de la NC</param>
        /// <param name="puntoventa">string - Punto de Venta de la NC</param>
        /// <param name="numero">int? - Numero de la NC</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <returns>Lista de Notas de Credito</returns>
        /// ***********************************************************************************************
        public static NotaCreditoL getNotasCredito(Conexion oConn,
                                               byte? estacion, string serie, string puntoventa, int? numero, int? parte)
        {
            NotaCreditoL oNotas = new NotaCreditoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getNotasCredito";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("Serie", SqlDbType.Char, 1).Value = serie;
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = puntoventa;
                oCmd.Parameters.Add("numer", SqlDbType.Int).Value = numero;
                oCmd.Parameters.Add("Parte", SqlDbType.Int).Value = parte;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oNotas.Add(CargarNotaCredito(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oNotas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto NotaCredito
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Notas de Credito</param>
        /// <returns>Objeto Nota de Credito con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static NotaCredito CargarNotaCredito(System.Data.IDataReader oDR)
        {
            NotaCredito oNota = new NotaCredito();
            oNota.FechaGeneracion = (DateTime)oDR["ncr_fecha"];
            oNota.Estacion = new Estacion((byte)oDR["ncr_coest"], "");
            oNota.Identity = (int)oDR["ncr_ident"];
            oNota.MontoTotal = (decimal)oDR["ncr_monto"];
            oNota.MontoIVA = (decimal)oDR["ncr_iva"];
            oNota.MontoNeto = (decimal)oDR["ncr_ineto"];
            oNota.NumeroNC = (int)oDR["ncr_numer"];
            oNota.NumeroNCPreimpresa = (int)oDR["ncr_numpr"];
            oNota.Serie = oDR["ncr_serie"].ToString();
            oNota.SeriePreimpresa = oDR["ncr_serpr"].ToString();
            oNota.PuntoVenta = oDR["ncr_puvta"].ToString();
            oNota.Observacion = oDR["ncr_obser"].ToString();

            oNota.Anulada = (oDR["ncr_anula"].ToString() == "S");
            oNota.Cliente = new Cliente((int)oDR["ncr_numcl"], oDR["ncr_nombr"].ToString(), oDR["ncr_domic"].ToString());
            oNota.Cliente.Domicilio = oDR["ncr_domic"].ToString();
            oNota.Cliente.Telefono = oDR["ncr_telef"].ToString();
            oNota.Cliente.Localidad = oDR["fac_local"].ToString();
            if (oDR["fac_provi"] != DBNull.Value)
                oNota.Cliente.Provincia = new Provincia((int)oDR["fac_provi"], oDR["pro_descr"].ToString());
            oNota.Cliente.TipoDocumento = new TipoDocumento(0, oDR["ncr_tidoc"].ToString());
            oNota.Cliente.NumeroDocumento = oDR["ncr_docum"].ToString();
            if (oDR["fac_tiiva"] != DBNull.Value)
                oNota.Cliente.TipoIVA = new TipoIVA((byte)oDR["fac_tiiva"], oDR["tip_descr"].ToString());
            oNota.Parte = new Parte((int)oDR["ncr_parte"], DateTime.Today, 0);
            oNota.Factura = CargarFactura(oDR);


            return oNota;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve las ultimas facturas de un cliente exluyendo las del parte abierto
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="cliente">string - Nombre parcial del cliente</param>
        /// <param name="excluirParte">int - Numero de Parte a excluir</param>
        /// <param name="ultimas">int - Cantidad de Facturas a Devolver</param>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getUltimasFacturas(Conexion oConn,
                                               string cliente, int excluirParte, int ultimas)
        {
            return getFacturas(oConn, null, null, null, null, null, cliente, excluirParte, ultimas, null, null, false, null);

        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve las facturas de un cliente de las fechas pedidas exluyendo las del parte abierto
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="cliente">string - Nombre parcial del cliente</param>
        /// <param name="desde">DateTime - Fecha Desde</param>
        /// <param name="hasta">DateTime - Fecha Hasta</param>
        /// <param name="excluirParte">int - Numero de Parte a excluir</param>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getFacturasPorFecha(Conexion oConn,
                                               string cliente, DateTime desde, DateTime hasta, int excluirParte)
        {
            return getFacturas(oConn, null, null, null, null, null, cliente, excluirParte, null, desde, hasta, false, null);

        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la nota de credito para imprimir
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="serie">string - Serie de la NC</param>
        /// <param name="numero">int - Numero de la NC</param>
        /// <returns>DataSet con todo el detalle de la NC</returns>
        /// ***********************************************************************************************
        public static DataSet getNotaCreditoDetalle(Conexion oConn,
                                               byte estacion, string serie, int numero)
        {
            DataSet dsNotaCredito = new DataSet();
            dsNotaCredito.DataSetName = "NotaCredito_DetalleDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getNotaCreditoDetalle";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = serie;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = numero;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsNotaCredito, "DetalleNotaCredito");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsNotaCredito;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Nota de Credito
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oNotaCredito">NotaCredito - Objeto con la informacion de la NC</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addNotaCredito(Conexion oConn, NotaCredito oNotaCredito)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_GuardaNotaCredito";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oNotaCredito.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oNotaCredito.Factura.TipoFactura;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 1).Value = oNotaCredito.Serie;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oNotaCredito.NumeroNC;
                oCmd.Parameters.Add("@seriePR", SqlDbType.Char, 1).Value = oNotaCredito.SeriePreimpresa;
                oCmd.Parameters.Add("@numerPR", SqlDbType.Int).Value = oNotaCredito.NumeroNCPreimpresa;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oNotaCredito.PuntoVenta;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oNotaCredito.Parte.Numero;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oNotaCredito.FechaGeneracion;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = oNotaCredito.Cliente.NumeroCliente;
                oCmd.Parameters.Add("@Tidoc", SqlDbType.VarChar, 10).Value = oNotaCredito.Cliente.TipoDocumento.Descripcion; //En la factura se guarda la descripcion
                oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = oNotaCredito.Cliente.NumeroDocumento;
                oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 30).Value = oNotaCredito.Cliente.RazonSocial;
                oCmd.Parameters.Add("@Domic", SqlDbType.VarChar, 50).Value = oNotaCredito.Cliente.Domicilio;
                oCmd.Parameters.Add("@Telef", SqlDbType.VarChar, 15).Value = oNotaCredito.Cliente.Telefono;
                oCmd.Parameters.Add("@Obser", SqlDbType.VarChar, 3000).Value = oNotaCredito.Observacion;

                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oNotaCredito.MontoTotal;
                oCmd.Parameters.Add("@Ineto", SqlDbType.Money).Value = oNotaCredito.MontoNeto;
                oCmd.Parameters.Add("@Iva", SqlDbType.Money).Value = oNotaCredito.MontoIVA;
                oCmd.Parameters.Add("@facnum", SqlDbType.Int).Value = oNotaCredito.Factura.NumeroFactura;
                oCmd.Parameters.Add("@facserie", SqlDbType.Char, 1).Value = oNotaCredito.Factura.Serie;

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

        /// ***********************************************************************************************
        /// <summary>
        /// Anula una Nota de Credito
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oNotaCredito">NotaCredito - Objeto con la informacion de la NC</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool anularNotaCredito(Conexion oConn, NotaCredito oNotaCredito)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_AnularNotaCredito";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oNotaCredito.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oNotaCredito.Factura.TipoFactura;
                oCmd.Parameters.Add("@serie", SqlDbType.Char, 1).Value = oNotaCredito.Serie;
                oCmd.Parameters.Add("@factur", SqlDbType.Int).Value = oNotaCredito.Factura.NumeroFactura;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oNotaCredito.PuntoVenta;
                oCmd.Parameters.Add("@notcre", SqlDbType.Int).Value = oNotaCredito.NumeroNC;


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

        #region TIPOFACTURA: Clase de Datos de Tipos de Factura


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de Factura
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="tipo">String - tipo factura</param>
        /// <returns>Lista de Tipos de Factura</returns>
        /// ***********************************************************************************************
        public static TipoFacturaL getTiposFactura(Conexion oConn,
                                               string tipo)
        {
            TipoFacturaL oTipos = new TipoFacturaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getTiposFactura";
                oCmd.Parameters.Add("Codigo", SqlDbType.Char, 1).Value = tipo;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTipos.Add(CargarTipoFactura(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTipos;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto TipoFactura
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tipos de Factura</param>
        /// <returns>Objeto Factura con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoFactura CargarTipoFactura(System.Data.IDataReader oDR)
        {
            TipoFactura oTipo = new TipoFactura(oDR["tif_codig"].ToString(), oDR["tif_descr"].ToString());

            return oTipo;
        }
        #endregion

        #region TIPOITEMVENTA: Clase de Datos de Tipos de Items de Ventas

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de items de ventas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Tipos de Items de Venta</returns>
        /// ***********************************************************************************************
        public static TipoItemVentaL getTiposItemsVentas(Conexion oConn)
        {
            TipoItemVentaL oTipos = new TipoItemVentaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getTiposItemsVentas";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTipos.Add(CargarTiposItemsVentas(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTipos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto TipoItemVenta
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tipo Item Venta</param>
        /// <returns>Objeto TipoItemVenta con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoItemVenta CargarTiposItemsVentas(System.Data.IDataReader oDR)
        {
            TipoItemVenta oTipo = new TipoItemVenta((int)oDR["tii_codig"], oDR["tii_descr"].ToString());
            return oTipo;
        }

        #endregion
    }
}