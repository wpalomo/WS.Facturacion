using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Estructura de una entidad de la Subestación de peaje (sentido de la estación)
    /// </summary>
    [Serializable]
    public class Subestacion
    {
        /// <summary>
        /// Constructor Por Defecto, inicializa las propiedades Estacion y Sentido
        /// </summary>
        public Subestacion()
        {
            Estacion = new Estacion();
            Sentido = new ViaSentidoCirculacion();
        }
        
        /// <summary>
        /// Constructor al que se le pasan los objetos para armar completamente la subestacion
        /// </summary>
        public Subestacion(Estacion estacion, ViaSentidoCirculacion viasentidoCirculacion, int codigoSubestacion, string descripcionSubetacion)
        {
            Estacion = estacion;
            Sentido = viasentidoCirculacion;
            CodigoSubEstacion = codigoSubestacion;
            Descripcion = descripcionSubetacion;
        }
        
        /// <summary>
        /// Obtiene el número de la estación
        /// </summary>
        public int EstacionNumero
        {
            get { return Estacion.Numero; }
        }

        /// <summary>
        /// Numero de Estacion de Peaje
        /// </summary>
        public Estacion Estacion { get; set; }

        /// <summary>
        /// Obtiene el código del sentido de circulación
        /// </summary>
        public string SentidoCodigo
        {
            get { return Sentido.Codigo; }
        }

        /// <summary>
        /// Indica el sentido de la estación
        /// </summary>
        public ViaSentidoCirculacion Sentido { get; set; }

        /// <summary>
        /// Indica el código único e identificatorio para el sentido de la subestación
        /// </summary>
        public int CodigoSubEstacion { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece el número de Sia de la subestación
        /// </summary>
        public string NumeroSia { get; set; }

        /// <summary>
        /// Obtiene o establece una descripción que indica el sentido Cardinal de la subestación
        /// </summary>
        public string SentidoCardinal { get; set; }

        /// <summary>
        /// Obtiene o establece el numero de plaza grabado en la antena QFree
        /// </summary>
        public string PlazaAntena { get; set; }
    }

    /// <summary>
    /// Lista de objetos SubEstacion.
    /// </summary>
    [Serializable]
    public class SubestacionL : List<Subestacion>
    {
    }

}
