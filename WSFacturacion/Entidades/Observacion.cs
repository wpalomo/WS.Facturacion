using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region OBSERVACION: Clase para entidad de Observaciones en las que se divide la concesión.
    /// <summary>
    /// Estructura de una entidad Observacion
    /// </summary>

    [Serializable]

    public class Observacion
        {

            public Observacion()
            {

            }

            // Codigo de Observacion
            public int Codigo { get; set; }

            // Descripcion de la Observacion
            public string Descripcion { get; set; }

       }


    [Serializable]

    /// <summary>
    /// Lista de objetos Observacion.
    /// </summary>
    public class ObservacionL : List<Observacion>
    {
    }

    #endregion

    }

