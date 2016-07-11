using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public class ViaInformacionBs
    {
        #region ViaInformacion: Metodos para la clase ViaInformacion

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de objetos ViaInformacion
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 28/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  bGST - bool - Indica si se debe conectar con Gestion o Estacion
        //                                  xiCodEst - int - Permite filtrar por numero de estacion
        //                                  xiVia - int - Permite filtrar por número de vía
        //                    Retorna: Lista de ViaInformacion: ViaInformacionL
        // ----------------------------------------------------------------------------------------------
        public static ViaInformacionL getViaInformacion(bool bGST, int iCodEst, int? iVia)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bGST, false);
                return ViaInformacionDt.getViaInformacion(conn, iCodEst, iVia);
            }
        }

        #endregion

        #region ViaInformacionTotal: Metodos para la clase ViaInformacionTotal

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de objetos ViaInformacionTotal
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 29/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  bGST - bool - Indica si se debe conectar con Gestion o Estacion
        //                                  xiCodEst - int - Permite filtrar por numero de estacion
        //                                  xiVia - int - Permite filtrar por número de vía
        //                    Retorna: Lista de ViaInformacionTotal: ViaInformacionTotalL
        // ----------------------------------------------------------------------------------------------
        public static ViaInformacionTotalL getViaInformacionTotal(bool bGST, int iCodEst, int? iVia)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bGST, false);
                return ViaInformacionDt.getViaInformacionTotal(conn, iCodEst, iVia);
            }
        }

        #endregion

        #region ESTADO DE LA LISTA NEGRA DE LAS VIAS

        /// <summary>
        /// Devuelve una lista con los estados de las listas negras de cada via
        /// </summary>
        /// <param name="iCoest"></param>
        /// <param name="iVia"></param>
        /// <returns></returns>
        public static DataSet GetViaEstadosListaNegra(int? iVia)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarPlaza(false, false);
                return ViaInformacionDt.GetViaEstadosListaNegra(conn, iVia);
            }
        }

        public static DataSet GetViaEstadosListaNegra()
        {
            return GetViaEstadosListaNegra(null);
        }

        #endregion
    }
}
