using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    #region Cupones: Clase para entidad de CUPON

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Cupon
    /// </summary>*********************************************************************************************
    [Serializable]
    public class TipoCupon
    {
        public TipoCupon()
        {
        }

        public TipoCupon(int codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }

        /// <summary>
        /// Codigo de cupon
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Se usa en la via (S / N)
        /// </summary>
        public string UsoEnVia { get; set; }

        /// <summary>
        /// Devuelve la propiedad UsoEnVia como booleano
        /// </summary>
        public bool esUsoEnVia
        {
            get { return (UsoEnVia == "S"); }
            set { UsoEnVia = (value? "S" : "N");}
        }

        /// <summary>
        /// Tiene Codigo Barras (S / N)
        /// </summary>
        public string ConCodigoDeBarra { get; set; }

        /// <summary>
        /// Devuelve la propiedad ConCodigoDeBarra como booleano
        /// </summary>
        public bool esConCodigoDeBarra
        {
            get { return (ConCodigoDeBarra == "S"); }
            set { ConCodigoDeBarra = (value? "S" : "N"); }
        }

        /// <summary>
        /// Tipo de tarifa aplicada
        /// </summary>
        public TarifaDiferenciada TipoTarifa { get; set; }

        /// <summary>
        /// Indica si registra o no los cupones recibidos (S / N)
        /// </summary>
        public string RegistraCupones { get; set; }

        /// <summary>
        /// Convierte en booleano la propiedad RegistraRecibido
        /// </summary>
        public bool esRegistraCupones
        {
            get { return (RegistraCupones == "S"); }
            set { RegistraCupones = (value? "S" : "N"); }
        }
    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos Cupon
    /// </summary>*********************************************************************************************
    [Serializable]
    public class TipoCuponL : List<TipoCupon>
    {
    }

    #endregion
}
