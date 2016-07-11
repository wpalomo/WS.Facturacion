using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class Nivel2Bs
    {

        #region TRANSITOS

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Transitos por Forma de Pago para una Estación/Vía
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 15/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - int? - Permite filrar por código de estación
        //                                  xiVia - int? - Permite filrar por código de vía
        // ----------------------------------------------------------------------------------------------

        public static Nivel2_TrcXFpL getTrcXFp(bool bGST, int xiEstacion, int xiVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(bGST, false);
                    return Nivel2Dt.getTrcXFp(conn, xiEstacion, xiVia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region EXENTOS

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Exentos para el Nivel 2
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 19/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - int? - Permite filrar por código de estación
        //                                  xiVia - int? - Permite filrar por código de vía
        // ----------------------------------------------------------------------------------------------

        public static Nivel2_ExentoL getExentos(bool bGST, int xiEstacion, int xiVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(bGST, false);
                    return Nivel2Dt.getExentos(conn, xiEstacion, xiVia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region VENTAS

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de Ventas para el Nivel 2
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 19/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - int? - Permite filrar por código de estación
        //                                  xiVia - int? - Permite filrar por código de vía
        // ----------------------------------------------------------------------------------------------

        public static Nivel2_VentasL getVentas(bool bGST, int xiEstacion, int xiVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(bGST, false);
                    return Nivel2Dt.getVentas(conn, xiEstacion, xiVia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DIFERENCIAS entre lo que categoriza el Operador y el Dac
         
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de diferencias entre lo cat. por el operador y el dac
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 20/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - int? - Permite filrar por código de estación
        //                                  xiVia - int? - Permite filrar por código de vía
        // ----------------------------------------------------------------------------------------------

        public static Nivel2_OpeDacL getDiferencias(bool bGST, int xiEstacion, int xiVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(bGST, false);
                    return Nivel2Dt.getDiferencias(conn, xiEstacion, xiVia);
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
