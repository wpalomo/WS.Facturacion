using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region ValePrepagoListaNegra: Clase para entidad de Lista Negra de Vale Prepago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad de Lista Negra de Vale Prepago
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "ValePrepagoListaNegra", IsNullable = false)]

    public class ValePrepagoListaNegra
    {
        // Constructor vacio
        public ValePrepagoListaNegra()
        {
        }


        public ValePrepagoListaNegra(int identity,        DateTime fechaAlta,           DateTime fechaRecupero,
                                     Cliente cliente,     int serieInicial,             int serieFinal,
                                     CategoriaManual categoria, string estado,          string comentario)
        {
            this.Identity = identity;
            this.FechaAlta = fechaAlta;
            this.FechaRecupero = fechaRecupero;
            this.Cliente = cliente;
            this.SerieInicial = serieInicial;
            this.SerieFinal = serieFinal;
            this.Categoria = categoria;
            this.Estado = estado;
            this.Comentario = comentario;
        }


        // Identity
        public int Identity { get; set; }

        // Fecha de puesta en LN
        public DateTime FechaAlta{ get; set; }

        // Fecha de recupero de la LN
        public DateTime FechaRecupero { get; set; }

        // Cliente al cual pertenece el vale
        public Cliente Cliente { get; set; }

        // Serie inicial del vale
        public int SerieInicial { get; set; }

        // Serie final del vale
        public int SerieFinal { get; set; }

        // Categoria
        public CategoriaManual Categoria { get; set; }

        // Marca de estado del registro de LN ('I' – Lista Negra / 'R' – Recuperado)
        public string Estado { get; set; }

        // Determina si la entrega esta anulada o no
        public bool esDeshabilitado
        {
            get
            {
                return (Estado == "I");
            }
        }

        // Comentario colocado en el momento de ponerlo en LN
        public string Comentario { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ValePrepagoListaNegra
    /// </summary>*********************************************************************************************
    public class ValePrepagoListaNegraL : List<ValePrepagoListaNegra>
    {
    }

    #endregion
}
