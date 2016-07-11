using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using Telectronica.Utilitarios;

namespace Telectronica.Errores
{

    public static class ErrorManager
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Manejador de errores de la pagina. Centraliza el manejo y tratamiento de los errores producidos
        /// Invocamos al metodo ReportError que usa el parametro Server de la pagina
        /// </summary>
        /// <param name="ex">Exception - Objeto Exception generado al producirse el error</param>
        /// <param name="pagina">System.Web.UI.Page - Pagina en la que se produjo el error</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void ReportError(Exception ex, System.Web.UI.Page pagina)
        {
            ReportError(ex);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Manejador de errores de la aplicacion (Application de Global.asax). Centraliza el manejo y tratamiento de los errores producidos
        /// </summary>
        /// <param name="ex">Exception - Objeto Exception generado al producirse el error</param>
        /// <param name="server">System.Web.HttpServerUtility - Server, es el objeto existente en el Application</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void ReportError(Exception ex)
        {
            
            string sVolver = "back";
            if (ex is NotLoggedException)
            {
                //   sVolver = "Login";
                //a la pagina de login
                HttpContext.Current.Server.Transfer("~/OPE_Login.aspx?url=" + HttpContext.Current.Request.Path);
            }
            else if (ex is SinPermisoException)
            {
                sVolver = "Principal";  //TODO o volver a la anterior de la anterior
            }
            //TODO armar mejor el mensaje
            //Server.Transfer para que funcione el back
            HttpContext.Current.Server.Transfer("~/Errores.aspx?MensajeError=" + HttpContext.Current.Server.UrlEncode( getDescripcionError(ex) ) + "&Volver=" + sVolver);
            ////Server.Transfer no sirve en un pedido de Ajax
            //HttpContext.Current.Response.Redirect("~/Errores.aspx?MensajeError=" + HttpContext.Current.Server.UrlEncode(ex.ToString()) + "&Volver=" + sVolver);
            //TODO NO SE PUEDE EN PRODUCCION
            //System.Console.WriteLine(ex.Message);
        }

        public static string getDescripcionError(Exception exError)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Traduccion.Traducir("Se produjo un error en "));
            sb.Append(HttpContext.Current.Request.Path);
            sb.Append("\n\n");

            Exception ex = exError;
            while (ex != null)
            {
                sb.Append(Traduccion.Traducir("Error: "));
                sb.Append(ex.Message);
                sb.Append(Traduccion.Traducir(" (Origen: "));
                sb.Append(ex.Source);
                sb.Append(")\n");

                ex = ex.InnerException;
            }
            return sb.ToString();
        }

    }
}
