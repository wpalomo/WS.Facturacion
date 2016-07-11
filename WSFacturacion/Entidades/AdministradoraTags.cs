using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class AdministradoraTags
    {
        /// <summary>
        /// Obtiene o establece Codigo de Operadora
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion de la operadora
        /// </summary>
        public String Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion de la operadora
        /// </summary>
        public char TipoListaOper{ get; set; }

        /// <summary>
        /// Obtiene o establece si se envia o no a la via
        /// </summary>
        public char EnviaMaestro{ get; set; }

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public AdministradoraTags()
        {
        }

        /// <summary>
        /// Contructor con codogo y descripcion
        /// </summary>
        /// <param name="Codigo"></param>
        /// <param name="Descripcion"></param>
        public AdministradoraTags(int Codigo, string Descripcion)
        {
            this.Codigo = Codigo;
            this.Descripcion = Descripcion;
        }


    }

    [Serializable]
    /// <summary>
    /// Lista de objetos AdministradoraTags.
    /// </summary>
    public class AdministradoraTagsL : List<AdministradoraTags>
    {
    }
}
