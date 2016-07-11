using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;

namespace Telectronica.Facturacion
{

    #region CLIENTE: Clase para entidad de los Clientes de Medios de Pago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Cliente
    /// </summary>*********************************************************************************************

    [Serializable]

    public class ReemplazoTickets
    {

        // Constructor Vacio
        public ReemplazoTickets()
        {
        }

        // Constructor usado para cuando tiene pocos datos, principalmente usado en las operaciones de facturacion
        public ReemplazoTickets(int identity, Estacion estacion, DateTime fechaReemplazo,
                                Cuenta cuenta, FacturaItem itemFactura,Parte parte, 
                                  decimal monto, string anulada, string descripcion, Cliente cliente)
        {
            this.NumReemplazo = identity;
            this.Estacion = estacion;
            this.FechaOperacion = fechaReemplazo;
            this.Cuenta = cuenta;
            this.ItemFactura = itemFactura;
            this.Parte = parte;
            this.Monto = monto;
            this.Anulada = anulada;
            this.DescripcionVenta = descripcion;
            this.Cliente = cliente;

        }

        // Identity
        public int NumReemplazo { get; set; }

        // Ident de ItmFac
        public int IdentItem { get; set; }

        // Tickets que pertenecen al reemplazo de tickets
        public TicketL Tickets { get; set; } 

        // Estacion en la que se realiza la entrega
        public Estacion Estacion { get; set; }

        // Fecha de la Operacion
        public DateTime FechaOperacion { get; set; }

        // Item correspondiente a la factura 
        public FacturaItem ItemFactura { get; set; }

        // Parte
        public Parte Parte { get; set; }

        // Monto Total a cobrar por la operacion
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

        // Numero de factura
        public int NumeroFactura { get; set; }

        // Determina si la entrega esta anulada o no
        public bool esAnulada
        {
            get
            {
                return (Anulada == "S");
            }
        }

        // Descripcion
        public string DescripcionVenta { get; set; }

        public static implicit operator Operacion(ReemplazoTickets f)
        {
            Operacion operacion = new Operacion();
            operacion.TipoOperacion = Operacion.enmTipo.enmReemplazoTicket;
            operacion.DescripcionVenta = f.DescripcionVenta;
            operacion.Anulada = f.Anulada;
            operacion.Cliente = f.Cliente;
            operacion.Cuenta = f.Cuenta;
            operacion.Estacion = f.Estacion;
            operacion.FechaOperacion = f.FechaOperacion;
            operacion.Identity = f.NumReemplazo;
            operacion.ItemFactura = f.ItemFactura;
            operacion.Monto = f.Monto;
            operacion.Parte = f.Parte;
            operacion.Patente = f.Patente;
            operacion.NumeroReemplazo = f.NumReemplazo;
            return operacion;
        }

    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos ReemplazoTickets
    /// </summary>*********************************************************************************************
    public class ReemplazoTicketsL : List<ReemplazoTickets>
    {
        // Para convertir esta clase a una lista de OperacionL
        public static implicit operator OperacionL(ReemplazoTicketsL f)
        {
            OperacionL operaciones = new OperacionL();
            foreach (ReemplazoTickets item in f)
            {
                operaciones.Add(item);
            }
            return operaciones;
        }
    }

    #endregion

}
