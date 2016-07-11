using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TIPOPAGO: Clase para entidad de Tipos de Pago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoPago los distintos tipos de tarjeta o tag
    /// </summary>*********************************************************************************************

    [Serializable]
    public class TipoPago
    {
        // Constructor vacio, por defecto
        public TipoPago()
        { 
        }


        // En el constructor asignamos los valores a la clase
        public TipoPago(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;

        }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }

    }


    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoPago
    /// </summary>*********************************************************************************************
    [Serializable]
    public class TipoPagoL : List<TipoPago>
    {
    }

    #endregion
}
