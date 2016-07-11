using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class RangosTagBs
    {
        #region RangosTag: Metodos de la Clase RangosTagBs.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Rangos de Tag, sin filtro
        /// </summary>
        /// <returns>Lista de RangosTag</returns>
        /// ***********************************************************************************************
        public static RangosTagL getRangosTag()
        {
            return getRangosTag(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Rangos de Tag. 
        /// </summary>
        /// <param name="codigo">Int - Permite filtrar por un Rango determinado
        /// <returns>Lista de RangosTag</returns>
        /// ***********************************************************************************************
        public static RangosTagL getRangosTag(int? codigo)
        {
            return getRangosTag(ConexionBs.getGSToEstacion(), codigo);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Rangos definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoOSA">Int - Permite filtrar por un Rango determinado
        /// <returns>Lista de RangosTag</returns>
        /// ***********************************************************************************************
        public static RangosTagL getRangosTag(bool bGST, int? codigoOSA)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);
                    RangosTagL ListaRangoTag =  RangosTagDt.getRangosTag(conn, codigoOSA);
                    return ListaRangoTag;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Rango Tag
        /// </summary>
        /// <param name="oRangosTag">Rango Tag Predefinido - Estructura del Rango a insertar
        /// ***********************************************************************************************
        public static void addRangoTag(RangosTag oRangoTag)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos RangoTag
                    RangosTagDt.addRangoTag(oRangoTag, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRangosTag(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oRangoTag),
                                                           getAuditoriaDescripcion(oRangoTag)),
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
        /// Modificacion de un Rango Tag
        /// </summary>
        /// <param name="oRangoTag">RangoTagPredefinido - Estructura del RangoTag a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updRangoTag(RangosTag oRangoTag)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos RangosTag
                    RangosTagDt.updRangoTag(oRangoTag, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRangosTag(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oRangoTag),
                                                           getAuditoriaDescripcion(oRangoTag)),
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
        /// Eliminacion de un Rango Tag
        /// </summary>
        /// <param name="oRangoTag">RangoTagPredefinido - Estructura del RangoTag a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delRangoTag(RangosTag oRangoTag)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //eliminamos el RangosTag
                    RangosTagDt.delRangoTag(oRangoTag, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRangosTag(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oRangoTag),
                                       getAuditoriaDescripcion(oRangoTag)),
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
        private static string getAuditoriaCodigoAuditoriaRangosTag()
        {
            return "OSA";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(RangosTag oRangosTag)
        {
            return oRangosTag.Administradora.Codigo.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(RangosTag oRangosTag)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Texto", oRangosTag.RangoDesde + "-" + oRangosTag.RangoHasta);

            return sb.ToString();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Tecnologías que utiliza la administradora de Tag.
        /// </summary>
        /// <param name="codigoMensaje">Int - Permite filtrar por una Tecnología determinada
        /// <returns>Lista de RangosTag</returns>
        /// ***********************************************************************************************
        public static TecnoTagL getTecnoTag(int? codigo)
        {
            return getTecnoTag(ConexionBs.getGSToEstacion(), codigo);
        }

        public static TecnoTagL getTecnoTag()
        {
            return getTecnoTag(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Tecnologías que utiliza la administradora de Tag
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigo">Int - Permite filtrar por una Tecnología determinada
        /// <returns>Lista de RangosTag</returns>
        /// ***********************************************************************************************
        public static TecnoTagL getTecnoTag(bool bGST, int? codigo)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return RangosTagDt.getTecnoTag(conn, codigo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

    }
}



