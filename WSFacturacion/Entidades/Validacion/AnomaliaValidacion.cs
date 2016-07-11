using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Facturacion;


namespace Telectronica.Validacion
{
    [Serializable]
    public class AnomaliaValidacion:Anomalia
    {
        public AnomaliaValidacion(Int16 codigo, String descripcion)
            : base(codigo, descripcion)
        {
        }

        public TipoComprobante TipoComprobante { get; set; }

        public Estacion Estacion { get; set; }

        public string StatusTransitoEnviado { get; set; }
        public int DisIdent { get; set; }
        public int DovIdent { get; set; }
        public int EveIdent { get; set; }
        public string EveSenti { get; set; }
        public int DisNumev { get; set; }
        public int RecNumev { get; set; }
        public DateTime? FechaRecarga { get; set; }
        public int Bloque { get; set; }
        public string Validada { get; set; }
        public string ValidadaOriginal { get; set; }

        public bool EstaModificada { get; set; }
        public DateTime Fecha { get; set; }

        public Usuario Validador { get; set; }

        public TipoValidacion TipoValidacionConsolidado { get; set; }
        public TipoValidacion TipoValidacion { get; set; }

        public CategoriaManual CategoriaDetectada { get; set; }
        public int EjeAdicionalDetectado { get; set; }
        public CategoriaManual CategoriaTabulada { get; set; }
        public int EjeAdicionalTabulado { get; set; }
        public CategoriaManual CategoriaConsolidada { get; set; }
        public int EjeAdicionalConsolidado { get; set; }
        public CategoriaManual CategoriaTicketManual { get; set; }
        public int EjeAdicionalTicketManual { get; set; }
        public CategoriaManual CategoriaTag { get; set; }
        public int EjeAdicionalTag { get; set; }
        public CategoriaManual CategoriaSeparada { get; set; }
        public int EjeAdicionalSeparado { get; set; }
        public byte EjeSuspensoConsolidado { get; set; }
        public int EjeSuspensoTabulado { get; set; }


        public TarifaDiferenciada TipoTarifa { get; set; }
        public TarifaDiferenciada TipoTarifaOriginal { get; set; }
        public TarifaDiferenciada TipoTarifaConsolidado { get; set; }

        public decimal MontoDiferencia { get; set; }
        public decimal MontoConsolidado { get; set; }
        public decimal MontoOriginal { get; set; }
        public decimal MontoMovTag { get; set; }
        public decimal MontoMovRecTag { get; set; }
        public decimal MontoRecarga { get; set; }
        public decimal MontoFPConsolidada { get; set; }
        public decimal MontoMovTagDebito { get; set; }
        public decimal MontoMovTagCredito { get; set; }
        public decimal MontoMovRecTagDebito { get; set; }
        public decimal MontoMovRecTagCredito { get; set; }
        public decimal MontoTicketManual { get; set; }



        //SIPS
        public SimulacionDePaso SimuPaso { get; set; }

        public string ciudad { get; set; }
        public Estado EstadoUF { get; set; }

        //public string NumeroConsolidado { get; set; }
        //public string NumeroOriginal { get; set; }
        public string MovTag { get; set; }
        public string MovRecTag { get; set; }

        public int? TipoRecarga { get; set; }

        //public FormaPagoValidacion FormaPago { get; set; }
        public FormaPagoValidacion FormaPagoConsolidada { get; set; }
        public FormaPagoValidacion FormaPagoOriginal { get; set; }
        public FormaPagoValidacion FormaPagoConsolidadaAnterior { get; set; }   //Es lo que estaba validado antes de invalidar

        public ValePrepagoVenta Vale { get; set; }

        public TicketManual TicketManual { get; set; }

        public string Estado { get; set; }
        public CodigoValidacion CodAceptacionRechazo { get; set; }

        public string ModoApertura { get; set; }

        public string ObservacionAnomalia { get; set; }
        public string ObservacionExterna { get; set; }
        public string ObservacionInterna { get; set; }
        public string ObservacionPeajista { get; set; }
        public string ObservacionSupervisor { get; set; }

        public Cuenta CuentaOriginal { get; set; }
        public Cuenta CuentaConsolidada { get; set; }

        //public Vehiculo VehiculoViejo { get; set; }
        public Vehiculo VehiculoOriginal { get; set; }
        public Vehiculo VehiculoConsolidado { get; set; }
        public string NombreEmpresa { get; set; }
        public int? Movil { get; set; }

        public Via Via { get; set; }

        public string PuntoVenta { get; set; }

        public Huella Huella { get; set; }

        public string DACSeparado { get; set; }
        public string TipoDAC { get; set; }
        public string EstadoSeparada { get; set; }

        public bool Autorizado { get; set; }
        public string NombreVideo1 { get; set; }
        public string NombreVideo2 { get; set; }

        public string PeajistaDescripcion { get; set; }
        public string SupervisorDescripcion { get; set; }

        public PagoDiferidoSupervisor PagoDiferido { get; set; }

        // es la numeracion con la que figura una anomalia en la grilla
        public int Registro { get; set; }

        public bool EstaValidada
        {
            get
            {
                if (Validada == "S")
                    return true;
                else if (Validada == "I")
                    return false;
                else
                    return false;
            }
        }

