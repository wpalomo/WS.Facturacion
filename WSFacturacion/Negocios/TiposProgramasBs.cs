using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class TiposProgramasBs
    {

        /// <summary>
        /// Retorna la lista de los registros de tipos de programas
        /// </summary>
        /// <returns></returns>
        public static TipoProgramaL GetTiposProgramas(string codigoTipoPrograma)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return TiposProgramasDt.GetTiposProgramas(conn, codigoTipoPrograma);
            }
        }

    }
}
