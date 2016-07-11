using System;
using System.Collections.Generic;

namespace Telectronica.Tesoreria
{
    /// <summary>
    /// Clase entidad que representa el estado de un Retiro
    /// </summary>
    [Serializable]
    public class EstadoRetiros
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public EstadoRetiros() { }

        /// <summary>
        /// Constructor con el código como parámetro, carga la descripción automáticamente
        /// </summary>
        /// <param name="sCodigo"></param>
        public EstadoRetiros(string sCodigo)
        {
            Codigo = sCodigo;

            switch (sCodigo)
            {
                case "N":
                    Descripcion = "Sin Confirmar";
                    break;
                case "S":
                    Descripcion = "Confirmados";
                    break;
  
            }
        }

        #endregion

        /// <summary>
        /// Obtiene o establece el código único e identificatorio
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece una descripción
        /// </summary>
        public string Descripcion { get; set; }
    }

    [Serializable]
    public class EstadoRetirosL : List<EstadoRetiros>
    {
    }
}
