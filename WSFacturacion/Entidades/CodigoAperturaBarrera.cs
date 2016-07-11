using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase entidad que representa un Código de apertura de barrera
    /// </summary>
    public class CodigoAperturaBarrera
    {
        /// <summary>
        /// Código único e identificatorio para los códigos de apertura de barrera
        /// </summary>
        public byte Codigo { get; set; }

        /// <summary>
        /// Breve descripción, no mas de 30 caracteres
        /// </summary>
        public string Descripcion { get; set; }
    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos Código de apertura de barrera
    /// </summary>*********************************************************************************************
    [Serializable]
    public class CodigoAperturaBarreraL : List<CodigoAperturaBarrera>
    {
    }
}
