using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Tesoreria;

namespace Telectronica.Facturacion
{
    #region COBROACUENTA: Clase para entidad de Cobros a Cuenta de las Facturas 

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CobroACuenta
    /// </summary>*********************************************************************************************

    [Serializable]

    public class CobroACuenta
    {
        // Constructor vacio
        public CobroACuenta()
        { 
        }

        public CobroACuenta(Factura factura,                Parte parte,                decimal monto,
                            decimal montoRetencion,         decimal montoRetencionBienes,    DateTime fechaCobro)
        {
            this.Factura = factura;
            this.Parte = parte;
            this.Monto = monto;
            this.MontoRetencion = montoRetencion;
            this.MontoRetencionBienes = montoRetencionBienes;
            this.FechaCobro = fechaCobro;
        }


        // Factura a la que se le aplica el cobro a cuenta
        public Factura Factura { get; set; }

        // Parte en el que se cobra la factura pendiente (la factura tiene su propio parte en que se genero)
        public Parte Parte { get; set; }

        // Monto del cobro (monto total de la factura)
        public decimal Monto { get; set; }

        // Monto de Retencion en la fuente
        public decimal MontoRetencion { get; set; }

        // Monto de Retencion en la fuente (B)
        public decimal MontoRetencionBienes { get; set; }

        // Fecha de cobro
        public DateTime FechaCobro { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos CobroACuenta
    /// </summary>*********************************************************************************************
    public class CobroACuentaL : List<CobroACuenta>
    {
    }

    #endregion
}
