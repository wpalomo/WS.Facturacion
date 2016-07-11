using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CATEGORIAFORMAPAGO: Clase para entidad de Categorias habilitadas para una Forma de Pago


    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CategoriaFormaPago 
    /// </summary>*********************************************************************************************

    [Serializable]
    
    public class CategoriaFormaPago
    {
        // Categoria de la que se traen las formas de pago habilitadas y no habilitadas
        public byte Categoria { get; set; }

        // Item de forma de pago
        public FormaPago FormaPago { get; set; }

        // Habilitado para la forma y categoria indicada
        public bool Habilitada { get; set; }

        // Retorna la combinacion de tipo y subtipo de forma de pago
        public string CodigoFormaPago 
        {
            get { return FormaPago.CodigoFormaPago; }
        }

        // Retorna la descripcion de la forma de pago
        public string DescripcionFormaPago 
        {
            get { return FormaPago.Descripcion; }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos CategoriaFormaPago
    /// </summary>*********************************************************************************************
    public class CategoriaFormaPagoL : List<CategoriaFormaPago>
    {
        public CategoriaFormaPago FindCategoriaFormaPago( string codigo)
        {
            CategoriaFormaPago oFormaPago = null;


            foreach (CategoriaFormaPago item in this)
            {
                if (item.CodigoFormaPago == codigo)
                {
                    oFormaPago = item;
                    break;
                }
            }
            return oFormaPago;
        }
    }


    #endregion
}
