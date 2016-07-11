using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;


namespace Telectronica.Facturacion
{

    #region CONFIGURACIONCLIENTEFACTURACION: Clase para entidad con las variables de entorno del cliente de facturacion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CONFIGURACIONCLIENTEFACTURACION
    /// </summary>*********************************************************************************************

    [Serializable]

    public class ConfiguracionClienteFacturacion
    {
        public static int MinimaEstacionAdministrativa = 51;

        public static int _MaximoItemsPorFactura = 10;           // Items que entran en una factura

        // Items que entran en una factura
        public int MaximoItemsPorFactura { get; set; }

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


        // Indica si todas las impresoras llevan la misma numeracion 
        public bool MismaNumeracion { get; set; }


        // Objeto de terminal de facturacion logica que esta asignada a la terminal fisica
        public TerminalFacturacion TerminalActual { get; set; }


        // Objeto del parte actual de la terminal logica
        public Tesoreria.Parte ParteActual { get; set; }


        //Fecha y hora que abrio la terminal (es null si está cerrada)
        public DateTime? FechaAperturaTerminal { get; set; }


        //Fecha y hora del servido r
        public DateTime FechayHoraServidor { get; set; }


        //Lista de permisos del cliente de facturación
        public PermisoL Permisos { get; set; }


        //Lector de Tarjetas Chip de la Terminal
        public LectorChip LectorTarjetasChip { get; set; }

        // Objeto Usuario que contiene los datos del vendedor que tiene parte abierto en la terminal
        public Usuario VendedorActual        
        {
            get
            {
                Usuario oVendedor = null;
                if (ParteActual != null)
                    oVendedor = ParteActual.Peajista;
                return oVendedor;
            }
        }


        // Booleano que indica si el usuario que esta logueado es el mismo que el que tiene asignada la estacion
        public bool esUsuarioResponsableTerminal
        { 
            get
                {   bool MismoUsuario = false;

                    if (ParteActual != null)
                    {
                        MismoUsuario = (UsuarioActual.ID == ParteActual.PeajistaID);
                    }

                    return MismoUsuario;
                }
        }

    }

    #endregion
}
