using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Telectronica.Peaje
{
    public class AdministradoraTagsBs
    {

       
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las OSAs, sin filtro
        /// </summary>
        /// <returns>Lista de OSAs</returns>
        /// ***********************************************************************************************
        public static AdministradoraTagsL getOSAs()
        {
            return getOSAs(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las OSAs. 
        /// </summary>
        /// <param name="codigoMensaje">Int - Permite filtrar por una OSA determinado
        /// <returns>Lista de OSAs</returns>
        /// ***********************************************************************************************
        public static AdministradoraTagsL getOSAs(int? codigoOSA)
        {
            return getOSAs(ConexionBs.getGSToEstacion(), codigoOSA);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de OSAs definidas
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMensaje">Int - Permite filtrar por una OSA determinada
        /// <returns>Lista de OSAs</returns>
        /// ***********************************************************************************************
        public static AdministradoraTagsL getOSAs(bool bGST, int? codigoOSA)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return AdministradoraTagsDt.getOSAs(conn, codigoOSA);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una OSA
        /// </summary>
        /// <param name="oOSA">OSAPredefinida - Estructura de la OSA a insertar
        /// ***********************************************************************************************
        public static void addOSA(AdministradoraTags oOSA)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos OSA
                    AdministradoraTagsDt.addOSA(oOSA, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaOSA(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oOSA),
                                                           getAuditoriaDescripcion(oOSA)),
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
        /// Modificacion de una OSA
        /// </summary>
        /// <param name="oOSA">OSAPredefinido - Estructura de la OSA a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updOSA(AdministradoraTags oOSA)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos OSA
                    AdministradoraTagsDt.updOSA(oOSA, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaOSA(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oOSA),
                                                           getAuditoriaDescripcion(oOSA)),
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
        /// Eliminacion de una OSA
        /// </summary>
        /// <param name="oMensaje">OSAPredefinido - Estructura de la OSA a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delOSA(AdministradoraTags oOSA)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //eliminamos la OSA
                    AdministradoraTagsDt.delOSA(oOSA, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaOSA(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oOSA),
                                       getAuditoriaDescripcion(oOSA)),
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
        private static string getAuditoriaCodigoAuditoriaOSA()
        {
            return "OSA";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(AdministradoraTags oOSA)
        {
            return oOSA.Codigo.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(AdministradoraTags oOSA)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Texto", oOSA.Descripcion);

            return sb.ToString();
        }

        //Lo que trae esta son EMISORES y NO ADMINISTRADORAS (usar getOSas)
        /*
        public static AdministradoraTagsL getAdministradorasTag()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return AdministradoraTagsDt.getAdministradorasTags(conn); ;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }*/

        #endregion

    }
}
