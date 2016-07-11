using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace Telectronica.Peaje
{
    [Serializable]
    [XmlRootAttribute(ElementName = "CarrilStatus", IsNullable = false)]
    public class CarrilStatus
    {

        public CarrilStatus(int xiCarril, ViaStatus xStatusHaciaArriba, ViaStatus xStatusHaciaAbajo, bool xbCarrilAbierto,
                           bool xbEsBidi, string xsPrincipal)
        {

            this.Carril = xiCarril;
            this.StatusHaciaArriba = xStatusHaciaArriba;
            this.StatusHaciaAbajo = xStatusHaciaAbajo;
            this.CarrilAbierto = xbCarrilAbierto;
            ////////this.AbiertaHaciaArriba = xbAbiertaHaciaArriba;
            ////////this.AbiertaHaciaAbajo = xbAbiertaHaciaAbajo;
            this.esBidi = xbEsBidi;
            this.Principal = xsPrincipal;
        }

        public CarrilStatus()
        {

        }

        // DEFINICION DE PROPIEDADES

        public int Carril { get; set; }

        public ViaStatus StatusHaciaArriba { get; set; }

        public ViaStatus StatusHaciaAbajo { get; set; }

        public bool CarrilAbierto { get; set; }

        ////////public bool AbiertaHaciaArriba { get; set; }

        ////////public bool AbiertaHaciaAbajo { get; set; }

        public bool esBidi { get; set; }                          // INDICA SI EL CARRIL ES BI-DIRECCIONAL

        public string Principal { get; set; }                       // INDICA EL SENTIDO DE LA CABINA PRINCIPAL: A (ASCENDENTE) / D (DESCENDENTE)
    }

    [Serializable]
    public class CarrilStatusL : List<CarrilStatus>
    {

    }

}

