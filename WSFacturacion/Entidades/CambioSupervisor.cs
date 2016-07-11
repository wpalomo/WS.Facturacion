using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Tesoreria;

namespace Telectronica.Peaje
{
    #region CAMBIOSUPERVISOR: Clase para entidad de los Cambios de Supervisor


    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Categoria (categorias manuales)
    /// </summary>*********************************************************************************************

    [Serializable]

    public class CambioSupervisor
    {
        public Estacion Estacion { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFinal { get; set; }

        public Usuario Supervisor { get; set; }

        public int Identity { get; set; }

        public Parte Parte { get; set; }

        public bool NuevoParte { get; set; }

    }

    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos CambioSupervisor
    /// </summary>*********************************************************************************************
    public class CambioSupervisorL : List<CambioSupervisor>
    {
    }


    #endregion
}
