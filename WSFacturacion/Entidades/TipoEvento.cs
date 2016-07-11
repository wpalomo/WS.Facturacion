using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region TipoEvento:
    /// <summary>
    /// Estructura de una entidad ClaveEvento son los codigos de eventos
    /// </summary>
    
    [Serializable]
    [XmlRootAttribute(ElementName = "TipoEvento", IsNullable = false)]
    
    public class TipoEvento
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public TipoEvento()
        {
        }

        /// <summary>
        /// Constructor que recibe el tipo de evento por parámetro
        /// </summary>
        /// <param name="tipo"></param>
        public TipoEvento(string tipo)
        {
            Tipo = tipo;
        }

        /// <summary>
        /// Obtiene o establece el tipo de evento
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Obtiene o establece una breve descripción del mismo
        /// </summary>
        public string Descripcion { get; set; }
    }
    
    [Serializable]
    public class TipoEventoL:List<TipoEvento>
    {
    }

    #endregion
}
