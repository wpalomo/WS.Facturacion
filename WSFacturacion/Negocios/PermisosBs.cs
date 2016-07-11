using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de los permisos de los usuarios en cada pagina
    /// </summary>
    ///****************************************************************************************************
    public class PermisosBs
    {

        #region PERMISO: Clase de Negocios de Permisos de Usuarios

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de permisos de la pagina
        /// </summary>
        /// <param name="modulo">string - Codigo del modulo de la que desean obtener los permisos</param>
        /// <param name="pagina">string - Nombre de la pagina de la que desean obtener los permisos</param>
        /// <param name="soloHabi">bool - Devolvemos solo los objetos permiso que estan habilitados o todos</param>
        /// <returns>Lista de Permisos de la pagina</returns>
        /// ***********************************************************************************************
        public static PermisoL GetPermisos(string modulo, string pagina, bool soloHabi)
        {
            return GetPermisos(modulo, pagina, soloHabi, false, false, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de permisos de la pagina del cliente
        /// </summary>
        /// <param name="modulo">string - Codigo del modulo de la que desean obtener los permisos</param>
        /// <param name="pagina">string - Nombre de la pagina de la que desean obtener los permisos</param>
        /// <param name="soloHabi">bool - Devolvemos solo los objetos permiso que estan habilitados o todos</param>
        /// <returns>Lista de Permisos de la pagina del cliente</returns>
        /// ***********************************************************************************************
        public static PermisoL GetPermisosCliente(string modulo, string pagina, bool soloHabi)
        {
            return GetPermisos(modulo, pagina, soloHabi, false, true, true);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de permisos de la pagina
        /// </summary>
        /// <param name="modulo">string - Codigo del modulo de la que desean obtener los permisos</param>
        /// <param name="pagina">string - Nombre de la pagina de la que desean obtener los permisos</param>
        /// <param name="soloHabi">bool - Devolvemos solo los objetos permiso que estan habilitados o todos</param>
        /// <param name="bNoGenerarError">bool - true no genera el error si la lista de permisos es vacia, 
        ///             sí genera error si no hay usuario logueado</param>
        /// <param name="bForzarBD">bool - true fuerza la lectura de la BD (no usa cache) </param> 
        /// <returns>Lista de Permisos de la pagina</returns>
        /// ***********************************************************************************************
        public static PermisoL GetPermisos(string modulo, string pagina, bool soloHabi, bool bNoGenerarError, bool bForzarBD)
        {
            return GetPermisos(modulo, pagina, soloHabi, bNoGenerarError, bForzarBD, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de permisos de la pagina
        /// </summary>
        /// <param name="modulo">string - Codigo del modulo de la que desean obtener los permisos</param>
        /// <param name="pagina">string - Nombre de la pagina de la que desean obtener los permisos</param>
        /// <param name="soloHabi">bool - Devolvemos solo los objetos permiso que estan habilitados o todos</param>
        /// <param name="bNoGenerarError">bool - true no genera el error si la lista de permisos es vacia, 
        ///             sí genera error si no hay usuario logueado</param>
        /// <param name="bForzarBD">bool - true fuerza la lectura de la BD (no usa cache) </param> 
        /// <param name="bCliente">bool - true para obtener los permisos del cliente </param> 
        /// <returns>Lista de Permisos de la pagina</returns>
        /// ***********************************************************************************************
        public static PermisoL GetPermisos(string modulo, string pagina, bool soloHabi, bool bNoGenerarError, bool bForzarBD, bool bCliente)
        {
            PermisoL oPermisos = null;

            //Si no estoy logueado, generar un NotLoggedException
            string perfil = ConexionBs.getPerfil();

            if ( perfil == null)
            {
                throw new NotLoggedException("Para acceder al sistema debe hacer login");
            }

            //En base a donde estoy conectado obtengo los permisos
            bool bGST = ConexionBs.getGSToEstacion();

            //Si es el permiso de todo el menu lo cacheamos
            if (pagina == null && !bForzarBD)
            {
                oPermisos = (PermisoL)HttpContext.Current.Cache["Permisos_" + (bCliente ? "C" : (bGST ? "G" : "P")) + "_" + modulo + "_" + perfil];
            }
                        
            if( oPermisos == null )
            {


                try
                {
                    using (Conexion conn = new Conexion())
                    {
                        //sin transaccion
                        conn.ConectarGSTPlaza(bGST, false);

                        oPermisos = PermisosDt.getPermisos(conn, perfil, soloHabi, bGST, bCliente, modulo, pagina);
                        //Cacheamos los permisos
                        if (pagina == null)
                        {
                            HttpContext.Current.Cache["Permisos_" + (bCliente ? "C" : (bGST ? "G" : "P")) + "_" + modulo + "_" + perfil] = oPermisos;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            /// ***************************************************************************************************
            /// ***************************************************************************************************
            /// ***************************************************************************************************


            //si no hay ningun permiso generar un SinPermisoException
            //Solo para la pagina
            //y si no pidieron no generar error
            if ( pagina != null && !bNoGenerarError &&
                (oPermisos == null || oPermisos.Count == 0))
            {
                throw new SinPermisoException(Traduccion.Traducir("No tiene permiso para ver esta página"));
            }
            return oPermisos;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto permiso para un control determinado de la pagina.
        /// </summary>
        /// <param name="oPermisos">PermisoL - Lista de todos los permisos de la pagina</param>
        /// <param name="pagina">string - Nombre de la pagina de la que desea obtener el permiso del control</param>
        /// <param name="control">string - Nombre del control por el que desea averiguar su permiso de habilitacion</param>
        /// <returns>Objeto permiso de un control</returns>
        /// ***********************************************************************************************
        public static Permiso BuscarPermisoControl(PermisoL oPermisos, string pagina, string control)
        {
            Permiso oPermiso = null;
            foreach (Permiso oPer in oPermisos)
            {
                if (oPer.Pagina == pagina
                    && oPer.Control == control)
                {
                    oPermiso = oPer;
                    break;
                }
            }

            return oPermiso;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de iconos de la toolbar
        /// </summary>
        /// <param name="soloHabi">bool - Devolvemos solo los objetos permiso que estan habilitados o todos</param>
        /// <returns>Lista de Toolbar</returns>
        /// ***********************************************************************************************
        public static ToolbarL GetToolbar(bool soloHabi)
        {
            ToolbarL oToolbar = null;

            //Si no estoy logueado, generar un NotLoggedException
            string perfil = ConexionBs.getPerfil();

            if (perfil == null)
            {
                throw new NotLoggedException("Para acceder al sistema debe hacer login");
            }

            //En base a donde estoy conectado obtengo los permisos
            bool bGST = ConexionBs.getGSToEstacion();

            //Lo obtenemos del cache
            oToolbar = (ToolbarL)HttpContext.Current.Cache["Toolbar_" + (bGST ? "G" : "P") + "_" + perfil];

            if (oToolbar == null)
            {


                try
                {
                    using (Conexion conn = new Conexion())
                    {
                        //sin transaccion
                        conn.ConectarGSTPlaza(bGST, false);

                        oToolbar = PermisosDt.getToolbar(conn, perfil, soloHabi, bGST );
                        //Cacheamos los permisos
                        HttpContext.Current.Cache["Toolbar_" + (bGST ? "G" : "P") + "_" + perfil] = oToolbar;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return oToolbar;
        }
        #endregion

    }

        
}
