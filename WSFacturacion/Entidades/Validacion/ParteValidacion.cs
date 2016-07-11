using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Tesoreria;

namespace Telectronica.Validacion
{
    [Serializable]
    public class ParteValidacion : Parte
    {
        public ParteValidacion() : base ()
        {
        }

        public ParteValidacion(int numero, DateTime jornada, int turno):base(numero, jornada, turno)
        {
        }

        //public string EstadoValidacion {get; set;}
        //public string EstadoParte {get; set;}

        public EstadoValidacion EstadoValidacion { get; set; }
        public string EstadoFallo {get; set;}
        public string EstadoReposicion { get; set; }
        public string imgEstadoReposicion { get; set; }
        public string imgEstadoValidacion { get; set; }
        public string imgEstadoParte { get; set; }
        public string imgEstadoFallo { get; set; }
        public string   TipoParte {get; set;}
        public string NivelParte {get; set;}
        public int AnomaliasSinValidar {get; set;}
        public int? Transitos {get; set;}
        public int SIPs {get; set;}
        public int Cancelaciones {get; set;}
        public int DACs {get; set;}
        public int Exentos {get; set;}
        public int TicketsManuales {get; set;}
        public int Violaciones {get; set;}
        public int PagosDiferidos {get; set;}
        public int AutorizacionesPaso {get; set;}
        public double Sobrante {get; set;}
        public double Faltante {get; set;}
        public double Fallo {get; set;}
        public double PedidoFacturacionFallo {get; set;}                   
        public double ReposicionAdicional{get; set;}
        public double ReposicionAdicionalPaga {get; set;}
        public double ReposicionEconomicaPedida { get; set; }
        public double ReposicionEconomicaPaga { get; set; }
        public double FalloFacturado {get; set;}
        public string Observaciones {get; set;}
        public string Mante { get; set; }
                          
    }

    [Serializable]
    public class EstadoValidacion 
    {
        public string IDValidador { get; set; }
        public DateTime? Fecha { get; set; }
        public string Terminal { get; set; }
        public string EstadoParte { get; set; }
        public string Estado { get; set; }
        public string EstadoParteDesc 
        {
            get
            {
                string estado = "No visto";
                if (EstadoParte == "P") estado = "Pendiente";
                if (EstadoParte == "V") estado = "Validado";
                if (EstadoParte == "N") estado = "No visto";
                return estado;
            }
        }
        public string EstadoValidacionDesc
        {
            get
            {
                string estado = "Disponible";
                if(Estado == "D") estado = "Disponible";
                if(Estado == "V") estado = "Validandose";
                return estado;
            }
        }
    }

    /// <summary>
    /// Lista de objetos Parte.
    /// </summary>
    /// 
    [Serializable]
    public class ParteValidacionL : List<ParteValidacion>
    {
    }
}
