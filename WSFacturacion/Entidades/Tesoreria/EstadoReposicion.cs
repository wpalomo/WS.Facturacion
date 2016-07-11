using System;
using System.Collections.Generic;

namespace Telectronica.Tesoreria
{
    /// <summary>
    /// Clase entidad que representa el estado de una reposición
    /// </summary>
    [Serializable]
    public class EstadoReposicion
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public EstadoReposicion() { }

        /// <summary>
        /// Constructor con el código como parámetro, carga la descripción automáticamente
        /// </summary>
        /// <param name="sCodigo"></param>
        public EstadoReposicion(string sCodigo)
        {
            Codigo = sCodigo;

            switch (sCodigo)
            {
                
                
                case "S":
                    Descripcion = "Sem Reposición";
                    break;
                case "P":
                    Descripcion = "Pendente";
                    break;
                case "G":
                    Descripcion = "Pago";
                    break;
                case "X":
                    Descripcion = "Anulado";
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
    public class EstadoReposicionL : List<EstadoReposicion>
    {
    }
}
