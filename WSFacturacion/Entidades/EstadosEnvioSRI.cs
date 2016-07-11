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
    public class EstadosEnvioSRI
    {

        // Descripcion del Codigo de Error Envio SRI
        public string DescErrorEnvioSRI { get; set; }

        public EstadosEnvioSRI() { }

        public EstadosEnvioSRI(int codigo)
        {
            switch (codigo)
            {
                case 101:
                    DetalleRespuesta = "Error en la Apertura del Web Services al Enviar Factura";
                    EstadoEnvio = "PE"; //PENDIENTE DE ENVIO
                    break;
                case 102:
                    DetalleRespuesta = "Error en la Apertura del Web Services al Obtener Factura";
                    EstadoEnvio = "PR"; //PENDIENTE DE ENVIO
                    break;
                case 201:
                    DetalleRespuesta = "Fallo de Comunicación al crear Lote";
                    EstadoEnvio = "PE"; //QUEDA PENDIENTE DE ENVIO
                    break;
                case 301:
                    DetalleRespuesta = "Fallo de Comunicacion al crear Factura";
                    EstadoEnvio = "PE"; //QUEDA EN ESTADO PENDIENTE
                    break;
                case 401:
                    DetalleRespuesta = "Fallo de Comunicación al Intentar Obtener Estado de Factura";
                    EstadoEnvio = "PR"; // QUEDA EN ESTADO PENDIENTE DE RECEPCION DE RESPUESTA
                    break;
                case 200:
                    DetalleRespuesta = "Comunicación Correcta";
                    EstadoEnvio = "EN"; // YA FUE ENVIADO Y TIENE UNA RESPUESTA
                    break;
            }
        
        
        }


        // Resultado del envío a la agencia recaudadora
        public string EstadoEnvio { get; set; }

        // Detalle del resultado
        public string DetalleRespuesta { get; set; }

        // Numero de Seguimiento de la Factura
        public Guid? SeguimientoEnvio { get; set; }

        // Status de envio
        public string StatusEnvio { get; set; }

        public string StatusEnvioDescripcion
        {
            get
            {
            string sDescr = "";

            switch (StatusEnvio)
            {
                case "A":
                    sDescr = "Aceptada";
                    break;

                case "P":
                    sDescr = "Pendiente";
                    break;

                case "N":
                    sDescr = "No Aceptada";
                    break;

                case "E":
                    sDescr = "Rechazada";
                    break;

                case "X":
                    sDescr = "Pendiente de Regenerar";
                    break;

                case "G":
                    sDescr = "Factura ya Regenerada";
                    break;

            }

            return sDescr;

            }
        }

        // Status de envio a cambiar
        public string StatusEnvioNuevo { get; set; }

        // Status de envio
        public string DetalleStatus { get; set; }

        // Autorizacion del SRI para la Factura
        public string AutSRI { get; set; }

        // Fecha de Acuse de Recibo
        public DateTime? FechaRespuesta { get; set; }


        // Codigo de Error
        public int? CodigoError { get; set; }

        // Informacion Adicional sobre el error
        public string ErrorInfAdicional { get; set; }

    }

    [Serializable]

    // Lista de Envios de archivos a la agencia recaudadora
    public class EstadosEnvioSRIL : List<EstadosEnvioSRI>
    {
    }
}
