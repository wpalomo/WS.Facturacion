namespace Telectronica.Peaje
{
    using System.Collections.Generic;

    /// <summary>
    /// Clase entidad que representa un site
    /// </summary>    
    public class Site
    {

        // Constructor vacio
        public Site()
        {
        }


        public Site(int codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }


        /// <summary>
        /// Obtiene o establece un código único e identificatorio del número de site
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece una breve descripción del site
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Obtiene la descripción
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Descripcion??"";
        }
    }

    /// <summary>
    /// Clase lista de Site
    /// </summary>
    public class SiteL : List<Site>
    {    
    }
}
