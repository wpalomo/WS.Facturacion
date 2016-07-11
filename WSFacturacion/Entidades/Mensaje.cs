using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Telectronica.EntidadesSL
{
    #region MENSAJE: Clase para mensajes con su traduccion

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ERROREJECUCION
    /// </summary>*********************************************************************************************


    //[DataContract]
    public class Mensaje
    {
        public Mensaje()
        {
        }
        public Mensaje( string key )
        {
            Key = key; 
        }
        public string Key { get; set; }
        public string Pt { get; set; }
        public string En { get; set; }
        


    }

    /*
    public class MensajeL: ObservableCollection<Mensaje>
    {
        public Mensaje FindKey(string key)
        {
            Mensaje oMen = null;
            foreach (Mensaje item in this)
            {
                if( item.Key == key )
                {
                    oMen = item;
                    break;
                }
            }
            return oMen;
        }
    }
     */
    #endregion
}
