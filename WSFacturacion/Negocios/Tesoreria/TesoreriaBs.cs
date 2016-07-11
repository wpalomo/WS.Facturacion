using System;
using System.Collections.Generic;
using System.Text;
using Telectronica.Utilitarios;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    public static class TesoreriaBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Registra la modificación de Tesoreria en Configcco
        /// </summary>
        /// <param name="bGST">Int - Configcco a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updConfigTesoreria(GestionConfiguracion oConfigcco)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos Configcco para tesoreria
                GestionConfiguracionDt.updConfigCCOTesoreria(oConfigcco, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oConfigcco),
                                                        getAuditoriaDescripcion(oConfigcco)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Configcco definidas. 
        /// </summary>
        /// <returns>Lista de Configcco</returns>
        /// ***********************************************************************************************
        public static GestionConfiguracion getConfigTesoreria()
        {
            return getConfigTesoreria(ConexionBs.getGSToEstacion());
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configcco definidas
        /// </summary>
        /// <param name="bGST">Int - Permite filtrar por una Configcco determinada
        /// <returns>Lista de Configcco</returns>
        /// ***********************************************************************************************
        public static GestionConfiguracion getConfigTesoreria(bool bGST)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return GestionConfiguracionDt.getConfigcco(conn);
            }
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Autoriza Operación
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="control"></param>
        /// <param name="usuario"></param>
        /// <param name="password"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool AutorizarOperacion(string pagina, string control, string usuario, string password, string comentario, out string mensaje)
        {
            bool autoriza = false;
            PermisoL oPermisos = null;
            mensaje = "";

            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, sin transaccion
                conn.ConectarPlaza(false);


                Usuario oUsuario = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre());
                oUsuario.PerfilActivo = new Perfil(ConexionBs.getPerfil(), "");
                if (usuario != "" || password != "")
                    oUsuario = UsuarioBs.ValidarLogin(usuario, password, null, false);  //No se debe actualizar en la sesion el usuario logueado

                if (oUsuario != null)
                {

                    mensaje = "El usuario no tiene permiso para realizar la Operacion.";
                    string cont = control;

                    oPermisos = PermisosDt.getPermisos(conn, oUsuario.PerfilActivo.Codigo, true, ConexionBs.getGSToEstacion(), true, "TES", pagina);

                    foreach (var item in oPermisos)
                    {
                        if (item.Control == cont)
                        {
                            if (item.Habilitado)
                            {
                                if (!item.Autorizacion)
                                {
                                    autoriza = true;
                                    mensaje = "";
                                }
                                else
                                {
                                    autoriza = false;
                                    mensaje = "Esta operación debe ser autorizada por un usuario de mayor nivel";
                                }
                            }
                            break;
                        }
                       
                    }
                }
                else
                {
                    mensaje = "Usuario o Contraseña inválidos.";
                }
            

                // Si se Autorizo se deberia Grabar Una Auditoria
                if (autoriza) 
                {
                
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAutoriz(),
                                                            "A",
                                                            getAuditoriaCodigoRegistroAutoriz(pagina,control),
                                                            getAuditoriaDescripcionAutoriz(pagina, control,oUsuario,comentario)));
            
                }

            }

            return autoriza;
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.


        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaAutoriz()
        {
            return "OPA";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroAutoriz(string Pagina, string Control)
        {
            return "Autorización:" + Pagina + " - " + Control;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionAutoriz(string Pagina, string Control, Usuario oUsuario, string comentario)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Pagina - Control", Pagina + Control);
            AuditoriaBs.AppendCampo(sb, "Usuario que Autoriza", oUsuario.ID_Nombre);
            AuditoriaBs.AppendCampo(sb, "Comentario", comentario);
            return sb.ToString();
        }
        
        
        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "CTS";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(GestionConfiguracion configuracion)
        {
            return configuracion.IdReplicacion.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(GestionConfiguracion configuracion)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Monto de diferencia para alertar en la liquidación", Convert.ToString(configuracion.DiferenciaLiquidacion));
            return sb.ToString();
        }

        #endregion
    }
}
