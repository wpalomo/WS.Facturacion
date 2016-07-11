using System;
using System.Collections.Generic;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using System.Data;

namespace Telectronica.Peaje
{
    public class UsuarioBs
    {
        #region USUARIO

        /// ***********************************************************************************************
        /// <summary>
        /// Sobrecarga del metodo validaLogin que solo recibe usuario y pass, para que no rompa en el resto de los proyectos
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>objeto Usuario</returns>
        /// ***********************************************************************************************
        public static Usuario ValidarLogin(string username, string password)
        {

            return ValidarLogin(username, password, null);

        }


        /// ***********************************************************************************************
        /// <summary>
        /// Encargado de validar al usuario que se loguea
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>objeto Usuario</returns>
        /// ***********************************************************************************************
        public static Usuario ValidarLogin(string username, string password, int? Estacion)
        {
            return ValidarLogin(username, password, Estacion, true);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Encargado de validar al usuario que se loguea
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="Estacion"></param>
        /// <param name="bGrabarIngreso">true para actualizar la sesion con los datos del usuario logueado</param>
        /// <returns>objeto Usuario</returns>
        /// ***********************************************************************************************
        public static Usuario ValidarLogin(string username, string password, int? Estacion, bool bGrabarIngreso)
        {
            Usuario oUsuario = null;
            UsuarioL oUsuarios = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    oUsuarios = UsuarioDt.getUsuarios(conn, username, null, null, false, null, Estacion);

                    if (oUsuarios != null && oUsuarios.Count > 0)
                    {
                        oUsuario = oUsuarios[0];
                        if (oUsuario.Eliminado)
                        {
                            oUsuario = null;
                        }
                        else if (oUsuario.FechaEgreso != null && oUsuario.FechaEgreso < System.DateTime.Now)
                        {
                            oUsuario = null;
                        }
                        else
                        {
                            //Hash de la Password
                            //dllCrypt.clsCryptClass oEncriptado = new dllCrypt.clsCryptClass();

                            oUsuario.PasswordReal = password;
                            //Por si la password está vacia
                            if (!(oUsuario.Password.Trim() == "" && password.Trim() == "")
                                && (oUsuario.Password != Encriptado.EncriptarPassword(username.ToUpper() + password.ToUpper())))
                            // && (oUsuario.Password != oEncriptado.SetUsuarioPassword("SYS", username.ToUpper(), password.ToUpper())))
                            {
                                oUsuario = null;
                            }
                            else
                            {
                                if (ConexionBs.getGSToEstacion())
                                {
                                    //GST el perfil activo es el del usuario
                                    oUsuario.PerfilActivo = oUsuario.PerfilGestion;
                                }
                                else
                                {
                                    getEstacionesHabilitadas(oUsuario, conn);
                                    UsuarioEstacion oEstacionHabilitada = oUsuario.EstacionesHabilitadas.FindEstacion(ConexionBs.getNumeroEstacion());
                                    if (oEstacionHabilitada != null)
                                    {
                                        oUsuario.PerfilActivo = oEstacionHabilitada.Perfil;
                                    }
                                    else
                                    {
                                        oUsuario = null;
                                    }
                                }

                                if (oUsuario != null)
                                {
                                    //ConexionBs.setUsuario(username);

                                    //ConexionBs.setPerfil(oUsuario.PerfilActivo.Codigo);
                                    int? zona = null;
                                    if (oUsuario.ZonaPrincipal != null)
                                    {
                                        zona = oUsuario.ZonaPrincipal.Codigo;
                                    }

                                    //ConexionBs.setZona(zona);
                                    string perfil = null;

                                    if (oUsuario.PerfilActivo != null)
                                    {
                                        perfil = oUsuario.PerfilActivo.Codigo;
                                    }

                                    if (bGrabarIngreso)
                                    {
                                        ConexionBs.setDatosUsuario(username, password, perfil, zona, oUsuario.Nombre);

                                        try
                                        {
                                            //Auditoria de ingreso
                                            Auditoria oAuditoria = new Auditoria("ING", "I", oUsuario.ID, getAuditoriaIngresoDescripcion(oUsuario));
                                            AuditoriaBs.addAuditoria(oAuditoria, conn);
                                        }
                                        catch (Exception ex)
                                        {
                                            //No hacemos nada por si todavia no tenemos la info completa
                                        }
                                    }
                                    /*
                                    //Actualizamos el ultimo acceso
                                    //(en Gestion) si da error igual entramos
                                    try
                                    {
                                        using (Conexion connGST = new Conexion())
                                        {
                                            //con transaccion                    
                                            connGST.ConectarGST(true);
                                            UsuarioDt.updUltimoAcceso(connGST, oUsuario, System.DateTime.Now);

                                            //Finalizamos la transaccion
                                            connGST.Finalizar(true);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //No hacemos nada por si GST no está disponible
                                    }
                                    */
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oUsuario;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Datos del usuario logueado
        /// lo obtiene de la sesion
        /// devuelve null si no hay
        /// </summary>
        /// <returns>objeto Usuario</returns>
        /// ***********************************************************************************************
        public static Usuario getUsuarioLogueado()
        {
            Usuario oUser = null;
            string usuario = ConexionBs.getUsuario();
            if (usuario != "")
            {
                oUser = new Usuario(usuario, ConexionBs.getUsuarioNombre());
                oUser.PerfilActivo = new Perfil(ConexionBs.getPerfil(), "");
                int? zona = ConexionBs.getZona();
                if (zona != null)
                    oUser.ZonaPrincipal = new Zona((int)zona, "");
            }

            return oUser;

        }

        public static UsuarioL getUsuariosValidador()
        {
            UsuarioL oUsuarios = new UsuarioL();
            try
            {
                using (Conexion conn = new Conexion())
                {
                    bool bGST = ConexionBs.getGSToEstacion();
                    conn.ConectarGSTPlaza(bGST, false);
                    oUsuarios = UsuarioDt.getUsuariosValidador(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oUsuarios;
        }

        public static UsuarioL getUsuarios()
        {
            return getUsuarios(null, null, null, false, null);
        }

        public static UsuarioL getUsuarios(string nombre, int? estacion, bool incluirEliminados)
        {
            return getUsuarios(null, nombre, estacion, incluirEliminados, null);
        }

        public static UsuarioL getUsuarios(string nombre, int? estacion, bool incluirEliminados, bool bForzarGST)
        {
            return getUsuarios(null, nombre, estacion, incluirEliminados, null, ConexionBs.getNumeroEstacion(), bForzarGST);
        }

        public static UsuarioL getUsuarios(string id)
        {
            //Puede ser que este buscando uno eliminado
            return getUsuarios(id, null, null, true, null);
        }

        public static UsuarioL getUsuarios(string id, bool bForzarGST)
        {
            //Puede ser que este buscando uno eliminado
            return getUsuarios(id, null, null, true, null, ConexionBs.getNumeroEstacion(), bForzarGST);
        }

        public static UsuarioL getUsuarios(long tarjeta)
        {
            return getUsuarios(null, null, null, false, tarjeta);
        }

        public static UsuarioL getUsuarios(string id, string nombre, int? estacion, bool incluirEliminados)
        {
            return getUsuarios(id, nombre, estacion, incluirEliminados, null);
        }
        
        public static UsuarioL getUsuarios(string id, string nombre, int? estacion, bool incluirEliminados, long? tarjeta)
        {
            return getUsuarios(id, nombre, estacion, incluirEliminados, tarjeta, ConexionBs.getNumeroEstacion(), false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Lista de usuarios ordenada con los locales primero
        /// Inserta una linea vacía entre locales y no
        /// </summary>
        /// <param name="estacionLocal">int? - Estacion donde deben ser locales
        /// <returns>Lista de Usuarios</returns>
        /// ***********************************************************************************************
        public static UsuarioL getUsuariosLocales(int? estacionLocal)
        {
            UsuarioL oUsuarios =
                 getUsuarios(null, null, null, false, null, estacionLocal, false);
            //Buscamos el primero no local
            bool bEsLocal = false;
            for (int i = 0; i < oUsuarios.Count; i++)
            {
                Usuario item = oUsuarios[i];
                if (item.EsLocal)
                {
                    bEsLocal = true;
                }
                else
                {
                    if (bEsLocal)
                    {
                        Usuario oUsuario = new Usuario();
                        oUsuario.ID = "";
                        oUsuario.Nombre = "--------------------------------------";
                        oUsuarios.Insert(i, oUsuario);
                        break;
                    }
                }
            }
            return oUsuarios;
        }
        
        public static UsuarioL getUsuarios(string id, string nombre, int? estacion, bool incluirEliminados, long? tarjeta, int? estacionLocal, bool bForzarGST)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    bool bGST = ConexionBs.getGSToEstacion();
                    if (bForzarGST)
                    {
                        bGST = true;
                    }
                    conn.ConectarGSTPlaza(bGST, false);
                    return UsuarioDt.getUsuarios(conn, id, nombre, estacion, incluirEliminados, tarjeta, estacionLocal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve todos los usuarios peajistas de una estación
        /// </summary>
        /// <param name="xiEstacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static UsuarioL getUsuariosEstPeajista(int xiEstacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return UsuarioDt.getUsuariosEstPeajista(conn, xiEstacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un usuario
        /// </summary>
        /// <param name="oUsuario">Usuario - Objeto Usuario
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addUsuario(Usuario oUsuario)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Validamos que la tarjeta no exista
                    if (oUsuario.Tarjeta > 0)
                    {
                        long tarjeta = (long)oUsuario.Tarjeta;
                        UsuarioL auxUsuarios = getUsuarios(tarjeta);
                        if (auxUsuarios.Count > 0)
                        {
                            string msg = string.Format(Traduccion.Traducir("La tarjeta está asignada al usuario {0}"), auxUsuarios[0].Nombre);
                            throw new ErrorSPException(msg);
                        }

                    }
                    //Password inicial
                    oUsuario.Password = getPasswordNueva(oUsuario.ID);
                    oUsuario.UltimoCambioPassword = DateTime.Today;

                    //Agregamos Usuario
                    UsuarioDt.addUsuario(oUsuario, conn);

                    //Agregamos Estaciones
                    updEstacionesHabilitadas(oUsuario.ID, oUsuario.EstacionesHabilitadas, conn);

                    //Auditoria grabamos donde lo hacemos
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaUsuario(),
                                                               "A",
                                                               getAuditoriaCodigoRegistro(oUsuario),
                                                               getAuditoriaDescripcion(oUsuario)),
                                                               connAud);

                        connAud.Finalizar(true);
                    }

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
        /// Actualizacion de un usuario.
        /// </summary>
        /// <param name="oUsuario">Usuario - Objeto Usuario
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updUsuario(Usuario oUsuario)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Validamos que la tarjeta no exista
                    if (oUsuario.Tarjeta > 0)
                    {
                        long tarjeta = (long)oUsuario.Tarjeta;
                        UsuarioL auxUsuarios = getUsuarios(tarjeta);
                        if (auxUsuarios.Count > 0)
                        {
                            //Solo está mal si el usuario que tiene la tarjeta es otro
                            if (auxUsuarios[0].ID != oUsuario.ID)
                            {
                                string msg = string.Format(Traduccion.Traducir("La tarjeta está asignada al usuario {0}"), auxUsuarios[0].Nombre);
                                throw new ErrorSPException(msg);
                            }
                        }

                    }

                    //Modificamos Usuario
                    UsuarioDt.updUsuario(oUsuario, conn);

                    //Modificamos Estaciones
                    updEstacionesHabilitadas(oUsuario.ID, oUsuario.EstacionesHabilitadas, conn);

                    //Auditoria grabamos donde lo hacemos
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaUsuario(),
                                                               "M",
                                                               getAuditoriaCodigoRegistro(oUsuario),
                                                               getAuditoriaDescripcion(oUsuario)),
                                                               connAud);

                        connAud.Finalizar(true);
                    }

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
        /// Baja de un Usuario.
        /// </summary>
        /// <param name="oUsuario">Usuario - Objeto Usuario
        /// <param name="nocheck">bool - true para que no chequee las FK
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delUsuario(Usuario oUsuario, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "USERS",
                                                       new string[] { oUsuario.ID },
                                                       new string[] { "usesta", "Audito" },
                                                       nocheck);

                    //En la plaza solo eliminamos la estacion actual
                    if (ConexionBs.getGSToEstacion())
                    {
                        //Eliminamos Todas las Estaciones
                        UsuarioDt.delEstacionesHabilitadas(oUsuario.ID, conn);
                    }
                    else
                    {
                        //Eliminamos Estacion actual
                        if (oUsuario.EstacionesHabilitadas.Count > 0)
                        {
                            UsuarioDt.delEstacionesHabilitadas(oUsuario.ID, oUsuario.EstacionesHabilitadas[0], conn);
                        }
                    }

                    //Solo eliminamos el usuario si no tiene estaciones habilitadas
                    if (UsuarioDt.getEstacionesHabilitadas(conn, oUsuario, null, true).Count == 0)
                    {
                        //Eliminamos el usuario
                        UsuarioDt.delUsuario(oUsuario, conn);
                    }

                    //Auditoria grabamos donde lo hacemos
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaUsuario(),
                                                               "B",
                                                               getAuditoriaCodigoRegistro(oUsuario),
                                                               getAuditoriaDescripcion(oUsuario)),
                                                               connAud);

                        connAud.Finalizar(true);
                    }

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
        /// Recuperacion de un Usuario.
        /// </summary>
        /// <param name="oUsuario">Usuario - Objeto Usuario
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void recUsuario(Usuario oUsuario)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Password inicial
                    oUsuario.Password = getPasswordNueva(oUsuario.ID);
                    oUsuario.UltimoCambioPassword = DateTime.Today;


                    //Recuperamos el usuario
                    UsuarioDt.updUsuario(oUsuario, conn);

                    //Auditoria grabamos donde lo hacemos
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaUsuario(),
                                                               "R",
                                                               getAuditoriaCodigoRegistro(oUsuario),
                                                               getAuditoriaDescripcion(oUsuario)),
                                                               connAud);

                        connAud.Finalizar(true);
                    }

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
        /// Obtiene los usuarios que se encuentran en el grupo de supervisores de una estación
        /// </summary>
        /// <param name="sUsuarioId"></param>
        /// <param name="iNumeroEstacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static UsuarioL GetUsuarioSupervisorPorEstacion(int iNumeroEstacion, string sUsuarioId)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                bool bGST = ConexionBs.getGSToEstacion();
                conn.ConectarGSTPlaza(bGST, false);
                return UsuarioDt.GetUsuarioSupervisorPorEstacion(conn, iNumeroEstacion, sUsuarioId);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene un objeto Usuario genérico para usarlo en los combos y que represente a Todos o Ninguno, según la descripción
        /// </summary>
        /// <param name="sDescr"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static Usuario GetUsuarioGenerico(string sDescr)
        {
            return new Usuario
            {
                Nombre = Traduccion.Traducir(sDescr),
                ID = null
            };
        }

