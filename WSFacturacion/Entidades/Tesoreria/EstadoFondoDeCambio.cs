using System;
using System.Collections.Generic;

namespace Telectronica.Tesoreria
{
    /// <summary>
    /// Clase entidad que representa el estado de un Fondo de Cambio
    /// </summary>
    [Serializable]
    public class EstadoFondoDeCambio
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public EstadoFondoDeCambio() { }

        /// <summary>
        /// Constructor con el código como parámetro, carga la descripción automáticamente
        /// </summary>
        /// <param name="sCodigo"></param>
        public EstadoFondoDeCambio(string sCodigo)
        {
            Codigo = sCodigo;

            switch (sCodigo)
            {
                case "EN":
                    Descripcion = "Entregado";
                    break;
                case "DC":
                    Descripcion = "Devuelto Confirmado";
                    break;
                case "DS":
                    Descripcion = "Devuelto Sin Confirmar";
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
    public class EstadoFondoDeCambioL : List<EstadoFondoDeCambio>
    {
    }
}
