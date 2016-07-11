using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class CausaCierre
    {
        public CausaCierre(int codCierre, string cierreDescripcion)
        {
            Codigo = codCierre;
            Descripcion = cierreDescripcion;
        }

        // Codigo de causa de cierre
        public int Codigo { get; set; }

        // Descripcion de la causa de cierre
        public string Descripcion { get; set; }


        public string EnVia { get; set; }

        // Valor por defecto de la clase (para mostrar en la grilla)
        public override string ToString()
        {
            return Descripcion;
        }
    }
    public class causaCierreL : List<CausaCierre>
    {
    }
}
