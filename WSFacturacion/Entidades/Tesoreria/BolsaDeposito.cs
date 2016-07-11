using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region BOLSADEPOSITO: Clase para las Bolsas que componen un deposito
    /// <summary>
    /// Clase para las bolsas que componen un deposito
    /// </summary>
    [Serializable]
    public class BolsaDeposito
    {
        /// <summary>
        /// Obtiene o establece la estación donde se realizó el depósito de la bolsa
        /// </summary>
        public Estacion Estacion { get; set; }
        /// <summary>
        /// Obtiene o establece el número de depósito
        /// </summary>
        public int? NumeroDeposito { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha de la jornada
        /// </summary>
        public DateTime Jornada { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha en la que se realizó el movimiento
        /// </summary>
        public DateTime FechaMovimiento { get; set; }
        public int Turno { get; set; }
        public string Tipo { get; set; }
        public string TipoDescripcion { get; set; }
        //public string Movimiento { get; set; }
        //public string MovimientoDescripcion { get; set; }
        //public Moneda Moneda { get; set; }
        //public int? Parte { get; set; }
        public int? Bolsa { get; set; }
        //public int? NumeroMovimiento { get; set; }
        public decimal MontoEquivalente { get; set; }

        //Lo usamoes en el recuento
        public int? Remito { get; set; }
        public int NumeroApropiacion { get; set; }
        public bool Enviada { get; set; }   //Para bindear con el check
        public bool Verificada { get; set; }
        public bool Habilitada { get; set; }

        //Parte
        public int Parte { get; set; }

        //Reposicion de las bolsas
        public ReposicionPedida Reposicion { get; set; }

        //Usuario que hizo el movimiento
        public string Peajista { get; set; }

        //Si la bolsa esta depositada
        public bool BolsaDepositada { get; set; }
    }
    /// <summary>
    /// Lista de objetos BolsaDeposito.
    /// </summary>
    /// 
    [Serializable]
    public class BolsaDepositoL : List<BolsaDeposito>
    {
    }
    #endregion
}
