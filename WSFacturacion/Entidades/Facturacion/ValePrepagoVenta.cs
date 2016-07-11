using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region ValePrepagoVenta: Clase para entidad de Venta de Vale Prepago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad de Venta de Vale Prepago
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "ValePrepagoVenta", IsNullable = false)]

    public class ValePrepagoVenta
    {
        // Constructor vacio
        public ValePrepagoVenta()
        {
        }


        public ValePrepagoVenta(int identity,        Estacion estacion,          DateTime fechaVenta,
                           Cuenta cuenta,           int serieInicial,           int serieFinal,
                           CategoriaManual categoria, FacturaItem itemFactura,    Parte parte, 
                           decimal monto,           string anulada,             string descripcion,
                           TarifaDiferenciada tipoTarifa, Cliente cliente)
        {
            this.Identity = identity;
            this.Estacion = estacion;
            this.FechaOperacion = fechaVenta;
            this.Cuenta = cuenta;
            this.SerieInicial = serieInicial;
            this.SerieFinal = serieFinal;
            this.Categoria = categoria;
            this.ItemFactura = itemFactura;
            this.Parte = parte;
            this.Monto = monto;
            this.Anulada = anulada;
            this.TipoTarifa = tipoTarifa;
            this.DescripcionVenta =  descripcion + " " + "Cat:" + categoria.ToString() + " Serie:" + serieInicial.ToString() + "/" + serieFinal.ToString();
            this.Cliente = cliente;
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


        // Serie inicial del vale
        public int SerieInicial { get; set; }

        // Serie final del vale
        public int SerieFinal { get; set; }

        // Categoria
        public CategoriaManual Categoria { get; set; }

        //Tipo de Tarifa
        public TarifaDiferenciada TipoTarifa { get; set; }
 
        // Estaciones habilitadas (para impresion y venta)
        public string EstacionesHabilitadasString { get; set; }

        public string DescripcionVenta { get; set; }

        public static implicit operator Operacion(ValePrepagoVenta f)
        {
            Operacion operacion = new Operacion();
            operacion.TipoOperacion = Operacion.enmTipo.enmValePrepagoVenta;
            operacion.DescripcionVenta = "Venta de Vales Cat:" + f.Categoria.ToString() + " Serie:" + f.SerieInicial.ToString() + "/" + f.SerieFinal.ToString();
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
            operacion.ValePrepagoVenta = f;
            return operacion;
        }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ValePrepago
    /// </summary>*********************************************************************************************
    public class ValePrepagoVentaL : List<ValePrepagoVenta>
    {
        // Para convertir esta clase a una lista de OperacionL
        public static implicit operator OperacionL(ValePrepagoVentaL f)
        {
            OperacionL operaciones = new OperacionL();
            foreach (ValePrepagoVenta item in f)
            {
                operaciones.Add(item);
            }
            return operaciones;
        }
    }

    #endregion
}
