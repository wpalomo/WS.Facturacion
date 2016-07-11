using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    [Serializable]

    public class ListaActualizacionModo
    {
        // Constructor sin parametros
        public ListaActualizacionModo()
        {
        }

        // En el constructor recibimos el parametro de codigo de Modo para cuando se arma la estructura del Listado
        public ListaActualizacionModo(string codigoModo,
                                       string descripcionModo)
        {
            this.Codigo = codigoModo;
            this.Descripcion = descripcionModo;
        }

        // Codigo de Modo de la Lista
        public string Codigo { get; set; }

        // Descripcion de Modo de la Lista
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]

    public class ListaActualizacionModoL : List<ListaActualizacionModo>
    {
    }

}
