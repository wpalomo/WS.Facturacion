using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class Imagen
    {
        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public Imagen()
        {
        }

        /// <summary>
        /// nombre de la imagen
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// ruata de la imagen a mostrar (URL o fisica)
        /// </summary>
        public string Ruta { get; set; }

        /// <summary>
        /// lugar donde se encuentra la imagen
        /// </summary>
        public string RutaFisica { get; set; }

        /// <summary>
        /// lugar donde se encuentra publicada la imagen
        /// </summary>
        public string RutaURL { get; set; }

        /// <summary>
        /// indica que tipo de seleccion tiene la imagen, si es frontal 'F' o lateral1 'L1' o lateral1 'L2'
        /// </summary>
        public string Seleccion { get; set; }

        /// <summary>
        ///  indica que tipo de imagen es,si es video 'V' o foto 'F'
        /// </summary>
        public string TipoImagen { get; set; }

        /// <summary>
        /// Camara con la que se saca la foto(Frontal 'F' o Lateral 'L')
        /// </summary>
        public string TipoCamara { get; set; }
        
    }

    [Serializable]
    /// <summary>
    /// Lista de Objetos Imagenes
    /// </summary>   
    public class ImagenL : List<Imagen>
    {
    }
}
