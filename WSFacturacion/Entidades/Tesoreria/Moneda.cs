using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Tesoreria
{
 
    #region MONEDA: Clase para entidad de Moneda.

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Moneda
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Moneda
    {

        public Moneda(Int16 Codigo, string Descripcion,
                      string Simbolo, string esMonedaReferencia)
        {
            this.Codigo = Codigo;
            this.Simbolo = Simbolo;
            this.Desc_Moneda = Descripcion;
            this.MonedaReferencia = esMonedaReferencia;
        }

        public Moneda()
        {

        }
        
        // Descripción de la Moneda
        public string Desc_Moneda { get; set; }

        // Simbolo de la Moneda
        public string Simbolo { get; set; }

        // Código de la Moneda
        public Int16 Codigo { get; set; }

        public string sCodigo
        {
            get
            {
                return Codigo.ToString();
            }
        }

        // Moneda Compatible con la Via
        public string MonedaReferencia { get; set; }

        public bool esMonedaReferencia
        {
            get { return MonedaReferencia == "S"; }
            set { MonedaReferencia = value ? "S" : "N"; }
        }

    }


   [Serializable]

   /// *********************************************************************************************<summary>
   /// Lista de objetos Moneda
   /// </summary>*********************************************************************************************
   public class MonedaL : List<Moneda>
   {

       /// ***********************************************************************************************
       /// <summary>
       /// Devuelve una determinada moneda localizada mediante los parametros
       /// </summary>
       /// <param name="moneda">int16 - Moneda que se desea localizar</param>
       /// <returns>objeto Moneda buscada</returns>
       /// ***********************************************************************************************
       public Moneda FindMoneda(Int16 moneda)
       {
           Moneda oMoneda = null;

           foreach (Moneda oMon in this)
           {
               if (moneda == oMon.Codigo)
               {
                   oMoneda = oMon ;
                   break;
               }
           }

           return oMoneda;
       }
   }
   
     #endregion
}
