using System;
using Telectronica.Peaje;
using System.Collections.Generic;

namespace Telectronica.Tesoreria
{
    /// <summary>
    /// Clase entidad para los pagos de reposiciones
    /// </summary>
    [Serializable]
    public class ReposicionPedida
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ReposicionPedida()
        {
        }
        
        #endregion

        /// <summary>
        /// Obtiene o establece el código único e identificatorio
        /// </summary>
        public int Identity { get; set; }

        /// <summary>
        /// Obtiene o establece la estación
        /// </summary>
        public Estacion Estacion { get; set; }

        /// <summary>
        /// Obtiene o establece el parte relacionado con la reposición
        /// </summary>
        public Parte Parte { get; set; }

        /// <summary>
        /// Obtiene o establece el monto a pagar
        /// </summary>
        public decimal Monto { get; set; }


        /// <summary>
        /// Numero de Bolsa de la reposicion
        /// </summary>
        public string bolsa { get; set; }

        /// <summary>
        /// Obtiene o establece el estádo de la reposicion (Pendiente, Liquidado, Anulada)
        /// </summary>
        public EstadoReposicion Estado { get; set; }

        /// <summary>
        /// Obtiene o establece el sentido de la estacion
        /// </summary>
        public ViaSentidoCirculacion Sentido { get; set; }

        /// <summary>
        /// Obtiene o establece un valor booleano que indica si la reposición fue o no pagada
        /// </summary>
        public bool Pagado { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de ingreso del movimiento
        /// </summary>
        public DateTime FechaIngreso { get; set; }

        /// <summary>
        /// Obtiene o establece el pago referido a la reposición
        /// </summary>
        public MovimientoCajaReposicion PagoReposicion { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de la Reposición
        /// </summary>
        public TipoDeReposicion TipoDeReposicion { get; set; }


        public string Peajista { get; set; }
    }

    [Serializable]
    public class ReposicionPedidaL : List<ReposicionPedida>
    {
    }
}
