using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TARIFA: Clase para entidad de TARIFAS de las categorias

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Tarifa
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Tarifa
    {
        // Constructor Vacio
        public Tarifa()
        {
            // Inicializamos el puntero de lista de tarifas
            this.TarifasDetalle = new TarifaDetalleL();
        }


        // Creacion de la clase con todos los atributos
        public Tarifa(int identity, Estacion estacion, DateTime fechaInicialVigencia, DateTime fechaFinalVigencia, TarifaDetalleL tarifasDetalle)
        {
            this.Identity = identity;
            this.Estacion = estacion;
            this.FechaInicialVigencia = fechaInicialVigencia;
            this.FechaFinalVigencia = fechaFinalVigencia;

            // Inicializamos el puntero de lista de tarifas
            this.TarifasDetalle = new TarifaDetalleL();

            if (tarifasDetalle != null)
            {
                this.TarifasDetalle = tarifasDetalle;
            }
        }


        // Identity del registro
        public int Identity { get; set; }

        // Estacion donde tiene vigencia la tarifa
        public Estacion Estacion { get; set; }

        // Valor del numero de estacion (para la grilla)
        public int NumeroEstacion
        {
            get
            {
                return Estacion.Numero;
            }
        }

        // Registros de las tarifas del presente cambio de tarifa
        public TarifaDetalleL TarifasDetalle { get; set; }

        // Fecha de vigencia inicial de la tarifa
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

        // Monto minimo posible de una tarifa
        public static string MontoMinimoTarifa
        {
            get { return "0.00"; }
        }

        // Monto maximo posible de una tarifa
        public static string MontoMaximoTarifa
        {
            get { return "1000000.00"; }
        }

        public double PorcentajeIva { get; set; }
    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Tarifa
    /// </summary>*********************************************************************************************
    public class TarifaL : List<Tarifa>
    {
    }


    #endregion
}
