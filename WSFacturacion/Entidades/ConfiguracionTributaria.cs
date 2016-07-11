using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CFGTRIB: Clase para entidad de Configcco en las que se divide la concesión.
    /// <summary>
    /// Estructura de una entidad Cfgtrib
    /// </summary>

    [Serializable]

    public class ConfiguracionTributaria
    {
        public ConfiguracionTributaria()
        {

        }

        public int IdReplicacion { get; set; }

        //public string Idioma { get; set; }

        public double PorcentajeIva { get; set; }

        //public int MonedaVia { get; set; }

        public string Nombre { get; set; }

        public string RazonSocial { get; set; }

        public string RUC { get; set; }

        public double PorcentajeRetencionServicios { get; set; }

        public double PorcentajeRetencionBienes { get; set; }

        // Direccion de la Casa Matriz
        public string Direccion { get; set; }

        public string ContribuyenteEspecial;

        public DateTime? FechaContribuyenteEspecial;

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos CFGTRIB.
    /// </summary>
    public class ConfiguracionTributariaL : List<ConfiguracionTributaria>
    {
    }

    #endregion
}
