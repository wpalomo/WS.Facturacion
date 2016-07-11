using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;


namespace Telectronica.Tesoreria
{
    #region PARTESTOTALES: Clase para entidad de totales de Partes por estado.
        /// <summary>
        /// Estructura de una entidad Parte
        /// </summary>
    [Serializable]
    public class PartesTotales
    {
        public enum enmStatus
        {
            enmFondoNoDevuelto,
            enmNoLiquidado,
            enmLiquidado,
            enmValidado

        }
        public DateTime Jornada { get; set; }
        public int Turno { get; set; }
        public bool EstaLiquidado { get; set; }
        public bool EstaValidado { get; set; }
        public int Cantidad { get; set; }
        public bool DevolvioFondo { get; set; }
        public string JornadaString
        {
            get
            {
                return Jornada.ToShortDateString();
            }

        }
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
                else if( !DevolvioFondo )
                {
                    status = enmStatus.enmFondoNoDevuelto;
                }

                return status;
            }
        }
    }

    /// <summary>
    /// Lista de objetos PartesTotales.
    /// </summary>
    /// 
    [Serializable]
    public class PartesTotalesL : List<PartesTotales>
    {
        public PartesTotalesEstadoL getTotalesEstado()
        {
            DateTime antJornada = new DateTime();
            int antTurno = 0;
            PartesTotalesEstadoL oTotalesEstado = new PartesTotalesEstadoL();
            oTotalesEstado.Total = new PartesTotalesEstado();
            PartesTotalesEstado oTot = null;
            foreach (PartesTotales item in this)
            {
                if (item.Jornada != antJornada || item.Turno != antTurno)
                {
                    if (oTot != null)
                        oTotalesEstado.Add(oTot);

                    antJornada = item.Jornada;
                    antTurno = item.Turno;
                    oTot = new PartesTotalesEstado();
                    oTot.Jornada = item.Jornada;
                    oTot.Turno = item.Turno;
                }

                switch (item.Status)
                {
                    case PartesTotales.enmStatus.enmFondoNoDevuelto:
                        oTot.NoLiquidados = item.Cantidad;
                        oTotalesEstado.Total.NoLiquidados += item.Cantidad;
                        break;
                    case PartesTotales.enmStatus.enmNoLiquidado:
                        oTot.NoLiquidados = item.Cantidad;
                        oTotalesEstado.Total.NoLiquidados += item.Cantidad;
                        break;
                    case PartesTotales.enmStatus.enmLiquidado:
                        oTot.Liquidados = item.Cantidad;
                        oTotalesEstado.Total.Liquidados += item.Cantidad;
                        break;
                    case PartesTotales.enmStatus.enmValidado:
                        oTot.Validados = item.Cantidad;
                        oTotalesEstado.Total.Validados += item.Cantidad;
                        break;
                }

            }
            if (oTot != null)
                oTotalesEstado.Add(oTot);

            return oTotalesEstado;
        }
    }
    #endregion

    #region PARTESTOTALESESTADO: Clase para entidad de totales de Partes con campos por cada estado.
    /// <summary>
    /// Estructura de una entidad Parte
    /// </summary>
    [Serializable]
    public class PartesTotalesEstado
    {
        public PartesTotalesEstado()
        {
            NoDevueltos = 0;
            NoLiquidados = 0;
            Liquidados = 0;
            Validados = 0;
        }

        public DateTime Jornada { get; set; }
        public int Turno { get; set; }
        public int NoDevueltos { get; set; }
        public int NoLiquidados { get; set; }
        public int Liquidados { get; set; }
        public int Validados { get; set; }
        public string JornadaString
        {
            get
            {
                return Jornada.ToShortDateString();
            }

        }
    }

    /// <summary>
    /// Lista de objetos PartesTotalesEstado.
    /// </summary>
    /// 
    [Serializable]
    public class PartesTotalesEstadoL : List<PartesTotalesEstado>
    {
        public PartesTotalesEstado Total { get; set; }
    }
    #endregion
}
