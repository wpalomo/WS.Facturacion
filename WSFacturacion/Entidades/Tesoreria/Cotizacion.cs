using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region COTIZACION: Clase para entidad de COTIZACIONES de las monedas

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Cotizacion
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Cotizacion
    {
        // Constructor Vacio
        public Cotizacion()
        {
            // Inicializamos el puntero de lista de Cotizaciones
            this.Cotizaciones = new CotizacionDetalleL();
        }


        // En el Constructor asignamos los valores a la clase
        public Cotizacion(int identity, Estacion estacionOrigen, Moneda moneda, CotizacionDetalleL cotizacionesDetalle,
                          DateTime fechaInicialVigencia, DateTime fechaFinalVigencia,DateTime fechaCarga)
        {
            this.Identity = identity;
            this.EstacionOrigen = estacionOrigen;
            this.MonedaReferencia = moneda;
            this.FechaInicialVigencia = fechaInicialVigencia;
            this.FechaFinalVigencia = fechaFinalVigencia;
            this.FechaCarga = fechaCarga;


            // Inicializamos o asignamos el puntero de lista de Cotizaciones
            if (cotizacionesDetalle != null)
            {
                this.Cotizaciones = cotizacionesDetalle;
            }
            else
            {
                this.Cotizaciones = new CotizacionDetalleL();
            }

        }


        // Identity del registro
        public int Identity { get; set; }

        // Estacion en la cual se cargo la cotizacion
        public Estacion EstacionOrigen { get; set; }

        public int EstacionOrigenNumero
        {
            get 
            { 
                return this.EstacionOrigen.Numero; 
            }
        }

        // Moneda de referencia
        public Moneda MonedaReferencia { get; set; }

        // Lista con valores de las cotizaciones de las monedas
        public CotizacionDetalleL Cotizaciones { get; set; }

        // Fecha inicial de vigencia
        public DateTime? FechaInicialVigencia { get; set; }

        // Fecha inicial de vigencia en formato string "dd/mm/yyyy hh:mm"
        public string FechaInicialVigenciaString
        {
            get
            {
                DateTime dAux;
                string sRet;


                if (FechaInicialVigencia == null)
                {
                    sRet = "";
                }
                else
                {
                    dAux = FechaInicialVigencia.Value;
                    sRet = dAux.ToString("dd/MM/yyyy HH:mm");
                }

                return sRet;
            }

        }

        // Fecha de vigencia final de la tarifa
        public DateTime? FechaFinalVigencia { get; set; }

        // Fecha final de vigencia en formato string "dd/mm/yyyy hh:mm"
        public string FechaFinalVigenciaString
        {
            get
            {
                DateTime dAux;
                string sRet;


                if (FechaFinalVigencia == null)
                {
                    sRet = "";
                }
                else
                {
                    dAux = FechaFinalVigencia.Value;
                    sRet = dAux.ToString("dd/MM/yyyy HH:mm");
                }

                return sRet;
            }
        }

        // Fecha de carga de la cotizacion
        public DateTime FechaCarga{ get; set; }

        // Fecha de carga de la cotizacion en formato string "dd/mm/yyyy hh:mm"
        public string FechaCargaString
        {
            get
            {
                return FechaCarga.ToString("dd/MM/yyyy HH:mm");
            }

        }

        // Monto minimo posible de una cotizacion
        public static string MontoMinimoCotizacion
        {
            get { return "0"; }
        }

        // Monto maximo posible de una Cotizacion
        public static string MontoMaximoCotizacion
        {
            get { return "1000000.00"; }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Cotizacion
    /// </summary>*********************************************************************************************
    public class CotizacionL : List<Cotizacion>
    {
    }


    #endregion
}
