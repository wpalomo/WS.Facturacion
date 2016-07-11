using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]

    public class ListaActualizacionTipoLista
    {
        // Constructor sin parametros
        public ListaActualizacionTipoLista()
        {
        }

        // En el constructor recibimos el parametro de codigo de Tipo de Lista para cuando se arma la estructura del Listado
        public ListaActualizacionTipoLista(string codigoTipoLista,
                                           string descripcionTipoLista)
        {
            this.Codigo = codigoTipoLista;
            this.Descripcion = descripcionTipoLista;
        }

        // Codigo de Tipo de Lista
        public string Codigo { get; set; }

        // Descripcion del Tipo de Lista
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }


    [Serializable]

    public class ListaActualizacionTipoListaL : List<ListaActualizacionTipoLista>
    {
    }
}