        public enum enmCausa
        {
            OK,
            Jerarquia,
            EsSiMismo,
            EsMaestro
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Verfica si el perfil del login puede modificar a este usuario.
        /// No se puede modificar ni dar de baja si
        ///    a. es el usuario maestro
        ///    b. es a si mismo
        ///    c. el usuario tiene un perfil que no está en la jerarquia
        /// Se debe llamar al seleccionar el usuario para habilitar o no los botones
        /// y al terminar de modificar para validar que no se haya seleccionado
        /// un perfil inalcanzable
        /// </summary>
        /// <param name="oUsuario">Usuario - usuario a chequear
        /// <returns></returns>
        /// ***********************************************************************************************
        public static enmCausa chkUsuarioModificable(Usuario oUsuario)
        {
            enmCausa ret = enmCausa.OK;
            if (oUsuario.ID == ConexionBs.getUsuario())
            {
                ret = enmCausa.EsSiMismo;
            }
            else
            {
                ret = enmCausa.OK;
                Perfil oLogin = new Perfil(ConexionBs.getPerfil(), "");

                //El administrador puede todos
                if (!oLogin.EsAdministrador())
                {
                    PerfilBs.getJerarquias(oLogin);

                    //Verificamos si algun perfil de oUsuario no está en oLogin
                    //Incluyendo el de gestion
                    foreach (UsuarioEstacion item in oUsuario.EstacionesHabilitadas)
                    {
                        if (item.Perfil != null)
                        {
                            PerfilJerarquia oJerarquia = oLogin.Jerarquia.FindJerarquia(item.Perfil.Codigo);
                            if (oJerarquia == null || !oJerarquia.Controlado)
                            {
                                ret = enmCausa.Jerarquia;
                                break;
                            }
                        }

                    }
                }
            }

            if (ret == enmCausa.OK)
            {
                if (oUsuario.EsUsuarioMaestro())
                {
                    ret = enmCausa.EsMaestro;
                }
            }
            return ret;
        }

