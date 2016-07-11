using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class MonedaBs
    {
        #region MONEDA: Metodos de la Clase Moneda.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los monedas definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas()
        {
            return getMonedas(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los monedas definidos. 
        /// </summary>
        /// <param name="codigoMonedas">Int - Permite filtrar por una moneda determinado
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas(int? codigoMonedas)
        {
            return getMonedas(ConexionBs.getGSToEstacion(), codigoMonedas);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de monedas definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMoneda">Int - Permite filtrar por una Moneda determinada
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas(bool bGST, int? codigoMonedas)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return MonedaDt.getMonedas(conn, codigoMonedas);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Moneda
        /// </summary>
        /// <param name="oMoneda">Monedas - Estructura de la Moneda a insertar
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static void addMonedas(Moneda oMonedas)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que no exista la Moneda

                    //Agregamos Mensaje
                    MonedaDt.addMonedas(oMonedas, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMoneda(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oMonedas),
                                                           getAuditoriaDescripcion(oMonedas)),
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


        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Moneda
        /// </summary>
        /// <param name="oMoneda">Monedas - Estructura de la mondeda a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updMoneda(Moneda oMoneda)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que ya exista la Moneda

                    //Modificamos Moneda

                    MonedaDt.addMonedas(oMoneda, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMoneda(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oMoneda),
                                                           getAuditoriaDescripcion(oMoneda)),
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


        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Moneda
        /// </summary>
        /// <param name="oMoneda">MensajePredefinido - Estructura de la Moneda a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delMoneda(Moneda oMoneda)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verficamos que haya registros para la Moneda

                    //eliminamos el Mensaje
                    MonedaDt.delMoneda(oMoneda, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMoneda(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oMoneda),
                                       getAuditoriaDescripcion(oMoneda)),
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
        private static string getAuditoriaCodigoAuditoriaMoneda()
        {
            return "MON";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Moneda oMoneda)
        {
            return oMoneda.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Moneda oMoneda)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Moneda", oMoneda.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Simbolo", oMoneda.Desc_Moneda);

            return sb.ToString();
        }

        #endregion


        #endregion
    }
}
