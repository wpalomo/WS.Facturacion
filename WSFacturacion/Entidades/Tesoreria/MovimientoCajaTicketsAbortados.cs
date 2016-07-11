using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;


namespace Telectronica.Tesoreria
{
    #region MOVIMIENTOCAJATICKETSABORTADOS: Tickets Abortados rendidos
        /// <summary>
        /// Clase para tickets abortados rendidos
        /// </summary>
    [Serializable]
    public class MovimientoCajaTicketsAbortados
    {
        public int Cantidad { get; set; }
        public CategoriaManual Categoria { get; set; }
        
    }
    /// <summary>
    /// Lista de objetos MovimientoCajaDetalle.
    /// </summary>
    /// 
    [Serializable]
    public class MovimientoCajaTicketsAbortadosL : List<MovimientoCajaTicketsAbortados>
    {

    }
    #endregion
}
