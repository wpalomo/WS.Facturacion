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
    [XmlRootAttribute(ElementName = "Bloque", IsNullable = false)]
    public class Bloque
    {
        // Constructor Vacio
        public Bloque()
        {
        }

        //Via
        public Via Via { get; set; }

        // Numero de Bloque
        public int Numero { get; set; }

        //Apertura y Cierre
        public DateTime Apertura { get; set; }

        //Cierre
        public DateTime Cierre { get; set; }

        //Modo
        public ViaModo Modo { get; set; }

        // Sentido de Circulacion 
        public ViaSentidoCirculacion SentidoCirculacion { get; set; }

        // Si la via tiene habilitado el modo autotabulante
        public string Autotabulante { get; set; }

        //Peajista
        public Usuario Peajista { get; set; }

        //Identity del bloque
        public int Identity { get; set; }

        //Bloque conflictivo que no lo pueden ingresar en la liquidacion
        public bool HayBloqueConflictivo { get; set; }

        //peajista con el Bloque conflictivo que no lo pueden ingresar en la liquidacion
        public string PeajistaConBloqueConflictivo { get; set; }

        //Total de Transito
        public int? Transito { get; set; }

        //Visa
        public decimal VisaIntegrado { get; set; }
    }
    
    /// *********************************************************************************************<summary>
    /// Lista de objetos Bloque
    /// </summary>*********************************************************************************************
    [Serializable]
    public class BloqueL : List<Bloque>
    {
    }

    #endregion
}