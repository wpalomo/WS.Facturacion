using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region BLOQUE: Clase para entidad de los Bloques trabajados en las vias

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Bloque
    /// </summary>*********************************************************************************************
    [Serializable]
    [XmlRootAttribute(ElementName = "CabezeraTicket", IsNullable = false)]
    public class CabeceraTicket
    {
       

         public CabeceraTicket()
        {
            Estacion = new Estacion();
            Sentido = new ViaSentidoCirculacion();
        }
        
        /// <summary>
        /// Constructor al que se le pasan los objetos para armar completamente la subestacion
        /// </summary>
        public CabeceraTicket(Estacion estacion, ViaSentidoCirculacion viasentidoCirculacion, int codigoSubestacion, string descripcionSubetacion)
        {
            Estacion = estacion;
            Sentido = viasentidoCirculacion;
            subest = new Subestacion(Estacion, Sentido, codigoSubestacion, descripcionSubetacion);   
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

        public string SentidoDescr
        {
            get { return Sentido.Descripcion; }
        }

        public string SubestDescr
        {
            get { return subest.Descripcion; }
        }

        /// <summary>
        /// Indica el sentido de la estación
        /// </summary>
        public ViaSentidoCirculacion Sentido { get; set; }


        public Subestacion subest { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        public String Descripcion { get; set; }
    

       
    }

    /// <summary>
    /// Lista de objetos CabeceraTicket.
    /// </summary>
    [Serializable]
    public class CabeceraTicketL : List<CabeceraTicket>
    {
    }

}
    #endregion