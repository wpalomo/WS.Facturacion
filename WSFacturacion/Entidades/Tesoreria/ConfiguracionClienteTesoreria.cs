using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region CONFIGURACIONClienteTesoreria: Clase para entidad con las variables de entorno del cliente de facturacion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CONFIGURACIONClienteTesoreria
    /// </summary>*********************************************************************************************
    [Serializable]
    public class ConfiguracionClienteTesoreria
    {
        /// <summary>
        /// Indica si liquidamos con o sin bolsa
        /// </summary>
        /// TODO si hay tesorero es sin bolsa
        public bool ConBolsa = true;

        /// <summary>
        /// Indica como agrupamos para depositar
        /// </summary>
        public enum enmAgrupacionDeposito
        {
            enmTurno,
            enmParte,
            enmBolsa,
            enmNada
        }

        public enmAgrupacionDeposito AgrupacionDepositoLocal = enmAgrupacionDeposito.enmNada;

        public static enmAgrupacionDeposito AgrupacionDeposito
        {
            get
            {
                ConfiguracionClienteTesoreria ct = new ConfiguracionClienteTesoreria();
                return ct.AgrupacionDepositoLocal;
            }
        }

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
        /// Fecha y hora del servido r
        /// </summary>
        public DateTime FechayHoraServidor { get; set; }
        
        /// <summary>
        /// Lista de permisos del cliente de tesoreria
        /// </summary>
        public PermisoL Permisos { get; set; }

        /// <summary>
        /// Indica la diferencia que puede haber entre lo liquidado y lo registrado por la vía para poder mostrar advertencia.
        /// </summary>
        public decimal DiferenciaLiquidacion { get; set; }

        /// <summary>
        /// Indica si la estacion esta configurada para aceptar retiros anticipados o no
        /// </summary>
        public bool PermiteRetirosAnticipados { get; set; }

        /// <summary>
        /// Moneda en la que se expresan los movimientos
        /// </summary>
        public Moneda MonedaVia { get; set; }
    }

    #endregion
}
