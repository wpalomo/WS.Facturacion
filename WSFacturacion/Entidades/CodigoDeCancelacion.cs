using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase entidad que representa un Código de cancelacion
    /// </summary>
    public class CodigoDeCancelacion
    {
        /// <summary>
        /// Código único e identificatorio para los códigos de cancelacion
        /// </summary>
        public byte Codigo { get; set; }

        /// <summary>
        /// Breve descripción, no mas de 30 caracteres
        /// </summary>
        public string Descripcion { get; set; }
    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos Código de Cancelacion
    /// </summary>*********************************************************************************************
    [Serializable]
    public class CodigoDeCancelacionL : List<CodigoDeCancelacion>
    {
    }
}
