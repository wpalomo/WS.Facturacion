using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;

namespace Telectronica.Facturacion
{
    #region CAMBIOVENDEDOR: Clase para entidad de los cambios de vendedor de las terminales de facturacion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad CambioVendedor
    /// </summary>*********************************************************************************************

    [Serializable]

    public class CambioVendedor
    {
        // Constructor vacio
        public CambioVendedor()
        {
        }


        // Constructor con todos los datos
        public CambioVendedor(Estacion estacion, TerminalFacturacion terminalFacturacion, DateTime fechaInicio, 
                              DateTime? fechaFinal, Usuario supervisor, int identity, Parte parte)
        {
            this.Estacion = estacion;
            this.TerminalFacturacion = terminalFacturacion;
            this.FechaInicio = FechaInicio;
            this.FechaFinal = fechaFinal;
            this.Supervisor = supervisor;
            this.Identity = identity;
            this.Parte = parte;

        }



        // Estacion en la que se genera el cambio de Vendedor
        public Estacion Estacion { get; set; }


        // Terminal que se asigna el supervisor
        public TerminalFacturacion TerminalFacturacion { get; set; }


        // Fecha de inicio del cambio
        public DateTime FechaInicio { get; set; }


        // Fecha final del cambio (NULL cuando aun esta abierta)
        public DateTime? FechaFinal { get; set; }


        // Usuario que esta a cargo del turno
        public Usuario Supervisor { get; set; }


        // Identity del registro
        public int Identity { get; set; }


        // Parte asignado al vendedor
        public Parte Parte { get; set; }


        // Indica si se utilizo un parte anterior o es un nuevo parte
        public bool NuevoParte { get; set; }


        // Indica si la terminal del vendedor esta abierta o no (es una forma de encapsular la interpretacion de ciertos datos del parte)
        public bool TerminalAbierta
        {
            get
            {
                return (FechaFinal == null);
            }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos CambioVendedor
    /// </summary>*********************************************************************************************
    public class CambioVendedorL : List<CambioVendedor>
    {
    }


    #endregion
}
