using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIDEOEVENTO: Clase para entidad de los Eventos configurados en la Captura de video

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoEvento
    /// </summary>*********************************************************************************************

    [Serializable]

    public class VideoEvento
    {

        public VideoEvento(ViaModelo modeloVia, VideoEventoCodigo evento, VideoConfiguracionAlmacenamiento almacenamientoC1, byte porcentajeC1, VideoConfiguracionAlmacenamiento almacenamientoC2, byte porcentajeC2)
        {
            this.ModeloVia = modeloVia;
            this.Evento = evento;
            this.AlmacenamientoC1 = almacenamientoC1;
            this.PorcentajeMuestreoC1 = getPorcentajeMuestreo(almacenamientoC1.Codigo, porcentajeC1);

            this.AlmacenamientoC2 = almacenamientoC2;
            this.PorcentajeMuestreoC2 = porcentajeC2;
            this.PorcentajeMuestreoC2 = getPorcentajeMuestreo(almacenamientoC2.Codigo, porcentajeC2);
        }


        
        // Modelo de Via
        public ViaModelo ModeloVia { get; set; }

        // Evento a configurar
        public VideoEventoCodigo Evento { get; set; }

        // Exponemos el valor del codigo de Evento para el DataKeyName
        public string vEvento
        {
            get { return Evento.Codigo; }
        }


        // Tipo de Almacenamiento (Siempre, Nunca o Muestreo) para la camara 1
        public VideoConfiguracionAlmacenamiento AlmacenamientoC1 { get; set; }

        // Porcentaje de muestreo de la camara 1 (solo para Almacenamiento = Muestreo)
        public byte PorcentajeMuestreoC1 { get; set; }

        // Tipo de Almacenamiento (Siempre, Nunca o Muestreo) para la camara 2
        public VideoConfiguracionAlmacenamiento AlmacenamientoC2 { get; set; }

        // Porcentaje de muestreo de la camara 2 (solo para Almacenamiento = Muestreo)
        public byte PorcentajeMuestreoC2 { get; set; }

        // Centralizamos en una funcion el porcentaje de filmacion (Nunca = 0, Siempre = 100, Muestreo = lo que haya configurado)
        protected byte getPorcentajeMuestreo(string codigoAlamcenamiento, byte porcentaje)
        {
            byte porce;
            switch (codigoAlamcenamiento)
            {
                case "S":
                    porce = 100;
                    break;

                case "N":
                    porce = 0;
                    break;

                case "M":
                    porce = porcentaje;
                    break;

                default:
                    porce = porcentaje;
                    break;
            }

            return porce;
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoEvento
    /// </summary>*********************************************************************************************
    public class VideoEventoL : List<VideoEvento>
    {
    }

    #endregion
}
