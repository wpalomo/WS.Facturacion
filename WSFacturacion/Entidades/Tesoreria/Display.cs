using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje.Tesoreria
{
     #region DISPLAY: Clase para entidad de Mensajes del Display.
        /// <summary>
    /// Estructura de una entidad Mensajes del Display
        /// </summary>

    public class Display
    {


        public Display(string Sentido, string Codigo,
                       string Texto, string esActivo,
                       string esTitilante, string esLetraAncha,
                       string Efectos, Int16 Velocidad)
        {

            this.Sentido = Sentido;
            this.Codigo = Codigo;
            this.Texto = Texto;
            this.Activo = esActivo;
            this.Titilante = esTitilante;
            this.LetraAncha = esLetraAncha;
            this.Efectos = Efectos;
            this.Velocidad = Velocidad;

        }

        public Display()
        {

        }
        
        // Sentidos del Mensajes del Display

        public string Sentido { get; set; }

        // Codigo del Mensajes del Display

        public string Codigo { get; set; }

        // Texto del Mensajes del Display

        public string Texto { get; set; }

        // Activo del Mensajes del Display

        public string Activo{ get; set; }

        public bool esActivo
        {
            get { return Activo == "S"; }
            set { Activo = value ? "S" : "N"; }
        }

        // Titilante del Mensajes del Display

        public string Titilante { get; set; }

        public bool esTitilante
        {
            get { return Titilante == "S"; }
            set { Titilante = value ? "S" : "N"; }
        }

        // Letra Ancha del Mensajes del Display

        public string LetraAncha { get; set; }

        public bool esLetraAncha
        {
            get { return LetraAncha == "S"; }
            set { LetraAncha = value ? "S" : "N"; }
        }

        // Efectos Ancha del Mensajes del Display

        public string Efectos { get; set; }

        // Velocidad Ancha del Mensajes del Display

        public Int16 Velocidad { get; set; }

        /// <summary>
        /// Lista de objetos Mensaje Display.
        /// </summary>
        /// 

    }
        public class DisplayL : List<Display>
        {

        }        

     #endregion
}

