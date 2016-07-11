using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    [Serializable]

    public class VehiculoMarca
    {

        public VehiculoMarca(int Codigo, string Descripcion, string Delete)
        {
            this.Codigo = Codigo;
            this.Descripcion = Descripcion;
            this.Delete = Delete;
        }

        public VehiculoMarca()
        {
        }
        
        // Codigo de la Marca
        public int Codigo { get; set; }

        // Descripcion de la Marca
        public string Descripcion { get; set; }

        // Delete de la Marca
        public string Delete { get; set; }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Marca.
    /// </summary>
    public class VehiculoMarcaL : List<VehiculoMarca>
        {

        }        

}

