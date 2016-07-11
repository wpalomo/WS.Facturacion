using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TarifaAutorizacionPasoTipoVencimiento: Clase para entidad de tipo de vencimiento de las Tarifas de Autorizacion de Paso

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TarifaAutorizacionPasoFormaDescuento
    /// </summary>*********************************************************************************************

    [Serializable]

    public class TarifaAutorizacionPasoTipoVencimiento
    {
        // Constructor vacio
        public TarifaAutorizacionPasoTipoVencimiento()
        {
        }


        // En el constructor asigno los valores a la clase
        public TarifaAutorizacionPasoTipoVencimiento(string codigo, string descripcion)
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
    /// Lista de objetos TarifaAutorizacionPasoTipoVencimiento
    /// </summary>*********************************************************************************************
    public class TarifaAutorizacionPasoTipoVencimientoL : List<TarifaAutorizacionPasoTipoVencimiento>
    {
    }


    #endregion
}
