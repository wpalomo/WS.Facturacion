using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class AnomaliasBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de formas de pago
        /// </summary>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static FormaPagoValidacionL getForpagAConsolidar(int? codAnomalia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return AnomaliasDt.getForpagAConsolidar(conn, codAnomalia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
