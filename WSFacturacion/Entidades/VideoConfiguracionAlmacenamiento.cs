using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region VIDEOCONFIGURACIONALMACENAMIENTO: Clase para entidad que determina los tipos de almacenamiento de video

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoConfiguracionAlmacenamiento
    /// </summary>*********************************************************************************************    

    [Serializable]
    
    public class VideoConfiguracionAlmacenamiento
    {
        // En el constructor asignamos los valores
        public VideoConfiguracionAlmacenamiento(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        // Tipo de almacenamiento (Valor del Combo)
        public string Codigo { get; set; }

        // Descripcion del Tipo de almacenamiento (Lo que se muestra en el Combo)
        public string Descripcion { get; set; }

    }


    [Serializable]

    public class VideoConfiguracionAlmacenamientoL : List<VideoConfiguracionAlmacenamiento>
    {
    }

    #endregion
}
