using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region EXENTOESTACION: Clase para entidad de Exento Estacion.

    /// <summary>
    /// Estructura de una entidad Exento Estacion
    /// En este cliente se habilitan los exentos por Sub-Estacion (Estacion y sentido)
    /// </summary>

    [Serializable]

    public class ExentoEstacion
    {

        public ExentoEstacion(Subestacion oSubEstacion, string Habilitado)
        {
            this.subEstacion = oSubEstacion;
            this.Habilitado = Habilitado;
        }

        public ExentoEstacion()
        {

        }

        // Código de Franquisia
        public Int16 CodigoExento { get; set; }

        // Estacon en la que esta habilitada
        public Subestacion subEstacion { get; set; }

        // Retorna el numero de estacion 
        public int NumeroEstacion
        {
            get { return subEstacion.Estacion.Numero; }
        }

        // Retorna el nombre de la estacion 
        public string NombreEstacion
        {
            get { return subEstacion.Estacion.Nombre; }
        }

        // Retorna el numero de la sub-estacion
        public int NumeroSubEstacion 
        {
            get { return subEstacion.CodigoSubEstacion; }
        }

        // Retorna la descripcion de la sub-estacion
        public string DescripcionSubEstacion
        {
            get { return subEstacion.Descripcion; }
        }

       // Habilitado
        public string Habilitado { get; set; }

        public bool esHabilitado
        {
            get { return Habilitado == "S"; }
            set { Habilitado = value ? "S" : "N"; }

            /// <summary>
            /// Lista de objetos Exento Estacion.
            /// </summary>
            /// 
        }

    }


    [Serializable]

    public class ExentoEstacionL : List<ExentoEstacion>
        {

        /// ***********************************************************************************************
        /// <summary>
        /// Busca en el Tipo de Exento una sub-estacion determinada
        /// </summary>
        /// <param name="codigo">string - codigo de sub-estacion a buscar
        /// <returns>objeto ExentoEstacion del tipo de exento buscado</returns>
        /// ***********************************************************************************************
        public ExentoEstacion FindEstaciones(string codigoSubEstacion)
        {
            ExentoEstacion oExentoEst=null;
            foreach (ExentoEstacion item in this)
            {
                if (item.NumeroSubEstacion.ToString() == codigoSubEstacion)
                {
                    oExentoEst = item;
                    break;
                }
            }
            return oExentoEst;
        }

     }

    #endregion

}
