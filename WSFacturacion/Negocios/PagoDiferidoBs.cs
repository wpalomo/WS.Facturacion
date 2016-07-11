using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Telectronica.Peaje
{
    public class PagoDiferidoBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los pagos diferidos generados en la via
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Fecha Desde</param>
        /// <param name="jornadaHasta">DateTime - Fecha Hasta</param>
        /// <param name="estado">string - Estado del pago diferido (A: Aceptado, R:Rechazado)</param>
        /// <returns>Lista de Pagos diferidos</returns>
        /// ***********************************************************************************************
        public static PagoDiferidoSupervisorL getPagosDiferidos(int estacion, DateTime fechaDesde, DateTime fechaHasta, string estado)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return PagoDiferidoDt.getPagosDiferidos(conn, estacion, fechaDesde, fechaHasta, estado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un pago diferido sup
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="estado">string - Estado del pago diferido (A: Aceptado, R:Rechazado)</param>
        /// <returns>Pago diferido</returns>
        /// ***********************************************************************************************
        //public static PagoDiferidoSupervisor getPagoDiferido(int estacion, int numPago)
        //{
        //    try
        //    {
        //        using (Conexion conn = new Conexion())
        //        {
        //            //sin transaccion
        //            conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

        //            return PagoDiferidoDt.getPagoDiferido(conn, estacion, numPago);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

             /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el reconocimiento de deuda de pago diferido Como un DataSet
        /// </summary>
        /// <param name="numeroPago">Int - Numero del pago diferido</param>
        /// <param name="patente">String - Patente del vehiculo</param>
        /// <param name="cantidadCopias">int - Cantidad de copias a imprimir</param>
        /// <returns>DataSet con el reconocimiento</returns>
        /// ***********************************************************************************************
        public static DataSet getReconocimientoDeuda(int numeroPago, int estacion, DateTime fecge, int cantidadCopias)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return PagoDiferidoDt.getReconocimientoDeuda(conn, numeroPago, estacion, fecge, cantidadCopias);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda el pago diferido aceptado o rechazado por el supervisor
        /// </summary>
        /// <param name="pdSupervisor">PagoDiferidoSupervisor - pdSupervisor</param>
        /// ***********************************************************************************************
        public static void addPagoDiferidoSupervisor(PagoDiferidoSupervisor pdSupervisor)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    PagoDiferidoDt.addPagoDiferidoSupervisor(conn, pdSupervisor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void updPagoDiferidoSupervisor(PagoDiferidoSupervisor pdSupervisor)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    PagoDiferidoDt.updPagoDiferidoSupervisor(conn, pdSupervisor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de causas de pago diferido
        /// </summary>
        /// <returns>Objeto CausaPagoDiferido</returns>
        /// ***********************************************************************************************
        public static CausaPagoDiferidoL getCausasPagoDiferido()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return PagoDiferidoDt.getCausasPagoDiferido(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los pagos diferidos generados en la via POR PLACA Y OPERADOR
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// /// <param name="estacion">int - via</param>
        /// <param name="jornadaDesde">DateTime - Fecha Desde</param>
        /// <param name="jornadaHasta">DateTime - Fecha Hasta</param>
        /// <param name="estado">string - operador</param>
        /// <param name="estado">string - placa</param>
        /// ***********************************************************************************************
        public static DataSet getPagosDiferidosPorPlaca(int? zona, int? estacion, int? via, DateTime fechaDesde, DateTime fechaHasta, string operador, string placa)
        {
            DataSet dsPagosDif = null;

            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    dsPagosDif = PagoDiferidoDt.getPagosDiferidosPorPlaca(conn, zona, estacion, via, fechaDesde, fechaHasta, operador, placa);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsPagosDif;
        }


    }
}
