using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region Autorizacion SRI: Clase para entidad de Autorizacion en las que se divide la concesión.
    /// <summary>
    /// Estructura de una entidad Autorizacion
    /// </summary>

    [Serializable]

    public class Autorizacion
    {
        public Autorizacion()
        {
           
        }

        public string TipoDocumento  { get; set; }

        public string NumeroAutorizacion { get; set; }

        public DateTime FechaInicio { get; set; }       //Fecha que se imprime en el comprobante

        public DateTime FechaCaducidad { get; set; }    //Fecha de vigencia impresa en el comprobante

        public DateTime FechaProgramacion { get; set; }     //Fecha en que las vias comienzan a usar la autorizacion

        public string TipoDocumentoDesc {

            get
            {
                string Named = "Facturas";
                if (TipoDocumento != null && TipoDocumento.Trim() != "1")
                    Named = "Otro Tipo";
                return Named;
            }

        }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Autorizacion.
    /// </summary>
    public class AutorizacionL : List<Autorizacion>
    {
    }

    #endregion
}
