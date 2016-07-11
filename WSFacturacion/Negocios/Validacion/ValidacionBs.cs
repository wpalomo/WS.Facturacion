using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Validacion
{
    public class ValidacionBs
    {

        /// <summary>
        /// Guarda las anomalias modificadas
        /// </summary>
        /// <param name="codAnomalia"></param>
        /// <param name="estacion"></param>
        /// <param name="parte"></param>
        /// <param name="anomalias"></param>
        /// <param name="validador"></param>
        public static void SetAnomaliasValidadas(Anomalia codAnomalia, Estacion estacion, ParteValidacion parte, AnomaliaValidacionL anomalias, string validador)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    //TODO: Mandar las anomalias por lotes para evitar timeout
                    switch (codAnomalia.Codigo)
                    {
                        case (int)Anomalia.eAnomalia.enmEXENTOS:
                            ValExentosDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmSIPS:
                            SIPsDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmTICKET_MANUAL:
                            TicketManualDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmDACS_AFAVOR:
                        case (int)Anomalia.eAnomalia.enmDACS:
                            DACsDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmTRAN_NORMALES:
                            TransitoNormalDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_ABIERTA:
                        case (int)Anomalia.eAnomalia.enmVIOLAC_SUBE_BARRERA:
                        case (int)Anomalia.eAnomalia.enmVIOLAC_QUIEBRE:
                            ViolacionesDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_CERRADA:
                            ViolacionesDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmAUTORIZA_PASO:
                            AutorizacionPasoDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmCANCELAC_TRANSITO:
                            CancelaTransitoDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmPAGO_DIFERIDO:
                            ValPagoDiferidoDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmCANCELAC_OTROS:
                            CancelaTransitoDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        case (int)Anomalia.eAnomalia.enmTAG_MANUAL:
                            TagManualDt.setAnomaliasValidadas(conn, codAnomalia.Codigo, estacion.Numero, parte.Numero, parte.Jornada, anomalias, validador);
                            break;
                        default:
                            break;
                    }

                    //Grabamos auditoria
                    //TODO grabar un registro por anomalia
                    //Auditoria
                    using (Conexion connAud = new Conexion())
                    {
                        //conn.ConectarGSTThenPlaza();
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                        //Grabamos auditoria
                        string descripcion = getAuditoriaDescripcion(codAnomalia, estacion, parte, anomalias);
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaValidarAnomalias(),
                                                                "M",
                                                                getAuditoriaCodigoRegistro(parte),
                                                                descripcion),
                                                                connAud);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vuelve las anomalias al estado original
        /// </summary>
        /// <param name="anomalia"></param>
        public static void Invalidar(AnomaliaValidacion anomalia)
        {
            anomalia.ValidadaOriginal = "N";
            anomalia.Validada = "I";

            switch (anomalia.Codigo)
            {
                case (int)Anomalia.eAnomalia.enmEXENTOS:
                    #region Invalidar Exentos
                    
                    if (anomalia.EsTransitoPrepago)
                    {
                        anomalia.MontoMovTagDebito = anomalia.MontoConsolidado;
                    }
                    anomalia.FormaPagoConsolidada = null;
                    //anomalia.FormaPago = null;
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.VehiculoOriginal = null;
                    anomalia.NombreEmpresa = null;
                    anomalia.Movil = null;
                    anomalia.MontoFPConsolidada = 0;
                    anomalia.VehiculoConsolidado = null;
                    anomalia.TipoTarifaConsolidado = null;
                    anomalia.CuentaConsolidada = null;

                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmSIPS:
                    #region Invalidar SIPs

                    if (!anomalia.EstaModificada)
                    {
                        if (anomalia.Estado == "A")
                        {
                            if (anomalia.EsTransitoPrepago)
                            {
                                if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                                {
                                    anomalia.MovTag = "D"; //debito
                                    anomalia.MontoMovTag = anomalia.MontoOriginal;
                                    if (anomalia.TipoRecarga == null)
                                    {
                                        anomalia.MontoMovRecTag = 0;
                                    }
                                    else
                                    {
                                        anomalia.MovRecTag = "C"; //credito
                                        anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                    }
                                }
                                else
                                {
                                    if (anomalia.TipoRecarga == 1)
                                    {
                                        if (anomalia.MontoConsolidado != anomalia.MontoOriginal)
                                        {
                                            anomalia.MovTag = "D";
                                            anomalia.MontoMovTag = anomalia.MontoOriginal;
                                            anomalia.MovRecTag = "C";
                                            anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                        }
                                    }
                                    else
                                    {
                                        anomalia.MontoMovRecTag = 0;
                                        if (ValEstacionesBs.getCategFormaPagoHabil(anomalia.FormaPagoConsolidada.MedioPago, anomalia.FormaPagoConsolidada.FormaPago, anomalia.CategoriaConsolidada.Categoria))
                                        {
                                            if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                                            {
                                                anomalia.MovTag = "C"; //credito
                                                anomalia.MontoMovTag = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                                            }
                                            else
                                            {
                                                anomalia.MovTag = "D"; //debito
                                                anomalia.MontoMovTag = anomalia.MontoOriginal - anomalia.MontoConsolidado;
                                            }
                                        }
                                        else
                                        {
                                            //La categoria consolidada no estaba habilitada
                                            //le debitamos nuevamente el monto original
                                            anomalia.MovTag = "D";
                                            anomalia.MontoMovTag = anomalia.MontoOriginal;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Lo habia rechazado, debo acreditar
                            anomalia.MovTag = "D"; //debito
                            if (anomalia.EsTransitoPrepago)
                            {
                                anomalia.MontoMovTag = anomalia.MontoOriginal;
                                anomalia.MontoMovRecTag = 0;
                            }
                            else
                            {
                                anomalia.MontoMovTag = 0;
                                anomalia.MontoMovRecTag = 0;
                            }
                        }
                    }
                    else
                    {
                        anomalia.MontoMovTag = 0;
                        if (anomalia.EsTransitoPrepago)
                        {
                            if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                            {
                                if (anomalia.TipoRecarga != null)
                                {
                                    anomalia.MovTag = "D";
                                    anomalia.MontoMovTag = anomalia.MontoOriginal;
                                    anomalia.MovRecTag = "C";
                                    anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                }
                            }
                        }
                    }                    
                    anomalia.CategoriaConsolidada = anomalia.CategoriaTabulada;
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;

                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmTICKET_MANUAL:
                    #region Invalidar Ticket Manual

                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.CategoriaConsolidada = null;
                    anomalia.CategoriaTicketManual = null;
                    anomalia.TicketManual = null;

                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmDACS:
                case (int)Anomalia.eAnomalia.enmDACS_AFAVOR:                
                    #region Invalidar DACs
                    if (anomalia.Estado == "R" && !anomalia.EstaModificada)
                    {
                        if (anomalia.EsTransitoPrepago)
                        {
                            if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                            {
                                anomalia.MovTag = "D";
                                anomalia.MontoMovTag = anomalia.MontoOriginal;
                                if (anomalia.TipoRecarga == null)
                                {
                                    anomalia.MontoMovRecTag = 0;
                                }
                                else
                                {
                                    anomalia.MovRecTag = "C";
                                    anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                }
                            }
                            else
                            {
                                if (anomalia.TipoRecarga == 1)
                                {
                                    if (anomalia.CategoriaConsolidada.Categoria != anomalia.CategoriaTabulada.Categoria &&
                                       (anomalia.MontoConsolidado != anomalia.MontoOriginal || anomalia.TipoTarifa.CodigoTarifa != anomalia.TipoTarifaConsolidado.CodigoTarifa))
                                    {
                                        anomalia.MovTag = "D";
                                        anomalia.MontoMovTag = anomalia.MontoOriginal;
                                        anomalia.MovRecTag = "C";
                                        anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                    }
                                }
                                else
                                {
                                    anomalia.MontoMovRecTag = 0;
                                    if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                                    {
                                        anomalia.MovTag = "C";
                                        anomalia.MontoMovTag = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                                    }
                                    else
                                    {
                                        anomalia.MovTag = "D";
                                        anomalia.MontoMovTag = anomalia.MontoOriginal - anomalia.MontoConsolidado;
                                    }
                                }
                            }
                        }
                        else
                        {
                            anomalia.MovTag = "D";
                            anomalia.MontoMovTag = 0;
                            anomalia.MontoMovRecTag = 0;
                        }
                    }
                    else
                    {
                        anomalia.MovTag = "D";
                        anomalia.MontoMovTag = 0;
                        anomalia.MontoMovRecTag = 0;
                        if (anomalia.EsTransitoPrepago)
                        {
                            if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                            {
                                if (anomalia.TipoRecarga != null)
                                {
                                    anomalia.MovTag = "D";
                                    anomalia.MontoMovTag = anomalia.MontoOriginal;
                                    anomalia.MovRecTag = "C";
                                    anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                }
                            }
                        }
                    }
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.DACSeparado = "N";
                    anomalia.CategoriaConsolidada = null;
                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmTRAN_NORMALES:
                    #region Invalidar Transitos Normales
                    if (anomalia.Estado == "R" && !anomalia.EstaModificada)
                    {
                        if (anomalia.EsTransitoPrepago)
                        {
                            if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                            {
                                anomalia.MovTag = "D";
                                anomalia.MontoMovTag = anomalia.MontoOriginal;
                                if (anomalia.TipoRecarga == null)
                                {
                                    anomalia.MontoMovRecTag = 0;
                                }
                                else
                                {
                                    anomalia.MovRecTag = "C";
                                    anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                }
                            }
                            else
                            {
                                if (anomalia.TipoRecarga == 1)
                                {
                                    if (anomalia.CategoriaConsolidada.Categoria != anomalia.CategoriaTabulada.Categoria &&
                                       (anomalia.MontoConsolidado != anomalia.MontoOriginal || anomalia.TipoTarifa.CodigoTarifa != anomalia.TipoTarifaConsolidado.CodigoTarifa))
                                    {
                                        anomalia.MovTag = "D";
                                        anomalia.MontoMovTag = anomalia.MontoOriginal;
                                        anomalia.MovRecTag = "C";
                                        anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                    }
                                }
                                else
                                {
                                    anomalia.MontoMovRecTag = 0;
                                    if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                                    {
                                        anomalia.MovTag = "C";
                                        anomalia.MontoMovTag = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                                    }
                                    else
                                    {
                                        anomalia.MovTag = "D";
                                        anomalia.MontoMovTag = anomalia.MontoOriginal - anomalia.MontoConsolidado;
                                    }
                                }
                            }
                        }
                        else
                        {
                            anomalia.MovTag = "D";
                            anomalia.MontoMovTag = 0;
                            anomalia.MontoMovRecTag = 0;
                        }
                    }
                    else
                    {
                        anomalia.MovTag = "D";
                        anomalia.MontoMovTag = 0;
                        anomalia.MontoMovRecTag = 0;
                        if (anomalia.EsTransitoPrepago)
                        {
                            if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                            {
                                if (anomalia.TipoRecarga != null)
                                {
                                    anomalia.MovTag = "D";
                                    anomalia.MontoMovTag = anomalia.MontoOriginal;
                                    anomalia.MovRecTag = "C";
                                    anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                }
                            }
                        }
                    }
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.DACSeparado = "N";
                    anomalia.CategoriaConsolidada = null;
                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmVIOLAC_SUBE_BARRERA:
                case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_ABIERTA:
                case (int)Anomalia.eAnomalia.enmVIOLAC_QUIEBRE:
                    #region Invalidar Violacion Via Abierta
                    
                    if (anomalia.EsTransitoPrepagoConsolidado)                    
                        anomalia.MontoMovTagDebito = anomalia.MontoConsolidado;

                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.CategoriaConsolidada = null;
                    anomalia.FormaPagoConsolidada = null;
                    //anomalia.FormaPago = null;
                    anomalia.TipoTarifaConsolidado = null;
                    anomalia.VehiculoConsolidado = null;
                    anomalia.TicketManual = null;                    

                    #endregion
                    break;                
                    #region Invalidar Violacion Sube Barrera
                    
                    if (anomalia.EsTransitoPrepagoConsolidado)                    
                        anomalia.MontoMovTagDebito = anomalia.MontoConsolidado;

                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.CategoriaConsolidada = null;
                    anomalia.FormaPagoConsolidada = null;
                    //anomalia.FormaPago = null;
                    anomalia.TipoTarifaConsolidado = null;
                    anomalia.VehiculoConsolidado = null;
                    anomalia.TicketManual = null;                    

                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_CERRADA:
                    #region Invalidar Violacion Via Cerrada

                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.CategoriaConsolidada = null;
                    anomalia.VehiculoConsolidado = null;
                    anomalia.FormaPagoConsolidada = null;

                    #endregion 
                    break;
                case (int)Anomalia.eAnomalia.enmAUTORIZA_PASO:
                    #region Invalidar Autorizacion de paso

                    anomalia.CategoriaConsolidada = anomalia.CategoriaTabulada;
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;

                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmCANCELAC_OTROS:
                case (int)Anomalia.eAnomalia.enmCANCELAC_TRANSITO:
                    #region Invalidar Cancelacion de Transitos

                    anomalia.MontoMovTag = 0;
                    anomalia.MontoMovRecTag = 0;
                    if (!anomalia.EstaModificada)
                    {
                        if (anomalia.Estado == "R")
                        {
                            if (anomalia.EsTransitoPrepago)
                            {
                                if (!anomalia.EsFacturaErronea)
                                {
                                    //Lo habia rechazado, debo acreditar
                                    anomalia.MovTag = "C";
                                    anomalia.MontoMovTag = anomalia.MontoConsolidado;
                                    if (anomalia.TipoRecarga != null)
                                    {
                                        anomalia.MovRecTag = "D";
                                        anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                                    }
                                }
                            }
                        }
                    }

                    anomalia.CategoriaConsolidada = anomalia.CategoriaTabulada;
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;

                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmPAGO_DIFERIDO:
                    #region Invalidar Pago Diferido

                    anomalia.CategoriaConsolidada = anomalia.CategoriaTabulada;
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.VehiculoConsolidado = anomalia.VehiculoOriginal;                    

                    #endregion
                    break;
                case (int)Anomalia.eAnomalia.enmTAG_MANUAL:
                    #region Invalidar Tag Manual

                    TagManualBs.CalcularMontos(anomalia);

                    if (anomalia.FormaPagoConsolidada.FormaPago == "P")
                    {
                        anomalia.MontoMovTagCredito = anomalia.MontoOriginal;
                        if (anomalia.TipoRecarga != null)
                        {
                            anomalia.MontoMovRecTagDebito = anomalia.MontoRecarga;
                        }
                    }
                    if (anomalia.FormaPagoConsolidada.FormaPago == "P")
                    {
                        if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                        {
                            anomalia.MontoMovTagDebito = 0;
                        }
                        else if (anomalia.Estado == "A")
                        {
                            anomalia.MontoMovTagDebito = anomalia.MontoConsolidado;
                            if (anomalia.TipoRecarga != null)
                            {
                                anomalia.MontoMovRecTagCredito = anomalia.MontoRecarga;
                            }
                        }
                    }
                    anomalia.CategoriaConsolidada = anomalia.CategoriaTabulada;
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.CuentaConsolidada = anomalia.CuentaOriginal;
                    anomalia.VehiculoConsolidado = anomalia.VehiculoOriginal;
                    anomalia.FormaPagoConsolidada = anomalia.FormaPagoOriginal;
                    anomalia.TipoTarifaConsolidado = anomalia.TipoTarifa;

                    #endregion
                    break;
                default:
                    break;
                
            }
            anomalia.EstaModificada = true;
            anomalia.Estado = null;
            anomalia.CodAceptacionRechazo = null;
            anomalia.ObservacionInterna = null;
            anomalia.ObservacionExterna = null;
            anomalia.MontoDiferencia = 0;
            anomalia.TipoValidacionConsolidado = null;

        }

        /// <summary>
        /// Calcula los montos de las anomalias
        /// </summary>
        /// <param name="anomalia"></param>
        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            switch (anomalia.Codigo)
            {
                case (int)Anomalia.eAnomalia.enmEXENTOS:
                    ValExentoBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmSIPS:
                    SIPsBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmTICKET_MANUAL:
                    TicketManualBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmDACS:
                case (int)Anomalia.eAnomalia.enmDACS_AFAVOR:
                    DACsBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmTRAN_NORMALES:
                    TransitoNormalBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmVIOLAC_SUBE_BARRERA:
                case (int)Anomalia.eAnomalia.enmVIOLAC_QUIEBRE:
                case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_ABIERTA:
                    ViolacionesBs.CalcularMontosViolacionViaAbierta(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmVIOLAC_VIA_CERRADA:
                    ViolacionesBs.CalcularMontosViolacionViaCerrada(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmAUTORIZA_PASO:
                    AutorizacionPasoBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmCANCELAC_OTROS:
                case (int)Anomalia.eAnomalia.enmCANCELAC_TRANSITO:
                    CancelaTransitoBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmPAGO_DIFERIDO:
                    ValPagoDiferidoBs.CalcularMontos(anomalia);
                    break;
                case (int)Anomalia.eAnomalia.enmTAG_MANUAL:
                    TagManualBs.CalcularMontos(anomalia);
                    break;                
                default:
                    break;
            }
        }

        

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaValidarAnomalias()
        {
            return "VAN";
        }      


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Parte parte)
        {
            return parte.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Anomalia anomalia, Estacion estacion, Parte parte, ViaDefinicion via, int cantidad, CategoriaManual categoria, DateTime fechaDesde, DateTime fechaHasta, string estado, CodigoValidacion codigoValidacion)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estación", estacion.Nombre);
            AuditoriaBs.AppendCampo(sb, "Parte", parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Jornada Contable", parte.Jornada.ToString("dd/MM/yyyy"));
            AuditoriaBs.AppendCampo(sb, "Tipo Anomalia", anomalia.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Vía", via.NombreVia);
            AuditoriaBs.AppendCampo(sb, "Cantidad", cantidad.ToString());
            AuditoriaBs.AppendCampo(sb, "Desde", fechaDesde.ToString("dd/MM/yyyy hh:mm:ss"));
            if( cantidad > 1 )
                AuditoriaBs.AppendCampo(sb, "Hasta", fechaHasta.ToString("dd/MM/yyyy hh:mm:ss"));
            if (estado == "A")
            {
                AuditoriaBs.AppendCampo(sb, "Aceptada", codigoValidacion.Descripcion);
            }
            else if (estado == "R")
            {
                AuditoriaBs.AppendCampo(sb, "Rechazada", codigoValidacion.Descripcion);
            }
            else
            {
                AuditoriaBs.AppendCampo(sb, "No validada", "");
            }
            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Anomalia anomalia, Estacion estacion, Parte parte, AnomaliaValidacionL anomalias)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estación", estacion.Nombre);
            AuditoriaBs.AppendCampo(sb, "Parte", parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Jornada Contable", parte.Jornada.ToString("dd/MM/yyyy"));
            AuditoriaBs.AppendCampo(sb, "Tipo Anomalia", anomalia.Descripcion);
            //TODO datos de cada anomalia
            return sb.ToString();
        }
    

        #endregion



    }
}
