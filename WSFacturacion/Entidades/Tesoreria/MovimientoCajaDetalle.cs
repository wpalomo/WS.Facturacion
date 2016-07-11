using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJADETALLE: Item de la liquidacion (corresponde a una Denominacion)

    /// <summary>
    /// Clase para cada item de una liquidacion
    /// Tambien se usa para cada item de una apropiacion
    /// </summary>
    [Serializable]
    public class MovimientoCajaDetalle
    {
        private int cantidad;
        private Denominacion denominacion;
        private decimal montoTotal;
        public Moneda Moneda { get; set; }
        public decimal Cotizacion { get; set; }
        public Denominacion Denominacion 
        {
            get
            {
                return denominacion;
            }
            set
            {
                denominacion = value;
                montoTotal = Cantidad * Denominacion.ValorDenominacion;
            }
        }
        public int Cantidad 
        {
            get
            {
                return cantidad;
            }
            set
            {
                cantidad = value;
                montoTotal = Cantidad * Denominacion.ValorDenominacion;
            }
        }
        public decimal MontoTotal 
        {
            get
            {
                return Cantidad * Denominacion.ValorDenominacion;
            }
        }
        public string sMontoTotal
        {
            get
            {
                //TODO si tiene mas de 2 decimales, devolver el total de decimales
                return MontoTotal.ToString("F02");
            }
        }
    }
    
    /// <summary>
    /// Lista de objetos MovimientoCajaDetalle.
    /// </summary>
    /// 
    [Serializable]
    public class MovimientoCajaDetalleL : List<MovimientoCajaDetalle>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve agrupados por el valor de la denominacion
        /// </summary>
        /// <returns>MovimientoCajaDenominacionTotalL</returns>
        /// ***********************************************************************************************
        public MovimientoCajaDenominacionTotalL getPorDenominacion()
        {
            MovimientoCajaDenominacionTotalL oMovimientos = new MovimientoCajaDenominacionTotalL();

            foreach (MovimientoCajaDetalle item in this)
            {
                MovimientoCajaDenominacionTotal oMov = oMovimientos.FindValor(item.Denominacion.ValorDenominacion);
                if (oMov == null)
                {
                    oMov = new MovimientoCajaDenominacionTotal();
                    oMov.ValorDenominacion = item.Denominacion.ValorDenominacion;
                    oMov.Detalle = new MovimientoCajaDetalleL();

                    oMovimientos.Add(oMov);
                }

                oMov.Detalle.Add(item);
            }

            return oMovimientos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve agrupados por la moneda
        /// </summary>
        /// <returns>MovimientoCajaDetalleMonedaL</returns>
        /// ***********************************************************************************************
        public MovimientoCajaDetalleMonedaL getPorMoneda()
        {
            MovimientoCajaDetalleMonedaL oMovimientos = new MovimientoCajaDetalleMonedaL();

            foreach (MovimientoCajaDetalle item in this)
            {
                MovimientoCajaDetalleMoneda oMov = oMovimientos.FindValor(item.Moneda);
                if (oMov == null)
                {
                    oMov = new MovimientoCajaDetalleMoneda();
                    oMov.Moneda = item.Moneda;
                    oMov.Cotizacion = item.Cotizacion;
                    oMov.Detalle = new MovimientoCajaDetalleL();

                    oMovimientos.Add(oMov);
                }

                oMov.Detalle.Add(item);
            }

            return oMovimientos;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de monedas usadas (y las activas)
        /// Si se dio de baja una moneda la seguimos teniendo para liquidaciones viejas
        /// </summary>
        /// <returns>MonedaL</returns>
        /// ***********************************************************************************************
        public MonedaL getMonedas()
        {
            MonedaL oMonedas = new MonedaL();

            foreach (MovimientoCajaDetalle item in this)
            {
                Moneda oMon = oMonedas.FindMoneda(item.Moneda.Codigo);
                if (oMon == null)
                {
                    //TODO insertar ordenado
                    oMonedas.Add(item.Moneda);
                }
            }

            return oMonedas;
        }
    }

    #endregion
}
