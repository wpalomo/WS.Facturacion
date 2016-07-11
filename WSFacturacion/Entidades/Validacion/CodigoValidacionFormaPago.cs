using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class CodigoValidacionFormaPago
    {
        #region CodigoValidacionFormaPago: Clase para Codigos de Validacion con forma de pago.
        
        public CodigoValidacion CodigoValidacion { get; set; }
        public FormaPagoValidacion FormaPago { get; set; }
        public String Checked { get; set; }

        public bool IsChecked
        {
            get { return Checked == "S"; }
            set { Checked = value ? "S" : "N"; }
        }

        public String PorDefecto { get; set; }

        public bool esPorDefecto
        {
            get { return PorDefecto == "S"; }
            set { PorDefecto = value ? "S" : "N"; }
        }

        public CodigoValidacionFormaPago(CodigoValidacion codigoValidacion, String check, String porDefecto)
        {
            this.CodigoValidacion = codigoValidacion;
            this.Checked = check;
            this.PorDefecto = porDefecto;
        }

        public CodigoValidacionFormaPago(CodigoValidacion codigoValidacion, FormaPagoValidacion formaPago, String check, String porDefecto)
        {
            this.CodigoValidacion = codigoValidacion;
            this.Checked = check;
            this.PorDefecto = porDefecto;
            this.FormaPago = formaPago;
        }

        public CodigoValidacionFormaPago()
        {
        }

        #endregion
    }

    public class AnomaliaFormaPago
    {  
        public Anomalia Anomalia { get; set; }
        public FormaPagoValidacion FormaPago { get; set; }

        public AnomaliaFormaPago(Anomalia anomalia, FormaPagoValidacion formaPago)
        {            
            this.Anomalia = anomalia;
            this.FormaPago = formaPago;
        }

        public AnomaliaFormaPago()
        {
        }
        
    }

    [Serializable]
    public class FormaPagoValidacion
    {
        public int CodAnomalia { get; set; } //fvr_anomal
        public String MedioPago { get; set; } //fvr_tipop
        public String FormaPago { get; set; } //fvr_tipbo
        public Int16? SubformaPago { get; set; } //fvr_subfp
        public String Descripcion { get; set; } //fvr_descr
        public String ExentoDesc { get; set; }
        public int? Exento { get; set; }
        public TarifaDiferenciada TipoTarifa { get; set; }

        public String Codigo {
            get { return MedioPago + "-" + FormaPago; }
        }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }

        public FormaPagoValidacion(String medioPago, String formaPago, Int16? subformaPago, String descripcion)
        {
            this.MedioPago = medioPago;
            this.FormaPago = formaPago;
            this.SubformaPago = subformaPago;
            this.Descripcion = descripcion;
        }

        public FormaPagoValidacion(String medioPago, String formaPago, Int16? subformaPago, String descripcion, String exentoDescripcion, int? exento)
        {
            this.MedioPago = medioPago;
            this.FormaPago = formaPago;
            this.SubformaPago = subformaPago;
            this.Descripcion = descripcion;
            this.ExentoDesc = exentoDescripcion;
            this.Exento = exento;
        }

        public FormaPagoValidacion()
        {
        }
    }

    /// <summary>
    /// Lista de objetos FormaPagoValidacion.
    /// </summary>
    /// 
    [Serializable]
    public class FormaPagoValidacionL : List<FormaPagoValidacion>
    {
    }

    /// <summary>
    /// Lista de objetos CodigoValidacionFormaPago.
    /// </summary>
    /// 
    [Serializable]
    public class CodigoValidacionFormaPagoL : List<CodigoValidacionFormaPago>
    {
    }

    /// <summary>
    /// Lista de objetos AnomaliaFormaPago.
    /// </summary>
    /// 
    [Serializable]
    public class AnomaliaFormaPagoL : List<AnomaliaFormaPago>
    {
    }
}
