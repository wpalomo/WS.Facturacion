using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region VIDEOEVENTOCODIGO: Clase para entidad de los posibles Eventos para configuración de video

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoEventoCodigo
    /// </summary>*********************************************************************************************

    [Serializable]

    public class VideoEventoCodigo
    {
        public VideoEventoCodigo(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }



        // Codigo de Evento
        public string Codigo { get; set; }

        // Descripcion del Evento
        public string Descripcion { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoEventoCodigo
    /// </summary>*********************************************************************************************
    public class VideoEventoCodigoL : List<VideoEventoCodigo>
    {
    }

    #endregion
}
