using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    [Serializable]
    public class Recuento
    {
        // Constructor vacio
        public Recuento() { }


        public Usuario Usuario { get; set; }

        // Id del usuario que registra el recuento
        public string IdUsuario { get; set; }

        // Nombre del archivo procesado
        public string NombreArchivo { get; set; }

        // Estacion
        public Estacion Estacion { get; set; }

        // Número de Recuento
        public int NumeroRecuento { get; set; }

        // Fecha
        public DateTime Fecha { get; set; }

        // Detalle de los recuentos
        public RecuentoDetalleL Detalles { get; set; }

        // Detalle de los recuentos
        public Parte Partes { get; set; }

        // Monto Total Recontado
        public decimal Recontado { get; set; }
    }

    [Serializable]
    public class RecuentoL : List<Recuento>
    {
    }

}
