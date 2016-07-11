using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class TipoComprobante
    {
        public string Tipo { get; set; }
        public string Descripcion { get; set; }

        public TipoComprobante(string tipo, string descripcion ){
            Tipo = tipo;
            Descripcion = descripcion;
        }
    }

    /// <summary>
    /// Lista de objetos TipoComprobante.
    /// </summary>
    [Serializable]
    public class TipoComprobanteL : List<TipoComprobante>
    {
    }
}
