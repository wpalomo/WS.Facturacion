using System;
using System.Linq;
using System.Text;
using Telectronica.Facturacion;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    [Serializable]
    [XmlRootAttribute(ElementName = "InfoEvento", IsNullable = false)]
    public class InfoEvento
    {

        public InfoEvento(DateTime xFecha, string xVideoFoto1, string xVideoFoto2, string xTipoEvento, string xDescTipoEvento,
                            string xManual, string xDac, string xSentido, string xFormaPagoCorta, string xExento, string xBloque,
                            string xTicket, string xOperador, string xNroTransito, string xEjes, string xDobleEje, string xAltura,
                            string xMatricula, string xObservaciones, string xCodEst, string xNuvia, string xIdent, string xCodEve, 
                            string xFormaPagoInicial, string xSubTipoFormaPago, string xFormaPagoDesc, string xExentoDesc,
                            string xCtaAgrupacion, string xNroTurno, string xIdUsuario, string xNombreVia, string xCupom, 
                            string xSentidoCardinal, string xEjesLevantados)
        {

            this.Fecha = xFecha;
            this.NombreVia = xNombreVia;
            this.VideoFoto1 = xVideoFoto1;
            this.VideoFoto2 = xVideoFoto2;
            this.TipoEvento = xTipoEvento;
            this.DescTipoEvento = xDescTipoEvento;
            this.Manual = xManual;
            this.Dac = xDac;
            this.Sentido = xSentido;
            this.SentidoCardinal = xSentidoCardinal;
            
            this.FormaPagoCorta = xFormaPagoCorta;

            this.Exento = xExento;
            this.Cupom = xCupom;

            this.Bloque = xBloque;
            this.Ticket = xTicket;
            this.Operador = xOperador;
            this.NroTransito = xNroTransito;
            this.Ejes = xEjes;
            this.DobleEje = xDobleEje;
            this.Altura = xAltura;
            this.EjesLevantados = xEjesLevantados;

            this.Matricula = xMatricula;
            this.Observaciones = xObservaciones;
            this.CodEst = xCodEst;
            this.Nuvia = xNuvia;
            this.Ident = xIdent;
            this.CodEve = xCodEve;
            this.FormaPagoInicial = xFormaPagoInicial;
            this.SubTipoFormaPago = xSubTipoFormaPago;
            this.FormaPagoDesc = xFormaPagoDesc;


           // this.ExentoDesc = xExentoDesc;
            this.CtaAgrupacion = xCtaAgrupacion;


            this.IdUsuario = xIdUsuario;
        }

        public InfoEvento()
        {
        }

        // DEFINICION DE PROPIEDADES

        public DateTime Fecha { get; set; }
        public string NombreVia{ get; set; }
        public string VideoFoto1 { get; set; }
        public string VideoFoto2 { get; set; }
        public string TipoEvento { get; set; }
        public string DescTipoEvento { get; set; }
        //public int Manual { get; set; }
        public string Manual { get; set; }
        //public int Dac { get; set; }
        public string Dac { get; set; }
        public string Sentido { get; set; }
        public string SentidoCardinal { get; set; }
        /// <summary>
        /// Abreviatura de la forma de pago, ej.: Efec, Chip, Tag
        /// </summary>
        public string FormaPagoCorta { get; set; }

        public string Exento { get; set; }
        public string Cupom { get; set; }

        //public int Bloque { get; set; }              
        public string Bloque { get; set; }              
        //public int Ticket { get; set; }
        public string Ticket { get; set; }
        public string Operador { get; set; }
        //public int NroTransito { get; set; }
        public string NroTransito { get; set; }
        
        public string Ejes { get; set; }
        public string EjesLevantados { get; set; }
        public string SuspTab { get; set; }
        //public int DobleEje { get; set; }
        public string DobleEje { get; set; }
        public string Altura { get; set; }
        public string Matricula { get; set; }
        public string Observaciones { get; set; }

        //public int CodEst { get; set; }
        public string CodEst { get; set; }
        //public int Nuvia { get; set; }
        public string Nuvia { get; set; }
        //public int Ident { get; set; }
        public string Ident { get; set; }
        //public int IdTran { get; set; }
        public string IdTran { get; set; }
        //public int CodEve { get; set; }
        public string CodEve { get; set; }
        /// <summary>
        /// Inicial de la forma de pago, ej.: E, C, T
        /// </summary>
        public string FormaPagoInicial { get; set; }
        public string SubTipoFormaPago { get; set; }
        /// <summary>
        /// Descripción de la forma de pago, ej.: Efectivo, Tarjeta Chip Exento
        /// </summary>
        public string FormaPagoDesc { get; set; }
        //public string ExentoDesc { get; set; }
        public string CtaAgrupacion { get; set; }
        public string IdUsuario { get; set; }
        /// <summary>
        /// Obtiene o establece las observaciones del supervisor
        /// </summary>
        public string obsSupervisor { get; set; }
        /// <summary>
        /// Obtiene o establece el monto del tránsito
        /// </summary>
        public decimal MontoTransito { get; set; }
        /// <summary>
        /// Indica si el supervisor debe comentar este evento o no (Valor Booleano)
        /// </summary>
        public bool esDebeComentarSupervisor
        {
            get { return (Observaciones.Length <= 0); }
        }
        /// <summary>
        /// Obtiene o establece la velocidad del auto del evento
        /// </summary>
        public int Velocidad { get; set; }
        /// <summary>
        /// Obtiene o establece el Tag
        /// </summary>
        public Tag Tag { get; set; }
    }

    [Serializable]
    public class InfoEventoL : List<InfoEvento>
    {
    }
}

