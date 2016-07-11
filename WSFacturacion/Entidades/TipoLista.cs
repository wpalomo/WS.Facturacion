using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region TipoLista:
    /// <summary>
    /// Estructura de una entidad TipoLista. Son los posibles tipos de listas a importar, utilizado en los Comandos
    /// </summary>
    
    [Serializable]
    [XmlRootAttribute(ElementName = "TipoLista", IsNullable = false)]
    
    public class TipoLista
    {
        public TipoLista()
        {
        }

        public TipoLista(string xsCodigo, string xsDescripcion)
        {
            this.Codigo = xsCodigo;
            this.Descripcion = xsDescripcion;
        }

        public string Codigo { get; set; }                  // CODIGO
        public string Descripcion { get; set; }             // DESCRIPCION
    }

    [Serializable]
    public class TipoListaL : List<TipoLista>
    {
    }
    #endregion
}
