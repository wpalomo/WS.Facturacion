using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CONFIGURACIONCLIENTEGRAFICO: Clase para entidad con las variables de entorno del cliente grafico

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CONFIGURACIONCLIENTETORERIA
    /// </summary>*********************************************************************************************

    [Serializable]

    public class ConfiguracionClienteGrafico
    {
        /// <summary>
        /// Objeto Estacion que contiene los datos de la estacion actual
        /// </summary>
        public Estacion EstacionActual { get; set; }
        
        /// <summary>
        /// Objeto Usuario que contiene los datos del usuario actualmente logueado
        /// </summary>
        public Usuario UsuarioActual { get; set; }
        
        /// <summary>
        /// String que contiene el numero de version del sistema
        /// </summary>
        public string VersionCliente { get; set; }
        
        /// <summary>
        /// Nombre del servidor web al que se esta accediendo
        /// </summary>
        public string NombreServidorWeb { get; set; }
        
        /// <summary>
        /// Nombre de la pagina actual (usado por la asincronicidad de los timers, para saber si sigo en la misma pantalla o no)
        /// </summary>
        public string PaginaActual { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y la hora del servidor
        /// </summary>
        public DateTime FechayHoraServidor { get; set; }

        /// <summary>
        /// Lista de permisos del cliente gráfico
        /// </summary>
        public PermisoL Permisos { get; set; }

        /// <summary>
        /// Obtiene o establece la configuración general
        /// </summary>
        public GestionConfiguracion Gestionconfiguracion { get; set; }
    }

    #endregion
}
