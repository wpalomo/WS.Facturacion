using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;


namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJA: Clase base para los movimientos de caja (corresponde a la tabla mocaja)
        /// <summary>
        /// Clase base para los movimientos de caja
        /// </summary>
    [Serializable]
    public class MovimientoCaja
    {
        public enum enmTipo
        {
            enmApertura,
            enmCierre,
            enmRetiro,
            enmReposicion,
            enmLiqAdicional
        }

        /// <summary>
        /// Obtiene o establece la estación
        /// </summary>
        public Estacion Estacion { get; set; }
        /// <summary>
        /// Obtiene o establece el número de movimiento de la Caja
        /// </summary>
        public int NumeroMovimiento { get; set; }
        public enmTipo Tipo { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha del movimiento
        /// </summary>
        public DateTime FechaHoraIngreso { get; set; }
        /// <summary>
        /// Obtiene o establece el parte asociado al movimiento
        /// </summary>
        public Parte Parte { get; set; }
        /// <summary>
        /// Obtiene o establece el peajista asociado al movimiento
        /// </summary>
        public Usuario Peajista { get; set; }
        /// <summary>
        /// Obtiene o establece el liquidador
        /// </summary>
        public Usuario Liquidador { get; set; }
        /// <summary>
        /// Monto total equivalente
        /// </summary>
        public Decimal MontoTotal { get; set; }
        /// <summary>
        /// Numero de la apropiacion (cabecera). Sirve para que al anular el movimiento sea mas facil eliminar la apropiacion
        /// </summary>
        public int? NumeroApropiacionCabecera { get; set; }

        public int ParteNumero
        {
            get
            {
                return Parte.Numero;
            }
        }

        public int ParteTurno
        {
            get
            {
                return Parte.Turno;
            }
        }

        public string JornadaString
        {
            get
            {
                return Parte.JornadaString;
            }
        }
        
        public string HoraIngreso
        {
            get
            {
                return FechaHoraIngreso.ToString("HH:mm");
            }
        }
    }

    /// <summary>
    /// Lista de objetos MovimientoCaja.
    /// </summary>
    /// 
    [Serializable]
    public class MovimientoCajaL : List<MovimientoCaja>
    {
    }
    #endregion
}
