using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region MEDIOPAGOCONFIGURACION: Clase para entidad de Configuracion de Medios de Pago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Forma de Pago Validas
    /// </summary>*********************************************************************************************

    [Serializable]

    public class MedioPagoConfiguracion
    {

       public MedioPagoConfiguracion()
        {

        }

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de una forma de medios de pagos
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public MedioPagoConfiguracion(FormaPago FormaDePago, double CostoMedioPago, double CostoReposicionMedioPago)
        {
            this.FormaDePago = FormaDePago;
            this.CostoMedioPago = CostoMedioPago;
            this.CostoReposicionMedioPago = CostoReposicionMedioPago;
        }

        //Tipo de Medio Tag Magnetica Chip
        public FormaPago FormaDePago { get; set; }

        // Retorna la combinacion de tipo y subtipo de forma de pago
        public string CodigoFormaPago
        {
            get { return FormaDePago.CodigoFormaPago; }
        }

        // Retorna la descripcion de la forma de pago
        public string DescripcionFormaPago
        {
            get { return FormaDePago.Descripcion; }
        }

        //costo de Medio de Pago
        public double CostoMedioPago { get; set; }

        //costo de Reposición de Medio de Pagos
        public double CostoReposicionMedioPago { get; set; }

        // Devolvemos el costo del medio de pago con el formato de moneda
        public string sValorCostoMedioPago
        {
            get{ return CostoMedioPago.ToString("C02"); }
        }

        // Devolvemos el costo de reposicion del medio de pago con el formato de moneda
        public string sValorCostoReposicionMedioPago
        {
            get { return CostoReposicionMedioPago.ToString("C02"); }
        }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos MedioPagoConfiguracion
    /// </summary>*********************************************************************************************
    public class MedioPagoConfiguracionL : List<MedioPagoConfiguracion>
    {
    }


    #endregion  

}

