using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace Telectronica.Facturacion
{
    #region TERMINALFACTURACION: Clase para entidad de las Terminales de Facturacion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TerminalFacturacion
    /// </summary>*********************************************************************************************

    [Serializable]
    public class TerminalFacturacion
    {
    
        // Constructor Vacio
        public TerminalFacturacion()
        {
        }
    
        // Constructor Vacio
        public TerminalFacturacion(Estacion estacion, Int16 numero, string descripcion, Impresora impresoraFacturacion)
        {
            this.Estacion = estacion;
            this.Numero = numero;
            this.Descripcion = descripcion;
            this.ImpresoraFacturacion = impresoraFacturacion;
        }
    

        // Estacion en la que se define la terminal de facturacion
        public Estacion Estacion { get; set; }


        // Numero de terminal
        public Int16 Numero { get; set; }


        // Descripcion de la terminal
        public string Descripcion { get; set; }


        // Terminal Fisica
        public TerminalFacturacionFisica TerminalFisica { get; set; }


        // Impresora de facturacion que se le asigna a esta terminal
        public Impresora ImpresoraFacturacion { get; set; }

        // Lector chip 
        public LectorChip LectorChip { get; set; }

        // Metodo para usar en la grilla el numero de terminal de facturacion
        public string ImpresoraString
        {
            get
            {
                return ImpresoraFacturacion.Codigo.ToString();
            }
        }

        //Imprime Fallos
        public string ImprimeFallos { get; set; }

        public string ImpresoraDescription
        {
            get
            {
                string Named = "No";
                if (ImprimeFallos != null && ImprimeFallos.Trim() == "S")
                    Named = "Si";
                return Named;
            }
        }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TerminalFacturacion
    /// </summary>*********************************************************************************************
    public class TerminalFacturacionL : List<TerminalFacturacion>
    {
    }

    #endregion
}
