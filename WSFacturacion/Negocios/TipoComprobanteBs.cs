using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class TipoComprobanteBs
    {
        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <returns></returns>
        public static TipoComprobanteL getTiposComprobantes(bool incluirComprobanteTodas)
        {
            TipoComprobanteL comprobantes = new TipoComprobanteL();
            if(incluirComprobanteTodas)
                comprobantes.Add(new TipoComprobante(null, "<Todas>"));
            comprobantes.Add(new TipoComprobante("1", "Factura"));
            comprobantes.Add(new TipoComprobante("2", "Boleta"));

            return comprobantes;
        }
    }
}
