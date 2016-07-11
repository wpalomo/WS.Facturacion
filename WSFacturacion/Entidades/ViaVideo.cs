using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIAVIDEO: Clase para entidad de los posibles usos que se le da a la camara de video (Foto, Video, Sin Camara)

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViaVideo
    /// </summary>*********************************************************************************************

    [Serializable]

    public class ViaVideo
    {
        // Constructor por defecto            
        public ViaVideo()
        {
        }

        public ViaVideo(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        
        // Codigo del tipo de Video
        public string Codigo { get; set; }

        // Descripcion del tipo de Video
        public string Descripcion { get; set; }

        // Por defecto la estructura retorna la descripcion (para las grillas)
        public override string ToString()
        {
            return Descripcion;
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaVideo
    /// </summary>*********************************************************************************************
    public class ViaVideoL: List<ViaVideo>
    {
    }

    #endregion
}
