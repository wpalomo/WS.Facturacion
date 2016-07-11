using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Entidad para un Abandono de Troco
    /// </summary>
    [Serializable]
    public class AbandonoDeTroco
    {
        /// <summary>
        /// Obtiene o establece el código único e identificatorio
        /// </summary>
        public int Identity { get; set; }

        /// <summary>
        /// Obtiene o establece la estación donde se produjo el Abandono de Troco
        /// </summary>
        public Estacion Estacion { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de registracion del abandono del cambio
        /// </summary>
        public DateTime FechaRegAbandono { get; set; }

        /// <summary>
        /// Obtiene o establece el monto abandonado registrado por el usuario
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// Obtiene o establece la vía donde se produjo el abandono del cambio
        /// </summary>
        public Via Via { get; set; }

        /// <summary>
        /// Obtiene o establece el supervisor que aprueba el abandono de troco
        /// </summary>
        public Usuario Supervisor { get; set; }

        /// <summary>
        /// Obtiene o establece el peajista que recibió el abandono de cambio
        /// </summary>
        public Usuario Peajista { get; set; }

        /// <summary>
        /// Obtiene o establece el turno abierto en la vía
        /// </summary>
        public TurnoTrabajo Turno { get; set; }

        /// <summary>
        /// Obtiene o establece el sentido de la vía donde se produjo el abandono del cambio
        /// </summary>
        public ViaSentidoCirculacion Sentido { get; set; }
    }

    /// <summary>
    /// Clase lista de AbandonoDeTroco
    /// </summary>
    [Serializable]
    public class AbandonoDeTrocoL : List<AbandonoDeTroco>
    {
    }
}
