using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Facturacion
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase de negocios para reportes de clientes
    /// </summary>
    /// ***********************************************************************************************
    public class RptClienteBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los transitos automaticos
        /// </summary>
        /// <param name="desde">DateTime - Fecha y Hora Desde</param>
        /// <param name="hasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="cliente">int? - Numero de Cliente</param>
        /// <param name="patente">string - Patente</param>
        /// <param name="tag">string - Numero de Tag</param>
        /// <param name="chip">int? - Numero Externo de Tarjeta Chip</param>
        /// <param name="zona">int? - Zona (Concesionario)</param>
        /// <param name="tipoTransito">string - Tipo de Transito</param>
        /// ***********************************************************************************************
        public static DataSet getTransitosAutomaticos(DateTime desde, DateTime hasta,
            int? cliente, string patente, string tag, int? chip, int? zona, string tipoTransito, int? tagExt)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //TODO si no puedo con consolidado conectar con la plaza
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidado(false);
                    return RptClienteDt.getTransitosAutomaticos(conn, desde, hasta, cliente, patente, tag, chip, zona, tipoTransito, tagExt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los cobros a cuenta
        /// </summary>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <param name="cliente"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getCobrosACuenta(DateTime desde, DateTime hasta, int? cliente, string usuario)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptClienteDt.getCobrosACuenta(conn, desde, hasta, cliente, usuario);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getFacturacion(DateTime desde, DateTime hasta, int? cliente, string usuario, int? factura, byte? estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptClienteDt.getFacturacion(conn, desde, hasta, cliente, usuario, factura, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getFacturasPorPV(DateTime desde, DateTime hasta, int? estacion, int? puntoventa,
                                int? facturaDesde, int? facturaHasta, string usuario, string anulada, char? SinCF, char? SoloAud ,char? TariDif)
        {
            DataSet ds = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    ds = RptClienteDt.getFacturasPorPV(conn, desde, hasta, estacion, puntoventa, facturaDesde, facturaHasta, usuario, anulada, SinCF, SoloAud, TariDif);

                    //Auditoria
                    using (Conexion connAud = new Conexion())
                    {
                        //conn.ConectarGSTThenPlaza();
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                        // Ahora tenemos que grabar la auditoria:
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaInformeFacturas(),
                                                               "C",
                                                               "",
                                                               getAuditoriaDescripcionInformeFacturas(desde, hasta, estacion, puntoventa, facturaDesde, facturaHasta, usuario, anulada)),
                                                               connAud);

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaInformeFacturas()
        {
            return "AUD";
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionInformeFacturas(DateTime desde, DateTime hasta, int? estacion, int? puntoventa,
                                int? facturaDesde, int? facturaHasta, string usuario, string anulada)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Auditoría de Transacciones, ");
            AuditoriaBs.AppendCampo(sb, "Desde", desde.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Hasta", hasta.ToShortDateString());
            if( estacion != null )
                AuditoriaBs.AppendCampo(sb, "Estación", ((int)estacion).ToString());
            if (puntoventa != null)
                AuditoriaBs.AppendCampo(sb, "Punto Emisión", ((int)puntoventa).ToString());
            if (facturaDesde != null)
                AuditoriaBs.AppendCampo(sb, "Factura Desde", ((int)facturaDesde).ToString());
            if (facturaHasta != null)
                AuditoriaBs.AppendCampo(sb, "Factura Hasta", ((int)facturaHasta).ToString());
            if (usuario != null)
                AuditoriaBs.AppendCampo(sb, "Usuario", usuario);
            if (anulada == "S")
                sb.Append("Sólo Anuladas");
            else if (anulada == "N")
                sb.Append("Sólo Emitidas");

            return sb.ToString();
        }

        public static DataSet getListadoFacturasCliente(DateTime desde, DateTime hasta, string documento, char? SinCF, char? SoloAud, char? TariDif, bool bConDetalle)
        {
            return getListadoFacturasCliente(desde, hasta, documento, SinCF, SoloAud, TariDif, bConDetalle, false);
            /*
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptClienteDt.getListadoFacturasCliente(conn, desde, hasta, cliente ,  SinCF,  SoloAud , TariDif, bConDetalle );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }*/
        }

        public static DataSet getListadoFacturasCliente(DateTime desde, DateTime hasta, string documento, char? SinCF, char? SoloAud, char? TariDif, bool bConDetalle, bool bPorJornada)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptClienteDt.getListadoFacturasCliente(conn, desde, hasta, documento, SinCF, SoloAud, TariDif, bConDetalle, bPorJornada);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        public static DataSet getListadoFacturasCliente(DateTime desde, DateTime hasta, int? cliente)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptClienteDt.getListadoFacturasCliente(conn, desde, hasta, cliente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        public static DataSet getNotasDeCredito(DateTime desde, DateTime hasta, int? cliente, string usuario)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptClienteDt.getNotasDeCredito(conn, desde, hasta, cliente, usuario);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getOperaciones(DateTime desde, DateTime hasta, int? cliente, string usuario, int? iOperacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base del CCO                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return RptClienteDt.getOperaciones(conn, desde, hasta, cliente, usuario, iOperacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getDetalleClienteCabecera(int ClienteID)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarGSTThenPlaza();
                    return RptClienteDt.getDetalleClienteCabecera(conn, ClienteID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getDetalleClienteVehiculos(int ClienteID)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarGSTThenPlaza();
                    return RptClienteDt.getDetalleClienteVehiculos(conn, ClienteID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getDetalleClienteCuentas(int ClienteID)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarGSTThenPlaza();
                    return RptClienteDt.getDetalleClienteCuentas(conn, ClienteID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getListaClientes(int EstacionID, int? AgrupacionID, int? CuentaID)
        {

            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion, contra base de gestion
                    conn.ConectarGSTThenPlaza();
                    return RptClienteDt.getListaClientes(conn, EstacionID, AgrupacionID, CuentaID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
