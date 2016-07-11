using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region TAG: Clase para entidad de un Tag

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Tag
    /// </summary>*********************************************************************************************
    [Serializable]
    public class Tag
    {
        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Tag()
        {
        }

        /// <summary>
        /// Constructor con muchos parámetros
        /// </summary>
        /// <param name="estacioEmision"></param>
        /// <param name="patente"></param>
        /// <param name="numeroTag"></param>
        /// <param name="habilitado"></param>
        /// <param name="fechaEntrega"></param>
        /// <param name="ListaNegra"></param>
        /// <param name="numeroExterno"></param>
        public Tag(Estacion estacioEmision,             string patente,                 string numeroTag,
                   string habilitado,                   DateTime fechaEntrega,          TagListaNegra listaNegra,
                   int? numeroExterno)
        {
            EstacionEmision = estacioEmision;
            Patente = patente;
            NumeroTag = numeroTag;
            Habilitado = habilitado;
            FechaEntrega = fechaEntrega;
            ListaNegra = listaNegra;
            NumeroExterno = numeroExterno;
        }

        /// <summary>
        /// Constructor con muchos parámetros
        /// </summary>
        /// <param name="patente"></param>
        /// <param name="numeroTag"></param>
        /// <param name="numeroEmisor"></param> 
        /// <param name="habilitado"></param>
        ///<param name="estado"></param>

        public Tag(string patente, string numeroTag,string numeroEmisor,
                   string habilitado, string estado)
        {
            Patente = patente;
            NumeroTag = numeroTag;
            EmisorTag = numeroEmisor;
            Habilitado = habilitado;
            EstadoTag = estado;
        }
        /// <summary>
        /// Estacion en la que se dio de alta el tag
        /// </summary>
        public Estacion EstacionEmision { get; set; }

        /// <summary>
        /// Numero externo
        /// </summary>
        public int? NumeroExterno { get; set; }

        /// <summary>
        /// Patente a la que pertenece 
        /// </summary>
        public string Patente { get; set; }

        /// <summary>
        /// Numero de tag
        /// </summary>
        public string NumeroTag { get; set; }

        /// <summary>
        /// Obtiene o establece el emisor del tag
        /// </summary>
        public string EmisorTag { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de vencimiento del tag
        /// </summary>
        public DateTime? FechaVencimiento { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del tag
        /// </summary>
        public string EstadoTag { get; set; }

        /// <summary>
        /// Número de secuencia de la Lista Negra
        /// </summary>
        public int? Secuencia { get; set; }

        /// <summary>
        /// Número de secuencia del Tag
        /// </summary>
        public int? SecuenciaTag { get; set; }
    
        /// <summary>
        /// Si esta o no habilitado
        /// </summary>
        public string Habilitado { get; set; }

        /// <summary>
        /// Si esta o no entregado
        /// </summary>
        public bool Entregado { get; set; }

        /// <summary>
        /// Fecha de entrega
        /// </summary>
        public DateTime FechaEntrega { get; set; }

        /// <summary>
        /// Booleano que determina si esta habilitado o no
        /// </summary>
        public bool esHabilitado
        {
            get
            {
                return (Habilitado == "S");
            }
        }

        /// <summary>
        /// Obtiene o establece la fecha en que se puso en lista negra
        /// </summary>
        public DateTime? FechaListaNegra { get; set; }

        /// <summary>
        /// Si esta en lista negra o no
        /// </summary>
        public TagListaNegra ListaNegra { get; set; }

        /// <summary>
        /// Booleano que determina si esta o no en lista negra
        /// </summary>
        public bool esListaNegra
        {
            get
            {
                return (ListaNegra != null);
            }
        }
    }
    
    /// *********************************************************************************************<summary>
    /// Lista de objetos Tag
    /// </summary>*********************************************************************************************
    [Serializable]
    public class TagL : List<Tag>
    {
    }

    #endregion
}
