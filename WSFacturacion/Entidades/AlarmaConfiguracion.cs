using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region ALARMA CONFIGURACION: Clase para entidad de Configuracion de Alarmas.
    /// <summary>
    
    /// </summary>
    /// 

    [Serializable]

    public class AlarmaConfiguracion
    {
        // Tipo Alarma
        public String TipoAlarma { get; set; }

        // Duración de la Sirena
        public Int16 DuracionSirena { get; set; }

        // Duración Alarma Visual
        public Int16 DuracionAlarmaVisual { get; set; }

        // Tipo de Sonido
        public Int16 TipoSonido { get; set; }

        // Anulacion Por Teclado
        public String AnulacionTeclado { get; set; }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Feriado.
    /// </summary>
    public class AlarmaConfiguracionL : List<AlarmaConfiguracion>
    {
    }

    #endregion

}
