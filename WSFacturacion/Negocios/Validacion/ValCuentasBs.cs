using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Facturacion;

namespace Telectronica.Validacion
{
    public class ValCuentasBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos de la cuenta por patente
        /// </summary>
        /// ***********************************************************************************************
        public static VehiculoL GetDatosCuentaPorPatente(string patente, int estacion, string sTipop)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return ValCuentasDt.GetDatosCuentaPorPatente(conn, patente, estacion, sTipop);
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos de la cuenta por tag
        /// </summary>
        /// ***********************************************************************************************
        public static VehiculoL GetDatosCuentaPorTag(string emisor, string tag, int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return ValCuentasDt.GetDatosCuentaPorTag(conn, emisor, tag, estacion);
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
