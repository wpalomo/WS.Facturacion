using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    #region TIPOIVA: Clase para entidad de Tipos de IVA

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoIVA
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class TipoIVA
    {
        public TipoIVA(int codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            
            //Puede a cuenta
            if (this.Codigo != 1)
                this.PuedeACuenta = true;

            //Puede Retenciones
            if (this.Codigo == 3)
                this.PuedeRetenciones = true;
        }


        // Codigo de Tipo de IVA
        public int Codigo { get; set; }

        // Descripcion del Tipo de IVA
        public string Descripcion { get; set; }

        //Tipo de Factura Asociada
        public TipoFactura TipoFactura { get; set; }

        //Puede a cuenta?
        public bool PuedeACuenta { get; set; }

        //Puede retenciones?
        public bool PuedeRetenciones { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoDocumento
    /// </summary>*********************************************************************************************
    public class TipoIVAL : List<TipoIVA>
    {
    }

    #endregion
}
