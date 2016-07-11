using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region TERMINALFACTURACIONFISICA: Clase para entidad de las Terminales Fisicas de Facturacion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TerminalFacturacionFisica
    /// </summary>*********************************************************************************************

    [Serializable]

    public class TerminalFacturacionFisica
    {
        // Constructor vacio
        public TerminalFacturacionFisica()
        {
        }


        // Constructor con los datos
        public TerminalFacturacionFisica(string nombreTerminal)
        {
            this.NombreTerminalFisica = nombreTerminal;
        }


        // Nombre (o IP) de la terminal fisica que corresponde a esta terminal logica
        public string NombreTerminalFisica { get; set; }


 
    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TerminalFacturacionFisica
    /// </summary>*********************************************************************************************
    public class TerminalFacturacionFisicaL : List<TerminalFacturacionFisica>
    {
    }

    #endregion
}
