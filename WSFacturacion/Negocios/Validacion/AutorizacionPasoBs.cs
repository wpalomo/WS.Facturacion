using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    class AutorizacionPasoBs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anomalia"></param>
        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            decimal valorCons = 0;
            decimal valorTran = 0;
            int nTitari = 0;

            nTitari = (int)anomalia.TipoTarifa.CodigoTarifa;

            if (anomalia.CategoriaTabulada.Categoria != anomalia.CategoriaConsolidada.Categoria)
            {
                if (anomalia.CategoriaConsolidada.Categoria == 20)
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, nTitari);
                else
                    anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, nTitari);
            }
            else
            {
                valorCons = anomalia.MontoOriginal;
                if (anomalia.CategoriaConsolidada.Categoria == 20 && anomalia.EjeAdicionalConsolidado != anomalia.EjeAdicionalTabulado)
                    valorCons = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, nTitari);
            }

            if (anomalia.Estado == "R")
            {
                //Calculo del valor del transito con tipo de tarifa 0
                if (anomalia.CategoriaConsolidada.Categoria == 20)
                    valorTran = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);
                else
                    valorTran = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);
                
                anomalia.MontoConsolidado = valorTran;
                anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
            }
            else if (anomalia.Estado == "A")
            {
                anomalia.MontoConsolidado = valorCons;
                if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                    anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
                else
                    anomalia.MontoDiferencia = 0;
            }

        }
    }
}
