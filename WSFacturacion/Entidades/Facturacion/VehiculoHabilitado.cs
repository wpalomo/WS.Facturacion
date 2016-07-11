using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region VEHICULOHABILITADO: Clase para entidad de los Vehiculos habilitados de una cuenta

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VehiculoHabilitado
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "VehiculoHabilitado", IsNullable = false)]
    public class VehiculoHabilitado 

    {


        // Constructor vacio
        public VehiculoHabilitado()
        {
        }

        public VehiculoHabilitado(Vehiculo vehiculo, bool habil)
        {
            this.Vehiculo = vehiculo;
            this.Habilitado = habil;
        }

        // Estacion
        public Vehiculo Vehiculo { get; set; }

        // Habilitado
        public bool Habilitado{ get; set; }

    }

    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VehiculoHabilitado
    /// </summary>*********************************************************************************************
    public class VehiculoHabilitadoL : List<VehiculoHabilitado>
    {
    }

    #endregion
}
