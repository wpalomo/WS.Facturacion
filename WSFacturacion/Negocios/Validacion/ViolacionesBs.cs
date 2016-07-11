using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Errores;

namespace Telectronica.Validacion
{
    public class ViolacionesBs
    {        
        /// <summary>
        /// enumerado que devuelve el metodo setNuevasViolVAbierta
        /// </summary>
        public enum eViolacionError
        {
            eMasdeunBloque = 0,
            eViaCerrada = 2,
            eViaOtroParte = 3,
            eFueradeJornada = 4,
            eViolacAExistente = 5,
            eQuiebreControlado = 6,
            eQuiebreLiberado = 7,
            eViaNoEnQuiebre = 8,
            eViolacErrorSP = 9
        };

        /// <summary>
        /// Calcula los montos de una violacion en via Cerrada
        /// </summary>
        /// <param name="anomalia"></param>
        public static void CalcularMontosViolacionViaCerrada(AnomaliaValidacion anomalia)
        {
            #region Violacion Via Cerrada
            int nCateg;
            int ejesAdicionales;
            int nTitari;
            if (anomalia.CategoriaConsolidada != null && anomalia.CategoriaConsolidada.Categoria > 0)
            {
                nCateg = anomalia.CategoriaConsolidada.Categoria;
                ejesAdicionales = anomalia.EjeAdicionalConsolidado;
            }
            else
            {
                nCateg = anomalia.CategoriaDetectada.Categoria;
                ejesAdicionales = anomalia.EjeAdicionalDetectado;
            }

            if (anomalia.Estado == "R")
            {
                nTitari = 0;
            }
            else
            {
                nTitari = (int)anomalia.TipoTarifaConsolidado.CodigoTarifa;
            }

            //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
            if(nCateg == 20)
                anomalia.MontoConsolidado = ejesAdicionales * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, nTitari);
            else
                anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, nTitari);

            anomalia.TipoTarifaConsolidado.CodigoTarifa = nTitari;

            if (anomalia.Estado == "R")
            {
                anomalia.MontoDiferencia = anomalia.MontoConsolidado;
            }
            else
            {
                anomalia.MontoDiferencia = 0;
            }

