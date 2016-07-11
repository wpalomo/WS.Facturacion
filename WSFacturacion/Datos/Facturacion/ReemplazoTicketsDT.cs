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
    public class ReemplazoTicketsDT
    {

        #region REEMPLAZO DE TICKETS OBTENER LISTAS


    
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Items de las Facturas con reemplazo de Tickets Facturados
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oFactura">Factura - Factura</param>
        /// <returns>Lista de Items de Reemplazo de Tickets de una Factura</returns>
        /// ***********************************************************************************************
        public static ReemplazoTicketsL getReemplazoTicketsNoFacturados(Conexion oConn, int parte, Cliente cliente)
        {
            ReemplazoTicketsL oReemplazo = new ReemplazoTicketsL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_getItemsNoFacturados";
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente.NumeroCliente;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oReemplazo.Add(CargarItemReemplazoNoFacturados(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oReemplazo;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem
        /// Publico porque lo usan las distintas ventas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de ItmFac</param>
        /// <returns>Objeto FacturaItem con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        public static FacturaItem CargarItemTicketNoFacturado(System.Data.IDataReader oDR)
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
        /// Carga un elemento DataReader en un objeto FacturaItem (ValePrepagoVenta)
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Venta de Vales</param>
        /// <returns>Objeto Item de Factura correspondiente a una venta de vales</returns>
        /// ***********************************************************************************************
        private static ReemplazoTickets CargarItemReemplazoNoFacturados(System.Data.IDataReader oDR)
        {
            ReemplazoTickets oItem = ReemplazoTicketsDT.CargarReemplazoTickets(oDR);
            return oItem;
        }




        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Items de las Facturas con reemplazo de Tickets Facturados
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oFactura">Factura - Factura</param>
        /// <returns>Lista de Items de Reemplazo de Tickets de una Factura</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getReemplazoTicketsFacturados(Conexion oConn, Factura oFactura)
        {
            FacturaItemL OReemplazo = new FacturaItemL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_getItems";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = oFactura.Estacion.Numero;
                oCmd.Parameters.Add("tipfa", SqlDbType.Char, 1).Value = oFactura.TipoFactura;
                oCmd.Parameters.Add("serie", SqlDbType.Char, 1).Value = oFactura.Serie;
                oCmd.Parameters.Add("numero", SqlDbType.Int).Value = oFactura.NumeroFactura;
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = oFactura.PuntoVenta;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    OReemplazo.Add(CargarItemReemplazo(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OReemplazo;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem (ValePrepagoVenta)
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Venta de Vales</param>
        /// <returns>Objeto Item de Factura correspondiente a una venta de vales</returns>
        /// ***********************************************************************************************
        private static FacturaItem CargarItemReemplazo(System.Data.IDataReader oDR)
        {
            FacturaItem oItem = FacturacionDt.CargarItem(oDR);

            oItem.Operacion = (Operacion)CargarReemplazoTickets(oDR);

            return oItem;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Reemplazo de Tickets
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="parte">int - Numero de Parte</param>
        /// <returns>Lista de Cobros A Cuenta</returns>
        /// ***********************************************************************************************
        public static ReemplazoTicketsL getReemplazoTicketParte(Conexion oConn,byte estacion, int parte)
        {
            ReemplazoTicketsL oCobros = new ReemplazoTicketsL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_getNoFacturados";
                oCmd.Parameters.Add("Parte", SqlDbType.Int).Value = parte;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCobros.Add(CargarReemplazoTickets(oDR));
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
        /// Carga un elemento DataReader en un objeto ReemplazoTickets
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Cobros A Cuenta</param>
        /// <returns>Objeto Cobro A Cuenta con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static ReemplazoTickets CargarReemplazoTickets(System.Data.IDataReader oDR)
        {

            Parte oAuxParte = new Parte((int)oDR["ree_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);

            ReemplazoTickets oReemplazo = new ReemplazoTickets((int)oDR["ree_ident"],
                                                                    new Estacion((byte)oDR["ree_coest"], ""),
                                                                    (DateTime)oDR["ree_fecha"],
                                                                    null,
                                                                    null,
                                                                    oAuxParte,
                                                                    (decimal)oDR["ree_monto"],
                                                                    oDR["ree_anula"].ToString(),
                                                                    oDR["tii_descr"].ToString(),
                                                                    new Cliente((int)oDR["cli_numcl"], oDR["cli_nombr"].ToString(), oDR["cli_domic"].ToString())
                                   );

            //if (oDR["ree_factu"] != DBNull.Value)
            //oReemplazo.NumeroFactura =  Convert.ToInt32(oDR["ree_factu"]);


            return oReemplazo;
        }



        #endregion


        public static int getDiasCambio(int p)
        {
            throw new NotImplementedException();
        }
        
        
        
        
        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene La Cantidad de dias de cambio de tickets
        /// </summary>
        /// ***********************************************************************************************
        public static int getDiasCambio(Conexion oConn, int Estacion)
        {
           
            int Dias = 0;
            
            try
            {
                SqlDataReader oDR;
               
                
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_getDiasCambio";
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = Estacion;

                
                oDR = oCmd.ExecuteReader();

                
                if (oDR.Read())
                {
                    ConfiguracionClienteFacturacion oConf = new ConfiguracionClienteFacturacion();
                    if (oDR["zon_diafac"] != DBNull.Value)
                        Dias = Convert.ToInt32(oDR["zon_diafac"]);
                    else
                        Dias = 4; //Si no tiene Cantidad de dias es 4 por defecto
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Dias;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los Datos de un Ticket
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="parte">int - Numero de Parte</param>
        /// <returns>Carga la informacion perteneciente a un tickets</returns>
        /// ***********************************************************************************************
        public static Ticket getDatosTickets(Conexion oConn, int estacion, string puntoVenta, int NumerTicket, int Categoria)
        {
            Ticket oTicket = null;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_getDatosTicketWEB";
                oCmd.Parameters.Add("@PuVta", SqlDbType.Char,4).Value = puntoVenta;
                oCmd.Parameters.Add("@Ticke", SqlDbType.Int).Value = NumerTicket;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = Categoria;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    oTicket = CargaDatosTickets(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTicket;
        }


        

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto FacturaItem
        /// Publico porque lo usan las distintas ventas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de ItmFac</param>
        /// <returns>Objeto FacturaItem con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        public static Ticket CargaDatosTickets(System.Data.IDataReader oDR)
        {


            Ticket oTicket = new Ticket();
            oTicket.FechaTicket = (DateTime)oDR["Fecha"];
            oTicket.Estacion = new Estacion((byte)oDR["Estacion"], "");
            oTicket.Via = new Via((byte)oDR["Estacion"], (byte)(oDR["Via"]));
            if(oDR["Turno"] != DBNull.Value)
                oTicket.NumeroTurno = (int)(oDR["Turno"]);
            oTicket.Monto = (decimal)(oDR["Monto"]);
            oTicket.PuntoVenta = (oDR["PuntoVenta"]).ToString();
            oTicket.Anulado = (oDR["Anulado"] == "S" ? true : false);
            oTicket.Reemplazado = (oDR["Reemplazado"].ToString() == "S" ? true : false);
            oTicket.Expirado = (oDR["TicketExpirado"] == "S" ? true : false);
            oTicket.TieneRUC = (oDR["TieneRuc"] == "S" ? true : false);
            oTicket.RazonSocial = (oDR["NombreCliente"]).ToString();
            oTicket.NumeroTicket = (oDR["NumeroTicket"]).ToString();
            oTicket.Categoria = new CategoriaManual();
            oTicket.Categoria.Categoria = (byte)oDR["Categoria"];
            oTicket.CategoriaDiferente = (oDR["CategoDiferente"] == "S" ? true : false);


            return oTicket;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de tickets que ya fueron reemplazados
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Byte - Numero de Estacion</param>
        /// <param name="Numero de Reemplazo">Byte - Numero de Reemplazo</param>
        /// <returns>Carga la informacion perteneciente a un tickets</returns>
        /// ***********************************************************************************************
        public static TicketL getDatosTicketsReemplazados(Conexion oConn, int estacion, int NumeroReemplazo)
        {
            TicketL oListaTicket = new TicketL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_getDetalleTicket";
                oCmd.Parameters.Add("@ItmReemp", SqlDbType.Int).Value = NumeroReemplazo;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oListaTicket.Add(CargaDatosTickets(oDR));

                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oListaTicket;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la Cabecera de un Reemplazo de Ticket
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oFactura">ReemplazoTickets - Objeto con la informacion del reemplazo de ticket a agregar</param>
        /// <returns>el numero identity que agrego, si viene negativo genera una excepcion</returns>
        /// ***********************************************************************************************
        public static int addReemplazo(Conexion oConn, ReemplazoTickets oReemplazo)
        {
            int ret = -1;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_GrabarCabeceraReemplazo";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oReemplazo.Estacion.Numero;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oReemplazo.FechaOperacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oReemplazo.Parte.Numero;
                oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = oReemplazo.Monto;
                oCmd.Parameters.Add("@ClientGestion", SqlDbType.Int).Value = oReemplazo.Cliente.NumeroCliente;
                oCmd.Parameters.Add("@ClientLocal", SqlDbType.Int).Value = null; //Le paso null porque no tenemos la funcionalidad de crear clientes en el RT
                
                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval < 0)
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
                    ret = retval;
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
        /// Elimina un reemplazo de tickets
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oFactura">ReemplazoTickets - Objeto con la informacion del reemplazo de ticket a agregar</param>
        /// <returns>el numero identity que agrego, si viene negativo genera una excepcion</returns>
        /// ***********************************************************************************************
        public static bool delReemplazo(Conexion oConn, ReemplazoTickets oReemplazo)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_AnularReemplazo";
                oCmd.Parameters.Add("@IdReemplazo", SqlDbType.Int).Value = oReemplazo.NumReemplazo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval < 0)
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
        /// Agrega un Item al reemplazo de tickets
        /// Le asigna el Identity al item
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oItem">Ticket - Detalle del Ticket</param>
        /// <param name="oRemplazo">ReemplazoTickets - Se agrega el objeto reemplazo de Ticket porque tiene informacion general del reemplazo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addReemplazoTicketItem(Conexion oConn, Ticket oItem, ReemplazoTickets oRemplazo)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_GrabarDetalleReemplazo";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRemplazo.Estacion.Numero;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oRemplazo.FechaOperacion;
                oCmd.Parameters.Add("@idree", SqlDbType.Int).Value = oRemplazo.NumReemplazo;
                oCmd.Parameters.Add("@fecti", SqlDbType.DateTime).Value = oItem.FechaTicket;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = oItem.Via.NumeroVia;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = oItem.Categoria.Categoria;
                oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = oItem.Monto;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oItem.PuntoVenta;
                oCmd.Parameters.Add("@ticke", SqlDbType.Int).Value = oItem.NumeroTicket;
                oCmd.Parameters.Add("@estre", SqlDbType.TinyInt).Value = oItem.Estacion.Numero;

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
        /// Graba los datos del reemplazo en la tabla de los detalles
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una entrega)</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool FacturarReemplazo(Conexion oConn, FacturaItem oItem)
        {
            return FacturarReemplazo(oConn, oItem, false, false);
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
        public static bool FacturarReemplazo(Conexion oConn, FacturaItem oItem, bool porNC)
        {
            return FacturarReemplazo(oConn, oItem, !porNC, porNC);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Anula un Item al reemplazo de tickets
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oRemplazo">ReemplazoTickets, le paso el Numero de Item que tiene asignado</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool AnularItemReemplazo(Conexion oConn, FacturaItem oItem)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_AnularItem";
                oCmd.Parameters.Add("@nItem", SqlDbType.Int).Value = oItem.Identity;

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
        /// Graba los datos de la factura en el registro de una entrega (habilitando)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oItem">FacturaItem - Item de la Factura (que corresponde a una entrega)</param>
        /// <param name="anular">bool - true para anular la factura</param>
        /// <param name="anularNC">bool - true para anular la factura por Nota de Credito</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static bool FacturarReemplazo(Conexion oConn, FacturaItem oItem, bool anular, bool anularNC)
        {
            bool bRet = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_ReemplazoTickets_FacturarItem";
                if (!anular)
                {
                    oCmd.Parameters.Add("@Factu", SqlDbType.Int).Value = oItem.NumeroFactura;
                    oCmd.Parameters.Add("@ItmId", SqlDbType.Int).Value = oItem.Identity;
                    oCmd.Parameters.Add("@IdReemplazo", SqlDbType.Int).Value = oItem.Operacion.NumeroReemplazo;
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

    }
}
