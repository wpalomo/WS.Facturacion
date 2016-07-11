using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Validacion
{
    public class CodigoValidacion
    {
        #region CodigoValidacion: Clase para entidad de Codigos de Validacion.

        public Anomalia Anomalia { get; set; }

        /// <summary>
        /// Tipo de Aceptacion ("A","R","")
        /// </summary>
        public String Tipo { get; set; }

        public String TipoDesc
        {
            get { return Tipo == "A" ? "Aceptación" : (Tipo == "R" ? "Rechazo" : ""); }
        }

        /// <summary>
        /// Codigo de Aceptacion
        /// </summary>
        public int? Codigo { get; set; }

        /// <summary>
        /// Descripcion de Aceptacion
        /// </summary>
        public String Descripcion { get; set; }

        public string FormaPago { get; set; }
        public string MedioPago { get; set; }
        public short? SubformaPago { get; set; }
        public string DescripcionFormaPago { get; set; }
        public int? TipoTarifa { get; set; }

        public CodigoValidacion(Anomalia anomalia, String tipo, Int16? codigo, String descripcion, string formaPago, string medioPago, Int16? subformaPago, string descripcionFormaPago)
        {
            this.Anomalia = anomalia;
            this.Tipo = tipo; 
            this.Codigo = codigo;
            this.Descripcion = descripcion;                        
            this.FormaPago = formaPago;
            this.MedioPago = medioPago;
            this.SubformaPago = subformaPago;
            this.DescripcionFormaPago = descripcionFormaPago;
        }

        // Se usan?
        public String OtrosFaltantes { get; set; }
        public String CodigoInvisible { get; set; }
        public String PorDefecto { get; set; }
        public String HabilitadoParaDefecto { get; set; }
        
        public bool esPorDefecto
        {
            get { return PorDefecto == "S"; }
            set { PorDefecto = value ? "S" : "N"; }
        }

        public bool estaHabilitadoParaDefecto
        {
            get { return HabilitadoParaDefecto == "S"; }
            set { HabilitadoParaDefecto = value ? "S" : "N"; }
        }

        public bool esCodigoInvisible
        {
            get { return CodigoInvisible == "S"; }
            set { CodigoInvisible = value ? "S" : "N"; }
        }

        public bool esOtrosFaltantes
        {
            get { return OtrosFaltantes == "S"; }
            set { OtrosFaltantes = value ? "S" : "N"; }
        }

        public CodigoValidacion(String tipo, Int16? codigo, String descripcion, Anomalia anomalia, String otrosFaltantes, String codigoInvisible, String porDefecto, String habParaDefecto)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            this.Tipo = tipo;
            this.Anomalia = anomalia;
            this.OtrosFaltantes = otrosFaltantes;
            this.CodigoInvisible = codigoInvisible;
            this.PorDefecto = porDefecto;
            this.HabilitadoParaDefecto = habParaDefecto;
        }

        public CodigoValidacion(String tipo, Int16? codigo, String descripcion, Anomalia anomalia)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            this.Tipo = tipo;
            this.Anomalia = anomalia;
        }

        public CodigoValidacion()
        {

        }

        public String CodigoInvisibleDesc
        {
            get
            {
                string desc = "";
                if (esCodigoInvisible)
                    desc = "Sí";
                else
                    desc = "No";
                return desc;
            }
        }

        public String OtrosFaltantesDesc
        {
            get
            {
                string desc = "";
                if (esOtrosFaltantes)
                    desc = "Sí";
                else
                    desc = "No";
                return desc;
            }
        }

        public String PorDefectoDesc
        {
            get
            {
                string desc = "";
                if (esPorDefecto)
                    desc = "Sí";
                else
                    desc = "No";
                return desc;
            }
        }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }

        #endregion
    }

    /// <summary>
    /// Lista de objetos CodigoValidacion.
    /// </summary>
    /// 
    [Serializable]
    public class CodigoValidacionL : List<CodigoValidacion>
    {
    }
}
