using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    [Serializable]
    public class Apropiacion:MovimientoCaja
    {
        public Apropiacion()
        {
            base.Tipo = MovimientoCaja.enmTipo.enmCierre;  //
        }

        // Por ahora lo vemos desde la base
        public Estacion Estacion { get; set; }
                
        //Usuario que realiza la apropiacion (tesorero)
        public Usuario Usuario { get; set; }
        
        // Numero de bolsa en la que junta los movimientos de caja
        public int? Bolsa { get; set; }

        // Numero de apropiacion generada (identity generado que usa para grabar el detalle)
        public int NumeroApropiacion { get; set; }

        // Monto total de la suma de los movimientos
        public decimal Total { get; set; }

        // Fecha en que realiza la apropiacion
        public DateTime Fecha { get; set; }

        // Jornada de las bolsas apropiadas
        public DateTime Jornada { get; set; }

        // Turno de las bolsas apropiadas
        public int Turno{ get; set; }

        // Turno de las bolsas apropiadas
        public TurnoTrabajo TurnoTrabajo { get; set; }

        // Indica si la apropiacion fue depositada
        public string Depositado { get; set; }

        // Indica si la apropiacion fue recontada
        public string Recontado { get; set; }

        // Lista de las apropiaciones de bolsa
        public BolsaApropiacionL apropiacionDetalleL { get; set; }

        //numero de Consignacion Interna
        public int ConsignacionInterna { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de apropiación
        /// </summary>
        public string TipoApropiacion { get; set; }

        private MovimientoCajaDetalleL detalle;
        private MovimientoCajaDetalleMonedaL detallePorMoneda;
        private MovimientoCajaDenominacionTotalL detallePorDenominacion;
        private MonedaL monedas;

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

        //Agrupado por monedas
        public MovimientoCajaDetalleMonedaL DetallePorMoneda
        {
            get
            {
                return detallePorMoneda;
            }
        }

        //Agrupado por denominacion
        public MovimientoCajaDenominacionTotalL DetallePorDenominacion
        {
            get
            {
                return detallePorDenominacion;
            }
        }

        //Lista de Monedas
        public MonedaL Monedas
        {
            get
            {
                return monedas;
            }
        }
    }

    [Serializable]
    public class ApropiacionL : List<Apropiacion>
    {
    }

}
