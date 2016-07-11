using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region VIASTOTALES: Clase para entidad del total de vias abiertas

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViasTotales
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "ViasTotales", IsNullable = false)]

    public class ViasTotales
    {
        public enum enmStatus
        {
            enmAbierta,
            enmCerrada,
            enmDesconocido
        }
        public string Modo { get; set; }
        public string ModoDesc { get; set; }
        public string Sentido { get; set; }
        public enmStatus Estado { get; set; }
        public int Cantidad { get; set; }

        public string EstadoString
        {
            get
            {
                string desc = "";
                switch (Estado)
                {
                    case enmStatus.enmAbierta:
                        desc = "A";
                        break;
                    case enmStatus.enmCerrada:
                        desc = "C";
                        break;
                    case enmStatus.enmDesconocido:
                        desc = "D";
                        break;
                    default:
                        desc = "D";
                        break;
                }
                return desc;
            }
            set
            {
                switch (value)
                {
                    case "A":
                        Estado = enmStatus.enmAbierta;
                        break;
                    case "C":
                        Estado = enmStatus.enmCerrada;
                        break;
                    default:
                        Estado = enmStatus.enmDesconocido;
                        break;
                }
            }
        }
        public string SentidoDesc 
        {
            get
            {
                string desc = "";
                switch (Sentido)
                {
                    case "A":
                        desc = "Ascendente";
                        break;
                    case "D":
                        desc = "Descendente";
                        break;
                    default:
                        desc = "Desconocido";
                        break;
                }

                return desc;
            }
        }

        public string EstadoDesc
        {
            get
            {
                string desc = "";
                switch (Estado)
                {
                    case enmStatus.enmAbierta:
                        desc = "Abierta";
                        break;
                    case enmStatus.enmCerrada:
                        desc = "Cerrada";
                        break;
                    case enmStatus.enmDesconocido:
                        desc = "Desconocido";
                        break;
                    default:
                        desc = "Desconocido";
                        break;
                }
                return desc;
            }
        }

    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos ViasTotales
    /// </summary>*********************************************************************************************
    [Serializable]
    public class ViasTotalesL : List<ViasTotales>
    {
        public ViasTotalesSentidoL getTotalesSentido(ViasTotales.enmStatus? status)
        {
            string antModo = "";
            ViasTotalesSentidoL oTotalesSentido = new ViasTotalesSentidoL();
            oTotalesSentido.Total = new ViasTotalesSentido();
            ViasTotalesSentido oTot = null;
            foreach (ViasTotales item in this)
	        {
                if( status == null || item.Estado == status )
                {
                    if( item.Modo != antModo )
                    {
                        if( oTot != null )
                            oTotalesSentido.Add(oTot);

                        antModo = item.Modo;
                        oTot = new ViasTotalesSentido();
                        oTot.Modo = item.Modo;
                        oTot.ModoDesc = item.ModoDesc;
                    }

                    if (item.Sentido == "A")
                    {
                        oTot.Ascendente = item.Cantidad;
                        oTotalesSentido.Total.Ascendente += item.Cantidad;
                    }
                    else if (item.Sentido == "D")
                    {
                        oTot.Descendente = item.Cantidad;
                        oTotalesSentido.Total.Descendente += item.Cantidad;
                    }
                }        		 
	        }
            if( oTot != null )
                oTotalesSentido.Add(oTot);

            return oTotalesSentido;
        }
    }

    [Serializable]
    [XmlRootAttribute(ElementName = "ViasTotalesSentido", IsNullable = false)]

    public class ViasTotalesSentido
    {
        public ViasTotalesSentido()
        {
            Ascendente = 0;
            Descendente = 0;
        }
        public string Modo { get; set; }
        public string ModoDesc { get; set; }
        public int Ascendente { get; set; }
        public int Descendente { get; set; }
    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos ViasTotalesSentido
    /// </summary>*********************************************************************************************
    [Serializable]
    public class ViasTotalesSentidoL : List<ViasTotalesSentido>
    {
        public ViasTotalesSentido Total { get; set; }
    }

    #endregion
}
