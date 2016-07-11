using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    #region Atributos
    /// <summary>
    /// Estructura de una entidad Exento Codigo
    /// </summary>
    [Serializable]
    #endregion
    public class ExentoCodigo
    {
        /// <summary>
        /// Constructor con 4 parámetros
        /// </summary>
        /// <param name="iCodigoExento"></param>
        /// <param name="sDescrExento"></param>
        /// <param name="esMuestraDiplay"></param>
        /// <param name="esRequiereAutorizacion"></param>
        public ExentoCodigo(Int16 iCodigoExento, string sDescrExento, string esMuestraDiplay, string esRequiereAutorizacion)
        {
            CodigoExento = iCodigoExento;
            DescrExento = sDescrExento;
            MuestraDiplay = esMuestraDiplay;
            RequiereAutorizacion = esRequiereAutorizacion;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ExentoCodigo()
        {
        }

        /// <summary>
        /// Codigo de Exento
        /// </summary>
        public Int16 CodigoExento { get; set; }

        /// <summary>
        /// Representa el motivo del exento.
        /// </summary>
        public ExentoMotivo ExentoMotivo { get; set; }

        /// <summary>
        /// Descripcion de la Exento
        /// </summary>
        public string DescrExento { get; set; }

        /// <summary>
        /// Muestra en Display (S/N)
        /// </summary>
        public string MuestraDiplay { get; set; }

        /// <summary>
        /// Obtiene el valor de la propiedad "MuestraDiplay" en valor booleano
        /// </summary>
        public bool esMuestraDiplay
        {
            get { return MuestraDiplay == "S"; }
            set { MuestraDiplay = value ? "S" : "N"; }
        }

        /// <summary>
        /// Requiere autorizacion del supervisor (S/N)
        /// </summary>
        public string RequiereAutorizacion { get; set; }

        /// <summary>
        /// Obtiene el valor de la propiedad "RequiereAutorizacion" en valor Booleano
        /// </summary>
        public bool esRequiereAutorizacion
        {
            get { return RequiereAutorizacion == "S"; }
            set { RequiereAutorizacion = value ? "S" : "N"; }
        }

        /// <summary>
        /// Lista de Estaciones habilitadas para el Tipo de Exento dado
        /// </summary>
        public ExentoEstacionL EstacionesHabilitadas { get; set; }

        /// <summary>
        /// Indica el típo de código de autorización
        /// </summary>
        public TipoAutorizacionExento TipCodigoAutorizacion { get; set; }

        /// <summary>
        /// Indica si es obligatorio el código de autorización (S/N)
        /// </summary>
        public string CodigoAutorizacionObligatoria { get; set; }

        /// <summary>
        /// Indica si es obligatorio el código de autorización (true/false)
        /// </summary>
        public bool esCodigoAutorizacionObligatoria
        {
            get { return CodigoAutorizacionObligatoria == "S"; }
            set { CodigoAutorizacionObligatoria = (value ? "S" : "N" ); }
        }
    }
    
    [Serializable]
    public class ExentoCodigoL : List<ExentoCodigo>
    {
    }
}
