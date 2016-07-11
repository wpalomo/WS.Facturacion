using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace Telectronica.Peaje
{

    /// <summary>
    /// Funciones varias usadas en las paginas
    /// </summary>
    public static class mdlGeneral
    {

        #region PATHIMAGENES
        /// ***********************************************************************************************
        /// <summary>
        /// Ubicacion de las imagenes de las categorias
        /// </summary>
        /// <returns>path imagenes</returns>
        /// ***********************************************************************************************
        public static string getPathImagenesCatego()
        {
            return "~/Imagenes/Dibujos/Catego/";
        }
        public static string getPathImagenesVarias()
        {
            return "~/App_Themes/Stylo/img/";
        }
        public static string getPathImagenesVarias(bool RutaCompleta)
        {
            if (RutaCompleta)
                return HttpContext.Current.Request.MapPath(getPathImagenesVarias());
            else
                return getPathImagenesVarias();

        }

        #endregion

    }
}
