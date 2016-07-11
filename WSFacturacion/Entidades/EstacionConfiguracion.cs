using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CONFIG: Clase para entidad de Config coniguracion de la plaza.

    /// <summary>
    /// Estructura de una entidad Config
    /// </summary>
    [Serializable]
    public class EstacionConfiguracion
    {
        public EstacionConfiguracion()
        {
        }

        public int IdEstacion { get; set; }
        public string SentidoAsc { get; set; }
        public string SentidoDesc { get; set; }
        public int? ViaDecalada { get; set; }
        public string EstacionDecalada
        {
            get { return ViaDecalada > 0 ? "S" : "N"; }
        }
        public int TiempoViaInactiva { get; set; }
        public double DiscrepanciaAlarma { get; set; }
        public int TicketAdAsc { get; set; }
        public int TicketAdDesc { get; set; }
        public string PrimerCarrilalaIzquierda { get; set; }
        public bool esPrimerCarrilalaIzquierda
        {
            get { return (PrimerCarrilalaIzquierda == "S"); }
            set { PrimerCarrilalaIzquierda = (value? "S" : "N");}
        }
        public string AscendenteHaciaArriba { get; set; }
        public bool esAscendenteHaciaArriba
        {
            get { return (AscendenteHaciaArriba == "S"); }
            set { AscendenteHaciaArriba = (value? "S" : "N");}
        }
        /// <summary>
        /// Si la estacion posee o no tesorero
        /// </summary>
        public string EstacionConTesorero { get; set; }
        /// <summary>
        /// Obtiene o establece el valor de la propiedad EstacionConTesorero desde un booleano
        /// </summary>
        public bool esEstacionConTesorero
        { 
            get { return (EstacionConTesorero == "S"); }
            set { EstacionConTesorero = (value ? "S" : "N"); }
        }
        /// <summary>
        /// Obtiene o establece el numero de plaza grabado en la antena QFree
        /// </summary>
        public string PlazaAntena { get; set; }
        /// <summary>
        /// Obtiene o establece el BEACON ID de la antena QFree
        /// </summary>
        public string BeaconId { get; set; }
    }
    
    /// <summary>
    /// Lista de objetos Config.
    /// </summary>
    [Serializable]
    public class EstacionConfiguracionL : List<EstacionConfiguracion>
    {
    }

    #endregion
}