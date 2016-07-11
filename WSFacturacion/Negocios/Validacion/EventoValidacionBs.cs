using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class EventoValidacionBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la el contexto de una anomalia
        /// </summary>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static DataSet getContextoAnomalia(int estacion, int via, DateTime fechaDesde, DateTime fechaHasta)
        {
            DataSet contexto = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    contexto = EventoValidacionDt.getContextoAnomalia(conn, estacion, via, fechaDesde, fechaHasta, ConexionBs.getPerfil());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contexto;
        }
        


        /// <summary>
        ///  Obtiene la historia de rechazos de una placa y un tag.
        /// </summary>
        /// <param name="placa">Patente</param>
        /// <param name="ntag">Numero de Tag</param>
        public static DataSet getHistoriaRechazos (string placa, string ntag, string emisor, DateTime fecha)
        {
            DataSet historia = null;
            try
            {
                using (Conexion con = new Conexion())
                {
                    con.ConectarConsolidado(false);
                    historia = EventoValidacionDt.getHistoriaRechazos(con, placa, fecha, ntag, emisor);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return historia;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de transitos del parte
        /// </summary>
        /// <returns>Lista de Transitos</returns>
        /// ***********************************************************************************************
        public static DataSet getTransitos(int estacion, List<int> vias, int parte, DateTime fechaDesde, DateTime fechaHasta, int perfil)
        {
            DataSet transitos = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    transitos = EventoValidacionDt.getTransitos(conn, estacion, vias, parte, fechaDesde, fechaHasta, perfil);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transitos;
        }
    }
}
