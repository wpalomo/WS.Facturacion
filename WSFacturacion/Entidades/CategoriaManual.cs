using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CATEGORIA: Clase para entidad de las Categorias Manuales definidas
    
    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Categoria (categorias manuales)
    /// </summary>*********************************************************************************************
    [Serializable]    
    public class CategoriaManual
    {
        #region CONSTRUCTORES

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public CategoriaManual()
        {
        }
        
        /// <summary>
        /// Constructor con dos parámetros
        /// </summary>
        /// <param name="categoria"></param>
        /// <param name="descripcion"></param>
        public CategoriaManual(byte categoria, string descripcion)
        {
            Categoria = categoria;
            Descripcion = descripcion;
        }

        #endregion

        #region PROPIEDADES

        /// <summary>
        /// Sobrecargamos el metodo ToString() con el numero de categoria
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CategoriaNull;
        }
        
        /// <summary>
        /// Path de las imagenes
        /// </summary>
        public static string PathImagenes { get; set; }

        /// <summary>
        /// Numero de categoria
        /// </summary>
        public byte Categoria { get; set; }


        public string PrincipalCgmp { get; set; }

        /// <summary>
        /// Descripcion de la categoria
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Descripcion larga de la categoria
        /// </summary>
        public string DescripcionLarga  { get; set; }

        /// <summary>
        /// Equivalente
        /// </summary>
        public float Equivalente { get; set; }

        /// <summary>
        /// Nombre de la imagen asociada a la categoria
        /// </summary>
        public string Imagen { get; set; }

        /// <summary>
        /// Path completo de la imagen asociada a la categoria
        /// </summary>
        public string PathImagenCompleto
        {
            get
            {
                return PathImagenes + "/" + Imagen;
            }
        }

        /// <summary>
        /// Para usar de DataTextValue y que muestre vacio la categoria 0
        /// </summary>
        public string CategoriaNull
        {
            get
            {
                string descr = "";
                if (Categoria > 0)
                {
                    descr = Categoria.ToString();
                }
                return descr;
            }
        }

        /// <summary>
        /// Cantidad maxima de categorias definibles
        /// </summary>
        public static byte MaximoCategorias
        {
            get { return 99; }
        }

        /// <summary>
        /// Equivalente ANTT
        /// </summary>
        public byte EquivalenteANTT { get; set; }

        /// <summary>
        /// Equivalente CGMP
        /// </summary>
        public byte EquivalenteCGMP { get; set; }
        
        /// <summary>
        /// Ejes Adicionales Artespi
        /// </summary>
        public byte EjesAdicionalesANTT { get; set; }




        /// <summary>
        /// Lista de las formas de pago habilitadas para la categoria dada
        /// </summary>
        public CategoriaFormaPagoL FormasPagoHabilitadas { get; set; }

        #endregion
    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos Categoria
    /// </summary>*********************************************************************************************
    [Serializable]
    public class CategoriaManualL : List<CategoriaManual>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una determinada moneda localizada mediante los parametros
        /// </summary>
        /// <param name="categoria">int16 - Moneda que se desea localizar</param>
        /// <returns>objeto Moneda buscada</returns>
        /// ***********************************************************************************************
        public CategoriaManual FindCategoria(byte categoria)
        {
            CategoriaManual oCategoria = null;

            foreach (CategoriaManual oCat in this)
            {
                if (categoria == oCat.Categoria)
                {
                    oCategoria = oCat;
                    break;
                }
            }

            return oCategoria;
        }

    }

    #endregion
}
