using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Validacion
{
    #region Anomalia: Clase para entidad de Anomalia.

    /// <summary>
    /// Estructura de una entidad Anomalia
    /// </summary>

    [Serializable]
    public class Anomalia
    {
        public enum enmStatus
        {
            enmAceptacion,
            enmRechazo
        }

        public enum eAnomalia
        {
            enmVIOLAC_VIA_ABIERTA = 1,
            enmVIOLAC_SUBE_BARRERA = 2,
            enmVIOLAC_QUIEBRE = 3,
            enmVIOLAC_VIA_CERRADA = 4,
            enmDACS = 5,
            enmEXENTOS = 6,
            enmSIPS = 7,
            enmCANCELAC_TRANSITO = 8,
            enmCANCELAC_RECARGA = 9,
            enmCANCELAC_VENTA = 10,
            enmAUTORIZA_PASO = 11,
            enmPAGO_DIFERIDO = 12,
            enmTICKET_MANUAL = 13,
            enmNOTA_DEBITO = 14,
            enmPASE = 15,
            enmCANCELAC_OTROS = 16,
            enmRECARGA = 17,
            enmVENTA = 18,
            enmDACS_AFAVOR = 21,
            enmTAG_MANUAL = 22,
            enmVALES_PREPAGOS = 23,
            enmTRAN_NORMALES = 25
        }

        public int Codigo { get; set; }
        public String Descripcion { get; set; }
        public int Orden { get; set; }
        public enmStatus EstadoDefecto { get; set; }
        public String GeneradaPorValidacion { get; set; }
        public bool esGeneradaPorValidacion
        {
            get { return GeneradaPorValidacion == "S"; }
            set { GeneradaPorValidacion = value ? "S" : "N"; }
        }
        public int Total { get; set; }
        public int Pendientes { get; set; }

        public String StatusDesc
        {
            get
            {
                String desc = "";
                switch (EstadoDefecto)
                {
                    case enmStatus.enmAceptacion:
                        desc = "Aceptacion";
                        break;
                    case enmStatus.enmRechazo:
                        desc = "Rechazo";
                        break;
                    default:
                        break;
                }
                return desc;
            }
        }

        public Anomalia(Int16 codigo, String descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        public Anomalia()
        {

        }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }

    /// <summary>
    /// Lista de objetos Anomalia.
    /// </summary>
    /// 
    [Serializable]
    public class AnomaliaL : List<Anomalia>
    {
    }
    #endregion
}
