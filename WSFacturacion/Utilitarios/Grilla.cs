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
using System.Web.UI.WebControls;
using Telectronica.EntidadesSL;

namespace Telectronica.Utilitarios
{
    public static class Grilla
    {
        #region GETLIST Obtiene el archivo XML de Columnas como una lista para enviar al Silverlight
        public static List<Columna> getListColumnas(string nombreGrilla)
        {
            List<Columna> columnas = new List<Columna>();
            XPathNavigator xpn = getNavigator();
            XPathNodeIterator xpi1, xpi2, xpi3;
            xpi1 = xpn.SelectChildren(XPathNodeType.Element);
            foreach (XPathNavigator xpn1 in xpi1)
            {
                if (xpn1.Name == "Columnas")
                {
                    xpi2 = xpn1.SelectChildren(XPathNodeType.Element);
                    foreach (XPathNavigator xpn2 in xpi2)
                    {
                        if (xpn2.Name.Equals("Grilla"))
                        {
                            if (nombreGrilla.Equals(xpn2.GetAttribute("Nombre", "")))
                            {
                                xpi3 = xpn2.SelectChildren(XPathNodeType.Element);
                                int i = 0;
                                foreach (XPathNavigator xpn3 in xpi3)
                                {
                                    if (xpn3.Name == "Columna")
                                    {
                                        Columna columna = new Columna();
                                        columna.Campo = xpn3.GetAttribute("Campo", "");
                                        columna.Header = xpn3.GetAttribute("Header", "");
                                        columna.Visible = xpn3.GetAttribute("Visible", "");
                                        columna.Width = xpn3.GetAttribute("Width", "");
                                        columna.Style = xpn3.GetAttribute("Style", "");
                                        columna.Orden = xpn3.GetAttribute("Orden", "");
                                        columna.Posicion = i;
                                        columnas.Add(columna);
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return columnas;
        }
        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// genera un navigator del archivo XML de columnas
        /// </summary>
        /// <returns>XPathNavigator - Navigator del archivo xml</returns>
        /// ***********************************************************************************************      
        private static XPathNavigator getNavigator()
        {

            XPathNavigator xpn = null;
            XPathDocument xp = new XPathDocument(HttpContext.Current.Server.MapPath("~/XMLColumnas.xml"));
            xpn = xp.CreateNavigator();

            return xpn;
        }

        /// <summary>
        /// Genera un XLS a partir de un datagrid y lo guarda en la carpeta indicada.
        /// </summary>
        /// <param name="dg"></param>
        public static void exportDG(DataGrid dg)
        {
            
        }
    }
}
