using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion y registro de la auditoria realizada en el sistema
    /// </summary>
    ///****************************************************************************************************

    public static class AuditoriaBs
    {
        #region AUDITORIA: Clase de Negocios de Auditoria


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un movimiento de auditoria.
        /// </summary>
        /// <param name="oAuditoria">Auditoria - Objeto de auditoria a insertar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addAuditoria(Auditoria oAuditoria)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    addAuditoria(oAuditoria, conn);

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
        /// Alta de un movimiento de auditoria.
        /// </summary>
        /// <param name="oAuditoria">Auditoria - Objeto de auditoria a insertar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addAuditoria(Auditoria oAuditoria, Conexion conn)
        {
            addAuditoria(oAuditoria, ConexionBs.getUsuario(), conn);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un movimiento de auditoria.
        /// pasamos el usuario para cuando no tenemos el usuario logueado
        /// </summary>
        /// <param name="oAuditoria">Auditoria - Objeto de auditoria a insertar
        /// <param name="usuario">string - Codigo del usuario
        /// <param name="conn">Conexion - Conexion a la base de datos
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addAuditoria(Auditoria oAuditoria, string usuario, Conexion conn)
        {
            try
            {
                //Agregamos datos fijos
                oAuditoria.Estacion = ConexionBs.getNumeroEstacion();
                oAuditoria.Fecha = DateTime.Now;
                oAuditoria.Terminal = ConexionBs.getTerminal();
                oAuditoria.Usuario = usuario;

                //Grabamos la auditoria
                AuditoriaDt.addAuditoria(oAuditoria, conn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Metodo que ayuda en el armado de la descripcion de la auditoria, centralizando el formato generado
        /// </summary>
        /// <param name="sb">StringBuilder - Objeto utilizado para armar la descripcion. Pasa por referencia</param>
        /// <param name="nombreCampo">string - Texto que identifica al campo. Se traduce dentro de la funcion</param>
        /// <param name="valorCampo">string - Valor que se graba en el campo auditado</param>
        /// <returns>Lista de Codigos de Auditoria</returns>
        /// ***********************************************************************************************
        public static void AppendCampo(StringBuilder sb, string nombreCampo, string valorCampo)
        {
            if (nombreCampo != "")
            {
                sb.Append(Traduccion.Traducir(nombreCampo));
                sb.Append(": ");
            }
            sb.Append(valorCampo);
            sb.Append("; ");
        }


        #endregion



        #region CODIGO_AUDITORIA: Clase de Negocios de los Codigos de Auditoria


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los codigos de auditoria definidos, sin filtro. 
        /// </summary>
        /// <returns>Lista de Codigos de Auditoria</returns>
        /// ***********************************************************************************************
        public static AuditoriaCodigoL getCodigosAuditoria()
        {
            return getCodigosAuditoria(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los codigos de auditoria definidos. Permite filtrar por codigo de auditoria
        /// </summary>
        /// <param name="codigoAuditoria">Int - Permite filtrar por un codigo de auditoria determinado
        /// <returns>Lista de Codigos de Auditoria</returns>
        /// ***********************************************************************************************
        public static AuditoriaCodigoL getCodigosAuditoria(string codigoAuditoria)
        {
            return getCodigosAuditoria(ConexionBs.getGSToEstacion(), codigoAuditoria);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los codigos de auditoria definidos. Permite filtrar por codigo de auditoria
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoAuditoria">string - Permite filtrar por un codigo de auditoria determinado
        /// <returns>Lista de Codigos de Auditoria</returns>
        /// ***********************************************************************************************
        public static AuditoriaCodigoL getCodigosAuditoria(bool bGST, string codigoAuditoria)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return AuditoriaDt.getCodigosAuditoria(conn, codigoAuditoria);
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
