using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region FACTURAITEM: Clase para entidad de un Item de una Factura

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad FACTURAITEM
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "FacturaItem", IsNullable = false)]

    public class FacturaItem
    {
        // Contructor vacio
        public FacturaItem()
        {
        }

        // Identity
        public int Identity { get; set; }

        // Estacion en la que se realiza la entrega
        public Estacion Estacion { get; set; }

        
        // Monto del Item
        public decimal Monto { get; set; }

        //Cantidad
        public int Cantidad { get; set; }

        // Monto Unitario
        public decimal MontoUnitario { get; set; }

        // Formatea el monto con el simbolo de moneda ("$")
        public string MontoString
        {
            get
            {
                return Monto.ToString("C");
            }
        }

        //Datos de la Factura

        // Cliente al que se le realiza la entrega del tag
        public Cliente Cliente { get; set; }

        // Tipo de Factura
        public string TipoFactura { get; set; }
        public string TipoFacturaDescr { get; set; }

        //Serie de la factura
        public string Serie { get; set; }

        // Numero de factura
        public int NumeroFactura { get; set; }

        // Punto de Venta
        public string PuntoVenta { get; set; }

        //Operacion Asociada
        private Operacion operacion;
        public Operacion Operacion 
        {
            get
            {
                return operacion;
            }
            set
            {
                operacion = value;
                this.TipoOperacion = Operacion.TipoOperacion;
                this.DescripcionVenta = Operacion.DescripcionVenta;
                this.Monto = Operacion.Monto;
                this.Cliente = Operacion.Cliente;
                this.Estacion = Operacion.Estacion;

                //TODO Siempre cantidad es 0?
                this.Cantidad = 1;
                if (this.Cantidad != 0)
                    this.MontoUnitario = this.Monto / this.Cantidad;
            }
        }

        //Para bindear al facturar
        public bool Facturar { get; set; }

        //Tipo de la Operacion
        public Operacion.enmTipo TipoOperacion { get; set; }

        //Descripcion Venta
        public string DescripcionVenta { get; set; }
    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos FacturaItem
    /// </summary>*********************************************************************************************
    public class FacturaItemL : List<FacturaItem>
    {
        public void AddOperaciones(OperacionL operaciones)
        {
            foreach (Operacion item in operaciones)
            {
                FacturaItem facItem = new FacturaItem();
                facItem.Operacion = item;

                this.Add(facItem);
            }
        }
    }

    #endregion

}
