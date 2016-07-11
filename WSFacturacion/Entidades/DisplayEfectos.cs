using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    [Serializable]

    public class DisplayEfectos
    {
        // Constructor sin parametros
        public DisplayEfectos()
        {
        }

        // En el constructor recibimos el parametro de codigo de efecto para cuando se arma la estructura del mensaje
        public DisplayEfectos(string codigoEfecto,
                              string decripcionEfecto)
        {
            this.Codigo = codigoEfecto;
            this.Descripcion = decripcionEfecto;
        }

        // Codigo de Efecto
        public string Codigo { get; set; }

        // Descripcion del efecto
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
  }


    [Serializable]
    
    public class DisplayEfectosL : List<DisplayEfectos>
        {
        }
}
