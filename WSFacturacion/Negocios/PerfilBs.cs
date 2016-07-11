using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Telectronica.Peaje
{
    public class PerfilBs
    {
        #region PERFIL

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Perfiles
        /// </summary>
        /// <returns>Lista de Perfiles</returns>
        /// ***********************************************************************************************
        public static DataSet rptPerfiles()
        {
            return rptPerfiles(null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Perfiles
        /// </summary>
        /// <param name="perfil">String - Permite filtrar por un perfil determinado
        /// <returns>Lista de Perfiles</returns>
        /// ***********************************************************************************************
        public static DataSet rptPerfiles(string perfil)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    DataSet dsPerfil = PerfilDt.rptPerfiles(conn, perfil);
                    PerfilDt.rptJerarquias(conn, dsPerfil);
                    PermisosDt.rptPermisos(conn, dsPerfil);
                    PerfilDt.rptEventos(conn, dsPerfil);
                    return dsPerfil;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Perfiles
        /// </summary>
        /// <returns>Lista de Perfiles</returns>
        /// ***********************************************************************************************
        public static PerfilL getPerfiles()
        {
            return getPerfiles(null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Perfiles
        /// </summary>
        /// <param name="perfil">String - Permite filtrar por un perfil determinado
        /// <returns>Lista de Perfiles</returns>
        /// ***********************************************************************************************
        public static PerfilL getPerfiles(string perfil)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return PerfilDt.getPerfiles(conn, perfil);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un perfil
        /// </summary>
        /// <param name="oPerfil">Perfil - Objeto Perfil
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addPerfil(Perfil oPerfil)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos Perfil
                    PerfilDt.addPerfil(oPerfil, conn);

                    //Agregamos Permisos
                    updPermisos(oPerfil.Codigo, oPerfil.Permisos, conn);

                    //Agregamos Eventos
                    updEventos(oPerfil.Codigo, oPerfil.PermisosEventos, conn);

                    //Agregamos Jerarquias
                    updJerarquias(oPerfil.Codigo, oPerfil.Jerarquia, conn);

                    //Agregamos la jerarquia que pueda faltar para el grupo Administrador
                    addJerarquiasAdmin(conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaPerfil(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oPerfil),
                                                           getAuditoriaDescripcion(oPerfil)),
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
        /// Actualizacion de un perfil.
        /// </summary>
        /// <param name="oPerfil">Perfil - Objeto Perfil
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updPerfil(Perfil oPerfil)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos Perfil
                    PerfilDt.updPerfil(oPerfil, conn);

                    //Modificamos Permisos
                    updPermisos(oPerfil.Codigo, oPerfil.Permisos, conn);

                    //Modificamos Eventos
                    updEventos(oPerfil.Codigo, oPerfil.PermisosEventos, conn);

                    //Modificamos Jerarquias
                    updJerarquias(oPerfil.Codigo, oPerfil.Jerarquia, conn);

                    //Agregamos la jerarquia que pueda faltar para el grupo Administrador
                    addJerarquiasAdmin(conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaPerfil(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oPerfil),
                                                           getAuditoriaDescripcion(oPerfil)),
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
        /// Baja de un Perfil.
        /// </summary>
        /// <param name="oPerfil">Perfil - Objeto Perfil
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delPerfil(Perfil oPerfil, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "GRUPOS", 
                                                       new string[] { oPerfil.Codigo }, 
                                                       new string[] { "CONTROLES_PERFILES", "GRUJERARQ" }, 
                                                       nocheck);

                    //Eliminamos Jerarquias
                    PerfilDt.delJerarquias(oPerfil.Codigo, conn);

                    //Eliminamos Eventos
                    PerfilDt.delEventos(oPerfil.Codigo, conn);

                    //Eliminamos Permisos
                    PermisosDt.delPermisos(oPerfil.Codigo, conn);

                    //Eliminamos la estacion
                    PerfilDt.delPerfil(oPerfil, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaPerfil(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oPerfil),
                                                           getAuditoriaDescripcion(oPerfil)),
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
        private static string getAuditoriaCodigoAuditoriaPerfil()
        {
            return "PER";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Perfil oPerfil)
        {
            return oPerfil.Codigo;
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Perfil oPerfil)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Descripción", oPerfil.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Nivel en la Vía", oPerfil.NivelVia.Descripcion);
            StringBuilder sb2 = new StringBuilder();
            bool bComa = false;
            foreach (PerfilJerarquia item in oPerfil.Jerarquia)
            {
                if (item.Controlado)
                {
                    if( bComa )
                        sb2.Append(", ");
                    sb2.Append(item.PerfilMenor.Descripcion);
                    bComa = true;
                }
            }
            AuditoriaBs.AppendCampo(sb, "Jerarquía entre Perfiles", sb2.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion


        #region PERMISOS
        /// ***********************************************************************************************
        /// <summary>
        /// Carga la lista de permisos de un perfil
        /// </summary>
        /// <param name="oPerfil">Perfil - objeto perfil al que se le carga la lista
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        public static void getPermisos(Perfil oPerfil)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    oPerfil.Permisos = PermisosDt.getPermisos(conn, oPerfil.Codigo, false, null, false, null, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Recorre la lista de permisos y da de alta los habilitados y de baja los no habilitados
        /// </summary>
        /// <param name="perfil">string - codigo de perfil del permiso
        /// <param name="oPermisos">PermisoL - objeto con los permisos
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        protected static void updPermisos(string perfil, PermisoL oPermisos, Conexion conn)
        {
            try
            {
                foreach (Permiso oPermiso in oPermisos)
                {
                    if (oPermiso.Habilitado || oPermiso.Autorizacion)
                    {
                        PermisosDt.updPermiso(perfil, oPermiso, conn);
                    }
                    else
                    {
                        PermisosDt.delPermiso(perfil, oPermiso, conn);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region PERFILJERARQUIA


        /// ***********************************************************************************************
        /// <summary>
        /// Carga la lista de perfiles controlados por un perfil
        /// </summary>
        /// <param name="oPerfil">Perfil - objeto perfil al que se le carga la lista</param>
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        public static void getJerarquias(Perfil oPerfil)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    oPerfil.Jerarquia = PerfilDt.getJerarquias(conn, oPerfil.Codigo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Recorre la lista de jerarquias y da de alta los controlados y de baja los no controlados
        /// </summary>
        /// <param name="perfil">string - codigo de perfil 
        /// <param name="oPermisos">PermisoL - objeto con las jerarquias
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        protected static void updJerarquias(string perfil, PerfilJerarquiaL oJerarquias, Conexion conn)
        {
            try
            {
                foreach (PerfilJerarquia oJerarquia in oJerarquias)
                {
                    if (oJerarquia.Controlado)
                    {
                        PerfilDt.updJerarquia(perfil, oJerarquia, conn);
                    }
                    else
                    {
                        PerfilDt.delJerarquia(perfil, oJerarquia, conn);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Inserta los registros de Jerarquia de Grupos para el administrador
        /// </summary>
        /// <param name="conn">Conexion - Objeto de conexion con la base de datos</param>
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        protected static void addJerarquiasAdmin(Conexion conn)
        {
            try
            {
                PerfilDt.addJerarquiasAdmin(conn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region EVENTOPERFIL
        /// ***********************************************************************************************
        /// <summary>
        /// Carga la lista de eventos que puede ver un perfil
        /// </summary>
        /// <param name="oPerfil">Perfil - objeto perfil al que se le carga la lista
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        public static void getEventos(Perfil oPerfil)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    oPerfil.PermisosEventos = PerfilDt.getEventos(conn, oPerfil.Codigo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Recorre la lista de eventos y da de alta los habilitados y de baja los no habilitados
        /// </summary>
        /// <param name="perfil">string - codigo de perfil 
        /// <param name="oEventos">EventoPermisoL - objeto con los eventos habilitados
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        protected static void updEventos(string perfil, EventoPerfilL oEventos, Conexion conn)
        {
            try
            {
                foreach (EventoPerfil oEvento in oEventos)
                {
                    if (oEvento.Habilitado)
                    {
                        PerfilDt.updEvento(perfil, oEvento, conn);
                    }
                    else
                    {
                        PerfilDt.delEvento(perfil, oEvento, conn);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region PERFILNIVEL

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Niveles de Via
        /// </summary>
        /// <returns>Lista de PerfilNivel</returns>
        /// ***********************************************************************************************
        public static PerfilNivelL getNiveles()
        {
            return getNiveles(null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Niveles de via
        /// </summary>
        /// <param name="nivel">int? - Permite filtrar por un nivel determinado
        /// <returns>Lista de PerfilNivel</returns>
        /// ***********************************************************************************************
        public static PerfilNivelL getNiveles(int? nivel)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return PerfilDt.getNiveles(conn, nivel);
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
