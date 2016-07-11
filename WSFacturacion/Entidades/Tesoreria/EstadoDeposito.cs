using System;
using System.Collections.Generic;

namespace Telectronica.Tesoreria
{
    /// <summary>
    /// Clase entidad que representa el estado de una reposición
    /// </summary>
    [Serializable]
    public class EstadoDeposito
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public EstadoDeposito() { }

        /// <summary>
        /// Constructor con el código como parámetro, carga la descripción automáticamente
        /// </summary>
        /// <param name="sCodigo"></param>
        public EstadoDeposito(string sCodigo)
        {
            Codigo = sCodigo;

            switch (sCodigo)
            {
                case "S":
                    Descripcion = "Sem Depositar";
                    break;
                case "D":
                    Descripcion = "Depositado";
                    break;
				//case "R":
				//    Descripcion = "Reposição";
				//    break;
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
    public class EstadoDepositoL : List<EstadoDeposito>
    {
    }
}
