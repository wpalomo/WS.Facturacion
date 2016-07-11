using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region TARIFADIFERENCIADA: Clase para entidad de las Tarifas Diferenciadas definidas

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TarifaDiferenciada
    /// </summary>*********************************************************************************************

    [Serializable]
    
    public class TarifaDiferenciada

    {

        public TarifaDiferenciada()
        {
        }

        public TarifaDiferenciada(int? CodigoTarifa,string Descripcion)
        {
            this.CodigoTarifa = CodigoTarifa;
            this.Descripcion = Descripcion;
         }

        public TarifaDiferenciada(int? CodigoTarifa, string Descripcion, float Porcentaje)
        {
            this.CodigoTarifa = CodigoTarifa;
            this.Descripcion = Descripcion;
            this.Porcentaje = Porcentaje;
        }



        // Codigo de tarifa diferenciada
        public int? CodigoTarifa { get; set; }

        // Descripcion de la tarifa diferenciada
        public string Descripcion { get; set; }

        // Porcentaje de pago de la tarifa
        public float Porcentaje { get; set; }

        // Porcentaje de pago de la tarifa con el signo de porcentaje
        public string sPorcentaje 
        {
            get { return Porcentaje.ToString() + "%" ; }
        }

        // Sobrecargamos el metodo para visualizar en la grilla la descripcion
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]
    
    /// *********************************************************************************************<summary>
    /// Lista de objetos TarifaDiferenciada
    /// </summary>*********************************************************************************************
    public class TarifaDiferenciadaL : List<TarifaDiferenciada>
    {
    }


    #endregion
}
