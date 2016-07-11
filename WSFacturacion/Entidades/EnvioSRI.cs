using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]

    /// <summary>
    /// Estructura de la entidad para estados de envio de archivos a la agencia de recaudacion
    /// </summary>
    public class EnvioSRI
    {


        // Codigo de Error Envio SRI
        public int CodigoEnvioSRI { get; set; }

        // Descripcion del Codigo de Error Envio SRI
        public string DescErrorEnvioSRI { get; set; }

        // Contructor Vacio
        public EnvioSRI() { }

        // Estacion de origen del documento
        public Estacion estacionComprobante { get; set; }

        public Estacion estacionGeneracion { get; set; }

        // Numero de Estacion (expuesto para usar en las grillas)
        public int EstacionNumero { get { return estacionComprobante.Numero; } }

        // Nombre del archivo generado
        public string Archivo { get; set; }

        // Fecha de Generacion
        public DateTime FechaGeneracion { get; set; }

        // RUC del concesionario
        public string RucConcesionario { get; set; }

        // Tipo de comprobante 
        public string TipoComprobante { get; set; }

        // Establecimiento
        public int Establecimiento { get; set; }

        // Punto de Venta
        public string PuntoVenta { get; set; }

        // Numero de Comprobante
        public int NumeroComprobante { get; set; }

        // Numero de comprobante en formato string
        public string NumeroComprobanteLargo { get { return NumeroComprobante.ToString("00000000"); } }

        // Fecha del comprobante
        public DateTime FechaComprobante { get; set; }

        // Fecha del comprobante
        public string TipoComprobanteSRI { get; set; }

        public EstadosEnvioSRI EstadoEnvioSRI { get; set; }

        // Tipo de procesamiento (A: Automatico, M: Manual)
        public string TipoProcesamiento { get; set; }

        // Descripcion de la forma como se ejecuto el proceso al momento de enviar los archivos
        public string TipoProcesamientoDesc
        {
            get
            {
                if (TipoProcesamiento == "A")
                    return "Automático";
                if (TipoProcesamiento == "M")
                    return "Manual";
                return "";
            }
        }

        // Codigo de Usuario 
        public string Usuario { get; set; }

        // Password de acceso del WS
        public string Password { get; set; }

        // Entity del WS
        public string Entity { get; set; }

        // Objeto Cliente al cual se genero la factura
        public Facturacion.Cliente Client { get; set; }

        // Fecha Fiscal del Comprobante
        public DateTime FechaFiscal { get; set; }

        // Numero completo del comprobante, formateado
        public string NumeroFacturaCompletoFormateado
        {
            get
            {
                // Numero de factura completo:  "EEE-PPP-NNNNNNNNN"
                return Establecimiento.ToString().PadLeft(3, '0').Trim() + "-" + PuntoVenta.Trim().PadLeft(3, '0').Trim() + "-" + NumeroComprobante.ToString().PadLeft(9, '0').Trim();
            }
        }


    }

    [Serializable]

    // Lista de Envios de archivos a la agencia recaudadora
    public class EnvioSRIL : List<EnvioSRI>
    {
    }
}
