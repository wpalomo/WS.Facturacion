using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Utilitarios.SL
{
    public class Fechas
    {
        #region Fechas: Tratamientos de Fechas

        /// <summary>
        /// Tiempo de diferencia entre el servidor y la terminal
        /// </summary>
        public static TimeSpan DiferenciaHoraServidor { get; set; }

        /// <summary>
        /// FUNCIONALIDAD ...: Retorna la fecha actual en formato DD/MM/AAAA
        /// AUTOR ...........: Cristian Binaghi
        /// FECHA CREACIÓN ..: 30/07/2009
        /// ULT.FEC.MODIF. ..:
        /// OBSERVACIONES ...:
        /// </summary>
        /// <returns></returns>
        public static string hoyDDMMAAAA()
        {
            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Day.ToString().PadLeft(2, '0') + '/' + dtNow.Month.ToString().PadLeft(2, '0') + '/' + dtNow.Year.ToString();
        }

        /// <summary>
        /// FUNCIONALIDAD ...: Retorna la fecha actual en formato DDMMAAAA
        /// AUTOR ...........: Cristian Binaghi
        /// FECHA CREACIÓN ..: 07/09/2009
        /// ULT.FEC.MODIF. ..:
        /// OBSERVACIONES ...:
        /// </summary>
        /// <returns></returns>
        public static string hoyDDMMAAAASinBarras()
        {
            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Day.ToString().PadLeft(2, '0') + dtNow.Month.ToString().PadLeft(2, '0') + dtNow.Year.ToString();
        }

        #endregion
        
        #region Horas: Tratamientos de Time

        /// <summary>
        /// FUNCIONALIDAD ...: Retorna la hora actual en formato HH:MM:SS
        /// AUTOR ...........: Cristian Binaghi
        /// FECHA CREACIÓN ..: 30/07/2009
        /// ULT.FEC.MODIF. ..:
        /// OBSERVACIONES ...:
        /// </summary>
        /// <returns></returns>
        public static string hoyHHMMSS()
        {
            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Hour.ToString().PadLeft(2, '0') + ':' + dtNow.Minute.ToString().PadLeft(2, '0') + ':' + dtNow.Second.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// Retorna la hora actual
        /// </summary>
        /// <returns></returns>
        public static DateTime Hoy()
        {
            return DateTime.Now + DiferenciaHoraServidor;
        }


        /// <summary>
        /// FUNCIONALIDAD ...: Retorna la hora actual en formato HHMMSS
        /// AUTOR ...........: Cristian Binaghi
        /// FECHA CREACIÓN ..: 07/09/2009
        /// ULT.FEC.MODIF. ..:
        /// OBSERVACIONES ...:
        /// </summary>
        /// <returns></returns>
        public static string hoyHHMMSSSinPuntos()
        {
            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Hour.ToString().PadLeft(2, '0') + dtNow.Minute.ToString().PadLeft(2, '0') + dtNow.Second.ToString().PadLeft(2, '0');
        }

        /*
        public static bool IsDate(string expression)
        {
            if (expression == null) { return false; }

            try
            {
                DateTime dateTime = DateTime.Parse(expression);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
        */

        #endregion
    }
}
