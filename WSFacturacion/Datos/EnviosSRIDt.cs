using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Facturacion;
using Telectronica.Validacion;

namespace Telectronica.Peaje
{
    public class EnviosSRIDt
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de facturas anuladas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">Datetime - Jornada de la que se desean obtener las facturas anuladas</param>
        /// <returns>Lista de Facturas anuladas de una jornada</returns>
        /// ***********************************************************************************************
        public static FacturaL GetFacturasAnuladas(Conexion conn, DateTime jornada)
        {
            FacturaL oFacturas = new FacturaL();

            try
            {
                SqlDataReader oDR;
                Factura facturas = null;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_getInterfaseFacturasAnuladas";
                oCmd.Parameters.Add("fejor", SqlDbType.DateTime).Value = jornada;

                oCmd.CommandTimeout = 300;
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    Factura oFacturaAux = InterfaceDT.CargarListaFacturas(oDR);
                    //en el primer registro cargamos la factura
                    if (facturas == null || facturas.Identificado != oFacturaAux.Identificado
                        || facturas.Establecimiento != oFacturaAux.Establecimiento
                        || facturas.PuntoVenta != oFacturaAux.PuntoVenta
                        || facturas.SecuencialFactura != oFacturaAux.SecuencialFactura)
                    {
                        facturas = oFacturaAux;
                        oFacturas.Add(oFacturaAux);
                    }
                    //en todos cargamos el detalle
                    facturas.detalles.Add(InterfaceDT.CargarListaFacturasDetalle(oDR));
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
        /// Obtiene una lista de XML de boletas emitidas por las vías, no enviadas al SRI.
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de XML de boletas</returns>
        /// ***********************************************************************************************
        public static Facturacion.FacturaL GetFacturas(Conexion oConn, int numeroThread, Zona oZona, int registrosAProcesar)
        {
            Telectronica.Facturacion.FacturaL oFacturas = new Facturacion.FacturaL();
            try
            {
                SqlDataReader oDR;                

                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 600000;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_getFacturasVia";
                oCmd.Parameters.Add("thread", SqlDbType.TinyInt).Value = numeroThread;
                oCmd.Parameters.Add("zona", SqlDbType.TinyInt).Value = oZona.Codigo;
                oCmd.Parameters.Add("CantRows", SqlDbType.Int).Value = registrosAProcesar;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oFacturas.Add(CargarFacturaSRI(oDR));
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
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oZona"></param>
        /// ***********************************************************************************************
        public static bool MarcarEnviosProcesados(Conexion oConn, Zona oZona)
        {
            bool ret = false;
            try
            {
               
                SqlDataReader oDR;

                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 600000;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_updTrcruc";
                oCmd.Parameters.Add("zona", SqlDbType.TinyInt).Value = oZona.Codigo;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    ret = true;
                }


                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Marca como procesada una boleta de la vía.
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="coest">Código de estación</param>
        /// <param name="nuvia">Número de vía</param>
        /// <param name="fecha">Fecha de la factura</param>
        /// <param name="numev">Número de evento</param>
        /// <param name="estado">Estado a grabar</param>
        /// ***********************************************************************************************
        public static void MarcarProcesada(Conexion oConn, int coest, int nuvia, DateTime fecha, int? numev,
                                           string estado)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 600000;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasSunat_MarcarProcesada";
                oCmd.Parameters.Add("coest", SqlDbType.SmallInt).Value = coest;
                oCmd.Parameters.Add("nuvia", SqlDbType.SmallInt).Value = nuvia;
                oCmd.Parameters.Add("fecha", SqlDbType.DateTime).Value = fecha;
                oCmd.Parameters.Add("numev", SqlDbType.Int).Value = numev;
                oCmd.Parameters.Add("estado", SqlDbType.Char).Value = estado;

                oCmd.ExecuteNonQuery();


                // Cerramos el objeto
                oCmd = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga en un objeto Factura los datos del xml
        /// </summary>
        /// ***********************************************************************************************
        private static Facturacion.Factura CargarFacturaSRI(SqlDataReader oDR)
        {

            Facturacion.Factura oFactura = new Facturacion.Factura();
                       

            // Objeto Estacion con todos sus datos
            oFactura.Estacion = new Estacion();
            oFactura.Estacion.Numero = (byte)oDR["CodigoEstacion"];
            oFactura.Estacion.Nombre = oDR["NombreEstacion"].ToString();
            oFactura.Estacion.Razsoc = oDR["RazonSocial"].ToString();
            oFactura.Estacion.NombreComercial = oDR["NombreComercial"].ToString();
            oFactura.Estacion.Direccion = oDR["Direccion"].ToString();
            oFactura.Estacion.DireccMatriz = oDR["DireccionMatriz"].ToString();
            oFactura.Estacion.RUC = oDR["RUCConcesionario"].ToString();

            // Datos de la factura
            oFactura.Establecimiento = Convert.ToByte(oDR["Establecimiento"]);
            oFactura.PuntoVenta = oDR["PuntoVenta"].ToString();
            oFactura.TipoFactura = oDR["TipoFactura"].ToString();         
            oFactura.NumeroFactura = (int)oDR["NumeroFactura"];
            oFactura.FechaGeneracion = (DateTime)oDR["FechaGeneracion"];
            oFactura.MontoTotal = (decimal)oDR["MontoTotal"];
            oFactura.MontoNeto = (decimal)oDR["MontoNETO"];
            oFactura.MontoIVA = Convert.ToDecimal(oDR["MontoIVA"]);

            oFactura.CodigoOrigen = "VIA"; 

            oFactura.CodigoRIDE = oDR["codigoRIDE"].ToString();                         // Clave de Acceso (RIDE)

            oFactura.AmbienteFactElectronica = oDR["AmbienteFactElect"].ToString();     // Ambiente (prueba o produccion)    


            // Objeto Cliente con todos sus datos
            oFactura.Cliente = new Cliente();
            oFactura.Cliente.NumeroDocumento = oDR["DocumentoCliente"].ToString();
            oFactura.Cliente.RazonSocial = oDR["NombreCliente"].ToString();
            oFactura.Cliente.RazonSocialFactur = oDR["NombreCliente"].ToString();

            if (oDR["EmailCliente"] != DBNull.Value)
                oFactura.Cliente.Email = oDR["EmailCliente"].ToString();
            else
                oFactura.Cliente.Email = "";

            oFactura.Cliente.TipoDocumento = new TipoDocumento(Convert.ToInt16(oDR["TipoDocumentoCliente"]), "");
            oFactura.Cliente.TipoIVA = new TipoIVA(Convert.ToInt16(oDR["TipoIVACliente"]), "");


            oFactura.Impuesto = new Impuesto();
            oFactura.Impuesto.PorcentajeIva = Convert.ToDouble(oDR["PorcentajeIVA"]);


            // Item de la factura (1 solo item)
            FacturaItem oItem = new FacturaItem();            
            oItem.Cantidad = 1;
            oItem.Identity = (int)oDR["IdentityTRCRUC"];
            oItem.DescripcionVenta = oDR["DescripcionItem"].ToString(); 
            oItem.MontoUnitario = oFactura.MontoTotal;
            oItem.Monto = oFactura.MontoTotal;
            oFactura.Items = new FacturaItemL();
            oFactura.Items.Add(oItem);


            return oFactura;
                       
        }
  

        /// ***********************************************************************************************
        /// <summary>
        /// Registra el envío de un documento a SRI
        /// </summary>
        /// <param name="conn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEnvioSRI">Objeto de EnvioSRI a grabar</param>
        /// ***********************************************************************************************
        public static void RegistrarEnvio(Conexion conn, EnvioSRI oEnvioSRI)
        {
            try
            {

                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;
                oCmd.CommandTimeout = 600000;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_RegistrarEnvio";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oEnvioSRI.estacionComprobante.Numero;
                oCmd.Parameters.Add("@archi", SqlDbType.VarChar, 100).Value = oEnvioSRI.Archivo;
                oCmd.Parameters.Add("@fegen", SqlDbType.DateTime).Value = oEnvioSRI.FechaGeneracion;
                oCmd.Parameters.Add("@rucco", SqlDbType.BigInt).Value = oEnvioSRI.RucConcesionario;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oEnvioSRI.TipoComprobante;
                oCmd.Parameters.Add("@estab", SqlDbType.SmallInt).Value = oEnvioSRI.Establecimiento;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oEnvioSRI.PuntoVenta;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oEnvioSRI.NumeroComprobante;
                oCmd.Parameters.Add("@fecom", SqlDbType.DateTime).Value = oEnvioSRI.FechaComprobante;
                oCmd.Parameters.Add("@tipro", SqlDbType.Char, 1).Value = oEnvioSRI.TipoProcesamiento;
                oCmd.Parameters.Add("@fefis", SqlDbType.DateTime).Value = oEnvioSRI.FechaFiscal;
                oCmd.Parameters.Add("@status", SqlDbType.Char, 1).Value = oEnvioSRI.EstadoEnvioSRI.StatusEnvio;
                oCmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = oEnvioSRI.EstadoEnvioSRI.SeguimientoEnvio;
                oCmd.Parameters.Add("@EstadoEnvio", SqlDbType.Char, 3).Value = oEnvioSRI.EstadoEnvioSRI.EstadoEnvio;
                oCmd.Parameters.Add("@DetalleEstadoEnvio", SqlDbType.VarChar,1000).Value = oEnvioSRI.EstadoEnvioSRI.DetalleRespuesta;
                oCmd.Parameters.Add("@ErrorWS", SqlDbType.VarChar, 1000).Value = oEnvioSRI.EstadoEnvioSRI.DescErrorEnvioSRI;
                oCmd.Parameters.Add("@CodigoErrorWS", SqlDbType.Int).Value = oEnvioSRI.EstadoEnvioSRI.CodigoError;
                oCmd.Parameters.Add("@ErrorInfAdic", SqlDbType.VarChar,2000).Value = oEnvioSRI.EstadoEnvioSRI.ErrorInfAdicional;
                oCmd.Parameters.Add("@coestOrigen", SqlDbType.TinyInt).Value = oEnvioSRI.estacionGeneracion.Numero;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
    
        }
        

        /// ***********************************************************************************************
        /// <summary>
        /// Registra el status del envío de un documento al SRI
        /// </summary>
        /// <param name="conn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oEnvioSRI">Objeto de EnvioSRI a actualizar</param>
        /// ***********************************************************************************************
        public static void ActualizarEstadoEnvio(Conexion conn, EnvioSRI oEnvioSRI)
        {
            try
            {

                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;
                oCmd.CommandTimeout = 600000;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_ActualizarEstadoEnvio";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oEnvioSRI.estacionComprobante.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oEnvioSRI.TipoComprobante;
                oCmd.Parameters.Add("@estab", SqlDbType.SmallInt).Value = oEnvioSRI.Establecimiento;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oEnvioSRI.PuntoVenta;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oEnvioSRI.NumeroComprobante;
                oCmd.Parameters.Add("@fefis", SqlDbType.DateTime).Value = oEnvioSRI.FechaComprobante;
                oCmd.Parameters.Add("@feare", SqlDbType.DateTime).Value = oEnvioSRI.EstadoEnvioSRI.FechaRespuesta;
                oCmd.Parameters.Add("@codres", SqlDbType.VarChar, 2).Value = (oEnvioSRI.EstadoEnvioSRI.EstadoEnvio == null ? "" : oEnvioSRI.EstadoEnvioSRI.EstadoEnvio.ToString());
                oCmd.Parameters.Add("@resdet", SqlDbType.VarChar, 200).Value = (oEnvioSRI.EstadoEnvioSRI.DetalleRespuesta == null ? "" : oEnvioSRI.EstadoEnvioSRI.DetalleRespuesta);
                oCmd.Parameters.Add("@status", SqlDbType.Char, 1).Value = oEnvioSRI.EstadoEnvioSRI.StatusEnvio;
                oCmd.Parameters.Add("@auSRI", SqlDbType.VarChar, 50).Value = (oEnvioSRI.EstadoEnvioSRI.AutSRI == null ? "" : oEnvioSRI.EstadoEnvioSRI.AutSRI);
                oCmd.Parameters.Add("@ErrorWS", SqlDbType.VarChar, 1000).Value = oEnvioSRI.EstadoEnvioSRI.DescErrorEnvioSRI;
                oCmd.Parameters.Add("@CodigoErrorWS", SqlDbType.Int).Value = oEnvioSRI.EstadoEnvioSRI.CodigoError;
                oCmd.Parameters.Add("@ErrorInfAdic", SqlDbType.VarChar,2000).Value = oEnvioSRI.EstadoEnvioSRI.ErrorInfAdicional;
                oCmd.Parameters.Add("@RucConce", SqlDbType.BigInt).Value = oEnvioSRI.estacionComprobante.RUC;
                oCmd.Parameters.Add("@guid", SqlDbType.UniqueIdentifier).Value = oEnvioSRI.EstadoEnvioSRI.SeguimientoEnvio;
                oCmd.Parameters.Add("@coestOrigen", SqlDbType.TinyInt).Value = oEnvioSRI.estacionGeneracion.Numero;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
    
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Cambia el estado de las facturas con numero repetido a rEchazado (si es una factura diferente) o Repetido (si es la misma factura)
        /// </summary>
        /// ***********************************************************************************************
        public static void MarcarFacturasRepetidas(Conexion conn)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;
                oCmd.CommandTimeout = 600000;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_MarcarFacturasRepetidas";

                oCmd.ExecuteNonQuery();


                // Cerramos el objeto
                oCmd = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el proximo numero a generar de la tabla numeradora para un codigo de registro determinado
        /// </summary>
        /// <param name="conn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="nombreArchivo">string - Tabla: Nombre del codigo de la tabla a numerar</param>
        /// ***********************************************************************************************
        public static int getProximoNumeroTabla(Conexion oConn, string tabla)
        {
            int proximoNumero = 0;

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.getNumeroTabla";
                oCmd.Parameters.Add("@tabla", SqlDbType.VarChar, 30).Value = tabla;

                SqlParameter numero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                numero.Direction = ParameterDirection.Output;

                oCmd.ExecuteNonQuery();

                if (numero.Value != DBNull.Value)
                {
                    proximoNumero = Convert.ToInt32(numero.Value);
                }

                // Cerramos el objeto
                oCmd = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return proximoNumero;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de los registros de archivos enviados al SRI
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="fechaDesde">Fecha Inicial</param>
        /// <param name="fechaHasta">Fecha Final</param>
        /// <param name="zona">Zona</param>
        /// <param name="establecimiento">Establecimiento del comprobante</param>
        /// <param name="puntoVenta">Punto de Venta del comprobante</param>
        /// <param name="numeroComprobante">Numero de comprobante</param>
        /// <param name="statusComprobante">Status del comprobante</param>
        /// <returns>Lista de comprobantes que coinciden con los parametros</returns>
        /// ***********************************************************************************************
        public static EnvioSRIL getRegistroEnviosSRI(Conexion oConn, DateTime fechaDesde, DateTime fechaHasta, int? zona, int? establecimiento,
                                                     string puntoVenta, int? numeroComprobante, string statusComprobante, string nombreCliente, string nroRuc, char incluirPendientes, out bool llegoTop, int top)
        {
            EnvioSRIL envios = new EnvioSRIL();
            int cantidad = 0;
            try
            {
                 SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_GetEnvio";
                oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@zona", SqlDbType.Int).Value = zona;
                oCmd.Parameters.Add("@estab", SqlDbType.SmallInt).Value = establecimiento;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = puntoVenta;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numeroComprobante;
                oCmd.Parameters.Add("@status", SqlDbType.Char, 1).Value = statusComprobante;
                oCmd.Parameters.Add("@nombreCliente", SqlDbType.VarChar).Value = nombreCliente;
                oCmd.Parameters.Add("@nroDocumento", SqlDbType.VarChar, 13).Value = nroRuc;
                oCmd.Parameters.Add("@incluyePendiente", SqlDbType.Char, 1).Value = incluirPendientes;
                oCmd.Parameters.Add("@top", SqlDbType.Int).Value = top;

                oDR = oCmd.ExecuteReader();
                llegoTop = false;
                while (oDR.Read())
                {                                                          
                    envios.Add(CargarRegistroEnvio(oDR));
                    cantidad += 1;
                    if (top == cantidad)
                    {
                        llegoTop = true;
                        break;
                    }                        
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return envios;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de los registros de archivos enviados al SRI
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns>Lista de comprobantes que coinciden con los parametros</returns>
        /// ***********************************************************************************************
        public static DataSet getRegistroSRI_EnviosPorEstacion(Conexion oConn, DateTime fechaDesde, DateTime fechaHasta)
        {
            DataSet dsEstadoEnvio = new DataSet();
            dsEstadoEnvio.DataSetName = "RptPeaje_GetEnviosPorEstacion";        
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 5000;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_GetEnviosPorEstacion";
                oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechaHasta;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsEstadoEnvio, "GetEnviosPorEstacion");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsEstadoEnvio;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de los registros de archivos enviados a la Sunat
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ResumenSRIL getResumenEnviosSunat(Conexion oConn)
        {
            ResumenSRIL envios = new ResumenSRIL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 300;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasSunat_getResumenEnviosSunat";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    envios.Add(CargarResumenEnvio(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return envios;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los datos del ODR y Retorna el objeto de envio de archivos a la sunat
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static EnvioSRI CargarRegistroEnvio(System.Data.IDataReader oDR)
        {
            EnvioSRI envio = new EnvioSRI();

            // DATOS DE SRI_PROCE
            if (oDR["spr_archi"] != DBNull.Value)
                envio.Archivo = oDR["spr_archi"].ToString();
            if (oDR["spr_fegen"] != DBNull.Value)
            envio.FechaGeneracion = Convert.ToDateTime(oDR["spr_fegen"]);


            envio.EstadoEnvioSRI = new EstadosEnvioSRI();

            if (oDR["spr_status"] != DBNull.Value)
                envio.EstadoEnvioSRI.StatusEnvio = oDR["spr_status"].ToString();
            else
                envio.EstadoEnvioSRI.StatusEnvio = "";
        
            if (oDR["spr_res"] != DBNull.Value)
                envio.EstadoEnvioSRI.EstadoEnvio = oDR["spr_res"].ToString();
            if (oDR["spr_resdet"] != DBNull.Value)
                envio.EstadoEnvioSRI.DetalleRespuesta = oDR["spr_resdet"].ToString();
            if (oDR["spr_feare"] != DBNull.Value)
                envio.EstadoEnvioSRI.FechaRespuesta = Convert.ToDateTime(oDR["spr_feare"]);
            if (oDR["spr_guid"] != DBNull.Value)
                envio.EstadoEnvioSRI.SeguimientoEnvio = (Guid)oDR["spr_guid"];
            if (oDR["spr_tipro"] != DBNull.Value)
                envio.TipoProcesamiento = oDR["spr_tipro"].ToString();
            if (oDR["spr_errws"] != DBNull.Value)
                envio.EstadoEnvioSRI.DescErrorEnvioSRI = oDR["spr_errws"].ToString();
            if (oDR["spr_coderr"] != DBNull.Value)
                envio.EstadoEnvioSRI.CodigoError = Convert.ToInt32(oDR["spr_coderr"]);
            if (oDR["spr_infadi"] != DBNull.Value)
                envio.EstadoEnvioSRI.ErrorInfAdicional = oDR["spr_infadi"].ToString();
            if (oDR["spr_tipfa"] != DBNull.Value)
                envio.TipoComprobante = oDR["spr_tipfa"].ToString();
            if (oDR["spr_numer"] != DBNull.Value)
                envio.NumeroComprobante = Convert.ToInt32(oDR["spr_numer"]);
            if (oDR["spr_fefis"] != DBNull.Value)
                envio.FechaComprobante = Convert.ToDateTime(oDR["spr_fefis"]);
            if (oDR["spr_puvta"] != DBNull.Value)
                envio.PuntoVenta = oDR["spr_puvta"].ToString();


            //DATOS DE LA ESTACION Y ZONA
            envio.estacionComprobante = new Estacion();
            envio.estacionComprobante.Numero = Convert.ToInt32(oDR["est_codig"]);
            envio.estacionComprobante.Nombre = oDR["est_nombr"].ToString();
            envio.estacionComprobante.RUC = oDR["zon_struc"].ToString();
            envio.estacionGeneracion = new Estacion();
            envio.estacionGeneracion.Numero = Convert.ToInt32(oDR["EstacionGeneracion"]);

            envio.Establecimiento = Convert.ToInt32(oDR["est_estab"]);
            envio.estacionComprobante.Zona = new Zona();
            envio.estacionComprobante.Zona.Codigo = Convert.ToInt32(oDR["zon_zona"]);
            envio.estacionComprobante.Zona.entityWSFactElectronica = oDR["zon_entityws"].ToString();
            envio.estacionComprobante.Zona.usuarioWSFactElectronica = oDR["zon_usuarws"].ToString();
            envio.estacionComprobante.Zona.passwordWSFactElectronica = oDR["zon_passwws"].ToString();

            //DATOS DEL CLIENTE
            envio.Client = new Cliente();
            if (oDR["trc_nombr"] != DBNull.Value)
            {
                envio.Client.RazonSocial = oDR["trc_nombr"].ToString();
            }

            if (oDR["trc_ruc"] != DBNull.Value)
            {
                envio.Client.NumeroDocumento = oDR["trc_ruc"].ToString();
            }

            return envio;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los datos del ODR y Retorna el objeto de envio de archivos a la sunat
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ResumenSRI CargarResumenEnvio(System.Data.IDataReader oDR)
        {
            ResumenSRI envio = new ResumenSRI();

            envio.Titulo = oDR["Titulo"].ToString();

            if (oDR["Valor"] != DBNull.Value)
            {
                envio.Valor = oDR["Valor"].ToString();
            }
            else
            {
                envio.Valor = "";
            }

            envio.Falla = oDR["Falla"].ToString();

            return envio;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de facturas de via pendientes de recepcion de estado 
        /// </summary>
        /// <param name="conn">Conexion con la base de datos</param>
        /// <param name="oZona">Zona de la que se desean recibir los comprobantes</param>
        /// <param name="registrosAProcesar">Cantidad de registros a procesar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static EnvioSRIL GetEnviosPendientesVia(Conexion conn, int numeroThread, Zona oZona, int registrosAProcesar)
        {

            EnvioSRIL envios = new EnvioSRIL();
            try
            {
                 SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();

                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;
                oCmd.CommandTimeout = 600000;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_GetEnviosPendientesVia";
                oCmd.Parameters.Add("thread", SqlDbType.TinyInt).Value = numeroThread;
                oCmd.Parameters.Add("zona", SqlDbType.TinyInt).Value = oZona.Codigo;
                oCmd.Parameters.Add("CantRows", SqlDbType.Int).Value = registrosAProcesar;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    envios.Add(CargarRegistroEnvio(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return envios;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna una factura puntual, que se consulta para poder actualizar su estado
        /// </summary>
        /// <param name="conn">Conexion con la base de datos</param>
        /// <param name="oNotCre">Objeto de la factura</param>
        /// <returns>Envio en el que se genero</returns>
        /// ***********************************************************************************************
        public static EnvioSRI GetEnviosFactura(Conexion conn, Facturacion.Factura oFact)
        {

            EnvioSRI envios = new EnvioSRI();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_GetEnvioFactura";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFact.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oFact.TipoFactura;
                oCmd.Parameters.Add("@estab", SqlDbType.SmallInt).Value = oFact.Establecimiento;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oFact.PuntoVenta;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oFact.NumeroFactura;
                oCmd.Parameters.Add("@fefis", SqlDbType.DateTime).Value = oFact.FechaGeneracion;
               

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    envios = CargarRegistroEnvio(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return envios;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna una nota de credito puntual, que se consulta para poder actualizar su estado
        /// </summary>
        /// <param name="conn">Conexion con la base de datos</param>
        /// <param name="oNotCre">Objeto de la nota de credito</param>
        /// <returns>Envio en el que se genero</returns>
        /// ***********************************************************************************************
        public static EnvioSRI GetEnviosNotaCredito(Conexion conn, Facturacion.NotaCredito oNotCre)
        {

            EnvioSRI envios = new EnvioSRI();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_GetEnvioNotaCredito";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oNotCre.Estacion.Numero;
                oCmd.Parameters.Add("@tipfa", SqlDbType.Char, 1).Value = oNotCre.Factura.TipoFactura;
                oCmd.Parameters.Add("@estab", SqlDbType.SmallInt).Value = oNotCre.Estab;
                oCmd.Parameters.Add("@puvta", SqlDbType.Char, 4).Value = oNotCre.PuntoVenta;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oNotCre.NumeroNC;
                oCmd.Parameters.Add("@fefis", SqlDbType.DateTime).Value = oNotCre.FechaGeneracion;


                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    envios = CargarRegistroEnvio(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return envios;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de los threads configurados a ser ejecutado por cada zona
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ThreadProcesoSRIL getThreadsPorZona(Conexion oConn)
        {
            ThreadProcesoSRIL threadsProceso = new ThreadProcesoSRIL();
            
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_getThreadsEnvio";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    threadsProceso.Add(CargarThread(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return threadsProceso;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los datos del ODR y Retorna el objeto de threadProceso 
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ThreadProcesoSRI CargarThread(System.Data.IDataReader oDR)
        {
            ThreadProcesoSRI oThread = new ThreadProcesoSRI();

            oThread.NumeroThread = Convert.ToInt32(oDR["sth_thread"]);

            oThread.Zona = EstacionDt.CargarZona(oDR);    


            return oThread;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Regenera un comprobante, marcandolo para que sea reenviado al SRI
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="fecha">Fecha del comprobante</param>
        /// <param name="estacion">Estacion de generacion del comprobante</param>
        /// <param name="establecimiento">Establecimiento de la estacion del comprobante</param>
        /// <param name="puntoVenta">Punto de venta</param>
        /// <param name="numeroComprobante">Numero de comprobante</param>
        /// ***********************************************************************************************
        public static void RegenerarComprobante(Conexion oConn, Facturacion.Factura oFactu)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_FacturasElectronicas_RegenerarComprobante";
                oCmd.Parameters.Add("coest", SqlDbType.Int).Value = oFactu.Estacion.Numero;
                oCmd.Parameters.Add("estab", SqlDbType.SmallInt).Value = oFactu.Establecimiento;
                oCmd.Parameters.Add("puvta", SqlDbType.Char, 4).Value = oFactu.PuntoVenta;
                oCmd.Parameters.Add("numer", SqlDbType.Int).Value = oFactu.NumeroFactura;
                oCmd.Parameters.Add("fecha", SqlDbType.DateTime).Value = oFactu.FechaGeneracion;
                oCmd.Parameters.Add("estadoNuevo", SqlDbType.Char).Value = oFactu.EstadoEnvio.StatusEnvioNuevo;
                oCmd.Parameters.Add("nroDocumetoNuevo", SqlDbType.VarChar).Value = oFactu.Cliente.NumeroDocumento;

                oCmd.ExecuteNonQuery();

                // Cerramos el objeto
                oCmd = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
 
}
