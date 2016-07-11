using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Telectronica.Peaje
{
    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoDiaHora
    /// </summary>*********************************************************************************************

    [Serializable]

    public class TipoDiaHora
    {

        public TipoDiaHora(string Codigo, string Descripcion, string descripcionCorta)
        {
            this.Codigo = Codigo;
            this.Descripcion = Descripcion;
            this.DescripcionCorta = descripcionCorta;
        }

        public TipoDiaHora()
        {

        }

        // Codigo del TipoDiaHora
        public string Codigo { get; set; }

        // Descripcion del TipoDiaHora
        public string Descripcion { get; set; }

        // Descripcion corta para mostrar en combos como de bandas horarias
        public string DescripcionCorta { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoDiaHora.
    /// *********************************************************************************************<summary>
    public class TipoDiaHoraL : List<TipoDiaHora>
    {

        
        
    }

}

