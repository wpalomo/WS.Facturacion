using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class EventoBs
    {
        #region CLAEVE

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Codigos de Evento
        /// </summary>
        /// <returns>Lista de Codigos de Evento</returns>
        /// ***********************************************************************************************
        public static ClaveEventoL getClavesEvento()
        {
            return getClavesEvento(null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Codigos de Evento
        /// </summary>
        /// <param name="codigo">Int - Permite filtrar por un codigo determinado
        /// <returns>Lista de Codigos de Evento</returns>
        /// ***********************************************************************************************
        public static ClaveEventoL getClavesEvento(int? codigo)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    int? estacion=null;
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    if (!ConexionBs.getGSToEstacion())
                        estacion = ConexionBs.getNumeroEstacion();
                    return EventoDt.getClavesEvento(conn, codigo, estacion, ConexionBs.getUsuario());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region TipoEvento

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de todos los Tipos de Evento
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 10/08/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...:
        // ----------------------------------------------------------------------------------------------

        public static TipoEventoL getTiposEvento()
        {
            return getTiposEvento(null, null);
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de todos los Tipos de Evento
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 10/08/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xTipo - string - Permite filrar por código de evento
        // ----------------------------------------------------------------------------------------------

        public static TipoEventoL getTiposEvento(string xTipo, int? xiCodEstacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return EventoDt.getTiposEvento(conn, xTipo, xiCodEstacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
