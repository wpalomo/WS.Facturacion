using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region Operacion: Clase Base para Operaciones de Facturacion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad EntregaTag
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "Operacion", IsNullable = false)]

    public class Operacion
    {
        // Constructor vacio
        public Operacion()
        {
        }

        //ATENCION! el numero del enumerado debe coincidir con el codigo de TIPITM
        public enum enmTipo
        {
            enmEntregaTag = 1,
            enmRecargaSupervision,
            enmEntregaChip,
            enmReemplazoTicket,
            enmCuotaAbono,
            enmVinculacionChip,
            enmValePrepagoVenta,
            enmFallos = 8,
            enmViolaciones = 9
        }

        public Operacion(enmTipo tipo, int identity, Estacion estacion, DateTime fechaOperacion,
                          FacturaItem itemFactura,
                          Parte parte, decimal monto, string anulada,
                          Cliente cliente)
        {
            this.TipoOperacion = tipo;
            this.Identity = identity;
            this.Estacion = estacion;
            this.FechaOperacion = fechaOperacion;
            this.ItemFactura = itemFactura;
            this.Parte = parte;

            this.Monto = monto;
            this.Anulada = anulada;
            this.Cliente = cliente;
        }

        //Tipo de la Operacion
        public enmTipo TipoOperacion { get; set; }

        //Descripcion de la Venta
        public string DescripcionVenta { get; set; }

        // Identity
        public int Identity { get; set; }

        // Estacion en la que se realiza la entrega
        public Estacion Estacion { get; set; }

        // Fecha de la Operacion
        public DateTime FechaOperacion { get; set; }

        // Item correspondiente a la factura 
        public FacturaItem ItemFactura { get; set; }

        // Parte
        public Parte Parte { get; set; }

        // Monto a cobrar por la operacion
        public decimal Monto { get; set; }

        // Formatea el monto con el simbolo de moneda ("$")
        public string MontoString
        {
            get
            {
                return Monto.ToString("C");
            }
        }

        // Cliente al que se le realiza la entrega del tag
        public Cliente Cliente { get; set; }

        // Cuenta a la cual se le hace la entrega del tag
        public Cuenta Cuenta { get; set; }

        // Patente a la cual se le realiza la entrega
        public string Patente { get; set; }

        // Marca de entrega anulada
        public string Anulada { get; set; }

        // Determina si la entrega esta anulada o no
        public bool esAnulada
        {
            get
            {
                return (Anulada == "S");
            }
        }

        //Datos completos de la operacion
        public EntregaTag EntregaTag { get; set; }
        public EntregaChip EntregaChip { get; set; }
        public RecargaSupervision RecargaSupervision { get; set; }
        public ValePrepagoVenta ValePrepagoVenta { get; set; }
        public Fallos Fallo { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Operacion
    /// </summary>*********************************************************************************************
    public class OperacionL : List<Operacion>
    {
    }

    #endregion
}
