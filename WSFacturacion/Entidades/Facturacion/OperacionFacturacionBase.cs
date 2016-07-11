using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region OPERACIONFACTURACIONBASE: Clase para entidad de Operaciones que se realizan en facturacion. De esta derivan las demas operaciones

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Operacion Facturacion Base
    /// </summary>*********************************************************************************************

    [Serializable]

    public class OperacionFacturacionBase
    {

        public enum enmTipoOperacion
        {
            enmENTREGA_TAG = 0,
            enmENTREGA_CHIP = 1,
            enmVINCULACION_CHIP = 2,
            enmVENTA_VALE = 3,
            enmRECARGA_PREPAGO = 4,
            enmREEMPLAZO_TICKET = 5,
            enmFALLO_PEAJISTA = 6
        }

        public OperacionFacturacionBase() 
        { 

        }


        // Estacion en la que se genera la operacion
        public Estacion Estacion { get; set; }


        // Identity del registro de la operacion
        public int Identity { get; set; }


        // Fecha y hora de la operacion
        public DateTime FechaOperacion { get; set; }


        // Numero de Factura
        public Factura Factura { get; set; }


        // Determina si una operacion esta facturada o no
        public bool Facturado
        {
            get
            {
                return (this.Factura != null);
            }
        }


        // Tipo de operacion de la que se trata
        public enmTipoOperacion TipoOperacion { get; set; }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos OperacionFacturacionBase
    /// </summary>*********************************************************************************************
    public class OperacionFacturacionBaseL : List<OperacionFacturacionBase>
    {
    }

    #endregion
}
