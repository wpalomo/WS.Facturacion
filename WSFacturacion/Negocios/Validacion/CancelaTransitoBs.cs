using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class CancelaTransitoBs
    {

        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            #region Cancelacion Transito

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

            //Criterios para ver quien paga la diferencia
            //  Tag o Chip
            //       Exento no paga nadie
            //       UFRE y Federado la paga el peajista (valor consolidado a tarifa plena menos valor original)
            //       Prepago la paga el usuario, si era bonificado la diferencia a tarifa plena,
            //           si no era bonificado tambien porque como todo es a tarifa plena
            //           (valor consolidado a tarifa plena menos valor de cat original a tarifa plena)
            //
            //  Exento no paga nadie
            //  Efectivo, Vale paga el peajista


            bCategHabil = ValEstacionesBs.getCategFormaPagoHabil(anomalia.FormaPagoConsolidada.MedioPago, anomalia.FormaPagoConsolidada.FormaPago, anomalia.CategoriaConsolidada.Categoria);

            if (anomalia.CategoriaTabulada.Categoria == anomalia.CategoriaConsolidada.Categoria)
            {
                //Si es categoria especial
                anomalia.MontoConsolidado = anomalia.MontoOriginal;
                if (anomalia.CategoriaConsolidada.Categoria == 20 && anomalia.EjeAdicionalConsolidado != anomalia.EjeAdicionalTabulado)
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);            
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

            anomalia.MontoMovRecTag = 0;

            if (anomalia.Estado == "R")
            {
                if (!bCategHabil)
                {
                    //El peajista paga el transito completo
                    anomalia.MontoDiferencia = anomalia.MontoConsolidado;
                }
                else if (bEsTranEfectivo || bEsTranVale)
                {
                    //El peajista paga el transito completo
                    anomalia.MontoDiferencia = anomalia.MontoConsolidado;
                }
                else if (bEsTranPrepago)
                {
                    if (anomalia.EsFacturaErronea)
                    {
                    }
                    else
                    {
                        //Debito de la cuenta el transito completo                        
                        anomalia.MovTag = "D";
                        anomalia.MontoMovTag = anomalia.MontoConsolidado;
                        if (anomalia.TipoRecarga != null)
                        {
                            anomalia.MovRecTag = "C";
                            anomalia.MontoMovRecTag = anomalia.MontoRecarga == 0 ? anomalia.MontoOriginal : anomalia.MontoRecarga;
                        }
                    }
                    if (anomalia.TipoRecarga != null)
                    {
                        //La recarga pasa a sumarse a la recaudacion del cajero
                        anomalia.MontoDiferencia = anomalia.MontoRecarga == 0 ? anomalia.MontoOriginal : anomalia.MontoRecarga;
                    }

                }
                else
                {
                    //Si es rechazado no se puede cambiar la categoria consolidada
                    anomalia.MontoDiferencia = 0;
                }
            }
            else
            {
                anomalia.MontoDiferencia = 0;
            }

            #endregion
        }

    }
}
