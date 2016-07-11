using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    #region AgrupacionDb: Clase para entidad de Agrupacion de base de datos

    /// <summary>
    /// Estructura de una entidad Agrupacion de base de datos
    /// </summary>
    [Serializable]    
    public class AgrupacionDb
    {
        /// <summary>
        /// Codigo identity de la tabla AgruDb
        /// </summary>
        public int Codigo { get; set; }
        
        /// <summary>
        /// Nombre de la Base de Datos 
        /// </summary>
        public string BaseDatos { get; set; }

        /// <summary>
        /// Nombre del servidor de datos
        /// </summary>
        public string ServidorDatos { get; set; }

        /// <summary>
        /// Direccion URL del sitio del sistema en la plaza
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Descripcion de la agrupacion
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que concatena el Id y la Descripcion para el combo
        /// </summary>
        public string Id_Descripcion {
            get { return string.Format("{0} - {1}", Codigo, Descripcion); } 
        }
    }    

    /// <summary>
    /// Lista de objetos AgrupacionDb.
    /// </summary>
    [Serializable]
    public class AgrupacionDbL : List<AgrupacionDb>
    {       
    }

    #endregion
}