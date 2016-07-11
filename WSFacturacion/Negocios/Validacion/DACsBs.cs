using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class DACsBs
    {
        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            #region DAC
            bool bEsTranAutomatico;
            bool bEsModoAutomatico;
            bool bEsTranAbono;
            bool bEsTranPrepago;
            bool bEsTranPospago;
            bool bEsTranEfectivo;
            bool bEsTranVale;
            bool bEsTranExento;

            bool bEsFacturaErronea;
            bool bEsFacturaTarifa0;

            bool bCategHabil;

            //Criterios para ver quien paga la diferencia
            //  Tag o Chip
            //       Exento no paga nadie
            //       UFRE y Federado la paga el peajista (valor consolidado a tarifa plena menos valor original)
            //       Prepago la paga el usuario, si era bonificado la diferencia a tarifa plena,
            //           si no era bonificado tambien porque como todo es a tarifa plena
            //           (valor consolidado a tarifa plena menos valor de cat original a tarifa plena)

            //  Exento no paga nadie
            //  Efectivo, Vale paga el peajista

            bEsTranAutomatico = anomalia.EsTransitoAutomatico;
            bEsTranAbono = anomalia.EsTransitoAbono;
            bEsTranPrepago = anomalia.EsTransitoPrepago;
            bEsTranPospago = anomalia.EsTransitoPospago;
            bEsModoAutomatico = anomalia.EsModoAutomatico;
            bEsTranEfectivo = anomalia.EsTransitoPagoEfectivo;
            bEsTranVale = anomalia.EsTransitoVale;
            bEsTranExento = anomalia.EsTransitoExento;

            bEsFacturaErronea = anomalia.EsFacturaErronea;
            bEsFacturaTarifa0 = anomalia.EsFacturaTarifa0;

            //Calcular segun formula de anomalia
            //En la AVI aceptar es cat dac, por lo que tambien hay que calcular
            if (anomalia.Estado == "R" || anomalia.CategoriaTabulada.Categoria != anomalia.CategoriaConsolidada.Categoria)
            {
                //TODO A veces me graba F (probablemente porque no alcanza a cargar los datos, por las dudas lo comentamos)
                //TODO ya que solo no podria ser moto
                bCategHabil = true;
                /*
                bCategHabil = ValEstacionesBs.getCategFormaPagoHabil(anomalia.FormaPagoConsolidada.MedioPago, anomalia.FormaPagoConsolidada.FormaPago, anomalia.CategoriaConsolidada.Categoria);

                //Combinacion no habilitada, usamos la tarifa basica y grabamos como fallo
                if (!bCategHabil)
                {
                    anomalia.TipoTarifaConsolidado = new TarifaDiferenciada();
                    anomalia.TipoTarifaConsolidado.CodigoTarifa = 0;
                    anomalia.FormaPagoConsolidada = new FormaPagoValidacion();
                    anomalia.FormaPagoConsolidada.MedioPago = "F";
                    anomalia.FormaPagoConsolidada.FormaPago = "";
                    anomalia.FormaPagoConsolidada.SubformaPago = 0;
                }
                else */
                {
                    anomalia.TipoTarifaConsolidado = anomalia.TipoTarifa;
                    anomalia.FormaPagoConsolidada = new FormaPagoValidacion();
                    anomalia.FormaPagoConsolidada.MedioPago = anomalia.FormaPagoOriginal.MedioPago;
                    anomalia.FormaPagoConsolidada.FormaPago = anomalia.FormaPagoOriginal.FormaPago;
                    anomalia.FormaPagoConsolidada.SubformaPago = anomalia.FormaPagoOriginal.SubformaPago;
                }

                //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                if(anomalia.CategoriaConsolidada.Categoria == 20)
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, (int)anomalia.TipoTarifaConsolidado.CodigoTarifa);
                else
                    anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, (int)anomalia.TipoTarifaConsolidado.CodigoTarifa);

                //PANAVIAL
                /*
                if (anomalia.TipoRecarga == 1 &&
                    anomalia.CategoriaConsolidada.Categoria != anomalia.CategoriaTabulada.Categoria &&
                    (anomalia.MontoConsolidado != anomalia.MontoOriginal || anomalia.TipoTarifa.CodigoTarifa != anomalia.TipoTarifaConsolidado.CodigoTarifa))
                {
                    //Para tecla viaje solo se permite si es el mismo valor
                    bCategHabil = true;
                    bEsTranEfectivo = true;
                    anomalia.TipoTarifaConsolidado = new TarifaDiferenciada();
                    anomalia.TipoTarifaConsolidado.CodigoTarifa = 0;
                    anomalia.FormaPagoConsolidada = new FormaPagoValidacion();
                    anomalia.FormaPagoConsolidada.MedioPago = "F";
                    anomalia.FormaPagoConsolidada.FormaPago = "";
                    anomalia.FormaPagoConsolidada.SubformaPago = 0;
                    
                    //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                    if (anomalia.CategoriaConsolidada.Categoria == 20)
                        anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, (int)anomalia.TipoTarifaConsolidado.CodigoTarifa);
                    else
                        anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, (int)anomalia.TipoTarifaConsolidado.CodigoTarifa);
                    
                }*/

                //Paga el peajista (solo si es mayor el valor consolidado
                // Efectivo (incluye Ufre y Federado)
                // Vale
                if (!bCategHabil)
                {
                    anomalia.MontoDiferencia = anomalia.MontoConsolidado;
                }
                else if ((bEsTranEfectivo || bEsTranVale) &&
                    anomalia.MontoConsolidado > anomalia.MontoOriginal)
                {
                    anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                }
                else
                {
                    anomalia.MontoDiferencia = 0;
                }

                //Paga el usuario
                // Es Prepago
                // Es Pospago
                //Pero solo generamos movcta si es prepago
                //Usamos MontoOriginal y no el de la categoria del tag
                if (bEsTranPrepago || bEsTranPospago)
                {
                    if (bEsFacturaErronea || bEsFacturaTarifa0)
                    {
                        anomalia.MontoDiferencia = 0;
                        anomalia.MovTag = "C"; //Credito
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
                            if (anomalia.CategoriaConsolidada.Categoria != anomalia.CategoriaTabulada.Categoria &&
                                (anomalia.MontoConsolidado != anomalia.MontoOriginal || anomalia.TipoTarifa.CodigoTarifa != anomalia.TipoTarifaConsolidado.CodigoTarifa))
                            {
                                anomalia.MovTag = "C";
                                anomalia.MontoMovTag = anomalia.MontoOriginal;
                                anomalia.MovRecTag = "D";
                                anomalia.MontoMovRecTag = anomalia.MontoOriginal;
                            }
                        }
                        else
                        {
                            anomalia.MontoMovRecTag = 0;
                            if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                            {
                                anomalia.MovTag = "D";
                                anomalia.MontoMovTag = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                                //El monto consolidado pasa a ser MontoOriginal + MontoMovTag
                                anomalia.MontoConsolidado = anomalia.MontoOriginal + anomalia.MontoMovTag;
                            }
                            else if (anomalia.MontoConsolidado < anomalia.MontoOriginal)
                            {
                                //Para acreditar restamos de lo que realmente pago, lo que deberia haber pagado
                                //Tengo que acreditar (eje levantado)
                                anomalia.MovTag = "C";
                                anomalia.MontoMovTag = anomalia.MontoOriginal - anomalia.MontoConsolidado;
                            }
                            else
                            {
                                anomalia.MovTag = "D";
                                anomalia.MontoMovTag = 0;
                            }
                        }

                        if (anomalia.TipoRecarga == 1)
                        {
                            if (anomalia.MontoConsolidado > anomalia.MontoRecarga)
                            {
                                anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoRecarga;
                                //Con tecla viaje el cajero paga la diferencia
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

                //No paga nadie
                //  Exento
                if (bEsTranExento)
                {
                    anomalia.MontoDiferencia = 0;
                }
            }
            // si es aceptada
            else
            {
                if (bEsTranPrepago || bEsTranPospago)
                {
                    if (bEsFacturaErronea || bEsFacturaTarifa0)
                    {
                        anomalia.MontoDiferencia = 0;
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
                        anomalia.MontoConsolidado = anomalia.MontoOriginal;
                        anomalia.MontoDiferencia = 0;
                        anomalia.MovTag = "D";
                        anomalia.MontoMovTag = 0;
                    }
                }
                else
                {
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.MontoDiferencia = 0;
                    anomalia.MovTag = "D";
                    anomalia.MontoMovTag = 0;
                }

                anomalia.TipoTarifaConsolidado = anomalia.TipoTarifa;
                anomalia.FormaPagoConsolidada = new FormaPagoValidacion();
                anomalia.FormaPagoConsolidada.MedioPago = anomalia.FormaPagoOriginal.MedioPago;
                anomalia.FormaPagoConsolidada.FormaPago = anomalia.FormaPagoOriginal.FormaPago;
                anomalia.FormaPagoConsolidada.SubformaPago = anomalia.FormaPagoOriginal.SubformaPago;
            }

            #endregion
        }
    }
}
