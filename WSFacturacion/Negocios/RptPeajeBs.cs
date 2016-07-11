using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Threading;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Peaje
    /// </summary>
    /// ***********************************************************************************************
    public class RptPeajeBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con loa bloques sin liquidar
        /// </summary>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// ***********************************************************************************************
        public static DataSet getBloquesSinLiquidar(int? zona, int? estacion, DateTime fechahoraDesde, DateTime fechahoraHasta)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptPeajeDt.getBloquesSinLiquidar(conn, zona, estacion, fechahoraDesde, fechahoraHasta);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con loa bloques abiertos 
        /// </summary>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="modoIngreso">string - Modo de Ingreso</param>
        /// <param name="operador">string - operador</param>
        /// ***********************************************************************************************
        public static DataSet getViasAbiertas(int? zona, int? estacion, int? via, DateTime fechahoraDesde, DateTime fechahoraHasta, string modoIngreso, int? causaCierre, string operador, string sModoParte, string sSenti)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptPeajeDt.getViasAbiertas(conn, zona, estacion, via, fechahoraDesde, fechahoraHasta, modoIngreso, causaCierre, operador, sModoParte, sSenti);
            }         
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con loa Quiebres de barrera 
        /// </summary>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="modoQuiebre">string - Modo de Quiebre</param>
        /// <param name="operador">string - operador</param>
        /// ***********************************************************************************************
        public static DataSet getQuiebresBarrera(int? zona, int? estacion, int? via, DateTime fechahoraDesde, DateTime fechahoraHasta, string modoQuiebre, string operador)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptPeajeDt.getQuiebresBarrera(conn, zona, estacion, via, fechahoraDesde, fechahoraHasta, modoQuiebre, operador);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con loa Cambios de Estado 
        /// </summary>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// ***********************************************************************************************
        public static DataSet getCambiosEstado(int? zona, int? estacion, int? via, DateTime fechahoraDesde, DateTime fechahoraHasta)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptPeajeDt.getCambiosEstado(conn, zona, estacion, via, fechahoraDesde, fechahoraHasta);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los Eventos 
        /// </summary>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="oVias">ViaL - Numeros de Via</param>
        /// <param name="oClaveEventos">ClaveEventoL - Codigos de Evento</param>
        /// <param name="patente">string - Patente</param>
        /// ***********************************************************************************************
        public static DataSet getEventos(int? estacion, DateTime fechahoraDesde, DateTime fechahoraHasta, ViaL oVias, ClaveEventoL oClaveEventos, string patente)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RptPeajeDt.getEventos(conn, estacion, fechahoraDesde, fechahoraHasta, oVias, oClaveEventos, patente, ConexionBs.getUsuario());
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con las Fechas Minimas y Maximas de Bloques y Transitos 
        /// </summary>
        /// ***********************************************************************************************
        public static DataSet getFechasMinMax()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion   
                //en conslidado
                conn.ConectarConsolidado(false);
                return RptPeajeDt.getFechasMinMax(conn);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la ultima fecha de replicacion de maestros
        /// </summary>
        /// ***********************************************************************************************
        public static DateTime getFechaMaestros()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion   
                //en la plaza
                conn.ConectarPlaza(true);
                return RptPeajeDt.getFechaMaestros(conn);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de eventos de video por dia y plaza
        /// <param name="fecha">DateTime - Fecha Hasta</param>
        /// </summary>
        /// ***********************************************************************************************
        public static VideoCantidadesL getCantidadVideos(DateTime fecha)
        {
            VideoCantidadesL oVideos = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion   
                    //en conslidado
                    conn.ConectarConsolidado(false);
                    oVideos = RptPeajeDt.getCantidadVideos(conn, fecha, VideoCantidades.CantidadDias);

                    //    try
                    //    {
                    //        foreach (VideoCantidades oPlaza in oVideos)
                    //        {
                    //            foreach (VideoFecha oFech in oPlaza.Videos)
                    //            {
                    //                oFech.CantidadArchivos = Util.getCantidadArchivos(oFech.Path, "wmv|avi|bmp|jpg");
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //
                    //    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideos;
        }
        ///// ***********************************************************************************************
        ///// <summary>
        ///// Devuelve la cantidad de eventos de video por dia y plaza
        ///// <param name="fecha">DateTime - Fecha Hasta</param>
        ///// </summary>
        ///// ***********************************************************************************************
        //public static VideoCantidadesL getCantidadVideos(DateTime fecha, bool bForzar)
        //{
        //    VideoCantidadesL oVideos = null;

        //    if (HttpContext.Current.Cache["CantidadVideos"+fecha.ToShortDateString()] != null && !bForzar)
        //        oVideos = (VideoCantidadesL)HttpContext.Current.Cache["CantidadVideos"+fecha.ToShortDateString()];
        //    else
        //    {
        //        try
        //        {
        //            using (Conexion conn = new Conexion())
        //            {
        //                //sin transaccion   
        //                //en conslidado
        //                conn.ConectarConsolidado(false);
        //                oVideos = RptPeajeDt.getCantidadVideos(conn, fecha, VideoCantidades.CantidadDias);
        //            }
        //            //Guardamos en cache, 3 minutos para que si en un rato no se actualiza con la cantidad no se vuelva a generar
        //            HttpContext.Current.Cache.Add("CantidadVideos" + fecha.ToShortDateString(), oVideos, null, DateTime.Now.AddMinutes(3), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

        //            ThreadPool.QueueUserWorkItem(new WaitCallback(getCantidadVideosAsync), fecha);
        //        }
        //        catch (Exception)
        //        {
        //            //
        //        }

        //    }
        //    return oVideos;
        //}

        //public static void getCantidadVideosAsync(Object oParam)
        //{

        //    DateTime fecha = (DateTime)oParam;
            

        //    try
        //    {

        //        VideoCantidadesL oVideos = null;
        //        oVideos = (VideoCantidadesL)HttpRuntime.Cache["CantidadVideos" + fecha.ToShortDateString()];

        //        if (oVideos != null)
        //        {
        //            foreach (VideoCantidades oPlaza in oVideos)
        //            {
        //                foreach (VideoFecha oFech in oPlaza.Videos)
        //                {
        //                    oFech.CantidadArchivos = Util.getCantidadArchivos(oFech.Path, "wmv|avi|bmp|jpg|mp4");
        //                }
        //            }
        //            //Guardamos en cache
        //            HttpRuntime.Cache.Add("CantidadVideos" + fecha.ToShortDateString(), oVideos, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //
        //    }

        //}
        public static DataSet getValesPrepagos(DateTime Desde, DateTime Hasta, int? Cliente, int? Zona, int? Estacion)
        {
            DataSet oValesPrepagos = null;

            using (Conexion conn = new Conexion())
            {
                //sin transaccion   
                //en conslidado
                conn.ConectarGSTThenPlaza();
                oValesPrepagos = RptPeajeDt.getValesPrepagos(conn, Desde, Hasta, Cliente, Zona, Estacion);
            }
            return oValesPrepagos;
        }
        public static DataSet getTransitosValesPrepagos(DateTime Desde, DateTime Hasta, int? Cliente, int? Zona, int? Estacion, int? SerieDesde, int? SerieHasta, int? Categoria)
        {
            DataSet oValesPrepagos = null;
            
            using (Conexion conn = new Conexion())
            {
                //sin transaccion   
                //en conslidado
                conn.ConectarConsolidado(false);
                oValesPrepagos = RptPeajeDt.getTransitosValesPrepagos(conn, Desde, Hasta, Cliente, Zona, Estacion, SerieDesde, SerieHasta, Categoria);
            }
            return oValesPrepagos;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los logos que se utilizaran en las cabeceras de los reportes dentro de un dataset.
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getLogosCabeceras()
        {
            return getLogosCabeceras(mdlGeneral.getPathImagenesVarias(true));
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los logos que se utilizaran en las cabeceras de los reportes dentro de un dataset.
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getLogosCabeceras(string path)
        {
            DataSet oLogosCabeceras = RptPeajeDt.getLogosCabeceras(path);
            return oLogosCabeceras;
        }
    }
}
