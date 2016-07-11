using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region ACCIONES DE SUPERVISION REMOTA
    /// <summary>
    /// Estructura de una entidad Accion
    /// </summary>
 
    [Serializable]
    public class Accion
    {
        
        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de un alarma en particular
        /// </summary>
        /// ***********************************************************************************************

        public Accion()
        { 
        }

        // Identity
        public int ID { get; set; }

        // Estacion
        public int Estacion { get; set; }

        // Via
        public int Via { get; set; }

        // Fecha Generación
        public DateTime FechaGen { get; set; }

        // Modo
        public string ModoDescr { get; set; }

        // Modo Desripcion
        public string Modo
        {
            get
            {
                switch (ModoDescr)
                {
                    case "Manual":
                        return "M";
                    case "Manual-Dinámica":
                        return "MD";
                    case "Dinámica":
                        return "D";
                    default:
                        return "";
                }
            }

            set
            {
                switch (Convert.ToString(value).Trim())
                {
                    case "M":
                        ModoDescr = "Manual";
                        break;
                    case "MD":
                        ModoDescr = "Manual-Dinámica";
                        break;
                    case "D":
                        ModoDescr = "Dinámica";
                        break;
                    default:
                        ModoDescr = "";
                        break;
                }
            }
        }
        
        // Tipo de Alarma
        public int TipoAlarma { get; set; }

        // Descripcion de Tipo de Alarma
        public string TipoAlarmaDescr { get; set; }

        // Descripcion de Estado Color
        public string EstadoColorDescr { get; set; }

        // Estado Color
        public string EstadoColor
        {
            get
            {
                switch (EstadoColorDescr)
                {
                    case "Amarillo":
                        return "C";
                    case "Rojo":
                        return "P";
                    case "Verde":
                        return "A";
                    case "Gris":
                        return "F";
                    case "Naranja":
                        return "S";
                    default:
                        return "";
                }
            }          

        }

        // Descripción de Estado
        public string EstadoDescr { get; set; }
        
        // Estado
        public string Estado
        {
            get
            {
                switch (EstadoDescr)
                {
                    case "En Curso":
                        return "C";
                    case "Pendiente":
                        return "P";
                    case "Autorizado":
                        return "A";
                    case "Finalizado":
                        return "F";
                    case "Siguio":
                        return "S";
                    default:
                        return "";
                }
            }

            set
            {
                switch (Convert.ToString(value).Trim())
                {
                    case "C":
                        EstadoColorDescr = "Amarillo";
                        EstadoDescr = "En Curso";
                        break;
                    case "P":
                        EstadoColorDescr = "Rojo";
                        EstadoDescr = "Pendiente";
                        break;
                    case "A":
                        EstadoColorDescr = "Verde";
                        EstadoDescr = "Autorizado";
                        break;
                    case "F":
                        EstadoColorDescr = "Gris";
                        EstadoDescr = "Finalizado";
                        break;
                    case "S":
                        EstadoColorDescr = "Naranja";
                        EstadoDescr = "Siguio";
                        break;
                    default:
                        EstadoColorDescr = "";
                        EstadoDescr = "Finalizado";
                        break;
                }
            }

        }

        // Usuario que autorizó
        public string UsuarioAutorizo { get; set; }

        // Terminal que autorizó
        public string TerminalAutorizo { get; set; }

        // Fecha y hora que autorizó
        public DateTime FechaAutorizo { get; set; }

        // Subforma de Pago
        public int SubFp { get; set; }

        // Patente 
        public string Patente { get; set; }

        // Número de Tag
        public string NumeroTag { get; set; }

        // Descripcion de Categoria
        public string CategoriaDescr { get; set; }

        // Categoria
        public int Categoria { get; set;}

        // Codigo de Causa
        public int CodigoCausa { get; set; }

        // Causa
        public string Causa { get; set; }

        // Comentario
        public string Comentario { get; set; }
        
        // Fecha Finalización
        public DateTime FechaFin { get; set; }
        
        // Causa Finalización
        public string CausaFin { get; set; }

        // ID Alarma en ALAVIA
        public int IDAlarma { get; set; }


        public int CategoriaDAC { get; set; }

        public string NombreFoto { get; set; }

        public int Numtran { get; set; }

        public string DescrForpag { get; set; }

        public string Emisor { get; set; }

        public string tipop { get; set; }

        public string tipbo { get; set; }

        public bool SinTag { get; set; }
    }


        [Serializable]

        /// <summary>
        /// Lista de objetos Accion.
        /// </summary>
    public class AccionL : List<Accion>
        {
        }
    
    #endregion
}