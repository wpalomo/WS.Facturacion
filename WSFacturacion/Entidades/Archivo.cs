using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Telectronica.Peaje
{
    #region ARCHIVO: Clase para entidad de Archivos del sistema en general


    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Archivo
    /// </summary>*********************************************************************************************

    [Serializable]
    
    public class Archivo
    {
        // Sobrecargamos el constructor para asignarle directamente las propiedades en el momento de crearlo
        public Archivo(string pathfisico,
                       string carpetacontenedora)
        {
            this.PathFisico = pathfisico;
            this.CarpetaContenedora = carpetacontenedora;
        }

        
        // Programamos el constructor sin parametros
        public Archivo()
        { 
        }

        //Path Fisico Completo
        public string PathFisico { get; set; }

        // Carpeta de la aplicacion WEB en la que se encuentra el archivo (~/xxxx)
        public string CarpetaContenedora { get; set; }

        // Nombre del archivo (sin path)
        public string Nombre 
        {
            get
            {
                int pos = PathFisico.LastIndexOf('\\');
                return PathFisico.Substring(pos + 1);
            }
        }

        // Extension del archivo
        public string Extension 
        {
            get
            {
                int pos = PathFisico.LastIndexOf('.');
                return PathFisico.Substring(pos + 1);
            }
        }

        // Path Logico del archivo (~/xxxx/Nombre.ext)
        public string PathLogico 
        {
            get
            {
                return CarpetaContenedora + "/" + Nombre;
            }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Archivo (para manejar el contenido de una carpeta)
    /// </summary>*********************************************************************************************
    public class ArchivoL : List<Archivo>
    {
    }


    #endregion
}
