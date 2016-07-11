using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    [Serializable]

    public class DisplayVelocidad
    {
        // Constructor sin parametros
        public DisplayVelocidad()
        {
        }

        // En el constructor recibimos el parametro de codigo de velocidad para cuando se arma la estructura del mensaje
       public DisplayVelocidad(byte codigoVelocidad,
                               string descripcionVelocidad)
       {
           this.Codigo = codigoVelocidad;
           this.Descripcion = descripcionVelocidad;
       }

       // Codigo de velocidad con la que se muestra el mensaje
        public int Codigo { get; set; }

       // Descripcion de la velocidad con la que se muestra el mensaje
       public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }


    [Serializable]

    public class DisplayVelocidadL : List<DisplayVelocidad>
    {
    }

}
