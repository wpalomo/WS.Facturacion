using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class RangosTag
    {
    /// ***********************************************************************************************
    /// <summary>
    /// En el constructor de la clase asigna los valores de un Emisor de Tag
    /// </summary>
    /// <typeparam name="string">string</typeparam>
    /// <param name="RangoDesde">Rango Desde</param>
    /// <typeparam name="string">string</typeparam>
    /// <param name="RangoHasta">Rango Hastar</param>
    /// <typeparam name="OSA">OSA</typeparam>
    /// <param name="oOSA">Objeto OSA</param>
    /// <typeparam name="EmisoresTag">EmisoresTag</typeparam>
    /// <param name="oEmisor">Objeto Emisor</param>
    /// <returns></returns>
    /// ***********************************************************************************************

        public RangosTag(int identity, string RangoDesde, string RangoHasta, AdministradoraTags oOSA, EmisoresTag oEmisor, TecnoTag oTecnoTag)
        {
            this.Codigo = identity;
            this.RangoDesde = RangoDesde;
            this.RangoHasta = RangoHasta;
            this.Administradora = oOSA;
            this.Emisor = oEmisor;
            this.Tecnologia = oTecnoTag;
        }

        public RangosTag()
        {

        }

        // Primery Key
        public int Codigo { get; set; }

        // Rango Desde
        public string RangoDesde { get; set; }

        // Rango Hasta
        public string RangoHasta { get; set; }

        // Administradora del Tag
        public AdministradoraTags Administradora { get; set; }

        // Emisor del Tag
        public EmisoresTag Emisor { get; set; }

        // Tecnología que utiliza la administradora de Tag
        public TecnoTag Tecnologia { get; set; }

        //Para mostrar en las grillas, etc
        public string Descripcion()    
        {
                return Administradora.Descripcion;
        }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Emisores de Tag.
    /// </summary>
    public class RangosTagL : List<RangosTag>
    {
    }

 }
