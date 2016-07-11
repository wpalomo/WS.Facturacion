using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIAMODO: Clase para entidad de los Modos posibles de Vias

    #region Atributos de la Clase
    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViaModo
    /// </summary>*********************************************************************************************
    [Serializable]
    #endregion
    public class ViaModo
    {
        #region Constructores

        /// <summary>
        /// Constructor con dos argumentos
        /// </summary>
        /// <param name="xsModo"></param>
        /// <param name="xsDescripcion"></param>
        public ViaModo(string xsModo, string xsDescripcion)
        {
            this.Modo = xsModo;
            this.Descripcion = xsDescripcion;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ViaModo()
        { }

        #endregion

        #region Atributos

        /// <summary>
        /// Modo de Via
        /// </summary>
        public string Modo { get; set; }
        
        /// <summary>
        /// Descripcion
        /// </summary>
        public string Descripcion { get; set; }

        #endregion
    }

    #region Atributos de la Clase
    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaModo
    /// </summary>*********************************************************************************************
    [Serializable]
    #endregion
    public class ViaModoL : List<ViaModo>
    {
    }

    #endregion
}
