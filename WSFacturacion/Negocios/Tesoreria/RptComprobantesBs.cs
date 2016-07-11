using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Tesoreria
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Detalle de una Apropiacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptComprobantesBs
    {
        #region APROPIACIÓN

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con la apropiación correspondiente y sus detalles
        /// </summary>
        /// <param name="oApropiacion">int - Numero de Apropiacion</param>
        /// ***********************************************************************************************
        public static DataSet getApropiacionCompleta(Apropiacion oApropiacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DataSet dsApropiacion = RptComprobantesDT.getBolsas(conn, oApropiacion.Estacion.Numero, oApropiacion.NumeroApropiacion);
                RptComprobantesDT.getDetalle(conn, dsApropiacion, oApropiacion.Estacion.Numero, oApropiacion.NumeroApropiacion);
                return dsApropiacion;
            }
        }

        #endregion

        #region LIQUIDACIÓN

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene un DataSet Con la Liquidación
        /// </summary>
        /// <param name="oApropiacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getLiquidacionCompleta(MovimientoCajaLiquidacion liquidacion)
        {
            using (Conexion conn = new Conexion())
            {                
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                DataSet dsPartes = RptComprobantesDT.getLiquidacion(conn, liquidacion.Estacion.Numero, liquidacion.Parte.Numero);

                RptDetalleParteJornadaDt.getDenominaciones(conn, dsPartes, liquidacion.Parte.Numero, null, null,
                    null, liquidacion.Parte.Estacion.Numero, "PARTE", null, null, true, true);

                RptDetalleParteJornadaDt.getBolsas(conn, dsPartes, liquidacion.Parte.Numero, null, null,
                    null, liquidacion.Parte.Estacion.Numero, "PARTE", null, null, true, true);

                RptDetalleParteJornadaDt.getCuponLiquidacion(conn, dsPartes, liquidacion.Parte.Numero, null, null, null,
                    liquidacion.Parte.Estacion.Numero, "PARTE", null, null, false, true);

                return dsPartes;
            }
        }

        #endregion

        #region REPOSICIÓN

        /// <summary>
        /// Obtiene un DataSet con los pagos de reposiciones
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="sEstado"></param>
        /// <param name="dtFechaDesde"></param>
        /// <param name="dtFechaHasta"></param>
        /// <param name="iMalote"></param>
        /// <returns></returns>
        public static DataSet ObtenerRptPagoDeReposiciones(ReposicionPedidaL reposicionesPagadas)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarPlaza(ConexionBs.getGSToEstacion(), false);
                return RptComprobantesDT.getReposicionesPedidas(conn, reposicionesPagadas);
            }
        }

        #endregion
    }
}
