using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region BANDAHORARIAINTERVALO: Clase para entidad de los intervalos posibles entre las Bandas Horarias

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad BandaHorariaIntervalo
    /// </summary>*********************************************************************************************

    [Serializable]

    public class BandaHorariaIntervalo
    {
        public BandaHorariaIntervalo(int intervalo, string descripcion)
        {
            this.Intervalo = intervalo;
            this.Descripcion = descripcion;
        }


        // Intervalo posible (valor)
        public int Intervalo { get; set; }

        // Descripcion (lo que se muestra en el combo)
        public string Descripcion { get; set; }

        // Valor por defecto de la clase (para mostrar en la grilla)
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos BandaHorariaIntervalo
    /// </summary>*********************************************************************************************
    public class BandaHorariaIntervaloL : List<BandaHorariaIntervalo>
    {
    }

    #endregion
}
