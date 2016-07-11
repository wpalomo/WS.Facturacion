using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class ValEstacionesBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve true si esta habilitada la categoria para una forma de pago
        /// </summary>
        /// ***********************************************************************************************

        public static bool getCategFormaPagoHabil(string tipop, string tipbo, int categoria)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ValEstacionesDt.getCategFormaPagoHabil(conn, tipop, tipbo, categoria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CodigoValidacionL getCodigosAceptacionRechazo()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return ValEstacionesDt.getCodigosAceptacionRechazo(conn);
            }

        }
    }
}
