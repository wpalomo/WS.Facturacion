using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje.Tesoreria
{
    public class Velocidad
        {
            public int Codigo { get; set; }
            public string Descripcion { get; set; }

            //Para mostrar en las grillas, etc
            public override string ToString()
            {
                return Descripcion;
            }
        }

    public class VelocidadL : List<Velocidad>
        {
        }

 }
