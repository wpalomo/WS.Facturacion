using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class SimulacionDePaso
    {
        public SimulacionDePaso(int codSip, string descSip)
        {
            Codigo = codSip;
            Descripcion = descSip;
        }

        public SimulacionDePaso()   // Constructor vacio
        {
        }

        // Codigo de causa de cierre
        public int Codigo { get; set; }

        // Descripcion de la causa de cierre
        public string Descripcion { get; set; }

        // Valor por defecto de la clase (para mostrar en la grilla)
        public override string ToString()
        {
            return Descripcion;
        }


        // Atributos usados en validacion

        // via SIP
        public int viaSIP { get; set; }

        // Fecha y Hora del SIP
        public DateTime FechaSIP { get; set; }

        // Numero de Evento del SIP
        public int numevSip { get; set; }

        // Forma de Pago del SIP
        public FormaPago forpag { get; set; }

        // Categoria del SIP
        public CategoriaManual Categ { get; set; }

        // Numero de Ticket
        public int ticket { get; set; }

        // Parte
        public int parte { get; set; }

        // Tipo Simulacion
        public string tipoSimulacion { get; set; }

        // Placa
        public string Placa { get; set; }

        // Observacion Plaza
        public string observacionPlaza { get; set; }

        // Si el SIP fue utilizado
        public bool utilizado { get; set; }

        // via Violacion
        public int? viaViol { get; set; }

        //Fecha y Hora Violacion
        public DateTime? FechaViol { get; set; }

        // Parte
        public int? parteViol { get; set; }

    }
    public class SimulacionDePasoL : List<SimulacionDePaso>
    {
    }
}
