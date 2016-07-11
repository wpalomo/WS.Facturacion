using System;
using System.Collections.Generic;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class ComentarioSupervisorBs
    {
        #region COMENTARIO DEL SUPERVISOR: Metodos de la Clase Comentarios del Supervisor.

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Comentario de Supervisor
        /// </summary>
        /// <param name="oMensaje">ComentarioSupervisor - Estructura del Comentario a insertar
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static void addMensaje(ComentarioSupervisor oComentario)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la estación, con transaccion
                    conn.ConectarPlaza(true);

                    //Agregamos Mensaje
                    ComentarioSupervisorDt.addComentarioSupervisor(oComentario, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaComentario(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oComentario),
                                                           getAuditoriaDescripcion(oComentario)),
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
        /// Devuelve todas las opciones de filtro disponible para los comentarios de supervisor
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static FiltroComentarioSupL GetFiltrosComentarioSup()
        {
            FiltroComentarioSupL filtros = new FiltroComentarioSupL();

            FiltroComentarioSup unFiltro = new FiltroComentarioSup();
            unFiltro.Key = "P";
            unFiltro.Descripcion = Traduccion.Traducir("Pendiente de Comentar");
            unFiltro.esPorDefecto = true;

            filtros.Add(unFiltro);

            unFiltro = new FiltroComentarioSup();
            unFiltro.Key = "C";
            unFiltro.Descripcion = Traduccion.Traducir("Tránsitos Comentados");

            filtros.Add(unFiltro);

            unFiltro = new FiltroComentarioSup();
            unFiltro.Key = "T";
            unFiltro.Descripcion = Traduccion.Traducir("Todos");

            filtros.Add(unFiltro);

            return filtros;
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaComentario()
        {
            return "COS";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ComentarioSupervisor oComentario)
        {
            return oComentario.id.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ComentarioSupervisor oComentario)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Via", oComentario.via.ToString());
            AuditoriaBs.AppendCampo(sb, "Tránsito", oComentario.transito.ToString());
            AuditoriaBs.AppendCampo(sb, "Comentario", oComentario.comentario.ToString());

            return sb.ToString();
        }
        
        #endregion

        #endregion
    }
}
