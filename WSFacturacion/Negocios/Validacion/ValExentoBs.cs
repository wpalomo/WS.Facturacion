using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class ValExentoBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de subformas de pago
        /// </summary>
        /// <returns>Lista de subformas de pago</returns>
        /// ***********************************************************************************************
        public static FormaPagoValidacionL getCodFranquiciaForpag()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ValExentosDt.getCodFranquiciaForpag(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            #region Exento
            int nTipoTar;
            decimal valorTran;
            decimal valorTranBasico;
            bool bEsTranPrepago; //Prepago, Transporte u Omnibus

            bEsTranPrepago = anomalia.EsTransitoPrepagoConsolidado;

            //Si esta rechazada, el codigo de tarifa es 0 sino el que corresponde segun la forma de pago
            if (anomalia.Estado == "A")
            {
                nTipoTar = (int)anomalia.TipoTarifaConsolidado.CodigoTarifa;
            }
            else
            {
                nTipoTar = 0;
            }

            if (bEsTranPrepago)
            {
                if (anomalia.CategoriaConsolidada.Categoria != anomalia.CategoriaTag.Categoria)
                {
                    //Calculo del valor del transito con tipo de tarifa 0

                    //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                    if (anomalia.CategoriaConsolidada.Categoria == 20)
                        anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);
                    else
                        anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, 0);
                    
                }
                else
                {
                    //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                    if (anomalia.CategoriaConsolidada.Categoria == 20)
                        anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, nTipoTar);
                    else
                        anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, nTipoTar);                    
                    
                }

                //Categoria especial
                if (anomalia.CategoriaTag.Categoria == 20)
                {
                    valorTranBasico = anomalia.EjeAdicionalTag * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, 0);
                    valorTran = anomalia.EjeAdicionalTag * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, nTipoTar);
                }
                else
                {
                    valorTranBasico = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, 0);
                    valorTran = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTag.Categoria, nTipoTar);
                }
                
            }
            else
            {
                //Si es categoria especial, multiplico la cantidad de ejes por el valor cobrado por eje
                if (anomalia.CategoriaConsolidada.Categoria == 20)
                    anomalia.MontoConsolidado = anomalia.EjeAdicionalConsolidado * TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, nTipoTar);
                else
                    anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaConsolidada.Categoria, nTipoTar);                                        
                
            }

            if (anomalia.Estado == "R" || bEsTranPrepago && nTipoTar > 0)
            {
                anomalia.MontoDiferencia = anomalia.MontoConsolidado;
            }
            else
            {
                anomalia.MontoDiferencia = 0;
                anomalia.MontoFPConsolidada = anomalia.MontoOriginal;
            }
            //Si el tránsito Consolidado es PREPAGO le debito el monto de la categoría consolidada
            if (bEsTranPrepago)
            {
                //Solo tocamos el saldo si no es por tecla viaje
                if (nTipoTar == 0)
                {
                    anomalia.MontoMovTagDebito = anomalia.MontoConsolidado;
                }
                else
                {
                    anomalia.MontoMovTagDebito = 0;
                }
            }
            #endregion
        }
    }
}
