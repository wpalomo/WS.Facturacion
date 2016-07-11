using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class EmisoresTag
    {
    /// ***********************************************************************************************
    /// <summary>
    /// En el constructor de la clase asigna los valores de un Emisor de Tag
    /// </summary>
    /// <typeparam name="int">int</typeparam>
    /// <param name="codigo">Codigo del Emisor</param>
    /// <typeparam name="int">string</typeparam>
    /// <param name="codigo">Descripcion del Emisor</param>
    /// <returns></returns>
    /// ***********************************************************************************************
            
        public EmisoresTag(string codigo, string descripcion,AdministradoraTags oOSA)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            this.Administradora = oOSA;
        }

        public EmisoresTag(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        public EmisoresTag()
        {

        }


        // Codigo del Emisor
        public string Codigo { get; set; }

        // Descripcion del Emisor
        public string Descripcion { get; set; }

        // Administradora del Tag
        public AdministradoraTags Administradora { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }

        public string DescrAdminis
        {
            get { return Administradora.Descripcion; }
        }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Emisores de Tag.
    /// </summary>
    public class EmisoresTagL : List<EmisoresTag>
    {
    }

 }