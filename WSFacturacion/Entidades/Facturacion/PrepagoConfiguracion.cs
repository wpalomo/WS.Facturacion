using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{

    #region PrepagoConfiguracion: Clase de una entidad de configuracion de prepagos

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad de configuracion de prepagos
    /// </summary>*********************************************************************************************
    [Serializable]
    [XmlRootAttribute(ElementName = "PrepagoConfiguracion", IsNullable = false)]
    public class PrepagoConfiguracion
    {
        //Constructor vacio
        public PrepagoConfiguracion() { }

        // Identity
        //public string ID { get; set; }

        // Estacion
        public bool Habilitado { get; set; }

    }


    /// *********************************************************************************************<summary>
    /// Lista de objetos PrepagoConfiguracion
    /// </summary>*********************************************************************************************
    [Serializable]
    public class PrepagoConfiguracionL : List<PrepagoConfiguracion>
    {
    }

    #endregion

}
