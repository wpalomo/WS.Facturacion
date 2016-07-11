using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class AlarmaRetiroBS
    {

        #region ALARMAS DE RETIROS: Metodos de la Clase Alarma de Retiro.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las alarmas de retiros vigentes
        /// </summary>
        /// <returns>Lista de Alarmas</returns>
        /// ***********************************************************************************************
        public static AlarmaRetiroL getAlarmasRetiro()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return AlarmaRetiroDT.getAlarmasRetiro(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de retiros pendientes de realizar.
        /// </summary>
        /// <returns>Int</returns>
        /// ***********************************************************************************************
        public static int getHayAlarmasRetiro()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return AlarmaRetiroDT.getHayAlarmasRetiro(conn);
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
