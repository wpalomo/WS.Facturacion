using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class SIPsBs
    {
        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            #region SIP
            bool bEsTranAutomatico; //Tag o Chip
            bool bEsModoAutomatico; //Modo de via
            bool bEsTranAbono; //Abono
            bool bEsTranPrepago; //Prepago, Transporte u Omnibus
            bool bEsTranPospago; //pospago
            bool bEsTranEfectivo; //Efectivo
            bool bEsTranVale; //Vale
            bool bEsTranExento;

            bool bCategHabil; //Categoria Habilitada

            bEsTranAutomatico = anomalia.EsTransitoAutomatico;
            bEsTranAbono = anomalia.EsTransitoAbono;
            bEsTranPrepago = anomalia.EsTransitoPrepago;
            bEsTranPospago = anomalia.EsTransitoPospago;
            bEsModoAutomatico = anomalia.EsModoAutomatico;
            bEsTranEfectivo = anomalia.EsTransitoPagoEfectivo;
            bEsTranVale = anomalia.EsTransitoVale;
            bEsTranExento = anomalia.EsTransitoExento;

            bCategHabil = ValEstacionesBs.getCategFormaPagoHabil(anomalia.FormaPagoConsolidada.MedioPago, anomalia.FormaPagoConsolidada.FormaPago, anomalia.CategoriaConsolidada.Categoria);

            //Calcular segun formula de la anomalia SIP

            if (anomalia.CategoriaTabulada.Categoria == anomalia.CategoriaConsolidada.Categoria)
            {                
                anomalia.MontoConsolidado = anomalia.MontoOriginal;
                if (anomalia.CategoriaConsolidada.Categoria == 20 && anomalia.EjeAdicionalConsolidado != anomalia.EjeAdicionalTabulado)
                {
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);
                }
            }
            else
            {
                //Si no esta habilitado usamos el tipo de tarifa basico
                //Si cambiaron la categoria del abono tambien

                //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                if (anomalia.CategoriaConsolidada.Categoria == 20)
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);
                else
                    anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);
                
            }
            if (anomalia.Estado == "R")
            {
                if (bEsTranEfectivo || bEsTranVale)
                {
                    anomalia.MontoDiferencia = -anomalia.MontoOriginal;
                }
                else if (bEsTranPrepago)
                {
                    //Tengo que acreditar porque se cancelo el transito
                    anomalia.MovTag = "C"; //credito
                    anomalia.MontoMovTag = anomalia.MontoOriginal;
                }
                else
                {
                    //si es rechazado no se puede cambiar la categoria consolidada
                    anomalia.MontoDiferencia = 0;
                }
            }
            else
            {
                //Paga el peajista (solo si es mayor el valor consolidado)
                //Efectivo (incluye Ufre y Federado)
                //Vale
                if (!bCategHabil)
                {
                    anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                }
                else if ((bEsTranEfectivo || bEsTranVale) && anomalia.MontoConsolidado > anomalia.MontoOriginal)
                {
                    anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                }
                else
                {
                    anomalia.MontoDiferencia = 0;
                }
                //Paga el usuario
                //Es Prepago
                //Es Pospago
                //Pero solo generamos movcta si es prepago
                //Usamos MontoOriginal y no el de la categoria del tag
                if (bEsTranPrepago || bEsTranPospago)
                {
                    if (anomalia.EsFacturaErronea || anomalia.EsFacturaTarifa0)
                    {
                        anomalia.MovTag = "C"; //credito
                        anomalia.MontoMovTag = anomalia.MontoOriginal;
                        if (anomalia.TipoRecarga == null)
                        {
                            anomalia.MontoMovRecTag = 0;
                        }
                        else
                        {
                            anomalia.MovRecTag = "D"; //debito
                            anomalia.MontoMovRecTag = anomalia.MontoRecarga;
                        }
                    }
                    else
                    {
                        if (anomalia.TipoRecarga == 1)
                        {
                            if (anomalia.MontoConsolidado != anomalia.MontoOriginal)
                            {
                                anomalia.MovTag = "C"; //credito
                                anomalia.MontoMovTag = anomalia.MontoOriginal;
                                anomalia.MovRecTag = "D"; //debito
                                anomalia.MontoMovRecTag = anomalia.MontoOriginal;
                            }
                        }
                        else
                        {
                            anomalia.MontoMovRecTag = 0;
                            if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                            {
                                anomalia.MovTag = "D"; //debito
                                anomalia.MontoMovTag = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                            }
                            else if (anomalia.MontoConsolidado < anomalia.MontoOriginal)
                            {
                                //TODO Ver si al acreditar a un bonificado deberia usar tarifa plena
                                //Tengo que acreditar (eje levantado)
                                anomalia.MovTag = "C"; //credito
                                anomalia.MontoMovTag = anomalia.MontoOriginal - anomalia.MontoConsolidado;
                            }
                            else
                            {
                                anomalia.MovTag = "D"; //debito
                                anomalia.MontoMovTag = 0;
                            }
                            //El monto consolidado pasa a ser MontoOriginal + MontoMovTag
                            anomalia.MontoConsolidado = anomalia.MontoOriginal + anomalia.MontoMovTag;
                        }
                    }

                    if (anomalia.TipoRecarga == 1)
                    {
                        if (anomalia.MontoConsolidado > anomalia.MontoRecarga)
                        {
                            anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoRecarga;
                        }

                    }
                }
                else
                {
                    anomalia.MovTag = "D"; //debito
                    anomalia.MontoMovTag = 0;
                    anomalia.MontoMovRecTag = 0;
                }

                //No paga nadie
                //exento
                if (bEsTranExento)
                {
                    anomalia.MontoDiferencia = 0;
                }
            }
            #endregion
        }



        public static SimulacionDePasoL GetSIPs(int estacion, DateTime fecha, int Tolerancia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return SIPsDt.GetSIPs(conn, estacion, fecha, Tolerancia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
