using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Runtime.Serialization;

namespace Telectronica.Tesoreria
{
    #region FondoDeCambio: Clase para entidad de Fondo de Cambio.

    /// <summary>
    /// Estructura de una entidad Fondo de Cambio
    /// </summary>    
    [Serializable]
    public class FondoDeCambio
    {
        public FondoDeCambio()
        {
        }

        public Estacion Estacion { get; set; }
        public Parte Parte { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public Usuario TesoreroEntrega { get; set; }
        public string Estado { get; set; }
        public string DescEstado { get; set; }
        public bool Entregado { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public Usuario TesoreroDevolucion { get; set; }
        public string Confirmado { get; set; }
        public bool EstaConfirmado { get; set; }
        public bool EstaSeleccionado { get; set; }
        public DateTime Jornada { get; set; }
        public string ComentarioEliminar { get; set; }
        public int Turno { get; set; }
    }

    /// <summary>
    /// Lista de objetos FondoDeCambio.
    /// </summary>
    [Serializable]
    public class FondoDeCambioL : List<FondoDeCambio>
    {
    }

    #endregion

    #region ValorFondoDeCambio: Clase para entidad de Fondo de Cambio.

    /// <summary>
    /// Estructura de una entidad Valor de Fondo de Cambio
    /// </summary>
    [Serializable]
    public class ValorFondoDeCambio
    {
        /// <summary>
        /// Obtiene o establece el código del Valor de Fondo de Cambio
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece el valor monetario
        /// </summary>
        public decimal? Valor { get; set; }

        /// <summary>
        /// Obtiene o establece las estaciones habilitadas
        /// </summary>
        public ValorFCEstacionL EstacionesHabilitadas { get; set; }
    }

    /// <summary>
    /// Lista de objetos ValorFondoDeCambio.
    /// </summary>
    [Serializable]
    public class ValorFondoDeCambioL : List<ValorFondoDeCambio>
    {
    }

    /// <summary>
    /// Estructura para la lista de estacion habilitadas para un valor de fondo de cambio
    /// </summary>
    [Serializable]
    public class ValorFCEstacion
    {
        public Estacion Estacion { get; set; }
        public int NumeroEstacion
        {
            get { return Estacion.Numero; }
        }
        public string NombreEstacion
        {
            get { return Estacion.Nombre; }
        }
        public string Habilitado { get; set; }
        public bool EsHabilitado
        {
            get { return Habilitado == "S" ? true : false; }
            set { Habilitado = value ? "S" : "N"; }
        }
    }

    /// <summary>
    /// Lista de objetos ValorFCEstacion
    /// </summary>
    [Serializable]
    public class ValorFCEstacionL : List<ValorFCEstacion>
    {
    }

    #endregion
}