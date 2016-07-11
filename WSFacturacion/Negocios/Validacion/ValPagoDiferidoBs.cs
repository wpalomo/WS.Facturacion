using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class ValPagoDiferidoBs
    {
        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            #region Pago Diferido

            if (anomalia.CategoriaConsolidada.Categoria != anomalia.CategoriaTabulada.Categoria)
            {
                if (anomalia.CategoriaConsolidada.Categoria == 20)
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, (int)anomalia.TipoTarifaOriginal.CodigoTarifa);
                else
                    anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, (int)anomalia.TipoTarifaOriginal.CodigoTarifa);
            }
            else
            {
                anomalia.MontoConsolidado = anomalia.MontoOriginal;
                if (anomalia.CategoriaConsolidada.Categoria == 20 && anomalia.EjeAdicionalConsolidado != anomalia.EjeAdicionalTabulado)
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, (int)anomalia.TipoTarifaOriginal.CodigoTarifa);
            }

            if (anomalia.Estado == "R")
                anomalia.MontoDiferencia = Util.Max(anomalia.MontoConsolidado, anomalia.MontoOriginal);
            else if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
                anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
            else
                anomalia.MontoDiferencia = 0;

            #endregion
        }
    }
}
