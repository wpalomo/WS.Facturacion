using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    #region PROVINCIA: Clase para entidad de Provincias

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Provincia
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class Provincia
    {
        public Provincia(int codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        // Codigo de Provincia
        public int Codigo { get; set; }

        // Nombre de la Provincia
        public string Descripcion { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Provincia
    /// </summary>*********************************************************************************************
    public class ProvinciaL : List<Provincia>
    {
    }

    #endregion

}
