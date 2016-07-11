using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class StatusTratamientoBs
    {

        public static StatusTratamientoL getTransitosPexTotalesPorEstado(DateTime desde, DateTime hasta, int? administradora, int? estacion)
        {
            StatusTratamientoL transitosPexTotalesPorEstado = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    transitosPexTotalesPorEstado = StatusTratamientoDt.getTransitosPexTotalesPorEstado(conn, desde, hasta, administradora, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return transitosPexTotalesPorEstado;
        }

    }
}
