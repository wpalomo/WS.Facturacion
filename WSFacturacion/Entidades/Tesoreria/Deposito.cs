using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region DEPOSITO: Clase para los depositos
    /// <summary>
    /// Clase para los depositos
    /// </summary>
    [Serializable]
    public class Deposito
    {
        private string tipo = "";
        private string tipoDescripcion = "";

        /// <summary>
        /// Obtiene o establece la estación a la cual pertenece el movimiento
        /// </summary>
        public Estacion Estacion { get; set; }
        /// <summary>
        /// Obtiene o establece el identificatorio del depósito
        /// </summary>
        public int Numero { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha en la que se registro el movimiento
        /// </summary>
        public DateTime Fecha { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha de la jornada a la que pertenece el depósito
        /// </summary>
        public DateTime FechaJornada { get; set; }
        /// <summary>
        /// Obtiene o establece el usuario Tesorero
        /// </summary>
        public Usuario Tesorero { get; set; }
        public string sRemito { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para el efectivo
        /// </summary>
        public string Funda { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para los cheques
        /// </summary>
        public string FundaCheque { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para la reposición financiera
        /// </summary>
        public string FundaRepFinanciera { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para la reposición económica
        /// </summary>
        public string FundaRepEconomica { get; set; }
        /// <summary>
        /// Obtiene o establece la bolsa para los abandono de troco
        /// </summary>
        public string FundaAbTroco { get; set; }
        /// <summary>
        /// Obtiene o establece el monto del depósito
        /// </summary>
        public decimal Monto { get; set; }
        public DateTime? Llegada { get; set; }
        public DateTime? Salida { get; set; }
        

        public bool EstaRecontado { get; set; }
        public bool Verificado { get; set; }
        public bool? jornadaCerrada { get; set; }
        /// <summary>
        /// Bolsas del deposito
        /// </summary>
        public BolsaDepositoL Bolsas { get; set; }

        public string Tipo
        {
            get
            {
                return tipo;
            }
            set
            {
                tipo = value;
                if (tipo == "E")
                    tipoDescripcion = "Efectivo";
                else if (tipo == "C")
                    tipoDescripcion = "Cheque";

            }

        }
        public string TipoDescripcion
        {
            get
            {
                return tipoDescripcion;
            }
        }

    }
    /// <summary>
    /// Lista de objetos Deposito.
    /// </summary>
    /// 
    [Serializable]
    public class DepositoL : List<Deposito>
    {
    }
    #endregion
}
