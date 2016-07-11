using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CONFIGCCO: Clase para entidad de Configcco en las que se divide la concesión.

    /// <summary>
    /// Estructura de una entidad Configcco
    /// </summary>
    [Serializable]
    public class GestionConfiguracion
    {
        public GestionConfiguracion()
        {
        }

        public int IdReplicacion { get; set; }

        public string Idioma { get; set; }

        public DateTime FechaDesdeDatos { get; set; }

        public double PorcentajeIva { get; set; }

        public int CategoriaMonocategoria { get; set; }

        public int VencimientoPagoDiferido { get; set; }

        public double MultiplicadorViolaciones { get; set; }

        public int MonedaVia { get; set; }

        public string NombreConcesionario { get; set; }

        public string DireccionURL { get; set; }

        public double PorcentajeRetencionServicios { get; set; }

        public double PorcentajeRetencionBienes { get; set; }

        public string CobraEixos { get; set; }

        /// <summary>
        /// Obtiene o establece el código de pais grabado en la antena QFree
        /// </summary>
        public string PaisAntena { get; set; }

        /// <summary>
        /// Obtiene o establece el Código de emisor de TAG grabado en la antena QFree
        /// </summary>
        public string ConcesionarioAntena { get; set; }

        /// <summary>
        /// Cada que cantidad de ejes saca una foto con la via AVI
        /// </summary>
        public byte EjesParaFotoAVI { get; set; }

        /// <summary>
        /// Indica la diferencia que puede haber entre lo liquidado y lo registrado por la vía para poder mostrar advertencia.
        /// </summary>
        public decimal DiferenciaLiquidacion { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica el tiempo en segundos que tiene que pasar para que el cliente gráfico muestre las alarmas correspondient
        /// </summary>
        public int TiempoAlarmaClienteGrafico { get; set; }

        /// <summary>
        /// Devuelve el valor de la propiedad DiferenciaLiquidacion como string y formateado
        /// </summary>
        public string sDiferenciaLiquidacion
        {
            get { return DiferenciaLiquidacion.ToString("F02"); }
        }
    }
    
    /// <summary>
    /// Lista de objetos Configcco.
    /// </summary>
    [Serializable]
    public class GestionConfiguracionL : List<GestionConfiguracion>
    {
    }

    #endregion
}
