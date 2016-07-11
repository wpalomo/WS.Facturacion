using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region ValePrepagoHabilitacion: Clase para entidad de Habilitacion del Vale Prepago para ser vendido

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad de Habilitacion de Vale Prepago
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "ValePrepagoHabilitacion", IsNullable = false)]

    public class ValePrepagoHabilitacion
    {
        // Constructor vacio
        public ValePrepagoHabilitacion()
        {
        }


        public ValePrepagoHabilitacion(int lote,                Estacion estacionHabilitada,           
                                       decimal montoTarifa,     string habilitada)
        {
            this.Lote = lote;
            this.EstacionHabilitada = estacionHabilitada;
            this.MontoTarifa = montoTarifa;
            this.Habilitada = habilitada;
        }


        // Numero de lote
        public int Lote { get; set; }

        // Estacion en la que esta habilitada
        public Estacion EstacionHabilitada { get; set; }

        // Monto de la tarifa en esa estacion
        public decimal MontoTarifa { get; set; }

        // Indica si esta habilitada o no 
        public string Habilitada { get; set; }

        // Determina si la entrega esta habilitada o no
        public bool esHabilitada
        {
            get
            {
                return (Habilitada == "S");
            }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ValePrepagoHabilitacion
    /// </summary>*********************************************************************************************
    public class ValePrepagoHabilitacionL : List<ValePrepagoHabilitacion>
    {
    }

    #endregion
}