            #endregion
        }

        ///***********************************************************************************
        /// <summary>
        /// Calcula los montos de una violacion en via abierta
        /// </summary>
        /// <param name="anomalia"></param>
        ///***********************************************************************************
        public static void CalcularMontosViolacionViaAbierta(AnomaliaValidacion anomalia)
        {
            #region Violacion Via Abierta
            int nCateg;
            int ejesAdicionales;
            int nTitari;
            decimal valorTran = 0;
            decimal valorTranBasico = 0;

            if (anomalia.CategoriaConsolidada != null && anomalia.CategoriaConsolidada.Categoria > 0)
            {
                nCateg = anomalia.CategoriaConsolidada.Categoria;
                ejesAdicionales = anomalia.EjeAdicionalConsolidado;
            }
            else
            {
                nCateg = anomalia.CategoriaDetectada.Categoria;
                ejesAdicionales = anomalia.EjeAdicionalDetectado;
            }

            if (anomalia.Estado == "R")
                nTitari = 0;
            else
                nTitari = (int)anomalia.TipoTarifaConsolidado.CodigoTarifa;

            //Violacion fantasma
            if (anomalia.FormaPagoConsolidada != null && anomalia.FormaPagoConsolidada.FormaPago == "F")
            {
                anomalia.MontoConsolidado = 0;
            }
            else
            {
                anomalia.TipoTarifaConsolidado.CodigoTarifa = nTitari;
                if (anomalia.EsTransitoPrepagoConsolidado)
                {

                    if (TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, nTitari) != TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, nTitari))
                    {
                        //Calculo del valor del transito con tipo de tarifa 0

                        //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                        if (nCateg == 20)
                            anomalia.MontoConsolidado = ejesAdicionales * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, 0);
                        else
                            anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, 0);
                    }
                    else
                    {
                        //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                        if (nCateg == 20)
                            anomalia.MontoConsolidado = ejesAdicionales * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, nTitari);
                        else
                            anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, nTitari);

                    }

                    if (anomalia.CategoriaTag.Categoria == 20)
                    {
                        valorTranBasico = anomalia.EjeAdicionalTag * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, 0);
                        valorTran = anomalia.EjeAdicionalTag * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, nTitari);
                    }
                    else
                    {
                        valorTranBasico = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, 0);
                        valorTran = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, nTitari);
                    }

                }
                else
                {
                    if (nCateg == 20)
                        anomalia.MontoConsolidado = ejesAdicionales * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, nTitari);
                    else
                        anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCateg, nTitari);
                }
            }            

            if (anomalia.Estado == "R" || (anomalia.FormaPagoConsolidada != null && anomalia.FormaPagoConsolidada.MedioPago == "E") || (anomalia.EsTransitoPrepagoConsolidado && nTitari > 0))
                anomalia.MontoDiferencia = anomalia.MontoConsolidado;
            else
                anomalia.MontoDiferencia = 0;

            
            //Si el transito consolidado es prepago le debito el monto de la categoria consolidada
            if (anomalia.EsTransitoPrepagoConsolidado)
            {
                if (anomalia.MontoConsolidado > valorTranBasico)
                    anomalia.MontoConsolidado = valorTran + anomalia.MontoConsolidado - valorTranBasico;

                anomalia.MontoMovTagDebito = anomalia.MontoConsolidado;
            }
            else
                anomalia.MontoMovTagDebito = 0;


            #endregion
        }
        
        /// <summary>
        /// Guarda nuevas violaciones
        /// </summary>
        /// <param name="anomalia"></param>
        /// <param name="estacion"></param>
        /// <param name="parte"></param>
        /// <param name="via"></param>
        /// <param name="cantidad"></param>
        /// <param name="categoria"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="estado"></param>
        /// <param name="codigoValidacion"></param>
        public static void SetNuevasViol(Anomalia anomalia, Estacion estacion, ParteValidacion parte, ViaDefinicion via, int cantidad, CategoriaManual categoria, DateTime fechaDesde, DateTime fechaHasta, string estado, CodigoValidacion codigoValidacion,string obsInterna, string obsExterna)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //con transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), true);

                    eViolacionError causaError = (eViolacionError)EventoValidacionDt.getBloquePorFechaParte(conn, estacion, via, fechaDesde, fechaHasta, parte);

                    switch ((int)causaError)
                    {
                        case 1:
                            // Si la anomalia era viol en quiebre, y proc dio bien, dar error
                            if (anomalia.Codigo == (int)Anomalia.eAnomalia.enmVIOLAC_QUIEBRE)
                            {
                                causaError = eViolacionError.eViaNoEnQuiebre;
                                throw new ErrorJornadaCerrada(getStringError(causaError));
                            }
                            break;
                        case -1:
                            causaError = eViolacionError.eMasdeunBloque;
                            throw new ErrorJornadaCerrada(getStringError(causaError));
                            break;
                        case 0:
                            if (anomalia.Codigo != (int)Anomalia.eAnomalia.enmVIOLAC_VIA_CERRADA)
                            {
                                causaError = eViolacionError.eViaCerrada;
                                throw new ErrorJornadaCerrada(getStringError(causaError));
                            }
                            break;
                        case 2:
                            causaError = eViolacionError.eViaOtroParte;
                            throw new ErrorJornadaCerrada(getStringError(causaError));
                            break;
                        case 3:
                            causaError = eViolacionError.eFueradeJornada;
                            throw new ErrorJornadaCerrada(getStringError(causaError));
                            break;
                        case 6:
                            // Si la anomalia no era viol en quiebre
                            // dar error
                            if (anomalia.Codigo == (int)Anomalia.eAnomalia.enmVIOLAC_VIA_ABIERTA)
                            {
                                causaError = eViolacionError.eQuiebreControlado;
                                throw new ErrorJornadaCerrada(getStringError(causaError));
                            }
                            break;
                        case 7:
                            // Si la anomalia no era viol en quiebre
                            // dar error
                            if (anomalia.Codigo == (int)Anomalia.eAnomalia.enmVIOLAC_VIA_ABIERTA)
                            {
                                causaError = eViolacionError.eQuiebreLiberado;
                                throw new ErrorJornadaCerrada(getStringError(causaError));
                            }
                            break;
                    }

                    if (!JornadaBs.esFechaDeJornada(conn, fechaDesde, fechaHasta, parte.Jornada, estacion))
                        throw new ErrorJornadaCerrada("La fecha ingresada no es una fecha de jornada");

                    // es violacion via cerrada
                    if (anomalia.Codigo == (int)Anomalia.eAnomalia.enmVIOLAC_VIA_CERRADA)
                    {
                        ViolacionesDt.SetNuevasViolVCerrada(conn, anomalia, estacion, parte, via, cantidad,
                            categoria, fechaDesde, fechaHasta, estado, codigoValidacion, ConexionBs.getUsuario(), obsInterna, obsExterna);
                    }
                    else
                    {
                        ViolacionesDt.SetNuevasViolVAbierta(conn, anomalia, estacion, parte, via, cantidad,
                            categoria, fechaDesde, fechaHasta, estado, codigoValidacion, ConexionBs.getUsuario(), obsInterna, obsExterna);
                    }

                    //recalculamos los totales para que quede actualizado el treeview de anomalias
                    //TODO este SP es lento, seria bueno hacer solo la parte de las cantidades totales
                    ParteValidacionDt.setCalcularJornadaInicio(conn, estacion.Numero, parte.Jornada, parte.Numero);

                    //Auditoria
                    using (Conexion connAud = new Conexion())
                    {
                        //conn.ConectarGSTThenPlaza();
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                        // Ahora tenemos que grabar la auditoria:
                        //TODO si es a via cerrada grabar otro codigo
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaNuevaViolacion(),
                                            "A",
                                            getAuditoriaCodigoRegistro(parte),
                                            getAuditoriaDescripcion(anomalia, estacion, parte, via, cantidad, categoria, fechaDesde, fechaHasta, estado, codigoValidacion)),
                                            connAud);

                    }


                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna mensaje de error para las nuevas violaciones a via abierta
        /// </summary>
        /// <param name="nError"></param>
        /// <returns></returns>
        private static string getStringError(eViolacionError nError)
        {
            switch (nError)
            {
                case eViolacionError.eMasdeunBloque: return "Hay más de un bloque en el período solicitado";
                case eViolacionError.eViaCerrada: return "La vía estaba cerrada en el horario solicitado";
                case eViolacionError.eViaOtroParte: return "La vía estaba abierta con otro parte";
                case eViolacionError.eFueradeJornada: return "La fecha inicial o fecha final no está comprendida dentro de la jornada contable";
                case eViolacionError.eViolacAExistente: return "Ya existe un Tránsito en la misma Hora y Vía que una de las violaciones que se intentó registrar.";
                case eViolacionError.eQuiebreControlado: return "El Bloque de la Vía es un Quiebre Controlado.";
                case eViolacionError.eQuiebreLiberado: return "El Bloque de la Vía es un Quiebre Liberado.";
                case eViolacionError.eViaNoEnQuiebre: return "El bloque de la vía no es un quiebre.";
                case eViolacionError.eViolacErrorSP: return "Error en el SP de registro de nuevas violaciones";
                default: return String.Empty;
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        private static string getAuditoriaCodigoAuditoriaNuevaViolacion()
        {
            return "RVA";
        }

        private static string getAuditoriaCodigoAuditoriaNuevaViolacionCerrada()
        {
            return "RVC";
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
            if (cantidad > 1)
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

        #endregion

    }
}
