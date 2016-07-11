using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIDEOPATH: Clase para entidad de las rotaciones de carpetas de videos

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoPath
    /// </summary>*********************************************************************************************

    [Serializable]

    public class VideoPath
    {
        // Constructor por defecto
        public VideoPath()
        {
        }


        // Constructor sobrecargado
        public VideoPath(string pathCarpeta, string pathCarpetaFTP)
        {
            this.PathCarpeta = pathCarpeta;
            this.PathCarpetaFTP = pathCarpetaFTP;
        }


        // Constructor sobrecargado
        public VideoPath(byte ID, string pathCarpeta, string pathCarpetaFTP)
        {
            this.PathCarpeta = pathCarpeta;
            this.PathCarpetaFTP = pathCarpetaFTP;
            this.IDRegistro = ID;
        }


        // Identity del registro, para identificarlo
        public byte IDRegistro { get; set; }

        // Path de la carpeta de rotacion
        public string PathCarpeta { get; set; } 

        // Path FTP de la carpeta de rotacion
        public string PathCarpetaFTP { get; set; } 

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoPath
    /// </summary>*********************************************************************************************
    public class VideoPathL : List<VideoPath>
    {
    }

    #endregion
}
