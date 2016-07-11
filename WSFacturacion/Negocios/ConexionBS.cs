using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public static class ConexionBs
    {   /// ***********************************************************************************************<summary>
        /// Clase con metodos y funciones propias de la capa de Negocios
        /// ********************************************************************************************** </summary>


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el nombre de la empresa
        /// </summary>
        /// <returns>Nombre empresa</returns>
        /// ***********************************************************************************************      
        public static string getNombreEmpresa()
        {
            return WebConfigurationManager.AppSettings["NombredeEmpresa"];
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el nombre del logo de la empresa
        /// </summary>
        /// <returns>Nombre Logo empresa (sin path)</returns>
        /// ***********************************************************************************************      
        public static string getLogoEmpresa()
        {
            return WebConfigurationManager.AppSettings["LogoEmpresa"];
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el nombre del logo con el nombre de la empresa
        /// </summary>
        /// <returns>Nombre Logo del nombre empresa (sin path)</returns>
        /// ***********************************************************************************************      
        public static string getLogoNombreEmpresa()
        {
            return WebConfigurationManager.AppSettings["LogoNombreEmpresa"];
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Setea el usuario logueado. LO guarda en la Sesion
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static void setUsuario(string usuario)
        {
            HttpContext.Current.Session["Permisos_Usuario"]=usuario;
        }
        

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el usuario logueado. Se obtiene desde la Sesion
        /// </summary>
        /// <returns>Usuario</returns>
        /// ***********************************************************************************************      
        public static string getUsuario()
        {
            string ret = null;
            if (HttpContext.Current.Session["Permisos_Usuario"] != null)
                ret =  HttpContext.Current.Session["Permisos_Usuario"].ToString();
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Setea el perfil del usuario logueado. LO guarda en la Sesion
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static void setPerfil(string perfil)
        {
            HttpContext.Current.Session["Permisos_Perfil"] = perfil;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Setea el nombre usuario logueado. LO guarda en la Sesion
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static void setUsuarioNombre(string nombre)
        {
            HttpContext.Current.Session["Permisos_UsuarioNombre"] = nombre;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el nombre usuario logueado. Se obtiene desde la Sesion
        /// </summary>
        /// <returns>Usuario</returns>
        /// ***********************************************************************************************      
        public static string getUsuarioNombre()
        {
            return (string)HttpContext.Current.Session["Permisos_UsuarioNombre"];
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Setea los datos del usuario logueado. 
        /// Usuario, Password, Perfil, Zona y Nombre
        /// Los guarda en la sesison y un encriptado de todo
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static void setDatosUsuario(string usuario, string password, string perfil, int? zona, string nombre)
        {
            setUsuario(usuario);
            setPerfil(perfil);
            setZona(zona);
            setUsuarioNombre(nombre);

            string dato = DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + usuario + "|" + password;
            string datos;
#if !COMPATIBLE_WIN32

            //TODO Probar este define
            byte[] bts = Encriptado.Encriptar(dato, dato.Length);
            datos = Convert.ToBase64String(bts);
#else
            datos = Encriptado.Encriptar(dato);
#endif
            HttpContext.Current.Session["Permisos_DatosUsuario"] = datos;

            /*
            //Testeo del encriptado
            string aux = Encriptado.Desencriptar(bts, bts.Length);
            string base64 = Convert.ToBase64String(bts, Base64FormattingOptions.None);
            byte[] bts64 = Convert.FromBase64String(base64);
            string aux2 = Encriptado.Desencriptar(bts64, bts64.Length);
            string url = HttpContext.Current.Server.UrlEncode(base64);
            string base64b = HttpContext.Current.Server.UrlDecode(url);
            byte[] bts64b = Convert.FromBase64String(base64b);
            string aux3 = Encriptado.Desencriptar(bts64b, bts64b.Length);
              */
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos del usuario encriptados. 
        /// para pasar como parametro a otros sitios
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static string getDatosUsuario()
        {
            //byte[] btsDatosUsuario = (byte[]) HttpContext.Current.Session["Permisos_DatosUsuario"];
            //return Convert.ToBase64String(btsDatosUsuario, Base64FormattingOptions.None);
            string ret = "";
            if (HttpContext.Current.Session["Permisos_DatosUsuario"] != null)
                ret = HttpContext.Current.Session["Permisos_DatosUsuario"].ToString();

            return ret;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el usuario y la password a partir del encriptado
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static void getUsuarioPassword(string datosUsuario, ref string usuario, ref string password)
        {
            try
            {
#if !COMPATIBLE_WIN32
                byte[] btData = Convert.FromBase64String(datosUsuario);
                string datos = Encriptado.Desencriptar(btData, btData.Length);
#else
                string datos = Encriptado.Desencriptar(datosUsuario);
#endif
                string fecha = datos.Substring(0, 14);
                int anio = Convert.ToInt32(fecha.Substring(0,4));
                int mes = Convert.ToInt32(fecha.Substring(4,2));
                int dia = Convert.ToInt32(fecha.Substring(6,2));
                int hora = Convert.ToInt32(fecha.Substring(8,2));
                int minuto = Convert.ToInt32(fecha.Substring(10,2));
                int segundo = Convert.ToInt32(fecha.Substring(12,2));
                
                DateTime date = new DateTime(anio,mes,dia,hora,minuto,segundo);
                
                
#if !DEBUG 
                if (date < DateTime.Now - new TimeSpan(24, 0, 0))
                {
                    throw new BadLoggedUserException("Datos de usuario vencidos");
                }
#endif
                int pos = datos.IndexOf('|');
                if (pos > 0)
                {
                    datos = datos.Substring(pos + 1);
                    pos = datos.IndexOf('|');
                    if (pos > 0)
                    {
                        usuario = datos.Substring(0, pos);
                        password = datos.Substring(pos + 1);
                    }
                    else
                    {
                        throw new BadLoggedUserException("Datos de usuario inválidos");
                    }
                }
                else
                {
                    throw new BadLoggedUserException("Datos de usuario inválidos");
                }
            }
            catch (Exception ex)
            {
                throw new BadLoggedUserException(Traduccion.Traducir("Datos de usuario incorrectos"), ex);
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el perfil del usuario logueado. Se obtiene desde la Sesion
        /// </summary>
        /// <returns>Usuario</returns>
        /// ***********************************************************************************************      
        //Numero de plaza en que estamos
        public static string getPerfil()
        {
            return (string)HttpContext.Current.Session["Permisos_Perfil"];
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Setea la zona del usuario logueado. Lo guarda en la Sesion
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static void setZona(int? zona)
        {
            HttpContext.Current.Session["Permisos_Zona"] = zona;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la zona del usuario logueado. Se obtiene desde la Sesion
        /// </summary>
        /// <returns>Usuario</returns>
        /// ***********************************************************************************************      
        public static int? getZona()
        {
            return (int?)HttpContext.Current.Session["Permisos_Zona"];
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Borra los datos del usuario y perfil
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static void clearUsuarioPerfil()
        {
            HttpContext.Current.Session["Permisos_Usuario"] = null;
            HttpContext.Current.Session["Permisos_Perfil"] = null;
            HttpContext.Current.Session["Permisos_Zona"] = null;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Determina si estamos en GST o en la Estacion. Se obtiene desde el Web.config
        /// </summary>
        /// <returns>Booleano que indica si estamos en GST (true) o en la Estacion(false)</returns>
        /// ***********************************************************************************************      
        public static bool getGSToEstacion()
        {
            //sacarlo del web.config
            bool bGST = Convert.ToBoolean(WebConfigurationManager.AppSettings["Gestion"]);
            
            return bGST;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el nombre de estacion con la que estoy conectado. Se obtiene desde el Web.config
        /// </summary>
        /// <returns>Nombre de estacion</returns>
        /// ***********************************************************************************************      
        public static string getNombreEstacion()
        {
            string nombre = Traduccion.getTextoGestion();
            try
            {
                if (!getGSToEstacion())
                {
                    if (HttpContext.Current.Session["NombreEstacion"] == null)
                    {
                        int numero = EstacionConfiguracionBs.getConfig().IdEstacion;
                        EstacionL oEst = EstacionBs.getEstaciones();
                        foreach (Estacion oEstacion in oEst)
                        {
                            if (numero == oEstacion.Numero)
                            {
                                nombre = oEstacion.Nombre;
                            }
                        }

                        HttpContext.Current.Session["NombreEstacion"] = nombre;
                    }
                    else
                    {
                        nombre = (string)HttpContext.Current.Session["NombreEstacion"];
                    }
                }
            }
            catch (Exception ex)
            {
                //Pasa si la base es nueva
            }
            return nombre;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna el numero de estacion con la que estoy conectado. Se obtiene desde el Web.config
        /// </summary>
        /// <returns>Numero de estacion</returns>
        /// ***********************************************************************************************      
        public static int getNumeroEstacion()
        {
            //Si es gestion devolvemos 0
            int numero=0;
            try
            {
                if (!getGSToEstacion())
                {
                    if (HttpContext.Current.Session["NumeroEstacion"] == null)
                    {
                        numero = EstacionConfiguracionBs.getConfig().IdEstacion;
                        HttpContext.Current.Session["NumeroEstacion"] = numero;
                    }

                    numero = (int)HttpContext.Current.Session["NumeroEstacion"];
                }
            }
            catch (Exception ex)
            {
                //Pasa si la base es nueva
            }
            return numero;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna La zona actual
        /// </summary>
        /// <returns>El numero de zona actual</returns>
        /// ***********************************************************************************************      
        public static int getZonaActual()
        {
            int zona = 0;
            try
            {
                if (!getGSToEstacion())
                {
                    if (HttpContext.Current.Session["ZonaActual"] == null)
                    {
                        zona = EstacionBs.getEstacionActual().Zona.Codigo;
                        HttpContext.Current.Session["ZonaActual"] = zona;
                    }

                    zona = (int) HttpContext.Current.Session["ZonaActual"];
                }
            }
            catch (Exception ex)
            {
            }
            return zona;
        }

        /// ***********************************************************************************************
        /// <summary>
        ///  Fuerza a que cargue de nuevo el numero de estacion
        /// </summary>
        /// <returns>Nada</returns>
        /// ***********************************************************************************************      
        public static void SetNumeroEstacion(int estacion)
        {
            HttpContext.Current.Session["NumeroEstacion"] = estacion;
            HttpContext.Current.Session["ZonaActual"] = null;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Verifica el estado de la sesión. Si se terminó la sesión arrojar un error.
        /// </summary>        
        /// ***********************************************************************************************
        public static void verificarSesion()
        {
            if (HttpContext.Current.Session["Permisos_UsuarioNombre"] == null)
            {
                throw new NotLoggedException("La sesión ha caducado.");
            }
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Verifica el estado de la conexion con GST
        /// </summary>        
        /// ***********************************************************************************************        
        public static void verificarGST()
        {
            Conexion conn = new Conexion();
            try
            {
                conn.ConectarGST(false);
            }
            catch( Exception ex )
            {
                throw ex;
            }
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el nombre de la terminal
        /// </summary>        
        /// ***********************************************************************************************
        public static string getTerminal()
        {
            return HttpContext.Current.Request.UserHostName;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna los datos de una estacion del grupo. 
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************      
        public static Estacion getEstacionDelGrupo(int estacion)
        {
            Estacion ret = null;
            EstacionL grupo;
            if (HttpContext.Current.Session["EstacionesDelGrupo"] != null)
            {
                grupo = (EstacionL)HttpContext.Current.Session["EstacionesDelGrupo"];
                foreach (Estacion item in grupo)
                {
                    if (item.Numero == estacion)
                    {
                        ret = item;
                        break;
                    }
                }
            }

            return ret;
        }




    }
}
