using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using Telectronica.Peaje;
using Telectronica.Validacion;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class MotivoRechazoBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Motivos de Rechazos definidos. 
        /// </summary>
        /// <returns>Lista de MotivoRechazo</returns>
        /// ***********************************************************************************************
        public static MotivoRechazoL getMotivosRechazos()
        {

            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return MotivoRechazoDt.getMotivosRechazos(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
