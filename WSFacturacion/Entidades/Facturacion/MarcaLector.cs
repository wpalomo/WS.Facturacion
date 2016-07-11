using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    [Serializable]
    public class MarcaLector
    {

        public MarcaLector()
        {
        }

        public MarcaLector(short codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }

        public short Codigo { get; set; }
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }


    [Serializable]

    public class MarcaLectorL : List<MarcaLector>
    {
    }
}
