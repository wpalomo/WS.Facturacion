using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    #region JORNADA: Clase para entidad de Jornada.
    /// <summary>
    /// Estructura de una entidad Parte
    /// </summary>
    [Serializable]
    public class Jornada
    {

        public enum enmStatus
        {
            enmAbierta,
            enmCerrada,
            enmReabierta
        }

        public Estacion Estacion { get; set; }
        public DateTime Fecha { get; set; }
        public enmStatus Status { get; set; }
        public Usuario Validador { get; set; }
        public string Terminal { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Reaperturas { get; set; }

        public string StatusDesc
        {
            get
            {
                string desc = "";
                switch (Status)
                {
                    case enmStatus.enmAbierta:
                        desc = "Abierta";
                        break;
                    case enmStatus.enmCerrada:
                        desc = "Cerrada";
                        break;
                    case enmStatus.enmReabierta:
                        desc = "Reabierta";
                        break;
                    default:
                        break;
                }
                return desc;
            }
        }

        public string FechaString
        {
            get
            {
                return Fecha.ToString("dd/MM/yyyy");
            }
        }

    }

    /// <summary>
    /// Lista de objetos Jornada.
    /// </summary>
    /// 
    [Serializable]
    public class JornadaL : List<Jornada>
    {
    }
    #endregion
}
