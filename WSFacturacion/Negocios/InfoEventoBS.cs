using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using System.Web.Configuration;
namespace Telectronica.Peaje
{
    public class InfoEventoBs
    {
        #region InfoEvento: Metodos para la clase InfoEvento

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de eventos, por defecto devuelve hasta 200 registros
        /// </summary>
        /// <param name="bGST">Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="iCodEst">Permite filtrar por numero de estacion</param>
        /// <param name="dFechaDesde">Permite filtrar por período desde</param>
        /// <param name="dFechaHasta">Permite filtrar por período hasta</param>
        /// <param name="iMinIdent">Permite filtrar a partir de que nro. de identidad</param>
        /// <param name="sVias">Permite filtrar para una lista de vias</param>
        /// <param name="sTiposEventos">Filtra por tipo de eventos</param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static InfoEventoL getInfoEventos(bool bGST, int iCodEst, DateTime? dFechaDesde, DateTime? dFechaHasta, int? iMinIdent, string sVias, string sTiposEventos, ref bool llegoAlTope)
        {
            return getInfoEventos(bGST, iCodEst, dFechaDesde, dFechaHasta, iMinIdent, sVias, sTiposEventos, 200, ref llegoAlTope);
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de eventos
        /// </summary>
        /// <param name="bGST">Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="iCodEst">Permite filtrar por numero de estacion</param>
        /// <param name="dFechaDesde">Permite filtrar por período Desde</param>
        /// <param name="dFechaHasta">Permite filtrar por período Hasta</param>
        /// <param name="iMinIdent">Permite filtrar a partir de que nro. de identidad</param>
        /// <param name="sVias">Permite filtrar para una lista de vias</param>
        /// <param name="sTiposEventos">Permite filtrar para una lista de eventos</param>
        /// <param name="iCantRows">Cantidad de registros pedidos</param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static InfoEventoL getInfoEventos(bool bGST, int iCodEst, DateTime? dFechaDesde, DateTime? dFechaHasta, int? iMinIdent, string sVias, string sTiposEventos, int iCantRows, ref bool llegoAlTope)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bGST, false);

                ViaL lListaVias = new ViaL();
                TipoEventoL lListaEventos = new TipoEventoL();

                // PASAR DE LA LISTA DE STRING A LA LISTA DE OBJETOS CORRESPONDIENTES
                // VIAS
                string[] auxVias = sVias.Split(',');

                if (auxVias[0] != "")
                {
                    foreach (string word in auxVias)
                    {
                        lListaVias.Add(new Via(iCodEst, Convert.ToByte(word),""));
                    }
                }

                // EVENTOS
                string[] auxEventos = sTiposEventos.Split(',');

                if (auxEventos[0] != "")
                {
                    foreach (string word in auxEventos)
                    {
                        lListaEventos.Add(new TipoEvento(word));
                    }
                }

                return InfoEventoDt.getInfoEventos(conn, iCodEst, dFechaDesde, dFechaHasta, iMinIdent, lListaVias, lListaEventos, iCantRows, ref llegoAlTope);
            }
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de eventos para el comentario del supervisor
        /// </summary>
        /// <param name="bGST">Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="iCodEst">Permite filtrar por numero de estacion</param>
        /// <param name="dFechaDesde">Permite filtrar por período Desde</param>
        /// <param name="dFechaHasta">Permite filtrar por período Hasta</param>
        /// <param name="iMinIdent">Permite filtrar a partir de que nro. de identidad</param>
        /// <param name="sVias">Permite filtrar para una lista de vias</param>
        /// <param name="sTiposEventos">Permite filtrar para una lista de eventos</param>
        /// <param name="iCantRows">Cantidad de registros pedidos</param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static InfoEventoL getInfoEventosComentariosSup(bool bGST, int iCodEst, DateTime? dFechaDesde, DateTime? dFechaHasta, int? iMinIdent, string sVias, string sEstadoComent, int iCantRows, ref bool llegoAlTope)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bGST, false);

                ViaL lListaVias = new ViaL();
                TipoEventoL lListaEventos = new TipoEventoL();

                // PASAR DE LA LISTA DE STRING A LA LISTA DE OBJETOS CORRESPONDIENTES
                // VIAS
                string[] auxVias = sVias.Split(',');

                if (auxVias[0] != "")
                {
                    foreach (string word in auxVias)
                    {
                        lListaVias.Add(new Via(iCodEst, Convert.ToByte(word), ""));
                    }
                }

                return InfoEventoDt.getInfoEventosComentariosSup(conn, iCodEst, dFechaDesde, dFechaHasta, iMinIdent, lListaVias, sEstadoComent, iCantRows, ref llegoAlTope);
            }
        }
        
        ///// ----------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Devuelve una lista de eventos para el comentario del supervisor
        ///// </summary>
        //public static List<String> getImagenes(int iCoest, int iNroVia, int iNroEvento)
        //{
        //    using (Conexion conn = new Conexion())
        //    {
        //        conn.ConectarPlaza(false);
        //        return InfoEventoDt.getImagenes(conn, iCoest, iNroVia, iNroEvento);
        //    }          
        //}



        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de nombres de imagenes, que corresponden a un transito
        /// </summary>
        /// ----------------------------------------------------------------------------------------------
        public static ImagenL getImagenes(int xiCodEst, int xiVia, int nEvento, string sFotos, bool bCCO)
        {
            using (Conexion conn = new Conexion())
            {
                //Si es de otra estacion hay que obtenerlo del CCO
                bool bGST = false;
                if (ConexionBs.getGSToEstacion() || xiCodEst != ConexionBs.getNumeroEstacion())
                    bGST = true;

                //conn.ConectarGSTPlaza(bGST, false);                
                conn.ConectarConsolidadoPlaza(bGST, false);

                // Si se define por configuración buscar videos en la estacion,
                // se usa el SP de Consolidado que busca en BORVID filtrando la estacion que corresponda
                // sino, se usa el SP de Consolidado que busca en BORVIDCCO
                if (!bCCO && Convert.ToBoolean(WebConfigurationManager.AppSettings["VideoLocalEnLaEstacion"]))
                    return InfoEventoDt.getImagenesCCOEstacion(conn, xiCodEst, xiVia, nEvento, sFotos);
                else
                    return InfoEventoDt.getImagenes(conn, xiCodEst, xiVia, nEvento, sFotos);
            }

        }




        #endregion

        #region INFOEVENTOS SUPERVISION REMOTA
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de eventos
        // AUTOR ...........: Pablo Barrera
        // FECHA CREACIÃ“N ..: 12/08/2014
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: ParÃ¡metros: 
        //                                  bGST - bool - Indica si se debe conectar con Gestion o Estacion
        //                                  estacion - int - Permite filtrar por numero de estacion
        //                                  nuvia - int - Permite filtrar por numero de via
        //                    Retorna: Lista de InfoEvento: InfoEventoL
        // ----------------------------------------------------------------------------------------------

        public static InfoEventoL getInfoEventosSupervision(bool bGST, int estacion, int nuvia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(bGST,false, false);

                    return InfoEventoDt.getInfoEventosSupervision(conn, estacion, nuvia);
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
