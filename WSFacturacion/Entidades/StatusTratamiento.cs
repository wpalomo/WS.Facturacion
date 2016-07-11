using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class StatusTratamiento
    {
        public string CodigoRechazo { get; set; }        
        public string Estado { get; set; }
        public string CodEstado { get; set; }        
        public int Cantidad { get; set; }
        public double Monto { get; set; }
        public string Causa{ get; set; }
        public OSAsTag Administradora { get; set; }

        public StatusTratamiento()
        {
        }

        public StatusTratamiento(string codigoRechazo, string estado, int cantidad, double monto, string causa, string codEstado)
        {
            this.CodigoRechazo = codigoRechazo;
            this.Estado = estado;
            this.Cantidad = cantidad;
            this.Monto = monto;
            this.Causa = causa;
            this.CodEstado = codEstado;
        }
    }

    [Serializable]
    public class StatusTratamientoL : List<StatusTratamiento>
    {
    }
}
