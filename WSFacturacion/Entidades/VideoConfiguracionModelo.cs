using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region VIDEOCONFIGURACIONMODELO: Clase para entidad de la configuracion de video por cada modelo de via

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoConfiguracionModelo
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class VideoConfiguracionModelo
    {

        public VideoConfiguracionModelo(ViaModelo modelo, byte cuadros)
        {
            this.Modelo = modelo;
            this.CuadrosPorSegundo = cuadros;
        }


        // Modelo de Via
        public ViaModelo Modelo { get; set; }

        // Cantidad de cuadros por segundo que se deben capturar para este modelo de via
        public byte CuadrosPorSegundo { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoConfiguracionModelo
    /// </summary>*********************************************************************************************
    public class VideoConfiguracionModeloL : List<VideoConfiguracionModelo>
    {
    }

    #endregion
}
