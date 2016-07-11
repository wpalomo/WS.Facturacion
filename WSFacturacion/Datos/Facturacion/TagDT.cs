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
    public class TagDT
    {
        #region ENTREGATAGS: Clase de datos de Entrega de Tags

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Entregas de Tags NO facturadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="parte">int - Numero de parte por el cual buscar las operaciones pendientes</param>
        /// <param name="cliente">Cliente - Cliente</param>
        /// <returns>Lista de Entregas de Tags pendientes</returns>
        /// ***********************************************************************************************
        public static EntregaTagL getEntregasNoFacturadas(Conexion oConn, int parte, Cliente cliente, bool Todos)
        {
            EntregaTagL oEntregas = new EntregaTagL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_getNoFacturados";
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
            if (cliente != null)
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente.NumeroCliente;
            else
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = null;

            oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = null;
            oCmd.Parameters.Add("@Todos", SqlDbType.Char, 1).Value = Todos ? "S" : "N";

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oEntregas.Add(CargarEntrega(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oEntregas;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Entregas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Entregas</param>
        /// <returns>Lista con los elementos de entrega de tags de la base de datos</returns>
        /// ***********************************************************************************************
        private static EntregaTag CargarEntrega(IDataReader oDR)
        {
            // Creamos objetos auxiliares para componer el definitivo
            TipoCuenta oAuxTipoCuenta = new TipoCuenta((int)oDR["cta_tipcu"], oDR["tic_descr"].ToString());

            AgrupacionCuenta oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["cta_subfp"], oDR["ctg_descr"].ToString(), null, null, null, null, null, null);

            Cliente oAuxCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());

            Cuenta oAuxCuenta = new Cuenta((int)oDR["ent_cuent"],
                                           oAuxTipoCuenta,
                                           Util.DbValueToNullable<DateTime>(oDR["cta_feegr"]),
                                           oAuxAgrupacion,
                                           oDR["cta_descr"].ToString(),
                                           oDR["cta_delet"].ToString(),
                                           oAuxCliente);

            Parte oAuxParte = new Parte((int)oDR["ent_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);
            int? numExt = null;
            if (!String.IsNullOrEmpty(oDR["ent_nuext"].ToString()))
            {
                numExt = (int)oDR["ent_nuext"];
            }
            
            EntregaTag oEntrega = new EntregaTag((int)oDR["ent_ident"],
                                                new Estacion((byte)oDR["ent_coest"], ""),
                                                (DateTime)oDR["ent_fecha"],
                                                oAuxCuenta,
                                                null,
                                                oDR["ent_numer"].ToString(),
                                                oAuxParte,
                                                (int)oDR["ent_monto"],
                                                oDR["ent_anula"].ToString(),
                                                oDR["ent_abona"].ToString(),
                                                oDR["ent_paten"].ToString(),
                                                oDR["ent_mednu"].ToString(),
                                                oAuxCliente,
                                                oDR["tii_descr"].ToString(),
                                                oDR["ent_pospa"].ToString(),
                                                oDR["ent_habil"].ToString(),
                                                oDR["ent_habil"].ToString(),
                                                numExt);

            return oEntrega;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Entregas de Tags NO facturadas que coincidan con la patente y/o el tag
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="patente">string - Patente para la cual deseo saber si tiene entregas de tag pendiente de facturar</param>
        /// <param name="tag">string - Numero de tag para el cual deseo saber si tiene entregas de tag pendiente de facturar</param>
        /// <returns>Lista de Entregas de Tags pendientes para la patente y tag indicados</returns>
        /// ***********************************************************************************************
        public static EntregaTagL getEntregasNoFacturadasValid(Conexion oConn, string patente, string tag)
        {
            EntregaTagL oEntregas = new EntregaTagL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_getNoFacturadasValid";
            oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@nTag", SqlDbType.VarChar).Value = tag;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oEntregas.Add(CargarEntregaValid(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oEntregas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la Entrega de Tags Como un DataSet
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
            dsEntrega.DataSetName = "EntregaTag_DetalleDS";
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_getNoFacturados";
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
            if (cliente != null)
            {
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente.NumeroCliente;
            }
            if (Identity != null)
            {
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = Identity;
            }
            oCmd.Parameters.Add("@nCopias", SqlDbType.TinyInt).Value = cantidadCopias;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsEntrega, "DetalleEntregaTag");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
            return dsEntrega;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Entregas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Entregas</param>
        /// <returns>Lista con los elementos de entrega de tags de la base de datos</returns>
        /// ***********************************************************************************************
        private static EntregaTag CargarEntregaValid(IDataReader oDR)
        {
            Cliente oAuxCliente = new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString());

            int? numExt = null;
            if (!String.IsNullOrEmpty(oDR["ent_nuext"].ToString()))
            {
                numExt = (int)oDR["ent_nuext"];
            }

            EntregaTag oEntrega = new EntregaTag((int)oDR["ent_ident"],
                                                    new Estacion((byte)oDR["ent_coest"], ""),
                                                    (DateTime)oDR["ent_fecha"],
                                                    null,
                                                    null,
                                                    oDR["ent_numer"].ToString(),
                                                    null,
                                                    (int)oDR["ent_monto"],
                                                    oDR["ent_anula"].ToString(),
                                                    oDR["ent_abona"].ToString(),
                                                    oDR["ent_paten"].ToString(),
                                                    oDR["ent_mednu"].ToString(),
                                                    oAuxCliente,
                                                    oDR["tii_descr"].ToString(),
                                                    oDR["ent_pospa"].ToString(),
                                                    oDR["ent_habil"].ToString(),
                                                    oDR["ent_habil"].ToString(),
                                                    numExt);
            return oEntrega;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Entregas de Tags facturadas en una factura
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oFactura">Factura - Factura</param>
        /// <returns>Lista de Items de Entregas de Tags de una Factura</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getEntregasFacturadas(Conexion oConn, Factura oFactura)
        {
            FacturaItemL oEntregas = new FacturaItemL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_getFacturados";
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
            return oEntregas;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem (EntregaTag)
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Entregas</param>
        /// <returns>Objeto Item de Factura correspondiente a una entrega</returns>
        /// ***********************************************************************************************
        private static FacturaItem CargarItemEntrega(IDataReader oDR)
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
        /// <param name="oEntrega">EntregaTag - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarEntrega(Conexion oConn, EntregaTag oEntrega)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_Guardar";
            oCmd.Parameters.Add("@Coest", SqlDbType.Int).Value = oEntrega.Estacion.Numero;
            oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oEntrega.Cuenta.Numero;
            oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oEntrega.Parte.Numero;
            oCmd.Parameters.Add("@nTag", SqlDbType.VarChar).Value = oEntrega.NumeroTag;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oEntrega.FechaOperacion;
            oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = oEntrega.Monto;
            oCmd.Parameters.Add("@abona", SqlDbType.Char, 1).Value = oEntrega.Abona;
            oCmd.Parameters.Add("@Patente", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
            oCmd.Parameters.Add("@EsReposicion", SqlDbType.Char, 1).Value = oEntrega.Reposicion;
            oCmd.Parameters.Add("@EsPospago", SqlDbType.Char, 1).Value = oEntrega.Pospago;
            oCmd.Parameters.Add("@Nuext", SqlDbType.Int).Value = oEntrega.NumeroExterno;
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
                {
                    msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                //Devuelve el identity asignado
                oEntrega.Identity = retval;
            }
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
            EntregaTag oEntrega = oItem.Operacion.EntregaTag;
            if (oEntrega == null)
            {
                throw new InvalidCastException("No es una entrega de tags");
            }

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_FacturarItem";
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
                {
                    msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No se encontró el registro de la operación");
                }
                throw new ErrorSPException(msg);
            }

            bRet = true;
            return bRet;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba en la tabla de maestros la entrega realizada. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaTag - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GrabarEnMaestro(Conexion oConn, EntregaTag oEntrega)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_GrabaTags";
            oCmd.Parameters.Add("@nTag", SqlDbType.VarChar).Value = oEntrega.NumeroTag;
            oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
            oCmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = oEntrega.FechaOperacion;
            oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = oEntrega.Estacion.Numero;
            //oCmd.Parameters.Add("@Nuext", SqlDbType.Int).Value = oEntrega.NumeroExterno;

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
                {
                    msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina la entrega (no facturada) de la base de datos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaTag - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularEntrega(Conexion oConn, EntregaTag oEntrega)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_AnularEntrega";
            oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = oEntrega.Patente;
            oCmd.Parameters.Add("@nTag", SqlDbType.VarChar).Value = oEntrega.NumeroTag;
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
                {
                    msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina en la tabla de maestros la entrega realizada. Habilita la anterior
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEntrega">EntregaTag - Objeto de la entrega que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EliminarEnMaestro(Conexion oConn, EntregaTag oEntrega)
        {
            EliminarEnMaestro(oConn, oEntrega.NumeroTag, oEntrega.Patente, oEntrega.FechaOperacion, true);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina en la tabla de maestros un tag, no habilita la anterior. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oTag">Tag - Objeto del tag que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EliminarEnMaestro(Conexion oConn, Tag oTag)
        {
            EliminarEnMaestro(oConn, oTag.NumeroTag, oTag.Patente, null, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina en la tabla de maestros la entrega realizada. 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroTag">string - numero de tag/param>
        /// <param name="patente">string - patente del vehiculo</param>
        /// <param name="fechaOperacion">DateTime - fecha y hora de la operacion</param>
        /// <param name="habilita">bool - true si volvemos a habilitar el tag anterior</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EliminarEnMaestro(Conexion oConn, string numeroTag, string patente, DateTime? fechaOperacion, bool habilita)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_EliminarTag";
            oCmd.Parameters.Add("@nTag", SqlDbType.VarChar).Value = numeroTag;
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
                {
                    msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                }
                throw new ErrorSPException(msg);
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
        public static Tag getTagPatente(Conexion oConn, string patente)
        {
            Tag oTags = new Tag();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getTagPorPatente";
            oCmd.Parameters.Add("@paten", SqlDbType.NVarChar,10).Value = patente;

            oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTags = (CargarTag(oDR));
                }
            
            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTags;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader a la lista de tags
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tags </param>
        /// <returns>Objeto Tag</returns>
        /// ***********************************************************************************************
        private static Tag CargarTag(IDataReader oDR)
        {
            Tag oTag = new Tag(oDR["tag_pattg"].ToString(), oDR["tag_ntag"].ToString(), oDR["tag_emitg"].ToString(), 
                oDR["tag_baja"].ToString(), oDR["tag_accion"].ToString());

            return oTag;
        }
        #endregion
        
        #region TAGSLISTANEGRA: Clase de datos de Tags

        public static void addTagListaNegra(Conexion oConn, TagListaNegra oTagListaNegra)
        {
            //SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_ListaNegra_addTag";
            oCmd.Parameters.Add("@numer", SqlDbType.VarChar).Value = oTagListaNegra.NumeroTag;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oTagListaNegra.FechaInhabilitacion;
            oCmd.Parameters.Add("@usuar", SqlDbType.VarChar, 10).Value = oTagListaNegra.Usuario.ID;
            oCmd.Parameters.Add("@comen", SqlDbType.VarChar, 2000).Value = oTagListaNegra.Comentario;

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
                {
                    msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                }
                throw new ErrorSPException(msg);
            }
        }

        public static void delTagListaNegra(Conexion oConn, TagListaNegra oTagListaNegra)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_ListaNegra_delTag";
            oCmd.Parameters.Add("@numer", SqlDbType.VarChar).Value = oTagListaNegra.NumeroTag;
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
                {
                    msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                }
                throw new ErrorSPException(msg);
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
            SqlDataReader oDR;
            
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_getTagListaNegra";
            oCmd.Parameters.Add("@nTag", SqlDbType.VarChar).Value = numeroTag;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTagsLN.Add(CargarTagLN(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTagsLN;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Tags en Lista Negra
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tags en lista negra</param>
        /// <returns>Lista con los elementos de tags en lista negra de la base de datos</returns>
        /// ***********************************************************************************************
        private static TagListaNegra CargarTagLN(IDataReader oDR)
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
