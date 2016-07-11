using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
     #region DISPLAY: Clase para entidad de Mensajes del Display.
        /// <summary>
    /// Estructura de una entidad Mensajes del Display
        /// </summary>

    [Serializable]

    public class Display
    {
        public  Display(string CodSentido,string DescrSentido, 
                       string IdCodigo ,string DescrCodigo,
                       string Texto, string esActivo,
                       string esTitilante, string esLetraAncha,
                       DisplayEfectos Efectos, DisplayVelocidad Velocidad)
        {

            this.CodigoSentido = CodSentido;
            this.DescrSentido = DescrSentido;
            this.IdCodigo = IdCodigo;
            this.DescrCodigo = DescrCodigo;
            this.Texto = Texto;
            this.Activo = esActivo;
            this.Titilante = esTitilante;
            this.LetraAncha = esLetraAncha;
            this.Efectos = Efectos;
            this.Velocidad = Velocidad;

        }

        public Display(string CodSentido, string DescSentido)
        {
            this.CodigoSentido = CodSentido;
            this.DescrSentido = DescSentido;
        }

        public Display()
        {

        }
        
        // Codigo de Sentido del Mensajes del Display
        public string CodigoSentido { get; set; }

        // Descripcion de Sentido del Mensajes del Display
        public string DescrSentido { get; set; }

        // Id Codigo del Mensajes del Display
        public string IdCodigo { get; set; }

        // Descripcion del Codigo del Mensajes del Display
        public string DescrCodigo { get; set; }

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
        public DisplayEfectos Efectos { get; set; }

        // Velocidad Ancha del Mensajes del Display
        public DisplayVelocidad Velocidad { get; set; }

        /// <summary>
        /// Lista de objetos Mensaje Display.
        /// </summary>
        /// 

    }


    [Serializable]
    
    public class DisplayL : List<Display>
    {
    }        

     #endregion
}


