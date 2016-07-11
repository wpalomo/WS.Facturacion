using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region RecargaSupervision: Clase para entidad de recarga de supervision

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Recarga de Supervision
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "RecargaSupervision", IsNullable = false)]

    public class RecargaSupervision
    {
        // Constructor vacio
        public RecargaSupervision()
        {
        }


        public RecargaSupervision(int identity,         Estacion estacion,          DateTime fechaRecarga,
                                  Cuenta cuenta,        FacturaItem itemFactura,    Parte parte, 
                                  decimal monto,        string anulada,             string descripcion)
        {
            this.Identity = identity;
            this.Estacion = estacion;
            this.FechaOperacion = fechaRecarga;
            this.Cuenta = cuenta;
            this.ItemFactura = itemFactura;
            this.Parte = parte;
            this.Monto = monto;
            this.Anulada = anulada;
            this.DescripcionVenta = descripcion;
        }

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

        public string DescripcionVenta { get; set; }

        public static implicit operator Operacion(RecargaSupervision f)
        {
            Operacion operacion = new Operacion();
            operacion.TipoOperacion = Operacion.enmTipo.enmRecargaSupervision;
            operacion.DescripcionVenta = f.DescripcionVenta;
            operacion.Anulada = f.Anulada;
            operacion.Cliente = f.Cliente;
            operacion.Cuenta = f.Cuenta;
            operacion.Estacion = f.Estacion;
            operacion.FechaOperacion = f.FechaOperacion;
            operacion.Identity = f.Identity;
            operacion.ItemFactura = f.ItemFactura;
            operacion.Monto = f.Monto;
            operacion.Parte = f.Parte;
            operacion.Patente = f.Patente;
            operacion.RecargaSupervision = f;
            return operacion;
        }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos RecargaSupervision
    /// </summary>*********************************************************************************************
    public class RecargaSupervisionL : List<RecargaSupervision>
    {
        // Para convertir esta clase a una lista de OperacionL
        public static implicit operator OperacionL(RecargaSupervisionL f)
        {
            OperacionL operaciones = new OperacionL();
            foreach (RecargaSupervision item in f)
            {
                operaciones.Add(item);
            }
            return operaciones;
        }
    }

    #endregion
}
