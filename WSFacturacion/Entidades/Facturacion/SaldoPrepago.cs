using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region SaldoPrepago: Clase para entidad de Saldo Prepago del cliente

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Prepago (saldo del cliente)
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "SaldoPrepago", IsNullable = false)]

    public class SaldoPrepago
    {
        // Constructor vacio
        public SaldoPrepago()
        {
        }

        //ATENCION! el numero del enumerado debe coincidir con el codigo de TIMOVE
        public enum enmTipo
        {
            enmAjuste = 1,
            enmTransito,
            enmRecarga,
            enmRecargaAnulada,
            enmDebitoValidacion,
            enmCreditoValidacion,
            enmBonificacionAP,
            enmRecargaExterna,
            enmReversaExterna,
            enmBonificacionAPAnulada
        }


        // Zona del saldo
        public Zona Zona { get; set; }

        // Saldo
        public decimal? Saldo { get; set; }

        // Formatea el saldo con el simbolo de moneda ("$")
        public string SaldoString
        {
            get
            {
                string aux = "";
                if (Saldo != null)
                    aux = ((decimal)Saldo).ToString("C");

                return aux;
            }
        }

        // Fecha del ultimo movimiento
        public DateTime? FechaUltimoMovimiento { get; set; }

        // Monto maximo para girar en rojo
        public decimal? GiroRojo { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Prepago
    /// </summary>*********************************************************************************************
    public class SaldoPrepagoL : List<SaldoPrepago>
    {
    }

    #endregion
}
