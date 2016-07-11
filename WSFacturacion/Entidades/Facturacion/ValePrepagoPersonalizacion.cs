using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region ValePrepagoPersonalizacion: Clase para entidad de Personalizacion de Vale Prepago

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad de Personalizacion de Vale Prepago
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "ValePrepagoPersonalizacion", IsNullable = false)]

    public class ValePrepagoPersonalizacion
    {
        // Constructor vacio
        public ValePrepagoPersonalizacion()
        {
        }


        public ValePrepagoPersonalizacion(int lote,             Zona zona,                  DateTime fechaCarga,
                                          Cuenta cuenta,        Usuario usuario,            int serieInicial,           
                                          int serieFinal,       CategoriaManual categoria,  int valesXHoja,
                                          string anulada)
        {
            this.Lote = lote;
            this.Zona = zona;
            this.FechaCarga = fechaCarga;
            this.Cuenta = cuenta;
            this.Usuario = usuario;
            this.SerieInicial = serieInicial;
            this.SerieFinal = serieFinal;
            this.Categoria = categoria;
            this.ValesxHoja = valesXHoja;
            this.Anulada = anulada;
        }


        // Numero de lote
        public int Lote { get; set; }

        // Zona para la cual se personaliza el vale
        public Zona Zona { get; set; }

        // Fecha de la personalizacion 
        public DateTime FechaCarga { get; set; }

        // Cuenta a la cual se le imputa la recarga
        public Cuenta Cuenta { get; set; }

        // Usuario que realizo la carga 
        public Usuario Usuario { get; set; }

        // Serie inicial del vale
        public int SerieInicial { get; set; }

        // Serie final del vale
        public int SerieFinal { get; set; }

        // Categoria
        public CategoriaManual Categoria { get; set; }

        // Cantidad de vales por hoja
        public int ValesxHoja { get; set; }

        // Marca de recarga anulada
        public string Anulada { get; set; }

        // Determina si la entrega esta anulada o no
        public bool esAnulada
        {
            get
            {
                return (Anulada == "S");
            }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ValePrepagoPersonalizacion
    /// </summary>*********************************************************************************************
    public class ValePrepagoPersonalizacionL : List<ValePrepagoPersonalizacion>
    {
    }

    #endregion
}
