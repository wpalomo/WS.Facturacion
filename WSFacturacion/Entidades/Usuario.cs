using System;
using System.Collections.Generic;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class Usuario
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Usuario()
        {
        }

        /// <summary>
        /// Constructor con dos parámetros
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        public Usuario(string id, string nombre)
        {
            ID = id;
            Nombre = nombre;
            ID_Nombre = ID + " - " + Nombre;
        }

        #endregion

        /// <summary>
        /// Identificador único
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Perfil en la estacion donde estamos trabajando
        /// </summary>
        public Perfil PerfilActivo { get; set; }

        /// <summary>
        /// Password Hasheada
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string PasswordReal { get; set; }

        /// <summary>
        /// Indica el nombre
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Indica el Id Del nombre
        /// </summary>
        public string ID_Nombre { get; set; }

        /// <summary>
        /// Tarjeta
        /// </summary>
        public long? Tarjeta { get; set; }

        /// <summary>
        /// Perfil en Gestion
        /// </summary>
        public Perfil PerfilGestion { get; set; }

        public Zona ZonaPrincipal { get; set; }

        public Zona ZonaHabitual { get; set; }

        public DateTime? FechaEgreso { get; set; }

        public string FechaEgresoString
        {
            get
            {
                string sFecha = "";
                if (FechaEgreso != null)
                    sFecha = ((DateTime)FechaEgreso).ToString("dd/MM/yyyy");
                return sFecha;
            }
        }

        public DateTime? UltimoAcceso { get; set; }
        
        public DateTime UltimoCambioPassword { get; set; }

        /// <summary>
        /// Baja Logica
        /// </summary>
        public bool Eliminado { get; set; }

        /// <summary>
        /// Obtiene o establece un objeto que representa el tipo de personal
        /// </summary>
        public TipoPersonal TipoPersonal { get; set; }
        
        /// <summary>
        /// Debe actualiza clave?
        /// Si la clave es vacia, igual al usuario o la fecha de vencimiento ya paso
        /// </summary>
        /// <returns></returns>
        public bool DebeActualizarClave()
        {
            return (PasswordReal.Trim() == "") 
                || PasswordReal.ToUpper() == ID.ToUpper()
                || (DiasVencimiento > 0  &&
                    UltimoCambioPassword.AddDays(DiasVencimiento) < System.DateTime.Now);
        }
        
        /// <summary>
        /// Perfiles por estacion
        /// </summary>
        public UsuarioEstacionL EstacionesHabilitadas { get; set; }

        /// <summary>
        /// Es el usuario maestro?
        /// </summary>
        /// <returns></returns>
        public bool EsUsuarioMaestro()
        {
           return (ID == "masteru");
        }

        /// <summary>
        /// Si el usuario es local para la estacion que se pidio
        /// </summary>
        public bool EsLocal { get; set; }

        /// <summary>
        /// Dias de vencimiento de password (de configuracion de password)
        /// </summary>
        public int DiasVencimiento { get; set; }

        /// <summary>
        /// Para las grillas
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Nombre;
        }

        /// <summary>
        /// Obtiene un valor que indica si el usuario es el Supervisor a Cargo
        /// </summary>
        public string EsSupervisorACargo
        {
            get;
            set;
        }

        // Nombre corto que se utilizara en los recibos de las vias (Tickets)
        public string NombreCorto { get; set; }

    }

    [Serializable]    
    public class UsuarioL : List<Usuario>
    {  
    }
}
