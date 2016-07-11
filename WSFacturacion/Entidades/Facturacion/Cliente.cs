using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{

    #region CLIENTE: Clase para entidad de los Clientes de Medios de Pago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Cliente
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Cliente
    {

        // Constructor Vacio
        public Cliente()
        {
        }

        // Constructor usado para cuando tiene pocos datos, principalmente usado en las operaciones de facturacion
        public Cliente(int numeroCliente, string razonSocial, string domicilio)
        {
            this.NumeroCliente = numeroCliente;
            this.RazonSocial = razonSocial;
            this.Domicilio = domicilio;
        }

        // Constructor con todos los datos
        public Cliente(int numeroCliente, string razonSocial, TipoDocumento tipoDocumento,
                       string numeroDocumento, string domicilio, Provincia provincia,
                       string localidad, string telefono, TipoIVA tipoIVA,
                       string comentario, string expediente, string email,
                       bool eliminado, string numeroTarjeta)
        {
            this.NumeroCliente = numeroCliente;
            this.RazonSocial = razonSocial;
            this.TipoDocumento = tipoDocumento;
            this.NumeroDocumento = NumeroDocumento;
            this.Domicilio = domicilio;
            this.Provincia = provincia;
            this.Localidad = localidad;
            this.Telefono = telefono;
            this.TipoIVA = tipoIVA;
            this.Comentario = comentario;
            this.Expediente = expediente;
            this.Email = email;
            this.Eliminado = eliminado;
            this.NumeroTarjeta = numeroTarjeta;
        }

        // Numero de cliente
        public int NumeroCliente { get; set; }

        // Para determinar si el cliente se definio en Gestion o Local en la Estacion (para reemplazo de tickets)
        public string ClienteLocal { get; set; }

        // Booleano que determina cuando el cliente es local
        public bool esClienteLocal
        {
            get
            {
                return (ClienteLocal == "S");
            }
        }

        // Razon Social 
        public String RazonSocial { get; set; }
        // Tipo de Documento
        public TipoDocumento TipoDocumento { get; set; }

        // Numero de documento
        public string NumeroDocumento { get; set; }

        // Domicilio
        public string Domicilio { get; set; }

        // Provincia
        public Provincia Provincia { get; set; }

        // Localidad
        public String Localidad { get; set; }

        // Telefono
        public String Telefono { get; set; }

        // Tipo de IVA
        public TipoIVA TipoIVA { get; set; }

        //Tipo de Factura (de acuerdo al tipo de iva)
        public TipoFactura TipoFactura { get; set; }

        // Comentario
        public String Comentario { get; set; }

        // Expediente con el que se registra el nuevo cliente
        public String Expediente { get; set; }

        // Email
        public String Email { get; set; }

        // Eliminado (baja logica)
        public bool Eliminado { get; set; }

        // Saldo del cliente para la zona en la que se ejecuta la consulta
        public SaldoPrepago SaldoPrepago { get; set; }

        // Cuenta Prepaga
        public Cuenta CuentaPrepaga { get; set; }

        // Lista de Saldo Prepagos de TODAS las estaciones/zonas
        public SaldoPrepagoL SaldosPrepagos { get; set; }

        // Lista de Vehiculos.
        public VehiculoL Vehiculos { get; set; }

        // Lista de cuentas
        public CuentaL Cuentas { get; set; }

        // Numero de tarjeta encriptada
        public string NumeroTarjeta { get; set; }

        // Tiene Cuenta Pospago?
        public bool TieneCuentaPospago { get; set; }

    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos Cliente
    /// </summary>*********************************************************************************************
    [Serializable]
    public class ClienteL : List<Cliente>
    {
        public ClienteSLL ConvertEnSL()
        {
            ClienteSLL lista = new ClienteSLL();
            foreach (Cliente item in this)
            {
                lista.Add(new ClienteSL(item));
            }
            return lista;
        }
    }

    [Serializable]

    public class ClienteSL
    {

        // Constructor Vacio
        public ClienteSL()
        {
        }

        // Constructor Vacio
        public ClienteSL(Cliente oCliente)
        {
            this.NumeroCliente = oCliente.NumeroCliente;
            this.RazonSocial = oCliente.RazonSocial;
        }

        // Constructor usado para cuando tiene pocos datos, principalmente usado en las operaciones de facturacion
        public ClienteSL(int numeroCliente, string razonSocial, string domicilio)
        {
            this.NumeroCliente = numeroCliente;
            this.RazonSocial = razonSocial;

        }
        // Numero de cliente
        public int NumeroCliente { get; set; }
        // Razon Social 
        public String RazonSocial { get; set; }


    }


    /// *********************************************************************************************<summary>
    /// Lista de objetos Cliente
    /// </summary>*********************************************************************************************
    [Serializable]
    public class ClienteSLL : List<ClienteSL>
    {

    }




    #endregion

}