        public static DataSet getRptUsuariosEstaciones(string nombre, int? estacion, bool incluirEliminados)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    if (!ConexionBs.getGSToEstacion())
                    {
                        estacion = ConexionBs.getNumeroEstacion();
                    }

                    return UsuarioDt.getRptUsuariosEstaciones(conn, nombre, estacion, incluirEliminados);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet getRptUsuarios(string nombre, int? estacion, bool incluirEliminados)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    if (!ConexionBs.getGSToEstacion())
                    {
                        estacion = ConexionBs.getNumeroEstacion();
                    }

                    return UsuarioDt.getRptUsuarios(conn,nombre, estacion, incluirEliminados);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PASSWORD

        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de la password.
        /// </summary>
        /// <param name="usuario">string - usuario
        /// <param name="pwdvieja">string - password anterior
        /// <param name="pwdnueva">string - password nueva
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updPassword(string usuario, string pwdvieja, string pwdnueva)
        {
            updPassword(usuario, pwdvieja, pwdnueva, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// password de usuarios nuevos o al resetear
        /// </summary>
        /// <param name="usuario">string - usuario
        /// <returns>Password nueva</returns>
        /// ***********************************************************************************************
        protected static string getPasswordNueva(string usuario)
        {
            string pwdnueva = "";

            //dllCrypt.clsCryptClass oEncriptado = new dllCrypt.clsCryptClass();
            //Hash de la Password
            string pwdnuevah = Encriptado.EncriptarPassword(usuario.ToUpper() + pwdnueva.ToUpper());
            //string pwdnuevah = oEncriptado.SetUsuarioPassword("SYS", usuario.ToUpper(), pwdnueva.ToUpper());
            return pwdnuevah;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Borrar la password
        /// </summary>
        /// <param name="usuario">string - usuario
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delPassword(string usuario)
        {
            //TODO Verificar si tenemos permisos para hacer esto
            string pwdnueva = getPasswordNueva(usuario);
            updPassword(usuario, null, pwdnueva, true);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de la password.
        /// </summary>
        /// <param name="usuario">string - usuario
        /// <param name="pwdvieja">string - password anterior
        /// <param name="pwdnueva">string - password nueva
        /// <param name="bBorrar">bool - true para borrar la password
        /// <returns></returns>
        /// ***********************************************************************************************
        protected static void updPassword(string usuario, string pwdvieja, string pwdnueva, bool bBorrar)
        {
            try
            {
                DateTime dtVencimiento;

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    string pwdviejah = "";
                    //dllCrypt.clsCryptClass oEncriptado = new dllCrypt.clsCryptClass();
                    //Hash de la Password
                    //al borrar ya viene encriptada
                    string pwdnuevah = pwdnueva;
                    if (!bBorrar)
                    {
                        //pwdnuevah = oEncriptado.SetUsuarioPassword("SYS", usuario.ToUpper(), pwdnueva.ToUpper());
                        pwdnuevah = Encriptado.EncriptarPassword(usuario.ToUpper() + pwdnueva.ToUpper());
                    }

                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    bool bEncriptar = true;
                    if (bBorrar || pwdvieja.Trim() == "")
                    {
                        //La clave la grabamos vencida
                        dtVencimiento = DateTime.Today;
                        //La password vieja es la actual
                        UsuarioL oUsuarios = UsuarioDt.getUsuarios(conn, usuario, null, null, false, null, null);
                        if (oUsuarios.Count > 0)
                        {
                            if (bBorrar || oUsuarios[0].Password == null || oUsuarios[0].Password.Trim() == "")
                            {
                                pwdviejah = oUsuarios[0].Password;
                                bEncriptar = false;
                            }
                        }
                    }
                    if (bEncriptar)
                    {
                        //Hash de la Password
                        //pwdviejah = oEncriptado.SetUsuarioPassword("SYS", usuario.ToUpper(), pwdvieja.ToUpper());
                        pwdviejah = Encriptado.EncriptarPassword(usuario.ToUpper() + pwdvieja.ToUpper());
                    }
                    
                    //Modificamos Password
                    UsuarioDt.updPassword(conn, usuario, pwdviejah, pwdnuevah, DateTime.Today);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);

                    //Auditoria grabamos donde lo hacemos
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria("USU",
                                                               "M",
                                                               usuario,
                                                               "Modificación de contraseña"),
                                                               usuario,
                                                               connAud);

                        connAud.Finalizar(true);
                    }
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
        private static string getAuditoriaCodigoAuditoriaUsuario()
        {
            return "USU";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Usuario oUsuario)
        {
            return oUsuario.ID;
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Usuario oUsuario)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre", oUsuario.Nombre);
            if (oUsuario.Tarjeta > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Tarjeta", oUsuario.Tarjeta.ToString());
            }
            if (oUsuario.ZonaPrincipal != null)
            {
                AuditoriaBs.AppendCampo(sb, "Zona Principal", oUsuario.ZonaPrincipal.Descripcion);
            }
            if (oUsuario.ZonaHabitual != null)
            {
                AuditoriaBs.AppendCampo(sb, "Zona Habitual", oUsuario.ZonaHabitual.Descripcion);
            }
            if (oUsuario.TipoPersonal != null)
            {
                AuditoriaBs.AppendCampo(sb, "Tipo Personal", oUsuario.TipoPersonal.Descripcion);
            }
            if (oUsuario.FechaEgreso != null)
            {
                AuditoriaBs.AppendCampo(sb, "Nivel en la Vía", ((DateTime)oUsuario.FechaEgreso).ToString("dd/MM/yyyy"));
            }
            AuditoriaBs.AppendCampo(sb, "Nombre Corto", oUsuario.NombreCorto);


            StringBuilder sb2 = new StringBuilder();
            bool bComa = false;
            foreach (UsuarioEstacion item in oUsuario.EstacionesHabilitadas)
            {
                if (item.Perfil != null)
                {
                    if (bComa)
                    {
                        sb2.Append(", ");
                    }
                    sb2.Append(item.Estacion.Nombre + ":" + item.Perfil.Descripcion);
                    bComa = true;
                }
            }

            AuditoriaBs.AppendCampo(sb, "Perfiles en Estaciones", sb2.ToString());
            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaIngresoDescripcion(Usuario oUsuario)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Traduccion.Traducir("Ingreso al Sistema"));
            sb.Append("-> ");
            AuditoriaBs.AppendCampo(sb, "Nombre", oUsuario.Nombre);
            AuditoriaBs.AppendCampo(sb, "Perfil", oUsuario.PerfilActivo.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Cookies", System.Web.HttpContext.Current.Request.Browser.Cookies.ToString());
            AuditoriaBs.AppendCampo(sb, "JavaScript", System.Web.HttpContext.Current.Request.Browser.EcmaScriptVersion.ToString());
            AuditoriaBs.AppendCampo(sb, "CSS", System.Web.HttpContext.Current.Request.Browser.SupportsCss.ToString());
            AuditoriaBs.AppendCampo(sb, "Host", System.Web.HttpContext.Current.Request.UserHostName);
            AuditoriaBs.AppendCampo(sb, "Browser", System.Web.HttpContext.Current.Request.UserAgent);
            return sb.ToString();
        }

        #endregion

        #region ESTACIONES HABILITADAS

        /// ***********************************************************************************************
        /// <summary>
        /// Estaciones en las que esta habilitado un usuario
        /// En GST traigo todas, en la plaza solo la estacion en que estoy
        /// </summary>
        /// <param name="oUsuario">Usuario - objeto usuario, le asigna la lista de estaciones habilitadas
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void getEstacionesHabilitadas(Usuario oUsuario)
        {
            getEstacionesHabilitadas(oUsuario, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Estaciones en las que esta habilitado un usuario
        /// En GST traigo todas, en la plaza solo la estacion en que estoy
        /// </summary>
        /// <param name="oUsuario">Usuario - objeto usuario, le asigna la lista de estaciones habilitadas
        /// <param name="bForzarGST">bool - true para traer datos de gestion
        ///                                 false los trae de donde estoy
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void getEstacionesHabilitadas(Usuario oUsuario, bool bForzarGST)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    bool bGST = ConexionBs.getGSToEstacion();
                    if (bForzarGST)
                    {
                        bGST = true;
                    }
                    conn.ConectarGSTPlaza(bGST, false);
                    getEstacionesHabilitadas(oUsuario, conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void getEstacionesHabilitadas(Usuario oUsuario, Conexion conn)
        {
            int? estacion = null;
            if (!ConexionBs.getGSToEstacion())
            {
                estacion = ConexionBs.getNumeroEstacion();
            }
            oUsuario.EstacionesHabilitadas = UsuarioDt.getEstacionesHabilitadas(conn, oUsuario, estacion);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Recorre la lista de estaciones habilitadas y da de alta los habilitados y de baja los no habilitados
        /// </summary>
        /// <param name="usuario">string - codigo de usuario
        /// <param name="oEstaciones">UsuarioEstacionL - objeto con las estaciones habilitadas
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        protected static void updEstacionesHabilitadas(string usuario, UsuarioEstacionL oEstaciones, Conexion conn)
        {
            try
            {
                foreach (UsuarioEstacion oEstacion in oEstaciones)
                {
                    //Salteamos Gestion 
                    if (oEstacion.Estacion.Numero > 0)
                    {
                        if (oEstacion.Perfil != null)
                        {
                            UsuarioDt.updEstacionHabilitada(usuario, oEstacion, conn);
                        }
                        else
                        {
                            UsuarioDt.delEstacionesHabilitadas(usuario, oEstacion, conn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TIPOPERSONAL: Metodos de la Clase de Negocios de la entidad TipoPersonal.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de Personal. 
        /// </summary>
        /// <returns>Lista de Tipos de Personal</returns>
        /// ***********************************************************************************************
        public static TipoPersonalL getTiposPersonal()
        {
            TipoPersonalL oTiposPersonal = new TipoPersonalL();
            oTiposPersonal.Add(getTipoPersonal("E"));
            oTiposPersonal.Add(getTipoPersonal("V"));

            return oTiposPersonal;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Objeto Tipo de Personal
        /// </summary>
        /// <param name="codigo">string - Codigo de tipo de personal que deseamos obtener</param>
        /// <returns>Objeto TipoPersonal</returns>
        /// ***********************************************************************************************
        public static TipoPersonal getTipoPersonal(string codigo)
        {
            return UsuarioDt.getTipoPersonal(codigo);
        }
        
        #endregion
    }
}
