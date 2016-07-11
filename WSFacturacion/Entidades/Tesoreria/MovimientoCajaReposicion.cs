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
    public class MovimientoCajaReposicion : MovimientoCaja
    {
        /// <summary>
        /// Obtiene o establece el còdigo de pago
        /// </summary>
        public int IdPago { get; set; }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public MovimientoCajaReposicion()
        {
            base.Tipo = MovimientoCaja.enmTipo.enmReposicion;
        }

        /// <summary>
        /// Obtiene o establece la moneda del movimiento
        /// </summary>
        public Moneda Moneda;

        /// <summary>
        /// Monto en la moneda ingresada
        /// </summary>
        public decimal MontoMoneda;

        /// <summary>
        /// Devuelve la moneda con formato en string
        /// </summary>
        public string sMontoMoneda
        {
            get
            {
                //TODO si tiene mas de 2 decimales, devolver el total de decimales
                return MontoMoneda.ToString("F02");
            }
        }

        /// <summary>
        /// Devuelve el símbolo de la moneda
        /// </summary>
        public string MonedaSimbolo
        {
            get
            {
                return Moneda.Simbolo;
            }
        }

        /// <summary>
        /// Obtiene o establece la bolsa
        /// </summary>
        public int? Bolsa { get; set; }

        /// <summary>
        /// Obtiene o establece el malote
        /// </summary>
        public int? Precinto { get; set; }

        /// <summary>
        /// Obtiene o establece el típo de la reposición (Financiera o Económica)
        /// </summary>
        public TipoDeReposicion TipoDeReposicion { get; set; }

        /// <summary>
        /// Obtiene o establece el sentido de la estacion
        /// </summary>
        public ViaSentidoCirculacion Sentido { get; set; }

        /// <summary>
        /// Obtiene o establece el número de recibo generado al grabar la reposición
        /// </summary>
        public int? Recibo { get; set; }
        
        //Fecha GTV no usamos mas, sino que vamos a tomar en que deposito va
        //Grabar la fecha de ingreso en ese campo
    }

    /// <summary>
    /// Lista de objetos MovimientoCajaRetiro.
    /// </summary>
    [Serializable]
    public class MovimientoCajaReposicionL : List<MovimientoCajaReposicion>
    {
    }

    #endregion
}
