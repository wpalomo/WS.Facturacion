using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region BANDAHORARIA: Clase para entidad de Bandas Horarias para los diferentes tipos de dia de la semana

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad BandaHoraria
    /// </summary>*********************************************************************************************

    [Serializable]

    public class BandaHoraria
    {
        // Constructor vacio, por defecto
        public BandaHoraria()
        { 
        }


        // En el constructor asignamos los valores a la clase
        public BandaHoraria(int identity, Estacion estacion, ViaSentidoCirculacion sentidoCirculacion, 
                            DateTime fechaInicialVigencia, DateTime? fechaFinalVigencia, BandaHorariaIntervalo intervalo, 
                            BandaHorariaDetalleL bandasHorarias)
        {
            this.Identity = identity;
            this.Estacion = estacion;
            this.SentidoCirculacion = sentidoCirculacion;
            this.FechaInicialVigencia = fechaInicialVigencia;
            this.FechaFinalVigencia = fechaFinalVigencia;
            this.Intervalo = intervalo;
            this.BandasHorarias = bandasHorarias;
        }


        // Identity del registro
        public int Identity { get; set; }

        // Estacion a la que pertenece la banda horaria
        public Estacion Estacion { get; set; }

        // Sentido de circulacion de la banda definida
        public ViaSentidoCirculacion SentidoCirculacion { get; set; }

        // Fecha Inicial de vigencia de la banda
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

        // Fecha Final de vigencia de la banda
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
                {   dAux = FechaFinalVigencia.Value;
                    sRet = dAux.ToString("dd/MM/yyyy HH:mm");
                }

                return sRet;
            }
 
        }

        // Intervalo de minutos entre las bandas horarias configuradas
        public BandaHorariaIntervalo Intervalo { get; set; }


        // Lista de bandas horaras definidas para el cambio realizado
        public BandaHorariaDetalleL BandasHorarias { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos BandaHoraria
    /// </summary>*********************************************************************************************
    public class BandaHorariaL: List<BandaHoraria>
    {
    }

    #endregion

}
