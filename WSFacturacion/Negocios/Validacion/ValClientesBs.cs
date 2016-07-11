using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class ValClientesBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de ultimos transitos
        /// </summary>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static DataSet getUltimosTransitos(string patente, DateTime fecha, int top)
        {
            DataSet transitos = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    transitos = ValClientesDt.getUltimosTransitos(conn, patente, fecha, top);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return transitos;
        }
    }
}
