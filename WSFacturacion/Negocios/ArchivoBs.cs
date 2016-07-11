using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de archivos y carpetas 
    /// </summary>
    ///****************************************************************************************************

    public static class ArchivoBs
    {

        #region ARCHIVOS: Clase de Negocios de Archivos

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna una lista con los archivos graficos de las categorias
        /// </summary>
        /// <returns>Lista de archivos de una carpeta que tengan la extension mencionada</returns>
        /// ***********************************************************************************************      
        public static ArchivoL getListaArchivosGraficosCategoria()
        {
            return getListaArchivos(mdlGeneral.getPathImagenesCatego(), getExtensionesArchivosGraficos());
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna una lista con los archivos graficos que se encuentran en la carpeta indicada
        /// </summary>
        /// <returns>Lista de archivos de una carpeta que tengan la extension mencionada</returns>
        /// ***********************************************************************************************      
        public static ArchivoL getListaArchivosGraficos(string CarpetaContenedora)
        {
            return getListaArchivos(CarpetaContenedora, getExtensionesArchivosGraficos());
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna una lista con los archivos que se encuentran en la carpeta indicada que tengan extension dada.
        /// </summary>
        /// <returns>Lista de archivos de una carpeta que tengan la extension mencionada</returns>
        /// ***********************************************************************************************      
        public static ArchivoL getListaArchivos(string CarpetaContenedora, 
                                                string ExtensionesValidas)
        {
            ArchivoL oListaArchivos = new ArchivoL();
            string[] extenciones = ExtensionesValidas.Split('|');


            foreach (string extencion in extenciones)
            {
                string[] oDir = Directory.GetFiles(HttpContext.Current.Server.MapPath(CarpetaContenedora), extencion.Trim());
                foreach (string oArchi in oDir)
                    oListaArchivos.Add(new Archivo(oArchi, CarpetaContenedora));
            }


            return oListaArchivos;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de extensiones de archivos graficos validos
        /// </summary>
        /// <returns>Archivos graficos valisod</returns>
        /// ***********************************************************************************************      
        public static string getExtensionesArchivosGraficos()
        {
            //return "*.BMP | *.JPG | *.PNG";
            return "*.PNG | *.BMP";
            //return "*.BMP";
        }


        #endregion

    }
}
