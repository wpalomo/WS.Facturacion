using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region ZONA: Clase para entidad de Zonas en las que se divide la concesión.
    /// <summary>
    /// Estructura de una entidad Zona
    /// </summary>

    [Serializable]

    public class Zona
    {
        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de una zona en particular
        /// </summary>
        /// <typeparam name="int">int</typeparam>
        /// <param name="codigo">Codigo de Zona</param>
        /// <typeparam name="int">string</typeparam>
        /// <param name="codigo">Descripcion de la Zona</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public Zona(int codigo,
                    string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        public Zona()
        {

        }


        // Codigo de Zona
        public int Codigo{ get; set; }

        // Descripcion de la Zona
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Zona.
    /// </summary>
    public class ZonaL : List<Zona>
    {
    }

    #endregion
}
