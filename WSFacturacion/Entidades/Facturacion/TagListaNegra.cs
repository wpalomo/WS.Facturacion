using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region TAGLISTANEGRA: Clase para entidad de Lista Negra de un Tag

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TagListaNegra
    /// </summary>*********************************************************************************************

    [Serializable]
    public class TagListaNegra
    {
        public TagListaNegra()
        {
        }

        public TagListaNegra(string numeroTag,              DateTime fechaInhabilitacion,           
                             Usuario usuario,               string comentario)
        {
            this.NumeroTag = numeroTag;
            this.FechaInhabilitacion = fechaInhabilitacion;
            this.Usuario = usuario;
            this.Comentario = comentario;
        }


        // Numero de tag
        public string NumeroTag { get; set; }

        // Fecha de puesta en LN
        public DateTime FechaInhabilitacion { get; set; }

        // Usuario que lo puso en LN
        public Usuario Usuario { get; set; }

        // Comentario colocado en el momento de ponerlo en LN
        public string Comentario { get; set; }

        // Patente
        public string Patente { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TagListaNegra
    /// </summary>*********************************************************************************************
    public class TagListaNegraL : List<TagListaNegra>
    {
    }

    #endregion
}
