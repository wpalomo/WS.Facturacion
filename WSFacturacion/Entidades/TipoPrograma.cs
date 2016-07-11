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
    public class TipoPrograma
    {
        /// <summary>
        /// Codigo del Tipo de Programa
        /// </summary>
        public string CodigoTipoPrograma { get; set; }

        /// <summary>
        /// Descripcion del Tipo de Programa
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Nombre del archivo del Tipo de Programa
        /// </summary>
        public string Archivo { get; set; }

        /// <summary>
        /// Path del Tipo de Programa
        /// </summary>
        public string Ubicacion { get; set; }

    }

    [Serializable]

    /// <summary>
    /// Lista de Tipos de Programas
    /// </summary>
    public class TipoProgramaL : List<TipoPrograma>
    {
    }
}
