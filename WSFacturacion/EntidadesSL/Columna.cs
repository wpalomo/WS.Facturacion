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
    public class Columna
    {
        public Columna()
        {
        }
        
        public string Campo { get; set; }
        public string Header { get; set; }
        public string Visible { get; set; }
        public string Width { get; set; }
        public string Style { get; set; }
        public string Orden { get; set; }
        public int Posicion { get; set; }
        
    }
}
