using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJALIQUIDACION: Cierre de caja (Liquidacion)

    /// <summary>
    /// Clase para los movimientos de cierre de caja (Liquidacion)
    /// </summary>
    [Serializable]
    public class MovimientoCajaLiquidacion:MovimientoCaja
    {
        public MovimientoCajaLiquidacion()
        {
            base.Tipo = MovimientoCaja.enmTipo.enmCierre;
        }
        /// <summary>
        /// Obtiene o establece el monto total de los Abandonos de Troco que se produjeron
        /// </summary>
        public decimal AbandonoDeTroco { get; set; }
        /// <summary>
        /// Obtiene o establece el monto en efectivo
        /// </summary>
        public decimal MontoEfectivo { get; set; }
        public decimal MontoCheque { get; set; }
        public decimal MontoVales { get; set; }
        public decimal MontoVisa { get; set; }
        public decimal MontoVisaIntegrado { get; set; }
        public decimal MontoTicketManuales { get; set; }

        public decimal MontoChequePlaza { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para el efectivo
        /// </summary>
        public int? Bolsa { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para los cheques
        /// </summary>
        public int? BolsaCheques { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para los abandono de troco
        /// </summary>
        public int? BolsaAbTroco { get; set; }
        public int? Precinto { get; set; }
        public string Observacion { get; set; }
        private MovimientoCajaDetalleL detalle;
        private MovimientoCajaDetalleMonedaL detallePorMoneda;
        private MovimientoCajaDenominacionTotalL detallePorDenominacion;
        private MonedaL monedas;
        private MovimientoCajaDetalleCuponL detalleCupon;
        private MovimientoCajaDetalleTipoCuponL detallePorTipoCupon;
        private CategoriaManualL categorias;
        public BloqueL Bloques { get; set; }
        public MovimientoCajaDetalleL Detalle 
        {
            get
            {
                return detalle;
            }
            set
            {
                detalle = value;
                detallePorMoneda = detalle.getPorMoneda();
                detallePorDenominacion = detalle.getPorDenominacion();
                monedas = detalle.getMonedas();
            }
        }
        /// <summary>
        /// Agrupado por monedas
        /// </summary>
        public MovimientoCajaDetalleMonedaL DetallePorMoneda
        {
            get
            {
                return detallePorMoneda;
            }
        }
        /// <summary>
        /// Agrupado por denominacion
        /// </summary>
        public MovimientoCajaDenominacionTotalL DetallePorDenominacion
        {
            get
            {
                return detallePorDenominacion;
            }
        }
        /// <summary>
        /// Lista de Monedas
        /// </summary>
        public MonedaL Monedas
        {
            get
            {
                return monedas;
            }
        }
        public MovimientoCajaDetalleCuponL DetalleCupon
        {
            get
            {
                return detalleCupon;
            }
            set
            {
                detalleCupon = value;
                detallePorTipoCupon = detalleCupon.getPorTipoCupon();
                categorias = detalleCupon.getCategorias();
            }
        }
        /// <summary>
        /// Agrupado por monedas
        /// </summary>
        public MovimientoCajaDetalleTipoCuponL DetallePorTipoCupon
        {
            get
            {
                return detallePorTipoCupon;
            }
        }
        /// <summary>
        /// Lista de Categorias
        /// </summary>
        public CategoriaManualL Categorias
        {
            get
            {
                return categorias;
            }
        }
        public string sMontoEfectivo
        {
            get
            {
                //TODO si tiene mas de 2 decimales, devolver el total de decimales
                return MontoEfectivo.ToString("F02");
            }
        }
        public string sMontoCheque
        {
            get
            {
                //TODO si tiene mas de 2 decimales, devolver el total de decimales
                return MontoCheque.ToString("F02");
            }
        }
        public bool EsParteSpervisor { get; set; }
        /// <summary>
        /// true si tuvo a cargo la estacion
        /// </summary>
        public InfoEventoL Violaciones { get; set; }
        public MovimientoCajaRetiroL Retiros { get; set; }
        /// <summary>
        /// Cantidad Tickets Abortados por categoria
        /// </summary>
        public MovimientoCajaTicketsAbortadosL TicketsAbortados { get; set; }
        public int CantidadTicketAbortados { get; set; }
        /// <summary>
        /// Indica el monto facturado en neto y provisorio, ya que no es el valor liquidado.
        /// </summary>
        public decimal MontoFacturadoNetoProv { get; set; }
        /// <summary>
        /// Indica si ya se mostró la advertencia que indica el exceso entre lo liquidado y lo facturado.
        /// </summary>
        public string MostroDiferencia { get; set; }
        public int? NumeroApropiacionChequeCabecera { get; set; }
        public int? NumeroApropiacionAbTrocoCabecera { get; set; }

    }

    /// <summary>
    /// Lista de objetos MovimientoCajaLiquidacion.
    /// </summary>
    /// 
    [Serializable]
    public class MovimientoCajaLiquidacionL : List<MovimientoCajaLiquidacion>
    {
    }

    #endregion
}
