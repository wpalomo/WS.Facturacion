using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region TIPOCUENTA: Clase para entidad de los Tipos de Cuentas

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoCuenta
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "TipoCuenta", IsNullable = false)]

    public class TipoCuenta
    {

        public TipoCuenta(int Codigo, string Descripcion)
        {
            this.CodigoTipoCuenta = Codigo;
            this.Descripcion = Descripcion;
        }

        public TipoCuenta()
        {
        }

        // Codigo de TipoCuenta
        private int codigoTipoCuenta;

        public int CodigoTipoCuenta
        {
            get
            {
                return codigoTipoCuenta;
            }
            set
            {
                codigoTipoCuenta = value;
                if (value == 3)
                {
                    EsPrepago = true;
                    EsPospago = false;
                }
                else if (value == 4)
                {
                    EsPrepago = false;
                    EsPospago = true;
                }
            }
        }

        // Descripcion de TipoCuenta
        public string Descripcion { get; set; }

        public bool EsPrepago { get; set; }
        public bool EsPospago { get; set; }

        public string TipoBoleto { get; set; }
    }
    
    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoCuenta
    /// </summary>*********************************************************************************************
    public class TipoCuentaL : List<TipoCuenta>
    {
    }

    #endregion
}
