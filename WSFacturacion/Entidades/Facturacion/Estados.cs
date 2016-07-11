using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    #region ESTADOS Y CAPITALES: Clase para entidad de Estados y Capitales

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Provincia
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class Estado
    {
        public Estado(string codigo, string descripcion, string Capital, string Region)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            this.DescCapital = Capital;
            this.Region = Region;
        }

        public Estado() { }


        // Codigo del estado
        public string Codigo { get; set; }

        // Nombre del estado
        public string Descripcion { get; set; }

        // Nombre de la Capital
        public string DescCapital { get; set; }

        // Nombre del la Region
        public string Region { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Provincia
    /// </summary>*********************************************************************************************
    public class EstadoL : List<Estado>
    {
    }

    #endregion

}
