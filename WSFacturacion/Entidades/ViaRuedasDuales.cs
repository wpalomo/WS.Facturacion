using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIARUEDASDUALES: Clase para entidad de las posibles configuraciones de ruedas duales que tiene una via

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViaRuedasDuales
    /// </summary>*********************************************************************************************

    [Serializable]

    public class ViaRuedasDuales
    {
        // Constructor por defecto            
        public ViaRuedasDuales()
        {
        }

        public ViaRuedasDuales(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        
        // Codigo de ruedas duales
        public string Codigo { get; set; }

        // Descripcion del tipo de rueda dual
        public string Descripcion { get; set; }

        // Por defecto la estructura retorna la descripcion (para las grillas)
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaRuedasDuales
    /// </summary>*********************************************************************************************
    public class ViaRuedasDualesL : List<ViaRuedasDuales>
    {
    }

    #endregion
}
