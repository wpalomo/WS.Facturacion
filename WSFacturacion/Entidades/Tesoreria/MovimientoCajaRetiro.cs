using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;


namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJARTIRO: Clase para los retiros
        /// <summary>
        /// Clase para los retiros
        /// </summary>
    [Serializable]
    public class MovimientoCajaRetiro:MovimientoCaja
    {
        public MovimientoCajaRetiro()
        {
            base.Tipo = MovimientoCaja.enmTipo.enmRetiro;
        }

        public Moneda Moneda;
        //Monto en la moneda ingresada
        public decimal MontoMoneda;
        public string sMontoMoneda
        {
            get
            {
                //TODO si tiene mas de 2 decimales, devolver el total de decimales
                return MontoMoneda.ToString("F02");
            }
        }


        public string MonedaSimbolo
        {
            get
            {
                return Moneda.Simbolo;
            }
        }
        public int? Bolsa { get; set; }
        public int? Precinto { get; set; }
        public bool Confirmado { get; set; }
        public bool PuedeConfirmar { get; set; }
        public string ComentarioEliminacion { get; set; }

    }

    /// <summary>
    /// Lista de objetos MovimientoCajaRetiro.
    /// </summary>
    /// 
    [Serializable]
    public class MovimientoCajaRetiroL : List<MovimientoCajaRetiro>
    {
    }
    #endregion
}
