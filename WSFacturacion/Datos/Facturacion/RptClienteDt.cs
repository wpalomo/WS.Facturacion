using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    public class RptClienteDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un DataSet los datos de los transitos automaticos de un cliente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="desde">DateTime - Fecha y Hora Desde</param>
        /// <param name="hasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="cliente">int? - Numero de Cliente</param>
        /// <param name="patente">string - Patente</param>
        /// <param name="tag">string - Numero de Tag</param>
        /// <param name="chip">int? - Numero Externo de Tarjeta Chip</param>
        /// <param name="zona">int? - Zona (Concesionario)</param>
        /// <param name="tipoTransito">string - Tipo de Transito</param>
        /// ***********************************************************************************************
        public static DataSet getTransitosAutomaticos(Conexion oConn, DateTime desde, DateTime hasta,
            int? cliente, string patente, string tag, int? chip, int? zona, string tipoTransito, int? tagExt)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptCliente_TransitosAutomaticosDS";

            try
            {


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_RptCliente_getTransitosAutomaticos";
                oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@numcl", SqlDbType.Int).Value = cliente;
                oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 10).Value = patente;
                oCmd.Parameters.Add("@numtg", SqlDbType.VarChar, 15).Value = tag;
                oCmd.Parameters.Add("@numch", SqlDbType.Int).Value = chip;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@TipoTran", SqlDbType.Char, 1).Value = tipoTransito;
                oCmd.Parameters.Add("@nuexttg", SqlDbType.Int).Value = tagExt;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "TransitosAutomaticos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getCobrosACuenta(Conexion oConn, DateTime desde, DateTime hasta, int? cliente, string usuario)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_CobrosACuentaDS";

            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getListadoCobroCuenta";
                oCmd.Parameters.Add("@FecIni", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@FecFin", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@User", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "CobrosACuenta");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getFacturacion(Conexion oConn, DateTime desde, DateTime hasta, int? cliente, string usuario, int? factura, byte? estacion )
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_FacturacionDS";

            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getListadoFacturas";
                oCmd.Parameters.Add("@FecIni", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@FecFin", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@User", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;
                oCmd.Parameters.Add("@Factura", SqlDbType.Int).Value = factura;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;


                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "Facturacion");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        /*
        public static DataSet getListadoFacturasCliente(Conexion oConn, DateTime desde, DateTime hasta, int? cliente , char? SinCF, char? SoloAud , char? TariDif,  bool bConDetalle)
        {
            
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_FacturasClienteDS";
            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getListadoFacturasCliente";
                oCmd.Parameters.Add("@FecIni", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@FecFin", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;
                oCmd.Parameters.Add("@SinCF", SqlDbType.Char).Value = SinCF;
                oCmd.Parameters.Add("@SoloAud", SqlDbType.Char).Value = SoloAud;
                oCmd.Parameters.Add("@TariDif", SqlDbType.Char).Value = TariDif;
                oCmd.Parameters.Add("@ConDetalle", SqlDbType.Char).Value = bConDetalle?'S':'N';
                
                oCmd.CommandTimeout = 3000;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "FacturasCliente");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }
        */

        public static DataSet getListadoFacturasCliente(Conexion oConn, DateTime desde, DateTime hasta, string documento, char? SinCF, char? SoloAud, char? TariDif, bool bConDetalle, bool bPorJornada)
        {

            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_FacturasClienteDS";
            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getListadoFacturasCliente";
                oCmd.Parameters.Add("@FecIni", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@FecFin", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@Docum", SqlDbType.VarChar).Value = documento;
                oCmd.Parameters.Add("@SinCF", SqlDbType.Char).Value = SinCF;
                oCmd.Parameters.Add("@SoloAud", SqlDbType.Char).Value = SoloAud;
                oCmd.Parameters.Add("@TariDif", SqlDbType.Char).Value = TariDif;
                oCmd.Parameters.Add("@ConDetalle", SqlDbType.Char).Value = bConDetalle ? 'S' : 'N';
                oCmd.Parameters.Add("@PorJornada", SqlDbType.Char).Value = bPorJornada ? 'S' : 'N';

                oCmd.CommandTimeout = 3000;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "FacturasCliente");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }


        /*
        public static DataSet getListadoFacturasCliente(Conexion oConn, DateTime desde, DateTime hasta, int? cliente)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_FacturasClienteDS";

            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getListadoFacturasCliente";
                oCmd.Parameters.Add("@FecIni", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@FecFin", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "FacturasCliente");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }
         */

        public static DataSet getFacturasPorPV(Conexion oConn, DateTime desde, DateTime hasta, int? estacion, int? puntoventa,
                                int? facturaDesde, int? facturaHasta, string usuario, string anulada,char? SinCF, char? SoloAud, char? TariDif)
        {
            DataSet dsFacturas = new DataSet();
            dsFacturas.DataSetName = "RptFacturacion_FacturasPorPVDS";

            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getFacturasPorPV";
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("@punto_venta", SqlDbType.Int).Value = puntoventa;
                oCmd.Parameters.Add("@facdesde", SqlDbType.Int).Value = facturaDesde;
                oCmd.Parameters.Add("@fachasta", SqlDbType.Int).Value = facturaHasta;
                oCmd.Parameters.Add("@opeid", SqlDbType.VarChar).Value = usuario;
                oCmd.Parameters.Add("@anulado", SqlDbType.Char,1).Value = anulada;
                oCmd.Parameters.Add("@SinCF", SqlDbType.Char).Value = SinCF;
                oCmd.Parameters.Add("@SoloAud", SqlDbType.Char).Value = SoloAud;
                oCmd.Parameters.Add("@TariDif", SqlDbType.Char).Value = TariDif;
                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsFacturas, "Facturas");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsFacturas;
        }

        public static DataSet getNotasDeCredito(Conexion oConn, DateTime desde, DateTime hasta, int? cliente, string usuario)
        {
            DataSet dsNotasDeCredito = new DataSet();
            dsNotasDeCredito.DataSetName = "RptFacturacion_NotasDeCreditoDS";

            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getListadoNotasCredito";
                oCmd.Parameters.Add("@FecIni", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@FecFin", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@User", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = cliente;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsNotasDeCredito, "NotasDeCredito");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsNotasDeCredito;
        }

        public static DataSet getOperaciones(Conexion oConn, DateTime desde, DateTime hasta, int? cliente, string usuario, int? iOperacion)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_OperacionesDS";

            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Facturacion_getListadoOperaciones";
                oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@OpeId", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@Opera", SqlDbType.Int).Value = iOperacion;
                oCmd.Parameters.Add("@Clien", SqlDbType.Int).Value = cliente;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "Operaciones");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getDetalleClienteCabecera(Conexion oConn, int ClienteID)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_ClienteDetalleCabeceraDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Clientes_rptClienteCabecera";
                oCmd.Parameters.Add("@cli_numcl", SqlDbType.Int).Value = ClienteID;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "ClienteDetalleCabecera");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getDetalleClienteVehiculos(Conexion oConn, int ClienteID)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_ClienteDetalleVehiculosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Clientes_rptClienteVehiculos";
                oCmd.Parameters.Add("@cli_numcl", SqlDbType.Int).Value = ClienteID;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "ClienteDetalleVehiculos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getDetalleClienteCuentas(Conexion oConn, int ClienteID)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_ClienteDetalleCuentasDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Clientes_rptClienteCuentas";
                oCmd.Parameters.Add("@cli_numcl", SqlDbType.Int).Value = ClienteID;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "ClienteDetalleCuentas");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        public static DataSet getListaClientes(Conexion oConn, int EstacionID, int? AgrupacionID, int? CuentaID)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptFacturacion_ListaClientesDS";
            
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Clientes_rptListaClientes";
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = EstacionID;
                oCmd.Parameters.Add("@subfp", SqlDbType.Int).Value = AgrupacionID;
                oCmd.Parameters.Add("@tipcu", SqlDbType.Int).Value = CuentaID;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "ListaClientes");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }
    }
}
