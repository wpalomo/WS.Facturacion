using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Estructura de una identidad de Tipo ListaTarjetasRUTA
    /// </summary>
    [Serializable]

    public class ArchivoSubsidio
    {

        //Numero de Lista
        public int NumeroLista { get; set; }

        //Fecha de operacion
        public DateTime FechaOperacion { get; set; }

        //Fecha del Archivo
        public DateTime FechaArchivo { get; set; }

        //Status Activo, Importando, Viejo
        public char Status { get; set; }

        // Descripcion del status de la lista
        public string StatusDescripcion
        {
            get
            {
                string descripcion = "";

                switch (Status)
                {
                    case 'A':
                        descripcion =  "Actualizado";
                        break;

                    case 'V':
                        descripcion = "Desactualizado";
                        break;

                    default:
                        descripcion = "";
                        break;
                }

                return descripcion;
            }
        }

        //Usuario
        public string Usuario { get; set; }

        //Usuario
        public Usuario oUsuario { get; set; }

        //Estacion de Origen
        public int EstacionOrigen { get; set; }

        //Estacion de Origen
        public Estacion oEstacionOrigen { get; set; }

        //Descripcion de la estacion de origen
        public string EstacionOrigenDescripcion
        {
            get
            {
                return oEstacionOrigen.Nombre;
            }
        }

        //Nombre de la lista
        public string ListaNombre { get; set; }
    }

    public class ArchivoSubsidioL : List<ArchivoSubsidio>
    {
    }

}