        public bool EsModoAutomatico
        {
            get
            {
                return ModoApertura == "Dinamico" || ModoApertura == "Dinámico";
            }
        }

        public bool EsTransitoAutomatico
        {
            get
            {
                if (FormaPagoConsolidada != null)
                    return FormaPagoConsolidada.MedioPago == "T" || FormaPagoConsolidada.MedioPago == "C";
                else
                    return false;
            }
        }

        public bool EsTransitoAbono
        {
            get
            {
                if (FormaPagoConsolidada != null)
                    return ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && FormaPagoConsolidada.FormaPago == "A");
                else
                    return false;
            }
        }

        public bool EsTransitoPagoEfectivo
        {
            get
            {
                if (FormaPagoConsolidada != null)
                    return (FormaPagoConsolidada.MedioPago == "E") || ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && (FormaPagoConsolidada.FormaPago == "U" || FormaPagoConsolidada.FormaPago == "F"));
                else
                    return false;
            }
        }


        public bool EsTransitoExento
        {
            get
            {
                if (FormaPagoConsolidada != null)
                {
                    if (FormaPagoConsolidada.MedioPago == "X")
                        return true;
                    else if ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && FormaPagoConsolidada.FormaPago == "X")
                        return true;
                    else if (CodAceptacionRechazo != null)

                        if (CodAceptacionRechazo.Codigo >= 90 && CodAceptacionRechazo.Codigo <= 94)
                            return true;
                        else
                            return false;

                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public bool EsTransitoVale
        {
            get
            {
                if (FormaPagoConsolidada != null)

                    return (FormaPagoConsolidada.MedioPago == "V");

                else
                    return false;
            }
        }

        public bool EsTransitoPospago
        {
            get
            {
                if (FormaPagoConsolidada != null)

                    return ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && FormaPagoConsolidada.FormaPago == "C");

                else
                    return false;
            }
        }

        public bool EsTransitoPrepago
        {
            get
            {
                if (FormaPagoConsolidada != null)

                    return ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && (FormaPagoConsolidada.FormaPago == "P" || FormaPagoConsolidada.FormaPago == "T" || FormaPagoConsolidada.FormaPago == "B"));

                else
                    return false;
            }
        }

        public bool EsFacturaErronea
        {
            get
            {
                bool esErronea = false;
                if (Codigo == 8)
                {
                    if (CodAceptacionRechazo.Codigo == 1 || CodAceptacionRechazo.Codigo == 2)
                        esErronea = true;
                }
                if (CodAceptacionRechazo.Codigo >= 95 && CodAceptacionRechazo.Codigo <= 99)
                    esErronea = true;
                return esErronea;
            }
        }

        public bool EsFacturaTarifa0
        {
            get
            {
                bool esTarifa0 = false;
                if (CodAceptacionRechazo.Codigo >= 90 && CodAceptacionRechazo.Codigo <= 94)
                    esTarifa0 = true;
                return esTarifa0;
            }
        }

        public bool EsTransitoAutomaticoConsolidado
        {
            get
            {
                if (FormaPagoConsolidada != null)
                    return FormaPagoConsolidada.MedioPago == "T" || FormaPagoConsolidada.MedioPago == "C";
                else
                    return false;
            }
        }


        public bool EsTransitoPagoEfectivoConsolidado
        {
            get
            {
                if (FormaPagoConsolidada != null)
                    return (FormaPagoConsolidada.MedioPago == "E") || ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && (FormaPagoConsolidada.FormaPago == "U" || FormaPagoConsolidada.FormaPago == "F"));
                else
                    return false;
            }
        }

        public bool EsTransitoAbonoConsolidado
        {
            get
            {
                if (FormaPagoConsolidada != null)
                    return ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && FormaPagoConsolidada.FormaPago == "A");
                else
                    return false;
            }
        }


        public bool EsTransitoValeConsolidado
        {
            get
            {
                if (FormaPagoConsolidada != null)

                    return (FormaPagoConsolidada.MedioPago == "V");

                else
                    return false;
            }
        }

        public bool EsTransitoPospagoConsolidado
        {
            get
            {
                if (FormaPagoConsolidada != null)

                    return ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && FormaPagoConsolidada.FormaPago == "C");

                else
                    return false;
            }
        }

        public bool EsTransitoPrepagoConsolidado
        {
            get
            {
                if (FormaPagoConsolidada != null)

                    return ((FormaPagoConsolidada.MedioPago == "C" || FormaPagoConsolidada.MedioPago == "T") && (FormaPagoConsolidada.FormaPago == "P" || FormaPagoConsolidada.FormaPago == "T" || FormaPagoConsolidada.FormaPago == "B"));

                else
                    return false;
            }
        }
    }

    [Serializable]
    public class Huella
    {
        public decimal IndiceConfiabilidad { get; set; }
        public string StringHuella { get; set; }
        public int Ejes { get; set; }
        public int RuedasDobles { get; set; }
        public bool EsAlto { get; set; }
        public byte EjeSuspenso { get; set; }
    }

    [Serializable]
    public class AnomaliaValidacionL : List<AnomaliaValidacion>
    {
    }
    
}