using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region CodigoComando:

    #region Atributos de la Clase
    /// <summary>
    /// Estructura de una entidad CodigoComando son los codigos de comandos para vías
    /// </summary>    
    [Serializable]
    [XmlRootAttribute(ElementName = "CodigoComando", IsNullable = false)]
    #endregion
    public class CodigoComando
    {
        #region Constructores

        public CodigoComando()
        {
        }

        public CodigoComando(string xsTipo, string xsTipoDesc, string xsCodigo, string xsDescripcion)
        {
            this.Tipo = xsTipo;
            this.TipoDesc = xsTipoDesc;
            this.Codigo = xsCodigo;
            this.Descripcion = xsDescripcion;
        }

        #endregion

        #region Atributos

        public string Tipo { get; set; }

        public string TipoDesc { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        #endregion
    }

    #region Atributos de la Clase
    /// <summary>
    /// Clase Lista de CodigoComando
    /// </summary>
    [Serializable]
    #endregion
    public class CodigoComandoL : List<CodigoComando>
    {
    }

    #endregion
}