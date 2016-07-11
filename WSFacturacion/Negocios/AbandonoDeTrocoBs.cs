using System;
using System.Data;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase de negocios para Abandono de Troco
    /// </summary>
    public class AbandonoDeTrocoBs
    {
        /// <summary>
        /// Devuelve un objeto AbandonoDeTroco que contiene la información necesaria donde se produjo el mismo, para luego registrarla.
        /// </summary>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="iNumeroVia"></param>
        /// <returns></returns>
        public static AbandonoDeTroco ObtenerAbandonoDeTrocoOrigen(int iNumeroEstacion, int iNumeroVia)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarPlaza(ConexionBs.getGSToEstacion(), false);
                return AbandonoDeTrocoDt.getAbandonoDeTrocoOrigen(conn, iNumeroEstacion, iNumeroVia);
            }
        }

        /// <summary>
        /// Obtiene todos los Abandonos de Troco con filtros
        /// </summary>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="dFechaDesde"></param>
        /// <param name="dFechaHasta"></param>
        /// <param name="sSentido"></param>
        /// <param name="iNumeroVia"></param>
        /// <param name="sSupervisor"></param>
        /// <param name="sPeajista"></param>
        /// <param name="sRendidos"></param>
        /// <returns></returns>
        public static AbandonoDeTrocoL ObtenerAbandonoDeTroco(int? iNumeroEstacion, DateTime? dFechaDesde, DateTime? dFechaHasta, string sSentido, int? iNumeroVia, string sSupervisor, string sPeajista, string sRendidos)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarPlaza(ConexionBs.getGSToEstacion(), false);
                return AbandonoDeTrocoDt.getAbandonoDeTroco(conn, null, iNumeroEstacion, dFechaDesde, dFechaHasta, sSentido, iNumeroVia, sSupervisor, sPeajista, sRendidos);
            }
        }

        /// <summary>
        /// Obtiene un DataSet con todos los Abandonos de Troco con filtros
        /// </summary>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="dFechaDesde"></param>
        /// <param name="dFechaHasta"></param>
        /// <param name="sSentido"></param>
        /// <param name="iNumeroVia"></param>
        /// <param name="sSupervisor"></param>
        /// <param name="sPeajista"></param>
        /// <param name="sRendidos"></param>
        /// <returns></returns>
        public static DataSet ObtenerRptAbandonoDeTroco(int? iNumeroEstacion, DateTime? dFechaDesde, DateTime? dFechaHasta, string sSentido, int? iNumeroVia, string sSupervisor, string sPeajista, string sRendidos)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarPlaza(ConexionBs.getGSToEstacion(), false);
                return AbandonoDeTrocoDt.getRptAbandonoDeTroco(conn, null, iNumeroEstacion, dFechaDesde, dFechaHasta, sSentido, iNumeroVia, sSupervisor, sPeajista, sRendidos);
            }
        }

        /// <summary>
        /// Elimina un abandono de Troco
        /// </summary>
        /// <param name="abandonoDeTroco"></param>
        public static void EliminarAbandonoDeTroco(AbandonoDeTroco abandonoDeTroco)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarPlaza(ConexionBs.getGSToEstacion(), false);
                AbandonoDeTrocoDt.delAbandonoDeTroco(conn, abandonoDeTroco);
            }
        }

        /// <summary>
        /// Agrega un abandono de Troco
        /// </summary>
        /// <param name="abandonoDeTroco"></param>
        public static void AgregarAbandonoDeTroco(AbandonoDeTroco abandonoDeTroco)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarPlaza(ConexionBs.getGSToEstacion(), false);
                AbandonoDeTrocoDt.addAbandonoDeTroco(conn, abandonoDeTroco);
            }
        }
    }
}