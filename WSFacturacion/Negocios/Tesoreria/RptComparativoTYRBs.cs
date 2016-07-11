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
    /// Clase que trae los datos para Reportes de Detalle de Recaudacion
    /// </summary>
    /// ***********************************************************************************************
    public class RptComparativoTYRBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los traficos y tarifas por jornada
        /// </summary>
        /// <param name="jornada">DateTime? - Jornada</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turno">int? - Turno</param>
        /// ***********************************************************************************************
        public static DataSet getTraficosYRecaudo(DateTime? jornada, int? zona, int? estacion, int? turno)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    //DataSet inicial
                    DataSet dsTraficos = RptComparativoTYRDt.getTraficosYRecaudo(conn, jornada, zona, estacion, turno);

                    //Totales de Transitos
                    RptComparativoTYRDt.getTarifas(conn, dsTraficos, estacion, jornada);

                    return dsTraficos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los carriles segun el estado solicitado
        /// </summary>
        /// <param name="jornada">DateTime? - Jornada</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="Abiertos">bool - True Estado Abierto, Cerrado</param>
        /// ***********************************************************************************************
        public static Int32 getCantCarrilesXEst(int? zona, int? estacion, DateTime? jornada, bool Abiertos)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    //DataSet inicial
                    DataSet dsTraficos = RptComparativoTYRDt.getCantCarrilesXEst(conn, jornada, zona, estacion, Abiertos);

                    return dsTraficos.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
