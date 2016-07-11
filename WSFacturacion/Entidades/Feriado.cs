using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region FERIADO: Clase para entidad de Estaciones de Peaje.
    /// <summary>
    /// Estructura de una entidad Estacion de Peaje
    /// </summary>

    [Serializable]

    public class Feriado
    {
        // Fecha del Feriado
        public DateTime Fecha { get; set; }

        // Dia del Feriado
        public DiaSemana DiaSemana { get; set; }

        public string FechaString
        {
            get
            {
                return Fecha.ToString("dd/MM/yyyy");
            }
        }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Feriado.
    /// </summary>
    public class FeriadoL : List<Feriado>
    {       
    }

    #endregion

}
