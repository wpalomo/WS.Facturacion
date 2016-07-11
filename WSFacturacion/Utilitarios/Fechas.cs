using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Utilitarios
{
    public class Fechas
    {

        #region Fechas: Tratamientos de Fechas

        //Tiempo de diferencia entre el servidor y la terminal
        public static TimeSpan DiferenciaHoraServidor { get; set; }

        public static string hoyDDMMAAAA()
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Retorna la fecha actual en formato DD/MM/AAAA
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 30/07/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Day.ToString().PadLeft(2, '0') + '/' + dtNow.Month.ToString().PadLeft(2, '0') + '/' + dtNow.Year.ToString();
        }

        public static string hoyDDMMAAAASinBarras()
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Retorna la fecha actual en formato DDMMAAAA
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 07/09/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------
            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Day.ToString().PadLeft(2, '0') + dtNow.Month.ToString().PadLeft(2, '0') + dtNow.Year.ToString();
        }

        #endregion


        #region Horas: Tratamientos de Time

        public static string hoyHHMMSS()
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Retorna la hora actual en formato HH:MM:SS
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 30/07/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Hour.ToString().PadLeft(2, '0') + ':' + dtNow.Minute.ToString().PadLeft(2, '0') + ':' + dtNow.Second.ToString().PadLeft(2, '0');
        }

        public static string hoyHHMMSSSinPuntos()
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Retorna la hora actual en formato HHMMSS
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 07/09/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            DateTime dtNow = DateTime.Now + DiferenciaHoraServidor;
            return dtNow.Hour.ToString().PadLeft(2, '0') + dtNow.Minute.ToString().PadLeft(2, '0') + dtNow.Second.ToString().PadLeft(2, '0');
        }

        //////public static bool IsDate(string expression)
        //////{
        //////    if (expression == null) { return false; }

        //////    try
        //////    {
        //////        DateTime dateTime = DateTime.Parse(expression);
        //////    }
        //////    catch (FormatException)
        //////    {
        //////        return false;
        //////    }

        //////    return true;
        //////}


        #endregion
    }
}
