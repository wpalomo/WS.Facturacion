using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Telectronica.Peaje
{
    #region TARIFAAUTORIZACIONPASO: Clase para entidad de las Tarifas de Autorizacion de Paso

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TarifaAutorizacionPaso
    /// </summary>*********************************************************************************************

    [Serializable]

    public class TarifaAutorizacionPaso
    {

        // Constructor vacio
        public TarifaAutorizacionPaso()
        {
        }


        // En el constructor asigno los valores a la clase
        public TarifaAutorizacionPaso(Estacion estacionOrigen, ViaSentidoCirculacion sentidoOrigen, CategoriaManual categoria, 
                                      Estacion estacionDestino, ViaSentidoCirculacion sentidoDestino, TarifaAutorizacionPasoFormaDescuento formaDescuento,
                                      TarifaDiferenciada tipoTarifa, Int16 minutosVigencia,byte numeroPasada)
        {
            this.EstacionOrigen = estacionOrigen;
            this.SentidoOrigen = sentidoOrigen;
            this.Categoria = categoria;
            this.EstacionDestino = estacionDestino;
            this.SentidoDestino = sentidoDestino;
            this.FormaDescuento = formaDescuento;
            this.TipoTarifa = tipoTarifa;
            this.MinutosVigencia = minutosVigencia;
            this.NumeroPasada = numeroPasada;

        }



        // Estacion origen
        public Estacion EstacionOrigen { get; set; }

        // Sentido origen
        public ViaSentidoCirculacion SentidoOrigen { get; set; }

        // Categoria
        public CategoriaManual Categoria { get; set; }

        // Estacion destino
        public Estacion EstacionDestino { get; set; }

        // Sentido destino
        public ViaSentidoCirculacion SentidoDestino { get; set; }

        // Forma de descuento
        public TarifaAutorizacionPasoFormaDescuento FormaDescuento { get; set; }

        // Tipo de tarifa aplicada
        public TarifaDiferenciada TipoTarifa { get; set; }

        // Minutos de vigencia
        public Int16 MinutosVigencia { get; set; }

        // Numero de pasada
        public byte NumeroPasada { get; set; }

        public string MinutosVigenciaString 
        { 
            get 
            {
                return  getMinutosAFormatoHora(MinutosVigencia);
            }
            
        }

        /// ***********************************************************************************************
        /// <summary>
        // Metodo que retorna una cantidad de minutos en formato hora 
        /// </summary>
        /// <param name="minutos">int - Cantidad de minutos que se desean transformar al formato "hh:mm"</param>
        /// <returns>El formato "hh:mm" de una cantidad de minutos</returns>
        /// ***********************************************************************************************
        public string getMinutosAFormatoHora(int minutos)
        {
            string sRet;
            int nHoras;


            nHoras = minutos / 60;
            sRet = nHoras.ToString("00") + ":" + (minutos - (nHoras * 60)).ToString("00");


            return sRet;

        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TarifaAutorizacionPaso
    /// </summary>*********************************************************************************************
    public class TarifaAutorizacionPasoL : List<TarifaAutorizacionPaso>
    {
    }

    #endregion
}
