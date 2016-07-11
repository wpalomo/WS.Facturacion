using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    #region ESTACION: Clase para entidad de Estaciones de Peaje.

    /// <summary>
    /// Estructura de una entidad Estacion de Peaje
    /// </summary>
    [Serializable]    
    public class Estacion
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Estacion()
        {
            Sentido = new ViaSentidoCirculacion();
            Site = new Site();
        }

        /// <summary>
        /// Constructor con dos parámetros
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="nombre"></param>
        public Estacion(int numero, string nombre)
        {
            Numero = numero;
            Nombre = nombre;
            Sentido = new ViaSentidoCirculacion();
            Site = new Site();
        }

        /// <summary>
        /// Numero de Estacion de Peaje
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Nombre de la Estacion de Peaje
        /// </summary>
        public string Nombre { get; set; }


        public string EstablecimientoVisa { get; set; }

        /// <summary>
        /// Establecimiento
        /// </summary>
        public Int16 Establecimiento { get; set; }

        /// <summary>
        /// Devuelve el atributo Establecimiento como string
        /// </summary>
        public string EstablecimientoDesc
        {
            get
            {
                if (Establecimiento != null && Establecimiento != 0)
                {
                    return Establecimiento.ToString();
                }
                return "";
            }
        }
        
        /// <summary>
        /// Nombre de la Base de Datos 
        /// </summary>
        public string BaseDatos { get; set; }

        /// <summary>
        /// Nombre del servidor de datos
        /// </summary>
        public string ServidorDatos { get; set; }

        /// <summary>
        /// Zona a la que pertenece la Estacion de Peaje
        /// </summary>
        public Zona Zona { get; set; }

        /// <summary>
        /// Direccion URL del sitio del sistema en la plaza
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Direccion del Establecimiento
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Contiene el sentido de la circulación de la estación
        /// </summary>
        public ViaSentidoCirculacion Sentido { get; set; }

        /// <summary>
        /// Obtiene o establece el número de site
        /// </summary>
        public Site Site { get; set; }

        /// <summary>
        /// Indica si la estación permite los retiros anticipados
        /// </summary>
        public string PermiteRetirosAnticipados { get; set; }

        /// <summary>
        /// Indica si la estación permite los retiros anticipados (Valor Booleano)
        /// </summary>
        public bool esPermiteRetirosAnticipados
        {
            get { return PermiteRetirosAnticipados == "S"; }
            set { PermiteRetirosAnticipados = ((value)? "S" : "N"); }
        }

        /// <summary>
        /// Valor por defecto de la clase (para mostrar en la grilla)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Nombre;
        }
    }    

    /// <summary>
    /// Lista de objetos Estacion.
    /// </summary>
    [Serializable]
    public class EstacionL : List<Estacion>
    {       
    }

    #endregion
}
