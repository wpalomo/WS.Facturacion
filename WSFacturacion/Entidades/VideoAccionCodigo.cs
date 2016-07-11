using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIDEOACCIONCODIGO: Clase para entidad de las posibles Acciones para configuración de video

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoAccionCodigo
    /// </summary>*********************************************************************************************    
    
    [Serializable]

    public class VideoAccionCodigo
    {
        public VideoAccionCodigo(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        // Codigo de Accion
        public string Codigo { get; set; }

        // Descripcion de la accion
        public string Descripcion { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoAccionCodigo
    /// </summary>*********************************************************************************************
    public class VideoAccionCodigoL : List<VideoAccionCodigo>
    {
    }

    #endregion
}
