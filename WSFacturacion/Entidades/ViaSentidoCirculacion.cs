using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIASENTIDOCIRCULACION: Clase para entidad de los posibles sentidos de circulacion de una vía

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViaSentidoCirculacion
    /// </summary>*********************************************************************************************
    [Serializable]
    public class ViaSentidoCirculacion
    {
        /// <summary>
        /// Constructor por defecto            
        /// </summary>
        public ViaSentidoCirculacion()
        {
        }

        /// <summary>
        /// Constructor con dos parámetros
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="descripcion"></param>
        public ViaSentidoCirculacion(string codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }
                
        /// <summary>
        /// Codigo del sentido de circulacion
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Descripcion del Sentido de Circulacion
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Por defecto la estructura retorna la descripcion (para las grillas)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Descripcion;
        }
    }
    
    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaSentidoCirculacion
    /// </summary>*********************************************************************************************
    [Serializable]
    public class ViaSentidoCirculacionL: List<ViaSentidoCirculacion>
    {
    }
    
    #endregion
}
