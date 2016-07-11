using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TARIFADETALLE: Clase para entidad de DETALLE DE TARIFAS de las categorias

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TarifaDetalle
    /// </summary>*********************************************************************************************

    [Serializable]

    public class TarifaDetalle
    {
        // Constructor Vacio
        public TarifaDetalle()
        {
        }


        // Creacion de la clase con todos los atributos
        public TarifaDetalle(CategoriaManual categoria, TarifaDiferenciada tipoTarifa, TipoDiaHora tipoDia, bool habilitado, decimal valorTarifa)
        {
            this.Categoria = categoria;
            this.TarifaDiferenciada = tipoTarifa;
            this.TipoDia = tipoDia;
            this.Habilitado = habilitado;
            this.ValorTarifa = valorTarifa;
        }


        // Valor de la tarifa en formato para grilla
        public string sValorTarifa
        {
            get
            {
                return ValorTarifa.ToString("F02");
            }
        }


        // Categoria a la que pertenece el valor definido
        public CategoriaManual Categoria { get; set; }

        // Tipo de tarifa diferenciada a la que pertenece la definicion del valor
        public TarifaDiferenciada TarifaDiferenciada { get; set; }

        // Banda horaria a la que pertenece la definicion del valor
        public TipoDiaHora TipoDia { get; set; }

        // Marca de habilitacion de esta tarifa
        public bool Habilitado { get; set; }

        // Valor de la tarifa, definida para la estacion, categoria, tipo de tarifa, tipo de dia y fecha de vigencia.
        public decimal ValorTarifa { get; set; }

        // Valor de la tarifa redondeada, definida para la estacion, categoria, tipo de tarifa, tipo de dia y fecha de vigencia.
        public decimal ValorTarifaRed { get; set; }
    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TarifaDetalle
    /// </summary>*********************************************************************************************
    public class TarifaDetalleL : List<TarifaDetalle>
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una determinada TarifaDetalle localizada mediante los parametros
        /// </summary>
        /// <param name="categoria">byte - Categoria a buscar</param>
        /// <param name="tipoDia">string - Tipo de dia de la tarifa</param>
        /// <param name="tipoTarifa">int - Tipo de dia de la tarifa</param>
        /// <returns>objeto TarifaDetalle que corresponda a la tarifa buscada</returns>
        /// ***********************************************************************************************
        public TarifaDetalle FindTarifaDetalle(byte categoria, string tipoDia, int tipoTarifa)
        {
            TarifaDetalle oTarifaDetalle = null;

            foreach (TarifaDetalle oTar in this)
            {
                if ((categoria == oTar.Categoria.Categoria) &&
                    (tipoDia.Trim() == oTar.TipoDia.Codigo.Trim()) &&
                    (tipoTarifa == oTar.TarifaDiferenciada.CodigoTarifa))
                {

                    oTarifaDetalle = oTar;
                    break;
                }
            }

            return oTarifaDetalle;
        }

    }


    #endregion
}
