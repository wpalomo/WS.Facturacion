using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region LOG DE ACTIVIDAD que registra una aplicacion. Sirve para controlar que solo un proceso se ejecute a la vez 
    /// ***********************************************************************************************
	/// <summary>
    /// Estructura de una entidad LogActividad
    /// </summary>
 /// ***********************************************************************************************

    [Serializable]

    public class LogActividad
    {

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de un alarma en particular
        /// </summary>
        /// ***********************************************************************************************

        public LogActividad()
        {
        }


        public LogActividad(string tipoActividad, bool esejecuntadoProceso, bool esokResultadoUltimoProceso, 
                            DateTime? fechaInicio, DateTime? fechaUltimoLogueo, string nombreTerminal, 
                            string nombreSesion, string descripcion, int? elementosModificados)
        {

            this.TipoActividad = tipoActividad;
            this.esEjecutandoProcesoActual = esejecuntadoProceso;
            this.esOKUltimoProceso = esokResultadoUltimoProceso;
            this.FechaInicioProceso = fechaInicio;
            this.FechaUltimoLogueo = fechaUltimoLogueo;
            this.NombreTerminalEjecucionProceso = nombreTerminal;
            this.NombreSesionEjecucionProceso = nombreSesion;
            this.DescripcionEjecucionUltimoProceso = descripcion;
            this.ElementosAfectadosPorUltimoProceso = elementosModificados;

        }

        // Tipo de programa que loguea la actividad
        public string TipoActividad { get; set; }

        
        // Estado del proceso: A-Activo, I-Inactivo
        public string EstadoProcesoActual { get; set; }

        // Booleano para determinar si esta corriendo el proceso actual
        public bool esEjecutandoProcesoActual 
        { 
            get { return EstadoProcesoActual == "A"; }
            set { EstadoProcesoActual = value ? "A" : "I"; }
        }


        // Resultado de la ultima ejecucion: O-Ok, X-Error
        public string ResultadoUltimoProceso { get; set; }

        // Booleano para determinar si el ultimo proceso realizado funciono bien o no
        public bool esOKUltimoProceso
        {
            get { return ResultadoUltimoProceso == "O"; }
            set { ResultadoUltimoProceso = value ? "O" : "X"; }
        }


        // Fecha de activacion o inicio del proceso
        public DateTime? FechaInicioProceso { get; set; }

        // Fecha de ultimo logueo de la actividad
        public DateTime? FechaUltimoLogueo { get; set; }

        // Nombre de la terminal desde la cual se ejecuta el proceso
        public string NombreTerminalEjecucionProceso { get; set; }

        // Nombre de la sesion del usuario el cual ejecuta el proceso
        public string NombreSesionEjecucionProceso { get; set; }

        // Nombre de la sesion del usuario el cual ejecuta el proceso
        public string DescripcionEjecucionUltimoProceso { get; set; }

        // Numero que puede indicar los elementos, registros, etc que pudieran afectarse con la ultima ejecucion
        public int? ElementosAfectadosPorUltimoProceso { get; set; }


    }


    [Serializable]

    /// ***********************************************************************************************
    /// <summary>
    /// Lista de objetos LogActividad.
    /// </summary>
    /// ***********************************************************************************************
    public class LogActividadL : List<LogActividad>
    {
    }
    
    #endregion
}