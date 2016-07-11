using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class TipoListaBs
    {

        #region TIPOS DE LISTAS

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de los Tipos de Lista para ser importados. Utilizado en los Comandos
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 21/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...:
        // ----------------------------------------------------------------------------------------------

        public static TipoListaL getTiposLista()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return TipoListaDt.getTiposLista(conn);
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
