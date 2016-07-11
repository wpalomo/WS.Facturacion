using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TOOLBAR: Clase para entidad de Toolbar (iconos en la toolbar).
    /// <summary>
    /// Estructura de una entidad Toolbar
    /// </summary>

    [Serializable]
    
    public class Toolbar
    {
        public Toolbar()
        {
        }

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de una zona en particular
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public Toolbar(string perfil, string modulo, bool gst, 
                    string pagina, bool habilitado, string icono, int orden, bool nuevaventana)
        {
            this.Perfil = perfil;
            this.EsGestion = gst;
            this.Modulo = modulo;
            this.Pagina = pagina;
            this.Habilitado = habilitado;
            this.Icono = icono;
            this.Orden = orden;
            this.EnNuevaVentana = nuevaventana;
        }

        //Es de Gestion?
        public bool EsGestion { get; set; }

        //Modulo
        public string Modulo { get; set; }

        //Perfil
        public string Perfil { get; set; }

        // Nombre Pagina
        public string Pagina { get; set; }

        // Nombre del Icono
        public string Icono { get; set; }

        // Habilitado?
        public bool Habilitado { get; set; }

        // Orden
        public int Orden { get; set; }

        // Abrir en nueva ventana?
        public bool EnNuevaVentana { get; set; }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Toolbar.
    /// </summary>
    public class ToolbarL : List<Toolbar>
    {
    }

    #endregion
}

