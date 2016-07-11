using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region CUENTAESTACION: Clase para entidad de las Estaciones habilitadas de una cuenta

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CuentaEstacion
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "CuentaEstacion", IsNullable = false)]
    public class CuentaEstacion
    {
        // Constructor vacio
        public CuentaEstacion()
        {
        }

        public CuentaEstacion(Estacion estacion, bool habilitado)
        {
            this.Estacion = estacion;
            this.Habilitado = habilitado;
        }

        // Estacion
        public Estacion Estacion { get; set; }

        // Habilitado
        public bool Habilitado { get; set; }


    }

    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos CuentaEstacion
    /// </summary>*********************************************************************************************
    public class CuentaEstacionL : List<CuentaEstacion>
    {
        public CuentaEstacion FindEstacion(int estacion)
        {
            CuentaEstacion oEst = null;
            foreach (CuentaEstacion item in this)
            {
                if (item.Estacion.Numero == estacion)
                {
                    oEst = item;
                    break;
                }
            }
            return oEst;
        }
    }

    #endregion
}
