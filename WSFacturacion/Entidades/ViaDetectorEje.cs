using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIADETECTORESEJES: Clase para entidad de las posibles configuraciones de detectores de ejes que tiene una via

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViaDetectoresEjes
    /// </summary>*********************************************************************************************

    [Serializable]

    public class ViaDetectorEje
    {

        // Constructor por defecto            
        public ViaDetectorEje()
        {
        }

        public ViaDetectorEje(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        
        // Codigo del detector de ejes
        public string Codigo { get; set; }

        // Descripcion del detector de ejes
        public string Descripcion { get; set; }

        // Por defecto la estructura retorna la descripcion (para las grillas)
        public override string ToString()
        {
            return Descripcion; 
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaDetectorEje
    /// </summary>*********************************************************************************************
    public class ViaDetectorEjeL : List<ViaDetectorEje>
    {
    }

    #endregion
}
