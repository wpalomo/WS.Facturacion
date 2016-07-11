using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Telectronica.Facturacion
{
    [Serializable]

    public class VehiculoColor
    {

        public VehiculoColor(int Codigo, string Descripcion, string Delete)
        {
            this.Codigo = Codigo;
            this.Descripcion = Descripcion;
            this.Delete = Delete;
        }

        public VehiculoColor()
        {

        }

        // Codigo del Color

        public int Codigo { get; set; }

        // Descripcion del Color

        public string Descripcion { get; set; }

        // Delete de un Color

        public string Delete { get; set; }

        /// <summary>
        /// Lista de objetos Color.
        /// </summary>
        /// 

    }


    [Serializable]

    public class VehiculoColorL : List<VehiculoColor>
    {

    }

}

