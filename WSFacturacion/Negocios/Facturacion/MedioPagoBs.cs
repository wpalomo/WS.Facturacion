using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    public class MedioPagoBs
    {
        #region CONFIGURACIONMEDIOPAGO: Metodos de la Clase de Negocios de la entidad MEDIOPAGOCONFIGURACION.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de medios de Pago
        /// </summary>
        /// <returns>Lista de Configuracion de medios de pagos</returns>
        /// ***********************************************************************************************
        public static MedioPagoConfiguracionL getMedioPago()
        {
            return getMedioPago(ConexionBs.getGSToEstacion(), null, null);
        }

        public static MedioPagoConfiguracionL getMedioPago(string TipoMedio, string TipoBoleto)
        {
            return getMedioPago(ConexionBs.getGSToEstacion(), TipoMedio, TipoBoleto);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista Configuracion de medios de pagos
        /// </summary>
        /// <param name="bGST">bool - Permite conectarse a GST o la Estación
        /// <returns>Lista de Configuracion de medios de pagos</returns>
        /// ***********************************************************************************************
        public static MedioPagoConfiguracionL getMedioPago(bool bGST, string TipoMedio, string TipoBoleto)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return MedioPagoDt.getMedioPago(conn, TipoMedio, TipoBoleto);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Configuracion de medios de pagos
        /// </summary>
        /// <param name="oMedioPagoConfiguracion">MedioPagoConfiguracion - Estructura de Configuracion de medios de pagos
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updMedioPago(MedioPagoConfiguracion oMedioPagoConfiguracion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre con transaccion
                    conn.ConectarGST(true);

                    //Modificamos Config
                    MedioPagoDt.updMedioPago(oMedioPagoConfiguracion, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMedioPagoConfiguracion(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oMedioPagoConfiguracion),
                                                           getAuditoriaDescripcion(oMedioPagoConfiguracion)),
                                                           conn);


                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

            ///****************************************************************************************************<summary>
            /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoAuditoriaMedioPagoConfiguracion()
            {
                return "CON";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(MedioPagoConfiguracion oMedioPagoConfiguracion)
            {
                return oMedioPagoConfiguracion.FormaDePago.CodigoFormaPago;
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(MedioPagoConfiguracion oMedioPagoConfiguracion)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Forma de Pago", oMedioPagoConfiguracion.FormaDePago.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Costo Inicial", Convert.ToString(oMedioPagoConfiguracion.CostoMedioPago));
                AuditoriaBs.AppendCampo(sb, "Costo de Reposición", Convert.ToString(oMedioPagoConfiguracion.CostoReposicionMedioPago));

                return sb.ToString();
            }

            #endregion

        #endregion

    }
}
