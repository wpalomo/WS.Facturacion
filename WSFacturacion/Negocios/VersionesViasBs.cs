using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class VersionesViasBs
    {

        /// <summary>
        /// Retorna la lista de los registros de versiones de vias
        /// </summary>
        /// <returns></returns>
        public static VersionViaL GetVersionesVias(int? numeroEstacion, int? numeroVia, string codigoTipoPrograma)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return VersionesViasDt.GetVersionesVias(conn, numeroEstacion, numeroVia, codigoTipoPrograma);
            }
        }

    }
}
