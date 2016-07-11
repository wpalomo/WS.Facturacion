using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region BANDAHORARIADETALLE: Clase para entidad de detalle de las Bandas Horarias para los diferentes tipos de dia de la semana

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad BandaHorariaDetalle
    /// </summary>*********************************************************************************************

    [Serializable]

    public class BandaHorariaDetalle
    {

        // Constructor vacio, por defecto
        public BandaHorariaDetalle()
        { 
        }


        // En el constructor asignamos los valores a la clase
        public BandaHorariaDetalle(TipoDiaHora tipoDiaHora, DiaSemana diaSemana, 
                                   string horaInicial, string horaFinal)
        {
            this.TipoDiaHora = tipoDiaHora;
            this.DiaSemana = diaSemana;
            this.HoraInicial = horaInicial;
            this.HoraFinal = horaFinal;
        }


        // Tipo de dia definido para esta banda
        public TipoDiaHora TipoDiaHora { get; set; }

        // Dia de la semana
        public DiaSemana DiaSemana { get; set; }

        // Hora inicial de vigencia de esa banda (para ese dia)
        public string HoraInicial { get; set; }

        // Hora final de vigencia de esa banda (para ese dia)
        public string HoraFinal { get; set; }
    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos BandaHorariaDetalle
    /// </summary>*********************************************************************************************
    public class BandaHorariaDetalleL : List<BandaHorariaDetalle>
    {
    }

    #endregion
}
