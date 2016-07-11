using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using Telectronica.EntidadesSL;


namespace Telectronica.Utilitarios
{
    /// ***********************************************************************************************<summary>
    /// Clase para traduccion de mensajes y objetos de las paginas
    /// ********************************************************************************************** </summary>
    public static class Traduccion
    {
        private static string idioma = "";

        private static bool bMarcarIdioma = false;
        /// ***********************************************************************************************
        /// <summary>
        /// Realiza la traduccion de un texto, buscandolo segun el idioma configurado desde un archivo XML
        /// </summary>
        /// <param name="mensaje">string - Mensaje a traducir</param>
        /// <returns>string - Mensaje traducido</returns>
        /// ***********************************************************************************************      
        public static string Traducir(string mensaje)
        {
            string sRet = "";

            try
            {
                if (mensaje != "")
                {
                    mensaje = HttpContext.Current.Server.HtmlDecode(mensaje);
                    //Solo traducimos si tiene alguna letra (mayuscula o minuscula)
                    Regex re = new Regex("[a-z]|[A-Z]");
                    if (re.IsMatch(mensaje))
                    {
                        //buscar en un XML
                        sRet = BuscarTraduccion(mensaje);


                    }
                    else
                    {
                        sRet = mensaje;
                    }
                }
            }
            catch (Exception)
            {
                //Fallo la traduccion devuelvo sin traducir
                sRet = "[¡¡** " + mensaje + " **!!]";
            }

            return sRet;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// busca en un archivo XML el texto traducido en base al idioma del web config
        /// </summary>
        /// <param name="mensaje">string - Mensaje a traducir</param>
        /// <returns>string - Mensaje traducido</returns>
        /// ***********************************************************************************************      
        private static string BuscarTraduccion(string mensaje)
        {
            string ret = "";
            if (idioma == "")
            {
                idioma = WebConfigurationManager.AppSettings["Idioma"];
                bMarcarIdioma = Convert.ToBoolean(WebConfigurationManager.AppSettings["MarcarIdioma"]);
            }
            //idioma por defecto
            if ((idioma == "" || idioma == "es") && !bMarcarIdioma)
            {
                ret = mensaje;
            }
            else
            {
                XPathNavigator xpn = GetTraductorNavigator();
                XPathNavigator xpn1, xpn2;
                xpn1 = xpn.SelectSingleNode("//Mensaje[@key=\"" + mensaje.Trim() + "\"]");
                if (xpn1 != null)
                {
                    if ((idioma == "" || idioma == "es"))
                    {
                        //Solo buscamos para marcar el idioma
                        ret = mensaje;
                    }
                    else
                    {
                        //Buscamos Idioma y Cultura
                        xpn2 = xpn1.SelectSingleNode("@" + idioma);
                        if (xpn2 != null)
                            ret = xpn2.Value;
                        else
                        {
                            //Buscamos solo el idioma
                            string idioma2 = idioma.Substring(0, 2);
                            if (idioma2 != "es")
                            {
                                xpn2 = xpn1.SelectSingleNode("@" + idioma2);
                                if (xpn2 != null)
                                    ret = xpn2.Value;
                            }
                            else
                            {
                                ret = mensaje;
                            }
                        }
                    }
                }
                //Si no lo encontramos, retornamos el mismo string que recibimos 
                //pero resaltado para indicar que no esta traducido
                if (ret == "")
                {
                    // solo resaltarlo en ciertos ambientes
                    if (bMarcarIdioma)
                    {
                        ret = "[** " + mensaje.ToUpperInvariant() + " **]";

                        try
                        {
                            // Grabamos en un archivo el mensaje que no se encontro, con el formato listo para completar la traduccion
                            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Logs/Traduccion.err"), true);

                            sw.WriteLine(string.Format("<Mensaje key=\"{0}\" pt=\"{0}**\" en=\"{0}**\" />", mensaje.Trim()));
                            sw.Close();

                            /*
                            FileStream fs = new FileStream(Server.MapPath(filename), System.IO.FileMode.Create);

                            fs.Write(bytes, 0, bytes.Length);
                            fs.Close();
                             */
                        }
                        catch (Exception ex)
                        {
                            //Si da error no hago nada, es para que no me falle si no tengo permisos o no existe la carpeta
                        }
                    }
                    else
                    {
                        ret = mensaje;
                    }

                }
            }

            return ret;

        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// genera un navigator del archivo XML de traduccion
        /// Lo saca del cache para ahorrar tiempo
        /// </summary>
        /// <returns>XPathNavigator - Navigator del archivo xml</returns>
        /// ***********************************************************************************************      
        private static XPathNavigator GetTraductorNavigator()
        {
            XPathNavigator xpn = null;
            //Si hay que marcar el idioma no usamos cache
            if( !bMarcarIdioma )
                xpn = (XPathNavigator)HttpContext.Current.Cache["TraductorNavigator"];

            if (xpn == null)
            {

                XPathDocument xp = new XPathDocument(HttpContext.Current.Server.MapPath("~/XMLTraduccion.xml"));
                xpn = xp.CreateNavigator();
                HttpContext.Current.Cache["TraductorNavigator"] = xpn;
            }

            return xpn;
        }
        
        #region TEXTCOMBOS

        /// ***********************************************************************************************
        /// <summary>
        /// Texto "<Ninguno>" para los combos
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getNinguno()
        {
            return "<" + Traducir("Ninguno") + ">";
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Texto "<Ninguna>" para los combos
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getNinguna()
        {
            return "<" + Traducir("Ninguna") + ">";
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Texto "<Todos>" para los combos
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getTodos()
        {
            return "<" + Traducir("Todos") + ">";
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Texto "<Todas>" para los combos
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getTodas()
        {
            return "<" + Traducir("Todas") + ">";
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Texto "Gestión" para los combos
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getTextoGestion()
        {
            return Traducir("Gestión");
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Texto "Estación" para centralizar su traduccion e invocacion
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getTextoEstacion()
        {
            return Traducir("Estación");
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Texto "Sí" para centralizar su traduccion e invocacion
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getSi()
        {
            return Traducir("Sí");
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Texto "No" para centralizar su traduccion e invocacion
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getNo()
        {
            return Traducir("No");
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Texto "Si"o "No" para centralizar su traduccion e invocacion
        /// </summary>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getSiNo(bool bSi)
        {
            return bSi ? getSi() : getNo();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Traduce un parámetro del tipo string, S = Si, Cualquier otro valor para No
        /// </summary>
        /// <param name="sSiNo"></param>
        /// <returns>texto traducido</returns>
        /// ***********************************************************************************************
        public static string getSiNo(string sSiNo)
        {
            return getSiNo((sSiNo?? "N") == "S");
        }
        
        #endregion

        #region GETLIST Obtiene el archivo XML de traduccion como una lista para enviar al Silverlight

        public static List<Mensaje> getListMensajes(out string idioma, out bool marcarIdioma)
        {
            idioma = WebConfigurationManager.AppSettings["Idioma"];
            marcarIdioma = Convert.ToBoolean(WebConfigurationManager.AppSettings["MarcarIdioma"]);

            List<Mensaje> oMensajes = new List<Mensaje>();
            XPathNavigator xpn = GetTraductorNavigator();
            XPathNodeIterator xpi1,xpi2;
            xpi1 = xpn.SelectChildren(XPathNodeType.Element);
            foreach (XPathNavigator xpn1 in xpi1)
            {
                if (xpn1.Name == "Mensajes")
                {
                    xpi2 = xpn1.SelectChildren(XPathNodeType.Element);
                    foreach (XPathNavigator xpn2 in xpi2)
                    {
                        if (xpn2.Name == "Mensaje")
                        {
                            Mensaje men = new Mensaje(xpn2.GetAttribute("key", ""));
                            men.Pt = xpn2.GetAttribute("pt", "");
                            men.En = xpn2.GetAttribute("en", "");
                            men.EsEC = xpn2.GetAttribute("es-EC", "");
                            men.EsCO = xpn2.GetAttribute("es-CO", "");

                            oMensajes.Add(men);
                        }
                    }
                }
            }

            return oMensajes;
        }

        #endregion
    }
}
