using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase entidad que representa un Código de Simulación
    /// </summary>
    public class CodigoDeSimulacion
    {
        /// <summary>
        /// Código único e identificatorio para los códigos de simulación
        /// </summary>
        public byte Codigo { get; set; }

        /// <summary>
        /// Breve descripción, no mas de 30 caracteres
        /// </summary>
        public string Descripcion { get; set; }
    }
    
    /// *********************************************************************************************<summary>
    /// Lista de objetos Código de Simulación
    /// </summary>*********************************************************************************************
    [Serializable]
    public class CodigoDeSimulacionL : List<CodigoDeSimulacion>
    {
    }
}
