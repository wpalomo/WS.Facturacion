using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region VIDEOCONFIGURACION: Clase para entidad de la configuracion de video para la captura de video

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoConfiguracion
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class VideoConfiguracion
    {
        public VideoConfiguracion(short diasRotacion, short diasBorrado)
        {
            this.DiasRotacionDirectorios = diasRotacion;
            this.DiasBorradoVideos = diasBorrado;
        }


        // Dias para rotacion de directorios
        public short DiasRotacionDirectorios { get; set; }

        // Dias para borrado de archivos de video
        public short DiasBorradoVideos { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoConfiguracion
    /// </summary>*********************************************************************************************
    public class VideoConfiguracionL : List<VideoConfiguracion>
    {
    }


    #endregion
}
