using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class EmisoresTagBs
    {
        #region EmisoresTag: Metodos de la Clase EmisoresTagBs.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Emisores de Tag, sin filtro
        /// </summary>
        /// <returns>Lista de EmisoresTag</returns>
        /// ***********************************************************************************************
        public static EmisoresTagL getEmisoresTag()
        {
            return getEmisoresTag(ConexionBs.getGSToEstacion(), null, null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Emisores de Tag. 
        /// </summary>
        /// <param name="codigo">string - Permite filtrar por un Emisor determinado
        /// <returns>Lista de EmisoresTag</returns>
        /// ***********************************************************************************************
        public static EmisoresTagL getEmisoresTag(string codEmisor)
        {
            return getEmisoresTag(ConexionBs.getGSToEstacion(), codEmisor, null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Emisores de Tag. 
        /// </summary>
        /// <param name="codigo">string - Permite filtrar por un Emisor determinado
        /// <returns>Lista de EmisoresTag</returns>
        /// ***********************************************************************************************
        public static EmisoresTagL getEmisoresTag(int? codAdmin)
        {
            return getEmisoresTag(ConexionBs.getGSToEstacion(), null, codAdmin);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Emisores definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMensaje">Int - Permite filtrar por un Emisor determinado
        /// <returns>Lista de EmisoresTag</returns>
        /// ***********************************************************************************************
        public static EmisoresTagL getEmisoresTag(bool bGST, string codEmisor, int? codAdmin)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return EmisoresTagDt.getEmisoresTag(conn, codEmisor, codAdmin);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Emisor Tag
        /// </summary>
        /// <param name="oEmisoresTag">Emisor Tag Predefinido - Estructura del Emisor a insertar
        /// ***********************************************************************************************
        public static void addEmisorTag(EmisoresTag oEmisorTag)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos EmisorTag
                    EmisoresTagDt.addEmisorTag(oEmisorTag, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEmisoresTag(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oEmisorTag),
                                                           getAuditoriaDescripcion(oEmisorTag)),
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
        /// Modificacion de un Emisor Tag
        /// </summary>
        /// <param name="oEmisorTag">EmisorTagPredefinido - Estructura del EmisorTag a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updEmisorTag(EmisoresTag oEmisorTag)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos EmisoresTag
                    EmisoresTagDt.updEmisorTag(oEmisorTag, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEmisoresTag(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oEmisorTag),
                                                           getAuditoriaDescripcion(oEmisorTag)),
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
        /// Eliminacion de un Emisor Tag
        /// </summary>
        /// <param name="oEmisorTag">EmisorTagPredefinido - Estructura del EmisorTag a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delEmisorTag(EmisoresTag oEmisorTag)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //eliminamos el EmisoresTag
                    EmisoresTagDt.delEmisorTag(oEmisorTag, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEmisoresTag(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oEmisorTag),
                                       getAuditoriaDescripcion(oEmisorTag)),
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

        #endregion

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaEmisoresTag()
        {
            return "OSA";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(EmisoresTag oEmisoresTag)
        {
            return oEmisoresTag.Codigo.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(EmisoresTag oEmisoresTag)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Texto", oEmisoresTag.Descripcion);

            return sb.ToString();
        }

        #endregion

    }
}


