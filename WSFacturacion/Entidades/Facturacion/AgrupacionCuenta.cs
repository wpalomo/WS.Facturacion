using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region AGRUPACIONCUENTA: Clase para entidad de las Agrupaciones Cuentas

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad AgrupacionCuenta
    /// </summary>********************************************************************************************
    
    [Serializable]
    [XmlRootAttribute(ElementName = "AgrupacionCuenta", IsNullable = false)]
    public class AgrupacionCuenta
    {
        public AgrupacionCuenta(TipoCuenta TipoCuenta, Int16 SubTipoCuenta, string DescrAgrupacion, TarifaDiferenciada TipoTarifa,
                                TarifaDiferenciada TipoTarifaVenta, int? DiasDuracionCuenta, int? DiasAntesVencimiento, int? CantidadViajesMaximos,
                                TarifaDiferenciada TipoTarifa2)
        {
            this.TipoCuenta = TipoCuenta;
            this.SubTipoCuenta = SubTipoCuenta;
            this.DescrAgrupacion = DescrAgrupacion;
            this.TipoTarifa = TipoTarifa;
            this.TipoTarifaVenta = TipoTarifaVenta;
            this.DiasDuracionCuenta = DiasDuracionCuenta;
            this.DiasAntesVencimiento = DiasAntesVencimiento;
            this.CantidadViajesMaximos = CantidadViajesMaximos;
            this.TipoTarifa2 = TipoTarifa2;
        }

        public AgrupacionCuenta(TipoCuenta TipoCuenta, Int16 SubTipoCuenta, string DescrAgrupacion, TarifaDiferenciada TipoTarifa,
                                TarifaDiferenciada TipoTarifaVenta, int? DiasDuracionCuenta, int? DiasAntesVencimiento, int? CantidadViajesMaximos, 
                                TarifaDiferenciada TipoTarifa2, char? ControlaCategoria, char? ControlaPatente)
            {
                this.TipoCuenta = TipoCuenta;
                this.SubTipoCuenta = SubTipoCuenta;
                this.DescrAgrupacion = DescrAgrupacion;
                this.TipoTarifa = TipoTarifa;
                this.TipoTarifaVenta = TipoTarifaVenta;
                this.DiasDuracionCuenta = DiasDuracionCuenta;
                this.DiasAntesVencimiento = DiasAntesVencimiento;
                this.CantidadViajesMaximos = CantidadViajesMaximos;
                this.TipoTarifa2 = TipoTarifa2;
                this.ControlaCategoria = ControlaCategoria;
                this.ControlaPatente = ControlaPatente;
            }

        public AgrupacionCuenta(TipoCuenta TipoCuenta, Int16 SubTipoCuenta, string DescrAgrupacion)
        {
            this.TipoCuenta = TipoCuenta;
            this.SubTipoCuenta = SubTipoCuenta;
            this.DescrAgrupacion = DescrAgrupacion;
        }
        public AgrupacionCuenta()
            {
            }


        // Codigo de Tipo de Cuentas
        public TipoCuenta TipoCuenta { get; set; }

        // Codigo de Tipo de Tarifas
        public Int16 SubTipoCuenta { get; set; }

        // Descripcion de Agrupacion
        public string DescrAgrupacion { get; set; }

        // Tipo de Tarifa
        public TarifaDiferenciada TipoTarifa { get; set; }

        // Tipo de Tarifa para la Venta 
        public TarifaDiferenciada TipoTarifaVenta { get; set; }

        //dias de duracion de cuenta
        public int? DiasDuracionCuenta { get; set; }

        // dias antes del vencimiento para renovar papeles
        public int? DiasAntesVencimiento { get; set; }

        // Cantidad maxima de viajes
        public int? CantidadViajesMaximos { get; set; }

        // Tipo de Tarifa Alternativa, cuando se supera la cantidad maxima de viajes se utiliza esta tarifa
        public TarifaDiferenciada TipoTarifa2 { get; set; }

        // Indica si controla Categoria:
        public char? ControlaCategoria { get; set;}

        // Indica si controla Patente:
        public char? ControlaPatente { get; set; }

        public int CodigoTipoCuenta 
        { 
            get 
            {
                return TipoCuenta.CodigoTipoCuenta;
            }
        }

        public string DescripcionTipoCuenta
        {
            get
            {
                return TipoCuenta.Descripcion;
            }
        }

        public string DescripcionTipoTarifa
        {
            get
            {
                return TipoTarifa.Descripcion;
            }
        }

        public string DescripcionTipoTarifaVenta
        {
            get
            {
                return TipoTarifaVenta.Descripcion;
            }
        }

        public string DescripcionTipoTarifaExcedida
        {
            get
            {
                return TipoTarifa2.Descripcion;
            }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos AgrupacionCuenta
    /// </summary>*********************************************************************************************
    public class AgrupacionCuentaL : List<AgrupacionCuenta>
    {
    }

    #endregion
}
