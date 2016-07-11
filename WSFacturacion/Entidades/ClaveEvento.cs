using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region CLAVEEVENTO:

    /// <summary>
    /// Estructura de una entidad ClaveEvento son los codigos de eventos
    /// </summary>
    [Serializable]
    [XmlRootAttribute(ElementName = "ClaveEvento", IsNullable = false)]    
    public class ClaveEvento
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ClaveEvento()
        {
        }

        /// <summary>
        /// Constructor con tres parametros
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        public ClaveEvento(short codigo, string descripcion, string tipo)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Tipo = tipo;
            TipoEvento = new TipoEvento();
            TipoEvento.Tipo = tipo;
        }

        #endregion

        /// <summary>
        /// Obtiene o establece el código del evento
        /// </summary>
        public short Codigo { get; set; }
        /// <summary>
        /// Obtiene o establece la descripción del evento
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de evento
        /// </summary>
        public string Tipo { get; set; }
        /// <summary>
        /// Obtiene o establece un objeto que representa el tipo de evento
        /// </summary>
        public TipoEvento TipoEvento { get; set; }
        /// <summary>
        /// Devuelve el valor de la propiedad Codigo y Descripción concatenadas
        /// </summary>
        public string CodigoDescripcion
        {
            get
            {
                return Codigo.ToString() + " " + Descripcion;
            }
        }
    }
    
    [Serializable]
    public class ClaveEventoL:List<ClaveEvento>
    {
    }

    #endregion
}
