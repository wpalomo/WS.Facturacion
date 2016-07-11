using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region GRUPOESTACIONES: Clase para entidad de Grupo de Estaciones.
    /// <summary>
    /// Estructura de una entidad Grupo de Estaciones
    /// </summary>

    [Serializable]

    public class GrupoEstaciones
    {
        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de un grupo en particular
        /// </summary>
        /// <typeparam name="int">int</typeparam>
        /// <param name="codigo">Codigo del Grupo de Estaciones</param>
        /// <typeparam name="string">string</typeparam>
        /// <param name="descripcion">Descripcion del Grupo de Estaciones</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public GrupoEstaciones(int? codigo,
                                string descripcion
                                )
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            
        }

        public GrupoEstaciones(int? codigo)
        {
            Codigo = codigo;
        }

        public GrupoEstaciones()
        {

        }

        // Codigo del Grupo de Estaciones
        public int? Codigo{ get; set; }

        // Descripcion del Grupo de Estaciones
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
        
        /// <summary>
        /// Descripcion que se muestra en el Cliente Grafico.
        /// </summary>
        public string DescripcionAlt { get; set; }


        // Estaciones del grupo
        public EstacionL Estaciones { get; set; }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Grupo de Estaciones.
    /// </summary>
    public class GrupoEstacionesL : List<GrupoEstaciones>
    {
    }

    #endregion
}
