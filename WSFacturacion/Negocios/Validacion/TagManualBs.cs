using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class TagManualBs
    {
        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            #region Tag Manual

            decimal valorOri;
            int nCategoriaConsolidada;
            int ejesAdicionales;
            int nTitari;
            decimal valorTran;
            decimal valorCons;
            decimal valorTranBasico;

            bool bEsTagPrepagoO;
            bool bEsTagExentoO;
            bool bEsTagPrepagoC;
            bool bEsTagUfreFederadoO;

            bool bEsFacturaErronea;
            bool bEsFacturaTarifa0;

            bEsTagPrepagoO = anomalia.EsTransitoPrepago;
            bEsTagExentoO = anomalia.EsTransitoExento;
            bEsTagPrepagoC = anomalia.EsTransitoPrepagoConsolidado;
            //Ufre y Federado son de pago efectivo
            bEsTagUfreFederadoO = anomalia.EsTransitoPrepago;

            bEsFacturaErronea = anomalia.EsFacturaErronea;
            bEsFacturaTarifa0 = anomalia.EsFacturaTarifa0;

            nTitari = (int)anomalia.TipoTarifaConsolidado.CodigoTarifa;

            nCategoriaConsolidada = anomalia.CategoriaConsolidada.Categoria;

            ejesAdicionales = anomalia.EjeAdicionalConsolidado;

            //Calcula el valor del transito original
            valorOri = anomalia.MontoOriginal;

            anomalia.MontoMovRecTagCredito = 0;

            if (anomalia.Estado == "R" ||
                anomalia.CategoriaTabulada.Categoria != anomalia.CategoriaConsolidada.Categoria ||
                anomalia.VehiculoOriginal.Patente != anomalia.VehiculoConsolidado.Patente)
            {
                if (anomalia.Estado == "R")
                {
                    //Si es rechazado tomamos el valor a tipo de tarifa basica
                    if (nCategoriaConsolidada == 20)
                        valorCons = ejesAdicionales * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCategoriaConsolidada, 0);
                    else
                        valorCons = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCategoriaConsolidada, 0);
                }
                else
                {
                    if (nCategoriaConsolidada == 20)
                        valorCons = ejesAdicionales * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCategoriaConsolidada, nTitari);
                    else
                        valorCons = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCategoriaConsolidada, nTitari);
                    
                }

                if (anomalia.CategoriaTabulada.Categoria == 20)
                    valorTranBasico = anomalia.EjeAdicionalTag * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTabulada.Categoria, 0);
                else
                    valorTranBasico = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTabulada.Categoria, 0);

                if (anomalia.Estado == "R")
                {
                    //calculo el valor del transito con tipo de tarifa 0
                    if(anomalia.CategoriaTabulada.Categoria == 20)
                        valorTran = anomalia.EjeAdicionalTag * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTabulada.Categoria, 0);
                    else
                        valorTran = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTabulada.Categoria, 0);
                }
                else
                {
                    if (anomalia.CategoriaTabulada.Categoria == 20)
                        valorTran = anomalia.EjeAdicionalTag * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTabulada.Categoria, nTitari);
                    else
                        valorTran = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTabulada.Categoria, nTitari);
                    
                }
            }
            else
            {
                //Si no cambie nada queda todo igual
                valorCons = anomalia.MontoOriginal;
                valorTranBasico = anomalia.MontoOriginal;
                valorTran = anomalia.MontoOriginal;
            }
            if (anomalia.Estado == "R")
            {
                if (bEsFacturaErronea || bEsFacturaTarifa0)
                {
                    anomalia.MontoConsolidado = anomalia.MontoOriginal;
                    anomalia.MontoDiferencia = 0;
                }
                else
                {
                    anomalia.MontoConsolidado = valorTran;
                    if (anomalia.TipoRecarga == 1)
                    {
                        //Le cobro al peajista la diferencia
                        anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                    }
                    else if (bEsTagUfreFederadoO)
                    {
                        //Si era UFRE o Federado ya habia cobrado una parte en efectivo
                        //Le cobro al peajista la diferencia
                        anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                    }
                    else
                    {
                        anomalia.MontoDiferencia = anomalia.MontoConsolidado;
                    }
                }
                if (bEsTagPrepagoO)
                {
                    anomalia.MontoMovTagCredito = anomalia.MontoOriginal;
                    anomalia.MontoMovRecTagDebito = anomalia.MontoRecarga;
                }
            }
            else if (anomalia.Estado == "A")
            {
                anomalia.MontoConsolidado = valorCons;
                anomalia.MontoDiferencia = 0;

                //Si era UFRE o Federado el peajista deberia pagar la diferencia de categoria
                if (bEsTagUfreFederadoO)
                {
                    if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                    {
                        anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                    }
                }

                //Si el transito original es prepago, le acredito el monto de la categoria consolidada
                if (bEsTagPrepagoO)
                {
                    anomalia.MontoMovTagCredito = anomalia.MontoOriginal;
                    anomalia.MontoMovRecTagDebito = anomalia.MontoRecarga;
                }

                //Si el transito Consolidado es PREPAGO le debito el monto de la categoria consolidada
                if (bEsTagPrepagoC)
                {
                    if (bEsFacturaErronea || bEsFacturaTarifa0)
                    {
                        anomalia.MontoMovTagDebito = 0;
                    }
                    else
                    {
                        anomalia.MontoMovTagDebito = anomalia.MontoConsolidado;
                        if (anomalia.TipoRecarga != null)
                        {
                            anomalia.MontoMovRecTagCredito = anomalia.MontoRecarga;
                        }
                    }
                }

            }
            #endregion
        }
    }
}
