using System;
using System.Collections.Generic;

namespace Telectronica.Tesoreria
{
    /// <summary>
    /// Clase para la entidad que representa un Tipo de Reposición
    /// </summary>
    [Serializable]
    public class TipoDeReposicion
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public TipoDeReposicion() { }

        /// <summary>
        /// Constructor con un parámetro que indica el código, se completa automaticamente la descripción
        /// </summary>
        /// <param name="tipoCodigo"></param>
        public TipoDeReposicion(string tipoCodigo)
        {
            TipoCodigo = tipoCodigo;
            
            switch (tipoCodigo)
            {
                case "F":
                    Descripcion = "Financiera";
                    break;
                case "E":
                    Descripcion = "Económica";
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Indica el código del tipo de reposición
        /// </summary>
        public string TipoCodigo { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción
        /// </summary>
        public string Descripcion { get; set; }
    }

    [Serializable]
    public class TipoDeReposicionL : List<TipoDeReposicion>
    {
    }
}
