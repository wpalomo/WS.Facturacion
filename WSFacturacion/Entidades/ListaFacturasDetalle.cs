using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

namespace Telectronica.Peaje
{
    [Serializable]
    public class ListaFacturasDetalle
    {
        public int Cantidad;
        public string DescripcionItem;
        public decimal PrecioUnitario;

    }



    /// <summary>
    /// Lista de objetos ListaFacturasDetalle.
    /// </summary>
    [Serializable]
    public class ListaFacturasDetalleL : List<ListaFacturasDetalle>
    {
    }
}
