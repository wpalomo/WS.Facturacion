﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region EntregaChip: Clase para entidad de Entrega de Tarjetas Chips

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad EntregaChip
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "EntregaChip", IsNullable = false)]

    public class EntregaChip
    {
        // Constructor vacio
        public EntregaChip()
        {
        }

        public EntregaChip(int identity,            Estacion estacion,          DateTime fechaEntrega, 
                           Cuenta cuenta,           FacturaItem itemFactura,    string dispositivo, 
                           int numeroExterno,       Parte parte,                decimal monto,              
                           string anulada,          string abona,               string patente,             
                           string reposicion,       Cliente cliente,            string descripcion,
                           string Pospago,          string Habilitado,          string PendienteFacturacion,
                            int numeroInterno)
        {
            this.Identity = identity;
            this.Estacion = estacion;
            this.FechaOperacion = fechaEntrega;
            this.Cuenta = cuenta;
            this.ItemFactura = itemFactura;
            this.NumeroInterno = numeroInterno;
            this.NumeroExterno = numeroExterno;
            this.Parte = parte;

            this.Monto = monto;
            this.Anulada = anulada;
            this.Abona = abona;
            this.Patente = patente;
            this.Reposicion = reposicion;
            this.Cliente = cliente;
            this.DescripcionVenta = descripcion + " " + numeroExterno.ToString();
            this.Pospago = Pospago;

            this.Habilitado = Habilitado;
            this.PendienteFacturacion = (Habilitado == "N" ? "S" : "N");
            this.Dispositivo = dispositivo;
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

        // Numero interno
        public int NumeroInterno { get; set; }
        // dispositivo
        public string Dispositivo { get; set; }

        // Numero externo de tarjeta Chip
        public int NumeroExterno { get; set; }


        // Marca que indica si debe abonar la tarjeta Chip o no
        public string Abona { get; set; }

        // Determina si la entrega esta anulada o no
        public bool esDebeAbonar
        {
            get
            {
                return (Abona == "S");
            }
        }

        // Traduce a "Si/No" el valor del atributo si debe o no abonar el dispositivo
        public string esDebeAbonarString
        {
            get
            {
                return (Abona == "N" ? "No" : "Si");
            }
        }

        // Marca que indica si se cobra en cuenta corriente pospago
        public string Pospago { get; set; }

        // Determina si la entrega esta anulada o no
        public bool esPospago
        {
            get
            {
                return (Pospago == "S");
            }
        }

        // Traduce a "Si/No" el valor del atributo si debe o no abonar el dispositivo
        public string esPospagoString
        {
            get
            {
                return (Pospago == "N" ? "No" : "Si");
            }
        }

        // Marca que indica si es un medio de pago nuevo o reposicion
        public string Reposicion { get; set; }

        // Determina si es un medio de pago nuevo o reposicion
        public bool esReposicion
        {
            get
            {
                return (Reposicion == "N");
            }
        }
        // Traduce a "Si/No" el valor del atributo si es un medio de pago nuevo o reposicion
        public string esReposicionString
        {
            get
            {
                return (Reposicion == "N" ? "No" : "Si");
            }
        }

        public string DescripcionVenta { get; set; }



        // Marca que indica si es un medio de pago esta habilitado o no
        public string Habilitado { get; set; }

        // Marca que indica si es un medio de pago esta habilitado o no
        public bool esHabilitado
        {
            get
            {
                return (Habilitado == "S");
            }
        }
        // Traduce a "Si/No" el valor del atributo si es un medio de pago nuevo o reposicion
        public string esHabilitadoString
        {
            get
            {
                return (Habilitado == "N" ? "No" : "Si");
            }
        }

        // Marca que indica si es un medio de pago esta habilitado o no
        public string PendienteFacturacion { get; set; }

        // Marca que indica si es un medio de pago esta habilitado o no
        public bool esPendienteFacturacion
        {
            get
            {
                return (PendienteFacturacion == "S");
            }
        }
        // Traduce a "Si/No" el valor del atributo si es un medio de pago nuevo o reposicion
        public string esPendienteFacturacionString
        {
            get
            {
                return (PendienteFacturacion == "N" ? "No" : "Si");
            }
        }


        public static implicit operator Operacion(EntregaChip f)
        {
            Operacion operacion = new Operacion();
            operacion.TipoOperacion = Operacion.enmTipo.enmEntregaChip;
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
            operacion.EntregaChip = f;
            return operacion;
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos EntregaChip
    /// </summary>*********************************************************************************************
    public class EntregaChipL : List<EntregaChip>
    {
        // Para convertir esta clase a una lista de OperacionL
        public static implicit operator OperacionL(EntregaChipL f)
        {
            OperacionL operaciones = new OperacionL();
            foreach (EntregaChip item in f)
            {
                operaciones.Add(item);
            }
            return operaciones;
        }
    }

    #endregion
}
