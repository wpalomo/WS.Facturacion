using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region Prepago: Clase para entidad de Saldo Prepago del cliente

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Prepago (saldo del cliente)
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "Prepago", IsNullable = false)]

    public class Prepago
    {
        // Constructor vacio
        public Prepago()
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

        public Prepago( Cliente cliente,         Zona zona,          decimal saldo,
                        DateTime fechaUltMov,    decimal giroRojo)
        {
            this.Cliente = cliente;
            this.Zona = zona;
            this.Saldo = saldo;
            this.FechaUltimoMovimiento = fechaUltMov;
            this.GiroRojo = giroRojo;
        }


        // Cliente al que le corresponde el saldo
        public Cliente Cliente { get; set; }

        // Zona del saldo
        public Zona Zona { get; set; }

        // Saldo
        public decimal Saldo { get; set; }

        // Formatea el saldo con el simbolo de moneda ("$")
        public string SaldoString
        {
            get
            {
                return Saldo.ToString("C");
            }
        }

        // Fecha del ultimo movimiento
        public DateTime FechaUltimoMovimiento { get; set; }

        // Monto maximo para girar en rojo
        public decimal GiroRojo { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Prepago
    /// </summary>*********************************************************************************************
    public class PrepagoL : List<Prepago>
    {
    }

    #endregion
}
