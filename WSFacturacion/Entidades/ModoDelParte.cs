using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase que representa un modo posible de un parte
    /// </summary>
    [Serializable]    
    public class ModoDelParte
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ModoDelParte() {}

        /// <summary>
        /// Constructor por parámetros
        /// </summary>
        /// <param name="sCodigo"></param>
        /// <param name="sDesc"></param>
        public ModoDelParte(string sCodigo, string sDesc)
        {
            Codigo = sCodigo;
            Descripcion = sDesc;
        }

        /// <summary>
        /// Obtiene o establece el codigo del modo del parte
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del modo del parte
        /// </summary>
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// Lista de la clase ModoDelParteL
    /// </summary>
    [Serializable]
    public class ModoDelParteL : List<ModoDelParte>
    {
    }
}
