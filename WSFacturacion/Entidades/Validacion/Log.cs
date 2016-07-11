using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    #region Log: Clase para entidad de Log.
    /// <summary>
    /// Estructura de una entidad Parte
    /// </summary>
    [Serializable]
    public class Log
    {
        public int numero { get; set; }
        public string estacion{ get; set; }
        public DateTime fechaJornada { get; set; }
        public string tipo{ get; set; }
        public DateTime fecha{ get; set; }
        public string host { get; set; }
        public string usuario { get; set; }
    }

    /// <summary>
    /// Lista de objetos Log.
    /// </summary>
    /// 
    [Serializable]
    public class LogL : List<Log>
    {

    }

    #endregion
}
