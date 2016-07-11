using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Telectronica.Tesoreria
{
    #region PARTE: Clase para entidad de Parte de Trabajo.

    /// <summary>
    /// Estructura de una entidad Parte
    /// </summary>
    [Serializable]
    [XmlRootAttribute(ElementName = "Parte", IsNullable = false)]
    public class Parte
    {
        public enum enmStatus
        {
            enmNoLiquidado,
            enmLiquidado,
            enmValidado            
        }
        public Parte()
        {
        }
        public Parte(int numero, DateTime jornada, int turno)
        {
            Numero = numero;
            Jornada = jornada;
            Turno = turno;
        }
        public Parte(int numero, DateTime jornada, int turno, bool jornadaCerrada)
        {
            Numero = numero;
            Jornada = jornada;
            Turno = turno;
            JornadaCerrada = jornadaCerrada;
        }
        public int Numero { get; set; }
        public DateTime Jornada { get; set; }
        public bool JornadaCerrada { get; set; }
        public int Turno { get; set; }
        public Usuario Peajista { get; set; }
        public Usuario Validador { get; set; }
        public Estacion Estacion { get; set; }
        public int Nivel { get; set; }
        public DateTime Apertura { get; set; }
        public DateTime Liquidacion { get; set; }
        public bool EstaLiquidado { get; set; }
        public bool EstaValidado { get; set; }
        public bool ModoMantenimiento { get; set; }
        public string HoraI { get; set; }
        public string HoraF { get; set; }
        public string ComentarioEliminacion { get; set; }
        public Byte? Via { get; set; }
        public string ViaNombre { get; set; }
        public int? Bloque { get; set; }
        public DateTime? AperturaBloque { get; set; }
        public DateTime? CierreBloque { get; set; }
        public int? Transito { get; set; }
        public string Observacion { get; set; }
        public string FondodeCambio { get; set; }
        public string Vias { get; set; }
        public enmStatus Status 
        {
            get
            {
                enmStatus status = enmStatus.enmNoLiquidado;
                if (EstaLiquidado)
                {
                    if (EstaValidado)
                    {
                        status = enmStatus.enmValidado;
                    }
                    else
                    {
                        status = enmStatus.enmLiquidado;
                    }
                }

                return status;
            }
        }
        public bool DevolvioFondo { get; set; }
        public string StatusDesc
        {
            get
            {
                string desc="";
                switch (Status)
                {
                        //TODO Traduccion mas automatica
                    case enmStatus.enmNoLiquidado:
                        desc = "Não Declarado";
                        break;
                    case enmStatus.enmLiquidado:
                        desc = "Declarado";
                        break;
                    case enmStatus.enmValidado:
                        desc = "Validado";
                        break;
                }
                return desc;
            }
        }
        public string JornadaString
        {
            get
            {
                return Jornada.ToString("dd/MM/yyyy");
            }
        }
        public string PeajistaID
        {
            get
            {
                return Peajista.ID;
            }
        }
        public string TurnoString
        {
            get
            {
                return HoraI + " " + HoraF;
            }
        }
    }

    /// <summary>
    /// Lista de objetos Parte.
    /// </summary>
    /// 
    [Serializable]
    public class ParteL : List<Parte>
    { 
    }

    #endregion
}
