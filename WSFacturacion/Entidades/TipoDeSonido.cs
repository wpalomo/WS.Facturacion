using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase que representa una entidad de TipoDeSonido
    /// </summary>
    public class TipoDeSonido
    {
        /// <summary>
        /// Constructor Por Defecto
        /// </summary>
        public TipoDeSonido()
        {        
        }

        /// <summary>
        /// Constructor Con Un Parámetro
        /// </summary>
        /// <param name="iTipo"></param>
        public TipoDeSonido(byte? iTipo)
        {
            Tipo = iTipo;
            if (Tipo == null)
                Descripcion = "Sem Son";
            else
                Descripcion = "Critico";
        }
        public TipoDeSonido(byte? iTipo, string sDescripcion)
        {
            Tipo = iTipo;
            Descripcion = sDescripcion;
        }

        /// <summary>
        /// Tipo de sonido asociado a la alarma
        /// </summary>
        public byte? Tipo { get; set; }

        /// <summary>
        /// Obtiene el tipo de sonido como descripción
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Sobreescribe el método ToString para que devuelva la Descripción
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Descripcion;
        }
    }

    #region Atributos
    /// <summary>
    /// Clase lista de Tipo de Sonidos
    /// </summary>
    [Serializable]
    #endregion
    public class TipoDeSonidoL : List<TipoDeSonido>
    {
    }
}
