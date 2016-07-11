using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region VIDEOCATEGORIA: Clase para entidad de los tiempos por Categorias configuradas para captura de video

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoCategoria
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class VideoCategoria
    {
        public VideoCategoria(ViaModelo modeloVia, CategoriaManual categoria, 
                              double tiempoMaximoC1, VideoConfiguracionAlmacenamiento almacenamientoC1, byte porcentajeC1, 
                              double tiempoMaximoC2, VideoConfiguracionAlmacenamiento almacenamientoC2, byte porcentajeC2  )
        {
            this.ModeloVia = modeloVia;
            this.Categoria = categoria;
            this.TiempoMaximoGrabacionC1 = tiempoMaximoC1;
            this.AlmacenamientoC1 = almacenamientoC1;
            this.PorcentajeMuestreoC1 = getPorcentajeMuestreo(almacenamientoC1.Codigo, porcentajeC1);

            
            this.TiempoMaximoGrabacionC2 = tiempoMaximoC2;
            this.AlmacenamientoC2 = almacenamientoC2;
            this.PorcentajeMuestreoC2 = getPorcentajeMuestreo(almacenamientoC2.Codigo, porcentajeC2);
        }

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

        // Modelo de Via
        public ViaModelo ModeloVia { get; set; }

        // Codigo de Categoria
        public CategoriaManual Categoria { get; set; }

        // Exponemos el valor de la categoria para asignar el DataKeyName        
        public byte vCategoria 
        {
            get { return Categoria.Categoria; }
        }


        // Tiempo Maximo de grabacion de la camara 1
        public double TiempoMaximoGrabacionC1 { get; set; }

        // Cuando almacena (Siempre, Nunca o Muestreo) de la camara 1
        public VideoConfiguracionAlmacenamiento AlmacenamientoC1 { get; set; }

        // Porcentaje de muestreo (solo para Almacenamiento = Muestreo) de la camara 1
        public byte PorcentajeMuestreoC1  { get; set; }

        // Tiempo Maximo de grabacion de la camara 2
        public double TiempoMaximoGrabacionC2 { get; set; }


        // Cuando almacena (Siempre, Nunca o Muestreo) de la camara 2
        public VideoConfiguracionAlmacenamiento AlmacenamientoC2 { get; set; }

        // Porcentaje de muestreo (solo para Almacenamiento = Muestreo) de la camara 2
        public byte PorcentajeMuestreoC2 { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoCategoria
    /// </summary>*********************************************************************************************
    public class VideoCategoriaL : List<VideoCategoria>
    {
    }

    #endregion
}
