using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class TecnoTag
    {
    /// ***********************************************************************************************
    /// <summary>
    /// En el constructor de la clase asigna los valores de la Tecnología que utiliza la administradora de Tag
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

        public TecnoTag(int codigo, string descr)
        {
            this.Codigo = codigo;
            this.Descripcion = descr;
        }

        public TecnoTag()
        {

        }

        // Código de Tecnología que utiliza la administradora de Tag.
        public int Codigo { get; set; }

        // Descripción de Tecnología que utiliza la administradora de Tag.
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos de Tecnología que utiliza la administradora de Tag.
    /// </summary>
    public class TecnoTagL : List<TecnoTag>
    {
    }

 }