using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;


namespace Telectronica.Tesoreria
{
    #region VALESPREPAGOS: VALES PREPAGOS RENDIDOS
        /// <summary>
        /// Clase para vales prepagos rendidos
        /// </summary>
    [Serializable]
    public class ValesPrepagos
    {
        public int Cantidad { get; set; }
        public decimal Monto { get; set; }
        public CategoriaManual Categoria { get; set; }
        public string TipoTicket { get; set; }
        public int Descripcion { get; set; }
        public string NumeroVale { get; set; }
        public int NumSer { get; set; }
        public int NumCl { get; set; }
        public byte NumEstaci { get; set; }
        public string ExisteTicket { get; set; }
        public byte subfp { get; set; }
        public bool Manual { get; set; }
        public bool Anulado { get; set; }
        public bool Procesado { get; set; }
        public int Numer { get; set; }
        public Parte Parte { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Serializable]
    public class ValesPrepagosL : List<ValesPrepagos>
    {

    }
    #endregion
}
