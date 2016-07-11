using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace Telectronica.Facturacion
{
    #region LECTORCHIP: Clase para entidad de lectores de tarjetas chip

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad LectorChip
    /// </summary>*********************************************************************************************

    [Serializable]
    public class LectorChip
    {
    
        // Constructor Vacio
        public LectorChip()
        {
        }
    
        // Constructor Vacio
        public LectorChip(Estacion estacion, Int16 numero, string descripcion, Int16 puertoCOM, string UrlServicio)
        {
            this.Estacion = estacion;
            this.Numero = numero;
            this.Descripcion = descripcion;
            this.PuertoCOM = puertoCOM;
            this.UrlServicio = UrlServicio;
        }
    

        // Estacion en la que se define el lector chip
        public Estacion Estacion { get; set; }


        public string NombreEstacion
        {
            get
            {
                return Estacion.Nombre;
            }
        }

        // Numero de lector
        public Int16 Numero { get; set; }


        // Ubicación física del lector
        public string Descripcion { get; set; }


        // Puerto COM al que está conectado
        public Int16 PuertoCOM { get; set; }


        // Url del Web Service que gestiona al lector
        public string UrlServicio { get; set; }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos LectorChip 
    /// </summary>*********************************************************************************************
    public class LectorChipL : List<LectorChip>
    {
    }

    #endregion
}
