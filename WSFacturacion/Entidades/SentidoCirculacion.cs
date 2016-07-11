using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region SENTIDOCIRCULACION: Clase para entidad de los sentidos de circulacion Ascendente Descendente

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad SentidoCirculacion
    /// </summary>*********************************************************************************************
    [Serializable]
    public class SentidoCirculacion
    {
        /// <summary>
        /// Constructor por defecto            
        /// </summary>
        public SentidoCirculacion()
        {
        }

        /// <summary>
        /// Constructor con dos parámetros
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="descripcion"></param>
        public SentidoCirculacion(string codigo, string descripcion)
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
    /// Lista de objetos SentidoCirculacion
    /// </summary>*********************************************************************************************
    [Serializable]
    public class SentidoCirculacionL : List<SentidoCirculacion>
    {
    }

    #endregion
}
