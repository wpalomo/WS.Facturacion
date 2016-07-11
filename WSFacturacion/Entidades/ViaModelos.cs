using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region VIAMODELO: Clase para entidad de los Modelos posibles de Vias

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViaModelo
    /// </summary>*********************************************************************************************

    [Serializable]
    
    public class ViaModelo
    {
        public ViaModelo(string modelo, string descripcion)
        {
            this.Modelo = modelo;
            this.Descripcion = descripcion;
        }


        // Modelo de Via
        public string Modelo { get; set; }

        // Descripcion
        public string Descripcion { get; set; }

        // Por defecto la estructura retorna la descripcion (para las grillas)
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaModelo
    /// </summary>*********************************************************************************************
    public class ViaModeloL : List<ViaModelo>
    {
    }

    #endregion
}
