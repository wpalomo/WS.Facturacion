using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;


namespace Telectronica.Facturacion
{
    #region IMPUESTO: Clase para entidad de Impuesto

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Impuesto
    /// Tiene el porcentaje, si suma o resta, si es adicional, si aplica o no, y el importe
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Impuesto
    {
        // Constructor vacio
        public Impuesto()
        {
        }


        public double PorcentajeIva { get; set; }

        public double PorcentajeRetencion { get; set; }

        public double PorcentajeRetencionBienes { get; set; }
    

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Impuesto
    /// </summary>*********************************************************************************************
    public class ImpuestoL : List<Impuesto>
    {
    }

    #endregion
}
