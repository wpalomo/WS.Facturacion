using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje

     #region MONEDA: Clase para entidad de Moneda.
        /// <summary>
        /// Estructura de una entidad Moneda
        /// </summary>
       {

    public class Moneda
    {

        public Moneda(string Moneda,
                                  string Simbolo)
        {
            this.Simbolo = Simbolo;
            this.Desc_Moneda = Moneda;
        }

        public Moneda()
        {

        }

        public string Desc_Moneda { get; set; }

        public string Simbolo { get; set; }

        public Int16 Codigo { get; set; }


        /// <summary>
        /// Lista de objetos Moneda.
        /// </summary>
        /// 

    }
        public class MonedaL : List<Moneda>
        {

        }        

     #endregion
}
