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
    public class ListaPuntosVenta
    {
        public string Establecimiento { get; set; }
        public string PuntoVenta { get; set; }
        public string Nombre { get; set; }
        public int Numvia { get; set; }
        public string Origen { get; set; }
        public bool Deleted { get; set; }

        public string Descripcion
        {
            get { return PuntoVenta + " " + Nombre + (Deleted ? " (baja)":""); }

        }
    }

   
   
    /// <summary>
    /// Lista de objetos ListaPuntosVenta.
    /// </summary>
    [Serializable]
    public class ListaPuntosVentaL : List<ListaPuntosVenta>
    {
    }
}
