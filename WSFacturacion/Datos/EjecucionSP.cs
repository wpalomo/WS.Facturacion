using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Clase para centralizar el retorno de la ejecucion de los Stored Procedures.
    /// </summary>
    ///****************************************************************************************************

    public class EjecucionSP
    {

        // Enumerado con los codigos de retorno de los SP    
        public enum enmErrorSP
        {
            enmOK = 0,
            enmFALTA_PARAMETRO = -101,
            enmINCONSISTENCIA = -102,
            enmBAJA_LOGICA = -103
        }
    }
}
