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
    public class VersionVia
    {
        /// <summary>
        /// Estacion de la version del programa
        /// </summary>
        public int Estacion { get; set; }

        /// <summary>
        /// Nombre de la estacion de la version del programa
        /// </summary>
        public string NombreEstacion { get; set; }

        /// <summary>
        /// Via de la version del programa
        /// </summary>
        public int Via { get; set; }

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

        /// <summary>
        /// Version del programa
        /// </summary>
        public string NumeroVersion { get; set; }

        /// <summary>
        /// Fecha de Cambio
        /// </summary>
        public DateTime FechaCambio { get; set; }

        /// <summary>
        /// Fecha de Modificacion
        /// </summary>
        public DateTime FechaModificacion { get; set; }

        public string FechaCambioString
        {
            get
            {
                return FechaCambio.ToString("yyyy/MM/dd HH:mm");
            }
        }

        public string FechaModificacionString
        {
            get
            {
                return FechaModificacion.ToString("yyyy/MM/dd HH:mm");
            }
        }

    }

    [Serializable]

    /// <summary>
    /// Lista de Versiones de vias
    /// </summary>
    public class VersionViaL : List<VersionVia>
    {
    }
}