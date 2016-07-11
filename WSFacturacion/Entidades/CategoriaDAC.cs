using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CATEGORIADAC: Clase para entidad de Categorias del DAC

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CategoriaDAC (categorias automaticas)
    /// </summary>*********************************************************************************************
    [Serializable]    
    public class CategoriaDAC
    {
        /// <summary>
        /// Minima cantidad de ejes
        /// </summary>
        public static int MimimoEjes
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Maxima cantidad de ejes
        /// </summary>
        public static int MaximoEjes
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public CategoriaDAC()
        {
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="ejes"></param>
        /// <param name="bMoto"></param>
        /// <param name="bRuedaDual"></param>
        /// <param name="bAlto"></param>
        /// <param name="cat"></param>
        public CategoriaDAC(int ejes, bool bMoto, bool bRuedaDual, bool bAlto, byte? cat, string catEsp)
        {
            CantidadEjes = ejes;
            Categoria = cat;
            esMoto = bMoto;
            esRuedaDual = bRuedaDual;
            esAlto = bAlto;
            CategoriaEspecial = catEsp;
        }

        /// <summary>
        /// Cantidad de ejes detectados
        /// </summary>
        public int CantidadEjes { get; set; }

        /// <summary>
        /// Categoria aplicada para la cantidad de ejes, dualidad y altura
        /// puede ser null
        /// </summary>
        public byte? Categoria { get; set; }
        
        /// <summary>
        /// Si el vehiculo tiene o no ruedas duales
        /// </summary>
        public bool esRuedaDual
        {
            get { return RuedaDual != "0"; }
            set { RuedaDual = value ? "1" : "0"; }
        }
        
        /// <summary>
        /// Codigo de dualidad. En base a este dato se determina (en BS) si es dual o no
        /// </summary>
        public string RuedaDual { get; set; }
        
        /// <summary>
        /// Si el vehiculo es o no alto 
        /// </summary>
        public bool esAlto
        {
            get { return Altura == "A"; }
            set { Altura = value ? "A" : "B"; }
        }
        
        /// <summary>
        /// Codigo de altura. En base a este dato se determina (en BS) si es alto o no
        /// </summary>
        public string Altura { get; set; }
        
        /// <summary>
        /// Indica si se trata particularmente de categoria automatica para moto (aqui solo interviene la categoria)
        /// </summary>
        public bool esMoto
        {
            get { return Moto == "S"; }
            set { Moto = value ? "S" : "N" ; }
        }

        /// <summary>
        /// Codigo de Moto. En base a este dato se determina (en BS) si es una moto o no
        /// </summary>
        public string Moto { get; set; }

        /// <summary>
        /// Indica si es categoria especial (S / N)
        /// </summary>
        public string CategoriaEspecial { get; set; }
    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos CategoriaDAC
    /// </summary>*********************************************************************************************
    [Serializable]
    public class CategoriaDACL : List<CategoriaDAC>
    {    
    }
    
    #endregion
}
