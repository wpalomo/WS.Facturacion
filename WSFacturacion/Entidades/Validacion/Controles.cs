using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    #region Controles: Clase para entidad de Controles_Gral.

    /// <summary>
    /// Estructura de una entidad Controles
    /// </summary>

    [Serializable]
    public class Controles
    {
        public int? Id {get; set;}
        public int Prioridad {get; set;}
        public Estacion Estacion {get; set;}
        public FormaPagoValidacion FormaPago{get; set;}
		public int? Porcentaje {get; set;}
        public CodigoValidacion CodigoValidacion{get; set;}
		public TipoValidacion TipoValidacion {get; set;}        
        public int? PorcentajeValidacion { get; set; }
        public CategoriaManual Categoria { get; set; }
        public String CategoriaDesc { get { return Categoria != null ? Categoria.Descripcion : "Todas"; } }
        public CategoriaManual CategoriaDac { get; set; }
        public String CategoriaDacDesc { get { return CategoriaDac != null ? CategoriaDac.Descripcion : "Todas"; } }
        public String TipoValidacionDesc { get { return TipoValidacion != null ? TipoValidacion.Descripcion : ""; } }
        public String EstacionDesc { get { return Estacion.Nombre != "" ? Estacion.Nombre.ToString() : "Todas"; } }
        public String AnomaliaDesc { get { return CodigoValidacion.Anomalia.Descripcion; } }
        public String CodigoValidacionTipo { get { return CodigoValidacion.TipoDesc; } }
        public String FormaPagoDesc { get { return FormaPago.Descripcion!="" ? FormaPago.Descripcion : "Todas"; } }

        public Controles(int? id, int prioridad, Estacion estacion, FormaPagoValidacion formaPago, int? porcentaje, CodigoValidacion codValidacion, TipoValidacion tipoVal, CategoriaManual categoria, int? porcentajeValidacion)
        {
            this.Id = id;
            this.Prioridad = prioridad;
            this.Estacion = estacion;
            this.FormaPago = formaPago;
            this.Porcentaje = porcentaje;
            this.CodigoValidacion = codValidacion;
            this.TipoValidacion = tipoVal;           
            this.Categoria = categoria;
            this.PorcentajeValidacion = porcentajeValidacion;

        }

        public Controles()
        {
        }
    }

    /// <summary>
    /// Lista de objetos Controles.
    /// </summary>
    /// 
    [Serializable]
    public class ControlesL : List<Controles>
    {
    }

    [Serializable]
    public class TipoValidacionInvisible
    {
        public Anomalia Anomalia { get; set; }
        public String Tipo { get; set; }
        public String Invisible { get; set; }

        public String TipoDesc { get { return Tipo == "A" ? "Aceptación" : (Tipo == "R" ? "Rechazo" : "Todos"); } }

        public bool EsInvisible { 
            get { return Invisible == "S"; }
            set { Invisible = value ? "S" : "N"; }
        }

        public TipoValidacionInvisible(Anomalia anomalia, String tipo, string invisible)
        {
            this.Anomalia = anomalia;
            this.Tipo = tipo;
            this.Invisible = invisible;
        }

        public TipoValidacionInvisible()
        {
        }
    }

    /// <summary>
    /// Lista de objetos TipoValidacionInvisible.
    /// </summary>
    /// 
    [Serializable]
    public class TipoValidacionInvisibleL : List<TipoValidacionInvisible>
    {
    }

    [Serializable]
    public class TipoValidacion
    {       
        public String Codigo { get; set; }
        public String Descripcion { get; set; }

        public TipoValidacion(String codigo, String descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        public TipoValidacion()
        {
        }
       
    }

    /// <summary>
    /// Lista de objetos TipoValidacion.
    /// </summary>
    /// 
    [Serializable]
    public class TipoValidacionL : List<TipoValidacion>
    {
    }

    #endregion
}
