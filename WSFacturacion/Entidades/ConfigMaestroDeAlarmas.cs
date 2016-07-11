using System.Collections.Generic;
using System;

namespace Telectronica.Peaje
{
    #region Atributos
    /// <summary>
    /// Entidad que referencia la configuración de las alarmas
    /// </summary>
    [Serializable]
    #endregion
    public class ConfigMaestroDeAlarmas
    {
        /// <summary>
        /// Código único e identificatorio de la alarma
        /// </summary>
        public byte Codigo { get; set; }

        /// <summary>
        /// Descripción de la alarma
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Cantidad de veces para que un suceso provoque una alarma
        /// </summary>
        public byte Veces { get; set; }

        /// <summary>
        /// Tipo de sonido asociado a la alarma
        /// </summary>
        public TipoDeSonido Sonido { get; set; }
    }

    #region Atributos
    /// <summary>
    /// Clase lista de Alarma
    /// </summary>
    [Serializable]
    #endregion
    public class ConfigMaestroDeAlarmasL : List<ConfigMaestroDeAlarmas>
    {

    }
}