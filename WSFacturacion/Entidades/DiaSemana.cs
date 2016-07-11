using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    [Serializable]

    public class DiaSemana
    {
        // Constructor por defecto
        public DiaSemana()
        {
        }

        // En el constructor asignamos los valores al objeto
        public DiaSemana(int codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        // Codigo de dia de la semana
        public int Codigo { get; set; }

        // Descripcion del dia 
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }


    [Serializable]

    public class DiaSemanaL : List<DiaSemana>
    {
    }
}
