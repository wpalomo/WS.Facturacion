using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;

namespace Telectronica.Validacion
{
    #region CONFIGVALIDACION: Clase para entidad de Configuracion de Validacion.
    /// <summary>
    /// Estructura de una entidad Parte
    /// </summary>
    [Serializable]
    public class ConfigValidacion
    {
        /// <summary>
        /// Indica si validamos centralizado o no
        /// </summary>
        public static bool ValidacionCentralizada = true;

        public static bool CierreJornadaPorEstacion = true;

        public bool esValidacionCentralizada { get; set;}

        public bool esCierreJornadaPorEstacion { get; set; }

        public bool esValidacionLocal { get; set; }

        // Objeto Estacion que contiene los datos de la estacion actual
        public Estacion EstacionActual { get; set; }

        // Objeto Usuario que contiene los datos del usuario actualmente logueado
        public Usuario UsuarioActual { get; set; }

        // String que contiene el numero de version del sistema
        public string VersionCliente { get; set; }

        // Nombre del servidor web al que se esta accediendo
        public string NombreServidorWeb { get; set; }

        // Nombre de la pagina actual (usado por la asincronicidad de los timers, para saber si sigo en la misma pantalla o no)
        public string PaginaActual { get; set; }

        //Fecha y hora del servidor
        public DateTime FechayHoraServidor { get; set; }

        //Lista de permisos del cliente de facturación
        public PermisoL Permisos { get; set; }

        /// <summary>
        /// Indica el estado del parte para dar error en los listados
        /// </summary>
        public static string getEstadoPartePendiente(bool esGST)
        {
            string tipoParte = "";
            if( ValidacionCentralizada || esGST)
                tipoParte = "V";
            else
                tipoParte = "N";

            return tipoParte;
        }
        
        public static bool getEsValidacionLocal(bool esGST)
        {
            if (ConfigValidacion.ValidacionCentralizada)
            {
                return esGST;
            }
            else
            {
                return !esGST;
            }
        }

        /// <summary>
        /// Indica el estado del parte para poder imprimirlo
        /// </summary>
        public static string getEstadoParteOK(bool esGST)
        {
            string tipoParte = "";
            if (!ValidacionCentralizada || esGST)
                tipoParte = "V";
            else
                tipoParte = "L";

            return tipoParte;
        }

        public static string getEstadoParteOK(Parte.enmStatus status)
        {
            string tipoParte = "";
            if (status == Parte.enmStatus.enmValidado)
                tipoParte = "V";
            if (status == Parte.enmStatus.enmLiquidado)
                tipoParte = "L";
            return tipoParte;
        }

        /// <summary>
        /// Indica el estado del parte para poder imprimirlo
        /// </summary>
        public static Parte.enmStatus getStatusParteOK(bool esGST)
        {
            Parte.enmStatus tipoParte = Parte.enmStatus.enmValidado;
            if (!ValidacionCentralizada || esGST)
                tipoParte = Parte.enmStatus.enmValidado;
            else
                tipoParte = Parte.enmStatus.enmLiquidado;

            return tipoParte;
        }

        /// <summary>
        /// Indica la descripcion de estado necesario
        /// </summary>
        public static string getDescripcionStatusParteOK(bool esGST)
        {
            string tipoParte = "";
            if (!ValidacionCentralizada || esGST)
                tipoParte = "liquidado y validado";
            else
                tipoParte = "liquidado";

            return tipoParte;
        }



    }
    #endregion
}
