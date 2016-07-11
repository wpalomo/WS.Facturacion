using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Telectronica.Utilitarios.SL
{
    public class ValidationException: System.Exception
    {
        /// <summary>
        /// Constructor con un parámetro, que será el mensaje
        /// </summary>
        /// <param name="sMessage"></param>
        public ValidationException(string sMessage) : base(sMessage)
        {
        }

        /// <summary>
        /// Constructor con dos mensajes, uno es el mensaje y otro es un objeto que tiene relacion con la excepción
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sender"></param>
        public ValidationException(string sMessage, object sender)
            : base(sMessage)
        {
            Sender = sender;
        }

        /// <summary>
        /// Obtiene o establece un objeto relacionado a la excepción
        /// </summary>
        public Object Sender { get; set; }
    }
}
