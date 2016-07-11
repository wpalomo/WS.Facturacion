using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region Comando:

    /// <summary>
    /// Estructura de una entidad Comando. Son los comandos enviados a las vías
    /// </summary>    
    [Serializable]
    [XmlRootAttribute(ElementName = "Comando", IsNullable = false)]    
    public class Comando
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Comando()
        {
        }

        /// <summary>
        /// Constructor con los parametros de todos los atributos de la clase
        /// </summary>
        /// <param name="xdFechaPedido"></param>
        /// <param name="xsTipo"></param>
        /// <param name="xsCodigo"></param>
        /// <param name="xiEstacion"></param>
        /// <param name="xiNumeroVia"></param>
        /// <param name="xsUsuario"></param>
        /// <param name="xdFechaEjecucion"></param>
        /// <param name="xdFechaVencimiento"></param>
        /// <param name="xsStatus"></param>
        /// <param name="xsParametros"></param>
        /// <param name="xsTipoDesc"></param>
        /// <param name="xsCodigoDesc"></param>
        /// <param name="xsUsuarioDesc"></param>
        /// <param name="xsParametrosDesc"></param>
        /// <param name="xiSegundosTolerancia"></param>
        public Comando(DateTime? xdFechaPedido, string xsTipo, string xsCodigo, int xiEstacion, int xiNumeroVia, string xsUsuario, DateTime? xdFechaEjecucion, 
                        DateTime? xdFechaVencimiento, string xsStatus, string xsParametros, string xsTipoDesc, string xsCodigoDesc, string xsUsuarioDesc, string xsParametrosDesc, int xiSegundosTolerancia)
        {
            this.FechaPedido = xdFechaPedido;
            this.Tipo = xsTipo;
            this.TipoDesc = xsTipoDesc;
            this.Codigo = xsCodigo;
            this.CodigoDesc = xsCodigoDesc;
            this.Estacion = xiEstacion;
            this.NumeroVia = xiNumeroVia;
            this.Usuario = xsUsuario;
            this.UsuarioDesc = xsUsuarioDesc;
            this.FechaEjecucion = xdFechaEjecucion;
            this.FechaVencimiento = xdFechaVencimiento;
            this.Status = xsStatus;
            this.Parametros = xsParametros;
            this.ParametrosDesc = xsParametrosDesc;
            this.SegundosTolerancia = xiSegundosTolerancia;

        }
        
        #endregion

        public DateTime? FechaPedido { get; set; }          // FECHA Y HORA EN QUE SE PIDIO EL COMANDO
        /// <summary>
        /// TIPO DE COMANDO
        /// </summary>
        public string Tipo { get; set; }
        /// <summary>
        /// DESCRIPCION DEL TIPO DE COMANDO
        /// </summary>
        public string TipoDesc { get; set; }
        /// <summary>
        /// CODIGO DEL COMANDO
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// DESCRIPCION DEL CODIGO DEL COMANDO
        /// </summary>
        public string CodigoDesc { get; set; }
        public int Estacion { get; set; }                   // ESTACION
        public int NumeroVia { get; set; }                  // NRO. DE VIA
        public string Usuario { get; set; }                 // USUARIO SOLICITANTE
        public string UsuarioDesc { get; set; }             // NOMBRE DEL USUARIO SOLICITANTE
        public DateTime? FechaEjecucion { get; set; }       // FECHA Y HORA DE EJECUCION DEL COMANDO
        public DateTime? FechaVencimiento { get; set; }     // FECHA Y HORA DE VENCIMIENO DEL COMANDO. null = NO VENCE
        public string Status { get; set; }                  // ESTADO DEL COMANDO. P=PENDIENTE, E=EJECUTADO, V=VENCIDO, S=SUPERADO
        /// <summary>
        /// PARAMETROS (SEPARADOS POR |)
        /// </summary>
        public string Parametros { get; set; }
        /// <summary>
        /// DESCRIPCION PARAMETROS (SEPARADOS POR -)
        /// </summary>
        public string ParametrosDesc { get; set; }
        public int SegundosTolerancia { get; set; }         // Tolerancia de vencimiento del comando.
        /// <summary>
        /// Nombre de la vía.
        /// </summary>
        public string NombreVia { get; set; }
        /// <summary>
        /// Obtiene o establece los comentarios referidos al comando
        /// </summary>
        public string Comentario { get; set; }
    }

    [Serializable]
    [XmlRootAttribute(ElementName = "ComandoL", IsNullable = false)]
    public class ComandoL : List<Comando>
    {
    }

    #endregion
}
