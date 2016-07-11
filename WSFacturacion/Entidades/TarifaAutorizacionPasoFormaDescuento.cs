using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TARIFAAUTORIZACIONPASOFORMADESCUENTO: Clase para entidad de forma de descuento de las Tarifas de Autorizacion de Paso

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TarifaAutorizacionPasoFormaDescuento
    /// </summary>*********************************************************************************************

    [Serializable]

    public class TarifaAutorizacionPasoFormaDescuento
    {
        // Constructor vacio
        public TarifaAutorizacionPasoFormaDescuento()
        {
        }


        // En el constructor asigno los valores a la clase
        public TarifaAutorizacionPasoFormaDescuento(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }



        // Codigo de efecto
        public string Codigo { get; set; }

        // Descripcion
        public string Descripcion { get; set; }

        // Para las grillas
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TarifaAutorizacionPasoFormaDescuento
    /// </summary>*********************************************************************************************
    public class TarifaAutorizacionPasoFormaDescuentoL : List<TarifaAutorizacionPasoFormaDescuento>
    {
    }

    #endregion
}
