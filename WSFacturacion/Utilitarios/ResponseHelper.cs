using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Telectronica.Utilitarios
{
    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {

            HttpContext context = HttpContext.Current;

            if( (string.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && string.IsNullOrEmpty(windowFeatures) ) 
            {
                context.Response.Redirect(url);
            }
            else
            {
                Page page  = (Page)context.Handler;

                if( page == null )
                {

                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }

                url = page.ResolveClientUrl(url);

                string script;

                if (! string.IsNullOrEmpty(windowFeatures)) 
                {
                    script = "window.open(\"{0}\", \"{1}\", \"{2}\");";
                }
                else
                {
                    script = "window.open(\"{0}\", \"{1}\");";
                }

                script = string.Format(script, url, target, windowFeatures);

                StringBuilder sb = new StringBuilder();
                sb.Append("<SCRIPT LANGUAGE=javascript>\n");
                sb.Append("<!--\n");
                sb.Append(script + "\n");
                sb.Append("//-->\n");
                sb.Append("</SCRIPT>\n");
                /*
                context.Response.Write("<SCRIPT LANGUAGE=javascript>\n");
                context.Response.Write("<!--\n");
                context.Response.Write(script + "\n");
                context.Response.Write("//-->\n");
                context.Response.Write("</SCRIPT>\n");
                 */

                if (target != "_self")
                {
                    page.ClientScript.RegisterStartupScript(page.GetType(), "Redirect", sb.ToString());
                    //ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
                }
                else
                {
                    context.Response.Write(sb.ToString());
                    context.Response.End();
                }
            
            }
        }

    }
}
