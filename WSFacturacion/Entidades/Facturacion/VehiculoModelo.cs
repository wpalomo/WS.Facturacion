using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    [Serializable]

    public class VehiculoModelo
    {
        public VehiculoModelo(VehiculoMarca Marca, int Codigo, string Descripcion, string Delete)
        {
            this.Marca = Marca;
            this.Codigo = Codigo;
            this.Descripcion = Descripcion;
            this.Delete = Delete;
        }

        public VehiculoModelo()
        {

        }
        
        // Marca del vehiculo (estructura)
        public VehiculoMarca Marca { get; set; }

        // Codigo del Modelo
        public int Codigo { get; set; }

        // Descripcion del Modelo
        public string Descripcion { get; set; }

        // Marca de Delete del Modelo
        public string Delete { get; set; }

        // Codigo de la marca (para usar en la grilla)
        public int MarcaCodigo
        {
            get { return Marca.Codigo; }
        }

        // Descripcion de la marca (para mostrar en grilla)
        public string MarcaDescripcion
        {
            get { return Marca.Descripcion; }
        }


    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Modelo.
    /// </summary>
    public class VehiculoModeloL : List<VehiculoModelo>
    {

    }   
}

