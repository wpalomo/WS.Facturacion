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
    public class ChipDT
    {

        #region ENTREGACHIPS: Clase de datos de Entrega de Tarjetas Chips

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Entregas de Chips NO facturadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <param name="cliente">Cliente - Cliente</param>
        /// <returns>Lista de Entregas de Chips pendientes</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getEntregasNoFacturadas(Conexion oConn, int parte, Cliente cliente, bool Todos)
        {
            EntregaChipL oEntregas = new EntregaChipL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_getNoFacturados";
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
                if (cliente != null)
                    oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente.NumeroCliente;
                else
                    oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = null;

                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = null;
                oCmd.Parameters.Add("@Todos", SqlDbType.Char, 1).Value = Todos?"S":"N";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oEntregas.Add(CargarEntrega(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEntregas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Entregas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Entregas</param>
        /// <returns>Lista con los elementos de entrega de Chips de la base de datos</returns>
        /// ***********************************************************************************************
        private static EntregaChip CargarEntrega(System.Data.IDataReader oDR)
        {
            // Creamos objetos auxiliares para componer el definitivo
            TipoCuenta oAuxTipoCuenta = new TipoCuenta((int)oDR["cta_tipcu"], oDR["tic_descr"].ToString());

            AgrupacionCuenta oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["cta_subfp"], oDR["ctg_descr"].ToString(), null, null, null, null, null, null);

            Cliente oAuxCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());

            Cuenta oAuxCuenta = new Cuenta((int)oDR["ech_cuent"],
                                           oAuxTipoCuenta,
                                           Util.DbValueToNullable<DateTime>(oDR["cta_feegr"]),
                                           oAuxAgrupacion,
                                           oDR["cta_descr"].ToString(),
                                           oDR["cta_delet"].ToString(),
                                           oAuxCliente);

            Parte oAuxParte = new Parte((int)oDR["ech_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);

            int nuint = 0;
            if( oDR["ech_nuint"] != DBNull.Value )
                nuint = (int) oDR["ech_nuint"];
            EntregaChip oEntrega = new EntregaChip((int)oDR["ech_ident"],
                                                    new Estacion((byte)oDR["ech_coest"], ""),
                                                    (DateTime)oDR["ech_fecha"],
                                                    oAuxCuenta,
                                                    null,
                                                    oDR["ech_numer"].ToString(),
                                                    (int)oDR["ech_nuext"],
                                                    oAuxParte,
                                                    (int)oDR["ech_monto"],
                                                    oDR["ech_anula"].ToString(),
                                                    oDR["ech_abona"].ToString(),
                                                    oDR["ech_paten"].ToString(),
                                                    oDR["ech_mednu"].ToString(),
                                                    oAuxCliente,
                                                    oDR["tii_descr"].ToString(),
                                                    oDR["ech_pospa"].ToString(),
                                                    oDR["ech_habil"].ToString(),
                                                    oDR["ech_habil"].ToString(),
                                                    nuint
                                                );


            return oEntrega;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Entregas de Chips NO facturadas que coincidan con la patente y/o la tarjeta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="patente">string - Patente para la cual deseo saber si tiene entregas de Chips pendiente de facturar</param>
        /// <returns>Lista de Entregas de Chips pendientes para la patente y chip indicados</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getEntregasNoFacturadasValid(Conexion oConn, string patente)
        {
            EntregaChipL oEntregas = new EntregaChipL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_getNoFacturadasValid";
                oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = patente;
                //oCmd.Parameters.Add("@nChip", SqlDbType.Int).Value = numeroChip;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oEntregas.Add(CargarEntregaValid(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEntregas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Entregas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Entregas</param>
        /// <returns>Lista con los elementos de entrega de chips de la base de datos</returns>
        /// ***********************************************************************************************
        private static EntregaChip CargarEntregaValid(System.Data.IDataReader oDR)
        {
            Cliente oAuxCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());

            EntregaChip oEntrega = new EntregaChip((int)oDR["ech_ident"],
                                                    new Estacion((byte)oDR["ech_coest"], ""),
                                                    (DateTime)oDR["ech_fecha"],
                                                    null,
                                                    null,
                                                    oDR["ech_numer"].ToString(),
                                                    (int)oDR["ech_nuext"],
                                                    null,
                                                    (int)oDR["ech_monto"],
                                                    oDR["ech_anula"].ToString(),
                                                    oDR["ech_abona"].ToString(),
                                                    oDR["ech_paten"].ToString(),
                                                    oDR["ech_mednu"].ToString(),
                                                    oAuxCliente,
                                                    oDR["tii_descr"].ToString(),
                                                    oDR["ech_pospa"].ToString(),
                                                    oDR["ech_habil"].ToString(),
                                                    oDR["ech_habil"].ToString(),
                                                    (int)oDR["ech_nuint"]
                                                );


            return oEntrega;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la Entrega de Chip Como un DataSet
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <param name="cliente">Cliente - Cliente</param>
        /// <param name="Identity">int? - Identificador de la entrega</param>
        /// <returns>DataSet con la entrega</returns>
        /// ***********************************************************************************************
        public static DataSet getEntregaDetalle(Conexion oConn, int parte, Cliente cliente, int? Identity, int cantidadCopias)
        {
            DataSet dsEntrega = new DataSet();
            dsEntrega.DataSetName = "EntregaChip_DetalleDS";
            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_getNoFacturados";
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
                if (cliente != null)
                    oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente.NumeroCliente;
                if (Identity != null)
                    oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = Identity;
                oCmd.Parameters.Add("@nCopias", SqlDbType.TinyInt).Value = cantidadCopias;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsEntrega, "DetalleEntregaChip");
             
                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsEntrega;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Entregas de Chips facturadas en una factura
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oFactura">Factura - Factura</param>
        /// <returns>Lista de Items de Entregas de Chips de una Factura</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getEntregasFacturadas(Conexion oConn, Factura oFactura)
        {
            FacturaItemL oEntregas = new FacturaItemL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_getFacturados";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = oFactura.PuntoVenta;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oEntregas.Add(CargarItemEntrega(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEntregas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem (EntregaChip)
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Entregas</param>
        /// <returns>Objeto Item de Factura correspondiente a una entrega</returns>
        /// ***********************************************************************************************
        private static FacturaItem CargarItemEntrega(System.Data.IDataReader oDR)
        {
            FacturaItem oItem = FacturacionDt.CargarItem(oDR);

            oItem.Operacion = (Operacion)CargarEntrega(oDR);

            return oItem;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Graba el registro de la entrega en la base de datos. Aun no esta activa, hay un segundo paso para eso
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaChip - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarEntrega(Conexion oConn, EntregaChip oEntrega)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_Guardar";
                oCmd.Parameters.Add("@Coest", SqlDbType.Int).Value = oEntrega.Estacion.Numero;
                oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oEntrega.Cuenta.Numero;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oEntrega.Parte.Numero;
                oCmd.Parameters.Add("@nTarjeta", SqlDbType.VarChar, 12).Value = oEntrega.Dispositivo;
                oCmd.Parameters.Add("@Nuext", SqlDbType.Int).Value = oEntrega.NumeroExterno;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oEntrega.FechaOperacion;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oEntrega.Monto;
                oCmd.Parameters.Add("@abona", SqlDbType.Char, 1).Value = oEntrega.Abona;
                oCmd.Parameters.Add("@Patente", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
                oCmd.Parameters.Add("@EsReposicion", SqlDbType.Char, 1).Value = oEntrega.Reposicion;
                oCmd.Parameters.Add("@EsPospago", SqlDbType.Char, 1).Value = oEntrega.Pospago;
                oCmd.Parameters.Add("@Nuint", SqlDbType.Int).Value = oEntrega.NumeroInterno;

                SqlParameter parRetVal = oCmd.Parameters.Add("@nRet", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval < (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
                else
                {
                    //Devuelve el identity asignado
                    oEntrega.Identity = retval;
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
        /// Graba los datos de la factura en el registro de una entrega (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una entrega)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool FacturarEntrega(Conexion oConn, FacturaItem oItem)
        {
            return FacturarEntrega(oConn, oItem, false, false);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Anula los datos de la factura en el registro de una entrega (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una entrega)</param>
        /// <param name="anularNC">bool - true para anular la factura por Nota de Credito</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool AnularFacturaEntrega(Conexion oConn, FacturaItem oItem, bool porNC)
        {
            return FacturarEntrega(oConn, oItem, !porNC, porNC);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba los datos de la factura en el registro de una entrega (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una entrega)</param>
        /// <param name="anular">bool - true para anular la factura</param>
        /// <param name="anularNC">bool - true para anular la factura por Nota de Credito</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static bool FacturarEntrega(Conexion oConn, FacturaItem oItem, bool anular, bool anularNC)
        {
            bool bRet = false;
            try
            {
                EntregaChip oEntrega = oItem.Operacion.EntregaChip;
                if (oEntrega == null)
                    throw new InvalidCastException("No es una entrega de chips");

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_FacturarItem";
                if (!anular)
                {
                    oCmd.Parameters.Add("@Factu", SqlDbType.Int).Value = oItem.NumeroFactura;
                    oCmd.Parameters.Add("@ItmId", SqlDbType.Int).Value = oItem.Identity;
                }
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oEntrega.FechaOperacion;
                oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
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
        /// Graba en la tabla de maestros la entrega realizada. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaChip - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GrabarEnMaestro(Conexion oConn, EntregaChip oEntrega)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_GrabaChips";
                oCmd.Parameters.Add("@Numer", SqlDbType.VarChar, 12).Value = oEntrega.Dispositivo;
                oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
                oCmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = oEntrega.FechaOperacion;
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = oEntrega.Estacion.Numero;
                oCmd.Parameters.Add("@Nuext", SqlDbType.Int).Value = oEntrega.NumeroExterno;
                oCmd.Parameters.Add("@Nuint", SqlDbType.Int).Value = oEntrega.NumeroInterno;

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
        /// Elimina la entrega (no facturada) de la base de datos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaChip - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularEntrega(Conexion oConn, EntregaChip oEntrega)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_AnularEntrega";
                oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
                oCmd.Parameters.Add("@Numer", SqlDbType.VarChar, 12).Value = oEntrega.Dispositivo;
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = oEntrega.Identity;

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
        /// Elimina en la tabla de maestros la entrega realizada. Habilita la anterior
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaChip - Objeto de la entrega que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EliminarEnMaestro(Conexion oConn, EntregaChip oEntrega)
        {
            EliminarEnMaestro(oConn, oEntrega.Dispositivo, oEntrega.Patente, oEntrega.FechaOperacion, true);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina en la tabla de maestros una tarjeta chip, no habilita la anterior. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oChip">Chip - Objeto de la tarjeta que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EliminarEnMaestro(Conexion oConn, Chip oChip)
        {
            EliminarEnMaestro(oConn, oChip.Dispositivo, oChip.Patente, null, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina en la tabla de maestros la entrega realizada. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Dispositivo">string - numero de tarjeta/param>
        /// <param name="patente">string - patente del vehiculo</param>
        /// <param name="fechaOperacion">DateTime - fecha y hora de la operacion</param>
        /// <param name="habilita">bool - true si volvemos a habilitar la tarjeta anterior</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EliminarEnMaestro(Conexion oConn, string dispositivo, string patente, DateTime? fechaOperacion, bool habilita)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaChips_EliminarChip";
                oCmd.Parameters.Add("@nTarjeta", SqlDbType.VarChar, 12).Value = dispositivo;
                oCmd.Parameters.Add("@Patente", SqlDbType.VarChar, 8).Value = patente;
                oCmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = fechaOperacion;
                oCmd.Parameters.Add("@habilita", SqlDbType.Char, 1).Value = habilita ? "S" : "N";

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
        /// Devuelve el numero interno de la proxima tarjeta chip a generar
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Numero interno de la proxima tarjeta chip a generar</returns>
        /// ***********************************************************************************************
        public static int getProximoNumeroTarjeta(Conexion oConn)
        {
            int iNumeroTarjeta = 0;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.getNumeroTabla";
                oCmd.Parameters.Add("@tabla", SqlDbType.VarChar, 30).Value = "CHIPS";

                SqlParameter parNumeroTarjeta = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                parNumeroTarjeta.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();

                int retval = (int)parRetVal.Value;
                if (parNumeroTarjeta.Value != DBNull.Value)
                {
                    iNumeroTarjeta = Convert.ToInt32(parNumeroTarjeta.Value);
                }

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
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
            return iNumeroTarjeta;
        }

        #endregion


        #region VINCULACIONCHIPS: Clase de datos de Vinculacion de Tarjetas Chips

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Vinculaiones de Chips NO facturadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <returns>Lista de Vinculaciones de Chips pendientes</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getVinculacionesNoFacturadas(Conexion oConn, int parte)
        {
            EntregaChipL oEntregas = new EntregaChipL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VinculaChips_getNoFacturados";
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oEntregas.Add(CargarEntrega(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEntregas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Vinculaciones de Chips NO facturadas que coincidan con la patente y/o la tarjeta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="patente">string - Patente para la cual deseo saber si tiene vinculaciones de Chips pendiente de facturar</param>
        /// <param name="tag">string - Numero de tag para el cual deseo saber si tiene vinculaciones de Chips pendiente de facturar</param>
        /// <returns>Lista de Vinculaciones de Chips pendientes para la patente y chip indicados</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getVinculacionesNoFacturadasValid(Conexion oConn, string patente, int numeroChip)
        {
            EntregaChipL oEntregas = new EntregaChipL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_VinculacionChips_getNoFacturadasValid";
                oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = patente;
                oCmd.Parameters.Add("@nChip", SqlDbType.Int).Value = numeroChip;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oEntregas.Add(CargarEntregaValid(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEntregas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Graba el registro de la vinculacion en la base de datos. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaChip - Objeto de la vinculacion que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarVinculacion(Conexion oConn, EntregaChip oEntrega)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "usp_VinculaChips_Guardar";
                oCmd.Parameters.Add("Coest", SqlDbType.Int).Value = oEntrega.Estacion.Numero;
                oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oEntrega.Cuenta.Numero;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oEntrega.Parte.Numero;
                oCmd.Parameters.Add("@nTarjeta", SqlDbType.VarChar, 12).Value = oEntrega.Dispositivo;
                oCmd.Parameters.Add("@Nuext", SqlDbType.Int).Value = oEntrega.NumeroExterno;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oEntrega.FechaOperacion;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oEntrega.Monto;
                oCmd.Parameters.Add("@abona", SqlDbType.Char, 1).Value = oEntrega.Abona;
                oCmd.Parameters.Add("@Patente", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
                oCmd.Parameters.Add("@dispositivo", SqlDbType.VarChar, 20).Value = oEntrega.Dispositivo;

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
        /// Elimina la vinculacion (no facturada) de la base de datos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaChip - Objeto de la vinculacion que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularVinculacion(Conexion oConn, EntregaChip oEntrega)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "usp_VinculaChips_AnularEntrega";
                oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oEntrega.Cuenta.Numero;
                oCmd.Parameters.Add("@Nuext", SqlDbType.Int).Value = oEntrega.NumeroExterno;

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


        #region CHIPSLISTANEGRA: Clase de datos de Chips

        public static void addChipListaNegra(Conexion oConn, ChipListaNegra oChipListaNegra)
        {
            try
            {
                //SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ListaNegra_addChip";
                oCmd.Parameters.Add("@numer", SqlDbType.VarChar, 12).Value = oChipListaNegra.NumeroChip;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oChipListaNegra.FechaInhabilitacion;
                oCmd.Parameters.Add("@usuar", SqlDbType.VarChar, 10).Value = oChipListaNegra.Usuario.ID;
                oCmd.Parameters.Add("@comen", SqlDbType.VarChar, 2000).Value = oChipListaNegra.Comentario;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval < (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
                else
                {
                    //Devuelve el identity asignado
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static void delChipListaNegra(Conexion oConn, ChipListaNegra oChipListaNegra)
        {
            try
            {
                //SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ListaNegra_delChip";
                oCmd.Parameters.Add("@numer", SqlDbType.VarChar, 12).Value = oChipListaNegra.NumeroChip;
                //oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oTagListaNegra.FechaInhabilitacion;
                //oCmd.Parameters.Add("@usuar", SqlDbType.VarChar, 10).Value = oTagListaNegra.Usuario.ID;
                //oCmd.Parameters.Add("@comen", SqlDbType.VarChar, 2000).Value = oTagListaNegra.Comentario;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval < (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
                else
                {
                    //Devuelve el identity asignado
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tags
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroTag">string - Numero de Tag por el cual buscar</param>
        /// <returns>Lista de Tags en lista negra que coinciden con el buscado</returns>
        /// ***********************************************************************************************
        public static TagListaNegraL getTagListaNegra(Conexion oConn, string numeroTag)
        {
            TagListaNegraL oTagsLN = new TagListaNegraL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_EntregaTags_getTagListaNegra";
                oCmd.Parameters.Add("@nTag", SqlDbType.VarChar, 12).Value = numeroTag;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTagsLN.Add(CargarTagLN(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTagsLN;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Tags en Lista Negra
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tags en lista negra</param>
        /// <returns>Lista con los elementos de tags en lista negra de la base de datos</returns>
        /// ***********************************************************************************************
        private static TagListaNegra CargarTagLN(System.Data.IDataReader oDR)
        {
            TagListaNegra oTagLN = new TagListaNegra(oDR["lnt_numer"].ToString(),
                                                     (DateTime)oDR["lnt_fecha"],
                                                     new Usuario(oDR["lnt_usuar"].ToString(), ""),
                                                     oDR["lnt_comen"].ToString());

            return oTagLN;
        }

        #endregion

    }
}
