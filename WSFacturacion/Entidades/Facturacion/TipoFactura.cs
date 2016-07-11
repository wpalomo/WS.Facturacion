using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    #region TIPOFACTURA: Clase para entidad de Tipos de Factura

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoFactura
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class TipoFactura
    {
        public TipoFactura(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        // Codigo de Tipo de IVA
        public string Codigo { get; set; }

        // Descripcion del Tipo de IVA
        public string Descripcion { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoFactura
    /// </summary>*********************************************************************************************
    public class TipoFacturaL : List<TipoFactura>
    {
    }

    #endregion
}
