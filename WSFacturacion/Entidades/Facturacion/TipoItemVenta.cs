using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{

    #region TIPOFACTURA: Clase para entidad de Tipo de Items de Venta

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoItemVenta
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class TipoItemVenta
    {
        public TipoItemVenta(int codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        // Codigo de Tipo de Item de Venta
        public int Codigo { get; set; }

        // Descripcion del Tipo de Item de Venta
        public string Descripcion { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoItemVenta
    /// </summary>*********************************************************************************************
    public class TipoItemVentaL : List<TipoItemVenta>
    {
    }

    #endregion
}
