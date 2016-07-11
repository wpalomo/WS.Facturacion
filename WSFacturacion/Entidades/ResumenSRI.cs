using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]

    /// <summary>
    /// Estructura de la entidad para estados de envio de archivos a la sunat
    /// </summary>
    public class ResumenSRI
    {

        /// <summary>
        /// Título del estado del envío a Sunat
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Valor del estado del envío a Sunat
        /// </summary>
        public string Valor { get; set; }

        /// <summary>
        /// Existe falla en el estado del envío a Sunat
        /// </summary>
        public string Falla { get; set; } 

    }

    [Serializable]

    /// <summary>
    /// Lista de Envios de archivos a la Sunat
    /// </summary>
    public class ResumenSRIL : List<ResumenSRI>
    {
    }
}
