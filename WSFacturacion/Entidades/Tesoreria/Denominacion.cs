using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Tesoreria
   
    #region Denominacion: Clase para entidad de Moneda.
        /// <summary>
        /// Estructura de una entidad Moneda
        /// </summary
{

    [Serializable]

    public class Denominacion
    {

        public Denominacion(Moneda Moneda, short CodDenominacion,
                            string DescDenmominacion, decimal ValorDenominacion)
        {
            this.Moneda = Moneda;
            this.DescMoneda = Moneda.Desc_Moneda;
            this.CodDenominacion = CodDenominacion;
            this.DescDenominacion = DescDenmominacion;
            this.ValorDenominacion = ValorDenominacion;
        }

        public Denominacion()
        {

        }


        public Denominacion(Moneda Moneda, short CodDenominacion,
                            string DescDenmominacion, decimal ValorDenominacion,
                            string BilleteMoneda)
        {
            this.Moneda = Moneda;
            this.DescMoneda = Moneda.Desc_Moneda;
            this.CodDenominacion = CodDenominacion;
            this.DescDenominacion = DescDenmominacion;
            this.ValorDenominacion = ValorDenominacion;
            this.BilleteMoneda = BilleteMoneda;
        }

        
        // Estructura de la moneda 
        public Moneda Moneda { get; set; }

        // Descrición de la Moneda
        public string DescMoneda { get; set; }

        // Simbolo de la Moneda
        public string SimboloMoneda
        {
            get { return Moneda.Simbolo; }
        }

        // Codigo de la denominacion
        public short CodDenominacion { get; set; }

        // Descripcion de la denominacion
        public string DescDenominacion { get; set; }

        // Valor de la denominacion 
        public decimal ValorDenominacion { get; set; }

        public string sValorDenominacion
        {
            get
            {
                //TODO si tiene mas de 2 decimales, devolver el total de decimales
                return ValorDenominacion.ToString("F02");
            }
        }

        //es Billete o Moneda
        public string BilleteMoneda { get; set; }

        public string esBilleteMoneda
        {
            get
            {
                string Named = "Moneda";
                if (BilleteMoneda != null && BilleteMoneda.Trim() == "S")
                    Named = "Billete";
                return Named;
            }
        }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Denominacion.
    /// </summary>
    /// 
    public class DenominacionL : List<Denominacion>
    {
        public int Cantidad { get; set; }
    }

    #endregion
}
